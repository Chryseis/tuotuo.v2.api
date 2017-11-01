using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure.Util
{
    public class ImageUtils
    {
        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="originalImageBytes">原图片字节数组</param>
        /// <param name="imageFormat">图片格式</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="mode">压缩模式,HW、W、H、Cut、HorW</param>
        /// <returns></returns>
        public static byte[] CompressImage(byte[] originalImageBytes, string imageFormat,int width, int height, string mode)
        {
            byte[] bytes = null;
            Stream originalImageStream = new MemoryStream(originalImageBytes);
            Stream thumbnailImageStream = new MemoryStream();
            Image originalImage = Image.FromStream(originalImageStream);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            if (towidth > 0 || toheight > 0)
            {
                if (towidth <= 0)
                {
                    mode = "H";
                }
                if (toheight <= 0)
                {
                    mode = "W";
                }
                switch (mode)
                {
                    case "HW"://指定高宽缩放（可能变形）                
                        break;
                    case "W"://指定宽，高按比例,只有在图片宽度大于限定宽度的时候才压缩                   
                        {
                            if (towidth < ow)
                            {
                                toheight = originalImage.Height * width / originalImage.Width;
                            }
                            break;
                        }
                    case "H"://指定高，宽按比例,只有在图片高度大于限定高度的时候才压缩
                        {
                            if (toheight < oh)
                            {
                                towidth = originalImage.Width * height / originalImage.Height;
                            }
                            break;
                        }
                    case "Cut"://指定高宽裁减（不变形）        
                        {
                            if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                            {
                                oh = originalImage.Height;
                                ow = originalImage.Height * towidth / toheight;
                                y = 0;
                                x = (originalImage.Width - ow) / 2;
                            }
                            else
                            {
                                ow = originalImage.Width;
                                oh = originalImage.Width * height / towidth;
                                x = 0;
                                y = (originalImage.Height - oh) / 2;
                            }
                            break;
                        }
                    case "HorW":        //按照图片高度或者宽度同比压缩
                        {
                            if (towidth < ow && toheight < oh)    //如果图片宽和高都大于限定,则哪个比例大
                            {
                                if (((double)ow / towidth) > ((double)oh / toheight))   //如果宽的相差比例比较大,按照宽同比压缩
                                {
                                    toheight = originalImage.Height * width / originalImage.Width;
                                }
                                else
                                {
                                    towidth = originalImage.Width * height / originalImage.Height;
                                }
                            }
                            else if (towidth < ow)
                            {
                                toheight = originalImage.Height * width / originalImage.Width;
                            }
                            else if (toheight < oh)
                            {
                                towidth = originalImage.Width * height / originalImage.Height;
                            }
                            else
                            {
                                towidth = originalImage.Width;
                                toheight = originalImage.Height;
                            }
                            break;
                        }
                    default:
                        break;
                }

                //新建一个bmp图片
                System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

                //新建一个画板
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(System.Drawing.Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                    new System.Drawing.Rectangle(x, y, ow, oh),
                    System.Drawing.GraphicsUnit.Pixel);

                try
                {
                    bitmap.Save(thumbnailImageStream, GeImageFormatFromExtension(imageFormat));
                    bytes = new byte[thumbnailImageStream.Length];
                    thumbnailImageStream.Seek(0, SeekOrigin.Begin);
                    thumbnailImageStream.Read(bytes, 0, bytes.Length);
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    thumbnailImageStream.Dispose();
                    originalImageStream.Dispose();
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
            }

            return bytes;


        }

        /// <summary>
        /// 根据后缀获取图片格式
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static ImageFormat GeImageFormatFromExtension(string extension)
        {
            Enforce.That(!string.IsNullOrEmpty(extension),"文件后缀名不能为空");

            ImageFormat format = null;
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    format = ImageFormat.Jpeg;break;
                case ".png":
                    format = ImageFormat.Png; break;
                case ".bmp":
                    format = ImageFormat.Bmp; break;
                case ".gif":
                    format = ImageFormat.Gif; break;
            }
            Enforce.That(format!=null, "未匹配到合适的图片格式");
            return format;
        }
    }
}
