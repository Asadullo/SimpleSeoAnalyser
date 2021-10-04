using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleSEOAnalyser.Models
{
	public class SimpleModel
	{
        public string InputTxt { get; set; }
        public bool IsFilterStopWords { get; set; } = true;
        public bool IsCalcWordsOnPage { get; set; } = true;
        public bool IsCalcMetaTagWordsOnPage { get; set; } = true;
        public bool IsCalcExternalLinks { get; set; } = true;
        public int CountExternalLinksOnPage { get; set; }
        public Dictionary<string, int> WordCountsOnPage { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> WordCountsInMetaTags { get; set; } = new Dictionary<string, int>();        
        public bool IsValidModel { get; set; }
        public bool Error { get; set; }
    }
}