using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadZip
{
    public static class LogHelper
    {
        private static log4net.ILog _errorLoger;
        private static log4net.ILog _debugLoger;

        private static log4net.ILog ErrorLoger
        {
            get
            {
                if (_errorLoger == null)
                    _errorLoger = log4net.LogManager.GetLogger("Error");
                return _errorLoger;
            }

        }

        private static log4net.ILog DebugLoger
        {
            get
            {
                if (_debugLoger == null)
                    _debugLoger = log4net.LogManager.GetLogger("Debug");
                return _debugLoger;
            }

        }

        private static string ReplaceSpecialChars(string originalStr)
        {
            if (string.IsNullOrEmpty(originalStr))
                return originalStr;
            return originalStr.Replace("\u00A5", "&#x00A5;");
        }
              
        /// <summary>
        /// log an exception.
        /// </summary>
        /// <param name="ex">exception</param>
        public static void Exception(Exception ex)
        {
            ErrorLoger.Fatal(ex.Message, ex);
        }

        /// <summary>
        /// log an exception with a message
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="ex">exception</param>
        public static void Exception(string message, Exception ex)
        {
            message = ReplaceSpecialChars(message);
            ErrorLoger.Fatal(message, ex);
        }

        /// <summary>
        /// log a message
        /// </summary>
        /// <param name="message">message</param>
        public static void Debug(string message)
        {
            message = ReplaceSpecialChars(message);
            DebugLoger.Debug(message);
        }

        /// <summary>
        /// log a message
        /// </summary>
        /// <param name="message">message</param>
        public static void Debug(int message)
        {
            //message = ReplaceSpecialChars(message);
            DebugLoger.Debug(message);
        }

        /// <summary>
        /// log an exception with a message
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="ex">ex</param>
        public static void Debug(string message, Exception ex)
        {
            message = ReplaceSpecialChars(message);
            DebugLoger.Debug(message, ex);
        }
    }
}
