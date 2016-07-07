using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bing;
using System.Data.SQLite;
using System.Data.Entity;

namespace DrumpfReportUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new BingSearchContainer(
    new Uri("https://api.datamarket.azure.com/Bing/search/")
    );
          //  client.Credentials = new NetworkCredential("accountKey", "5/397uT61db2oYMqva78buLleEq9XbhFhz9iuOX+c5U");
          ////  var news = client.Composite("News", "Trump", null, null, "en-US", null, null, null, null, null, null, null, null, null, "Date").Execute();
          //  var marketData = client.News("Trump", null, "en-US", "Moderate", null, null, null, null, "Date").Execute();
            
          // // var Newslist = news.ToList()[0].News;
          //  foreach (var item in marketData )
          //  {
          //      Console.WriteLine(item);
          //  }
          //  Console.ReadLine();

            DbContext context = new DbContext("Data Source=DAL/DrumpfArticles.db;Version=3;");
     
            
           
        }


    }
}
