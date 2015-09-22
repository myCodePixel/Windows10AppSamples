using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWA_XML_Binding
{
    public class flipimage
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string imageurl;
        public string ImageUrl
        {
            get { return imageurl; }
            set { imageurl = value; }
        }
        public flipimage(string s, string sa)
        {
            title = s;
            imageurl = sa;
        }
    }
}
