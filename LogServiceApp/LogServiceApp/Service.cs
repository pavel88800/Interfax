using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace LogServiceApp
{
    public partial class Service : ServiceBase
    {
        private Logger logger;

        public Service()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger();
            var loggerThread = new Thread(logger.Start);
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }
    }

    internal class Logger
    {
        private bool enabled = true;
        private readonly object obj = new object();
        private readonly FileSystemWatcher watcher;

        public Logger()
        {
            watcher = new FileSystemWatcher("D:\\Test");
            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled) Thread.Sleep(1000);
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }

        // переименование файлов
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            var fileEvent = "переименован в " + e.FullPath;
            var filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }

        // изменение файлов
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var fileEvent = "изменен";
            var filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        // создание файлов
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var fileEvent = "создан";
            var filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        // удаление файлов
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var fileEvent = "удален";
            var filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void RecordEntry(string fileEvent, string filePath)
        {
            lock (obj)
            {
                using (var writer = new StreamWriter("D:\\Logs\\templog.txt", true))
                {
                    writer.WriteLine("{0} файл {1} был {2}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath,
                        fileEvent);
                    writer.Flush();
                }
            }
        }
    }
}