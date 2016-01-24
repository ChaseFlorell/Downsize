using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Downsize
{
    public class ImageScalerService
    {
        public Image Scale(int newWidth, int newHeight, string stPhotoPath)
        {
            using (var imgPhoto = Image.FromFile(stPhotoPath))
            {
                var sourceWidth = imgPhoto.Width;
                var sourceHeight = imgPhoto.Height;

                const int sourceX = 0;
                const int sourceY = 0;
                int destinationX = 0, destinationY = 0;

                float nPercent;
                var nPercentW = newWidth/(float) sourceWidth;
                var nPercentH = newHeight/(float) sourceHeight;
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                    destinationX = Convert.ToInt16((newWidth -
                                                    sourceWidth*nPercent)/2);
                }
                else
                {
                    nPercent = nPercentW;
                    destinationY = Convert.ToInt16((newHeight -
                                                    sourceHeight*nPercent)/2);
                }

                var destinationWidth = (int) (sourceWidth*nPercent);
                var destinationHeight = (int) (sourceHeight*nPercent);


                var bmPhoto = new Bitmap(newWidth, newHeight);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                    imgPhoto.VerticalResolution);

                using (var grPhoto = Graphics.FromImage(bmPhoto))
                {
                    grPhoto.SmoothingMode = SmoothingMode.HighQuality;
                    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    grPhoto.DrawImage(imgPhoto,
                        new Rectangle(destinationX, destinationY, destinationWidth, destinationHeight),
                        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                        GraphicsUnit.Pixel);
                }
                return bmPhoto;
            }
        }
    }
}