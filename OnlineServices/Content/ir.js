function init() {

 
	if (document.getElementById)
	{
		firstName = document.getElementById("first-name");
	}
	else if (document.all)
	{
		firstName = document.all["first-name"];
	}

	firstName.focus();
}

function WSI_preloadImages() { //v3.0
    var d = document; 
    if (d.images) {
        if (!d.MM_p) d.MM_p = new Array();
          var i, j = d.MM_p.length, a = WSI_preloadImages.arguments; 
          for (i = 0; i < a.length; i++)
            if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; } 
    }
}

function CompanySelectInit() {
   // MM_preloadImages('/images/ProcessingHexagonsRight.gif');
}

var overRide;
overRide = false;

function setOverRide() 
{
    overRide = true;
}

function ir_isDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;

    //Declare Regex 
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?
    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[1];
    dtDay = dtArray[3];
    dtYear = dtArray[5];
    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}


//this function checks that the required fields
function submitIR()
{

    //Move bodyparts data into hidden var
    saveRowData();


    //var bodyPartList;

    if (overRide) {
        overRide = false;       
        return true;
    }

    if  ( $.trim( $("#MainContent_txtFirstName").val()) == "")
    {
        alert("First Name is a required field.  Please enter the employee's first name.");
        $("#MainContent_txtFirstName").focus();
        return false;
    }
  
    if ($.trim($("#MainContent_txtLastName").val()) == "") {
        alert("Last Name is a required field.  Please enter the employee's first name.");
        $("#MainContent_txtLastName").focus();
        return false;
    }

    var flag;

    var ssn = $.trim($("#MainContent_txtSsn").val());
    if (ssn != "") {
        flag = ssn.match(/^(\d{9})$/);   //accepts 999999999
        if (!flag)
        {
            alert("The social security number you entered is invalid.  Please enter the social security number in the form 999999999.");
            $("#MainContent_txtSsn").select();
            $("#MainContent_txtSsn").focus();
            return false;
        }
    }

    var birthDt = $.trim($("#MainContent_txtBirthDate").val());
    if (birthDt == "")
    {
        alert("Date of Birth is a required field.  Please enter the employee's date of birth.");
        $("#MainContent_txtBirthDate").focus();
        return false;
    }
    else
    {
        flag = ir_isDate(birthDt);
        if (!flag) {

            alert(birthDt + " is not a valid date. Please re-enter date using MM/DD/YYYY format.");
            $("#MainContent_txtBirthDate").select();
            $("#MainContent_txtBirthDate").focus();
            return false;
        }
        else
        {
            // Check to see if selected date is in the past
            if (Date.parse(birthDt) > Date.now()) {
                alert(birthDt + " is not a valid birthdate. Please enter a past date.");
                $("#MainContent_txtBirthDate").select();
                $("#MainContent_txtBirthDate").focus();
                return false;
            }
        }
    }

    var injuryDt = $.trim($("#MainContent_txtDoi").val());
    if (injuryDt == "")
    {
        alert("Injury Date is a required field.  Please enter the employee's injury date.");
        $("#MainContent_txtDoi").focus();
        return false;
    }
    else
    {
        flag = ir_isDate(injuryDt);
        if (!flag) {
            alert(injuryDt + " is not a valid date. Please re-enter date using MM/DD/YYYY format.");
            $("#MainContent_txtDoi").select();
            $("#MainContent_txtDoi").focus();
            return false;
        }
        else {
            // Check to see if selected date is in the past
            if (Date.parse(injuryDt) > Date.now()) {
                alert(injuryDt + " is not a valid injury date. Please enter a past date.");
                $("#MainContent_txtDoi").select();
                $("#MainContent_txtDoi").focus();
                return false;
            }
        }
    }






    //var counter = "";
    //var countNumberBodyPartsChecked = $("#bodyPart input:checked").length;
    //if (countNumberBodyPartsChecked == 0)
    //{
    //    alert("Body Part Injured is a required field.  Please select the employee's body part injured.");
    //    $("#bodyPart").focus();
    //}

    if (!checkBodyPartInjured()) {
        alert("Body Part Injured is a required field.  Please select the employee's body part injured.");
        return false;
    }







    var subBy = $.trim($("#MainContent_txtYourName").val());
    if (subBy == "") {
        alert("Report is Submitted By is a required field.  Please enter your first and last name.");
        $("#MainContent_txtYourName").select();
        $("#MainContent_txtYourName").focus();
        return false;
    }

    if (subBy == "First and last name please")
    {
        alert("Report is Submitted By is a required field.  Please enter your first and last name.");
        $("#MainContent_txtYourName").select();
        $("#MainContent_txtYourName").focus();
        return false;
    }



    ////////////////////////////////////
    ////////////////////////////////////
    //alert('inserting json');
    //return false;
    ////////////////////////////////////
    ////////////////////////////////////





    return true;

}


function checkBodyPartInjured()
{
    var returnBool = false;
    var ttl = 0;
    $("[id*='intBodyParts'] select.BodyPartsClass").each(function () {
        if (this.selectedIndex > 0) {
            ttl++;
        }
    });
    //If at least one body part has been selected then return true
    if (ttl > 0) {
        returnBool = true;
    }
    return returnBool;
}


function removeButtonsForProcessing() {
    var msgStr;

    msgStr = "<img id='processingLeft' src='./images/ProcessingHexagonsLeft.gif'>&nbsp;&nbsp;<font color='#336699'>Processing.  Please wait.</font>&nbsp;&nbsp;<img id='processingRight' src='./images/ProcessingHexagonsRight.gif'>";

    var messageRow;

    if (document.getElementById) {
        messageRow = document.getElementById("waitMessageRow");
    }
    else if (document.all) {
        messageRow = document.all["waitMessageRow"];
    }

    messageRow.innerHTML = msgStr;
}
