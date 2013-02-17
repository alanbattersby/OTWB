using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Geometric_Chuck
{
    public class PageData
    {
           public string Title { get; set; }
           public string Description { get; set; }
           public BitmapImage Icon { get; set; }
           public Type PageType { get; set; }
           public bool IsCodePage { get; set; }

           public PageData(string title, string desc, Type page, BitmapImage img, bool iscode)
           {
               Title = title;
               Description = desc;
               PageType = page;
               Icon = img;
               IsCodePage = iscode;
           }

           public PageData(string title, string desc, Type page, BitmapImage img)
               :this(title,desc,page,img,false) {}
    }
}
