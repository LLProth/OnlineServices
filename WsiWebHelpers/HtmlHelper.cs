using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;

namespace WsiWebHelpers
{
  public static class HtmlHelper
  {
    public static string GetLinkFromCommonStore(string linkId, string commonStoreConnectString)
    {
      UrlHelper helper = new UrlHelper(commonStoreConnectString);
      return helper.GetUrl(linkId);
    }

    public static string LinkFromConfiguration(string configKey)
    {
      return ConfigurationManager.AppSettings[configKey].ToString();
    }

    public static string LocalPathFromBase(string pageName)
    {
      string str = ConfigurationManager.AppSettings["ApplicationBasePath"].ToString();
      string str3 = pageName;
      if (str.Length > 0)
      {
        return (str + pageName);
      }
      return str3;
    }

    public static void SetImageSourceFromConfig(HtmlImage img, string srcKey)
    {

      string src = ConfigurationManager.AppSettings[srcKey].ToString();
      img.Src = src;

    }

    public static void SetupLinkForPopupAnchor(HtmlAnchor anchor, string @ref)
    {
      anchor.HRef = @ref;
      string str = string.Format("loadWinPopup('{0}');", @ref);
      anchor.Attributes.Add("OnClick", str);
      anchor.Attributes.Add("OnKeyPress", str);
    }

    public static void SetupLinkForPopupAnchorWithMenu(HtmlAnchor anchor, string @ref)
    {
      anchor.HRef = @ref;
      string str = string.Format("loadWinPopup2('{0}');", @ref);
      anchor.Attributes.Add("OnClick", str);
      anchor.Attributes.Add("OnKeyPress", str);
    }

    public static void SetupLinkFromCommonStoreForPopupAnchor(ref HtmlAnchor anchor, string linkId, string commonStoreConnectString)
    {
      string linkFromCommonStore = GetLinkFromCommonStore(linkId, commonStoreConnectString);
      SetupLinkForPopupAnchor(anchor, linkFromCommonStore);
    }

    public static void SetupLinkFromConfig(HtmlAnchor anchor, string configKey)
    {
      string str = ConfigurationManager.AppSettings[configKey].ToString();
      anchor.HRef = str;
    }

    public static void SetupLinkFromConfigForPopupAnchor(HtmlAnchor anchor, string configKey)
    {
      string str2 = ConfigurationManager.AppSettings[configKey].ToString();
      anchor.HRef = str2;
      string str = string.Format("loadWinPopup('{0}');", str2);
      anchor.Attributes.Add("OnClick", str);
      anchor.Attributes.Add("OnKeyPress", str);
    }

  }
}
