using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;

namespace IncinerateService
{
    [RunInstallerAttribute(true)]
    public class IncinerateServiceInstaller : Installer
    {
        private ServiceProcessInstaller m_ServiceProcessInstaller;
        private ServiceInstaller m_ServiceInstaller;

        public IncinerateServiceInstaller()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.m_ServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.m_ServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // m_ServiceProcessInstaller
            // 
            this.m_ServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.m_ServiceProcessInstaller.Password = "evlikat";
            this.m_ServiceProcessInstaller.Username = "powerup";
            // 
            // m_ServiceInstaller
            // 
            this.m_ServiceInstaller.Description = "Incinerate Service Template";
            this.m_ServiceInstaller.DisplayName = "Incinerate Service";
            this.m_ServiceInstaller.ServiceName = "Incinerate";
            // 
            // IncinerateServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.m_ServiceProcessInstaller,
            this.m_ServiceInstaller});

        }
    }
}
