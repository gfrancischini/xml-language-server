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
            //if debug helps to attach debugger when debugging vscode
#if DEBUG
            while (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Threading.Thread.Sleep(100);
            }
#endif

            try
            {
                Logger.Initialize("xmllanguageserver", minimumLogLevel: LogLevel.Verbose, isEnabled: true);
                Logger.Write(LogLevel.Normal, "Starting Xml Language Service Host");


                // Grab the instance of the service host
                LanguageServiceHost languageServiceHost = LanguageServiceHost.Instance;

                // Start the service
                languageServiceHost.Start().Wait();

                // Initialize the services that will be hosted here
                WorkspaceService.Instance.InitializeService(languageServiceHost);
                XmlLanguageService.Instance.InitializeService(languageServiceHost);

                languageServiceHost.Initialize();
                languageServiceHost.WaitForExit();
            }
            catch (AggregateException e)
            {
                LogAggregateException(e);
                throw;
            }
            catch (Exception e)
            {
                Logger.Write(LogLevel.Error, e.Message + " - " + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Log Agregate Exception so we dont miss an exception on the log file
        /// </summary>
        /// <param name="e"></param>
        static void LogAggregateException(AggregateException e)
        {
            Logger.Write(LogLevel.Error, e.Message + " - " + e.StackTrace);
            foreach (Exception inner in e.InnerExceptions)
            {
                if (inner is AggregateException)
                {
                    LogAggregateException((AggregateException)inner);
                }
                else
                {
                    Logger.Write(LogLevel.Error, inner.Message + " - " + inner.StackTrace);
                }
            }

        }
    }
}