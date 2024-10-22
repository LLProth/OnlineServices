<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployerSearchFAQ.aspx.cs" Inherits="OnlineServices.EmployerSearchFAQ" Title="Employer Search"%>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
  <head>
    <title>Employer Searach FAQs</title>
    <link rel="stylesheet" type="text/css" href="/includes/wsi.css"/>
     <script src="Scripts/ocl.js" type= "text/javascript"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
  </head>
<body class="color">
<blockquote>
<h1>Frequently Asked Questions (FAQs)</h1>
<dl>
<p>
    <dt><strong>How can I find the FEIN (Federal Employer Identification Number) for an employer?</strong></dt>
        <dd><br/>Contact the policyholder/employer for whom you are verifying coverage.</dd>
</p>
<p>
    <dt><strong>If no match is found on the FEIN entered, does this indicate the employer I am searching for does not have an open account with Workforce&nbsp;Saftey&nbsp;&amp;&nbsp;Insurance?</strong></dt>
        <dd><br/>Not necessarily:</dd>
</p>
        <dd>
	        <ul>
		        <li>WSI may not have a FEIN on file for the employer you are searching for; or</li>
		        <li>WSI may have an incorrect FEIN on file for the employer you are searching for; or</li>
		        <li>You may have entered an incorrect FEIN</li>
	        </ul>
            <p>
                If you do not find a match for the FEIN you entered, you may want to contact WSI Customer Service.
            </p>
        </dd>
<p>
    <dt><strong>What does the "Business Name" field on the Search window represent?</strong></dt>
        <dd><br/>You may enter the Business Name or partial Business Name of the policyholder/employer you are searching for.</dd>
</p>
<p>
    <dt><strong>What does the "Legal Name / Doing Business As / Trading As" field on the Search window represent?</strong></dt>
        <dd><br/>Often times, a policyholder/employer will have a Business Name, but operate under another name. You may enter the Legal Name, a Doing Business As Name, or a Trading As Name, or a partial name for the policyholder/employer you are searching for.</dd>
</p>
<p>
    <dt><strong>What does the "City" field on the Search window represent?</strong></dt>
        <dd><br/>As part of the search, you may enter the name of the City where a policyholder/employer maintains a business address. If City is entered, the search will only return entries which match exactly.</dd>
</p>
<p>
    <dt><strong>What does the "State" field on the Search window represent?</strong></dt>
        <dd><br/>As part of the search, you may enter the name of the State in which a policyholder/employer maintains a business address. If State is entered, the search will only return entries which match exactly.</dd>
</p>
<p>
    <dt><strong>Are there any guidelines or restrictions for entering 'search' criteria in the Search window?</strong></dt>
        <dd><br/>You must enter data in at least one of the search criteria fields, but may enter data in multiple, including all, search criteria fields.</dd>
        <dd><br/>If multiple search criteria fields are entered, the search will only return those policyholders/employers that match on all of the search criteria entered.</dd>
        <dd><br/>If FEIN is entered, an exact match must be found or no entries will be returned.</dd>
        <dd><br/>The search will return a maximum of ten (10) policyholders/employers that match the search criteria entered. If this occurs, narrow your search by entering additional search criteria and resubmit the search.</dd>
        <dd><br/>You are allowed to enter a partial Business Name and/or Legal Name. If three or less characters are entered, the results will include those employers whose Business Name or Legal Name <b>starts</b> with this entry.  If more than three characters are entered, the results will include those employers in which the entry is included <b>anywhere</b> within the Business Name or Legal Name.  The more characters you enter in these search fields, the narrower your search will be.</dd>
        <dd><br/>In most cases, it is beneficial to enter Business Name or Legal Name, rather than both, since the search will return only policyholders/employers that match on all the criteria entered.</dd>
</p>
<p>
    <dt><strong>What does the "Business Name" column on the output display window represent?</strong></dt>
        <dd><br/>This is the Business Name of the policyholder/employer that WSI has on file.</dd>
</p>
<p>
    <dt><strong>What does the "Legal Name" column on the output display window represent?</strong></dt>
        <dd><br/>This is the Legal Name, Doing Business As Name, Trade Name, or continuation of the Business Name of the policyholder/employer that WSI has on file. The majority of policyholders/employers do not have data in this field.</dd>
</p>
<p>
    <dt><strong>What does the "City" column on the output display window represent?</strong></dt>
        <dd><br/>This is the City that WSI has on file, in which a policyholder/employer maintains a primary mailing address.</dd>
</p>
<p>
    <dt><strong>What does the "State" column on the output display window represent?</strong></dt>
        <dd><br/>This is the State that WSI has on file, in which a policyholder/employer maintains a primary mailing address.</dd>
</p>
<p>
    <dt><strong>What does the "Expiration Date" column on the output display indicate?</strong></dt>
        <dd><br/>This date identifies when the employer's workers compensation coverage with Workforce&nbsp;Safety&nbsp;&amp;&nbsp;Insurance is set to expire.  Upon policy renewal and payment of the initial premium installment, a new policy expiration date will be established.  If no date is indicated, you may want to contact WSI Customer Service.</dd>
</p>
<br/>
</blockquote>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button type="button" name="close" value="close" onclick="closeWndw();" onkeypress="closeWndw();">Close Window</button>
</body>
</html>
