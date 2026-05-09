using RummiKubUI.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Drawing;
namespace RummiKubUI.Converters
{
    public class ImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            string fileName = (string)value;
            if (fileName == "") return null;

            // string path = Path.Combine(Environment.CurrentDirectory, @"..\..\" + "/Images/" + fileName);
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\" + "/Assets/" + fileName);
            //string path = System.IO.Path.Combine(App.ImageDirectory, fileName);

            if (!File.Exists(path))//if file is not in the assets folder, get him from the server
                {
                    GetImage(fileName, path);
                }
            return new BitmapImage(new Uri(path));
        }

        private void GetImage(string fileName, string localFilePath)
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            byte[] imageArray = service.GetImage(fileName).ReturnedObject;

            var stream = new MemoryStream(imageArray);
            Image img =Image.FromStream(stream);
            img.Save(localFilePath);
        }

        ////private void DownloadImage(Uri uri, string localFilePath)
        ////{
        ////    using (System.Net.WebClient webClient = new System.Net.WebClient())
        ////    {
        ////        using (Stream stream = webClient.OpenRead(uri))
        ////        {
        ////            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream))
        ////            {
        ////                stream.Flush();
        ////                stream.Close();
        ////                bitmap.Save(localFilePath);
        ////            }
        ////        }
        ////    }
        ////}


        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
