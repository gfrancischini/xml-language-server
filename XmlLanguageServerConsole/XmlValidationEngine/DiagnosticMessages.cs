using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmlValidation
{
    class DiagnosticMessages
    {
        public static string SolutionCanHaveOnlyOneProjectOfType()
        {
            return "The solution can have only one project of this type.";
        }

        public static string InvalidFileTypeException(string filePath)
        {
            return String.Format("Invalid file type. (File: {0})", filePath);
        }

        public static string InvalidFolderLocationException(string folder)
        {
            return String.Format("Cannot create folder \"{0}\" in this location.", folder);
        }

        public static string InvalidFileTypeInFolderException(string folderName, string fileType, string filePath)
        {
            return String.Format("The {0} folder does not accept files of type \"{1}\". (File: {2})", folderName, fileType, filePath);
        }

        public static string StyleFileWithWrongName(string path)
        {
            return String.Format("The {0} file is a Style xml, but its name is invalid. It must be either MainStyle.xml, AndroidMainStyle.xml or iOSMainStyle.xml.", path);
        }

        public static string XmlFileOfUnknownTypeForServerConfigProject(string path)
        {
            return String.Format("The content of file '{0}' is not recognizable as a Form or Cube Definition.", path);
        }

        public static string XmlFileOfUnknownTypeForAppProject(string path)
        {
            return String.Format("The content of file '{0}' is not recognizable as either Style, Format or UI Element.", path);
        }

        public static string EnvironmentUrlRequired()
        {
            return "Environment location (URL) is required.";
        }

        public static string ModuleNameIsRequired()
        {
            return "Module Name is required.";
        }

        public static string ModuleNameAlreadyExists()
        {
            return "Module Name already exists.";
        }

        public static string UserLoginRequired()
        {
            return "User login is required.";
        }

        public static string UserPasswordRequired()
        {
            return "User password is required.";
        }

        public static string AppCodeRequired()
        {
            return "App Project Release Code is required.";
        }

        public static string EnvironmentUrlInvalid()
        {
            return "Environment location url is wrong or has an invalid format.";
        }

        public static string XmlAttributeLengthExceeded(string value, string attributeName, int maxLength)
        {
            return String.Format("The value '{0}' of the {1} attribute value has exceeded the maximum size of {2} characters.", value, attributeName, maxLength);
        }

        public static string XmlAttributeRangeOutside(string value, string attrName, int minValue, int maxValue)
        {
            return String.Format("The value '{0}' of the {1} attribute value is outside the valid range ({2} to {3}).", value, attrName, minValue, maxValue);
        }

        public static string XmlAttributeMustBeInteger(string attrName)
        {
            return String.Format("The {0} attribute must be integer.", attrName);
        }

        public static string XmlReadingError(string error)
        {
            return String.Format("{0}", error);
        }

        public static string XmlAttributeDuplicated(string elementName, string codeVal)
        {
            return String.Format("{0} element has attribute {1} duplicated with another {0}.", elementName, codeVal);
        }

        public static string FileNameMaxLengthExceeded(int maxLength)
        {
            return String.Format("Deploy error - Filename length must be {0} characters or less.", maxLength);
        }

        public static string FileContentLengthExceeded(int maxLength)
        {
            return String.Format("Deploy error - File content cannot exceed {0} characters.", maxLength);
        }

        public static string VersionNameRequired()
        {
            return "Version name is required.";
        }

        public static string FileNotFound(string path)
        {
            return string.Format("File {0} not found.", path);
        }

        public static string ErrorWhileLoadingFile(string file)
        {
            return string.Format("An error occurred while loading the file {0}", file);
        }

        public static string ConfirmationForProjectRemovalFromSolution()
        {
            return "This action will remove the project from the solution. Do you want to continue?";
        }

        public static string ConfirmationForModuleDelete()
        {
            return "This action will permanently delete the module and all its contained files. Do you want to continue?";
        }

        public static string ConfirmationForModuleRemoval()
        {
            return "This action will remove the module from the project. Do you want to continue?";
        }

        public static string ConfirmationForFileDelete()
        {
            return "This action will permanently delete the file. Do you want to continue?";
        }

        public static string ConfirmationForFileRemoval()
        {
            return "This action will remove the file from the project. Do you want to continue?";
        }

        public static string CannotSaveSinceFileIsReadOnly(string path)
        {
            return string.Format("The file {0} is read-only and cannot be saved. If it is under source control, perform check-out first and try again.", path);
        }

        public static string CannotDownloadXsdSinceFolderIsReadOnly(string folder)
        {
            return string.Format("Cannot download XSD files since the target folder is read-only. ({0})", folder);
        }

        public static string NoConnectionToWebService()
        {
            return "There is no connection to the Apps Studio webservice.";
        }

        public static string ActionInterruptedBecauseOfUnsavedFiles()
        {
            return "Action interrupted since there are unsaved files.";
        }

        public static string DeployInterruptedBecauseOfValidationErrors()
        {
            return "Deploy interrupted because of validation errors.";
        }

        public static string DeployInterruptedBecauseOfDownloadXsdError()
        {
            return "Deploy interrupted because of errors while downloading XSDs.";
        }

        public static string SelectModuleFolder()
        {
            return "Select the root folder for the module you want to add.";
        }

        public static string ModuleFolderMustBeBelowModulesFolder()
        {
            return "The module folder must reside below the \"Modules\" folder of the app project.\n\n" +
                "If the module you are trying to add resides in another location, first copy it through Windows Explorer to below the \"Modules\" folder, the try again the command.";
        }

        public static string FilesIgnoredWhileAddingModule(string ignoredFiles)
        {
            return string.Format("One or more files were ignored while adding the module:\n\n{0}", ignoredFiles);
        }

        public static string FileAlreadyExistsAtLocation(string targetPath)
        {
            return string.Format("File already exists at location:\n\n{0}", targetPath);
        }

        public static string CloseAllDocumentsFirstBeforeRenamingRemoveOrDelete()
        {
            return "Close all documents first before attempting a rename, remove or delete.";
        }

        public static string RenameOrDeleteFolderUnderSourceControlIsNotSupported()
        {
            return "Renaming or deleting a non-empty folder in a project under source control is not supported. You must:\n\n" +
                "- Perform the rename/delete directly in the source control;\n" +
                "- Edit the project file outside Apps Studio and rename/delete the folder references;\n" +
                "- Reload the project in Apps Studio.";
        }

        public static string OneOrMoreFilesUnderTheFolderAreReadOnly()
        {
            return "Cannot perform this operation since one or more files contained in this folder are read-only.";
        }

        public static string ProjectIsReadOnly()
        {
            return "Cannot perform this operation since the project file is read-only.";
        }

        public static string FileIsReadOnly(string fileName)
        {
            return string.Format("Cannot perform this operation since the file {0} is read-only.", fileName);
        }

        public static string SolutionIsReadOnly()
        {
            return "Cannot perform this operation since the solution file is read-only.";
        }

        public static string XsdFilesInSolutionCanBeDeleted()
        {
            return
                "One or more XSD files are included in the project. For a better experience, particularly when multiple developers work in the same project, it is strongly recommended that you remove those files from the project and from the source control.\n\n" +
                "Apps Studio will continue to download the XSD files as before, but they will not be included in the project. You can view these files by right-clicking the XSD folder and selecting \"Open folder in Windows Explorer\".";
        }

        public static string CannotDeleteXsdFileSinceItIsReadOnly(string fileName)
        {
            return string.Format("Cannot delete XSD file {0} since it is read-only.", fileName);
        }

        public static string CannotCreateServerProjectSinceTheAppProjectIsOldStructure()
        {
            return "Cannot create a Server Project since the solution is in the old format. Please open a ticket to support requesting structure migration, attaching the entire solution as a zip file.";
        }

        public static string XsdPathIsRequired()
        {
            return "XSD path is required.";
        }

        public static string EnvSettingsInSolutionCanBeDeleted()
        {
            return "Apps Studio has detected that the Environment_Settings.xml file is included in the project. For a better experience, particularly when multiple developers work in the same project, it is strongly recommended that you remove this file from the project and from the source control.";
        }

        public static string EnvSettingsIsReadOnly()
        {
            return "The Environment_Settings.xml file is read-only. If it is in source control, delete the file from the source control and try again.";
        }

        public static string NoReleasedAppWithName(string applicationCode)
        {
            return string.Format("There is no released app project named \"{0}\".", applicationCode);
        }

        public static string ThereIsAlreadyReleasedVersionWithName(string versionName)
        {
            return string.Format("There is already a released version named \"{0}\" for this app.", versionName);
        }

        public static string FolderExistsOnDiskWithSameNameAsModule()
        {
            return "A folder already exists on disk with this name.";
        }

        public static string DownloadXsdFirstBeforeAddingItems()
        {
            return "You must download XSDs from an environment first before adding items.";
        }

        public static string DeployErrorFileNameIsDuplicated(string filePath)
        {
            return string.Format("Deploy error - Name of file \"{0}\" is duplicated with another file.", filePath);
        }

        public static string DeployErrorGeneric(string message)
        {
            return string.Format("Deploy error - {0}.", message);
        }

        public static string DeployErrorXmlCodeDifferentThanFileName(string fileName)
        {
            return string.Format("Deploy error - XML file \"{0}\" has code attribute different than file name.", fileName);
        }

        public static string DeployErrorCodeAttributeNotValidJsIdentifier(string fileName)
        {
            return string.Format("Deploy error - Code attribute of file \"{0}\" is not a valid Javascript identifier.", fileName);
        }

        public static string CannotRenameSinceXmlIsInvalid()
        {
            return "Cannot rename the file since the XML is invalid. Fix any errors before trying again.";
        }

        public static string NoXSDSchemaDefined()
        {
            return "There is no xsd schema defined.";
        }

        public static string ErrorWhileExporting(Exception exception)
        {
            return string.Format("Error while exporting - {0}", exception.Message);
        }

        public static string SubFileAlreadyExists(string path)
        {
            return string.Format("File {0} already exists. Either delete it from the disk and try again, or add it using the \"Add Existing...\" command.", path);
        }

        public static string FileCannotBeEmpty()
        {
            return "File cannot be empty.";
        }

        public static string WebServiceUnexpectedError()
        {
            return "Unexpected error on WebService. Check the server logs for more information.";
        }

        public static string WebServiceIncompatible(string url, string version)
        {
            return string.Format("The WebService '{0}' is of version {1} and is incompatible with this version of Apps Studio. ", url, version);
        }

        public static string EnvironmentUrlDoesNotExist(string url)
        {
            return string.Format("The URL '{0}' does not exists or could not be contacted at this time.", url);
        }

        public static string XmlDoesNotContainmSeriesDeclaration(string fileName)
        {
            return string.Format("Xml file '{0}' does not contain the <?mSeries schema?> processing instruction.", fileName);
        }

        public static string XmlSchemaDifferentFromProcessingInstruction(string fileName)
        {
            return string.Format("Xml file '{0}' <?mSeries schema?> processing instruction is different than the XML schema declaration.", fileName);
        }

        public static string XmlSchemaDoesNotExist(string xsdFileName)
        {
            return string.Format("The xml schema '{0}' does not exists.", xsdFileName);
        }

        public static string ExternalEditorNotConfigured()
        {
            return "External Editor for this file type is not configured.\n\nYou can configure it in Tools -> Options -> Apps Studio Options -> External Editors.";
        }

        public static string NoItemsSelected()
        {
            return "No items were selected.";
        }

        public static string ApplicationCodeForDeployCannotExceed50Chars()
        {
            return "App Project Deploy Code cannot exceed 50 characters. Choose a smaller code.";
        }

        public static string MobileAppAssociatedToAnotherAppProject(string associatedAppCode, string thisAppCode)
        {
            return string.Format("The selected Mobile App is currently associated to App Project '{0}'.\n\n" +
                                 "If you continue and perform a deploy, the Mobile App will be associated to this App Project ('{1}').\n\n" +
                                 "Do you want to continue?", associatedAppCode, thisAppCode);
        }

        public static string DeployCancelledByUser()
        {
            return "Deploy cancelled by user.";
        }

        public static string MobileAppDoesNotExistsOrIsNotInTesting(string mobileAppName)
        {
            return string.Format("The Mobile App '{0}' does not exists or is not in status 'Testing'. Please choose another Mobile App in the Configure Environment screen.", mobileAppName);
        }

        public static string UserIsNotAssociatedToMobileApp(string mobileAppName, string userLogin)
        {
            return string.Format("User '{0}' is not associated to the Mobile App '{1}'.", userLogin, mobileAppName);
        }

        public static string FormTypeListMustHaveOnlyOneDimensionSslAsIdentifier()
        {
            return "Form Type LIST_FORM must have one, and only one, DimensionSSL field marked with the attribute identifier = true.";
        }

        public static string EnvironmentUrlNotFound()
        {
            return string.Format("The specified URL was not found. Check if the URL is corrrect, and if it has the correct protocol (http or https).");
        }

        public static string TypeScriptCompilationErrors()
        {
            return "TypeScript compilation errors were detected. Check all errors in the TypeScript Errors window.";
        }

        public static string TypeScriptCompiledFileNotFound()
        {
            return "The compiled JavaScript was not found. You must perform the \"TOOLS > Compile TypeScript\" action before attempting to open the compiled file.";
        }

        public static string TypeScriptLogFileNotFound(string file)
        {
            return string.Format(@"The TypeScript file ""{0}"" does not exists in this solution.", file);
        }

        public static string TypeScriptMappingFileNotFound(string file)
        {
            return string.Format(@"The TypeScript mapping file for ""{0}"" was not found. In order to generate this file, a TypeScript compilation must be performed. Do you want to execute it now?", file);
        }

        public static string MobileLogNoEnvironmentFound()
        {
            return "You should configure an environment before attempting to get the logs.";
        }

        public static string MobileApiVersionDoesNotExists(string path)
        {
            return string.Format(@"The Mobile API version ""{0}"" does not exists. Check the App Project properties.", path);
        }

        public static string MobileApiChangedAndUnsavedFilesOpen()
        {
            return "The Mobile API version has been changed and there are unsaved files. If you continue, the files will be saved and reopened in order for the change to be applied.\n\nDo you want to continue?";
        }

        public static string NoMobileLogsAvailable()
        {
            return "No Mobile Logs available.";
        }

        public static string NoTypeScriptTraceAvailable()
        {
            return "No TypeScript StackTrace available.";
        }

        public static string FileMustHaveOneChildren(string projectRelativeFilePath, string childrenTypeFriendlyName)
        {
            return string.Format(@"The file ""{0}"" is required to have one children of type ""{1}""", projectRelativeFilePath, childrenTypeFriendlyName);
        }
    }
}
