using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageScaler
{
    public class ImageProcessor
    {
        private readonly bool _dry;
        private readonly string _here;
        private readonly ImageScalerService _scalerService;

        public ImageProcessor(bool dry)
        {
            _dry = dry;
            _scalerService = new ImageScalerService();
            _here = Directory.GetCurrentDirectory();
        }

        public void Process(FileInfo image, string outPath, string prefix, string suffix, ImageFormat format)
        {
            LoggingService.WriteLine("Processing {0}", image.Name);
            var outFileName = string.Format("{0}{1}{2}", prefix, Path.GetFileNameWithoutExtension(image.FullName), suffix);
            var rootDir = string.IsNullOrWhiteSpace(outPath) ? Path.GetDirectoryName(_here) : new DirectoryInfo(outPath).FullName;
            var extension = format == null ? image.Extension : string.Format(".{0}", format.ToString().ToLower());
            LoggingService.WriteVerbose("New File Name: {0}{1}", outFileName, extension);


            int defaultHeight;
            int defaultWidth;

            using (var img = Image.FromFile(image.FullName))
            {
                defaultHeight = img.Height;
                defaultWidth = img.Width;
            }

            LoggingService.WriteVerbose("Original Dimensions: {0}W by {1}H", defaultWidth, defaultHeight);

            var expectedImages = ImageHelper.BuildImageInfo(rootDir, outFileName, extension);

            DirectoryHelper.SetupPaths(expectedImages, _dry);

            var codec = ImageHelper.GetImageCodecInfo(format);

            foreach (var expectedImage in expectedImages)
            {
                var path = expectedImage.Value.Item1;
                var scale = expectedImage.Value.Item2;
                var scaledWidth = (int) Math.Ceiling(defaultWidth/scale);
                var scaledHeight = (int) Math.Ceiling(defaultHeight/scale);
                var scaledImage = _scalerService.Scale(scaledWidth, scaledHeight, image.FullName);
                scaledImage.Save(path, codec, _dry);

                LoggingService.WriteVerbose("New Dimensions for {0}: {1}W by {2}H", expectedImage.Key, scaledWidth, scaledHeight);
            }
            LoggingService.WriteLine();
        }
    }
}