using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ESCommon.Rtf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CovidLib
{
    public class JsonArticleToDocument
    {
        #region Delegates

        public delegate void SetValue(int value);

        #endregion Delegates

        #region Public Methods

        public static int JsonArticleHowManyElementsToTranslate(JObject article)
        {
            int result = 0;

            var rtf = new RtfDocument();
            var listOfDocumentParts = new List<RtfDocumentContentBase>();

            var titleParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(16, RtfTextAlign.Center));
            if (article["metadata"] != null && article["metadata"]["title"] != null)
            {
                result++;
            }
            

            // Add abstract.
            var abstractParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(12, RtfTextAlign.Left));
            abstractParagraph.AppendParagraph();
            if (article["abstract"] != null && article["abstract"].Children().Count() > 0)
            {
                foreach (var articleAbstract in article["abstract"].Children())
                {
                    result++;
                }
            }
            listOfDocumentParts.Add(abstractParagraph);

            // Add content.
            if (article["body_text"] != null && article["body_text"].Children().Count() > 0)
            {
                foreach (var bodyText in article["body_text"].Children())
                {
                    result++;
                }
            }

            return result;
        }

        public static string JsonArticleToRtfTranslate(JObject article, string targetLanguage, SetValue setValueCallback)
        {
            int counter = 0;
            var rtf = new RtfDocument();
            var listOfDocumentParts = new List<RtfDocumentContentBase>();

            var titleParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(16, RtfTextAlign.Center));
            if (article["metadata"] != null && article["metadata"]["title"] != null)
            {
                setValueCallback(counter++);
                var translatedTitle = GoogleTranslate.Translate(article["metadata"]["title"].ToString(), targetLanguage);
                titleParagraph.AppendText(
                    new RtfFormattedText(translatedTitle, RtfCharacterFormatting.Bold));
            }
            titleParagraph.AppendParagraph();
            listOfDocumentParts.Add(titleParagraph);

            if (article["metadata"] != null && article["metadata"]["authors"]?.Children() != null)
            {
                foreach (var author in article["metadata"]["authors"].Children())
                {
                    var authorParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(14, RtfTextAlign.Left));
                    var authorLine = GetAuthor(author);

                    if (authorLine.ToString().Length >= 10)
                    {
                        // We should not add some dummy values.
                        var authorLineAsString = authorLine.Substring(0, authorLine.Length - 2);
                        authorParagraph.AppendText(authorLineAsString);
                        listOfDocumentParts.Add(authorParagraph);
                    }
                }
            }

            // Add abstract.
            var abstractParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(12, RtfTextAlign.Left));
            abstractParagraph.AppendParagraph();
            if (article["abstract"] != null && article["abstract"].Children().Count() > 0)
            {
                foreach (var articleAbstract in article["abstract"].Children())
                {
                    var translatedAbstract = GoogleTranslate.Translate(articleAbstract["text"].ToString(), targetLanguage);
                    setValueCallback(counter++);
                    abstractParagraph.AppendText(
                        new RtfFormattedText(translatedAbstract, RtfCharacterFormatting.Italic));
                    abstractParagraph.AppendParagraph();
                }
            }
            listOfDocumentParts.Add(abstractParagraph);

            // Add content.
            if (article["body_text"] != null && article["body_text"].Children().Count() > 0)
            {
                foreach (var bodyText in article["body_text"].Children())
                {
                    var contentParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(12, RtfTextAlign.Left));
                    setValueCallback(counter++);
                    var translatedBody = GoogleTranslate.Translate(bodyText["text"].ToString(), targetLanguage);
                    contentParagraph.AppendText(translatedBody);
                    contentParagraph.AppendParagraph();
                    listOfDocumentParts.Add(contentParagraph);
                }
            }

            // Add Bibliography
            foreach (var bibEntry in article["bib_entries"].Children())
            {
                var bibliographyParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(10, RtfTextAlign.Left));
                var bibEntryAsString = GetBibEntry(bibEntry);

                // Control spacing
                bibliographyParagraph.AppendText(bibEntryAsString);
                listOfDocumentParts.Add(bibliographyParagraph);
            }

            var arrayOfDocumentParts = listOfDocumentParts.ToArray();
            rtf.Contents.AddRange(arrayOfDocumentParts);

            var rtfWriter = new RtfWriter();
            var result = rtfWriter.GetRtfContent(rtf);

            return result;
        }

        public static string JsonArticleToRtf(JObject article)
        {
            var rtf = new RtfDocument();
            var listOfDocumentParts = new List<RtfDocumentContentBase>();

            var titleParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(16, RtfTextAlign.Center));
            if (article["metadata"] != null && article["metadata"]["title"] != null)
            {
                titleParagraph.AppendText(
                    new RtfFormattedText(article["metadata"]["title"].ToString(), RtfCharacterFormatting.Bold));
            }
            titleParagraph.AppendParagraph();
            listOfDocumentParts.Add(titleParagraph);

            if (article["metadata"] != null && article["metadata"]["authors"]?.Children() != null)
            {
                foreach (var author in article["metadata"]["authors"].Children())
                {
                    var authorParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(14, RtfTextAlign.Left));
                    var authorLine = GetAuthor(author);

                    if (authorLine.ToString().Length >= 10)
                    {
                        // We should not add some dummy values.
                        var authorLineAsString = authorLine.Substring(0, authorLine.Length - 2);
                        authorParagraph.AppendText(authorLineAsString);
                        listOfDocumentParts.Add(authorParagraph);
                    }
                }
            }

            // Add abstract.
            var abstractParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(12, RtfTextAlign.Left));
            abstractParagraph.AppendParagraph();
            if (article["abstract"] != null && article["abstract"].Children().Count() > 0)
            {
                foreach (var articleAbstract in article["abstract"].Children())
                {
                    abstractParagraph.AppendText(
                        new RtfFormattedText(articleAbstract["text"].ToString(), RtfCharacterFormatting.Italic));
                    abstractParagraph.AppendParagraph();
                }
            }
            listOfDocumentParts.Add(abstractParagraph);

            // Add content.
            if (article["body_text"] != null && article["body_text"].Children().Count() > 0)
            {
                foreach (var bodyText in article["body_text"].Children())
                {
                    var contentParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(12, RtfTextAlign.Left));
                    contentParagraph.AppendText(bodyText["text"].ToString());
                    contentParagraph.AppendParagraph();
                    listOfDocumentParts.Add(contentParagraph);
                }
            }

            // Add Bibliography
            foreach (var bibEntry in article["bib_entries"].Children())
            {
                var bibliographyParagraph = new RtfFormattedParagraph(new RtfParagraphFormatting(10, RtfTextAlign.Left));
                var bibEntryAsString = GetBibEntry(bibEntry);

                // Control spacing
                bibliographyParagraph.AppendText(bibEntryAsString);
                listOfDocumentParts.Add(bibliographyParagraph);
            }

            var arrayOfDocumentParts = listOfDocumentParts.ToArray();
            rtf.Contents.AddRange(arrayOfDocumentParts);

            var rtfWriter = new RtfWriter();
            var result = rtfWriter.GetRtfContent(rtf);

            return result;
        }

        public static void JsonArticleToDocx(JObject article, string wordFileName)
        {
            var wordProcessingDocument = WordprocessingDocument
                .Create(wordFileName, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);

            wordProcessingDocument.AddMainDocumentPart();
            wordProcessingDocument.MainDocumentPart.Document = new Document();
            wordProcessingDocument.MainDocumentPart.Document.Body = new Body();
            var body = wordProcessingDocument.MainDocumentPart.Document.Body;

            AddSection("Title", body, 20);
            var titleParagraph = body.AppendChild(new Paragraph());
            var titleRun = titleParagraph.AppendChild(new Run());
            var titleRunProperties = new RunProperties();
            var titleFontSize = new FontSize() { Val = "18" };
            titleRunProperties.Append(titleFontSize);
            titleRun.Append(titleRunProperties);
            titleRun.Append(new Text(article["metadata"]["title"].ToString()));

            AddSection("Authors", body, 20);
            foreach (var author in article["metadata"]["authors"].Children())
            {
                var authorLine = GetAuthor(author);
                if (authorLine.Length >= 10)
                {
                    // We should not add some dummy values.
                    var spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    var paragraphProperties = new ParagraphProperties();
                    paragraphProperties.Append(spacing);

                    var authorRunProperties = new RunProperties();
                    var authorFontSize = new FontSize() { Val = "10" };
                    authorRunProperties.Append(authorFontSize);
                    var authorParagraph = body.AppendChild(new Paragraph());
                    authorParagraph.Append(paragraphProperties);
                    var authorRun = authorParagraph.AppendChild(new Run());
                    authorRun.Append(authorRunProperties);
                    authorRun.Append(new Text(authorLine.Substring(0, authorLine.Length - 2) + "\r\n"));
                }
            }

            AppendEmptyLine(body);

            // Add abstract.
            AddSection("Abstract", body, 20);
            if (article["abstract"] != null && article["abstract"].Children().Count() > 0)
            {
                foreach (var articleAbstract in article["abstract"].Children())
                {
                    var abstractParagraph = body.AppendChild(new Paragraph());
                    var abstractRun = abstractParagraph.AppendChild(new Run());
                    var abstractRunProperties = new RunProperties();
                    var abstractFontSize = new FontSize() { Val = "12" };
                    abstractRunProperties.Append(abstractFontSize);
                    abstractRun.Append(abstractRunProperties);
                    abstractRun.Append(new Text(articleAbstract["text"].ToString()));
                }
            }

            AppendEmptyLine(body);


            // Add content.
            AddSection("Content", body, 20);
            if (article["body_text"] != null && article["body_text"].Children().Count() > 0)
            {
                foreach (var bodyText in article["body_text"].Children())
                {
                    var bodyTextParagraph = body.AppendChild(new Paragraph());
                    var bodyTextRun = bodyTextParagraph.AppendChild(new Run());
                    var bodyTextRunProperties = new RunProperties();
                    var bodyTextFontSize = new FontSize() { Val = "12" };
                    bodyTextRunProperties.Append(bodyTextFontSize);
                    bodyTextRun.Append(bodyTextRunProperties);
                    bodyTextRun.Append(new Text(bodyText["text"].ToString()));
                }
            }

            AddSection("Bibliography", body, 20);
            {
                foreach (var bibEntry in article["bib_entries"].Children())
                {
                    var bibEntryAsString = GetBibEntry(bibEntry);

                    // Control spacing
                    var spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                    var bibEntryParagraphProperties = new ParagraphProperties();
                    bibEntryParagraphProperties.Append(spacing);

                    var bibEntryRunProperties = new RunProperties();
                    var bibEntryFontSize = new FontSize() { Val = "10" };
                    bibEntryRunProperties.Append(bibEntryFontSize);
                    var bibEntryParagraph = body.AppendChild(new Paragraph());
                    var bibEntryRun = bibEntryParagraph.AppendChild(new Run());
                    bibEntryRun.Append(bibEntryRunProperties);
                    var bibEntryText = bibEntryAsString.Substring(0, bibEntryAsString.Length - 2);
                    bibEntryRun.Append(new Text(bibEntryText));
                }
            }

            try
            {
                wordProcessingDocument.SaveAs(wordFileName).Close();
            }
            catch
            {
                // For some reasone exception is thrown here.
            }

            wordProcessingDocument.Dispose();
        }

        public static Article GetArticle(string articleContent)
        {
            Article result = new Article();
            var title = "";
            var articleAbstractBuilder = new StringBuilder();
            var zippedJson = Zipper.Zip(articleContent);

            try
            {
                JObject article = JObject.Parse(articleContent);

                if (article["metadata"] != null && article["metadata"]["title"] != null)
                {
                    title = article["metadata"]["title"].ToString();
                }

                if (article["abstract"] != null && article["abstract"].Children().Count() > 0)
                {
                    foreach (var articleAbstract in article["abstract"].Children())
                    {
                        articleAbstractBuilder.AppendLine(articleAbstract["text"].ToString());
                    }
                }
            }
            catch
            { 
            }

            result.ZippedTitle = Zipper.Zip(title);
            result.ZippedAbstract = Zipper.Zip(articleAbstractBuilder.ToString());
            result.ZippedJson = zippedJson;

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private static void AppendEmptyLine(Body body)
        {
            var bodyParagraph = body.AppendChild(new Paragraph());
            var bodyRun = bodyParagraph.AppendChild(new Run());
            bodyRun.Append(new Text());
        }

        private static void AppendLine(StringBuilder builder, string nameOfElement, string elementToPotentiallyAppend)
        {
            if (!String.IsNullOrEmpty(elementToPotentiallyAppend))
            {
                builder.Append(string.Format("{0} {1}, ", nameOfElement, elementToPotentiallyAppend));
            }
        }

        private static void AddSection(string sectionName, Body body, int fontSizeAsInt)
        {
            var runProperties = new RunProperties();
            var fontSize = new FontSize() { Val = fontSizeAsInt.ToString() };
            runProperties.Append(fontSize);
            var paragraph = body.AppendChild(new Paragraph());
            var run = paragraph.AppendChild(new Run());
            run.Append(runProperties);
            run.Append(new Text(sectionName));
        }

        private static string GetBibEntry(JToken bibEntry)
        {
            var bibEntryBuilder = new StringBuilder();

            var refId = "";
            var title = "";
            var authors = new StringBuilder();
            var year = "";
            var venue = "";
            var volume = "";
            var issn = "";
            var pages = "";
            var otherIds = "";

            if (bibEntry.Children()["ref_id"].Count() > 0)
            {
                refId = bibEntry.Children()["ref_id"].First().ToString();
            }
            AppendLine(bibEntryBuilder, "Reference Id:", refId);
            if (bibEntry.Children()["title"].Count() > 0)
            {
                title = bibEntry.Children()["title"].First().ToString();
            }
            AppendLine(bibEntryBuilder, "Title:", title);

            if (bibEntry?.Children()["authors"]?.Children() != null && bibEntry.Children()["authors"].Children().Count() > 0)
            {
                foreach (var author in bibEntry.Children()["authors"])
                {
                    var bibRefAuthorFirstName = author[0]["first"].ToString();
                    if (!String.IsNullOrWhiteSpace(bibRefAuthorFirstName))
                    {
                        AppendLine(authors, "", bibRefAuthorFirstName);
                    }

                    var bibRefAuthorMiddleName = "";
                    if (author[0]["middle"].Children().Count() > 0)
                    {
                        bibRefAuthorMiddleName = author[0]["middle"].First().ToString();
                    }
                    if (!String.IsNullOrWhiteSpace(bibRefAuthorMiddleName))
                    {
                        AppendLine(authors, "", bibRefAuthorMiddleName);
                    }

                    var bibRefAuthorSurname = "";
                    if (author[0]["last"].ToString().Length > 0)
                    {
                        bibRefAuthorSurname = author[0]["last"].ToString();
                    }
                    if (!String.IsNullOrWhiteSpace(bibRefAuthorSurname))
                    {
                        AppendLine(authors, "", bibRefAuthorSurname);
                    }
                }
            }

            AppendLine(bibEntryBuilder, "Authors:", authors.ToString());

            if (bibEntry.Children()["year"].Count() > 0)
            {
                year = bibEntry.Children()["year"].First().ToString();
            }
            AppendLine(bibEntryBuilder, "Year:", year);

            if (bibEntry.Children()["venue"].Count() > 0)
            {
                venue = bibEntry.Children()["venue"].First().ToString();
            }
            AppendLine(bibEntryBuilder, "Venue:", venue);

            if (bibEntry.Children()["volume"].Count() > 0)
            {
                volume = bibEntry.Children()["volume"].First().ToString();
            }
            AppendLine(bibEntryBuilder, "Volume:", volume);

            if (bibEntry.Children()["issn"].Count() > 0)
            {
                issn = bibEntry.Children()["issn"].First().ToString();
            }
            AppendLine(bibEntryBuilder, "Issn:", issn);

            if (bibEntry.Children()["pages"].Count() > 0)
            {
                pages = bibEntry.Children()["pages"].First().ToString();
            }
            AppendLine(bibEntryBuilder, "Pages:", pages);

            if (bibEntry.Children()["otherIds"].Count() > 0)
            {
                otherIds = bibEntry.Children()["otherIds"].First().ToString();
            }
            AppendLine(bibEntryBuilder, "Other Ids:", otherIds);

            return bibEntryBuilder.ToString();
        }

        private static string GetAuthor(JToken author)
        {
            var firstName = author["first"].ToString();
            var surname = "";
            if (author["middle"].Children().Count() > 0)
            {
                surname = author["middle"].First().ToString();
            }
            if (String.IsNullOrEmpty(surname) && author["last"].ToString().Length > 0)
            {
                surname = author["last"].ToString();
            }
            var laboratory = "";
            var institution = "";
            var addrLine = "";
            var postCode = "";
            var settlement = "";
            var country = "";
            var email = "";

            if (author["affiliation"] != null)
            {
                laboratory = author["affiliation"]["laboratory"] != null ? author["affiliation"]["laboratory"].ToString() : "";
                institution = author["affiliation"]["institution"] != null ? author["affiliation"]["institution"].ToString() : "".ToString();
                if (author["affiliation"]["location"] != null)
                {
                    addrLine = author["affiliation"]["location"]["addrLine"] != null ? author["affiliation"]["location"]["addrLine"].ToString() : "";
                    postCode = author["affiliation"]["location"]["postCode"] != null ? author["affiliation"]["location"]["postCode"].ToString() : "";
                    settlement = author["affiliation"]["location"]["settlement"] != null ? author["affiliation"]["location"]["settlement"].ToString() : "";
                    country = author["affiliation"]["location"]["country"] != null ? author["affiliation"]["location"]["country"].ToString() : "";
                }
                email = author["email"].ToString();
            }

            if (author["email"] != null)
            {
                email = author["email"].ToString();
            }

            var authorLine = new StringBuilder();
            AppendLine(authorLine, "", firstName);
            AppendLine(authorLine, "", surname);
            AppendLine(authorLine, "", laboratory);
            AppendLine(authorLine, "", institution);
            AppendLine(authorLine, "", addrLine);
            AppendLine(authorLine, "", postCode);
            AppendLine(authorLine, "", settlement);
            AppendLine(authorLine, "", country);
            AppendLine(authorLine, "Email: ", email);

            return authorLine.ToString();
        }

        #endregion Private Methods
    }
}
