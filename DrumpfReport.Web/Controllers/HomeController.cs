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
#if(DEBUG)//TODO Find out how to de condition debg
         // savedArticlePath = "C:\\Users\\zzdia\\Documents\\visual studio 2015\\Projects\\DrumpfReportUpdater\\DrumpfReportUpdater\\bin\\Debug\\CurrentDrumpfArticles.json";
#endif   
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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}