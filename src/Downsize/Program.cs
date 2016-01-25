using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using NDesk.Options;

namespace Downsize
{
    internal class Program
    {
        private static string _appName;
        public static string AppName
        {
            get { return _appName ?? (_appName = Assembly.GetExecutingAssembly().GetName().Name); }
        }

        private static void Main(string[] args)
        {
            var helpRequested = false;
            var log = false;
            var verbose = false;
            var quiet = false;
            var images = new List<FileInfo>();
            var outPath = AppDomain.CurrentDomain.BaseDirectory;
            var prefix = string.Empty;
            var suffix = string.Empty;
            ImageFormat format = null;
            var dry = false;

            var options = new OptionSet
            {
                {
                    "i|img|image=", "Image to resize \nnote: this MUST be your largest version (xxhdpi/@3x)",
                    v => images.Add(new FileInfo(v))
                },
                {
                    "f|format=", "Output format (png, jpg)",
                    v => format = ImageHelper.ParseImageFormat(v)
                },
                {
                    "o|out|outpath=", "Path to save out the image\ndefaults to current directory of downsize.exe",
                    v => outPath = v
                },
                {
                    "pre|prefix=", "Prefix to prepend to the image name",
                    v => prefix = v
                },
                {
                    "suf|suffix=", "Suffix to append to the image name",
                    v => suffix = v
                },
                {
                    "l|log", "write a log file to the output directory",
                    v => log = v != null
                },
                {
                    "s|q|silent|quiet", "Don't write out to console",
                    v => quiet = v != null
                },
                {
                    "d|dry|dryrun", "Summary of what would happen",
                    v => dry = v != null
                },
                {
                    "v|verbose", "Write verbose information",
                    v => verbose = v != null
                },
                {
                    "?|h|help", "show help message and exit",
                    v => helpRequested = v != null
                }
            };

            try
            {
                var p = options.Parse(args);
                images.AddRange(p.Select(unParsed => new FileInfo(unParsed)));
            }
            catch (OptionException e)
            {
                Console.Write("{0}: ", AppName);
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", AppName);
                Console.WriteLine();
                return;
            }

            if (helpRequested)
            {
                ShowHelp(options);
                return;
            }

            LoggingService.Init(outPath, log, verbose, quiet);
            LoggingService.WriteLine("Begin Processing Images!");
            LoggingService.WriteVerbose("Writing files to {0}", outPath);

            var processor = new ImageProcessor(dry);
            foreach (var image in images)
            {
                try
                {
                    processor.Process(image, outPath, prefix, suffix, format);
                }
                catch (Exception ex)
                {
                    LoggingService.WriteError("An error occurred while processing {0}", image);
                    LoggingService.WriteError(ex.Message);
                }
            }
            LoggingService.WriteLine("Finished Processing Images!");
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine();
            Console.WriteLine("Help:\n");
            Console.WriteLine("Usage: {0} [OPTIONS]", AppName);
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}