using System.Drawing.Imaging;

namespace ImageScaler
{
    public static class ImageHelper
    {
        public static ImageFormat ParseImageFormat(string format)
        {
            switch (format.Replace(".", ""))
            {
                case "png": return ImageFormat.Png;
                case "jpg": return ImageFormat.Jpeg;
                case "jpeg": return ImageFormat.Jpeg;
                default: return null;
            }
        }
    }
}