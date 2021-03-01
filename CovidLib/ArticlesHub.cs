using Google.Protobuf;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace CovidLib
{
    [Serializable]
    public class ArticlesHub
    {
        #region Public Properties

        /// <summary>
        /// Dictionary translating keyword -> list of zipped article filenames.
        /// </summary>
        public StringToArticlesDictionary KeyWordsDictionary { get; set; }

        #endregion Public Properties

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        public ArticlesHub()
        {
            KeyWordsDictionary = new StringToArticlesDictionary();
        }

        #endregion Ctor

        #region Public Methods

        /// <summary>
        /// Adds a specific article to collection of articles, where 
        /// a single word maps to list of articles which have the same
        /// keyword.
        /// </summary>
        /// <param name="keyWord">Key word to be mapped.</param>
        /// <param name="article">Article to be added.</param>
        public void AddArticle(string keyWord, Article article)
        {
            if (!KeyWordsDictionary.Dictionary.Keys.Contains(keyWord))
            {
                var articles = new Articles();
                articles.List.Add(article);
                KeyWordsDictionary.Dictionary.Add(keyWord, articles);
            }
            else
            {
                var articles = KeyWordsDictionary.Dictionary[keyWord];
                articles.List.Add(article);
            }
        }

        /// <summary>
        /// Serialize to a specific fle location.
        /// </summary>
        /// <param name="outputFileName"></param>
        public void Serialize(string outputFileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var file = archive.CreateEntry(outputFileName);
                    using (var entryStream = file.Open())
                    {
                        using (var output = new CodedOutputStream(entryStream))
                        {
                            KeyWordsDictionary.WriteTo(output);
                        }
                    }
                }

                var path = outputFileName + ".zip";
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
            }
        }

        /// <summary>
        /// Deserialize from a specific file location.
        /// </summary>
        /// <param name="outputFileName"></param>
        public void Deserialize(string indexFileName)
        {
            using (var fs = new FileStream(indexFileName, FileMode.Open))
            {
                using (var zip = new ZipArchive(fs))
                {
                    using (StreamReader sr = new StreamReader(zip.Entries.First().Open()))
                    {
                        using (var input = new CodedInputStream(sr.BaseStream))
                        {
                            KeyWordsDictionary = StringToArticlesDictionary.Parser.ParseFrom(input);
                        }                        
                    }
                }
            }
        }

        #endregion Public Methods
    }
}
