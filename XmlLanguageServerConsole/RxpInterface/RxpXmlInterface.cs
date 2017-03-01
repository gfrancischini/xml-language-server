using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RxpInterface.Utility;

namespace RxpInterface
{
    public class RxpXmlInterface
    {
        /// <summary>
        /// 
        /// </summary>
        private StringBuilder errorData;

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder outputData;

        /// <summary>
        /// 
        /// </summary>
        public string ErrorData
        {
            get
            {
                return errorData.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OutputData
        {
            get
            {
                return outputData.ToString();
            }
        }

        public List<RxpError> Errors
        {
            get;
            private set;
        }

        private string RxpExecutablePath
        {
            get;
            set;
        }

        public RxpXmlInterface()
        {
            string currentLocation = Path.GetDirectoryName(typeof(RxpInterface.RxpXmlInterface).GetTypeInfo().Assembly.Location);
            this.RxpExecutablePath = Path.Combine(currentLocation, @"rxp1.0.exe");

            if (File.Exists(this.RxpExecutablePath) == false)
            {
                ResourceUtils.ExtractResource("RxpInterface.Resources.rxp1.0.exe", this.RxpExecutablePath);
            }
        }

        /// <summary>
        /// Parse the rcvd message and return 
        /// </summary>
        /// <param name="message"></param>
        public void ParseMessage(string message)
        {
            string regexExpression = @"Error:(.*) at line (\d+) char (\d+) of (.*)";

            Regex regex = new Regex(regexExpression, RegexOptions.IgnoreCase);
            foreach (Match match in regex.Matches(message))
            {
                if (match.Success)
                {
                    //0 - mensagem inteira
                    //1 - descricao do erro
                    //2 - linha do erro
                    //3 - coluna do erro
                    //4 - arquivo do erro

                    string description = match.Groups[1].Value;
                    int line = Convert.ToInt32(match.Groups[2].Value);
                    int column = Convert.ToInt32(match.Groups[3].Value);
                    string path = match.Groups[4].Value;

                    this.Errors.Add(new RxpError()
                    {
                        Description = description,
                        Line = line,
                        Column = column,
                        Path = path
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Run(string filePath)
        {
            return this.RunRxp(filePath);
        }


        protected bool RunRxp(string filePath)
        {
            this.errorData = new StringBuilder();
            this.outputData = new StringBuilder();
            this.Errors = new List<RxpError>();

            var process = new Process();


            string command = @"/C " + this.RxpExecutablePath + " \"file://" + filePath.Replace("\\", "/") + "\"";
            var startinfo = new ProcessStartInfo("cmd.exe", command);
            startinfo.RedirectStandardOutput = true;
            startinfo.RedirectStandardError = true;
            startinfo.UseShellExecute = false;
            startinfo.CreateNoWindow = true;
            //startinfo.WorkingDirectory = ProjectInfo.AppsConfiguration.EnvironmentSrc;
            process.StartInfo = startinfo;
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += delegate (object sender, System.Diagnostics.DataReceivedEventArgs args)
            {
                if (args.Data != null)
                {
                    errorData.Append(args.Data);
                }
            };
            process.OutputDataReceived += delegate (object sender, System.Diagnostics.DataReceivedEventArgs args)
            {
                if (args.Data != null)
                {
                    outputData.Append(args.Data);
                }
            };


            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                ParseMessage(this.ErrorData);
                return false;
            }
            return true;
        }
    }
}
