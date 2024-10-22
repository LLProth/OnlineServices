using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WsiWebHelpers;

namespace OnlineServices
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        //private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            ///////////////////////////////////////////////////
            /// ANTI-XSRF token was throwing errors ///////////
            /// Not sure of the benefit for this right ////////
            /// now so we're leaving this out /////////////////
            /// DP 09/04/2015 /////////////////////////////////


            //// The code below helps to protect against XSRF attacks
            //var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            //Guid requestCookieGuidValue;
            //if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            //{
            //  // Use the Anti-XSRF token from the cookie
            //  _antiXsrfTokenValue = requestCookie.Value;
            //  Page.ViewStateUserKey = _antiXsrfTokenValue;
            //}
            //else
            //{
            //  // Generate a new Anti-XSRF token and save to the cookie
            //  _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            //  Page.ViewStateUserKey = _antiXsrfTokenValue;

            //  var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            //  {
            //    HttpOnly = true,
            //    Value = _antiXsrfTokenValue
            //  };
            //  if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            //  {
            //    responseCookie.Secure = true;
            //  }
            //  Response.Cookies.Set(responseCookie);
            //}

            //Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //  // Set Anti-XSRF token
            //  ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            //  ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            //}
            //else
            //{
            //  // Validate the Anti-XSRF token
            //  if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
            //      || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            //  {
            //    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            //  }
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            HtmlTextWriter htmlW = new HtmlTextWriter(sw);

            string TempContent = "";
            string Content1 = "";

            int C1Start = 0;
            int c1End = 0;

            string Content2 = "";

            int C2Start = 0;
            int c2End = 0;

            string txt = "";
            string txt2 = "";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            System.Net.WebClient http = new System.Net.WebClient();

            byte[] result = null;

            Uri uri = new Uri("https://www.workforcesafety.com/template", UriKind.Absolute);
            result = http.DownloadData(uri);
            // Save wrapper page html to string

            txt = Encoding.Default.GetString(result);
            txt = txt.Replace("href=\"/", "href=\"https://www.workforcesafety.com/");
            txt = txt.Replace("src=\"/", "src=\"https://www.workforcesafety.com/");
            txt = txt.Replace("<span class=\"field field--name-title field--type-string field--label-hidden\">template</span>", "<span class=\"field field--name-title field--type-string field--label-hidden\">" + Page.Title + "</span>");

            txt = txt.Replace("<title>template | North Dakota Workforce Safety &amp; Insurance</title>", "<title>" + Page.Title + " | North Dakota Workforce Safety &amp; Insurance</title>");
            txt = txt.Replace("<!-- style sheet begins here -->", "<!-- style sheet begins here -->\n" + "<link rel=\"stylesheet\" media=\"all\" href=\"Content/all.css\">");
            txt = txt.Replace("<meta charset=\"utf-8\" />", "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=9; IE=edge\" /> \n<meta charset =\"utf-8\" />");
            txt = txt.Replace("Â", "");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(txt);
            try
            {
                // Add the page name to the Breadcrumbs
                doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[4]/div[1]/section[1]/div[1]/section[1]/nav[1]/ol[1]/li[2]").InnerHtml = Page.Title;
            }
            catch 
            { }
            txt = doc.DocumentNode.OuterHtml;




            base.Render(htmlW);

            // Content begins and ends comment lines found in html in master page
            TempContent = sb.ToString();
            C1Start = TempContent.IndexOf("<!-- Add Application Code here -->");
            c1End = TempContent.IndexOf("<!-- Application code ends here -->");
            Content1 = TempContent.Substring(C1Start + 34, c1End - C1Start - 34);

            C2Start = TempContent.IndexOf("<head>");
            c2End = TempContent.IndexOf("</head>");
            Content2 = TempContent.Substring(C2Start + 6, c2End - C2Start - 6);



            txt2 = txt.Replace("<!-- Add Application Code here -->", Content1);
            txt2 = txt2.Replace("<head>", "<head>\n" + Content2);


            Response.Write(txt2);
        }

    }
}