using System;
using System.Reflection;
using System.IO;
using System.Xml.Linq;

namespace Attuned
{
    /// <summary>
    /// Log file writing class
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Log file save directory
        /// </summary>
        public static string LogFileDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory
                    + AppDomain.CurrentDomain.RelativeSearchPath;
            }
        }

        /// <summary>
        /// The name of the application calling this function
        /// </summary>
        private static string ApplicationName
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Name;
            }
        }

        /// <summary>
        /// Generate the name that will be used for the file from the current date
        /// </summary>
        /// <param name="suffix"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetDateName(string suffix, string extension)
        {
            return DateTime.Now.ToString("yyyy_MM_dd") + suffix + extension;
        }

        /// <summary>
        /// Write text to the log file
        /// </summary>
        /// <param name="message"></param>
        public static void Write(string message)
        {
            string filename = LogFileDirectory + GetDateName("_LOG", ".log");

            // create the following structure using XML:
            //  logEntry
            //      Machine - The name of the machine
            //      User - The logged-in user's username
            //      Application - The name of the application that called the log writing function
            //      Date - The Current Date and Time
            //      Exception
            //          Source - who threw the exception (usually "System")
            //          Message - the message given with the exception
            //          Stack - where the error occured
            //          InnerException - Error that caused the error
            //              Source
            //              Message
            //              Stack
            XElement xmlEntry = new XElement(
                    "logEntry",
                    new XElement("Machine", Environment.MachineName),
                    new XElement("User", Environment.UserName),
                    new XElement("Application", ApplicationName),
                    new XElement("Date", DateTime.Now.ToString()),
                    new XElement("Message", message));
            // protect writing code with try
            try
            {
                StreamWriter sw = new StreamWriter(filename, true);
                sw.WriteLine(xmlEntry);
                sw.Close();
            }
            catch { }
        }

        /// <summary>
        /// Write exception to log file
        /// </summary>
        /// <param name="ex"></param>
        public static void Write(Exception ex)
        {
            
            string filename = LogFileDirectory + GetDateName("_LOG", ".log");

            // create the following structure using XML:
            //  logEntry
            //      Machine - The name of the machine
            //      User - The logged-in user's username
            //      Application - The name of the application that called the log writing function
            //      Date - The Current Date and Time
            //      Exception
            //          Source - who threw the exception (usually "System")
            //          Message - the message given with the exception
            //          Stack - where the error occured
            //          InnerException - Error that caused the error
            //              Source
            //              Message
            //              Stack

            XElement xmlEntry = new XElement("logEntry",
                    new XElement("Machine", Environment.MachineName),
                    new XElement("User", Environment.UserName),
                    new XElement("Application", ApplicationName),
                    new XElement("Date", DateTime.Now.ToString()),
                    new XElement(
                        "Exception",
                        new XElement("Source", ex.Source),
                        new XElement("Message", ex.Message),
                        new XElement("Stack", ex.StackTrace)
                     )
                );
            // has an inner exception?
            if (ex.InnerException != null)
            {
                xmlEntry.Element("Exception").Add(
                    new XElement(
                        "InnerException",
                        new XElement("Source", ex.InnerException.Source),
                        new XElement("Message", ex.InnerException.Message),
                        new XElement("Stack", ex.InnerException.StackTrace))
                    );
            }

            // protect writing code with try
            try
            {
                StreamWriter sw = new StreamWriter(filename, true);
                sw.WriteLine(xmlEntry);
                sw.Close();
            }
            catch { }
        }
    }
}
