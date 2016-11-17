using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bing;
using DrumpfReport.DAL;

namespace DrumpfReport.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string savedArticlePath = "d:\\home\\site\\wwwroot\\CurrentDrumpfArticles.json";

            if (ControllerContext.HttpContext.IsDebuggingEnabled)
            {
                savedArticlePath = @"C:\Users\Dave\Source\Repos\DrumpfReport\DrumpfReportUpdater\bin\Debug\CurrentDrumpfArticles.json";
            }

            List<NewsResult> articles = new List<NewsResult>();
            JsonFileManager jsonFileManager = new JsonFileManager();
            try
            {
                articles = jsonFileManager.GetSavedArticlesFromJson(savedArticlePath);
            }
            catch (Exception)
            {
                throw;
            }
            return View(articles);
        }

        public ActionResult About()
        {
            ViewBag.Message = "The Drumpf Report provide the latest news on Donald Drumpf.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}