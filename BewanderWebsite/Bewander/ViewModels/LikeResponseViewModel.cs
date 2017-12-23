using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bewander.ViewModels
{
    public class LikeResponseViewModel
    {
        public bool Success { get; set; }
        public ResponseError Error { get; set; }
        public int NumberOfLikes { get; set; }
    }

    public class ResponseError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

}