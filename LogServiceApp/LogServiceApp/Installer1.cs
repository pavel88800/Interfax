using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace LogServiceApp
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        private readonly ServiceProcessInstaller processInstaller;

        private readonly ServiceInstaller serviceInstaller;

        public Installer1()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "Service";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}