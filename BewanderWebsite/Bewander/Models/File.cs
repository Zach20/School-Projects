using Bewander.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bewander.Models
{
    public class File : IFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string FileName { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        public FileType FileType { get; set; }

        public string Caption { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; }

        public string UserID { get; set; }

        public virtual User User { get; set; }

        public bool flagged { get; set; }

        public File() { }

        public File(HttpPostedFileBase file)
        {
            FileName = System.IO.Path.GetFileName(file.FileName);
            Path = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
            FileType = FileType.Photo;
            DatePosted = DateTime.Now;
        }


        // Functions



    }
}