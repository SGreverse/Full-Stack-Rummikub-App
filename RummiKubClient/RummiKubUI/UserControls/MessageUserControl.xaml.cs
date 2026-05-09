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

namespace RummiKubUI.UserControls
{
    /// <summary>
    /// Interaction logic for MessageUserControl.xaml
    /// </summary>
    public partial class MessageUserControl : UserControl
    {
        public MessageUserControl()
        {
            InitializeComponent();
        }
        public MessageUserControl(Message message):this()
        {
            this.Message.Text = $"Sent By {message.SentBy}[{message.SendingTime.ToString("HH:mm")}]:{message.Msg}";
        }
    }
}
