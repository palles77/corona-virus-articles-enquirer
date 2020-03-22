using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CovidEnquirer
{
    public partial class MainForm : Form
    {
        private List<Article> FoundArticles;
        private int MarginRight;
        private int MarginBottom;

        public MainForm()
        {
            InitializeComponent();
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

            var files = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Articles\\").GetFiles("*.json");
            var textToFind = PhraseToSearchTextBox.Text.ToLower();
            
            int progressValue = 0;
            SetProgressBarMaximum(files.Count());
            foreach (var file in files)
            {
                progressValue++;
                SetProgressBarValue(progressValue);
                
                var article = JsonArticleToDocument.GetArticle(file.FullName);

                if (String.IsNullOrWhiteSpace(article.Title))
                {
                    continue;
                }

                int howManyWordsFound = 0;
                var wordsToFind = textToFind.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in wordsToFind)
                {
                    bool wasWordFound = false;
                    if (article.Title.ToLower().IndexOf(word) != -1)
                    {
                        howManyWordsFound++;
                        continue;
                    }

                    if (!wasWordFound && article.Abstract.ToLower().IndexOf(word) != -1)
                    {
                        howManyWordsFound++;
                        continue;
                    }

                    if (!wasWordFound && article.Content.ToLower().IndexOf(word) != -1)
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

            FoundArticles = FoundArticles.OrderBy(x => x.Title).ToList();
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
            MethodInvoker mi = new MethodInvoker(() => SearchResultsListBox.Items.Add(article.Title));
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

            string articleContent = File.ReadAllText(articleClicked.FileName);
            JObject article = JObject.Parse(articleContent);
            var rtfFileName = articleClicked.FileName + ".rtf";
            var rtfContent = JsonArticleToDocument.JsonArticleToRtf(article);
            ArticleRichTextBox.Rtf = rtfContent;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MarginRight = Width - ArticleRichTextBox.Right;
            MarginBottom = Height - ArticleRichTextBox.Bottom;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ArticleRichTextBox.Width = Width - MarginRight - ArticleRichTextBox.Left;
            ArticleRichTextBox.Height = Height - MarginBottom - ArticleRichTextBox.Top;
            SearchResultsListBox.Height = Height - MarginBottom - SearchResultsListBox.Top;
        }

        private void searchForArticleInGoogleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SearchResultsListBox.SelectedIndex >= 0 && SearchResultsListBox.SelectedIndex < SearchResultsListBox.Items.Count)
            {
                var article = FoundArticles[SearchResultsListBox.SelectedIndex];                
                String searchRequest = article.Title;
                System.Diagnostics.Process.Start("http://www.google.com.au/search?q=" + Uri.EscapeDataString(searchRequest));
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
                    var articleContent = File.ReadAllText(selectedArticle.FileName);
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
            var tempFileName = Path.GetTempFileName() + ".rtf";

            string articleContent = File.ReadAllText(selectedArticle.FileName);
            JObject article = JObject.Parse(articleContent);
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

        #region Mutlithreaded Access To Visual Elements
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
