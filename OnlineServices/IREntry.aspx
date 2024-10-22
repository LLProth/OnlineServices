<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IREntry.aspx.cs" Inherits="OnlineServices.IREntry" Title="Incident Report"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Claim Lookup | North Dakota Workforce Safety & Insurance.</title>
    <script src="Content/ir.js?datetime=<%= DateTime.Now %>"  type="text/javascript"></script>
    <%=returnAutoFills()%>

    <script language="javascript" type="text/javascript">
        var $optionsVar = "";
        var loadedOptions = false;
        //var totalRows = 0;

        function Count(text, lng) {
            //var maxlength = new Number(lng); // Change number to your max length.
            if (text.value.length > lng - 1)
                //if(document.getElementById().value.length > 500)
            {
                text.value = text.value.substring(0, lng);
                alert(" Only " + lng + " chars are allowed.");
            }
        }

       
      // This function will find the codeValue specified in the dropDownTxt string and put
      // the word "selected" after it to select that option in the dropdown.
      function selectDropOption(dropDownTxt, codeValue) {
          var returnTxt = dropDownTxt;

          if (codeValue != "") {
              // This way we make sure we are getting the right code and not matching with part of
              // some other code. If we searched on just codeValue, we could hit code 44a before 44
              // which might really mess this whole thing up.
              codeValue = "value='" + codeValue + "'";
              var codeLoc = parseInt(dropDownTxt.indexOf(codeValue));
              var codeValueLen = parseInt(codeValue.length);
              var dropDownTxtLen = parseInt(dropDownTxt.length);
              // beginning is the first characters, plus the length of the code
              var beginning = dropDownTxt.substring(0, (codeLoc + codeValueLen));
              // end is the character after the quote to the end
              var end = dropDownTxt.substring((codeLoc + codeValueLen), dropDownTxtLen);
              returnTxt = beginning + " selected " + end;
          }

          return returnTxt;
      }


      /*
      *   Function shows or hides appropriate
      *   options for dropdown
      */
      function setLoc(drpdwn) {
          var LocationOptionsArr = ["unk", "right", "left", "bi"];
          var val = '';
          var enableDrpdwn = 'n';
          var isTooth = false;
          if (drpdwn != null) {
              val = $(drpdwn).val();
              enableDrpdwn = $(drpdwn).find(":selected").attr('class');
              if (val.indexOf('27') != -1) {
                  isTooth = true;
              }
          }

          t = $(drpdwn).parent().next().children(':first-child');
          tname = t.get(0).tagName;
          if ((tname != '') && (tname != null)) {
              if (tname.toLowerCase().indexOf('select') != -1) {
                  $(t)[0].selectedIndex = 0;
                  if (enableDrpdwn != null) {
                      if (enableDrpdwn.indexOf('y') != -1) {
                          $(t).prop("disabled", false);

                          //Onload set vars
                          if (!loadedOptions) {
                              loadedOptions = true;
                              $optionsVar = t.clone();
                          }

                          $(t).find('option').remove().end();
                          if ($optionsVar != "") {
                              $($optionsVar.children()).each(function () {
                                  var x = LocationOptionsArr.length;
                                  var count = 0;
                                  for (i = 0; i < x; i++) {
                                      count++;
                                      if (isTooth) {
                                          if (this.value.toLowerCase().indexOf(LocationOptionsArr[i].toLowerCase()) == -1) {
                                              if (count == x) {
                                                  $(t).append($('<option>', {
                                                      value: this.value,
                                                      text: this.text
                                                  }));
                                              }
                                          } else {
                                              break;
                                          }
                                      } else {
                                          if (this.value.toLowerCase().indexOf(LocationOptionsArr[i].toLowerCase()) != -1) {
                                              $(t).append($('<option>', {
                                                  value: this.value,
                                                  text: this.text
                                              }));
                                              break;
                                          }
                                      }
                                  }
                              });
                          }
                      } else {
                          //Disable dropdown (void)
                          $(t).prepend("<option value='' selected='selected'></option>");
                          $(t).prop("disabled", true);
                      }
                  }
              }
          }
      }



      // This function will take an array of arrays of part of body values to be populated into
      // the body part table upon load.
      function AddMultipleRows(pobValuesColl) {
          for (var i = 0; i < pobValuesColl.length; ++i) {
              var pobValues = pobValuesColl[i];
              AddRow(pobValues.location, pobValues.bodyPart, pobValues.primary);
          }
      }


      /*
       *  Function iterates through body parts and locations and
       *  adds data to json object then inserts as a string into 
       *  hiddenBodyPartList
       */
      function saveRowData() {
          var jsonObj = {
              BodyPartsCol: []
          };

          $("[id*='intBodyParts'] tr").each(function(){
              var bpd = "";
              var lcd = "";
              var pchk = "";
              var addItm  = false;
              $(this).find('td').each(function(){
                  switch($(this).children().attr('id')) {
                      case 'BodyPartsDrpDwn':
                          bpd = $(this).find('option:selected').text();
                          if ($(this).children()[0].selectedIndex != 0) {
                              addItm = true;
                          }
                          break
                      case 'LocationsDrpDwn':
                          lcd = $(this).find('option:selected').text();
                          if (($(this).children()[0].selectedIndex == 0) || (($(this).children()[0].value == ''))) {
                              if ($(this).children()[0].value == 'unk')
                              {
                                  lcd = 'Unknown';
                              }
                              else
                              {
                                  lcd = '';
                              }
                          }
                          break;
                      case 'PrimaryCheckbox':

                          break;
                  }    
              });

              if (addItm) {
                  var item = {}
                  item["BodyPartsDrpDwn"] = bpd;
                  item["LocationsDrpDwn"] = lcd;
                  jsonObj.BodyPartsCol.push(item);
              }; 
          });

          //alert('returnStr=' + JSON.stringify(jsonObj));
          $("[id*='hiddenBodyPartList']").val(JSON.stringify(jsonObj));
          
          //Just for testing comment when done
          //return false;
      }


      function getObjInnerText(obj) {
          if (document.all) { // IE;
              return obj.innerText;
          }
          else {
              return obj.textContent;
          }
      }

       /*
        *  Check for overall number of body part and location rows
        *  only returns true if less than 8
        */
      function allowNewRow() {
          var totalRows = 0;
          $("[id*='intBodyParts']").find('input[type="checkbox"][id*=DeleteCheckbox]').each(function () {
              totalRows ++;
          });
          if (totalRows < 8) {
              return true;
          }
          return false;
      }

      
     /*
      *  Function removes all selected rows
      */
        function DeleteRow(dabox) {
            $("[id*='intBodyParts']").find('input[type="checkbox"][id*=DeleteCheckbox]:checked').each(function () {
                $(this).parent().parent().remove();
            });
            var rowCount = $("[id*='intBodyParts'] tr").length;
            if (rowCount <= 2) {
                //All forms have been removed so add a new row
                AddRow("", "", "");
            }
        }
     /*
      *  OLD FUNCTION
      */
      //function DeleteRow(dabox) {
      //    $("[id*='intBodyParts']").find('input[type="checkbox"][id*=DeleteCheckbox]:checked').each(function () {
      //        $(this).parentNode.parentNode.remove();
      //    });
      //    totalRows --;
      //    var rowCount = $("[id*='intBodyParts'] tr").length;
      //    if (rowCount <= 2) {
      //        //All forms have been removed so add a new row
      //        AddRow("", "", "");
      //    }
      //}


     /*
      *  Function adds an empty row to the body parts table
      */
      function AddRow(location, bodyPart, primary) {

          if (allowNewRow()) {
          //Setup Json arrays
          var BPLOpt = <%= returnBodyPartLocationOptions(true) %>;
          var BPOpt = <%=returnBodyPartOptions(true) %>;

          $row = $('<tr/>');
          $col1 = $('<td/>').attr('align', 'center');
          $col2 = $('<td/>');
          $col3 = $('<td/>');
          $col4 = $('<td/>').attr('align', 'center');

          //Setup delete checkbox
          $('<input>', { id : "DeleteCheckbox", type:"checkbox", name: "DeleteCheckbox"}).appendTo($col1);

          //Setup and populate the Body Parts dropdown
          var bpd = $('<select />').addClass("BodyPartsClass").attr('id','BodyPartsDrpDwn'); 
          $('<option />', {value: "", text: "-- Select a body part --"}).appendTo(bpd);
          $.each(BPOpt, function () {
              $('<option />', {value: this.value, text: this.original}).addClass(this.pob_opt_loc_ind).appendTo(bpd); 
          });
          bpd.change(function(e) { setLoc(this); });
          bpd.val(bodyPart);
          bpd.appendTo($col2);

          //Setup and populate the Body Parts Location dropdown
          var s = $('<select />').addClass("LocationsClass").attr('id','LocationsDrpDwn');
          $('<option />', {value: "", text: "-- Select a location --"}).appendTo(s); 
          $.each(BPLOpt, function () {
              $('<option />', {value: this.value, text: this.original}).appendTo(s); 
          });
          s.val(location);
          s.appendTo($col3);

          //Setup the primary Body Part checkbox
          var primChk = $('<input>', { id : "PrimaryCheckbox", type:"checkbox", name: "PrimaryCheckbox"});
          if (primary) {
              primChk.prop('checked', true);
          }
          primChk.click(function(e) { HandlePrimary(this); });
          primChk.appendTo($col4);

          $row.append($col1, $col2, $col3, $col4);
          $("[id*='intBodyParts'] tr:last").before($row);
          }
      }




        /*
         * This function is called when the primary checkbox is clicked on any of the body part
         * rows. This function will clear the other boxes that are checked (cuz only one can be
         * primary). This will also set the value of the hidded variable primaryIndex which allows
         * us to know which is set to primary when this is submitted.
         */
        function HandlePrimary(checkbox){
            var primaryBoxes;
            var myTable;
            var inputList;
            var primaryIndex;
            
            //Uncheck all
            $("[id*='intBodyParts']").find('input[type="checkbox"][id*=PrimaryCheckbox]').each(function () {
                this.checked = false;
            });
            checkbox.checked = true;
            primaryIndex.value = checkbox.parentNode.parentNode.rowIndex - 1;
        }



     /*
      *   Function creates the first empty form fields
      *   and appends them to the table
      */
      function FirstRows() {

          var bodyPartsString = $("input[name*='partValueHiddenField']").val();
          var bodyPartLocsString = $("input[name*='locValueHiddenField']").val();
          var added = false;
          var primaryIndex = $("input[name*='primaryIndexId']").val() - 0;

         // alert('bodyPartsString=' + bodyPartsString);
          //alert('bodyPartLocsString=' + bodyPartLocsString);
          //alert('primaryIndex=' + primaryIndex);


          if (bodyPartsString != "") {
              var bodyPartsArray = bodyPartsString.split(",");
              var bodyPartLocsArray = bodyPartLocsString.split(",");
              for (var bpi = 0; bpi < bodyPartsArray.length; bpi++) {
                  var bp = bodyPartsArray[bpi]
                  var bpl = "";
                  if (bodyPartLocsArray.length <= bpi) {

                  }
                  else {
                      bpl = bodyPartLocsArray[bpi];
                  }
                  if (bp != "") {
                      if (bpi == primaryIndex) {
                          AddRow(bpl, bp, true);
                      }
                      else {
                          AddRow(bpl, bp, false);
                      }
                      added = true;
                  }
              }

          }
          else {
              added = true;
              AddRow("", "", "");
          }

          if (added != true) {
              AddRow("", "", "");
          }
      }



        $(function () {
            $("textarea[maxlength]").bind('input propertychange', function () {
                var maxLength = $(this).attr('maxlength');
                if ($(this).val().length > maxLength) {
                    $(this).val($(this).val().substring(0, maxLength));
                }
                if ($(this).val().length > maxLength - 50) {
                    $("#MainContent_DescriptionOfInjuryLengthMessage").text("You have " + (maxLength - $(this).val().length) + " characters remaining.");
                } else {
                    $("#MainContent_DescriptionOfInjuryLengthMessage").text("");
                }
            });
        });
      /*
      *   Updating the Location dropdown options
      *   and selected values onload
      */
      $(document).ready(function () {
          //Populate the first set of rows in the intBodyParts table
          FirstRows();
      });

