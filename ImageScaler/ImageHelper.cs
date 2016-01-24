using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageScaler
{
    public static class ImageHelper
    {
        public static ImageFormat ParseImageFormat(string format)
        {
            switch (format.Replace(".", ""))
            {
                case "png":
                    return ImageFormat.Png;
                case "jpg":
                    return ImageFormat.Jpeg;
                case "jpeg":
                    return ImageFormat.Jpeg;
                default:
                    return null;
            }
        }

        public static Dictionary<string, Tuple<string, double>> BuildImageInfo(string rootDir, string newName, string extension)
        {
            var section = ImageConfigSection.GetConfigSection();
            IEnumerable<ImageConfigElement> images;
            if (section != null)
            {
                images = from ImageConfigElement i in section.ImageCollection select i;
            }
            else
            {
                images = ImageConfigSection.DefaultCollection();
            }
            var dict = new Dictionary<string, Tuple<string, double>>();
            foreach (var image in images)
            {
                var path = string.Format(image.PathFormat, rootDir, newName) + extension;
                dict.Add(image.Key, new Tuple<string, double>(path, image.Scale));
            }

            return dict;
        }


        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
        {
            if (format == null)
            {
                return null;
            }
            var codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}