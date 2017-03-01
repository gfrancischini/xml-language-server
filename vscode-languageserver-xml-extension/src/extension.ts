'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import * as path from 'path';
import Constants = require('./constants');
import { spawn, execFile, ChildProcess } from 'child_process';
import { LanguageClient, LanguageClientOptions, StreamInfo, SettingMonitor, ServerOptions, TransportKind } from 'vscode-languageclient';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {

    // Use the console to output diagnostic information (console.log) and errors (console.error)
    // This line of code will only be executed once when your extension is activated
    console.log('Congratulations, your extension "vscode-languageserver-xml-extension" is now active!');

    // The command has been defined in the package.json file
    // Now provide the implementation of the command with  registerCommand
    // The commandId parameter must match the command field in package.json
    let disposable = vscode.commands.registerCommand('extension.sayHello', () => {
        // The code you place here will be executed every time your command is executed

        // Display a message box to the user
        vscode.window.showInformationMessage('Hello World!');
    });

    context.subscriptions.push(disposable);


    openServerCommunication(context);
    
}

// this method is called when your extension is deactivated
export function deactivate() {
}


export function openServerCommunication(context: vscode.ExtensionContext) {
    let serverPath = context.asAbsolutePath(path.join(Constants.LANGUAGE_SERVER_EXE_PATH));

    const serverOptions = (): Promise<ChildProcess | StreamInfo> => {
        // The server is implemented in C#

        try {
            const childProcess = spawn(serverPath);
            childProcess.stderr.on('data', (chunk: Buffer) => {
                console.error(chunk + '');
            });
            childProcess.stdout.on('data', (chunk: Buffer) => {
                console.log(chunk + '');
            });
            return Promise.resolve(childProcess);
        }
        catch (e) {
            vscode.window.showInformationMessage("error starting: " + e);
            return null;
        }
    };

    // Options to control the language client
    let clientOptions: LanguageClientOptions = {
        // Register the server for php documents
        documentSelector: ['xml']
        // synchronize: {
        //     // Synchronize the setting section 'php' to the server
        //     configurationSection: 'php',
        //     // Notify the server about file changes to composer.json files contain in the workspace
        //     fileEvents: workspace.createFileSystemWatcher('**/composer.json')
        // }
    };

    // Create the language client and start the client.
    const disposable = new LanguageClient('XML Language Client', serverOptions, clientOptions).start();

    // Push the disposable to the context's subscriptions so that the
    // client can be deactivated on extension deactivation
    context.subscriptions.push(disposable);
    

}