</script>









</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">   

    <div>
        <p>
            If a worker was injured within the last or current business day and medical attention is <u>not</u> being sought, provide information regarding that incident below.
        </p>
        <p>
            If a worker was injured and medical attention is being sought, please file a claim using the new
            <a runat="server" id="linkFroi" name="ofroi" >
                Claim Filing/First Report of Injury
            </a>
            immediately, instead of this incident report.
        </p>
        <div >
            <table class="main" style="font-size:0.9em;width:95%" >
                <tr runat="server" id="errorMessageRow" visible="false">
                    <td colspan="2">
                        <span class='loud'>*</span> <b>Please complete one field on this page before performing a search.</b>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h2>
                            Worker Information
                        </h2>
                    </td>
                </tr>
                <tr>
                    <td class="aspLabel30"  >
                        <span class="loud">* </span>
                        <label for="first-name">
                            First Name:
                        </label>
                    </td>
                    <td class="aspTextbox" >
                        <asp:TextBox runat="server" tabindex="1" name="w_first_name" id="txtFirstName"  maxlength="25" width="40%" />
                    </td>
                </tr>
                <tr>
                    <td class="aspLabel30" >
                        <label for="middle-initial">
                            Middle Initial:
                        </label>
                    </td>
                    <td class="aspTextbox" >
                        <asp:TextBox runat="server" tabindex="2" name="w_middle_initial" id="txtMiddleInitial"  maxlength="1"  width="10%" />
                    </td>
                </tr>
                <tr>
                    <td class="aspLabel30" >
                        <span class="loud">* </span>
                        <label for="last-name">Last Name:</label>
                    </td>
                    <td class="aspTextbox" >
                        <asp:TextBox runat="server"  tabindex="3" name="w_last_name" id="txtLastName"  maxlength="25" width="40%" />
                    </td>
                </tr>
                <tr>
                    <td class="aspLabel30" >
                        <label for="social-security-number">Social Security Number:</label>
                    </td>
                    <td class="aspTextbox" >
                        <asp:TextBox runat="server"  tabindex="4" name="soc_sec_no" id="txtSsn"  maxlength="9" width="20%"/>
                        &nbsp;&nbsp;
                        <span class="exempligratia">(no&nbsp;dashes)</span>
                    </td>
                </tr>
                <tr>
                    <!--new stuff here - need to add javascript here for no date in future-->
                    <td class="aspLabel30" >
                        <span class="loud">* </span>
                        <label for="birthDateID">Birthdate:</label>
                    </td>
                    <td  class="aspTextbox" >
                        <asp:TextBox runat="server" tabindex="5" name="birth_dt" id="txtBirthDate"  maxlength="10" width="20%"/>
                        &nbsp;&nbsp;<span class="exempligratia">(mm/dd/yyyy)</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br/>
                        <h2>Injury Information</h2>
                    </td>
                </tr>
                 <tr>
                     <!--new stuff here - need to add javascript here for no date in future-->
                     <td class="aspLabel30">
                         <span class="loud">* </span><label for="txtDoi">Injury&nbsp;Date:</label>
                     </td>
                     <td   class="aspTextbox" >
                         <asp:TextBox runat="server"  tabindex="6" name="injury_dt" id="txtDoi"  maxlength="10" width="20%"/>
                         &nbsp;&nbsp;<span class="exempligratia">(mm/dd/yyyy)</span>
                     </td>
                </tr>
                <tr>
                     <td colspan="2">&nbsp;
                         <br />


                         
                         
                         






                         
 

