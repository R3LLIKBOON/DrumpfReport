using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bing;
using DrumpfReport.DAL;
using Newtonsoft.Json;


namespace DrumpfReport.Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            string savedArticlePath = "d:\\home\\site\\wwwroot\\CurrentDrumpfArticles.json";
#if DEBUG
            savedArticlePath = "CurrentDrumpfArticles.json";
#endif
            JsonFileManager jsonFileManger = new JsonFileManager();
            List<NewsResult> savedArticles = new List<NewsResult>();
            List<NewsResult> newArticles = GetNewArticles();

            ReplaceTrumpText(newArticles);
            DisplayNewArticles(newArticles);

            try
            {
                savedArticles = jsonFileManger.GetSavedArticlesFromJson(savedArticlePath);
                Console.WriteLine("Save File Read");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
            }

            savedArticles =  AddNewArticles(newArticles, savedArticles);
            //TODO Move to JsonFileManager?
            savedArticles = TrimArticles(savedArticles);
            DisplayArticles(savedArticles);

            try
            {
                jsonFileManger.SaveJsonObjectToFile(savedArticles, savedArticlePath);
                Console.WriteLine("jsonArticle file saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
          
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static List<NewsResult> TrimArticles(List<NewsResult> _savedArticles)
        {
            List<NewsResult> savedArticles = _savedArticles;
            int desiredArticleCount = 40;
            int savedArticleCount = (_savedArticles.Count < desiredArticleCount) ? _savedArticles.Count : desiredArticleCount;

            savedArticles = savedArticles.OrderByDescending(p => p.Date).ToList();

            return savedArticles.GetRange(0, savedArticleCount);
        }

        private static List<NewsResult> AddNewArticles(List<NewsResult> _newArticles, List<NewsResult> _savedArticles)
        {
            List<NewsResult> savedArticles = _savedArticles;

            foreach (var item in _newArticles)
            {
                if ((_savedArticles.Where(p => p.Title == item.Title).FirstOrDefault() == null))
                {
                    savedArticles.Add(item);
                }
            }
            return savedArticles;
        }

        private static void DisplayNewArticles(List<NewsResult> _newArticles)
        {
            Console.WriteLine("New Articles");
            Console.WriteLine("------------------------------------------------");
            foreach (var article in _newArticles)
            {
                Console.WriteLine(article.Title);
            }
        }

        private static void ReplaceTrumpText(List<NewsResult> _newArticles)
        {
            foreach (var article in _newArticles)
            {
                article.Title = article.Title.Replace("Trump", "Drumpf");
            }
        }


        private static void DisplayArticles(List<NewsResult> _savedArticles)
        {
            Console.WriteLine("Saved Articles");
            Console.WriteLine("------------------------------------------------");

            for (int i = 0; i < _savedArticles.Count; i++)
            {
                var article = _savedArticles[i];
                Console.WriteLine((i + 1) + " - " + article.Title + "   :" + article.Date.ToString());
            }
        }

        private static List<NewsResult> GetNewArticles()
        {
            var client = new BingSearchContainer(
              new Uri("https://api.datamarket.azure.com/Bing/search/")
              );

            client.Credentials = new NetworkCredential("accountKey", "5/397uT61db2oYMqva78buLleEq9XbhFhz9iuOX+c5U");
            return client.News("Trump", null, "en-US", "Moderate", null, null, null, null, "Date").Execute().ToList();
        }
    }
}
