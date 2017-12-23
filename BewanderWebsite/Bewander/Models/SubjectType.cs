using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    public enum SubjectType
    {
        [Display(Name = "Come here for...")]
        Default = 0,
        [Display(Name = "Good Eats")]
        GoodEats = 1,
        [Display(Name = "Safe Stays")]
        SafeStays = 2,
        [Display(Name = "Cool Sites")]
        CoolSites = 3,
        [Display(Name = "Fun Times")]
        FunTimes = 4,
        [Display(Name = "Legit Business")]
        LegitBusiness = 5,
        Other = 6
    }
}