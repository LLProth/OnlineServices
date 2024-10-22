using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineServices
{
  public partial class OnlineServicesEx : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {


      /////////////////////////////////////////////////
      /// Original error handling code ////////////////
      //if (Request.QueryString["Message"] == null)
      //{
      //  MessageText.Text = "No exception details were provided.";
      //}
      //else
      //{
      //  MessageText.Text = Request.QueryString["Message"].ToString();
      //}
      /// Original error handling code ////////////////
      /////////////////////////////////////////////////
      
          //Determine where error was handled
          string errorHandler = Request.QueryString["handler"];
          if (errorHandler == null)
          {
              errorHandler = "Error Page";
          }

          //Get last error from the server
          Exception exc = Server.GetLastError();
          string MessageStr = "A problem has occurred on this web site. Please try again. ";
          Exception lastEx = default(Exception);

          if ((exc != null))
          {
              //MessageText.Text = "Account Number: " & acctNo & vbCrLf & UrlDecode(Request.QueryString("Message"))
              try
              {
                  lastEx = Server.GetLastError();
                  MessageStr = MakeErrorMessage(lastEx, errorHandler);
              }
              catch (Exception ex)
              {
                  lastEx = new ApplicationException("Could not retrieve exeption from Server.GetLastError.");
                  MessageStr = "No Additional Information is Available.";
              }
          }
          MessageText.Text = MessageStr;


      if (Request.QueryString["EmailMessage"] == null)
      {
        MessageText.Text = "Unable to notify WSI Web Master of Exception.";
      }
      else
      {
        EmailStatusLabel.Text = Request.QueryString["EmailMessage"].ToString();
      }

      if (Session.Count > 0)
        Session.Clear();
    
    }




    //' <summary>
    // MakeErrorMessage function was moved from the global.asax. The error message is no longer being
    // passed through the request. MakeErrorMessage returns a generic error message for the user and
    // a detailed message if the showerrors session has been set.
    // </summary>
    private string MakeErrorMessage(Exception lastEx, string errorHandlerStr)
    {
        string resultString = string.Empty;
        if ((lastEx.InnerException != null))
        {
            resultString = MakeErrorMessage(lastEx.InnerException, errorHandlerStr);
        }

        if (lastEx.GetType().FullName == "System.Data.OracleClient.OracleException")
        {
            resultString = string.Format("{0}{1}{2}", resultString, System.Environment.NewLine, "An Exception While Accessing Database");
        }
        else if (lastEx.GetType().FullName != "System.Web.HttpUnhandledException")
        {
            if (Session["showerrors"] == "true")
            {
                //If the session is set then display a detailed error
                resultString = string.Format("{0}{1}{2}", resultString, System.Environment.NewLine, lastEx.Message);

                DetailedErrorPanel.Visible = true;
                ErrorHandler.Text = errorHandlerStr;
                ErrorDetailedMsg.Text = lastEx.Message;

                if ((lastEx.InnerException != null))
                {
                    InnerMessage.Text = lastEx.GetType().ToString() + "<br />" + lastEx.InnerException.Message;
                    InnerTrace.Text = lastEx.InnerException.StackTrace;
                }
                else
                {
                    InnerMessage.Text = lastEx.GetType().ToString();
                    if ((lastEx.StackTrace != null))
                    {
                        InnerTrace.Text = lastEx.StackTrace.ToString().TrimStart();
                    }
                }
            }
            else
            {
                resultString = "An exception has occurred";
            }
        }
        return resultString;
    }





  }
}