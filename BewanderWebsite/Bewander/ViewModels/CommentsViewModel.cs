using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bewander.ViewModels
{
	public class CommentsViewModel
	{

		public string Content { get; set; }
		public DateTime DatePosted { get; set; }
		public int PostID { get; set; }

		public string User_UserID{ get; set; }
		public CommentsViewModel() {}
	}
}