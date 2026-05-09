using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubUI.UtilClasses
{
    public class IconImage
    {
        private string imgName;
        public string ImgName { get => imgName; set => imgName = value; }

        public IconImage(string imgName)
        {
           this.ImgName = imgName;

        }
    }
}
