//
// commonAsp.js
// 

//this function takes a field and then cleans all spaces from its value
function trimSpaces(field){
    var oneChar;
    var newField = "";
    
    //alert("field name = " + field.name);
    //alert("field value = " + field.value);
    
    for(var i = 0; i < field.value.length; i++)
    {
       oneChar = field.value.charAt(i);
       
       if(oneChar != " "){
          newField += oneChar;
       }
    }
    
    return newField;
}


//this function gets the position of a field on a form  
function getPosition(field)
{
	//alert("in getPosition field = " + field);
	for(var i = 0; i < window.document.IncidentReport.elements.length; i++){
    	if(window.document.IncidentReport.elements[i] == field)
    	{
        	fieldPlace = i;
            return fieldPlace;
        }
    }
}


//this function returns which form user is in on page
function getForm(formName)
{
     for (var i = 0; i < window.document.forms.length; i++)
     {
    	     if(window.document.forms[i].name == formName)
    	     {
 			whichForm = i;
 			return whichForm;
 		}
	}
}


//this function validates that the time is of correct time format
function validateTime(formName, timeField, inFormat)
{
	var whichForm;
	//var whichField;
	var timeValue = timeField.value;
//     	          
//	whichForm = getForm(formName);
//     	
//     whichField = getPosition(timeField);
      
	if(inFormat == "HH:MMxm")
	{     		
		var colonIndex = timeValue.indexOf(":");
		var hour = timeValue.substring(0,colonIndex);
		var minutes = timeValue.substring(colonIndex + 1, colonIndex + 3);
		var amOrPm = timeValue.substring(colonIndex + 3).toUpperCase();
		
		if (hour < 1 || hour > 12 || minutes > 59 || (amOrPm != "PM" && amOrPm != "AM"))
		{  
	          alert(timeValue + " is not a valid time. Please re-enter time using HH:MMxm format. Example: 11:38pm");
//	          setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
//               setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
	          return false;
		}
	}
	else if(inFormat == "HH:MM")
	{
		var colonIndex = timeValue.indexOf(":");
		var hour = timeValue.substring(0,colonIndex);
		var minutes = timeValue.substring(colonIndex + 1, colonIndex + 3);
		//var amOrPm = timeValue.substring(colonIndex + 3).toUpperCase();
		
		if (hour < 1 || hour > 12 || minutes > 59) //|| (amOrPm != "PM" && amOrPm != "AM"))
		{  
	          alert(timeValue + " is not a valid time. Please re-enter time using HH:MM format. Example: 11:38");
//	          setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
//               setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
	          return false;
		}
	}
	else
	{ 
     	//alert("whichField3 = " + whichField);
          alert(timeValue + " is not a valid time. Please re-enter time using HH:MMxm format. Example: 11:35pm");
//          setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
//          setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
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
    //var whichField;
    //var whichForm;
    var d = new Date();          //creates a new date
    var dd = d.getDate();        //retrieves the day from the system
    var mm = d.getMonth() + 1;   //retrieves the month JS starts counting at zero, so add a 1 to start January at 1
    var yyyy = d.getYear();      //retrieves the year
    //sub = 1;
     	          
//     whichForm = getForm(formName);
//     
//    whichField = getPosition(field);
                  
    //alert("validateDate after set sub = " + sub);
    //alert("New Date - d - " + d);
    //alert("dd - " + dd);
    //alert("mm - " + mm);
    //alert("yyyy - " + yyyy);
     
    if (yyyy < 1000)
    {
    	yyyy = yyyy + 1900;
    }
     
    var month = field.value.substr(0, 2);   // month
    var slash1 = field.value.substr(2, 1);  // 1st slash
    var day = field.value.substr(3, 2);   // day
    var slash2 = field.value.substr(5, 1);  // 2nd slash
    var year = field.value.substr(6, 4);  // year
     
    //alert("month - " + month);
    //alert("slash1 - " + slash1);
    //alert("day - " + day);
    //alert("slash2 - " + slash2);
    //alert("year - " + year);
     	               
    if(field.value.length != 10)
    {
		//alert("validateDate length");
     	err = 1;
     }
     else if(isNaN(month) || isNaN(day) || isNaN(year))
     {
     	//alert("validateDate is NAN");
        err = 1;
     }
     else if(month < 1 || month > 12)
     {
     	//alert("validateDate month < 1 or > 12");
        err = 1;
     }
     else if(day < 1 || day > 31)
     {
     	//alert("validateDate day < 1 or > 31");
        err = 1;
     }
     else if(year < 1900)
     {
     	//alert("validateDate year < 1900");
        err = 1;
     }
     else if(month == 4 || month == 6 || month == 9 || month == 11)
     {
     	if(day == 31)
     	{
        	//alert("validateDate month 4,6,9,11 - day == 31");
            err = 1;
        }
     }
     else if (month == 2)
     {
     	if(day == 29 && ((year/4)!= parseInt(year/4)))
     	{
        	//alert("validateDate leap year");
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
//        setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
//        setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
        return false;
     }    
                    
	 //FUTURE 
	 if(err == 0)
	 {
     	if(year > yyyy)
     	{
			//alert("validateDate year > yyyy current year");
			err = 2;
		}
			
		if(year == yyyy && month > mm)
		{
			//alert("validateDate year = yyyy and month > current month");
		    err = 2;
		}
		
		if(year == yyyy && month == mm && day > dd)
		{
			//alert("validateDate year = yyyy and month = mm and day > current day");
		    err = 2;
		}
	}

	if((err == 2) && (future))
	{
		alert(field.value + " is a date in the future.  Please re-enter date.");			
//        setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
//        setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
		return false;
	}  
		    
	//PAST 
	if(err == 0)
	{
    	if(year < yyyy)
    	{
    		//alert("validateDate year < yyyy current year");
			err = 3;
		}
			
		if(year == yyyy && month < mm)
		{
			//alert("validateDate year = yyyy and month < current month");
		    err = 3;
		}
		
		if(year == yyyy && month == mm && day < dd)
		{
			//alert("validateDate year = yyyy and month = mm and day  current day");
		    err = 3;
		}
	}

	if((err == 3) && (past))
	{
		alert(field.value + " is a date in the past.  Please re-enter date.");			
//        setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
//        setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
		return false;
	}		                      
    
    return true;          
}

function setSysDate()
{
     var d;
     var dd;
     var mm;
     var yyyy;
     var now;
     
     /*creates a new date*/
     d = new Date();
     
     /*retrieves the date from the system*/
     dd = d.getDate();
     
     /*retrieves the month JS starts counting at zero, so add a 1 to start January at 1*/
     mm = d.getMonth() + 1;
     
     /*retrieves the year*/
     yyyy = d.getYear();
     
     if (yyyy < 1000)
     {
    	     yyyy = yyyy + 1900;
     }
     
     if(dd < 10)
     {
          dd = "0" + dd;
     }
     
     if(mm < 10)
     {
          mm = "0" + mm;
     }
     
     now = mm + "/" + dd + "/" + yyyy;
     
     return now;
}

//validates that ssn is correct a correct 9 digit number with dashes or not
function validateSSN(ssn)
{
	//var ssnSpot;
	var matchSsn;
	var ssnValue;
	
	ssnValue = trimSpaces(ssn);
	
	matchSsn = ssnValue.match(/^(\d{9}|\d{3})-?\d{2}-?\d{4}$/);   //accepts 999999999 or 999-99-9999
	
	if(!matchSsn)
	{
    	   alert("The social security number you entered is invalid.  Please enter the social security number in the form 999999999.");
        
        //ssnSpot = getPosition(ssn);
        //alert("ssnSpot = " + ssnSpot);
//        
//        setTimeout("window.document.IncidentReport.elements[ssnSpot].focus()", 100);
//        setTimeout("window.document.IncidentReport.elements[ssnSpot].select()", 100);
        return false;
     }
     return true;
}


//this function is used for textareas since they don't have a maxlength
//it is used to limit the number of characters a user can enter into
//field
function limitText(formName, formField, limit)
{
    var textLength;
    //var whichForm;
    //var whichField;
    
    textLength = formField.value.length;      

    if(textLength > limit)
    {
          alert(formField.name + " cannot exceed " + limit + " characters.  Please shorten your entry.");
          
//          whichField = getPosition(formField);
//          whichForm = getForm(formName);
//          
//          setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
//          setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
          return false; 
    }
    else
    {
    return true;
    }
}


//this function
function isValidNumber(formName, formField)
{
     var fieldValue = formField.value;
     
     if(isNaN(fieldValue))
     {
          alert(formField.value + " is not a valid number.  Please re-enter.");
          
//          whichField = getPosition(formField);
//          whichForm = getForm(formName);
//          
//          setTimeout("window.document.forms[whichForm].elements[whichField].focus()", 100);
//          setTimeout("window.document.forms[whichForm].elements[whichField].select()", 100);
          return false;   
     }
     return true;
}

function validateZip(zipField)
{
     var zipFieldValue = zipField.value;

     if(zipFieldValue != null && zipFieldValue != "")
     {
          matchZip = zipFieldValue.match(/^(\d{9}|\d{5}-{1}\d{4}|\d{5}|\D{1}\d{1}\D{1}\s?\d{1}\D{1}\d{1})$/); // accepts 99999-9999 or 999999999 or 99999 or Z9Z 9Z9 or Z9Z9Z9
          
          if(!matchZip)
          {
               alert("The zip code you entered is invalid.  Please enter the zip code in the form 99999-9999, 999999999, 99999, Z9Z 9Z9 or Z9Z9Z9.");

//               fieldPlace = getPosition(zipField);
//               setTimeout("window.document.IncidentReport.elements[fieldPlace].focus()", 100);
//               setTimeout("window.document.IncidentReport.elements[fieldPlace].select()", 100);
               return false;
          }
     }
     return true;
}

function validateAcctNo(acctNoField)
{
     var acctNoFieldValue = acctNoField.value;

     if(acctNoFieldValue != null && acctNoFieldValue != "")
     {
          //matchAcctNo = acctNoFieldValue.match(/^(\d{9}|\d{5}-{1}\d{4}|\d{5}|\D{1}\d{1}\D{1}\s?\d{1}\D{1}\d{1})$/); // accepts 99-99999 or 9999999
          //matchAcctNo = acctNoFieldValue.match(/^(\d{7}|\d{2}-{1}\d{5})$/); // accepts 99-99999 or 9999999
          matchAcctNo = acctNoFieldValue.match(/^(\d{7})$/); // accepts 9999999
          
          if(!matchAcctNo)
          {
               alert("The account number you entered is invalid.  Please enter the account number in the form 9999999.");

//               fieldPlace = getPosition(acctNoField);
//               setTimeout("window.document.IncidentReport.elements[fieldPlace].focus()", 100);
//               setTimeout("window.document.IncidentReport.elements[fieldPlace].select()", 100);
               return false;
          }
     }
     return true;
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