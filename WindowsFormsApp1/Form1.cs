using GroupDocs.Conversion;
using GroupDocs.Conversion.Options.Convert;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string[] DosyaYolu;
        int sayac = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            sayac = 0;
            string[] newPath = new string[DosyaYolu.Length-1];
            if (checkBox1.Checked)
            {
                using (ImageFactory factory = new ImageFactory(preserveExifData: false))
                {
                    for (int i = 0; i < (DosyaYolu.Length - 1); i++)
                    {
                        string path = DosyaYolu[i];

                        FileInfo info = new FileInfo(path);
                        string directoryPath = info.Directory.FullName + "\\output\\";
                        string fileName = info.Name.Replace(info.Extension, "");

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string output = directoryPath + fileName + ".jpg";
                        newPath[i] = output;

                        factory.Load(path)
                        .Format(new JpegFormat())
                        .Quality(100)
                        .Save(output);
                    }
                }
            }

            if (checkBox2.Checked)
            {
                if(newPath[0] == null)
                {
                    newPath = DosyaYolu;
                }

                List<Bitmap> bitmapList = new List<Bitmap>();

                for (int i = 0; i < newPath.Length; i++)
                {
                    bitmapList.Add(new Bitmap(newPath[i]));
                }

                FileInfo info = new FileInfo(newPath[0]);
                string dir = info.Directory.FullName+"\\output\\";

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                int isim = 0;

                while (bitmapList.Count != 0)
                {
                    var outputHeight = 0;
                    var outputWidth = 0;

                    int sayac = 0;
                    foreach (var item in bitmapList)
                    {

                        var sinir = (outputHeight + item.Height);
                        if (sinir < 30000)
                        {
                            sayac++;
                            outputHeight += item.Height;
                        }
                        if (item.Width > outputWidth)
                        {
                            outputWidth = item.Width;
                        }
                    }

                    Bitmap outputImage = new Bitmap(outputWidth, outputHeight);

                    using (Graphics graphics = Graphics.FromImage(outputImage))
                    {
                        int totalHeight = 0;
                        for (int i = 0; i < sayac; i++)
                        {
                            var source = bitmapList[i];
                            graphics.DrawImage(source,
                                new Rectangle(0, totalHeight, outputWidth, source.Height),
                                new Rectangle(0, 0, outputWidth, source.Height), 
                                GraphicsUnit.Pixel);
                            totalHeight += source.Height;
                        }
                    }
                    isim++;
                    outputImage.Save(dir+"output" + isim +".jpg", ImageFormat.Jpeg);
                    for (int i = 0; i < sayac; i++)
                    {
                        bitmapList.RemoveAt(0);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();

                DosyaYolu = dialog.FileNames;

                foreach (var item in DosyaYolu)
                {
                    listBox1.Items.Add(item);
                }
                button1.Enabled = true;
            }
        }
    }
}
