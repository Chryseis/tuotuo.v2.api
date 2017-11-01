using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public class Avatar
    {
        private static Color[] color = { 
                                           Color.FromArgb(255, 216, 168),
                                           Color.FromArgb(255, 236, 153),
                                           Color.FromArgb(216, 45, 162), 
                                           Color.FromArgb(178, 242, 187), 
                                           Color.FromArgb(150, 242, 215), 
                                           Color.FromArgb(153, 233, 242),
                                           Color.FromArgb(162,218,255),
                                           Color.FromArgb(186,200,255),
                                           Color.FromArgb(238,190,250),
                                           Color.FromArgb(255,201,201)
                                       };

        /// <summary>
        ///创建默认头像(200X200)
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] CreateAvatar(string content)
        {
            int width = 200;
            int height = 200;
            int fontSize = 80;
            Random rnd = new Random();
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            SolidBrush backgroundBrush = new SolidBrush(color[rnd.Next(color.Length)]);
            SolidBrush fontBrush = new SolidBrush(Color.White);
            g.FillRectangle(backgroundBrush, 0, 0, width, height);
            Font drawFont = new Font("Microsoft YaHei", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.DrawString(content, drawFont, fontBrush, new Rectangle((width - fontSize * content.Length) / 2, (width - fontSize) / 2, fontSize * content.Length, fontSize), sf);

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            bmp.Dispose();
            backgroundBrush.Dispose();
            fontBrush.Dispose();
            g.Dispose();

            var bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
