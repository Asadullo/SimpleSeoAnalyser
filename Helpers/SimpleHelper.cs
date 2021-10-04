using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleSEOAnalyser.Helpers
{
	public class SimpleHelper
	{
		public static bool IsValidURI(string URI)
		{
			Uri uriResult;
			bool result = Uri.TryCreate(URI, UriKind.Absolute, out uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
			return result;
		}
        public static int CalcExternalLinks(string text)
        {
            MatchCollection matchCollection = Regex.Matches(text, @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)");
            return matchCollection.Count; 
        }
        public static async Task<string> GetTextFromURL(string url)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Simple SEO Analyser");

                    var content = await client.GetStringAsync(url);

                    return FilterTextFromHTML(content);
                }
                catch (Exception ex) {
                    throw ex;
                }
            }

        }

        public static string FilterTextFromHTML(string HTMLCode)
        {
            // Remove new lines since they are not visible in HTML
            HTMLCode = HTMLCode.Replace("\n", " ");

            // Remove tab spaces
            HTMLCode = HTMLCode.Replace("\t", " ");

            // Remove multiple white spaces from HTML
            HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

            // Remove HEAD tag
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove any JavaScript
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
                 , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Replace special characters like &, <, >, " etc.
            StringBuilder sbHTML = new StringBuilder(HTMLCode);
            // Note: There are many more special characters, these are just
            // most common. You can add new characters in this arrays if needed
            string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;",
   "&gt;", "&reg;", "&copy;", "&bull;", "&trade;"};
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }

            // Check if there are line breaks (<br>) or paragraph (<p>)
            sbHTML.Replace("<br>", "\n<br>");
            sbHTML.Replace("<br ", "\n<br ");
            sbHTML.Replace("<p ", "\n<p ");

            // Finally, remove all HTML tags and return plain text
            return Regex.Replace(sbHTML.ToString(), "<[^>]*>", "");
        
    }

        public static Dictionary<string, int> CalcWordOccurences(string text)
        {
            string[] words = text.Split(' ');
            return words.Where(o => !string.IsNullOrEmpty(o)).GroupBy(o => o)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public static string FilterStopWords(string text)
        {            
            IList<string> stopWords = new List<string>()
                {
                    "or","a", "and", "the"
                };

            string[] words = text.Split(' ');
            return string.Join(" ",
                words.Where(o => !string.IsNullOrEmpty(o) && !stopWords.Contains(o)));
        }
    }
}