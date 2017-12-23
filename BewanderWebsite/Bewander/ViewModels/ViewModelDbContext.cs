using Bewander.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bewander.ViewModels
{
    public class ViewModelDbContext
    {
        public static BewanderContext db = new BewanderContext();
    }
}