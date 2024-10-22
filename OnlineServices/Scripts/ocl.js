//*****************************************************************
//* BELOW IS THE FUNCTION TO CONTROL POPUP WINDOW CHARACTERISTICS *
//*****************************************************************
// set up options for new pop up window
// these options are for loadWinPopup function
var popupOptions = "";
popupOptions += "status="        +   "yes";
popupOptions += ",directories="  +   "no";
popupOptions += ",location="     +   "no";
popupOptions += ",toolbar="      +   "no";
popupOptions += ",menubar="      +   "no";
popupOptions += ",scrollbars="   +   "yes";
popupOptions += ",resizable="    +   "yes";
popupOptions += ",width="        +   "1000";
popupOptions += ",height="       +   "600";
popupOptions += ",top="          +   "40";
popupOptions += ",screenY="      +   "40";
popupOptions += ",left="         +   "40";
popupOptions += ",screenX="      +   "40";

//*********************************************************
//* this is a sample of popupOptions for new window to
//* be passed into function from page
//---------------------------------------------------------
//*
//* var popupOptions = "";
//* popupOptions += "status="        +   "yes";
//* popupOptions += ",directories="  +   "no";
//* popupOptions += ",location="     +   "no";
//* popupOptions += ",toolbar="      +   "no";
//* popupOptions += ",menubar="      +   "yes";
//* popupOptions += ",scrollbars="   +   "yes";
//* popupOptions += ",resizable="    +   "yes";
//* popupOptions += ",width="        +   "600";
//* popupOptions += ",height="       +   "400";
//* popupOptions += ",top="          +   "40";
//* popupOptions += ",screenY="      +   "40";
//* popupOptions += ",left="         +   "40";
//* popupOptions += ",screenX="      +   "40";
//*
//---------------------------------------------------------
//* should happen before the function as global variable
//*********************************************************

// whenever the user clicks on a link, this global variable will be used to check if the target window has already been opened
var newDumpWindow = null;
var newPopupWindow = null;

// check browser versions -- we will skip all javascript functionality completely in NS2 and IE3
// (they don't support the window.closed property, plus IE3 doesn't support the window.focus method)
var is_IE3 = (navigator.appVersion.indexOf("MSIE 3") != -1) ? true : false;
var is_NS2 = ((navigator.appName == "Netscape") && (navigator.appVersion.charAt(0) == "2")) ? true : false;


function validateWorkerRegSubmit(frm) {
    if (!confirm("This will submit the information on this page. Would you like to proceed?")) {
        return false;
    }

    //var name = document.getElementById("MainContent_txtName");
    //if (name == null || name == "") {
    //    alert("An worker's name is required");
    //    return false;
    //}

    //var claim = document.lookupFormMain.ClaimNo.value;
    //if (claim == null || claim == "") {
    //    alert("The claim number is required");
    //    document.lookupFormMain.ClaimNo.focus();
    //    return false;
    //}

   

}




function loadWinPopup(page)
{
	 // if old browser version, abort this function and let the AREF and TARGET attributes of the original link take over
	 if (is_IE3 || is_NS2) {
	    return true;
	 }

	 // if the var is still null, a new window has never been created, so create one
	 if (! newPopupWindow) {
	    newPopupWindow = window.open("","newPopup",popupOptions);
	 }

	 // if the user closes the target window, the "closed" property will tell us this, and we can reopen it
	 if (newPopupWindow.closed) {
	    newPopupWindow = window.open("","newPopup",popupOptions);
	 }

	 // load the clicked page into the target window
	 newPopupWindow.location = page;

	 newPopupWindow.focus();

	 // return false so that the browser does not process the AREF and TARGET attributes of the original link
	 return false;
}

function init()
{
  //  MM_preloadImages('/images/ProcessingHexagonsRight.gif', '/images/ProcessingHexagonsLeft.gif');
	 
//     setTimeout("window.document.onlineClaimLookup.ssn.focus()", 100);
//     setTimeout("window.document.onlineClaimLookup.ssn.select()", 100);
}

