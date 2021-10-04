using SimpleSEOAnalyser.Helpers;
using SimpleSEOAnalyser.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SimpleSEOAnalyser.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			SimpleModel model = new SimpleModel();
			return View(model);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SimpleModel model)
        {   

            model.IsValidModel = ModelState.IsValid;
            String SimpleText;

            if (model.IsValidModel)
            {
                try
                {
                    if (SimpleHelper.IsValidURI(model.InputTxt)) 
                        SimpleText = await SimpleHelper.GetTextFromURL(model.InputTxt);
                    else
                        SimpleText = model.InputTxt;

                    if(model.IsFilterStopWords) SimpleText = SimpleHelper.FilterStopWords(SimpleText);

                    if (model.IsCalcWordsOnPage) model.WordCountsOnPage = SimpleHelper.CalcWordOccurences(SimpleText);

                    if (model.IsCalcExternalLinks) model.CountExternalLinksOnPage = SimpleHelper.CalcExternalLinks(SimpleText);
                }
                catch (Exception ex)
                {
                    model.IsValidModel = false;
                    {
                        ModelState.AddModelError("Error", ex.Message);
                    }
                }
            }

            return View(model);
        }

    }
}