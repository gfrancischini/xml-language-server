using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using RxpInterface;
using XmlValidation.FileTypes;
using System.Xml.Schema;

namespace XmlValidation
{
    public class XmlValidator
    {
        /// <summary>
        /// 
        /// </summary>
        XsdCache xsdCache = new XsdCache();

        /// <summary>
        /// 
        /// </summary>
        private RxpXmlInterface rxpXml = new RxpXmlInterface();

        /// <summary>
        /// 
        /// </summary>
        public List<XmlError> Errors
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileUri"></param>
        /// <returns>true when the file is valid. Check the Errors to get when false to get the errors</returns>
        public bool Validate(Uri fileUri, string content)
        {
            bool result = true;

            this.Errors = new List<XmlError>();

            // validate the syntax
            result = result && this.ValidateSyntax(fileUri.LocalPath);

            // create a new XmlFile
            XmlFile xmlFile = new XmlFile(fileUri.LocalPath, content);

            // validate with xsd
            result = result && this.ValidateAgainstXsd(xmlFile);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        private bool ValidateAgainstXsd(XmlFile xmlFile)
        {
            if (xmlFile.HasDeclaredXsd == false)
            {
                // no xsd validation
                return true;
            }

            if (File.Exists(xmlFile.XsdFilePath) == false)
            {
                // warning no xsd to validate against
                this.Errors.Add(new XmlError()
                {
                    FilePath = xmlFile.FilePath,
                    Column = 0,
                    Line = 0,
                    ErrorType = XmlErrorType.SEMANTIC,
                    ErrorCode = 0,
                    Message = DiagnosticMessages.NoXSDSchemaDefined(),
                    ErrorGravity = XmlErrorGravity.WARNING
                });

                return false;
            }

            try
            {
                XsdFile xsdFile = xsdCache.Get(xmlFile.XsdFilePath);

                XDocument xDocument = XDocument.Parse(xmlFile.Content, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo | LoadOptions.SetBaseUri);

                // Create the XmlSchemaSet class.
                XmlSchemaSet schemaSet = new XmlSchemaSet();

                // Add the schema to the collection.
                schemaSet.Add(null, xsdFile.Path);

                // validate the document
                xDocument.Validate(schemaSet, null);
            }
            catch (XmlSchemaValidationException ex)
            {
                this.Errors.Add(new XmlError()
                {
                    FilePath = xmlFile.FilePath,
                    Line = ex.LineNumber - 1,
                    Column = ex.LinePosition - 1,
                    Message = DiagnosticMessages.XmlReadingError(ex.Message),
                    ErrorType = XmlErrorType.SEMANTIC,
                    ErrorCode = 1,
                    ErrorGravity = XmlErrorGravity.ERROR

                });

                return false;
            }
            catch (Exception ex)
            {
                this.Errors.Add(new XmlError()
                {
                    FilePath = xmlFile.FilePath,
                    Line = 0,
                    Column = 0,
                    Message = DiagnosticMessages.XmlReadingError(ex.Message),
                    ErrorType = XmlErrorType.SEMANTIC,
                    ErrorCode = 2,
                    ErrorGravity = XmlErrorGravity.ERROR
                });
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool ValidateSyntax(String filePath)
        {
            bool result = rxpXml.Run(filePath);
            if (result == false)
            {
                //we got some well fordness errors here... we cannot continue with validation
                //rxpXml.Errors;

                foreach (RxpError error in rxpXml.Errors)
                {
                    this.Errors.Add(new XmlError()
                    {
                        FilePath = error.Path,
                        Line = error.Line - 1,
                        Column = error.Column - 1,
                        Message = error.Description,
                        ErrorType = XmlErrorType.SYNTAX,
                        ErrorCode = 3,
                        ErrorGravity = XmlErrorGravity.ERROR
                    });

                }

                return false;
            }
            return true;
        }
    }
}
