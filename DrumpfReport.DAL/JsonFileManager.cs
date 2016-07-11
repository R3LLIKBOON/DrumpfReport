using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bing;
using Newtonsoft.Json;

namespace DrumpfReport.DAL
{
    public class JsonFileManager
    {
        public List<NewsResult> GetSavedArticlesFromJson(string _jsonFilePath)
        {
            CreateFileIfNotExist(_jsonFilePath);
            return GetArticlesFromFile(_jsonFilePath);
        }

        public void SaveJsonObjectToFile(List<NewsResult> _savedArticles, string _jsonFilePath)
        {
            try
            {
                //string json = "var jsonArticles = { \"Articles\" :" + JsonConvert.SerializeObject(_savedArticles) + "};";
                string json =  JsonConvert.SerializeObject(_savedArticles);
                File.WriteAllText(_jsonFilePath, json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<NewsResult> GetArticlesFromFile(string _jsonFilePath)
        {
            List<NewsResult> savedArticles;

            using (StreamReader streamReader = new StreamReader(_jsonFilePath))
            {
                try
                {
                    string json = streamReader.ReadToEnd();
                    savedArticles = (List<NewsResult>)JsonConvert.DeserializeObject<List<NewsResult>>(json);
                }
                catch (Exception)
                {
                    streamReader.Close();
                    throw;
                }
            }
            if (savedArticles == null)
            {
                savedArticles = new List<NewsResult>();
            }
            return savedArticles;
        }

        private void CreateFileIfNotExist(string _jsonFilePath)
        {
            if (!File.Exists(_jsonFilePath))
            {
                try
                {
                    File.Create(_jsonFilePath).Close();

                }
                catch (Exception )
                {
                    throw;
                }
            }
        }
    }
}
