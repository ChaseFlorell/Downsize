using System;
using System.IO;

namespace Downsize
{
    public static class LoggingService
    {
        private static bool _logToFile;
        private static bool _verbose;
        private static string _logFilePath;
        private static bool _quiet;

        public static void Init(string logDir, bool logToFile, bool verbose, bool quiet)
        {
            _logToFile = logToFile;
            _verbose = verbose;
            _quiet = quiet;

            if (!logToFile) return;

            var timeStamp = DateTime.Now.ToString("yyMMdd");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            _logFilePath = string.Format("{0}\\Log-{1}.log", logDir, timeStamp);
        }

        public static void WriteLine(string message = "")
        {
            if (!_quiet)
            {
                Console.WriteLine(message);
            }
            WriteLog(string.Format("NORMAL: \t{0}", message));
        }

        public static void WriteError(string message = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (!_quiet)
            {
                Console.WriteLine(message);
            }
            Console.ResetColor();
            WriteLog(string.Format("ERROR: \t{0}", message));
        }

        public static void WriteVerbose(string message = "")
        {
            if (!_verbose) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (!_quiet)
            {
                Console.WriteLine(message);
            }
            Console.ResetColor();
            WriteLog(string.Format("VERBOSE: \t{0}", message));
        }

        public static void WriteLine(string messageFormat, params object[] arg)
        {
            var message = string.Format(messageFormat, arg);
            WriteLine(message);
        }

        public static void WriteError(string messageFormat, params object[] arg)
        {
            var message = string.Format(messageFormat, arg);
            WriteError(message);
        }


        public static void WriteVerbose(string messageFormat, params object[] arg)
        {
            var message = string.Format(messageFormat, arg);
            WriteVerbose(message);
        }

        private static void WriteLog(string message)
        {
            if (!_logToFile || string.IsNullOrWhiteSpace(message) || message.Trim().ToLower() == "normal:") return;

            using (var file = File.Open(_logFilePath, FileMode.Append))
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine("{0}\t{1}", DateTime.Now, message);
            }
        }
    }
}