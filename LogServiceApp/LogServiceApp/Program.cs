using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace LogServiceApp
{
    internal static class Program
    {
        /// <summary>
        ///     Главная точка входа для приложения.
        /// </summary>
        private static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Service()
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LogServiceApp", ex.ToString(), EventLogEntryType.Error);
            }
        }
    }
}