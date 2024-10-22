<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="OnlineServicesEx.aspx.cs" Inherits="OnlineServices.OnlineServicesEx" Title="Online Services Exception"%>

<asp:Content runat="server" ID="headerStuff" ContentPlaceHolderID="head">
    <title>Online Services Exception Report | North Dakota Workforce Safety & Insurance.</title>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>
        An Unexpected Exception Occurred while processing your request.
    </h2>
    <div style="padding-bottom:15px">
        <p>
            <b>Attention: </b>
            An unexpected error occurred processing your request.  
        </p>
        <p>
            Please call our Customer Service support line or send e-mail to
             <a style="color:white" href="mailto:wsiwebmaster@nd.gov" class="mailto">
                 WSI Web Master
             </a>
            to report your need for assistance.  We will work on resolving your issue immediately.
            You will need to provide us with the error information listed below. 
        </p>
        <p>
            <asp:TextBox runat="server" id="MessageText" ReadOnly="true" CssClass="data" Width="70%"   TextMode="MultiLine" Height="175px" />

        </p>
        <p>
            <asp:Label runat="server" ID="EmailStatusLabel" CssClass="data" />
        </p>
    </div>



    <asp:Panel ID="DetailedErrorPanel" runat="server" Visible="false">
        <p>&nbsp;</p>
        <h4>Detailed Error:</h4>
        <p>
            <asp:Label ID="ErrorDetailedMsg" runat="server" Font-Size="Small" /><br />
        </p>

        <h4>Error Handler:</h4>
        <p>
            <asp:Label ID="ErrorHandler" runat="server" Font-Size="Small" /><br />
        </p>

        <h4>Detailed Error Message:</h4>
        <p>
            <asp:Label ID="InnerMessage" runat="server" Font-Size="Small" /><br />
        </p>
        <p>
            <asp:Label ID="InnerTrace" runat="server"  />
        </p>
    </asp:Panel>



</asp:Content>
