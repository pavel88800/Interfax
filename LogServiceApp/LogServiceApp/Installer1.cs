using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace LogServiceApp
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        private readonly ServiceProcessInstaller _processInstaller;
        private readonly ServiceInstaller _serviceInstaller;

        public Installer1()
        {
            InitializeComponent();
            _serviceInstaller = new ServiceInstaller();
            _processInstaller = new ServiceProcessInstaller();

            _processInstaller.Account = ServiceAccount.LocalSystem;
            _serviceInstaller.StartType = ServiceStartMode.Manual;
            _serviceInstaller.ServiceName = "Service";
            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }
    }
}