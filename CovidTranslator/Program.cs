using CovidLib;
using System.IO;

namespace CovidTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputDir = args[0];
            var outputDir = "Articles.Out\\";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var articlesHubHelper = new ArticlesHubHelper(inputDir, outputDir);
            articlesHubHelper.SerializeJsonArticles(1000);
        }
    }
}
