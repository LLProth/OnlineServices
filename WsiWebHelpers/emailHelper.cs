using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace WsiWebHelpers
{
  public static class emailHelper
  {

    public static void mailException(string smtpServerUrl, string webMasterEmailAddress, Exception lastException, HttpRequest rq)
    {
      SmtpClient mailclient = new SmtpClient(smtpServerUrl);
      MailMessage message = new MailMessage();
      message.To.Add(webMasterEmailAddress);
      message.Subject = "Online Services Web Page Exception Details";
      MailAddress fromAddress = new MailAddress(webMasterEmailAddress);
      message.From = fromAddress;
      message.Body = string.Format("{0}{1}{2}", makeErrorMessageForLogging(lastException), Environment.NewLine, browserInfo(rq));
      message.IsBodyHtml = false;
      mailclient.Send(message);
    }


    public static string makeErrorMessageForLogging(Exception lastEx)
    {
      string resultString = string.Empty;
      if (lastEx.InnerException != null)
      {
        resultString = makeErrorMessageForLogging(lastEx.InnerException);
      }

      resultString = string.Format("{0}{1}{2}{1}{3}{1}{4}{1}----------------------------------", resultString, "\r\n", lastEx.GetType(), lastEx.Message, lastEx.StackTrace );
      ApplicationException appEx = lastEx as ApplicationException;
      if (appEx != null)
      {
        if (appEx.Data.Count > 0)
        {
          resultString = string.Format("{1}{0}AdditionalData:{0}", "\r\n", resultString);
          foreach (DictionaryEntry x in appEx.Data)
          {

            resultString = string.Format("{0}{1}: {2}{3}", new object[] { resultString, x.Key.ToString(), x.Value.ToString(), "\r\n" });
            if (x.Key.ToString() == "parmNames")
            {
              resultString = resultString + stringFromArray(RuntimeHelpers.GetObjectValue(x.Value)) + "\r\n";
            }
            if (x.Key.ToString() == "parmValues")
            {
              resultString = resultString + stringFromArray(RuntimeHelpers.GetObjectValue(x.Value)) + "\r\n";
            }
          }
        }
      }
      return ((resultString + "\r\n") + "----------------------------------" + "\r\n");
    }

    private static string stringFromArray(object arrayToShowObject) 
    {
      object[] myArray = arrayToShowObject as object[];
      string returnVal = string.Empty;
      if (myArray != null)
      {
        foreach (object arrayValue in myArray)
        {
          if (arrayValue == null)
          {
            returnVal = returnVal + " [NULL] ";
          }
          else
          {
            returnVal = returnVal + string.Format("'{0}' ", arrayValue.ToString());
          }
        }
      }
      return returnVal;
    }

    private static string browserInfo(HttpRequest Request)
    {
      string result = string.Empty;
      try
      {
        HttpBrowserCapabilities bc = Request.Browser;
        result = string.Format("Browser Information:{0}Type:{1}  Name:{2}  Version:{3}{0}Major Version:{4}  Minor Version:{5}", Environment.NewLine, bc.Type, bc.Browser, bc.Version, bc.MajorVersion, bc.MinorVersion);

      }
      catch (Exception)
      {
        result = "Browswer Information was not available.";
      }
      return result;
    }


  }
}
