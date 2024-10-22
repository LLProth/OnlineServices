<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployerSearch.aspx.cs" Inherits="OnlineServices.EmployerSearch" Title="Employer Search"%>


<asp:Content runat="server" ID="headerStuff" ContentPlaceHolderID="head">
    <title>Employer Search | North Dakota Workforce Safety & Insurance.</title>
</asp:Content>
    
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">



    
    By completing one or more of the search fields below, this search will identify whether an employer matching the
    search criteria has an open policy with Workforce Safety Insurance.  Only employers that match all of the search
    criteria entered will be returned.  Please note, a partial Business Name and/or partial Legal Name may be entered.
    Please check the
    <A title="This link opens a new window" class="text" onclick="loadWinPopup('EmployerSearchFAQ.aspx');" onkeypress="loadWinPopup('EmployerSearchFAQ.aspx');"
	        target="newPopup" href="EmployerSearchFAQ.aspx">
        FAQ
    </A> 
    page for help with the Employer Search.

    <div >
        <table class="main" style="font-size:0.9em" >
            <tr runat="server" id="errorMessageRow" visible="false">
                <td colspan="2">
                    &nbsp;<asp:Label ID="errorMessage" runat="server" Font-Bold="True" Text="* Please complete one field on this page before performing a search."></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="aspLabel"  >
                    <label for="txtfein">FEIN:</label>
                </td>
                <td class="aspTextbox" >
                    <asp:TextBox runat="server" tabindex="1" name="txtfein" id="txtfein" value='' maxlength="9" width="40%" />
      	            &nbsp;&nbsp;<span>Example: 999999999</span>
                </td>
            </tr>

            <tr>
    	        <td  class="aspLabel" >
    		        <label for="businessName">Business Name:</label>
    	        </td>
    	        <td align="left">
    		        <asp:TextBox runat="server" tabindex="2" name="txtbusinessName" id="txtbusinessName" value='' maxlength="40"  Width="50%" />
    	        </td>
            </tr>

            <tr>
    	        <td class="aspLabel" >
    		        <label for="legalName">Legal Name / Doing Business As / Trading As:</label>
    	        </td>
    	        <td align="left">
    		        <asp:TextBox runat="server" tabindex="3" name="txtlegalName" id="txtlegalName" value='' maxlength="40" Width="50%" />
    	        </td>
            </tr>

            <tr>
    	        <td class="aspLabel" >
    		        <label for="city">City:</label>
    	        </td>
    	        <td align="left">
    		        <asp:TextBox runat="server" tabindex="4" name="txtcity" id="txtcity" value='' maxlength="25" Width="40%" />
    	        </td>
            </tr>

            <tr>
    	        <td class="aspLabel" >
    		        <label for="state">State:</label>
    	        </td>
    	        <td align="left">
    		        <asp:TextBox runat="server"  tabindex="5" name="txtstate" id="txtstate" value='' maxlength="2" Width="30%" />
    	        </td>
            </tr>

            <tr>
    	        <td>
    		        &nbsp;&nbsp;
    	        </td>
    	        <td>
    		        &nbsp;&nbsp;
    	        </td>
            </tr>

            <tr>
                <td colspan="2">
                    <b>Disclaimer:</b>  WSI strives to provide accurate data through all online services.  Online data is intended to be 
                    informational in nature and will not create any additional liabilities should an inconsistency arise.  If you have 
                    any concerns about the accuracy of information you see online, please contact Customer Service at 701-328-3800 or 800-777-5033. 
                </td>
            </tr>
            <tr>
    	        <td>
    		        &nbsp;&nbsp;
    	        </td>
    	        <td>
    		        &nbsp;&nbsp;
    	        </td>
            </tr >
        </table>
    </div>
    <div style="text-align:center" >
        <asp:button runat="server" tabindex="6" value="Search" text="Search"
            width="70px" name="search" ID="btnSearch"
            OnClick="btnSearch_Click"/>
        &nbsp;&nbsp; &nbsp;&nbsp;
        <asp:button runat="server" tabindex="7" name="clear" ID="btnClear" text="Clear" width="70px" 
            value="clear" 
            OnClick="btnClear_Click"  />
    </div>       
</asp:Content>
