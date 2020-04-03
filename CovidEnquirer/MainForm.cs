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
        #region Properties

        private List<Article> FoundArticles;
        private List<Article> AllArticles = new List<Article>();
        private int MarginRight;
        private int MarginBottom;

        private Dictionary<string, string> Languages = new Dictionary<string, string>()
        {
            { "Chinese",    "zh-TW" },
            { "Czech",      "cs"    },
            { "French",     "fr"    },
            { "German",     "de"    },
            { "Greek",      "el"    },
            { "Italian",    "it"    },
            { "Japanese",   "jp"    },
            { "Polish",     "pl"    },
            { "Portuguese", "pt"    },
            { "Russian",    "ru"    },
            { "Spanish",    "es"    },
        };

        #endregion Properties

        #region Ctor

        public MainForm()
        {
            InitializeComponent();

            var threadStarter = new ThreadStart(GetAllArticles);
            var thread = new Thread(threadStarter);
            thread.IsBackground = true;
            thread.Start();
        }

        #endregion Ctor

        #region UI 

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
                    var jsonArticle = JObject.Parse(unzippedJson);
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
            var rtfContent = ArticleRichTextBox.Rtf;
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

        private void SetElementsEnabled(bool enabled)
        {
            SetSearchResultsContextMenuEnabled(enabled);
            SetSearchResultsListBoxEnabled(enabled);
            SetGoogleButtonEnabled(enabled);
            SetSaveArticleButtonEnabled(enabled);
            SetOpenArticleButtonEnabled(enabled);
            SetPhraseToSearchButtonEnabled(enabled);
            SetTranslateButtonEnabled(enabled);
            SetLanguagesListEnabled(enabled);
        }

        private void TranslateButton_Click(object sender, EventArgs e)
        {
            var articleClicked = FoundArticles[SearchResultsListBox.SelectedIndex];
            var targetLanguage = Languages[LanguagesComboBox.SelectedItem.ToString()];
            var threadStarter = new ThreadStart(() => { Translate(articleClicked, targetLanguage); });
            var thread = new Thread(threadStarter);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Translate(Article articleClicked, string targetLanguage)
        {
            SetElementsEnabled(false);
            var unzippedJson = Zipper.Unzip(articleClicked.ZippedJson);
            var article = JObject.Parse(unzippedJson);
            int howManyElementsToTranslate = JsonArticleToDocument.JsonArticleHowManyElementsToTranslate(article);
            SetProgressBarMaximum(howManyElementsToTranslate);
            var rtfContent = JsonArticleToDocument.JsonArticleToRtfTranslate(article, targetLanguage, TranslateCallback);
            SetArticleContent(rtfContent);
            SetOptionalMessage("Translation finished.");
            SetElementsEnabled(true);
        }

        private void TranslateCallback(int value)
        {
            SetProgressBarValue(value);
            SetOptionalMessage("Translation in progress...");
        }

        private void translateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TranslateButton_Click(sender, e);
        }

        #endregion UI

        #region Mutlithreaded Access To Visual Elements

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

        private void SetSearchResultsContextMenuEnabled(bool enabled)
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

        private void SetArticleContent(string rtfContent)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                ArticleRichTextBox.Rtf = rtfContent;
            });
            if (ArticleRichTextBox.InvokeRequired)
            {
                ArticleRichTextBox.Invoke(mi);
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

        private void SetTranslateButtonEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                TranslateButton.Enabled = enabled;
            });
            if (TranslateButton.InvokeRequired)
            {
                TranslateButton.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetLanguagesListEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                LanguagesComboBox.Enabled = enabled;
            });
            if (LanguagesComboBox.InvokeRequired)
            {
                LanguagesComboBox.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        private void SetLanguagesComboBox(List<string> languages)

        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                LanguagesComboBox.Items.AddRange(languages.ToArray());
                LanguagesComboBox.SelectedIndex = 0;

            });
            if (LanguagesComboBox.InvokeRequired)
            {
                LanguagesComboBox.Invoke(mi);
            }
            else
            {
                mi.Invoke();
            }
        }

        #endregion Multithreaded Access To Visual Elements

        #region Articles Search

        private void SearchArticles()
        {
            SetElementsEnabled(false);

            SetProgressBarValue(0);
            FoundArticles = new List<Article>();

            var textToFind = PhraseToSearchTextBox.Text.ToLower();

            int progressValue = 0;
            SetProgressBarMaximum(AllArticles.Count);

            var wordsToFind = textToFind.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<KnuthPrattMorris> kmpList = new List<KnuthPrattMorris>();
            foreach (var word in wordsToFind)
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

            SetElementsEnabled(true);
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
            SetElementsEnabled(false);

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

            SetElementsEnabled(true);
            SetLanguagesComboBox(Languages.Keys.ToList());
        }

        #endregion Articles Search
    }
}
