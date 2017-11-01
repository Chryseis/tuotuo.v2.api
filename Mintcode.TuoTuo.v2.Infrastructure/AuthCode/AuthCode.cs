﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Infrastructure
{
    public class AuthCode
    {
        //颜色列表，用于验证码、噪线、噪点 
        private static Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };

        //字体列表，用于验证码 
        private static string[] font = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };

        //验证码的字符集，去掉了一些容易混淆的字符 
        private static char[] character = { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
        
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片长度</param>
        /// <param name="format">图片格式</param>
        /// <returns>AuthCodeModel</returns>
        public static AuthCodeModel CreateAuthCode(int width, int height,ImageFormat format)
        {
            string chkCode = string.Empty;

            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }

            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线 
            //for (int i = 0; i < 10; i++)
            //{
            //    int x1 = rnd.Next(100);
            //    int y1 = rnd.Next(40);
            //    int x2 = rnd.Next(100);
            //    int y2 = rnd.Next(40);
            //    Color clr = color[rnd.Next(color.Length)];
            //    g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            //}
            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, 18);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 20 + 8, (float)8);
            }
            //画噪点 
            //for (int i = 0; i < 100; i++)
            //{
            //    int x = rnd.Next(bmp.Width);
            //    int y = rnd.Next(bmp.Height);
            //    Color clr = color[rnd.Next(color.Length)];
            //    bmp.SetPixel(x, y, clr);
            //}

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, format);
            ms.Seek(0, SeekOrigin.Begin);
            bmp.Dispose();
            g.Dispose();
            return new AuthCodeModel { Img = ms, Code = chkCode };
        }
    }
}
