using CovidLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace CovidTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            var articlesDir = args[0];
            var files = new DirectoryInfo(articlesDir).GetFiles("*.json");
            if (!Directory.Exists("Articles.Out"))
            {
                Directory.CreateDirectory("Articles.Out");
            }
            var listOfJsonArticles = new List<String>();
            var listOfArticleAbstracts = new List<Article>();

            int convertedArticlesIndex = 0;
            string baseOutputFileName;
            string outputFileName;
            foreach (var file in files)
            {
                baseOutputFileName = "converted_articles_" + convertedArticlesIndex + ".bin";
                string articleContent = File.ReadAllText(file.FullName);
                var article = JsonArticleToDocument.GetArticle(articleContent, baseOutputFileName + ".zip");
                listOfArticleAbstracts.Add(article);
                listOfJsonArticles.Add(articleContent);
                if (listOfJsonArticles.Count > 100)
                {
                    outputFileName = "Articles.Out\\" + baseOutputFileName;
                    SerialiseList(listOfJsonArticles, outputFileName);
                    convertedArticlesIndex++;
                    Console.WriteLine("Converted {0} out of {1} articles.", convertedArticlesIndex * 100, files.Length);
                }
            }

            if (listOfJsonArticles.Count > 0)
            {
                baseOutputFileName = "converted_articles_" + convertedArticlesIndex + ".bin";
                outputFileName = "Articles.Out\\" + baseOutputFileName;
                SerialiseList(listOfJsonArticles, outputFileName);
            }

            var indexFileName = "Articles.Out\\index.bin";
            SerialiseList(listOfArticleAbstracts, indexFileName);
        }

        private static void SerialiseList<T>(List<T> listOfArticles, string outputFileName)
        {     
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var demoFile = archive.CreateEntry(outputFileName);

                    using (var entryStream = demoFile.Open())                        
                    { 
                        BinaryFormatter serializer = new BinaryFormatter();
                        serializer.Serialize(entryStream, listOfArticles);
                    }
                }

                using (var fileStream = new FileStream(outputFileName + ".zip", FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
            }

            listOfArticles.Clear();
        }
    }
}
