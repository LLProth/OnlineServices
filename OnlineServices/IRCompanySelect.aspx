<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IRCompanySelect.aspx.cs" Inherits="OnlineServices.IRCompanySelect" Title="Incident Report" %>
 

<asp:Content runat="server" ID="headerStuff" ContentPlaceHolderID="head">
    <title>Incident Report | North Dakota Workforce Safety & Insurance.</title>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    

    <div style="padding-bottom:15px">
        <p>
            If a worker was injured within the last or current business day and medical 
            attention is <u>not</u> being sought, provide information regarding that 
            incident below.

        </p>
        <p>
            If a worker was injured and medical attention is being sought, please file a 
            claim using the new
            <a runat="server" id="linkFroi"  name="ofroi" target="newPopup">
                Claim Filing/First Report of Injury
            </a>
            immediately, instead of this incident report.
        </p>
    </div>
    <div id="searchCriteriaSection">
        <!-- Search Criteria Entry Start -->
        <table>
            <tr>
                <td>
                <asp:Label runat="server" ID="lblCompanyName" Text="Company Name">Company Name:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCompanyName" TabIndex="0" MaxLength="25"></asp:TextBox>
                </td> 
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblCompanyAccountNumber" Text="Account Number">Company Account Number:</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCompanyAccountNumber" MaxLength="8"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                          
                </td>
                <td>
                <asp:Button runat="server" ID="btnSearch" Text="Search" 
                    OnClientClick="removeButtonsForProcessing()" 
                    OnClick="btnSearch_Click1" CssClass="btnOLNS" />
                <h4>
                    <asp:Label runat="server" ID="lblResultsMessage" align="center" ForeColor="Red">
                    </asp:Label>
                </h4>
                </td>
            </tr>
            <tr align="center" >
                <td colspan="3" align="center" id="waitMessageRow" runat="server">
                            
                </td>
            </tr>
        </table>
    
        <div style="vertical-align:top; height:300px; overflow:auto" runat="server" id="searchResultsDiv" width="738">
            <asp:datagrid id="grdSearchResults" runat="server" Visible="False" 
                BackColor="White" ForeColor="#000066"  
                BorderColor="#CCCCCC" BorderWidth="1px"
                AutoGenerateColumns="False"  
                CellPadding="3" 
                Width="100%" 
                OnItemCommand="grdSearchResults_ItemCommand"
                
                >
                <HeaderStyle BackColor="#2a479d" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:ButtonColumn  ButtonType="PushButton" Text="Select" CommandName="selectButton">
                        <ItemStyle  HorizontalAlign="Center" VerticalAlign="Middle" BorderColor="#CCCCCC" BorderWidth="1px"  CssClass="btnOLNS" />
                    </asp:ButtonColumn>
                    <asp:BoundColumn DataField="ACCT_NO" ReadOnly="true" HeaderText="Account" Visible="False" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="BUSN_NM" ReadOnly="true" HeaderText="Business Name" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="City" ReadOnly="true" HeaderText="City" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="stt" ReadOnly="true" HeaderText="State" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundColumn>
                </Columns>
            </asp:datagrid>
         </div> 
        
        <div style="text-align:center" >
            <asp:Label runat="server" ForeColor="Red" ID="lblSearchErrorMsg" Text="" Visible="false" />
        </div>
        <p>

        </p>





        <%--This is for testing--%>
        <div runat="server" id="TestEx">
        <asp:Button runat="server" ID="ThrowEx" Text="Throw Ex" OnClick="showEx_Click" />
        </div>        
        <%--This is for testing--%>


    </div>

</asp:Content>
