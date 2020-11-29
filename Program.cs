using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CCC
{
    class Program
    {
        private static void SplitTiffImages(string sourceFilePath, string destFilePath)
        {
            Image image = Image.FromFile(sourceFilePath);
            int frames = 0;
            Guid[] guid = image.FrameDimensionsList;
            FrameDimension fd = new FrameDimension(guid[0]);
            frames = image.GetFrameCount(fd);

            for (int i = 0; i < frames; i++)
            {
                image.SelectActiveFrame(fd, i);
                Bitmap myBitmap = new Bitmap(image);
                //int x = myBitmap.GetPixel(277, 314);
                //Color SlColor = myBitmap.GetPixel(100, 100);
                //Console.Write(SlColor);
                for (int Xcount = 0; Xcount < myBitmap.Width; Xcount++)
                {
                    for (int Ycount = 0; Ycount < myBitmap.Height; Ycount++)
                    {
                        Color pixelColor = myBitmap.GetPixel(Xcount, Ycount);
                        //Console.Write(pixelColor.R);
                        //int intensity = (int)pixelColor.R;
                        //Console.Write(intensity);
                        /*if (intensity != 0)
                        {
                            Console.Write(intensity);
                        }*/
                        //int newpixel = 255 - intensity;
                        myBitmap.SetPixel(Xcount, Ycount, Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B));
                    }
                }
                //Color pixelColor = myBitmap.GetPixel(277, 314);
                Console.Write("{0}/{1}\n", i, frames);
                myBitmap.Save(destFilePath + @"\pic_" + i.ToString() + ".tif", ImageFormat.Tiff);
                //image.Save(destFilePath + @"\pic_" + i.ToString() + ".tif", System.Drawing.Imaging.ImageFormat.Tiff);
            }
        }
        private static void datToTiff(string datDir, string number, string destFilePath, int gain)
        {
            string filepath = datDir;
            string[] data = File.ReadAllLines(filepath);
            Bitmap bmp = new Bitmap(512, 512);
            int lineNumber = 0;
            foreach (string line in data)
            {
                string[] newData = line.Split('\t');
                //Console.WriteLine(newData.Length);
                for (int i = 0; i < newData.Length; i++)
                {
                    int intensity = int.Parse(newData[i]) / gain;
                    bmp.SetPixel(lineNumber, i, Color.FromArgb(intensity, intensity, intensity));
                }
                lineNumber++;
            }
            bmp.Save(destFilePath + @"\" + number + ".tif", ImageFormat.Tiff);
        }
        private static void ReadPhotos(string photoDir, string destFilePath, int gain)
        {
            DirectoryInfo folder = new DirectoryInfo(photoDir);
            foreach (FileInfo file in folder.GetFiles("*.dat"))
            {
                string name = file.Name;
                Console.WriteLine(name);
                string newName = Path.GetFileNameWithoutExtension(name);
                datToTiff(photoDir + @"\" + name, newName, destFilePath, gain);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter filename:");
            string fileName = Console.ReadLine();
            Console.WriteLine("Enter gain:");
            int gain = Console.ReadLine();
            string directoryName = @".\" + fileName;
            string destFilePath = @".\Output";
            if (Directory.Exists(destFilePath))
            {
                Console.WriteLine("The directory {0} already exists.", destFilePath);
            }
            else
            {
                Directory.CreateDirectory(destFilePath);
                Console.WriteLine("Create new directory, {0}.", destFilePath);
            }
            //ReadPhotos(directoryName);
            ReadPhotos(directoryName, destFilePath, gain);
            //datToTiff(@"C:\Users\陳鴻羽\Desktop\1017\fly1\500v 10v 5us 70deg 048_Image.dat", @"\2", destFilePath);

            //Image image = Image.FromFile(@"C:\Users\陳鴻羽\Desktop\1017\fly1\500v 10v 5us 70deg 048_Image.dat");
            //image.Save(@"C:\Users\陳鴻羽\Desktop\1017\HAHA.tif", System.Drawing.Imaging.ImageFormat.Tiff);
            /*
            Console.WriteLine("Enter filename:");
            string fileName = Console.ReadLine();
            //Console.WriteLine("Enter output filename:");
            //string fileName = Console.ReadLine();
            string sourceFilePath = @".\" + fileName + ".tif";
            Console.WriteLine("The directory {0} was created.", destFilePath);
            SplitTiffImages(sourceFilePath, destFilePath);
            */
            Console.WriteLine("Done!!!");
            Console.ReadLine();
        }
    }
}
