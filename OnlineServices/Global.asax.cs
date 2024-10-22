using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using OnlineServices;
using System.Configuration;
using System.Net.Mail;
using System.Collections;
using System.Runtime.CompilerServices;
using gov.nd.itd.util.logging;

namespace OnlineServices
{
    public class Global : HttpApplication
    {
        public static string AgencyAbbreviation = "WSI";
        public static string LoggerApplicationName = "Online Services";
        public static string LoggerCodeModuleName = "App";
        public static Logger ErrorLogger = new Logger(LoggerApplicationName, LoggerCodeModuleName, AgencyAbbreviation);
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ErrorLogger = new Logger(LoggerApplicationName, LoggerCodeModuleName, AgencyAbbreviation);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception lastException = null;
            string message = string.Empty;
            string emailStatusMessage = string.Empty;
            string caller = Request.Url.ToString();
            string smtpServerUrl = string.Empty;
            string webMasterEmailAddress = string.Empty;

            if (caller.Trim().ToLower().Contains(".aspx"))
            {
                //Log all errors in ITD Logging Utility
                Exception currentException = new Exception("", Server.GetLastError());
                ErrorLogger.LogError(currentException);

                // try to get last exception
                try
                {
                    lastException = Server.GetLastError();
                    message = makeErrorMessage(lastException);
                }
                catch (Exception)
                {
                    lastException = new ApplicationException("Could not retrieve exeption from Server.GetLastError.");
                    message = "No Additional Information is Available.";
                }

                // get smtp info from config for emailing details
                try
                {
                    smtpServerUrl = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                    webMasterEmailAddress = ConfigurationManager.AppSettings["Web_Master_Email_Address"].ToString();
                }
                catch (Exception)
                {
                    emailStatusMessage = "Unable to log exception details.  Unable to read log information from configuration.";
                }

                // try to send notification email
                if (emailStatusMessage.Length == 0)
                {
                    try
                    {
                        mailException(smtpServerUrl, webMasterEmailAddress, lastException);
                        emailStatusMessage = "Exception Details have been emailed to the WSI Web Master for review.";
                    }
                    catch (Exception)
                    {
                        emailStatusMessage = "Unable to log exception details.  SMTP failed.";
                    }




                    ////////////////////////////////////////////////////////////////////////
                    ////// Passing the error message as a string through the request is ////
                    ////// causing the potentially dangerous Request.QueryString error /////
                    ////// DP 09/21/2015 - REPLACED CODE ///////////////////////////////////
                    //Dim exc As Exception = Server.GetLastError()
                    //If TypeOf exc Is HttpUnhandledException Then
                    //    If Not exc.InnerException Is Nothing Then
                    //        exc = New Exception(exc.InnerException.Message.ToString())
                    //        'Server.ClearError()  '-- need to clear the error to tell IIS we've handled it - otherwise will be handled again at server level
                    //        Server.Transfer("oprException.aspx?handler=Application_Error%20-%20Global.asax&emailMessage=" & emailStatusMessage, True)
                    //    End If
                    //End If




                    Exception exc = Server.GetLastError();
                    if (exc is HttpUnhandledException)
                    {
                        if ((exc.InnerException != null))
                        {
                            exc = new Exception(exc.InnerException.Message.ToString());
                            //Server.ClearError()  '-- need to clear the error to tell IIS we've handled it - otherwise will be handled again at server level
                            Server.Transfer("onlineServicesEx.aspx?handler=Application_Error%20-%20Global.asax&emailMessage=" + emailStatusMessage, true);
                        }
                    }




                    /******************************************************/
                    /*********** ORIGINAL CODE ****************************/
                    //string redirectUrl = string.Format("onlineServicesEx.aspx?message={0}&emailMessage={1}",
                    //encodeMessage(message),
                    //emailStatusMessage
                    //);
                    //Server.ClearError();
                    //Response.Redirect(redirectUrl);
                    /*********** ORIGINAL CODE ****************************/
                    /******************************************************/




                    ////// Passing the error message as a string through the request is ////
                    ////// causing the potentially dangerous Request.QueryString error /////
                    ////// DP 09/21/2015 - REPLACED CODE ///////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////








                }
            }
        }

        private string browserInfo()
        {
            string result = string.Empty;
            try
            {
                HttpBrowserCapabilities bc = Request.Browser;
                result = string.Format("Browser Information:{0}Type:{1}  Name:{2}  Version:{3}{0}Major Version:{4}  Minor Version:{5}", Environment.NewLine, bc.Type, bc.Browser, bc.Version, bc.MajorVersion, bc.MinorVersion);

            }
            catch (Exception)
            {
                result = "Browser Information was not available.";
            }
            return result;
        }

        private string encodeMessage(string message)
        {
            return Server.UrlEncode(message);
        }


        private void mailException(string smtpServerUrl, string webMasterEmailAddress, Exception lastException)
        {

            SmtpClient mailclient = new SmtpClient(smtpServerUrl);
            MailMessage message = new MailMessage();
            message.To.Add(webMasterEmailAddress);
            message.Subject = "Online Services Web Page Exception Details";
            MailAddress fromAddress = new MailAddress(webMasterEmailAddress);
            message.From = fromAddress;
            message.Body = string.Format("{0}{1}{2}", makeErrorMessageForLogging(lastException), Environment.NewLine, browserInfo());
            message.IsBodyHtml = false;
            mailclient.Send(message);

        }

        private string makeErrorMessageForLogging(Exception lastEx)
        {
            string resultString = string.Empty;
            if (lastEx.InnerException != null)
            {
                resultString = this.makeErrorMessageForLogging(lastEx.InnerException);
            }

            resultString = string.Format("{0}{1}{2}{1}{3}{1}----------------------------------", new object[] { resultString, "\r\n", lastEx.Message, lastEx.StackTrace });
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
                            resultString = resultString + this.stringFromArray(RuntimeHelpers.GetObjectValue(x.Value)) + "\r\n";
                        }
                        if (x.Key.ToString() == "parmValues")
                        {
                            resultString = resultString + this.stringFromArray(RuntimeHelpers.GetObjectValue(x.Value)) + "\r\n";
                        }
                    }
                }
            }
            return ((resultString + "\r\n") + "----------------------------------" + "\r\n");
        }

        private string stringFromArray(object arrayToShowObject)
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


        private string makeErrorMessage(Exception lastException)
        {
            string resultString = string.Empty;

            // show inner exception details first
            if (lastException.InnerException != null)
            {
                resultString = makeErrorMessage(lastException.InnerException);
            }

            if (lastException.GetType().FullName == "System.Data.OracleClient.OracleException")
            {
                resultString = string.Format("{0}{1}{2}", resultString, Environment.NewLine, "An Exception Occured While Accessing Database");

            }
            else if (lastException.GetType().FullName == "oFroiDAL.OfroiPersistException")
            {
                resultString = string.Format("{0}{1}{2}", resultString, Environment.NewLine, "An Exception Occured While Accessing Database");
            }
            else if (lastException.GetType().FullName != "System.Web.HttpUnhandledException")
            {
                resultString = string.Format("{0}{1}{2}", resultString, Environment.NewLine, lastException.Message);
            }
            return resultString;

        }

    }
}
