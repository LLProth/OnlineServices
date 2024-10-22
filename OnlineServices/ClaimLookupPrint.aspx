<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClaimLookupPrint.aspx.cs" Inherits="OnlineServices.ClaimLookupPrint" Title="Claim Lookup"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Claim Lookup | North Dakota Workforce Safety & Insurance.</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <img src="Images/wsi-logo.png"   alt="Workforce Safety &amp; Insurance Logo"   border="0" id="Image21" />
     <asp:Panel ID="panelResults" runat="server" Visible="true">
                  <h1 align="left" class="seperator-safety">Claim Lookup Results</h1>
                  <asp:Label runat="server" ID="lblDate" />
                  <div >
                    <p align="left">The following claim information was found for the
                      <asp:Label ID="lblLookupCriteria" runat="server"></asp:Label>
                      . The primary body part injured for each claim is indicated by an asterisk&nbsp;(<span class='loud'>*</span>).
                    </p>
                    <asp:GridView runat="server" ID="resultGrid" name="resultGrid" 
                      AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                      BorderStyle="None" BorderWidth="1px" CellPadding="3" >
                      <RowStyle ForeColor="#000066" />
                      <Columns>
                        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                        <asp:BoundField DataField="InjuryDate" HeaderText="Date of Injury" />
                        <asp:BoundField DataField="Employer" HeaderText="Employer" />
                        <asp:BoundField DataField="ClaimNumber" HeaderText="Claim Number" />
                        <asp:BoundField DataField="ClaimStatus" HeaderText="Claim Status" />
                        <asp:BoundField DataField="BodyPart" HeaderText="Body Part" HtmlEncode="false" />
                      </Columns>
                      <FooterStyle BackColor="White" ForeColor="#000066" />
                      <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                      <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                      <HeaderStyle BackColor="#99ccff" Font-Bold="True" ForeColor="Black" />
                    </asp:GridView>
                    <p>
                      <b>Disclaimer:</b>  WSI strives to provide accurate data through all online 
                      services. Online data is intended to be informational in nature and will not 
                      create any additional liabilities should an inconsistency arise. If you have any 
                      concerns about the accuracy of information you see online, please contact Customer Service at 701-328-3800 or 800-777-5033.
                    </p>
                    <p align="center">
                      &nbsp;
                      </p>
                  </div>
                </asp:Panel>
    </div>
    </form>
    <script>window.print();</script>
</body>
</html>
