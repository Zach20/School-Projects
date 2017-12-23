using System.Web;
using System.Web.Optimization;

namespace Bewander
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/bundles/typer").Include(
					 "~/Scripts/typer.js"
			   ));

			bundles.Add(new ScriptBundle("~/bundles/waypoints").Include(
					 "~/Scripts/jquery.waypoints.min.js"
			   ));

			bundles.Add(new ScriptBundle("~/bundles/signalrJQuery").Include(
                      "~/Scripts/jquery.signalR-2.2.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                      "~/Scripts/SignalR.js"));

            bundles.Add(new ScriptBundle("~/bundles/searchbar").Include(
                      "~/Scripts/Custom/SearchBar.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalrJQuery").Include(
                      "~/Scripts/jquery.signalR-2.2.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                      "~/Scripts/SignalR.js"));

            bundles.Add(new ScriptBundle("~/bundles/greensock").Include(
                "~/Scripts/GreenSock/plugins/CSSPlugin.min.js",
                "~/Scripts/GreenSock/TweenMax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/postInteractions").Include(
                "~/Scripts/Custom/PostInteractions.js"));

            bundles.Add(new ScriptBundle("~/bundles/profileimagepreviews").Include(
                "~/Scripts/Custom/profileimagepreviews.js"));

            bundles.Add(new ScriptBundle("~/bundles/PublicImageLikeUnlike").Include(
                "~/Scripts/Custom/PublicImageLikeUnlike.js"));

            // CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/Styles.css",
                      "~/Content/createReview.css", // once content is moved into scss file and compiling is figured out remove
                      "~/Content/navbar.css", // once content is moved into scss file and compiling is figured out remove
                      "~/Content/profilePage.css",
                      "~/Content/travelogue.css",
                      "~/Content/PublicPhoto.css",
                      "~/Content/About.css"));

        }
    }
}
