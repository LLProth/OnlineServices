<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DebugPage.aspx.cs" Inherits="OnlineServices.DebugPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <p><asp:Label Text="App Links:" runat="server"/></p>
        <p><a href="ClaimLookup.aspx"> Claim Lookup</a></p>
        <p><a href="EmployerSearch.aspx"> Employer Search</a></p>
        <p><a href="IRCompanySelect.aspx"> Incident Report</a></p>
    </div>
</asp:Content>
