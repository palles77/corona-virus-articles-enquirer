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

        private Articles FoundArticles;
        private ArticlesHubHelper _articlesHubHelper;
        private int MarginRight;
        private int MarginBottom;
        private Object _ListBoxLock = new object();
        private const int MaxSearchAppendCount = 500;

        private readonly Dictionary<string, string> Languages = new Dictionary<string, string>()
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

            var threadStarter = new ThreadStart(ReadAllAbstracts);
            var thread = new Thread(threadStarter)
            {
                IsBackground = true
            };
            thread.Start();
        }

        #endregion Ctor

        #region UI 

        private void PhraseToSearchButtonClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(PhraseToSearchTextBox.Text))
            {
                return;
            }

            var threadStarter = new ThreadStart(SearchArticles);
            var thread = new Thread(threadStarter)
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void SearchResultsListClear()
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                lock (_ListBoxLock)
                {
                    SearchResultsListBox.Items.Clear();
                }
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

        private void SearchResultsListAppend(String[] titles)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                lock (_ListBoxLock)
                {
                    SearchResultsListBox.Items.AddRange(titles);
                }
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

        private void SearchResultsListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var articleClicked = GetSelectedArticle();
            if (ReferenceEquals(articleClicked, null))
            {
                return;
            }

            string unzippedJson = GetJsonArticle(articleClicked);
            try
            {
                var article = JObject.Parse(unzippedJson);
                var rtfContent = JsonArticleToDocument.JsonArticleToRtf(article);
                ArticleRichTextBox.Rtf = rtfContent;
            }
            catch { }
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            MarginRight = Width - ResultsSplitContainer.Right;
            MarginBottom = Height - ResultsSplitContainer.Bottom;
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            ResultsSplitContainer.Width = Width - MarginRight - ResultsSplitContainer.Left;
            ResultsSplitContainer.Height = Height - MarginBottom - ResultsSplitContainer.Top;
        }

        private void SearchForArticleInGoogleToolStripMenuItemClick(object sender, EventArgs e)
        {
            var articleClicked = GetSelectedArticle();
            if (ReferenceEquals(articleClicked, null))
            {
                return;
            }
            var unzippedTitle = Zipper.Unzip(articleClicked.ZippedTitle.ToByteArray());
            System.Diagnostics.Process.Start("http://www.google.com.au/search?q=" + Uri.EscapeDataString(unzippedTitle));
        }

        private void SaveArticleAsWordDocumentToolStripMenuItemClick(object sender, EventArgs e)
        {
            var articleClicked = GetSelectedArticle();
            if (ReferenceEquals(articleClicked, null))
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word document|*.docx",
                Title = "Save article as a Word File"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.
                if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
                {
                    SetElementsEnabled(false);
                    var unzippedJson = GetJsonArticle(articleClicked);
                    SetElementsEnabled(true);
                    var jsonArticle = JObject.Parse(unzippedJson);
                    JsonArticleToDocument.JsonArticleToDocx(jsonArticle, saveFileDialog.FileName);
                }
            }
        }

        private void OpenArticleInWordPadToolStripMenuItemClick(object sender, EventArgs e)
        {
            var articleClicked = GetSelectedArticle();
            if (ReferenceEquals(articleClicked, null))
            {
                return;
            }

            SetElementsEnabled(false);
            var unzippedJson = GetJsonArticle(articleClicked);
            SetElementsEnabled(true);
            var tempFileName = Path.GetTempFileName() + ".rtf";

            JObject article = JObject.Parse(unzippedJson);
            var rtfContent = ArticleRichTextBox.Rtf;
            File.WriteAllText(tempFileName, rtfContent);
            System.Diagnostics.ProcessStartInfo value = null;
            value = new System.Diagnostics.ProcessStartInfo("wordpad.exe", tempFileName);
            System.Diagnostics.Process.Start(value);
        }

        private void GoogleButtonClick(object sender, EventArgs e)
        {
            SearchForArticleInGoogleToolStripMenuItemClick(this, null);
        }

        private void SaveArticleButtonClick(object sender, EventArgs e)
        {
            SaveArticleAsWordDocumentToolStripMenuItemClick(this, null);
        }

        private void OpenArticleButtonClick(object sender, EventArgs e)
        {
            OpenArticleInWordPadToolStripMenuItemClick(this, null);
        } 

        private void SetElementsEnabled(bool enabled)
        {
            SetSearchResultsContextMenuEnabled(enabled);
            //SetSearchResultsListBoxEnabled(enabled);
            //SetGoogleButtonEnabled(enabled);
            //SetSaveArticleButtonEnabled(enabled);
            //SetOpenArticleButtonEnabled(enabled);
            SetPhraseToSearchButtonEnabled(enabled);
            SetTranslateButtonEnabled(enabled);
            SetLanguagesListEnabled(enabled);
            SetTitleSearchTextBoxEnabled(enabled);
        }

        private Article GetSelectedArticle()
        {
            Article result = null;
            lock (_ListBoxLock)
            {
                try
                {
                    result = FoundArticles.List[SearchResultsListBox.SelectedIndex];
                }
                catch { }
            }

            return result;
        }

        private void TranslateButtonClick(object sender, EventArgs e)
        {
            var articleClicked = GetSelectedArticle();
            if (ReferenceEquals(articleClicked, null))
            {
                return;
            }
            var targetLanguage = Languages[LanguagesComboBox.SelectedItem.ToString()];
            var threadStarter = new ThreadStart(() =>
            {
                Translate(articleClicked, targetLanguage);
            });
            var thread = new Thread(threadStarter)
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void Translate(Article articleClicked, string targetLanguage)
        {
            SetElementsEnabled(false);
            var unzippedJson = GetJsonArticle(articleClicked);
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

        private void TranslateToolStripMenuItemClick(object sender, EventArgs e)
        {
            TranslateButtonClick(sender, e);
        }

        private void TitleSearchTextBoxTextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TitleSearchTextBox.Text))
            {
                if (FoundArticles.List.Count != SearchResultsListBox.Items.Count)
                {
                    SearchResultsListClear();
                    CopyFoundArticles();
                }
            }
            else
            {
                SetElementsEnabled(false);
                SearchResultsListBox.Items.Clear();
                var list = FoundArticles.List
                    .Select(article => Zipper.Unzip(article.ZippedTitle.ToByteArray()))
                    .Where(title => !String.IsNullOrWhiteSpace(title) && 
                                    title.ToLower().IndexOf(TitleSearchTextBox.Text.ToLower()) != -1)
                    .ToList();

                var count = 0;
                var temporaryList = new List<string>();
                SearchResultsListClear();
                foreach (var title in list)
                {
                    temporaryList.Add(title);
                    if (temporaryList.Count() > MaxSearchAppendCount)
                    {
                        SearchResultsListAppend(temporaryList.ToArray());
                        temporaryList.Clear();
                    }
                    SetProgressBarValue(count++);
                }
                SearchResultsListAppend(temporaryList.ToArray());
            }
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

        private void SetTitleSearchTextBoxEnabled(bool enabled)
        {
            MethodInvoker mi = new MethodInvoker(() =>
            {
                TitleSearchTextBox.Enabled = enabled;
            });
            if (TitleSearchTextBox.InvokeRequired)
            {
                TitleSearchTextBox.Invoke(mi);
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
            SetOptionalMessage("Search in progress. After it finishes you can translate selected items.");

            var textToFind = PhraseToSearchTextBox.Text.ToLower();
            FoundArticles = _articlesHubHelper.SearchArticles(textToFind);

            CopyFoundArticles();

            SetElementsEnabled(true);
            SetOptionalMessage("Searching done.");
        }

        private void CopyFoundArticles()
        {
            var array = FoundArticles
                .List
                .Select(x => Zipper.Unzip(x.ZippedTitle.ToByteArray()))
                .ToArray();

            SetProgressBarMaximum(array.Count());

            var count = 0;
            var temporaryList = new List<string>();
            SearchResultsListClear();
            foreach (var title in array)
            {
                temporaryList.Add(title);
                if (temporaryList.Count() > MaxSearchAppendCount)
                {
                    SearchResultsListAppend(temporaryList.ToArray());
                    temporaryList.Clear();
                }
                SetProgressBarValue(count++);
            }
            SearchResultsListAppend(temporaryList.ToArray());
        }

        private Dictionary<String, String> GetJsonArticles(string bucketFileName)
        {
            String baseDirForArticles = GetDataDir();
            var result = new Dictionary<String, String>();
            using (var fs = new FileStream(Path.Combine(baseDirForArticles, bucketFileName), FileMode.Open))
            {
                using (var zip = new ZipArchive(fs))
                {
                    var entry = zip.Entries.First();

                    using (StreamReader sr = new StreamReader(entry.Open()))
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        result = serializer.Deserialize(sr.BaseStream) as Dictionary<String, String>;
                    }
                }
            }

            return result;
        }

        private void ReadAllAbstracts()
        {
            SetElementsEnabled(false);
            SetOptionalMessage("Reading articles...");

            String baseDirForArticles = GetDataDir();
            _articlesHubHelper = new ArticlesHubHelper(baseDirForArticles);
    
            SetElementsEnabled(true);
            SetOptionalMessage("Reading articles finished. You can start searching now.");
            SetLanguagesComboBox(Languages.Keys.ToList());
        }

        private String GetJsonArticle(Article article)
        {
            string result = string.Empty;
            var bucketFileName = Zipper.Unzip(article.ZippedBucketFileName.ToByteArray());
            var jsonArticles = GetJsonArticles(bucketFileName);
            var jsonFileName = Zipper.Unzip(article.ZippedJsonFileName.ToByteArray());
            if (jsonArticles.ContainsKey(jsonFileName))
            {
                result = jsonArticles[jsonFileName];
            }

            return result;
        }

        private string GetDataDir()
        {
            // TODO: LMP - uncomment when debug String baseDirForArticles = @"..\..\Data\";
            // TODO: LMP - comment out line below when debug
            var baseDirForArticles = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                "CovidEnquirer", 
                "Data");

            return baseDirForArticles;
        }

        #endregion Articles Search
    }
}
