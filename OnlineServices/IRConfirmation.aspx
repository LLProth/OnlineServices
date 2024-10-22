<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IRConfirmation.aspx.cs" Inherits="OnlineServices.IRConfirmation" Title="Incident Report" %>


<asp:Content runat="server" ID="headerStuff" ContentPlaceHolderID="head">
    <title>Incident Report | North Dakota Workforce Safety & Insurance.</title>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <h2>Incident Report Confirmation</h2>
    <p align="left">Your incident report was received. Here is what was submitted:</p>
    <blockquote>
        <h2>
            <u>Worker Information</u>
        </h2>
        First Name: 
        <span runat="server" id="spanFirstName" class="rptData"> </span><br/>
        Middle Initial: 
        <span runat="server" id="spanMI" class="rptData"> </span><br/>
        Last Name: 
        <span runat="server" id="spanLastName" class="rptData"> </span><br/>
        Social Security Number: 
        <span runat="server" id="spanSsn" class="rptData"> </span><br/>
        Birthdate: 
        <span runat="server" id="spanBirthDate" class="rptData"> </span>
        <br/>
        
        <h2><u>Injury Information</u></h2>
        Date of Injury: 
        <span runat="server" id="spanDateOfInjury" class="rptData"> </span><br/>
        Body Part(s) Injured: 
        <span runat="server" id="spanBodyPartInjured" class="rptData"> </span><br/>
        
        
        
        <div style="display:none;">
        Location of Body Part Injured:
        <span runat="server" id="spanLocationOfBodyPartInjured" class="rptData"> </span><br/>
        </div> 


        Nature of Injury: 
        <span runat="server" id="spanNatureOfInjury" class="rptData"> </span><br/>
        Describe clearly how the accident occurred: 
        <span runat="server" id="spanDescriptionOfAccident" class="rptData"> </span><br/>
        
        <h2><u>Employer Information</u></h2>
        Business Name: 
        <span runat="server" id="spanBusinessName"  class="rptData"> </span><br/>
        This report is submitted by: <span runat="server" id="spanSubmittedByName" class="rptData"> </span>
    </blockquote>
    
    <p align="left">
        Submitted Date and Time was: 
        <span runat="server" id="spanSubmittedDateTime" class="rptData"> </span>
    </p>
    <p align="left">
        Please note:  WSI will pay the $250.00 medical assessment if: 1) The incident report is filed on WSI&#8217;s website by midnight (central time) of the following business day and 2) A claim is received at WSI within 14 calendar days of the date of injury.</p>
    <p align="left">
        If this injury warrants medical care,  
        <a runat="server" id="anchorOFROI" name="ocf" href="">
            click here to complete a First&nbsp;Report&nbsp;of&nbsp;Injury
        </a>.
    </p>
    <p align="left">If you need to report another incident, <a href="irentry.aspx" title="link to a blank Incident Report">click here to complete another Incident Report</a>.</p>
          

</asp:Content>
