using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageScaler
{
    public class Processor
    {
        private readonly string _here;
        private readonly ImageScaler _scaler;

        public Processor()
        {
            _scaler = new ImageScaler();
            _here = Directory.GetCurrentDirectory();
        }

        public void Process(FileInfo image, string outPath, string prefix, string suffix, ImageFormat format)
        {
            var newName = string.Format("{0}{1}{2}", prefix, Path.GetFileNameWithoutExtension(image.FullName), suffix);
            var rootDir = string.IsNullOrWhiteSpace(outPath) ? Path.GetDirectoryName(_here) : new DirectoryInfo(outPath).FullName;
            var extension = format == null ? image.Extension : string.Format(".{0}", format.ToString().ToLower());
            int defaultHeight;
            int defaultWidth;

            using (var img = Image.FromFile(image.FullName))
            {
                defaultHeight = img.Height;
                defaultWidth = img.Width;
            }


            var paths = new[]
            {
                string.Format(@"{0}\Android\ldpi\", rootDir).Replace(@"\\", @"\"),
                string.Format(@"{0}\Android\mdpi\", rootDir).Replace(@"\\", @"\"),
                string.Format(@"{0}\Android\hdpi\", rootDir).Replace(@"\\", @"\"),
                string.Format(@"{0}\Android\xhdpi\", rootDir).Replace(@"\\", @"\"),
                string.Format(@"{0}\Android\xxhdpi\", rootDir).Replace(@"\\", @"\"),
                string.Format(@"{0}\iOS\", rootDir).Replace(@"\\", @"\")
            };
            SetupPaths(paths);

            var ldpiPath = string.Format("{0}{1}{2}", paths[0], newName, extension);
            var mdpiPath = string.Format("{0}{1}{2}", paths[1], newName, extension);
            var hdpiPath = string.Format("{0}{1}{2}", paths[2], newName, extension);
            var xhdpiPath = string.Format("{0}{1}{2}", paths[3], newName, extension);
            var xxhdpiPath = string.Format("{0}{1}{2}", paths[4], newName, extension);
            var iosPath = string.Format("{0}{1}{2}", paths[5], newName, extension);
            var ios2Path = string.Format("{0}{1}@2x{2}", paths[5], newName, extension);
            var ios3Path = string.Format("{0}{1}@3x{2}", paths[5], newName, extension);

            var codec = format != null ? GetEncoder(format) : null;

            _scaler.Scale(Scale(defaultHeight, 4), Scale(defaultWidth, 4), image.FullName).Save(ldpiPath, codec);
            _scaler.Scale(Scale(defaultHeight, 3), Scale(defaultWidth, 3), image.FullName).Save(mdpiPath, codec);
            _scaler.Scale(Scale(defaultHeight, 3), Scale(defaultWidth, 3), image.FullName).Save(iosPath, codec);
            _scaler.Scale(Scale(defaultHeight, 2), Scale(defaultWidth, 2), image.FullName).Save(hdpiPath, codec);
            _scaler.Scale(Scale(defaultHeight, 1.5), Scale(defaultWidth, 1.5), image.FullName).Save(xhdpiPath, codec);
            _scaler.Scale(Scale(defaultHeight, 1.5), Scale(defaultWidth, 1.5), image.FullName).Save(ios2Path, codec);
            _scaler.Scale(defaultHeight, defaultWidth, image.FullName).Save(xxhdpiPath, codec);
            _scaler.Scale(defaultHeight, defaultWidth, image.FullName).Save(ios3Path, codec);
        }

        private static void SetupPaths(IEnumerable<string> paths)
        {
            foreach (var path in paths.Where(path => !Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static int Scale(int original, double scale)
        {
            return (int)Math.Ceiling(original / scale);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}