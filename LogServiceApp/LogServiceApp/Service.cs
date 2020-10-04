using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using BL;

namespace LogServiceApp
{
    public partial class Service : ServiceBase
    {
        private Timer _aTimer;
        private string _config = "File";
        private ServiceHost _serviceHost;

        public Service()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            if (_serviceHost == null)
            {
                _serviceHost = new ServiceHost(typeof(TestService));
                _serviceHost.Open();
            }

            CallCheckConfig();
            CallWrite();
        }

        protected override void OnStop()
        {
            _serviceHost?.Close();
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
        ///     Вызвать метод в зависимости от конфика
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///     Запустить проверку конфига
        /// </summary>
        private void CallCheckConfig()
        {
            _aTimer = new Timer(10000);
            _aTimer.Elapsed += (source, _event) => { _config = ConfigurationManager.AppSettings.Get("WRITE"); };
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }
    }
}