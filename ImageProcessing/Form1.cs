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

            // Merkezden kırpma koordinatları hesaplama
            int startX = Math.Max(0, (imgW - cropW) / 2);
            int startY = Math.Max(0, (imgH - cropH) / 2);

            // Kırpılacak alan görüntünün dışına taşmasın diye sınırlandırıldı.
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

            if (value == 0)
            {
                // 0 değeri, orijinal boyut (zoom yok)
                zoomedMatrix = originalMatrix;
            }
            else
            {
                // 1–10 arası değerler için büyütme oranını belirle (1 -> 2x, 2 -> 3x, ..., 10 -> 11x)
                int scale = value + 1;
                zoomedMatrix = ZoomIn(originalMatrix, scale);
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

            // İki boyutlu dizi [width, height] değerlerini tutuyor
            var matrix = new Pixel[h, w];

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    // Bitmap'in belirli bir (x, y) konumundaki pikseli okunur rgb değerleri alınır ve diziye aktarılır.
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
                    // Pixel matrisi içindeki(y, x) konumundaki piksel alınır ve renk değerleri aktarılır.
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
            int h = original.GetLength(0), w = original.GetLength(1); // Resmin yüksekliğini (satır sayısı) ve genişliğini (sütun sayısı) al
            Pixel[,] grayMatrix = new Pixel[h, w]; // Gri tonlamalı yeni resmi tutacak matris oluştur

            for (int y = 0; y < h; y++) // Her satır için
            {
                for (int x = 0; x < w; x++) // Her sütun için
                {
                    Pixel p = original[y, x]; // İlgili konumdaki orijinal pikseli al

                    // Gri ton değeri hesapla: insan gözünün renk duyarlılığına göre ağırlıklı ortalama
                    int gray = (int)(0.299 * p.R + 0.587 * p.G + 0.114 * p.B);

                    // Değerin 0-255 aralığında olmasını sağla
                    original[y, x] = new Pixel(
                        (byte)ClampToByte(gray * 255),
                        (byte)ClampToByte(gray * 255),
                        (byte)ClampToByte(gray * 255)
                    );

                    // Yeni pikselde R, G, B aynı olacak şekilde gri piksel oluştur
                    grayMatrix[y, x] = new Pixel((byte)gray, (byte)gray, (byte)gray);
                }
            }
            return grayMatrix; // Gri tonlamaya dönüştürülmüş resmi döndür
        }

        private Pixel[,] ConvertToHSV(Pixel[,] original)
        {
            int h = original.GetLength(0), w = original.GetLength(1); // Resmin yüksekliği (h) ve genişliği (w)
            Pixel[,] result = new Pixel[h, w]; // Yeni işlenmiş resmi tutacak dizi

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Pixel p = original[y, x]; // Mevcut pikseli al
                    double r = p.R / 255.0;   // R, G, B değerlerini 0-1 aralığına getir
                    double g = p.G / 255.0;
                    double b = p.B / 255.0;

                    // Maksimum ve minimum renk bileşenlerini bul
                    double max = r > g ? (r > b ? r : b) : (g > b ? g : b);
                    double min = r < g ? (r < b ? r : b) : (g < b ? g : b);
                    double delta = max - min; // Renk farkı

                    double hVal = 0, s = 0, v = max; // Hue (hVal), Saturation (s), Value (v)

                    if (max != 0)
                        s = delta / max; // Saturation hesapla

                    // Hue hesaplama (renk tonu)
                    if (delta != 0)
                    {
                        if (max == r)
                            hVal = (g - b) / delta + (g < b ? 6 : 0);
                        else if (max == g)
                            hVal = (b - r) / delta + 2;
                        else
                            hVal = (r - g) / delta + 4;

                        hVal /= 6; // Hue değeri 0-1 aralığına çekilir
                    }

                    // Saturation ve Value değerlerini %50 artır (maksimum 1 ile sınırla)
                    s = s * 1.5;
                    if (s > 1) s = 1;

                    v = v * 1.5;
                    if (v > 1) v = 1;

                    // HSV'den tekrar RGB'ye dönüşüm
                    double r1 = 0, g1 = 0, b1 = 0;

                    if (s == 0)
                    {
                        // Renk doygunluğu sıfırsa, gri tonlu renk oluşur
                        r1 = g1 = b1 = v;
                    }
                    else
                    {
                        hVal *= 6; // Hue değeri 0-6 aralığına çıkarılır
                        int i = (int)hVal; // Hangi renk aralığında olduğunu bul
                        double f = hVal - i; // Aralığın içindeki konumu
                        double p1 = v * (1 - s);
                        double q = v * (1 - s * f);
                        double t = v * (1 - s * (1 - f));

                        // Renk dönüşümüne göre RGB değerlerini hesapla
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

                    // Sonuç piksellerini yeni dizide sakla (RGB değerleri tekrar 0-255'e çevrilir)
                    result[y, x] = new Pixel(
                        (byte)ClampToByte(r1 * 255),
                        (byte)ClampToByte(g1 * 255),
                        (byte)ClampToByte(b1 * 255)
                    );
                }
            }
            return result; // İşlenmiş görüntüyü döndür
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

                    // K (Black) bileşeni: RGB’deki en yüksek değere bakılarak belirlenir.
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
            int originalHeight = original.GetLength(0);  // Orijinal resmin yüksekliği
            int originalWidth = original.GetLength(1);   // Orijinal resmin genişliği

            int newHeight = originalHeight * scale;  // Yeni yüksekliği hesapla
            int newWidth = originalWidth * scale;   // Yeni genişliği hesapla

            Pixel[,] zoomed = new Pixel[newHeight, newWidth];  // Yeni resim için yer ayır

            // Orijinal resmin her bir pikselini işleme al
            for (int y = 0; y < originalHeight; y++)
            {
                for (int x = 0; x < originalWidth; x++)
                {
                    Pixel p = original[y, x];  // Orijinal resmin pikselini al

                    // Bu pikseli yeni resme yerleştir
                    for (int dy = 0; dy < scale; dy++) // Ölçek kadar y ekseninde kopyala
                    {
                        for (int dx = 0; dx < scale; dx++) // Ölçek kadar x ekseninde kopyala
                        {
                            zoomed[y * scale + dy, x * scale + dx] = p;  // Yeni resme yerleştir
                        }
                    }
                }
            }
            return zoomed;  // Büyütülmüş resmi döndür
        }

        private Pixel[,] ZoomOut(Pixel[,] original, int scale)
        {
            int originalHeight = original.GetLength(0);  // Orijinal resmin yüksekliği
            int originalWidth = original.GetLength(1);   // Orijinal resmin genişliği

            int newHeight = originalHeight / scale;  // Yeni yüksekliği hesapla
            int newWidth = originalWidth / scale;   // Yeni genişliği hesapla

            Pixel[,] zoomed = new Pixel[newHeight, newWidth];  // Yeni resim için yer ayır

            // Yeni resme her bir pikseli yerleştir
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    int rSum = 0, gSum = 0, bSum = 0;

                    // Ölçek kadar piksellerin ortalamasını al
                    for (int dy = 0; dy < scale; dy++)
                    {
                        for (int dx = 0; dx < scale; dx++)
                        {
                            Pixel p = original[y * scale + dy, x * scale + dx];  // Orijinal resmin pikselini al
                            rSum += p.R;  // Kırmızı kanalını topla
                            gSum += p.G;  // Yeşil kanalını topla
                            bSum += p.B;  // Mavi kanalını topla
                        }
                    }
                    int count = scale * scale;  // Ortalama için toplam piksel sayısı
                    zoomed[y, x] = new Pixel((byte)(rSum / count), (byte)(gSum / count), (byte)(bSum / count));  // Ortalama rengi yeni pikselle yerleştir
                }
            }
            return zoomed;  // Küçültülmüş resmi döndür
        }
    }
}
