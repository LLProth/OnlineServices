<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IRConfirmationDocument.aspx.cs" Inherits="OnlineServices.IRConfirmationDocument" Title="Incident Report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
 <head>
        <link rel="stylesheet" type="text/css" href="./includes/wsi.css" />
        <title>WSI: Online Services - Confirmation of Incident Report</title>

        <!-- Beginning of Head content -->
        <style type="text/css">
            .rptData {
	            font-family: Verdana, Arial, Helvetica, sans-serif;
	            font-size: 0.7em;
	            font-style: normal;
	            font-weight: bold;
	            font-variant: normal;
	            color: #000000;
	            line-height: 125%;
                }
        </style>
    </head>

    <body>
        <div  align="left">
        <h1>Incident Report Confirmation</h1>
            <p align="left">Your incident report was received. Here is what was submitted:</p>
            <blockquote>
             <h2><u>Worker Information</u></h2>
                 First Name: <span runat="server" id="spanFirstName" class="rptData"> </span><br/>
                 Middle Initial: <span runat="server" id="spanMI" class="rptData"> </span><br/>
                 Last Name: <span runat="server" id="spanLastName" class="rptData"> </span><br/>
                 Social Security Number: <span runat="server" id="spanSsn" class="rptData"> </span><br/>
                 Birthdate: <span runat="server" id="spanBirthDate" class="rptData"> </span>
             <br/><h2><u>Injury Information</u></h2>
                 Date of Injury: <span runat="server" id="spanDateOfInjury" class="rptData"> </span><br/>
                 Body Part Injured: <span runat="server" id="spanBodyPartInjured" class="rptData"> </span><br/>
                 Location of Body Part Injured: <span runat="server" id="spanLocationOfBodyPartInjured" class="rptData"> </span><br/>
                 Nature of Injury: <span runat="server" id="spanNatureOfInjury" class="rptData"> </span><br/>
                 Describe clearly how the accident occurred: <span runat="server" id="spanDescriptionOfAccident" class="rptData"> </span>
             <br/><h2><u>Employer Information</u></h2>
                
                 
                 Business Name: <span runat="server" id="spanBusinessName"  class="rptData"> </span><br/>
                 This report is submitted by: <span runat="server" id="spanSubmittedByName" class="rptData"> </span>
            </blockquote>

            <p align="left">Submitted Date and Time was: <span runat="server" id="spanSubmittedDateTime" class="rptData"> </span></p>

           

        </div>
    </body>
</html>
