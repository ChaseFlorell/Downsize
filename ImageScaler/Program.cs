using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using NDesk.Options;

namespace ImageScaler
{
    internal class Program
    {
        public const string AppName = "ImageScaler";

        private static void Main(string[] args)
        {
            Console.WriteLine();
            var helpRequested = false;
            var images = new List<FileInfo>();
            var outPath = string.Empty;
            var prefix = string.Empty;
            var suffix = string.Empty;
            ImageFormat format = null;

            var options = new OptionSet
            {
                {
                    "img|image=", "Image to resize \nnote: this MUST be your largest version (xxhdpi/@3x)",
                    v => images.Add(new FileInfo(v))
                },
                {
                    "format=", "Output format (png, jpg).",
                    v => format = ImageHelper.ParseImageFormat(v)
                },
                {
                    "out|outpath=", "Path to save out the image\nnote: if left empty, we'll overwrite.",
                    v => outPath = v
                },
                {
                    "pre|prefix=", "Prefix to prepend to the image name.",
                    v => prefix = v
                },
                {
                    "suf|suffix=", "Suffix to append to the image name.",
                    v => suffix = v
                },
                {
                    "?|h|help", "show this message and exit",
                    v => helpRequested = v != null
                }
            };
            try
            {
                options.Parse(args);
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

            var processor = new Processor();
            foreach (var image in images)
            {
                processor.Process(image, outPath, prefix, suffix, format);
            }
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