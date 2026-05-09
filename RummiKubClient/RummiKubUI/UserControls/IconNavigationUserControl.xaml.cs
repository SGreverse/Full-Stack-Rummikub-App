using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RummiKubUI.ServiceReference1;
using RummiKubUI.UtilClasses;

namespace RummiKubUI.UserControls
{
    /// <summary>
    /// Interaction logic for IconNavigationUserControl.xaml
    /// </summary>
    public partial class IconNavigationUserControl : UserControl
    {
        public DoublyLinkedImageList imageList;
        IconImage _currentImage;
        Player player;
        public IconNavigationUserControl()
        {
            InitializeComponent();
            RummiKubServiceClient service = new RummiKubServiceClient();
            List<string> defaultIcons = service.GetAllDefaultIcons();
            imageList = new DoublyLinkedImageList(new IconImage(defaultIcons[0]));

            _currentImage = imageList.data;

            for (int i = 1; i < defaultIcons.Count; i++)
            {
                imageList.SetNext(new IconImage(defaultIcons[i]));
                imageList = imageList.next;
            }

            this.DataContext = _currentImage;
        }
        public IconNavigationUserControl(Player player):this()
        {
            this.player = player;
            this.player.IconName = _currentImage.ImgName;
        }
        private void forceRefresh()
        {
            this.DataContext = null;
            this.DataContext = _currentImage;
            this.player.IconName = _currentImage.ImgName;

        }
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            string filename = ImageUtils.UploadImage_Dlg(player.Username);
            if (filename != null)
            {
                DoublyLinkedImageList current = imageList;
                while (current.next != null)
                {
                    current = current.next;
                }
                current.SetNext(new IconImage(filename));
                imageList=imageList.next;
                _currentImage = imageList.data;
                forceRefresh();
            }
            else
            {
                _currentImage = imageList.data;
                forceRefresh();

            }
        }

        private void PrevImage_Click(object sender, RoutedEventArgs e)
        {
            if (imageList.prev != null)
            {
                imageList = imageList.prev;
                _currentImage = imageList.data;
                forceRefresh();
            }
        }

        private void NextImage_Click(object sender, RoutedEventArgs e)
        {
            if (imageList.next != null)
            {
                imageList = imageList.next;
                _currentImage = imageList.data;
                forceRefresh();
            }
        }

        private void ShowCameraVideoCaptureClick(object sender, RoutedEventArgs e)
        {
            CameraImageCaptureWindow cameraWindow = new CameraImageCaptureWindow();
            cameraWindow.Show();
        }
    }
}
