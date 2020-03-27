using CovidLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace CovidEnquirer
{
    public partial class MainForm : Form
    {
        private List<Article> FoundArticles;
        private List<Article> AllArticles = new List<Article>();
        private int MarginRight;
        private int MarginBottom;

        public MainForm()
        {
            InitializeComponent();

            var threadStarter = new ThreadStart(GetAllArticles);
            var thread = new Thread(threadStarter);
            thread.IsBackground = true;
            thread.Start();
        }

        private void PhraseToSearchButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(PhraseToSearchTextBox.Text))
            {
                return;
            }

            var threadStarter = new ThreadStart(SearchArticles);
            var thread = new Thread(threadStarter);
            thread.IsBackground = true;
            thread.Start();
        }

        private void SearchArticles()
        {
            SetSearchResultsContextMenEnabled(false);
            SetSearchResultsListBoxEnabled(false);
            SetGoogleButtonEnabled(false);
            SetSaveArticleButtonEnabled(false);
            SetOpenArticleButtonEnabled(false);
            SetPhraseToSearchButtonEnabled(false);

            SetProgressBarValue(0);
            FoundArticles = new List<Article>();

            var textToFind = PhraseToSearchTextBox.Text.ToLower();
            
            int progressValue = 0;
            SetProgressBarMaximum(AllArticles.Count);

            var wordsToFind = textToFind.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<KnuthPrattMorris> kmpList = new List<KnuthPrattMorris>();
            foreach(var word in wordsToFind)
            {
                var kmp = new KnuthPrattMorris(word.ToLower());
                kmpList.Add(kmp);
            }

            foreach (var article in AllArticles)
            {
                progressValue++;

                SetOptionalMessage(string.Format("Analysing article {0} out of {1} articles", progressValue, AllArticles.Count));
                SetProgressBarValue(progressValue);
                
                if (article.ZippedTitle.Count() == 0)
                {
                    continue;
                }

                int howManyWordsFound = 0;
                foreach (var kmp in kmpList)
                {
                    bool wasWordFound = false;
                    var unzippedTitle = Zipper.Unzip(article.ZippedTitle);
                    if (kmp.Search(unzippedTitle.ToLower()))
                    {
                        howManyWordsFound++;
                        continue;
                    }

                    var unzippedAbstract = Zipper.Unzip(article.ZippedAbstract);
                    if (!wasWordFound && kmp.Search(unzippedAbstract.ToLower()))
                    {
                        howManyWordsFound++;
                        continue;
                    }

                    var unzippedContent = Zipper.Unzip(article.ZippedContent);
                    if (!wasWordFound && kmp.Search(unzippedContent.ToLower()))
                    {
                        howManyWordsFound++;
                    }

                    if (!wasWordFound)
                    {
                        break;
                    }
                }

                if (howManyWordsFound == wordsToFind.Count())
                {
                    FoundArticles.Add(article);
                }
            }

            SetSearchResultsListClear();

            FoundArticles = FoundArticles.OrderBy(x =>
            {
                var unzippedTitle = Zipper.Unzip(x.ZippedTitle);
                return unzippedTitle;
            }).ToList();
            foreach (var article in FoundArticles)
            {
                SearchResultsListAppend(article);
            }

            SetSearchResultsContextMenEnabled(true);
            SetSearchResultsListBoxEnabled(true);
            SetGoogleButtonEnabled(true);
            SetSaveArticleButtonEnabled(true);
            SetOpenArticleButtonEnabled(true);
            SetPhraseToSearchButtonEnabled(true);
        }

        private void SetProgressBarValue(int value)
        {

            MethodInvoker mi = new MethodInvoker(() => ProgressBar.Value = value);
            if (ProgressBar.InvokeRequired)
            {
                ProgressBar.Invoke(mi);
            }
            else
            {
                mi.Invoke();          
            }
        }

        private void SetProgressBarMaximum(int value)
        {
            MethodInvoker mi = new MethodInvoker(() => ProgressBar.Maximum = value);
            if (ProgressBar.InvokeRequired)
            {
                ProgressBar.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetSearchResultsListClear()
        {
            MethodInvoker mi = new MethodInvoker(() => SearchResultsListBox.Items.Clear());
            if (SearchResultsListBox.InvokeRequired)
            {
                SearchResultsListBox.Invoke(mi);
            }
            else
            {
                mi.Invoke();        
            
            }
        }

        private void SearchResultsListAppend(Article article)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                var unzippedTitle = Zipper.Unzip(article.ZippedTitle);
                SearchResultsListBox.Items.Add(unzippedTitle);
            });
            if (SearchResultsListBox.InvokeRequired)
            {
                SearchResultsListBox.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetSearchResultsContextEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                SearchResultsContextMenu.Enabled = enabled;
                SearchResultsListBox.Enabled = enabled;
                GoogleButton.Enabled = enabled;
                SaveArticleButton.Enabled = enabled;
                OpenArticleButton.Enabled = enabled;
                PhraseToSearchButton.Enabled = enabled;
            });
            if (SearchResultsContextMenu.InvokeRequired)
            {
                SearchResultsContextMenu.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SearchResultsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var articleClicked = FoundArticles[SearchResultsListBox.SelectedIndex];

            var unzippedJson = Zipper.Unzip(articleClicked.ZippedJson);
            var article = JObject.Parse(unzippedJson);
            var rtfContent = JsonArticleToDocument.JsonArticleToRtf(article);
            ArticleRichTextBox.Rtf = rtfContent;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MarginRight = Width - ResultsSplitContainer.Right;
            MarginBottom = Height - ResultsSplitContainer.Bottom;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ResultsSplitContainer.Width = Width - MarginRight - ResultsSplitContainer.Left;
            ResultsSplitContainer.Height = Height - MarginBottom - ResultsSplitContainer.Top;
        }

        private void searchForArticleInGoogleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SearchResultsListBox.SelectedIndex >= 0 && SearchResultsListBox.SelectedIndex < SearchResultsListBox.Items.Count)
            {
                var article = FoundArticles[SearchResultsListBox.SelectedIndex];
                var unzippedTitle = Zipper.Unzip(article.ZippedTitle);
                System.Diagnostics.Process.Start("http://www.google.com.au/search?q=" + Uri.EscapeDataString(unzippedTitle));
            }
        }

        private void saveArticleAsWordDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SearchResultsListBox.SelectedIndex < 0 ||  SearchResultsListBox.SelectedIndex >= SearchResultsListBox.Items.Count)
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word document|*.docx";
            saveFileDialog.Title = "Save article as a Word File";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.
                if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    var selectedArticle = FoundArticles[SearchResultsListBox.SelectedIndex];
                    var unzippedJson = Zipper.Unzip(selectedArticle.ZippedJson);
                    var articleContent = File.ReadAllText(unzippedJson);
                    var jsonArticle = JObject.Parse(articleContent);
                    JsonArticleToDocument.JsonArticleToDocx(jsonArticle, saveFileDialog.FileName);
                }
            }
        }

        private void openArticleInWordPadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SearchResultsListBox.SelectedIndex < 0 || SearchResultsListBox.SelectedIndex >= SearchResultsListBox.Items.Count)
            {
                return;
            }

            var selectedArticle = FoundArticles[SearchResultsListBox.SelectedIndex];
            var unzippedJson = Zipper.Unzip(selectedArticle.ZippedJson);
            var tempFileName = Path.GetTempFileName() + ".rtf";

            JObject article = JObject.Parse(unzippedJson);
            var rtfContent = JsonArticleToDocument.JsonArticleToRtf(article);
            File.WriteAllText(tempFileName, rtfContent);
            System.Diagnostics.ProcessStartInfo value = null;
            value = new System.Diagnostics.ProcessStartInfo("wordpad.exe", tempFileName);
            System.Diagnostics.Process.Start(value);
        }

        private void GoogleButton_Click(object sender, EventArgs e)
        {
            searchForArticleInGoogleToolStripMenuItem_Click(this, null);
        }

        private void SaveArticleButton_Click(object sender, EventArgs e)
        {
            saveArticleAsWordDocumentToolStripMenuItem_Click(this, null);
        }

        private void OpenArticleButton_Click(object sender, EventArgs e)
        {
            openArticleInWordPadToolStripMenuItem_Click(this, null);
        }

        private List<String> GetJsonArticles(string jsonZippedCollectionName)
        {
            var result = new List<String>();

            using (var fs = new FileStream(jsonZippedCollectionName, FileMode.Open))
            { 
                using (var zip = new ZipArchive(fs))
                {
                    var entry = zip.Entries.First();

                    using (StreamReader sr = new StreamReader(entry.Open()))
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        result = serializer.Deserialize(sr.BaseStream) as List<String>;
                    }
                }
            }

            return result;
        }

        private void GetAllArticles()
        {
            SetSearchResultsContextMenEnabled(false);
            SetSearchResultsListBoxEnabled(false);
            SetGoogleButtonEnabled(false);
            SetSaveArticleButtonEnabled(false);
            SetOpenArticleButtonEnabled(false);
            SetPhraseToSearchButtonEnabled(false);

            var baseDirForArticles = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + 
                "\\CovidArticles\\Articles\\";
            var files = new DirectoryInfo(baseDirForArticles).GetFiles("*.bin.zip");
            AllArticles.Clear();

            int progressValue = 0;
            SetProgressBarMaximum(files.Count());
            foreach (var file in files)
            {
                progressValue++;
                SetOptionalMessage(string.Format("Creating database - zipped article set {0} out of {1} articles sets", progressValue, files.Count()));

                SetProgressBarValue(progressValue);

                var listOfArticles = GetJsonArticles(file.FullName);
                foreach (var jsonArticle in listOfArticles)
                {
                    var article = JsonArticleToDocument.GetArticle(jsonArticle);
                    AllArticles.Add(article);
                }
                listOfArticles.Clear();
            }

            SetSearchResultsContextMenEnabled(true);
            SetSearchResultsListBoxEnabled(true);
            SetGoogleButtonEnabled(true);
            SetSaveArticleButtonEnabled(true);
            SetOpenArticleButtonEnabled(true);
            SetPhraseToSearchButtonEnabled(true);
        }

        #region Mutlithreaded Access To Visual Elements

        private void SetOptionalMessage(string message)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                OptionalMessageLabel.Text = message;
            });
            if (OptionalMessageLabel.InvokeRequired)
            {
                OptionalMessageLabel.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetSearchResultsContextMenEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                SearchResultsContextMenu.Enabled = enabled;
            });
            if (SearchResultsContextMenu.InvokeRequired)
            {
                SearchResultsContextMenu.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetSearchResultsListBoxEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                SearchResultsListBox.Enabled = enabled;                
            });
            if (SearchResultsListBox.InvokeRequired)
            {
                SearchResultsListBox.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetGoogleButtonEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                GoogleButton.Enabled = enabled;
            });
            if (GoogleButton.InvokeRequired)
            {
                GoogleButton.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetSaveArticleButtonEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                SaveArticleButton.Enabled = enabled;
            });
            if (SaveArticleButton.InvokeRequired)
            {
                SaveArticleButton.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetOpenArticleButtonEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            { 
                OpenArticleButton.Enabled = enabled;
            });
            if (OpenArticleButton.InvokeRequired)
            {
                OpenArticleButton.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetPhraseToSearchButtonEnabled(bool enabled)

        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                PhraseToSearchButton.Enabled = enabled;
            });
            if (PhraseToSearchButton.InvokeRequired)
            {
                PhraseToSearchButton.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        #endregion Multithreaded Access To Visual Elements
    }
}
