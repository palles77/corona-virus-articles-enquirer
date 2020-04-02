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
            var listOfArticles = new List<String>();
            int convertedAritclesIndex = 0;
            foreach (var file in files)
            {
                string articleContent = File.ReadAllText(file.FullName);
                listOfArticles.Add(articleContent);
                if (listOfArticles.Count > 1000)
                {
                    var outputFileName = "Articles.Out\\converted_articles_" + convertedAritclesIndex + ".bin";
                    SerialiseListOfConvertedArticles(listOfArticles, outputFileName);
                    convertedAritclesIndex++;
                }
            }

            if (listOfArticles.Count > 0)
            {
                var outputFileName = "converted_articles_" + convertedAritclesIndex + ".bin";
                SerialiseListOfConvertedArticles(listOfArticles, outputFileName);
                convertedAritclesIndex++;
            }
        }

        static void SerialiseListOfConvertedArticles(List<String> listOfArticles, string outputFileName)
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
