using HostingAdapter.Hosting;
using HostingAdapter.Utility;
using LanguageServerProtocol.Hosting.Contracts.Document;
using LanguageServerProtocol.Hosting.Contracts.General;
using LanguageServerProtocol.Hosting.Protocol;
using LanguageServerProtocol.Hosting.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XmlCompletionEngine;
using XmlCompletionEngine.Types;
using XmlLanguageServerConsole.Workspace;
using XmlLanguageServerConsole.Workspace.Contracts;
using XmlValidation;

namespace XmlLanguageServerConsole.LanguageService
{
    /// <summary>
    /// Main class for Language Service functionality including anything that requires knowledge of
    /// the language to perform, such as definitions, intellisense, etc.
    /// </summary>
    public class XmlLanguageService
    {
        public static readonly string[] CompletionTriggerCharacters = new string[] { "<", " ", "\\" };

        private static CancellationTokenSource ExistingRequestCancellation { get; set; }
        internal const int DiagnosticParseDelay = 750;

        #region Singleton Instance Implementation

        private static readonly Lazy<XmlLanguageService> instance = new Lazy<XmlLanguageService>(() => new XmlLanguageService());

        /// <summary>
        /// Gets the singleton instance object
        /// </summary>
        public static XmlLanguageService Instance
        {
            get { return instance.Value; }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the Language Service instance
        /// </summary>
        /// <param name="serviceHost"></param>
        /// <param name="context"></param>
        public void InitializeService(LanguageServiceHost serviceHost)
        {


            // Register the requests that this service will handle
            //serviceHost.SetRequestHandler(SignatureHelpRequest.Type, HandleSignatureHelpRequest);
            //serviceHost.SetRequestHandler(CompletionResolveRequest.Type, HandleCompletionResolveRequest);
            //serviceHost.SetRequestHandler(HoverRequest.Type, HandleHoverRequest);
            serviceHost.SetRequestHandler(CompletionRequest.Type, HandleCompletionRequest);
            serviceHost.SetRequestHandler(DefinitionRequest.Type, HandleDefinitionRequest);


            serviceHost.RegisterInitializeTask(this.InitializeCallback);


            // Register the file change update handler
            WorkspaceService.Instance.RegisterTextDocChangeCallback(HandleDidChangeTextDocumentNotification);

            // Register the file open update handler
            WorkspaceService.Instance.RegisterTextDocOpenCallback(HandleDidOpenTextDocumentNotification);


            // Register a no-op shutdown task for validation of the shutdown logic
            /*serviceHost.RegisterShutdownTask(async (shutdownParams, shutdownRequestContext) =>
            {
                Logger.Write(LogLevel.Verbose, "Shutting down language service");
                DeletePeekDefinitionScripts();
                await Task.FromResult(0);
            });

            // Register the configuration update handler
            WorkspaceService<SqlToolsSettings>.Instance.RegisterConfigChangeCallback(HandleDidChangeConfigurationNotification);

            // Register the file change update handler
            WorkspaceService<SqlToolsSettings>.Instance.RegisterTextDocChangeCallback(HandleDidChangeTextDocumentNotification);

            // Register the file open update handler
            WorkspaceService<SqlToolsSettings>.Instance.RegisterTextDocOpenCallback(HandleDidOpenTextDocumentNotification);

            // Register a callback for when a connection is created
            ConnectionServiceInstance.RegisterOnConnectionTask(UpdateLanguageServiceOnConnection);

            // Register a callback for when a connection is closed
            ConnectionServiceInstance.RegisterOnDisconnectTask(RemoveAutoCompleteCacheUriReference);

            // Store the SqlToolsContext for future use
            Context = context;*/

        }

        private async Task InitializeCallback(InitializeRequest startupParams, RequestContext<InitializeResult> requestContext)
        {
            // Send back what this server can do
            await requestContext.SendResult(
                    new InitializeResult
                    {
                        Capabilities = new ServerCapabilities
                        {
                            TextDocumentSync = TextDocumentSyncKind.Incremental,
                            DefinitionProvider = true,
                            ReferencesProvider = false,
                            DocumentHighlightProvider = false,
                            HoverProvider = false,
                            CompletionProvider = new CompletionOptions()
                            {
                                ResolveProvider = true,
                                TriggerCharacters = CompletionTriggerCharacters.ToList()
                            },

                        }
                    });
        }


        #endregion

        #region Instance Creators
        protected virtual XmlSymbolDefinitionProvider CreateSymbolDefinitionProvider(Text text, TextPosition textPosition)
        {
            return new XmlSymbolDefinitionProvider(text, textPosition);
        }

        protected virtual XmlCodeCompletionProvider CreateCodeCompletionProvider(Text text, TextPosition textPosition)
        {
            return new XmlCodeCompletionProvider(text, textPosition);
        }

        #endregion

        /// <summary>
        /// Auto-complete completion provider request callback
        /// </summary>
        /// <param name="textDocumentPosition"></param>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        internal async Task HandleCompletionRequest(TextDocumentPosition textDocumentPosition, RequestContext<CompletionList> requestContext)
        {
            CompletionList completionList = new CompletionList();
            completionList.IsIncomplete = false;

            Text text = this.CreateXmlTextFromTextDocumentUri(textDocumentPosition.TextDocument.Uri);

            TextPosition textPosition = PositionUtils.CreateTextPosition(text.Content, textDocumentPosition.Position);

            XmlCodeCompletionProvider xmlCodeCompletion = this.CreateCodeCompletionProvider(text, textPosition);
            XmlCompletionItemCollection xmlCompletionItemCollection = xmlCodeCompletion.RetrieveCompletionItemCollection();
            if (xmlCompletionItemCollection != null)
            {

                foreach (XmlCompletionItem xmlCompletionItem in xmlCompletionItemCollection)
                {
                    completionList.Items.Add(new CompletionItem()
                    {
                        Detail = null, //"This is the documentation. Not Implemented",
                        Documentation = xmlCompletionItem.Documentation,
                        Kind = CompletionItem.CompletionItemKind.Text,
                        Label = xmlCompletionItem.Text,
                    });
                }
            }
            await requestContext.SendResult(completionList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textDocumentPosition"></param>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        internal async Task HandleDefinitionRequest(TextDocumentPosition textDocumentPosition, RequestContext<Location[]> requestContext)
        {
            List<Location> locations = new List<Location>();

            Text text = this.CreateXmlTextFromTextDocumentUri(textDocumentPosition.TextDocument.Uri);

            TextPosition textPosition = PositionUtils.CreateTextPosition(text.Content, textDocumentPosition.Position);

            XmlSymbolDefinitionProvider xmlSymbolDefinition = this.CreateSymbolDefinitionProvider(text, textPosition);
            XmlObjectLocationCollection xmlObjectLocationCollection = xmlSymbolDefinition.RetrieveSymbolLocation();
            if (xmlObjectLocationCollection != null)
            {
                foreach (XmlObjectLocation xmlObjectLocation in xmlObjectLocationCollection)
                {
                    locations.Add(new Location()
                    {
                        Range = new Range()
                        {
                            Start = new LanguageServerProtocol.Hosting.Types.Position()
                            {
                                Line = xmlObjectLocation.LineNumber - 1,
                                Character = xmlObjectLocation.LinePosition - 1
                            },
                            End = new LanguageServerProtocol.Hosting.Types.Position()
                            {
                                Line = xmlObjectLocation.LineNumber - 1,
                                Character = xmlObjectLocation.LinePosition - 1
                            }
                        },
                        Uri = xmlObjectLocation.Uri.ToString(),
                    });
                }

            }
            await requestContext.SendResult(locations.ToArray());
        }

        /// <summary>
        /// Create the Text Xml Interface class based on the uri and position provided
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected Text CreateXmlTextFromTextDocumentUri(Uri uri)
        {
            ScriptFile scriptFile = WorkspaceService.Instance.Workspace.GetFile(
                uri);

            Text text = new Text(scriptFile.Contents, uri);
            return text;
            //return null;
        }


      

        /// <summary>
        /// Runs script diagnostics on changed files
        /// </summary>
        /// <param name="filesToAnalyze"></param>
        /// <param name="eventContext"></param>
        private Task RunScriptDiagnostics(ScriptFile[] filesToAnalyze, EventContext eventContext)
        {
            // If there's an existing task, attempt to cancel it
            try
            {
                if (ExistingRequestCancellation != null)
                {
                    // Try to cancel the request
                    ExistingRequestCancellation.Cancel();

                    // If cancellation didn't throw an exception,
                    // clean up the existing token
                    ExistingRequestCancellation.Dispose();
                    ExistingRequestCancellation = null;
                }
            }
            catch (Exception e)
            {
                Logger.Write(LogLevel.Error, string.Format("Exception while cancelling analysis task:\n\n{0}", e.ToString()));

                TaskCompletionSource<bool> cancelTask = new TaskCompletionSource<bool>();
                cancelTask.SetCanceled();
                return cancelTask.Task;
            }

            // Create a fresh cancellation token and then start the task.
            // We create this on a different TaskScheduler so that we
            // don't block the main message loop thread.
            ExistingRequestCancellation = new CancellationTokenSource();
            Task.Factory.StartNew(
                () =>
                    DelayThenInvokeDiagnostics(
                        XmlLanguageService.DiagnosticParseDelay,
                        filesToAnalyze,
                        eventContext,
                        ExistingRequestCancellation.Token),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Default);

            return Task.FromResult(true);
        }

        /// <summary>
        /// Actually run the script diagnostics after waiting for some small delay
        /// </summary>
        /// <param name="delayMilliseconds"></param>
        /// <param name="filesToAnalyze"></param>
        /// <param name="eventContext"></param>
        /// <param name="cancellationToken"></param>
        private async Task DelayThenInvokeDiagnostics(
            int delayMilliseconds,
            ScriptFile[] filesToAnalyze,
            EventContext eventContext,
            CancellationToken cancellationToken)
        {
            // First of all, wait for the desired delay period before
            // analyzing the provided list of files
            try
            {
                await Task.Delay(delayMilliseconds, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                // If the task is cancelled, exit directly
                return;
            }

            // If we've made it past the delay period then we don't care
            // about the cancellation token anymore.  This could happen
            // when the user stops typing for long enough that the delay
            // period ends but then starts typing while analysis is going
            // on.  It makes sense to send back the results from the first
            // delay period while the second one is ticking away.

            // Get the requested files
            foreach (ScriptFile scriptFile in filesToAnalyze)
            {
                /*if (IsPreviewWindow(scriptFile))
                {
                    continue;
                }*/

                Logger.Write(LogLevel.Verbose, "Analyzing script file: " + scriptFile.FilePath);
                List<Diagnostic> diagnostics = GetSemanticMarkers(scriptFile);
                Logger.Write(LogLevel.Verbose, "Analysis complete.");

                // Always send syntax and semantic errors.  We want to 
                // make sure no out-of-date markers are being displayed.
                await eventContext.SendEvent(
                    PublishDiagnosticsNotification.Type,
                    new PublishDiagnosticsNotification
                    {
                        Uri = scriptFile.ClientFilePath,
                        Diagnostics = diagnostics.ToArray()
                    });

                //await DiagnosticsHelper.PublishScriptDiagnostics(scriptFile, semanticMarkers, eventContext);
            }
        }

        /// <summary>
        /// Gets a list of semantic diagnostic marks for the provided script file
        /// </summary>
        /// <param name="scriptFile"></param>
        internal List<Diagnostic> GetSemanticMarkers(ScriptFile scriptFile)
        {
            List<Diagnostic> markers = new List<Diagnostic>();
            XmlValidator xmlValidator = new XmlValidator();
            if (xmlValidator.Validate(new Uri(scriptFile.FilePath), scriptFile.Contents) == false)
            {
                foreach (XmlError xmlError in xmlValidator.Errors)
                {
                    markers.Add(new Diagnostic()
                    {
                        Code = xmlError.ErrorCode.ToString(),
                        Message = xmlError.Message,
                        Range = new Range()
                        {
                            Start = new Position()
                            {
                                Character = xmlError.Column,
                                Line = xmlError.Line
                            },
                            End = new Position()
                            {
                                Character = xmlError.Column,
                                Line = xmlError.Line
                            }
                        },
                        Severity = DiagnosticSeverity.Error,
                        Source = "xml"
                    });
                }
            }
            return markers;
        }

        /// <summary>
        /// Handle the file open notification
        /// </summary>
        /// <param name="scriptFile"></param>
        /// <param name="eventContext"></param>
        /// <returns></returns>
        public async Task HandleDidOpenTextDocumentNotification(
            ScriptFile scriptFile,
            EventContext eventContext)
        {

            await RunScriptDiagnostics(
                new ScriptFile[] { scriptFile },
                eventContext);

        }

        /// <summary>
        /// Handles text document change events
        /// </summary>
        /// <param name="textChangeParams"></param>
        /// <param name="eventContext"></param>
        public async Task HandleDidChangeTextDocumentNotification(ScriptFile[] changedFiles, EventContext eventContext)
        {

            await this.RunScriptDiagnostics(
                changedFiles.ToArray(),
                eventContext);

        }
    }
}
