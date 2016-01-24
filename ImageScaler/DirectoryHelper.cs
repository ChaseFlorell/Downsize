using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageScaler
{
    public class DirectoryHelper
    {
        public static void SetupPaths(Dictionary<string, Tuple<string, double>> expectedImages, bool dry)
        {
            foreach (
                var path in
                    expectedImages.Select(meta => Path.GetDirectoryName(meta.Value.Item1)).Where(path => path != null && !Directory.Exists(path)))
            {
                if (dry)
                {
                    LoggingService.WriteVerbose("Would have created directory {0}", path);
                }
                else
                {
                    Directory.CreateDirectory(path);
                    LoggingService.WriteVerbose("Created directory {0}", path);
                }
            }
        }
    }
}