function clearFields()
{
     //clear all form[x] fields on page
     for(var i = 0; i < window.document.forms.length; i++)
     {
         for(var j = 0; j < window.document.forms[i].elements.length; j++)
         {
         	if(window.document.forms[i].elements[j].type != "button" &&
         	   window.document.forms[i].elements[j].type != "submit" &&
         	   window.document.forms[i].elements[j].type != "reset")
         	{
         		window.document.forms[i].elements[j].value = "";
         	}
         }
     }
}

//this function gets the position of a field on a form
function getPosition(field)
{
	//alert("in getPosition field = " + field);
	for(var i = 0; i < window.document.forms[1].elements.length; i++){
    	if(window.document.forms[1].elements[i] == field)
    	{
        	fieldPlace = i;
            return fieldPlace;
        }
    }
}

function stripDashes(dirtyValue)
{
	var dashFreeValue = "";
	var valueChar = "";
	var dash = "-";

	for(var i = 0; i < dirtyValue.value.length; i++)
	{
		valueChar = dirtyValue.value.charAt(i);
		//alert("valueChar = " + valueChar)

		if(dash.indexOf(valueChar) == -1)
		{
			dashFreeValue += valueChar;
			//alert("dashFreeValue in cleanValue = " + dashFreeValue);
		}
	}

	return dashFreeValue;
}

//this function takes a field and then cleans all spaces from its value
    function trimSpaces(field){
         var oneChar;
         var newField = "";
         for(var i = 0; i < field.value.length; i++)
         {
            oneChar = field.value.charAt(i);

            if(oneChar != " "){
               newField += oneChar;
            }
         }

         return newField;
    }

function validateFedId(fedId)
{
	var fedIdValue;

	fedIdValue = trimSpaces(fedId);

	matchFedId = fedIdValue.match(/^(\d{9}|\d{2})-?\d{7}$/);

	if(!matchFedId)
	{
    	alert("The federal tax id number you entered is invalid.  Please enter the federal tax id number in the form 999999999 or 99-9999999.");

        fedIdSpot = getPosition(fedId);
        //alert("fedIdSpot = " + fedIdSpot);

        setTimeout("window.document.forms[0].elements[fedIdSpot].focus()", 100);
        setTimeout("window.document.forms[0].elements[fedIdSpot].select()", 100);
        return false;
    }
    return true;
}

