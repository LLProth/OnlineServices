<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="EmployerSearchResults.aspx.cs" Inherits="OnlineServices.EmployerSearchResults" Title="Employer Search" %>


<asp:Content runat="server" ID="headerStuff" ContentPlaceHolderID="head">
    <title>Employer Search | North Dakota Workforce Safety & Insurance.</title>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:Panel runat="server" ID="panelGridResults" >
        <div align="left">
            <asp:Label runat="server" ID="lblnumberEmployers" /> 
            employer(s) were found matching your search criteria.
        </div>
        <div>
            <asp:GridView name="grdEmployerSearchResult" id = "grdEmployerSearchResult" 
                                runat="server"  BackColor="White" BorderColor="#CCCCCC" 
                                 BorderWidth="1px" CellPadding="3" 
                                AutoGenerateColumns="False" HorizontalAlign="Left" Width="100%"   >
                <RowStyle ForeColor="#000066" />
                <Columns>
                    <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="Business" HeaderText="Business"  >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="Legal Name" HeaderText="Legal Name" HeaderStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="City" HeaderText="City" HeaderStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ST" HeaderText="State" >
                        <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" BorderWidth="1px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Expiration Date" HeaderText="Expiration Date" >
                        <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#2a479d" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelNoResults" Visible="false">
        No records matching your criteria were found. Click the New Search button below to alter your criteria and search again.
    </asp:Panel>


    <asp:Panel runat="server" ID="panelSessionExpired" Visible="false">
        <div style="color:red;margin-top:45px;font-weight: 700;font-size:15pt;">Your session has expired</div>
    </asp:Panel>

    <p></p>
    <div >  
        <p>&nbsp;</p>
        <p style="font-size:12.24px">
            <b>Disclaimer:</b>  
            WSI strives to provide accurate data through all online 
            services. Online data is intended to be informational in nature and will not 
            create any additional liabilities should an inconsistency arise. If you have any 
            concerns about the accuracy of information you see online, please contact Customer Service at 701-328-3800 or 800-777-5033.
        </p>
      
        <p align="center">
            <asp:button runat="server" ID="btnNewSearch"  Text="New Search"
                    OnClick="btnNewSearch_Click"    
            />

        </p> 
    </div>


                
</asp:content>