<%--BODY DIV --%>
<div style="margin: 25px 0px 25px 0px">
    <label for="bodyPartId">Body Part Injured (Example: Left 2nd/middle finger, right shoulder, left ankle.)&nbsp;<b class="red">*</b>:</label> 
    <br/><br/>

    <asp:hiddenfield id="locValueHiddenField" value="" runat="server"/>
    <asp:hiddenfield id="partValueHiddenField" value="" runat="server"/>
    <asp:hiddenfield id="partVoidHiddenField" value="" runat="server"/>
    <asp:hiddenfield id="partOptLocHiddenField" value="" runat="server"/>
    <asp:hiddenfield id="locDescValueHiddenField" value="" runat="server"/>
    <asp:hiddenfield id="partDescValueHiddenField" value="" runat="server"/>
    <asp:HiddenField ID="primaryIndexId" Value="0" runat="server" />
    
    
    <%--BODY PARTS LIST SUBMISSION VAL --%>
    <input type="hidden" id="hiddenBodyPartList" runat="server" value="" />
    <%--BODY PARTS LIST SUBMISSION VAL --%>

    <%--Body Parts Table--%>
    <table id="intBodyParts" class="intBodyParts" runat="server" >              
    <TR>
    <TH>Delete</TH>
    <TH>Body Part</TH>
    <TH>Location</TH>
    <TH>Primary</TH>
    </TR> 
    <TR>
    <TD>
    <input type="button" value="Delete" onclick="DeleteRow()" name="delete-all-checked" id="Button1" class="btnOLNS" />
    </TD>
    <TD colspan="3" align="center">
    <input type="button" value="Add Another Body Part" onclick="AddRow('', '', '')" name="add-another" id="Button2" class="btnOLNS" />
    </TD>
    </TR>
    </table>
    <%--Body Parts Table--%>
