using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Loachs.Web
{
    /// <summary>
    ///     Watermark 的摘要说明
    /// </summary>
    public class Watermark
    {
        private Watermark()
        {
        }

        /// <summary>
        ///     添加图片水印
        /// </summary>
        /// <param name="oldFilePath">原始图片路径</param>
        /// <param name="newFilePath">将要添加水印图片路径</param>
        /// <param name="waterPosition">水印位置</param>
        /// <param name="waterImagePath">水印图片路径</param>
        /// <param name="watermarkTransparency">透明度</param>
        /// <param name="quality">质量</param>
        public static void CreateWaterImage(string oldFilePath, string newFilePath, int waterPosition,
            string waterImagePath, int watermarkTransparency, int quality)
        {
            Image image = Image.FromFile(oldFilePath);

            Bitmap bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            g.DrawImage(image, 0, 0, image.Width, image.Height);

            //设置透明度
            Image watermark = new Bitmap(waterImagePath);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap
            {
                OldColor = Color.FromArgb(255, 0, 255, 0),
                NewColor = Color.FromArgb(0, 0, 0, 0)
            };
            ColorMap[] remapTable = {colorMap};
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                transparency = (watermarkTransparency/10.0F);
            }

            float[][] colorMatrixElements =
            {
                new[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                new[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                new[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                new[] {0.0f, 0.0f, 0.0f, transparency, 0.0f}, //注意：倒数第二处为0.0f为完全透明，1.0f为完全不透明
                new[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
            };
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int width = image.Width;
            int height = image.Height;
            int xpos = 0;
            int ypos = 0;
            int watermarkWidth = 0;
            int watermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((width > watermark.Width*2) && (height > watermark.Height*2))
            {
                bl = 1;
            }
            else if ((width > watermark.Width*2) && (height < watermark.Height*2))
            {
                bl = Convert.ToDouble(height/2)/Convert.ToDouble(watermark.Height);
            }
            else if ((width < watermark.Width*2) && (height > watermark.Height*2))
            {
                bl = Convert.ToDouble(width/2)/Convert.ToDouble(watermark.Width);
            }
            else
            {
                if ((width*watermark.Height) > (height*watermark.Width))
                {
                    bl = Convert.ToDouble(height/2)/Convert.ToDouble(watermark.Height);
                }
                else
                {
                    bl = Convert.ToDouble(width/2)/Convert.ToDouble(watermark.Width);
                }
            }
            watermarkWidth = Convert.ToInt32(watermark.Width*bl);
            watermarkHeight = Convert.ToInt32(watermark.Height*bl);
            switch (waterPosition)
            {
                case 3:
                    xpos = width - watermarkWidth - 10;
                    ypos = 10;
                    break;
                case 2:
                    xpos = 10;
                    ypos = height - watermarkHeight - 10;
                    break;
                case 5:
                    xpos = width/2 - watermarkWidth/2;
                    ypos = height/2 - watermarkHeight/2;
                    break;
                case 1:
                    xpos = 10;
                    ypos = 10;
                    break;
                case 4:
                default:
                    xpos = width - watermarkWidth - 10;
                    ypos = height - watermarkHeight - 10;
                    break;
            }
            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermarkWidth, watermarkHeight), 0, 0, watermark.Width,
                watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            try
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs.Where(codec => codec.MimeType.IndexOf("jpeg") > -1))
                {
                    ici = codec;
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];

                if (quality < 0 || quality > 100)
                {
                    quality = 80;
                }

                qualityParam[0] = quality;

                EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    bmp.Save(newFilePath, ici, encoderParams);
                }
                else
                {
                    bmp.Save(newFilePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                watermark.Dispose();
                imageAttributes.Dispose();
                image.Dispose();
                bmp.Dispose();
            }
        }

        /// <summary>
        ///     添加文字水印
        /// </summary>
        /// <param name="oldFilePath">原始图片路径</param>
        /// <param name="newFilePath">将要添加水印图片路径</param>
        /// <param name="waterPosition">水印位置</param>
        /// <param name="waterText">水印内容</param>
        /// <param name="quality"></param>
        /// <param name="fontname"></param>
        /// <param name="fontsize"></param>
        public static void CreateWaterText(string oldFilePath, string newFilePath, int waterPosition, string waterText,
            int quality, string fontname, int fontsize)
        {
            Image image = Image.FromFile(oldFilePath);
            Bitmap bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            g.DrawImage(image, 0, 0, image.Width, image.Height);

            int width = bmp.Width;
            int height = bmp.Height;

            Font crFont = new Font(fontname, fontsize, FontStyle.Bold, GraphicsUnit.Pixel);
            SizeF crSize = g.MeasureString(waterText, crFont);

            float xpos = 0;
            float ypos = 0;
            switch (waterPosition)
            {
                case 3:
                    xpos = (width*(float) .99) - (crSize.Width/2);
                    ypos = height*(float) .01;
                    break;
                case 2:
                    xpos = (width*(float) .01) + (crSize.Width/2);
                    ypos = (height*(float) .99) - crSize.Height;
                    break;
                case 5:
                    xpos = ((width - crSize.Width)/2) + crSize.Width/2; //奇怪的表达式
                    ypos = (height - crSize.Height)/2 + crSize.Height/2;
                    break;
                case 1:

                    xpos = (width*(float) .01) + (crSize.Width/2);
                    ypos = height*(float) .01;
                    break;

                case 4:
                default:
                    xpos = (width*(float) .99) - (crSize.Width/2);
                    ypos = (height*(float) .99) - crSize.Height;
                    break;
            }

            StringFormat strFormat = new StringFormat {Alignment = StringAlignment.Center};

            //可设置透明度
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            g.DrawString(waterText, crFont, semiTransBrush, xpos, ypos, strFormat);

            try
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType.IndexOf("jpeg") > -1)
                    {
                        ici = codec;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityParam = new long[1];

                if (quality < 0 || quality > 100)
                {
                    quality = 80;
                }

                qualityParam[0] = quality;


                EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
                encoderParams.Param[0] = encoderParam;

                if (ici != null)
                {
                    bmp.Save(newFilePath, ici, encoderParams);
                }
                else
                {
                    bmp.Save(newFilePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                semiTransBrush.Dispose();
                image.Dispose();
                bmp.Dispose();
            }
        }
    }
}