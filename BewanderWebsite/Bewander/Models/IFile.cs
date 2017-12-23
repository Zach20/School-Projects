using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    interface IFile
    {
        int ID { get; set; }

        string FileName { get; set; }

        string Path { get; set; }

        string Caption { get; set; }

        [DataType(DataType.DateTime)]
        DateTime DatePosted { get; set; }

        string UserID { get; set; }
    }
}