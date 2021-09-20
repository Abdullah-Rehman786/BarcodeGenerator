using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BarcodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
                char x = 'n';

                do
                {
                    Console.WriteLine("Type barcode to be generated");
                    string code = Console.ReadLine();
                    
                    BarCodeGenerator barCode = new BarCodeGenerator(code);
                    barCode.SaveImage("C:\\Users\\testing");
                    Console.WriteLine("Wanna do it again?");
                    x = (char)Console.Read();

                } while (x == 'Y' || x == 'y');
            
        }
    }

    public class BarCodeGenerator
    {
        public BarCodeGenerator(string code, int barHeight = 200, int imageWidth = 420, int imageHeigth = 240)
        {
            _barCode = code;
        }

        public void SaveImage(string filePath)
        {
            Bitmap bmp = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bmp);

            g.FillRectangle(Brushes.White, 0, 0, Width, Height);

            String intercharacterGap = "0";
            String str = '*' + _barCode.ToUpper() + '*';
            int strLength = str.Length;

            for (int i = 0; i < _barCode.Length; i++)
            {
                if (alphabet39.IndexOf(_barCode[i]) == -1 || _barCode[i] == '*')
                {
                    g.DrawString("INVALID BAR CODE TEXT", new Font("Arial", 12), Brushes.Red, 10, 10);
                    return;
                }
            }

            String encodedString = "";

            for (int i = 0; i < strLength; i++)
            {
                if (i > 0)
                    encodedString += intercharacterGap;

                encodedString += coded39Char[alphabet39.IndexOf(str[i])];
            }

            int encodedStringLength = encodedString.Length;
            int widthOfBarCodeString = 0;
            double wideToNarrowRatio = 3;


            if (align != AlignType.Left)
            {
                for (int i = 0; i < encodedStringLength; i++)
                {
                    if (encodedString[i] == '1')
                        widthOfBarCodeString += (int)(wideToNarrowRatio * (int)weight);
                    else
                        widthOfBarCodeString += (int)weight;
                }
            }

            int x = 0;
            int wid = 0;
            int yTop = 0;
            SizeF hSize = g.MeasureString(headerText, headerFont);
            SizeF fSize = g.MeasureString(_barCode, footerFont);

            int headerX = 0;
            int footerX = 0;

            if (align == AlignType.Left)
            {
                x = leftMargin;
                headerX = leftMargin;
                footerX = leftMargin;
            }
            else if (align == AlignType.Center)
            {
                x = (Width - widthOfBarCodeString) / 2;
                headerX = (Width - (int)hSize.Width) / 2;
                footerX = (Width - (int)fSize.Width) / 2;
            }
            else
            {
                x = Width - widthOfBarCodeString - leftMargin;
                headerX = Width - (int)hSize.Width - leftMargin;
                footerX = Width - (int)fSize.Width - leftMargin;
            }

            if (showHeader)
            {
                yTop = (int)hSize.Height + topMargin;
                g.DrawString(headerText, headerFont, Brushes.Black, headerX, topMargin);
            }
            else
            {
                yTop = topMargin;
            }

            for (int i = 0; i < encodedStringLength; i++)
            {
                if (encodedString[i] == '1')
                    wid = (int)(wideToNarrowRatio * (int)weight);
                else
                    wid = (int)weight;

                g.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White, x, yTop, wid, height);

                x += wid;
            }

            yTop += height;

            if (showFooter)
                g.DrawString(_barCode, footerFont, Brushes.Black, footerX, yTop);

            g.Flush();
            using (FileStream stream = File.Create(filePath))
            {
                bmp.Save(stream, ImageFormat.Jpeg);
            }
        }

        private string alphabet39 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

        private string[] coded39Char =
        {
        /* 0 */ "000110100", 
        /* 1 */ "100100001", 
        /* 2 */ "001100001", 
        /* 3 */ "101100000",
        /* 4 */ "000110001", 
        /* 5 */ "100110000", 
        /* 6 */ "001110000", 
        /* 7 */ "000100101",
        /* 8 */ "100100100", 
        /* 9 */ "001100100", 
        /* A */ "100001001", 
        /* B */ "001001001",
        /* C */ "101001000", 
        /* D */ "000011001", 
        /* E */ "100011000", 
        /* F */ "001011000",
        /* G */ "000001101", 
        /* H */ "100001100", 
        /* I */ "001001100", 
        /* J */ "000011100",
        /* K */ "100000011", 
        /* L */ "001000011", 
        /* M */ "101000010", 
        /* N */ "000010011",
        /* O */ "100010010", 
        /* P */ "001010010", 
        /* Q */ "000000111", 
        /* R */ "100000110",
        /* S */ "001000110", 
        /* T */ "000010110", 
        /* U */ "110000001", 
        /* V */ "011000001",
        /* W */ "111000000", 
        /* X */ "010010001", 
        /* Y */ "110010000", 
        /* Z */ "011010000",
        /* - */ "010000101", 
        /* . */ "110000100", 
        /*' '*/ "011000100",
        /* $ */ "010101000",
        /* / */ "010100010", 
        /* + */ "010001010", 
        /* % */ "000101010", 
        /* * */ "010010100"
        };

        public enum AlignType
        {
            Left, Center, Right
        }

        public enum BarCodeWeight
        {
            Small = 1, Medium, Large
        }

        private AlignType align = AlignType.Center;
        private string _barCode = "1234567890";
        private int leftMargin = 10;
        private int topMargin = 20;
        private int height = 200;
        private bool showHeader = false;
        private bool showFooter = false;
        private String headerText = "BarCode Demo";
        private BarCodeWeight weight = BarCodeWeight.Small;
        private Font headerFont = new Font("Courier", 18);
        private Font footerFont = new Font("Courier", 8);

        private int _height = 240;
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        private int _width = 420;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
    }
}
