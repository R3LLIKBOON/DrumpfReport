using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bing;
using Newtonsoft.Json;

namespace DrumpfReportUpdater
{
    internal class JsonFileManager
    {
        internal List<NewsResult> GetSavedArticlesFromJson(string _jsonFilePath)
        {
            CreateFileIfNotExist(_jsonFilePath);
            return GetArticlesFromFile(_jsonFilePath);
        }

        private List<NewsResult> GetArticlesFromFile(string _jsonFilePath)
        {
            List<NewsResult> SavedArticles = new List<NewsResult>();

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

        private void CreateFileIfNotExist(string _jsonFilePath)
        {
            if (!File.Exists(_jsonFilePath))
            {
                try
                {
                    File.Create(_jsonFilePath).Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Writing File" + ex.Message);
                }
            }
        }
    }
}
