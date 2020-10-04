using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Helpers;
using BL;
using DB.Models;

namespace LogServiceApp
{
    class TestService : IContract
    {
        public async Task<string> GetInfo()
        {
            var writing = ConfigurationManager.AppSettings.Get("WRITE");
            List<Log> res = new List<Log>();
            switch (writing)
            {
                case "Database":
                    res = await GetFromDbAsync();
                    break;
                case "File":
                    res = GetFromFile();
                    break;
            }
            
            return Json.Encode(new Dto { Writing = writing, Logs = res });
        }

        /// <summary>
        ///     Получить данные из файла.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static List<Log> GetFromFile()
        {
            var loggingFile = new LoggingFile();
            return loggingFile.GetData();
        }

        private static async Task<List<Log>> GetFromDbAsync()
        {
            var loggingDb = new LoggingDb();
            return await loggingDb.Get();
        }
    }
}