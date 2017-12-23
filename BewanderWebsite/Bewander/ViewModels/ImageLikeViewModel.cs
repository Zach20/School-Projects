using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bewander.ViewModels
{
    public class ImageLikeViewModel
    {
        public Dictionary<string, string> ImageLikes { get; set; }

        public ImageLikeViewModel()
        {
            ImageLikes = new Dictionary<string, string>();
        }
    }
}