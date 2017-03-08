using HostingAdapter.Hosting;
using HostingAdapter.Utility;
using LanguageServerProtocol.Contracts.Document;
using LanguageServerProtocol.Contracts.Document.Types;
using LanguageServerProtocol.Hosting.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlLanguageServerConsole.Workspace.Contracts;

namespace XmlLanguageServerConsole.Workspace
{
    /// <summary>
    /// Class for handling requests/events that deal with the state of the workspace, including the
    /// opening and closing of files, the changing of configuration, etc.
    /// </summary>
    public class WorkspaceService
    {
        #region Singleton Instance Implementation

        private static Lazy<WorkspaceService> instance = new Lazy<WorkspaceService>(() => new WorkspaceService());

        public static WorkspaceService Instance
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Default, parameterless constructor.
        /// </summary>
        public WorkspaceService()
        {
            ConfigChangeCallbacks = new List<ConfigChangeCallback>();
            TextDocChangeCallbacks = new List<TextDocChangeCallback>();
            TextDocOpenCallbacks = new List<TextDocOpenCallback>();
            TextDocCloseCallbacks = new List<TextDocCloseCallback>();
        }

        /// <summary>
        /// Workspace object for the service. Virtual to allow for mocking
        /// </summary>
        public virtual Workspace Workspace { get; internal set; }



        /// <summary>
        /// Delegate for callbacks that occur when the configuration for the workspace changes
        /// </summary>
        /// <param name="newSettings">The settings that were just set</param>
        /// <param name="oldSettings">The settings before they were changed</param>
        /// <param name="eventContext">Context of the event that triggered the callback</param>
        /// <returns></returns>
        public delegate Task ConfigChangeCallback(dynamic newSettings, dynamic oldSettings, EventContext eventContext);

        /// <summary>
        /// Delegate for callbacks that occur when the current text document changes
        /// </summary>
        /// <param name="changedFiles">Array of files that changed</param>
        /// <param name="eventContext">Context of the event raised for the changed files</param>
        public delegate Task TextDocChangeCallback(ScriptFile[] changedFiles, EventContext eventContext);

        /// <summary>
        /// Delegate for callbacks that occur when a text document is opened
        /// </summary>
        /// <param name="openFile">File that was opened</param>
        /// <param name="eventContext">Context of the event raised for the changed files</param>
        public delegate Task TextDocOpenCallback(ScriptFile openFile, EventContext eventContext);

        /// <summary>
        /// Delegate for callbacks that occur when a text document is closed
        /// </summary>
        /// <param name="closedFile">File that was closed</param>
        /// <param name="eventContext">Context of the event raised for changed files</param>
        public delegate Task TextDocCloseCallback(ScriptFile closedFile, EventContext eventContext);

        /// <summary>
        /// List of callbacks to call when the configuration of the workspace changes
        /// </summary>
        private List<ConfigChangeCallback> ConfigChangeCallbacks { get; set; }

        /// <summary>
        /// List of callbacks to call when the current text document changes
        /// </summary>
        private List<TextDocChangeCallback> TextDocChangeCallbacks { get; set; }

        /// <summary>
        /// List of callbacks to call when a text document is opened
        /// </summary>
        private List<TextDocOpenCallback> TextDocOpenCallbacks { get; set; }

        /// <summary>
        /// List of callbacks to call when a text document is closed
        /// </summary>
        private List<TextDocCloseCallback> TextDocCloseCallbacks { get; set; }

        #endregion

        #region Public Methods

        public void InitializeService(LanguageServiceHost serviceHost)
        {
            Logger.Write(LogLevel.Normal, "InitializeService");

            // Create a workspace that will handle state for the session
            Workspace = new Workspace();

            // Register the handlers for when changes to the workspace occur
            serviceHost.SetEventHandler(DidChangeTextDocumentNotification.Type, HandleDidChangeTextDocumentNotification);
            serviceHost.SetEventHandler(DidOpenTextDocumentNotification.Type, HandleDidOpenTextDocumentNotification);
            serviceHost.SetEventHandler(DidCloseTextDocumentNotification.Type, HandleDidCloseTextDocumentNotification);

            // Register an initialization handler that sets the workspace path
            serviceHost.RegisterInitializeTask(async (parameters, contect) =>
            {
                Logger.Write(LogLevel.Verbose, "Initializing workspace service");

                if (Workspace != null)
                {
                    Workspace.WorkspacePath = parameters.RootPath;
                }
                await Task.FromResult(0);
            });

            // Register a shutdown request that disposes the workspace
            serviceHost.RegisterShutdownTask(async (parameters, context) =>
            {
                Logger.Write(LogLevel.Verbose, "Shutting down workspace service");

                if (Workspace != null)
                {
                    Workspace.Dispose();
                    Workspace = null;
                }
                await Task.FromResult(0);
            });
        }

