using System;
using System.Collections.Generic;
using System.IO;
using DB.Models;

namespace BL
{
    public class LoggingFile
    {
        /// <summary>
        ///     Записать данные в лог.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="mSize"></param>
        public void WriteData(DateTime dt, long mSize)
        {
            using (var writer = new StreamWriter("D:\\Logs\\templog.txt", true))
            {
                writer.WriteLine("{0}  {1} ", dt.ToString("dd/MM/yyyy hh:mm:ss"), mSize.ToString());
                writer.Flush();
            }
        }

        /// <summary>
        ///     Получить данные из лога.
        /// </summary>
        /// <returns></returns>
        public List<Log> GetData()
        {
            var logList = new List<Log>();
            using (var sr = new StreamReader("D:\\Logs\\templog.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var s = line.Split(new[] {"  "}, StringSplitOptions.None);
                    var dt = DateTime.Parse(s[0]);
                    var mem = long.Parse(s[1]);
                    logList.Add(new Log
                    {
                        Date = dt,
                        MemorySize = mem
                    });
                }
            }

            return logList;
        }
    }
}