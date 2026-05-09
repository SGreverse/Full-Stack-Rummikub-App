using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RummiKubUI.ServiceReference1;

namespace RummiKubUI.UtilClasses
{
    public class ImageUtils
    {
        private static string imageDirectory= Path.Combine(Environment.CurrentDirectory, @"..\..\" + "/Assets/");
        public static string ImageDirectory { get => imageDirectory; set => imageDirectory = value; }

        public static string UploadImage_Dlg(string username)
        {

            string filename = null;


            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            dlg.Filter = "All Images | *.jpg;*.jpeg;*.tif;*.tiff;*.bmp;*.png|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                filename = SaveImage(filename,username); // save the Picture in LocalFolder (if not exist) rns return only thr file name

            }

            return filename;
        }


        public static string SaveImage(string sourcefileName,string username)
        {
            string fileName = Path.GetFileName(sourcefileName);
            string extension = Path.GetExtension(fileName);
            int FilesApperances = GetImageCount(username);
            string updatedfilename = $"Pic{username}({FilesApperances}){extension}";
            string localFilePath = Path.Combine(imageDirectory, updatedfilename);

            byte[] imgArray = File.ReadAllBytes(sourcefileName);
            var stream = new MemoryStream(imgArray);
            Image img = Image.FromStream(stream);


            img.Save(localFilePath);

            return updatedfilename;
        }


        private static int GetImageCount(string filename)
        {
            RummiKubServiceClient service= new RummiKubServiceClient();
            return service.GetImageCount(filename).ReturnedObject;

        }
        public static void SendImage(string image)
        {

            string path = Path.Combine(imageDirectory, image);
            byte[] imgArray = File.ReadAllBytes(path);

            RummiKubServiceClient service = new RummiKubServiceClient();
            service.SaveImage(imgArray, image);
        }
    }
}
