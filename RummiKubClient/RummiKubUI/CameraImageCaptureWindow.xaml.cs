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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Accord.Video.DirectShow;
using Accord.Video;
using System.IO;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for CameraImageCaptureWindow.xaml
    /// </summary>
    public partial class CameraImageCaptureWindow : Window
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private bool isCapturing = false;

        public CameraImageCaptureWindow()
        {
            InitializeComponent();
            LoadVideoDevices();
        }


        private void LoadVideoDevices()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo device in videoDevices)
            {
                webcamDevicesCombo.Items.Add(device.Name);
            }

            if (webcamDevicesCombo.Items.Count > 0)
            {
                webcamDevicesCombo.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No video devices found");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (videoDevices.Count == 0 || webcamDevicesCombo.SelectedIndex == -1)
                return;

            videoSource = new VideoCaptureDevice(videoDevices[webcamDevicesCombo.SelectedIndex].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();

            isCapturing = true;
            startButton.IsEnabled = false;
            CaptureButton.IsEnabled = true;
            SaveButton.IsEnabled = false;
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                IntPtr hBitmap = IntPtr.Zero;
                try
                {
                    // Get the bitmap handle
                    hBitmap = eventArgs.Frame.GetHbitmap();

                    // Convert System.Drawing.Bitmap to BitmapSource
                    var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());

                    bitmapSource.Freeze();

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        videoFeedBrush.ImageSource = bitmapSource;
                    }));
                }
                finally //Free the memory block to avoid Memory Overflow
                {
                    if (hBitmap != IntPtr.Zero)
                    {
                        DeleteObject(hBitmap);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error processing frame:\n" + exc.Message, "Error");
                StopButton_Click(null, null);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isCapturing) return;

            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= VideoSource_NewFrame;
            }

            isCapturing = false;
            startButton.IsEnabled = true;
            CaptureButton.IsEnabled = false;
            SaveButton.IsEnabled = true;
        }

        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            if (videoFeedBrush.ImageSource == null) return;

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "webcam_capture",
                DefaultExt = ".png",
                Filter = "PNG Image (.png)|*.png|JPEG Image (.jpg)|*.jpg"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                BitmapSource cropped = CropToCircle((BitmapSource)videoFeedBrush.ImageSource,(int) videoFeed.ActualWidth); 

                BitmapEncoder encoder;
                if (saveFileDialog.FileName.EndsWith(".jpg"))
                {
                    encoder = new JpegBitmapEncoder();
                }
                else
                {
                    encoder = new PngBitmapEncoder();
                }


                encoder.Frames.Add(BitmapFrame.Create(cropped));

                using (var stream = File.Create(saveFileDialog.FileName))
                {
                    encoder.Save(stream);
                }

                MessageBox.Show("Image saved successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        private BitmapSource CropToCircle(BitmapSource source, int diameter)
        {
            int size = Math.Min(source.PixelWidth, source.PixelHeight);
            int x = (source.PixelWidth - size) / 2;
            int y = (source.PixelHeight - size) / 2;

            var cropped = new CroppedBitmap(source, new Int32Rect(x, y, size, size));

            var scaled = new TransformedBitmap(cropped, new ScaleTransform((double)diameter / size, (double)diameter / size));

            var visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                EllipseGeometry circle = new EllipseGeometry(new Point(diameter / 2, diameter / 2), diameter / 2, diameter / 2);
                dc.PushClip(circle);
                dc.DrawImage(scaled, new Rect(0, 0, diameter, diameter));
            }

            var target = new RenderTargetBitmap(diameter, diameter, 96, 96, PixelFormats.Pbgra32);
            target.Render(visual);
            target.Freeze();
            return target;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (isCapturing)
            {
                StopButton_Click(null, null);
            }
            base.OnClosing(e);
        }

    }
}
