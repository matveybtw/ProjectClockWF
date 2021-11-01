using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectClock
{
    public partial class Form1 : Form
    {
        public int s { get => DateTime.Now.Second; }
        public int m { get => DateTime.Now.Minute; }
        public int h { get => DateTime.Now.Hour; }
        public Form1()
        {
            InitializeComponent();
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    Invalidate();
                }
            });
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var Rect = new Rectangle(0, 0, width, height);
            var bitmap = new Bitmap(width, height);
            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var Mode = new ImageAttributes())
                {
                    Mode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, Rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, Mode);
                }
            }
            return bitmap;
        }
        public void EndOfLine(int xCenter, int yCenter, int lenght, int t, out double EndX, out double EndY)
        {
            EndX = xCenter + lenght * Math.Cos(Math.PI / 2 - t * (Math.PI / 180));
            EndY = yCenter - lenght * Math.Sin(Math.PI / 2 - t * (Math.PI / 180));
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Brushes.Black, 5);
            p.EndCap = LineCap.Triangle;
            Image img = ResizeImage(Image.FromFile(@"clock.png"), 500, 500);
            int t_s = 6 * s;
            int t_m = 6 * m;
            int t_h;
            if (h > 12)
                t_h = 30 * (h - 12);
            else
                t_h = 30 * h;
            int xCenter = 170;
            int yCenter = 170;
            double EndX;
            double EndY;
            g.DrawImage(img, 90, 90);
            EndOfLine(xCenter, yCenter, 60, t_s, out EndX, out EndY);
            p.Color = Color.Blue;
            g.DrawLine(p, xCenter, yCenter, (int)EndX, (int)EndY);
            EndOfLine(xCenter, yCenter, 40, t_m, out EndX, out EndY);
            p.Color = Color.Green;
            g.DrawLine(p, xCenter, yCenter, (int)EndX, (int)EndY);
            EndOfLine(xCenter, yCenter, 20, t_h, out EndX, out EndY);
            p.Color = Color.Red;
            g.DrawLine(p, xCenter, yCenter, (int)EndX, (int)EndY);
        }
    }
}