function clearFieldsAndFocusFirstElement()
{
	clearFields();
	init();
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

function closeWndw()
{
	window.close();
}

function removeButtonsForProcessing() {
    var msgStr;

    msgStr = "<img src='../images/ProcessingHexagonsLeft.gif'>&nbsp;&nbsp;<font color='#336699'>Processing.  Please wait.</font>&nbsp;&nbsp;<img src='../images/ProcessingHexagonsRight.gif'>";

    var messageRow;

    if (document.getElementById) {
        messageRow = document.getElementById("waitMessageRow");
    }
    else if (document.all) {
        messageRow = document.all["waitMessageRow"];
    }

    // messageRow.innerHTML = msgStr;
}

function validateSSN(ssn)
{
    var fieldPlace;
    
    
    
    //matchSsn = ssn.value.match(/^(\d{9}|\d{3})-?\d{2}-?\d{4}$/);   //accepts 999999999 or 999-99-9999
    matchSsn = ssn.value.match(/^(\d{9})$/);   //accepts 999999999

    if(!matchSsn)
    {
        alert("The social security number you entered is invalid.  Please enter the social security number in the form 999999999.");

        fieldPlace = getPosition(ssn);
        setTimeout("window.document.onlineClaimLookup.elements[fieldPlace].focus()", 100);
        setTimeout("window.document.onlineClaimLookup.elements[fieldPlace].select()", 100);
        return false;
    }
return true;
}

//this function validates dates
//pass in true for future if date cannot be in future otherwise pass false
//pass in true for past if date cannot be in past otherwise pass false
function validateDate(formName, field, future, past)
{
	var err = 0;				 //sets err value to zero
    var d = new Date();          //creates a new date
    var dd = d.getDate();        //retrieves the day from the system
    var mm = d.getMonth() + 1;   //retrieves the month JS starts counting at zero, so add a 1 to start January at 1
    var yyyy = d.getYear();      //retrieves the year

    for (var i = 0; i < window.document.forms.length; i++)
    {
    	if(window.document.forms[i].name == formName)
    	{
 			whichForm = i;
 		}
	}

    for(var i = 0; i < window.document.forms[whichForm].elements.length; i++)
    {
 		if(window.document.forms[whichForm].elements[i] == field)
 		{
 			whichField = i;
 		}
    }

    if (yyyy < 1000)
    {
    	yyyy = yyyy + 1900;
    }

    var month = field.value.substr(0, 2);   // month
    var slash1 = field.value.substr(2, 1);  // 1st slash
    var day = field.value.substr(3, 2);   // day
    var slash2 = field.value.substr(5, 1);  // 2nd slash
    var year = field.value.substr(6, 4);  // year

    if(field.value.length != 10)
    {
     	err = 1;
     }
     else if(isNaN(month) || isNaN(day) || isNaN(year))
     {
        err = 1;
     }
     else if(month < 1 || month > 12)
     {
        err = 1;
     }
     else if(day < 1 || day > 31)
     {
        err = 1;
     }
     else if(year < 1900)
     {
        err = 1;
     }
     else if(month == 4 || month == 6 || month == 9 || month == 11)
     {
     	if(day == 31)
     	{
            err = 1;
        }
     }
     else if (month == 2)
     {
     	if(day == 29 && ((year/4)!= parseInt(year/4)))
     	{
            err = 1;
        }

        if (day > 29)
        {
        	err = 1;
        }
     }

     if(err == 1)
     {
     	alert(field.value + " is not a valid date. Please re-enter date using MM/DD/YYYY format.");
        setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
        setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
        return false;
     }

     //FUTURE
	 if(err == 0)
	 {
     	if(year > yyyy)
     	{
			err = 2;
		}

		if(year == yyyy && month > mm)
		{
		    err = 2;
		}

		if(year == yyyy && month == mm && day > dd)
		{
		    err = 2;
		}
	}

	if((err == 2) && (future))
	{
		alert(field.value + " is a date in the future.  Please re-enter date.");
        setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
        setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
		return false;
	}

	//PAST
	if(err == 0)
	{
    	if(year < yyyy)
    	{
			err = 3;
		}

		if(year == yyyy && month < mm)
		{
		    err = 3;
		}

		if(year == yyyy && month == mm && day < dd)
		{
		    err = 3;
		}
	}

	if((err == 3) && (past))
	{
		alert(field.value + " is a date in the past.  Please re-enter date.");
        setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
        setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
		return false;
	}

    return true;
}

function checkSsn(obj)
 {
      var ssnList;

      ssnList = document.getElementById("txtSsn");
       //ssnList = document.getElementsByTagName("input");

       
           
         sub = validateSSN(ssnList);
         //alert("sub = " + sub);
         if(!sub)
         {
              return sub;
         }
         

       return sub;

 }

 function checkDate(obj)
 {
      var dateList;


      dateList = document.getElementById("txtInjuryDate");

      
                 sub = validateDate('onlineClaimLookup', dateList, true, false);

                 if (!sub) {
                     return sub;
                 }
                 else {
                     dateList = document.getElementById("txtBirthDate");
                     sub = validateDate('onlineClaimLookup', dateList, false, false);
                 }
          
       return sub;
 }


 function submitOCL(obj)
 {
      flag = true;
      var sErr = "";

      var noDataButton;
      if (document.getElementById) {
          noDataButton = document.getElementById("btnNewLookupFromNoData");
      }
      else if (document.all) {
            noDataButton = document.all["btnNewLookupFromNoData"];
      }
      
      if (noDataButton == null) {

          if (flag) {
              flag = checkSsn(obj); //check ssn
              if (!flag) {
                  return false;
              }
              else {
                  flag = checkDate(obj); //check date
                  if (!flag) {
                      return false;
                  }
              }
          }
          else {
              return false;
          }
          removeButtonsForProcessing();
          return true;
      }
      else {
          return true
      }
 }

     