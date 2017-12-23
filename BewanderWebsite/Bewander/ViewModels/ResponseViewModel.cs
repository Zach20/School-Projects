using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bewander.ViewModels
{
    public class ResponseViewModel
    {
        public bool Success { get; set; }
        public ResponseViewModelError Error { get; set; }
    }

    public class ResponseViewModelError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}