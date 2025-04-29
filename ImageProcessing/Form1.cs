using System.Collections;
using System.Drawing.Imaging;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap originalImage, loadedImage;
        Pixel[,] originalMatrix;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadImage_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                loadedImage = new Bitmap(openFileDialog1.FileName);
                originalImage = new Bitmap(loadedImage);
                pictureBox1.Image = originalImage;
            }
        }

        private void btnResetImage_Click(object sender, EventArgs e)
        {
            if (originalImage == null) return;

            originalImage = new Bitmap(loadedImage);
            pictureBox1.Image = originalImage;
        }

        private void btnCrop_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;

            int cropW = int.Parse(txtCropWidth.Text);
            int cropH = int.Parse(txtCropHeight.Text);

            var bmp = new Bitmap(pictureBox1.Image);
            var matrix = ConvertBitmapToPixelMatrix(bmp);

            int imgW = bmp.Width, imgH = bmp.Height;
            int startX = Math.Max(0, (imgW - cropW) / 2);
            int startY = Math.Max(0, (imgH - cropH) / 2);

            cropW = Math.Min(cropW, imgW - startX);
            cropH = Math.Min(cropH, imgH - startY);

            var cropped = CropPixelMatrix(matrix, startX, startY, cropW, cropH);
            pictureBox2.Image = ConvertPixelMatrixToBitmap(cropped);
        }

        private void btnToGray_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;

            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Pixel[,] matrix = ConvertBitmapToPixelMatrix(bmp);

            Pixel[,] grayMatrix = ConvertToGrayscale(matrix);
            pictureBox2.Image = ConvertPixelMatrixToBitmap(grayMatrix);
        }

        private void btnToHVS_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;

            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Pixel[,] matrix = ConvertBitmapToPixelMatrix(bmp);

            Pixel[,] hsvMatrix = ConvertToHSV(matrix);
            pictureBox2.Image = ConvertPixelMatrixToBitmap(hsvMatrix);
        }

        private void btnToCMYK_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) return;

            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Pixel[,] matrix = ConvertBitmapToPixelMatrix(bmp);

            Pixel[,] yuvMatrix = ConvertToCMYK(matrix);
            pictureBox2.Image = ConvertPixelMatrixToBitmap(yuvMatrix);
        }

        private void trackBarZoomScroll_Scroll(object sender, EventArgs e)
        {
            originalMatrix = ConvertBitmapToPixelMatrix(originalImage);
            if (originalMatrix == null) return;

            int value = trackBarZoomScroll.Value;

            Pixel[,] zoomedMatrix;

            if (value == 10)
            {
                zoomedMatrix = originalMatrix;
            }
            else if (value > 10)
            {
                int scale = value - 9; // örneğin 11 için scale 2 olur
                zoomedMatrix = ZoomIn(originalMatrix, scale);
            }
            else // value < 10
            {
                int scale = 11 - value; // örneğin 9 için scale 2 olur
                zoomedMatrix = ZoomOut(originalMatrix, scale);
            }

            Bitmap bmp = ConvertPixelMatrixToBitmap(zoomedMatrix);
            pictureBox2.Image = bmp;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Width = bmp.Width;
            pictureBox2.Height = bmp.Height;
        }

        private Pixel[,] ConvertBitmapToPixelMatrix(Bitmap image)
        {
            int h = image.Height, w = image.Width;
            var matrix = new Pixel[h, w];

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    var c = image.GetPixel(x, y);
                    matrix[y, x] = new Pixel(c.R, c.G, c.B);
                }

            return matrix;
        }

        private Bitmap ConvertPixelMatrixToBitmap(Pixel[,] matrix)
        {
            int h = matrix.GetLength(0), w = matrix.GetLength(1);
            var bmp = new Bitmap(w, h);

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    var p = matrix[y, x];
                    bmp.SetPixel(x, y, Color.FromArgb(p.R, p.G, p.B));
                }

            return bmp;
        }

        private Pixel[,] CropPixelMatrix(Pixel[,] original, int startX, int startY, int cropWidth, int cropHeight)
        {
            var cropped = new Pixel[cropHeight, cropWidth];

            for (int y = 0; y < cropHeight; y++)
                for (int x = 0; x < cropWidth; x++)
                    cropped[y, x] = original[startY + y, startX + x];

            return cropped;
        }

        private Pixel[,] ConvertToGrayscale(Pixel[,] original)
        {
            int h = original.GetLength(0), w = original.GetLength(1);
            Pixel[,] grayMatrix = new Pixel[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Pixel p = original[y, x];
                    int gray = (int)(0.299 * p.R + 0.587 * p.G + 0.114 * p.B);
                    if (gray > 255) gray = 255;
                    if (gray < 0) gray = 0;

                    grayMatrix[y, x] = new Pixel((byte)gray, (byte)gray, (byte)gray);
                }
            }
            return grayMatrix;
        }

        private Pixel[,] ConvertToHSV(Pixel[,] original)
        {
            int h = original.GetLength(0), w = original.GetLength(1);
            Pixel[,] result = new Pixel[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Pixel p = original[y, x];
                    double r = p.R / 255.0;
                    double g = p.G / 255.0;
                    double b = p.B / 255.0;

                    double max = r > g ? (r > b ? r : b) : (g > b ? g : b);
                    double min = r < g ? (r < b ? r : b) : (g < b ? g : b);
                    double delta = max - min;

                    double hVal = 0, s = 0, v = max;

                    if (max != 0)
                        s = delta / max;

                    if (delta != 0)
                    {
                        if (max == r)
                            hVal = (g - b) / delta + (g < b ? 6 : 0);
                        else if (max == g)
                            hVal = (b - r) / delta + 2;
                        else
                            hVal = (r - g) / delta + 4;
                        hVal /= 6;
                    }

                    // S & V'yi %50 artır (maksimum 1 olacak şekilde)
                    s = s * 1.5;
                    if (s > 1) s = 1;

                    v = v * 1.5;
                    if (v > 1) v = 1;

                    // HSV → RGB
                    double r1 = 0, g1 = 0, b1 = 0;

                    if (s == 0)
                    {
                        r1 = g1 = b1 = v;
                    }
                    else
                    {
                        hVal *= 6;
                        int i = (int)hVal;
                        double f = hVal - i;
                        double p1 = v * (1 - s);
                        double q = v * (1 - s * f);
                        double t = v * (1 - s * (1 - f));

                        switch (i % 6)
                        {
                            case 0: r1 = v; g1 = t; b1 = p1; break;
                            case 1: r1 = q; g1 = v; b1 = p1; break;
                            case 2: r1 = p1; g1 = v; b1 = t; break;
                            case 3: r1 = p1; g1 = q; b1 = v; break;
                            case 4: r1 = t; g1 = p1; b1 = v; break;
                            case 5: r1 = v; g1 = p1; b1 = q; break;
                        }
                    }

                    result[y, x] = new Pixel(
                        (byte)ClampToByte(r1 * 255),
                        (byte)ClampToByte(g1 * 255),
                        (byte)ClampToByte(b1 * 255)
                    );
                }
            }
            return result;
        }

        private Pixel[,] ConvertToCMYK(Pixel[,] matrix)
        {
            int h = matrix.GetLength(0);
            int w = matrix.GetLength(1);
            Pixel[,] result = new Pixel[h, w];

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Pixel p = matrix[y, x];
                    float r = p.R / 255f;
                    float g = p.G / 255f;
                    float b = p.B / 255f;

                    float k = 1f - Math.Max(r, Math.Max(g, b));
                    float c = (1f - r - k) / (1f - k + 0.0001f);
                    float m = (1f - g - k) / (1f - k + 0.0001f);
                    float yC = (1f - b - k) / (1f - k + 0.0001f);

                    // K'yı %20 artır, daha karanlık olsun
                    k += 0.2f;
                    if (k > 1f) k = 1f;

                    // Geri RGB’ye çevir
                    float R = 255 * (1 - c) * (1 - k);
                    float G = 255 * (1 - m) * (1 - k);
                    float B = 255 * (1 - yC) * (1 - k);

                    // Clamp
                    byte rByte = (byte)(R < 0 ? 0 : (R > 255 ? 255 : R));
                    byte gByte = (byte)(G < 0 ? 0 : (G > 255 ? 255 : G));
                    byte bByte = (byte)(B < 0 ? 0 : (B > 255 ? 255 : B));

                    result[y, x] = new Pixel(rByte, gByte, bByte);
                }
            }
            return result;
        }

        private int ClampToByte(double val)
        {
            if (val < 0) return 0;
            if (val > 255) return 255;
            return (int)val;
        }

        private Pixel[,] ZoomIn(Pixel[,] original, int scale)
        {
            int originalHeight = original.GetLength(0);
            int originalWidth = original.GetLength(1);

            int newHeight = originalHeight * scale;
            int newWidth = originalWidth * scale;

            Pixel[,] zoomed = new Pixel[newHeight, newWidth];

            for (int y = 0; y < originalHeight; y++)
            {
                for (int x = 0; x < originalWidth; x++)
                {
                    Pixel p = original[y, x];
                    for (int dy = 0; dy < scale; dy++)
                    {
                        for (int dx = 0; dx < scale; dx++)
                        {
                            zoomed[y * scale + dy, x * scale + dx] = p;
                        }
                    }
                }
            }
            return zoomed;
        }

        private Pixel[,] ZoomOut(Pixel[,] original, int scale)
        {
            int originalHeight = original.GetLength(0);
            int originalWidth = original.GetLength(1);

            int newHeight = originalHeight / scale;
            int newWidth = originalWidth / scale;

            Pixel[,] zoomed = new Pixel[newHeight, newWidth];

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    int rSum = 0, gSum = 0, bSum = 0;
                    for (int dy = 0; dy < scale; dy++)
                    {
                        for (int dx = 0; dx < scale; dx++)
                        {
                            Pixel p = original[y * scale + dy, x * scale + dx];
                            rSum += p.R;
                            gSum += p.G;
                            bSum += p.B;
                        }
                    }

                    int count = scale * scale;
                    zoomed[y, x] = new Pixel((byte)(rSum / count), (byte)(gSum / count), (byte)(bSum / count));
                }
            }
            return zoomed;
        }
    }
}
