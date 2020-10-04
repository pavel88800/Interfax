using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using BL;
using DB.Models;

namespace LogServiceApp
{
    public partial class Service : ServiceBase
    {
        private Timer _aTimer;
        private string _config = "File";

        public Service()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            CallCheckConfig();
            CallWrite();

            //var b = ConfigurationManager.AppSettings.Get("Key0"); // Получить ключи из конфига
            // EventLog.WriteEntry("LogServiceAppInfo", GetMemorySize().ToString(), EventLogEntryType.SuccessAudit); //запись в лог событий windows
        }

        protected override void OnStop()
        {
            _aTimer.Stop();
        }

        /// <summary>
        ///     Получить кол-во памяти занимаемой процессом
        /// </summary>
        /// <returns>
        ///     <see cref="long" />
        /// </returns>
        private static long GetMemorySize()
        {
            var memoryBite = Process.GetProcessesByName("LogServiceApp")[0].WorkingSet64;
            return memoryBite;
        }

        /// <summary>
        ///     Запуск таймера
        /// </summary>
        private void CallWrite()
        {
            _aTimer = new Timer(5000);
            _aTimer.Elapsed += CallMethods;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }

        /// <summary>
        ///     Запись в БД
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static async Task WriteInDbAsync()
        {
            var loggingDb = new LoggingDb();
            await loggingDb.WriteDateAsync(DateTime.Now, GetMemorySize());
        }

        /// <summary>
        ///     Запись в Файл
        /// </summary>
        private static async Task WriteInFileAsync()
        {
            await Task.Run(() =>
            {
                var loggingFile = new LoggingFile();
                loggingFile.WriteData(DateTime.Now, GetMemorySize());
            });
        }

        /// <summary>
        ///     Получить данные из файла.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static List<Log> Get(object source, ElapsedEventArgs e)
        {
            var loggingFile = new LoggingFile();

            return loggingFile.GetData();
        }

        private async void CallMethods(object source, ElapsedEventArgs e)
        {
            switch (_config)
            {
                case "Database":
                    await WriteInDbAsync();
                    break;
                case "File":
                    await WriteInFileAsync();
                    break;
            }
        }

        private void CallCheckConfig()
        {
            _aTimer = new Timer(10000);
            _aTimer.Elapsed += (source, _event) =>
            {
                _config = ConfigurationManager.AppSettings.Get("WRITE");


                EventLog.WriteEntry("ConfigApp", _config, EventLogEntryType.SuccessAudit);
            };
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }
    }
}