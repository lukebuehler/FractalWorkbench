using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FractalWorkbench.Core;

namespace FractalWorkbench.Explorer
{
    public partial class FractalImage : Control
    {
        private readonly IFractal _fractal;
        private readonly Range _defaultRange;
        private readonly int _maxTries;

        public FractalImage(IFractal fractal)
        {
            if (fractal == null) throw new ArgumentNullException("fractal");

            InitializeComponent();

            _fractal = fractal;
            _defaultRange = _fractal.DefaultRange;
            _maxTries = _fractal.MaxTries;

            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            var g = pe.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            var bmp = new Bitmap(Width*2, Height*2);
            var lockBmp = new LockBitmap(bmp);
            var viewport = new ViewPort(bmp.Width, bmp.Height);
            var data = _fractal.GetFractal(_defaultRange, viewport);

            lockBmp.LockBits();
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    var n = data[x, y];
                    var color = Color.White;
                    if (n >= _maxTries)
                        color = Color.Black;
                    lockBmp.SetPixel(x, y, color);
                }
            }
            lockBmp.UnlockBits();

            var bmpResized = ResizeImage(bmp, Width, Height);
            g.DrawImage(bmpResized, 0, 0);
            //base.OnPaint(pe);
        }

        //http://stackoverflow.com/questions/1922040/resize-an-image-c-sharp
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
