using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineServices
{
  public class htmTemplateHelper
  {

    private string _templateString = string.Empty;

    public htmTemplateHelper(string templateFileName)
    {
      if (System.IO.File.Exists(templateFileName))
      {
        _templateString = System.IO.File.ReadAllText(templateFileName);
      }
    }

    public string Template
    {
      get
      {
        return _templateString;
      }
    }

    public bool ReplaceTag(string tag, string value)
    {

      if (!tag.StartsWith(@"{{"))
        tag = "{{" + tag.Trim() + "}}";

      _templateString = _templateString.Replace(tag, value);
      return true;
    }

    public bool ReplaceTags(string[] tagValueArray)
    {

      for (int n = 0; n< tagValueArray.Length; n = n + 2)
      {
        ReplaceTag(tagValueArray[n], tagValueArray[n + 1]);
      }
      return true;

    }
  
  
  
  }
}