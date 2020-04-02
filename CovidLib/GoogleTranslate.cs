using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace CovidLib
{
    public class GoogleTranslate
    {
        public static String Translate(string sentencesToTranslate, string language)
        {
            var sourceBuilder = new StringBuilder();
            var targetBuilder = new StringBuilder();
            var translatedPart = String.Empty;

            foreach (var line in sentencesToTranslate.Split('.'))
            {
                if (sourceBuilder.ToString().Length + line.Length > 5000)
                {
                    translatedPart = TranslateInternal(sourceBuilder.ToString(), language);
                    targetBuilder.Append(translatedPart);

                    sourceBuilder.Clear();
                }

                sourceBuilder.Append(line);
            }

            translatedPart = TranslateInternal(sourceBuilder.ToString(), language);
            targetBuilder.Append(translatedPart);

            return targetBuilder.ToString();
        }

        public static string TranslateInternal(string input, string language)
        {
            var randomSeconds = new Random().Next(5);
            while (randomSeconds < 2)
            {
                randomSeconds = new Random().Next(5);
            }
            Thread.Sleep(randomSeconds * 1000);

            // Set the language from/to in the url (or pass it into this function)
            string url = String.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             "en", language, Uri.EscapeUriString(input));
            HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;

            // Get all json data
            var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);

            // Extract just the first array element (This is the only data we are interested in)
            var translationItems = jsonData[0];

            // Translation Data
            string translation = "";

            // Loop through the collection extracting the translated objects
            foreach (object item in translationItems)
            {
                // Convert the item array to IEnumerable
                IEnumerable translationLineObject = item as IEnumerable;

                // Convert the IEnumerable translationLineObject to a IEnumerator
                IEnumerator translationLineString = translationLineObject.GetEnumerator();

                // Get first object in IEnumerator
                translationLineString.MoveNext();

                // Save its value (translated text)
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }

            // Remove first blank character
            if (translation.Length > 1) 
            { 
                translation = translation.Substring(1); 
            };

            // Return translation
            return translation;
        }
    }
}
