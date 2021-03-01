using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CovidLib
{
    public class ArticlesHubHelper
    {
        private readonly String _inputDirectory;
        private readonly String _outputDirectory;

        private readonly String[] Letters = new string[] {
            "a", "b", "c", "d", "e", "f", "g", "h",
            "i", "j", "l", "m", "n", "o", "p", "q",
            "r", "s", "t", "u", "v", "w", "x", "y", "z"};

        private const string REST = "rest";

        /// <summary>
        /// Dictionary which splits keywords into a flat structure
        /// of two letters keys, which later get mapped into specific
        /// keys starting with that letter, that get mapped into list of
        /// articles which have titles or abstracts containing the key words.
        /// </summary>
        private Dictionary<String, ArticlesHub> ArticlesHubDictionary;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="inputDirectory">Input directory where JSON files are placed.</param>
        /// <param name="outputDirectory">Output directory where BIN packed JSON articles are placed with index.</param>
        public ArticlesHubHelper(String inputDirectory, String outputDirectory)
        {
            InitKeys();

            _inputDirectory = inputDirectory;

            _outputDirectory = outputDirectory;
            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="outputDirectory">Directory with articles zipped and indexed.</param>
        public ArticlesHubHelper(string outputDirectory)
        {
            InitKeys();
            _outputDirectory = outputDirectory;
        }

        #region Public Methods

        /// <summary>
        /// Searches for a specific list of articles containing key words in abstract.
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public Articles SearchArticles(string phraseToSearch)
        {
            var result = new Articles();
            var smallKeyWords = phraseToSearch
                .ToLower()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();

            var commonArticles = new Articles();
            bool haveCommonArticlesBeenSet = false;

            // First check if a key word is found or not.
            foreach (var keyWord in smallKeyWords)
            {
                if (keyWord.Length < 3)
                {
                    // no intereset in short words
                    continue;
                }
                var keyWordIndex = keyWord.ToLower().Substring(0, 3);
                if (!ArticlesHubDictionary.ContainsKey(keyWordIndex))
                {
                    keyWordIndex = REST;
                }

                // now look at contents of index file
                var articlesHub = new ArticlesHub();
                var indexPath = Path.Combine(_outputDirectory, "index", $"index_{keyWordIndex}.bin.zip");
                if (!File.Exists(indexPath))
                {
                    continue;
                }

                articlesHub.Deserialize(indexPath);

                // now search for keywords in this hub
                if (articlesHub.KeyWordsDictionary.Dictionary.Keys.Contains(keyWord))
                {
                    var articlesFound = articlesHub.KeyWordsDictionary.Dictionary[keyWord];
                    var commonArticlesCopy = new Articles();

                    if (haveCommonArticlesBeenSet)
                    {
                        var commonArticlesHashSet = commonArticles
                                .List.Select(x => Zipper.Unzip(x.ZippedJsonFileName.ToByteArray()))
                                .ToHashSet();

                        foreach (var articleFound in articlesFound.List)
                        {
                            var articleFoundFileName = Zipper.Unzip(articleFound.ZippedJsonFileName.ToByteArray());
                            var hasCommonArticle = commonArticlesHashSet.Contains(articleFoundFileName);
                            if (hasCommonArticle)
                            {
                                var isTitleEmpty =
                                    String.IsNullOrWhiteSpace(Zipper.Unzip(articleFound.ZippedTitle.ToByteArray()));
                                commonArticlesCopy.List.Add(articleFound);
                            }
                        }

                        commonArticles = commonArticlesCopy;
                    }
                    else
                    {
                        var nonEmptyTitlesList = articlesFound.List
                            .Where(x => !String.IsNullOrWhiteSpace(Zipper.Unzip(x.ZippedTitle.ToByteArray())));
                        commonArticles.List.AddRange(nonEmptyTitlesList);
                        haveCommonArticlesBeenSet = true;
                    }
                }
            }

            return commonArticles;
        }

        /// <summary>
        /// Retrieves J
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public string GetJsonArticle(Article article)
        {
            var unpackedDictionary = new Dictionary<String, String>();
            var jsonFileName = Zipper.Unzip(article.ZippedJsonFileName.ToByteArray());
            var bucketFileName = Zipper.Unzip(article.ZippedBucketFileName.ToByteArray());
            using (var fs = new FileStream(Path.Combine(_inputDirectory, bucketFileName), FileMode.Open))
            {
                using (var zip = new ZipArchive(fs))
                {
                    var entry = zip.Entries.First();

                    using (StreamReader sr = new StreamReader(entry.Open()))
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        unpackedDictionary = serializer.Deserialize(sr.BaseStream) as Dictionary<String, String>;
                    }
                }
            }
            var json = String.Empty;
            if (unpackedDictionary.ContainsKey(jsonFileName))
            {
                json = unpackedDictionary[jsonFileName];
            }

            return json;
        }

        /// <summary>
        /// Serializes input directory with JSON articles to output directory.
        /// </summary>
        /// <param name="articlesInputDir">Input directory where JSON files are located.</param>
        /// <param name="articlesOutputDir">Where BIN packed files with index are placed.</param>
        /// <param name="resetCounter">How many articles per single BIN packed file.</param>
        public void SerializeJsonArticles(int resetCounter = 2000)
        {
            var files = new DirectoryInfo(_inputDirectory).GetFiles("*.json");

            int convertedArticlesIndex = 0;
            string baseOutputFileName;
            string outputFileName;

            var dictionaryOfJsonArticles = new Dictionary<String, String>();

            foreach (var file in files)
            {
                // Adds article to temporary dictionary and updates collection of ArticlesHubDictionary.
                AddArticle(ref convertedArticlesIndex, resetCounter, file, _outputDirectory, files.Length, dictionaryOfJsonArticles);
            }

            // Save remnants of articles dictionary.
            if (dictionaryOfJsonArticles.Count > 0)
            {
                baseOutputFileName = "converted_articles_" + convertedArticlesIndex + ".bin";
                outputFileName = _outputDirectory + baseOutputFileName;
                SerializeDictionary(dictionaryOfJsonArticles, outputFileName);
            }

            // Now lets store ArticlesHubDictionary
            StoreArticlesHubDictionary();
        }
        
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Inits keys.
        /// </summary>
        private void InitKeys()
        {
            ArticlesHubDictionary = new Dictionary<string, ArticlesHub>();
            foreach (var letter1 in Letters)
            {
                foreach (var letter2 in Letters)
                {
                    foreach (var letter3 in Letters)
                    {
                        var threeLetters = letter1 + letter2 + letter3;
                        ArticlesHubDictionary[threeLetters] = new ArticlesHub();
                    }
                }
            }
            ArticlesHubDictionary[REST] = new ArticlesHub();
        }

        /// <summary>
        /// Serializes a current dictionary of 
        /// </summary>
        /// <param name="dictionaryOfArticles"></param>
        /// <param name="outputFileName"></param>
        private void SerializeDictionary(Dictionary<String, String> dictionaryOfArticles, string outputFileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var file = archive.CreateEntry(outputFileName);
                    using (var entryStream = file.Open())
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        serializer.Serialize(entryStream, dictionaryOfArticles);
                    }
                }

                using (var fileStream = new FileStream(outputFileName + ".zip", FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
            }

            dictionaryOfArticles.Clear();
        }

        /// <summary>
        /// Adds a selected article to dictionary of articles which is periodically saved every reset counter.
        /// </summary>
        /// <param name="convertedArticlesIndex">How many .bin.zip packed files are created.</param>
        /// <param name="resetCounter">Sets maximum amount of packed json articles per bin.zip file.</param>
        /// <param name="articleFileInfo">File info of JSON article.</param>
        /// <param name="filesCount">How many JSON files are analysed in total.</param>
        /// <param name="dictionaryOfJsonArticles">Temporary dictionary of articles periodically saved.</param>
        private void AddArticle(
            ref int convertedArticlesIndex, 
            int resetCounter,
            FileInfo articleFileInfo,
            string articlesOutputDir,
            int filesCount,
            Dictionary<String, String> dictionaryOfJsonArticles)
        {
            var baseOutputFileName = "converted_articles_" + convertedArticlesIndex + ".bin";
            var articleJsonContent = File.ReadAllText(articleFileInfo.FullName);
            var articleAbstract = JsonArticleToDocument.GetArticleAbstract(articleJsonContent);
            var articleTitle = JsonArticleToDocument.GetArticleTitle(articleJsonContent);

            if (String.IsNullOrWhiteSpace(articleAbstract) && String.IsNullOrWhiteSpace(articleTitle))
            {
                return;
            }

            var article = new Article()
            {
                ZippedJsonFileName = ByteString.CopyFrom(Zipper.Zip(articleFileInfo.Name)),
                ZippedBucketFileName = ByteString.CopyFrom(Zipper.Zip(baseOutputFileName + ".zip")),
                ZippedTitle = ByteString.CopyFrom(Zipper.Zip(articleTitle)),
                CounterIndex = convertedArticlesIndex
            };

            List<String> keyWords = new List<String>();            
            
            if (!String.IsNullOrWhiteSpace(articleAbstract))
            {
                // abstract takes precedence
                List<String> list = articleAbstract
                        .ToLower()
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Distinct()
                        .ToList();              
                keyWords.AddRange(list);
            }

            if (!String.IsNullOrWhiteSpace(articleTitle))
            {
                // is followed by title.
                List<String> list = articleTitle
                        .ToLower()
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        .Distinct()
                        .ToList();
                keyWords.AddRange(list);
            }

            // make sure no repetitions
            keyWords = keyWords
                .Distinct()
                .ToList();

            // Process every single key word.
            foreach (var keyWord in keyWords)
            {
                if (keyWord.Length < 3)
                {
                    // we dont care for less than 3 letters
                    continue;
                }

                var firstThreeLetters = keyWord.ToLower().Substring(0, 3);
                var articlesHubDictionaryKey = firstThreeLetters;
                if (!ArticlesHubDictionary.Keys.Contains(firstThreeLetters))
                {
                    articlesHubDictionaryKey = REST;
                }

                var articlesHub = ArticlesHubDictionary[articlesHubDictionaryKey];
                articlesHub.AddArticle(keyWord, article);
            }

            dictionaryOfJsonArticles[articleFileInfo.Name] = articleJsonContent;

            if (dictionaryOfJsonArticles.Keys.Count >= resetCounter)
            {
                var outputFileName = articlesOutputDir + baseOutputFileName;
                SerializeDictionary(dictionaryOfJsonArticles, outputFileName);
                convertedArticlesIndex++;
                Console.WriteLine("Converted {0} out of {1} articles.", convertedArticlesIndex * resetCounter, filesCount);
            }
        }

        /// <summary>
        /// Stores a collection of ArticlesHubDictionary in separate directories
        /// </summary>
        private void StoreArticlesHubDictionary()
        {
            foreach (var kvp in ArticlesHubDictionary)
            {
                var articlesHub = kvp.Value;
                var articlesCount = 0;
                foreach (var key in articlesHub.KeyWordsDictionary.Dictionary.Keys)
                {
                    articlesCount += articlesHub.KeyWordsDictionary.Dictionary[key].List.Count();
                }

                if (articlesCount == 0)
                {
                    continue;
                }

                string indexDirectory = Path.Combine(_outputDirectory, "index");
                if (!Directory.Exists(indexDirectory))
                {
                    Directory.CreateDirectory(indexDirectory);
                }
                var outputFileName = Path.Combine(indexDirectory, $"index_{kvp.Key}.bin");
                articlesHub.Serialize(outputFileName);
            }
        }

        #endregion Private Methods
    }
}