</div>
<%--BODY DIV --%>




<%--Old body part div--%>
                        
<%--Old body part div--%>





                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                
                
                <tr style="display:none;">
                    <td colspan="2" >

<%--Old body part location div--%>

<%--Old body part location div--%>
                        






                    </td>
                </tr>


                
                <tr id="natureOfInjuryRow">
                    <td  class="aspLabel30">
                        <label for="nature-of-injury-ID">
                            Nature of Injury
                            <span class="notasloud">&nbsp;(optional)</span>
                            :
                        </label>
                    </td>
                    <td class="aspTextbox">
                        <asp:DropDownList valign="bottom" TabIndex="44" ID="ddlNatureOfInjury" runat="server"/>&nbsp
                    </td>
                </tr>
               
                <tr id="describeItRow">
                     <td class="aspLabel30">
                         <label for="description-of-injury-ID">Describe clearly how the accident occurred:<br />(Limit 2000 characters)</label>
                     </td>
                    <td class="aspTextbox">
                        <textarea runat="server" name="description_of_injury" id="textAreaDescriptionOfInjury" tabindex="45" cols="35" rows="6"
                            style="width:80%" maxlength="2000"></textarea><br />
                        <span class="notasloud" id="DescriptionOfInjuryLengthMessage" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h2>Employer Information</h2>
                    </td>
                </tr>
                <tr runat="server" id="accountNumberRow" visible="false">
                    <td class="aspLabel30">
                        <span class="loud">* </span><label for="account-number">WSI Account Number:</label>
                    </td>
                    <td class="aspTextbox">
                        <asp:Label runat="server"  name="employer_act_no" id="lblAccountNumber" Font-Bold="true" />&nbsp;&nbsp;
                        <span class="exempligratia">(Based on your premium billing statement)</span>
                    </td>
                </tr>
                <tr id="businessNameRow">
                    <td class="aspLabel30">
                        <span class="loud">* </span><label for="business-name">Business Name:</label>
                    </td>
                    <td class="aspTextbox">
                        <asp:Label runat="server" name="business_name_label" ID="lblBusinessName" Font-Bold="true" />
                    </td>
                </tr>
                <tr id="submittedByRow">
                    <td class="aspLabel30">
                        <span class="loud">* </span><label for="your-name">This report is submitted by:</label>
                    </td>
                    <td class="aspTextbox">
                        <asp:TextBox runat="server"  tabindex="53" name="employer_signature" id="txtYourName"  maxlength="40" width="50%"  placeholder="First and last name please" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr style="text-align:center">
                    <td colspan="3"  id="buttonRow" >
                        <asp:Button ID="btnSubmit" TabIndex="54" Text="Submit" runat="server" OnClientClick="return submitIR(this);" CssClass="btnOLNS" OnClick="btnSubmit_Click" />
                        <input type="reset" tabindex="55" value=" Clear " name="reset" Class="btnOLNS" />
                        <asp:Button runat="server" ID="resetCompany" Text="Reset Business" OnClientClick="setOverRide()" CssClass="btnOLNS" OnClick="resetCompany_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>


 


    <input id="Hidden1" type='hidden' runat="server" name='IRvalue'  />
    <input id="Hidden2" type='hidden' runat="server" name='AppIDvalue' />



    <asp:Panel ID="xmlBodyPartPnl" runat="server">
        <div style="float:right">
            <div style="color:red" id="xmlBP_fail" visible="false" runat="server">*<div style="display:none">XML Body Part update failed - <asp:Literal ID="xmlBP_fail_Lit" runat="server" /></div></div>
            <div style="color:green" id="xmlBP_success" visible="false" runat="server">*<div style="display:none">XML Body Part update sucess</div></div>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="xmlBodyPartLocationPnl" runat="server">
        <div style="float:right">
            <div style="color:red" id="xmlBPL_fail" visible="false" runat="server">*<div style="display:none">XML Body Part LOCATION update failed - <asp:Literal ID="xmlBPL_fail_Lit" runat="server" /></div></div>
            <div style="color:green" id="xmlBPL_success" visible="false" runat="server">*<div style="display:none">XML Body Part LOCATION update sucess</div></div>
        </div>
    </asp:Panel> 

</asp:Content>