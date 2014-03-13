using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using IncinerateService.Utils;
using IncinerateService.Core;
using System.ServiceModel;
using System.ServiceModel.Description;
using IncinerateService.API;
using System.ServiceModel.Channels;
using NLog;

namespace IncinerateService
{
    public class Service : ServiceBase
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        const string ServiceID = "Incinerator";
        Thread m_WorkerThread;
        bool m_Disposed = false;
        bool m_IsService = false;
        ManualResetEvent m_ExitSignal = new ManualResetEvent(false);
        MainService m_MainService = new MainService();

        bool IsExited
        {
            get { return m_ExitSignal.WaitOne(0); }
        }

        public Service()
        {
            InitializeComponent();
        }

        public static void Main(string[] argv)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            using (Service application = new Service())
            {
                CommandLineArgs args = new CommandLineArgs(false, argv);
                if (args.NoArgs)
                {
                    application.RunAsConsoleApp();
                }
                else
                {
                    if (args.Help)
                    {
                        args.PrintUsage();
                    }
                    else if (args.Service && !args.Install && !args.Uninstall)
                    {
                        application.RunAsService();
                    }
                    else if (args.Install && !args.Uninstall && !args.Service)
                    {
                        Logger.Info("Installing service...");
                        int errorCode = Service.InstallService();
                        if (errorCode == 0)
                            Logger.Info(ServiceID + " service successfully installed");
                        else
                            Logger.Info(ServiceID + " service installation failed (error {0})", errorCode);
                    }
                    else if (args.Uninstall && !args.Install && !args.Service)
                    {
                        Logger.Info("Uninstalling service...");
                        int errorCode = Service.UninstallService();
                        if (errorCode == 0)
                            Logger.Info(ServiceID + " service successfully uninstalled");
                        else
                            Logger.Info(ServiceID + " service uninstall failed (error {0})", errorCode);
                    }
                    else
                    {
                        Logger.Info("Invalid command line arguments");
                        args.PrintUsage();
                    }
                }
            }
        }

        static int InstallService()
        {
            IntPtr databaseHandle = IntPtr.Zero;
            IntPtr serviceHandle = IntPtr.Zero;
            int lastError = 0;

            try
            {
                databaseHandle = Advapi32Dll.OpenSCManager(null, null, Advapi32Dll.ServiceManagerFullAccess);
                lastError = Marshal.GetLastWin32Error();
                if (lastError != 0 || databaseHandle == IntPtr.Zero)
                    return lastError;

                serviceHandle = Advapi32Dll.CreateService(
                    databaseHandle,
                    ServiceID,
                    ServiceID,
                    Advapi32Dll.ServiceAllAccess,
                    Advapi32Dll.ServiceOwnProcess,
                    ServiceStartType.Demand,
                    Advapi32Dll.ServiceErrorControlNormal,
                    Assembly.GetEntryAssembly().Location + " -s",
                    null, null, null, null, null);
                lastError = Marshal.GetLastWin32Error();
                if (lastError != 0 || serviceHandle == IntPtr.Zero)
                    return lastError;

                return 0;
            }
            finally
            {
                Advapi32Dll.CloseServiceHandle(serviceHandle);
                Advapi32Dll.CloseServiceHandle(databaseHandle);
            }
        }

        static int UninstallService()
        {
            IntPtr databaseHandle = IntPtr.Zero;
            IntPtr serviceHandle = IntPtr.Zero;
            int lastError = 0;

            try
            {
                databaseHandle = Advapi32Dll.OpenSCManager(null, null, Advapi32Dll.ServiceManagerFullAccess);
                lastError = Marshal.GetLastWin32Error();
                if (lastError != 0 || databaseHandle == IntPtr.Zero)
                    return lastError;

                serviceHandle = Advapi32Dll.OpenService(databaseHandle, ServiceID, Advapi32Dll.ServiceAllAccess);
                lastError = Marshal.GetLastWin32Error();
                if (lastError != 0 || serviceHandle == IntPtr.Zero)
                    return lastError;

                bool deleted = Advapi32Dll.DeleteService(serviceHandle);
                lastError = Marshal.GetLastWin32Error();
                if (lastError != 0 || !deleted)
                    return lastError;

                return 0;
            }
            finally
            {
                Advapi32Dll.CloseServiceHandle(serviceHandle);
                Advapi32Dll.CloseServiceHandle(databaseHandle);
            }
        }

        void RunAsService()
        {
            m_IsService = true;
            ServiceBase.Run(this);
        }

        void RunAsConsoleApp()
        {
            Console.TreatControlCAsInput = true;
            Console.Title = ServiceID;
            Logger.Info("Running as console application. Press Enter to exit");
            OnStart(null);
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;
            StopApplication();
        }

        void RunApplication()
        {
            using (Mutex mutex = new Mutex(false, @"Global\Incinerator.Application.Mutex"))
            {
                if (mutex.WaitOne(0))
                {
                    try
                    {
                        using (MainService service = new MainService())
                        {
                            NamedPipeTransportBindingElement transport = new NamedPipeTransportBindingElement();
                            transport.ConnectionPoolSettings.IdleTimeout = TimeSpan.MaxValue;
                            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), transport);
                            binding.OpenTimeout = TimeSpan.FromSeconds(3.0);
                            binding.ReceiveTimeout = TimeSpan.MaxValue;
                            binding.SendTimeout = TimeSpan.FromSeconds(6.0);
                            binding.CloseTimeout = TimeSpan.FromSeconds(3.0);

                            ServiceHost host = new ServiceHost(service);
                            host.AddServiceEndpoint(typeof(IIncinerateService), binding, "net.pipe://IncinerateService/incinerate");
                            host.Open();
                            Logger.Info("Server Started");
                            m_ExitSignal.WaitOne();
                            host.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e);
                        if (m_IsService)
                            Stop();
                    }
                    finally
                    {
                        // dispose all objects
                    }
                }
                else
                {
                    Logger.Info("Cannot run multiple instances of " + ServiceID);
                }
            }
        }

        void StopApplication()
        {
            if (IsExited)
                return;

            lock (typeof(Service))
            {
                // stop all jobs

                m_ExitSignal.Set();
            }
        }

        protected override void OnStart(string[] args)
        {
            m_WorkerThread = new Thread(RunApplication);
            m_WorkerThread.Start();
        }

        protected override void OnStop()
        {
            StopApplication();
        }

        protected override void OnShutdown()
        {
            StopApplication();
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    // disposing

                    m_ExitSignal.Close();
                }
                base.Dispose(disposing);
                m_Disposed = true;
            }
        }

        private void InitializeComponent()
        {
            this.CanShutdown = true;
            this.CanStop = true;
            this.ServiceName = ServiceID;
            this.CanStop = true;
            this.AutoLog = false;
            this.EventLog.Log = "Application";
            this.EventLog.Source = ServiceID;
        }
    }
}
