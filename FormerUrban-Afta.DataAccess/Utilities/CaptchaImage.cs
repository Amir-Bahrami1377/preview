using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FormerUrban_Afta.DataAccess.Utilities
{
    public class CaptchaImage
    {
        public string Text { get; }
        public Bitmap Image { get; private set; }
        public int Width { get; }
        public int Height { get; }

        // Private fields
        private readonly string _fontFamilyName;
        private readonly Random _random = new();
        private bool _disposed;

        // Constructors
        public CaptchaImage(string text, int width, int height)
            : this(text, width, height, FontFamily.GenericSerif.Name) { }

        public CaptchaImage(string text, int width, int height, string fontFamilyName)
        {
            ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));
            ValidateDimensions(width, height);

            Text = text;
            Width = width;
            Height = height;
            _fontFamilyName = ValidateFontFamily(fontFamilyName);
            GenerateImage();
        }

        // Resource cleanup
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Image?.Dispose();
            }

            _disposed = true;
        }

        ~CaptchaImage() => Dispose(false);

        // Validation methods
        private static void ValidateDimensions(int width, int height)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width, nameof(width));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height, nameof(height));
        }

        private static string ValidateFontFamily(string fontFamilyName)
        {
            try
            {
                using var font = new Font(fontFamilyName, 12f);
                return fontFamilyName;
            }
            catch
            {
                return FontFamily.GenericSerif.Name;
            }
        }

        // Image generation
        private void GenerateImage()
        {
            using var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            using var graphics = Graphics.FromImage(bitmap);

            ConfigureGraphics(graphics);
            var rect = new Rectangle(0, 0, Width, Height);

            DrawBackground(graphics, rect);
            DrawText(graphics, rect);
            AddNoise(graphics, rect);

            Image = (Bitmap)bitmap.Clone();
        }

        private void ConfigureGraphics(Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        private void DrawBackground(Graphics graphics, Rectangle rect)
        {
            using var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti,
                Color.LightGray,
                Color.White);
            graphics.FillRectangle(hatchBrush, rect);
        }

        private void DrawText(Graphics graphics, Rectangle rect)
        {
            using var font = CreateFittingFont(graphics, rect);
            using var path = CreateTextPath(font, rect);
            using var hatchBrush = new HatchBrush(HatchStyle.LargeConfetti,
                Color.Gray,
                Color.DarkGray);

            WarpPath(path, rect);
            graphics.FillPath(hatchBrush, path);
        }

        private Font CreateFittingFont(Graphics graphics, Rectangle rect)
        {
            float fontSize = rect.Height + 1;
            Font font;
            SizeF size;

            do
            {
                fontSize--;
                font = new Font(_fontFamilyName, fontSize, FontStyle.Bold);
                size = graphics.MeasureString(Text, font);
            } while (size.Width > rect.Width);

            return font;
        }

        private GraphicsPath CreateTextPath(Font font, Rectangle rect)
        {
            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var path = new GraphicsPath();
            path.AddString(Text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            return path;
        }

        private void WarpPath(GraphicsPath path, Rectangle rect)
        {
            const float v = 4f;
            var points = new[]
            {
            new PointF(_random.Next(rect.Width) / v, _random.Next(rect.Height) / v),
            new PointF(rect.Width - _random.Next(rect.Width) / v, _random.Next(rect.Height) / v),
            new PointF(_random.Next(rect.Width) / v, rect.Height - _random.Next(rect.Height) / v),
            new PointF(rect.Width - _random.Next(rect.Width) / v, rect.Height - _random.Next(rect.Height) / v)
        };

            using var matrix = new Matrix();
            matrix.Translate(0f, 0f);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0f);
        }

        private void AddNoise(Graphics graphics, Rectangle rect)
        {
            using var hatchBrush = new HatchBrush(HatchStyle.LargeConfetti,
                Color.Gray,
                Color.DarkGray);

            int maxDimension = Math.Max(rect.Width, rect.Height);
            int noiseCount = (int)(rect.Width * rect.Height / 30f);

            for (int i = 0; i < noiseCount; i++)
            {
                int x = _random.Next(rect.Width);
                int y = _random.Next(rect.Height);
                int w = _random.Next(maxDimension / 50);
                int h = _random.Next(maxDimension / 50);
                graphics.FillEllipse(hatchBrush, x, y, w, h);
            }
        }
    }
}
