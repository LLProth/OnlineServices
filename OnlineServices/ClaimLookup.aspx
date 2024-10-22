<%@ Page Title="Claim Lookup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClaimLookup.aspx.cs" Inherits="OnlineServices._Default" %>

<asp:Content runat="server" ID="headerStuff" ContentPlaceHolderID="head">
    <title>Claim Lookup | North Dakota Workforce Safety & Insurance.</title>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    <asp:Panel ID="panelLookupEntry" runat="server">
        
        <p>Provide an Worker's Social Security Number (SSN), Claim Number, or Name and Date of Birth, and Injury Date to retrieve claim verification information.
        </p>
        <p >The following are searches to retrieve claim verification information. 

        </p>
        <ul>
            <li>Use Claim Number only</li>
            <li  >Worker's SSN and Injury Date</li>
            <li>Last Name, First Name and Injury Date</li>
            <li>Last Name, Worker's Date of Birth and Injury Date</li>
        </ul>
        <div runat="server"  >
            <table runat="server" >
                <%--error message row:--%>
                <tr>
                    <td   colspan="2" style="text-align:center;font-size: 1.0em; COLOR: #FF0000;">
                        <asp:Label runat="server" ID="lblErrorMessage"  ></asp:Label>
                    </td>
                </tr>
                <%--claim number row--%>
                <tr>
                    <td class="aspLabel"  style="width:50%">
                        <label for="txtClaimNumber">
                                Claim Number:
                        </label>
                    </td>
                    <td class="aspTextbox" >
                        <asp:TextBox runat="server" tabindex="1" id="txtClaimNumber" 
                                    maxlength="12" Width="50%"  />
      	                &nbsp;&nbsp;<span style="font-style:italic">(no dashes)</span>
                    </td>
                </tr>  
                <%--Injured Worker's SSN row  --%>
                <tr>
                    <td class="aspLabel">
                        <label for="txtSsn">
                            Worker's SSN:
                        </label>
                    </td>
                    <td class="aspTextbox" >
                        <asp:TextBox runat="server" tabindex="2" id="txtSsn"  
                            maxlength="9"  Width="50%"/>
      	                &nbsp;&nbsp;
                        <span style="font-style:italic" >
                            (no dashes)
                        </span>
                    </td>
                </tr>
                <%--Injured Worker's Last Name--%>
                <tr>
                    <td class="aspLabel">
                        <label for="txtLastName">Worker's Last Name:</label>
                    </td>
                    <td class="aspTextbox">
                        <asp:TextBox runat="server" tabindex="3"   id="txtLastName" 
                             maxlength="25" Width="50%"/>
      	                &nbsp;&nbsp;
                    </td>
                </tr>
                <%--Injured workers first name--%>
                <tr>
                    <td class="aspLabel">
                        <label for="txtFirstName">
                            Worker's First Name:
                        </label>
                    </td>
                    <td class="aspTextbox">
                        <asp:TextBox runat="server" tabindex="4"  id="txtFirstName"  maxlength="25" width="50%"/>
      	                &nbsp;&nbsp;
                    </td>
                </tr>
                <%--Injured Worker's Date of Birth--%>
                <tr>
                    <td class="aspLabel">
                        <label for="txtBirthDate">
                            Worker's Date of Birth:
                        </label>                        
                    </td>
                    <td class="aspTextbox">
                        <asp:TextBox runat="server" tabindex="5"   id="txtBirthDate"  
                            maxlength="10" width="50%" />
                        &nbsp;
                        <span style="font-style:italic">  (mm/dd/yyyy)</span>
                    </td>
                </tr>
                <%--Injury Date--%>
                <tr>
                    <td class="aspLabel">
                        <label for="txtInjuryDate">Injury&nbsp;Date:</label>
    	            </td>
    	            <td style="text-align:left">
    		            <asp:TextBox runat="server" tabindex="6" name="injury-date" id="txtInjuryDate"  
                            maxlength="10" Width="50%" />
    		            &nbsp;&nbsp;
                        <span style="font-style:italic">  (mm/dd/yyyy)</span>
    	            </td>
                </tr>
                
                <tr>
                    <td colspan="2" style="text-align:center">
                        <span>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Results will reflect a +/- 5&#8722;day window from the date of injury.
                        </span>
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
                        <b>Disclaimer:</b>  WSI strives to provide accurate data through all online 
                        services. Online data is intended to be informational in nature and will not 
                        create any additional liabilities should an inconsistency arise. If you have any 
                        concerns about the accuracy of information you see online, please contact Customer Service at 701-328-3800 or 800-777-5033.
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
                <tr >
                    <td>
                        &nbsp;&nbsp;
                    </td>
    	            <td id="buttonRow" runat="server">
    	                <asp:button  runat="server" ID="btnSearch" Text="Lookup" 
                            tabindex="6" 
                            OnClientClick="removeButtonsForProcessing()" OnClick="btnSearch_Click" />

        	               <%--   <asp:button runat="server" Text="Clear" type="button" tabindex="7" name="clear" 
                              value="clear" Height="25px" Width="60px" onclick="Unnamed1_Click" /> --%>
                        </td>
                </tr>
                <tr style="text-align:center" >
                    <td colspan="2" style="text-align:center" id="waitMessageRow" runat="server">
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

     <asp:Panel ID="panelNoResults" runat="server" Visible="false" >
       
        <div >
            <p>
                No records matching the 
                <asp:Label runat="server" ID="lblNoResultMessage"    />

                were found. Click the
                <b>New Lookup</b> button below to alter your criteria and try again.
            </p>
            <p>
                <b>Disclaimer:</b>  WSI strives to provide accurate data through all online 
                services. Online data is intended to be informational in nature and will not 
                create any additional liabilities should an inconsistency arise. If you have any 
                concerns about the accuracy of information you see online, please contact Customer Service at 701-328-3800 or 800-777-5033.
            </p>
            <p style="text-align:center">
                <asp:button runat="server" ID="btnNewLookupFromNoData" Text="New Lookup"
                            onclick="btnNewLookupFromNoData_Click"  />
            </p>
        </div>
    </asp:Panel> 
                


    <asp:Panel ID="panelResults" runat="server" Visible="false">
       
        <div >
            <p>The following claim information was found for the
                <asp:Label ID="lblLookupCriteria" runat="server"></asp:Label>
                . The primary body part injured for each claim is indicated by an asterisk&nbsp;(<span class='loud'>*</span>).
            </p>
            <asp:GridView runat="server" ID="resultGrid" name="resultGrid" 
                          AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                          BorderStyle="None" BorderWidth="1px" CellPadding="3" >
                <RowStyle ForeColor="#000066" />
                <Columns>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField >    
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField >    
                    <asp:BoundField DataField="InjuryDate" HeaderText="Date of Injury" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField >    
                    <asp:BoundField DataField="Employer" HeaderText="Employer" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField >    
                    <asp:BoundField DataField="ClaimNumber" HeaderText="Claim Number" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField >    
                    <asp:BoundField DataField="ClaimStatus" HeaderText="Claim Status" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField >    
                    <asp:BoundField DataField="BodyPart" HeaderText="Body Part" HtmlEncode="false" >
                        <ItemStyle HorizontalAlign="Left" BorderColor="#CCCCCC" BorderWidth="1px"  />
                    </asp:BoundField >    
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#2a479d" Font-Bold="True" ForeColor="White" />
            </asp:GridView>
            <asp:Panel runat="server" ID="panelClosedStatusMessage" visible="false">
                <asp:Label runat="server" ID="lblClosedStatusMessage" />
            </asp:Panel>
            <asp:Panel runat="server" ID="panelPresumedClosedStatusMessage" Visible="false" >
                <asp:label runat="server" id="lblPresumedClosedOtherMessage" />
            </asp:Panel>
            <p>
                <b>Disclaimer:</b>  
                WSI strives to provide accurate data through all online 
                services. Online data is intended to be informational in nature and will not 
                create any additional liabilities should an inconsistency arise. If you have any 
                concerns about the accuracy of information you see online, please contact Customer Service at 701-328-3800 or 800-777-5033.    

            </p>
            <p  style="text-align:center">
                <asp:button runat="server" ID="btnNewLookupFromResults" Text="New Lookup" 
                            OnClick="btnNewLookupFromResults_Click"  />&nbsp;
                <asp:Button ID="btnPrint" runat="server" Text="Print" 
                            OnClick="btnPrint_Click"  />
            </p>
        </div>
    </asp:Panel>

</asp:Content>


