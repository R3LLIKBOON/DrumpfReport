using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bing;
using Newtonsoft.Json;


namespace DrumpfReportUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            string savedArticlePath = "d:\\home\\site\\wwwroot\\CurrentDrumpfArticles.js";

            #if DEBUG
                 savedArticlePath = "CurrentDrumpfArticles.js";
            #endif

            List<NewsResult> newArticles = GetNewArticles();

            Console.WriteLine("New Articles");
            Console.WriteLine("------------------------------------------------");

            foreach (var article in newArticles)
            {
                Console.WriteLine(article.Title);
                article.Title = article.Title.Replace("Trump", "Drumpf");
                article.Title = article.Title.Replace("\"", "&quot");
            }
            List<NewsResult> SavedArticles = GetSavedArticlesFromJson(savedArticlePath);

            foreach (var item in newArticles)
            {
                if ((SavedArticles.Where(p => p.Title == item.Title).FirstOrDefault() == null))
                {
                    SavedArticles.Add(item);
                }
            }

            SavedArticles = SavedArticles.OrderByDescending(p => p.Date).ToList();

            int desiredArticleCount = 40;
            int savedArticleCount = (SavedArticles.Count < desiredArticleCount) ? SavedArticles.Count : desiredArticleCount;

            SavedArticles = SavedArticles.GetRange(0, savedArticleCount);

            DisplayArticles(SavedArticles);
            SaveJsonObjectToFile(SavedArticles, savedArticlePath);
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void SaveJsonObjectToFile(List<NewsResult> _savedArticles, string _jsonFilePath)
        {
            try
            {
                string json = "var jsonArticles = { \"Articles\" :" + JsonConvert.SerializeObject(_savedArticles) + "};";
                File.WriteAllText(_jsonFilePath, json);
                Console.WriteLine("jsonArticle file saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static void DisplayArticles(List<NewsResult> _savedArticles)
        {//TODO Convert to for ?
            int i = 0;
            Console.WriteLine("Saved Articles");
            Console.WriteLine("------------------------------------------------");
            foreach (var item in _savedArticles)
            {
                i++;
                Console.WriteLine(i + " - " + item.Title + "   :" + item.Date.ToString());
            }
        }

        private static List<NewsResult> GetSavedArticlesFromJson(string _jsonFilePath)
        {
            List<NewsResult> SavedArticles = new List<NewsResult>();
            //TODO Refactor
            if (!File.Exists(_jsonFilePath))
            {
                try
                {
                    File.Create(_jsonFilePath).Close();
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Writing File" + ex.Message);
                    throw;
                }

            }

            using (StreamReader streamReader = new StreamReader(_jsonFilePath))
            {
                try
                {
                    string json = streamReader.ReadToEnd();
                    json = json.Replace("var jsonArticles = { \"Articles\" :", "");
                    json = json.Replace("};", "");
                    SavedArticles = (List<NewsResult>)JsonConvert.DeserializeObject<List<NewsResult>>(json);
                    Console.WriteLine("Save File Read");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error" + ex.Message);
                    streamReader.Close();
                }
            }
            return SavedArticles;
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
