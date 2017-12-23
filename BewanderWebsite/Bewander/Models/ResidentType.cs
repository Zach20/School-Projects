using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    public enum ResidentType
    {
        [Display(Name = "I am a...")]
        Default = 0,
        Local = 1,
        Traveler = 2,
        Other = 3
    }
}