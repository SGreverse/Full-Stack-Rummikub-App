using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Media3D;

namespace RummiKubUI
{
    public static class ParentConverter
    {
        public static object GetParentPage<T>(object obj)
        {
            // Traverse the visual tree to find the parent
            DependencyObject parent = VisualTreeHelper.GetParent(obj as DependencyObject);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }
    }
}
