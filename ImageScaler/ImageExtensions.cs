using System.Drawing;
using System.Drawing.Imaging;

namespace ImageScaler
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Extension method to help save out images. If codec is null, it'll save as original
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="path">Path to save it to</param>
        /// <param name="codec">Codec to save as (can be null).</param>
        public static void Save(this Image image, string path, ImageCodecInfo codec)
        {
            if (codec == null)
            {
                // Saves as original
                image.Save(path);
            }
            else
            {
                // Saves with customized codec
                image.Save(path, codec, null);
            }
        }
    }
}