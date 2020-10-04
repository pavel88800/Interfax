using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using BL;
using Timer = System.Timers.Timer;

namespace LogServiceApp
{
    public partial class Service : ServiceBase
    {
        private Timer _aTimer;
        private LoggingFile _logger;

        public Service()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            CallWrite();

            //var b = ConfigurationManager.AppSettings.Get("Key0");
            //EventLog.WriteEntry("LogServiceAppInfo", GetMemorySize().ToString(), EventLogEntryType.SuccessAudit);

            _logger = new LoggingFile();
            var loggerThread = new Thread(_logger.Start);
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            _logger.Stop();
            Thread.Sleep(1000);
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
            _aTimer.Elapsed += WriteInDb;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }

        /// <summary>
        ///     Запись в БД
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static async void WriteInDb(object source, ElapsedEventArgs e)
        {
            var loggingDb = new LoggingDb();
            await loggingDb.WriteDateAsync(DateTime.Now, GetMemorySize());
        }

        /// <summary>
        ///     Запись в Файл
        /// </summary>
        private void WriteInFile()
        {
        }
    }
}