        /// <summary>
        /// Adds a new task to be called when the configuration has been changed. Use this to
        /// handle changing configuration and changing the current configuration.
        /// </summary>
        /// <param name="task">Task to handle the request</param>
        public void RegisterConfigChangeCallback(ConfigChangeCallback task)
        {
            ConfigChangeCallbacks.Add(task);
        }

        /// <summary>
        /// Adds a new task to be called when the text of a document changes.
        /// </summary>
        /// <param name="task">Delegate to call when the document changes</param>
        public void RegisterTextDocChangeCallback(TextDocChangeCallback task)
        {
            TextDocChangeCallbacks.Add(task);
        }

        /// <summary>
        /// Adds a new task to be called when a text document closes.
        /// </summary>
        /// <param name="task">Delegate to call when the document closes</param>
        public void RegisterTextDocCloseCallback(TextDocCloseCallback task)
        {
            TextDocCloseCallbacks.Add(task);
        }

        /// <summary>
        /// Adds a new task to be called when a file is opened
        /// </summary>
        /// <param name="task">Delegate to call when a document is opened</param>
        public void RegisterTextDocOpenCallback(TextDocOpenCallback task)
        {
            TextDocOpenCallbacks.Add(task);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles text document change events
        /// </summary>
        internal Task HandleDidChangeTextDocumentNotification(
            DidChangeTextDocumentParams textChangeParams,
            EventContext eventContext)
        {
            Logger.Write(LogLevel.Verbose, "HandleDidChangeTextDocumentNotification");
            try
            {
                StringBuilder msg = new StringBuilder();
                msg.Append("HandleDidChangeTextDocumentNotification");
                List<ScriptFile> changedFiles = new List<ScriptFile>();

                // A text change notification can batch multiple change requests
                foreach (var textChange in textChangeParams.ContentChanges)
                {
                    msg.AppendLine(string.Format("  File: {0}", textChangeParams.TextDocument.Uri.ToString()));

                    ScriptFile changedFile = Workspace.GetFile(textChangeParams.TextDocument.Uri);

                    changedFile.ApplyChange(
                        GetFileChangeDetails(
                            textChange.Range.Value,
                            textChange.Text));

                    changedFiles.Add(changedFile);
                }

                Logger.Write(LogLevel.Verbose, msg.ToString());

                var handlers = TextDocChangeCallbacks.Select(t => t(changedFiles.ToArray(), eventContext));
                return Task.WhenAll(handlers);
            }
            catch
            {
                // Swallow exceptions here to prevent us from crashing
                // TODO: this probably means the ScriptFile model is in a bad state or out of sync with the actual file; we should recover here
                return Task.FromResult(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openParams"></param>
        /// <param name="eventContext"></param>
        /// <returns></returns>
        internal async Task HandleDidOpenTextDocumentNotification(
            DidOpenTextDocumentNotification openParams,
            EventContext eventContext)
        {
            Logger.Write(LogLevel.Verbose, "HandleDidOpenTextDocumentNotification");

            // read the SQL file contents into the ScriptFile 
            ScriptFile openedFile = Workspace.GetFileBuffer(openParams.TextDocument.Uri, openParams.TextDocument.Text);

            // Propagate the changes to the event handlers
            var textDocOpenTasks = TextDocOpenCallbacks.Select(
                t => t(openedFile, eventContext));

            await Task.WhenAll(textDocOpenTasks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="closeParams"></param>
        /// <param name="eventContext"></param>
        /// <returns></returns>
        internal async Task HandleDidCloseTextDocumentNotification(
           DidCloseTextDocumentParams closeParams,
           EventContext eventContext)
        {
            Logger.Write(LogLevel.Verbose, "HandleDidCloseTextDocumentNotification");

            // Skip closing this file if the file doesn't exist
            var closedFile = Workspace.GetFile(closeParams.TextDocument.Uri);
            if (closedFile == null)
            {
                return;
            }

            // Trash the existing document from our mapping
            Workspace.CloseFile(closedFile);

            // Send out a notification to other services that have subscribed to this event
            var textDocClosedTasks = TextDocCloseCallbacks.Select(t => t(closedFile, eventContext));
            await Task.WhenAll(textDocClosedTasks);
        }
      
        #endregion

        #region Private Helpers

        /// <summary>
        /// Switch from 0-based offsets to 1 based offsets
        /// </summary>
        /// <param name="changeRange"></param>
        /// <param name="insertString"></param>       
        private static FileChange GetFileChangeDetails(Range changeRange, string insertString)
        {
            // The protocol's positions are zero-based so add 1 to all offsets
            return new FileChange
            {
                InsertString = insertString,
                Line = changeRange.Start.Line + 1,
                Offset = changeRange.Start.Character + 1,
                EndLine = changeRange.End.Line + 1,
                EndOffset = changeRange.End.Character + 1
            };
        }

        #endregion
    }
}
