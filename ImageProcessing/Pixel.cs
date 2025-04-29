using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class Pixel
    {
        public byte R;
        public byte G;
        public byte B;

        public Pixel(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
