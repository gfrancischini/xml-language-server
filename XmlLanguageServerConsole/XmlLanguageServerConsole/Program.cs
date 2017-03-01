using HostingAdapter.Utility;
using LanguageServerProtocol.Hosting.Protocol;
using System;
using XmlLanguageServerConsole.LanguageService;
using XmlLanguageServerConsole.Workspace;
using XmlValidation;

namespace XmlLanguageServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {

#if DEBUG
            //Console.WriteLine("Waiting for Visual Studio debugger to attach");
            while (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Threading.Thread.Sleep(100);
            }
            //Console.Error.WriteLine("Visual Studio Debugger attached");
#endif

            // turn on Verbose logging during early development
            // we need to switch to Normal when preparing for public preview
            Logger.Initialize("xmllanguageserver", minimumLogLevel: LogLevel.Verbose, isEnabled: true);
            Logger.Write(LogLevel.Normal, "Starting Xml Language Service Host");


            // Grab the instance of the service host
            LanguageServiceHost languageServiceHost = LanguageServiceHost.Instance;

            // Start the service
            languageServiceHost.Start().Wait();

            // Initialize the services that will be hosted here
            WorkspaceService.Instance.InitializeService(languageServiceHost);
            XmlLanguageService.Instance.InitializeService(languageServiceHost);
            ///ConnectionService.Instance.InitializeService(serviceHost);
            //CredentialService.Instance.InitializeService(serviceHost);
            // QueryExecutionService.Instance.InitializeService(serviceHost);

            languageServiceHost.Initialize();
            languageServiceHost.WaitForExit();
        }
    }
}