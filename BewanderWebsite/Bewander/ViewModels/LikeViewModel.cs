using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bewander.DAL;
using Bewander.Models;
using System.ComponentModel.DataAnnotations;

namespace Bewander.ViewModels
{
    public class LikeViewModel
    {
        public Dictionary<string, string> Likes { get; set; }

        public LikeViewModel()
        {
            Likes = new Dictionary<string, string>();
        }
    }
}