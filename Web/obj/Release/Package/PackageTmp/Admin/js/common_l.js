function ShowHideObject(objID)
{
	var obj=eval("document.all."+objID);
	if(obj.style.display=="none")
	{
		obj.style.display="block";
	}
	else
	{
		obj.style.display="none";
	}
}


//Move item up
function moveUp(oSelect)
{
	var i, count=0;
	var display_1, display_2, value_1, value_2;
	
	if (oSelect.selectedIndex != -1)
	{
	  for (i=0;i<oSelect.length;i++)
				if (oSelect.options[i].selected)
					 count ++;
		
		if (count > 1)
		{
		  alert("Sorry. Only one option can be moved up at a time");
		}
		else
		{
		  i = oSelect.selectedIndex;
			if (i > 0)
			{
			display_1 = oSelect.options[i].text;
			display_2 = oSelect.options[i-1].text;
				
			value_1 = oSelect.options[i].value;
			value_2 = oSelect.options[i-1].value;
				
			oSelect.options[i].text = display_2;
			oSelect.options[i].value = value_2;
			oSelect.options[i].selected = false
					
			oSelect.options[i-1].text = display_1;
			oSelect.options[i-1].value = value_1;
			oSelect.options[i-1].selected = true;
					
			oSelect.selectedIndex = i-1;
			}
	  }
	}
	
	return true;
}

//Move item down
function moveDown(oSelect)
{
	var i, count=0;
	var display_1, display_2, value_1, value_2;
	
	if (oSelect.selectedIndex != -1)
	{
	  for (i=0;i<oSelect.length;i++)
				if (oSelect.options[i].selected)
					 count ++;
					 
		if (count > 1)
		{
		  alert("Only one reviewer can be moved down at a time")
		}
		else
		{
		  i = oSelect.selectedIndex;
			if (i < oSelect.length - 1)
			{
			display_1 = oSelect.options[i].text;
			display_2 = oSelect.options[i+1].text;
				
			value_1 = oSelect.options[i].value;
			value_2 = oSelect.options[i+1].value;
				
			oSelect.options[i].text = display_2;
			oSelect.options[i].value = value_2;
			oSelect.options[i].selected = false;
				
			oSelect.options[i+1].text = display_1;
			oSelect.options[i+1].value = value_1;
			oSelect.options[i+1].selected = true;
					
			oSelect.selectedIndex = i+1;
			}
	  }
	}
	
	return true;
}

//Resize the modal dialog

function ResizeDialog(width,height)
{
	var vOpener=window.opener;
	if(vOpener=="undefined" || vOpener==null)//modal dialog
	{
		window.dialogWidth=width+"px";	
		window.dialogHeight=height+"px";
		window.dialogTop=(screen.height-height)/2 + "px";
		window.dialogLeft=(screen.width-width)/2 + "px";
	}
	else //normal window
	{
		window.moveTo((screen.width-width)/2,(screen.height-height-76)/2)		
		window.resizeTo(width,height+76)
	}
}

//Open the modal window

function OpenModalWindow(url,w,h)
{
	   if (url.indexOf('?') > -1 ) {
        url += "&";
    } else {
		url += "?";
    }
	url += 't=' + Math.random()
	o = 'dialogWidth=' + w + "px;dialogHeight=" + h + "px;scroll=no;help=no";
	var ret = window.showModalDialog(url, '', o);

	
	if (typeof(ret)!="undefined" && ret!="")
	{
		redirectUrl(window.location.href);
		
	}

}
//重定向
function redirectUrl(url)
{
	var frm = document.createElement("form");
	document.body.insertBefore(frm);
	frm.method = "post";
	frm.action = url;
	frm.submit();
}
function getUrlParam(param)
{
	var re = new RegExp("(\\\?|&)" + param + "=([^&]+)(&|$)", "i");
	var m = document.location.href.match(re);
	if (m)
		return m[2];
	else
		return '';
}
// RefreshFrom
function RefrechFrom(url)
{
	var vTmd=Math.random();
	var vReturn='';
	vReturn=window.showModalDialog(url+'&tmd='+vTmd,null,'status:0;scroll:0;help:0;');
	if(typeof(vReturn)!="undefined" && vReturn!="")
	{
		window.location.reload();
	}
}

//Get PickerBase Control's Value
function GetPickerBaseValue(ctl)
{
	return eval("document.all."+ctl+"_Value.value");
}

//Get RadioButtonList Checked Value
function GetRadioButtonListCheckedValue(ctl)
{
	var oRadio=document.getElementById(ctl);
	var oRadioButtons=oRadio.getElementsByTagName("input");
	for(var i=0;i<oRadioButtons.length;i++)
	{
		if(oRadioButtons[i].type=="radio" && oRadioButtons[i].checked)
		{
			return oRadioButtons[i].value;
		}
	}
	
	return "";
}
//select change Color
function selectArow(sObject,GridID)
{
	if(sObject.attributes.getNamedItem("class").value!="DataGridHeader" && sObject.attributes.getNamedItem("class").value!="DataGridFooter")
	{
		for(var i=0;i<document.all.item(GridID).rows.length;i++)
			document.all.item(GridID).rows(i).bgColor=0xffffff;
		sObject.attributes.getNamedItem("class").value="";
		sObject.bgColor=0xDAE1F3;
	}
	else
	{
		for(var i=0;i<document.all.item(GridID).rows.length;i++)
			document.all.item(GridID).rows(i).bgColor=0xffffff;
	}
	
}

// Chenyh added begin -->
function openModalDialog(url,w,h)
{
    if (url.indexOf('?') > -1 ) {
        url += "&";
    } else {
		url += "?";
    }
	url += 't=' + Math.random()
	o = 'dialogWidth=' + w + "px;dialogHeight=" + h + "px;scroll=yes;help=no";
	var ret = window.showModalDialog(url, '', o);

	if (ret)
	{
		
		location.reload();	
		//__doPostBack('','');
	}
	
	// for debug below:
	//window.open(url,'','scrollbars=yes,resizable=yes,status=yes,dialogWidth=' + w + "px,dialogHeight=" + h + "px");
}


function openWindow(url,w,h)
{
	var left = (screen.width - w)/2;
	var top  = (screen.height - h)/2;
	var tempWin='win'+Math.round(Math.random()*1000);
	var tt=window.open(url,tempWin,'width='+w +',height='+h+',location=no,menubar=no,scrollbars=yes,resizable=yes,titlebar=no,toolbar=no,left='+left+',top='+top);
	tt.focus()
	//alert(tt);
}

function openWindow1(url,w,h)
{
	var left = (screen.width - w)/2;
	var top  = (screen.height - h)/2;
	if (url.indexOf('?') > -1 ) {
        url += "&";
    } else {
		url += "?";
    }
	url += 't=' + Math.random()
	
	var ret = window.open(url,'newwindow','width='+w +',height='+h+',status=yes,location=no,menubar=no,scrollbars=yes,resizable=no,titlebar=no,toolbar=no,left='+left+',top='+top);
	
}



function SelectAll(chkVal, AllId,SelId) { 
    var thisfrm = document.forms[0];
    // 查找Forms里面所有的元素
    for (i = 0; i < thisfrm.length; i++) {
		// 查找模板头中的CheckBox
		if (AllId.indexOf(AllId) != -1 && thisfrm.elements[i].id.indexOf(SelId) != -1) {
            if (chkVal) {
                 thisfrm.elements[i].checked = true;
            } else {
                 thisfrm.elements[i].checked = false;
            }
        } // if

       // 如果除题头以外的项没有全选上则取消题头的选择
        else if (AllId.indexOf (SelId) != -1) {
             if (!thisfrm.elements[i].checked) {
                 thisfrm.elements[i].checked = false; 
             }
        }
    } // for
}

function openModalDialog11(url,w,h)
{
    if (url.indexOf('?') > -1 ) {
        url += "&";
    } else {
		url += "?";
    }
	url += 't=' + Math.random()
	o = 'dialogWidth=' + w + "px;dialogHeight=" + h + "px;scroll=yes;help=no";
	var ret = window.showModalDialog(url, '', o);

	if (ret)
	{
		//__doPostBack('','__RefreshDataGrid')
		__doPostBack('btnQuery',''); 
		//location.reload();
		//redirectUrl(window.location.href);
	}
	
	// for debug below:
	//window.open(url,'','scrollbars=yes,resizable=yes,status=yes,dialogWidth=' + w + "px,dialogHeight=" + h + "px");
}

function openModalDialog10(url,w,h)
{
    if (url.indexOf('?') > -1 ) {
        url += "&";
    } else {
		url += "?";
    }
	url += 't=' + Math.random()
	o = 'dialogWidth=' + w + "px;dialogHeight=" + h + "px;scroll=yes;help=no";
	var ret = window.showModalDialog(url,window, o);

	if (ret)
	{		
		return true;	
	}
	else
	{
		return false;
	}
}

function openModalDialog19(url,w,h)
{
    if (url.indexOf('?') > -1 ) {
        url += "&";
    } else {
		url += "?";
    }
	url += 't=' + Math.random()
	o = 'dialogWidth=' + w + "px;dialogHeight=" + h + "px;scroll=yes;help=no";
	var ret = window.showModalDialog(url, '', o);

return false;
//	if (ret)
//	{		
//		return true;	
//	}
//	else
//	{
//		return false;
//	}
}


//<-- chenyh add end

//设置chkAll的勾选状态。
//当所有chkSel勾选时，则chkAll自动勾选，当有一个chkSel未勾选，则chkAll取消勾选
//add by zhaojg  2006-11-11
function SetchkAll(chkVal,AllId,SelId) 
{ 
    
    var thisfrm = document.forms[0];
    var bolFlag = true;
    var strName="";
    
    if (chkVal)
    {
     // 查找Forms里面所有的元素
		for (i = 0; i < thisfrm.length; i++) 
		{
			// 查找模板头中的CheckBox
			strName = thisfrm.elements[i].name;
			
			if (strName.indexOf (SelId) != -1) 
			{
				if (!thisfrm.elements[i].checked )
				{
					bolFlag=false;
					break;
				}
	            
			} // if       
		} // for
    }
    else
    {
		bolFlag=false;
    }    
    
	for (i = 0; i < thisfrm.length; i++) 
	{
		// 查找模板头中的chkAll
		strName = thisfrm.elements[i].name;
	    
		if (strName.indexOf (AllId) != -1) 
		{	
			if (bolFlag)
			{
				thisfrm.elements[i].checked = true; 			
	        }
	        else
	        {
				thisfrm.elements[i].checked = false; 
	        }
	        break;
		} // if       
	} // for
}
function _onCheckNumber()
{
	if(event.keyCode < 48 || event.keyCode > 57)
		event.returnValue = false;
}

function _onkeyup()
{
	
}

function _onkeypress()
{
	if((event.keyCode < 48 || event.keyCode > 57) && event.keyCode!=46) 
		event.returnValue = false;
}


// 检查日期 YYYY-MM-DD  46为 ASC("-")
function _onCheckDate()
{
	if((event.keyCode < 48 || event.keyCode > 57) && event.keyCode!=46 && event.keyCode!=45) 
		event.returnValue = false;
}


function SelectAll_kp(chkVal, idVal,objName_sel,objName_all) { 
    var thisfrm = document.forms[0];
    // 查找Forms里面所有的元素
    for (i = 0; i < thisfrm.length; i++) {
		// 查找模板头中的CheckBox
		if (idVal.indexOf (objName_all) != -1 && thisfrm.elements[i].id.indexOf(objName_sel) != -1) {
            if (chkVal) {
                 thisfrm.elements[i].checked = true;
            } else {
                 thisfrm.elements[i].checked = false;
            }
        } // if

       // 如果除题头以外的项没有全选上则取消题头的选择
        else if (idVal.indexOf (objName_sel) != -1) {
             if (!thisfrm.elements[i].checked) {
                 thisfrm.elements[i].checked = false; 
             }
        }
    } // for
}

//设置chkAll的勾选状态。
//当所有chkSel勾选时，则chkAll自动勾选，当有一个chkSel未勾选，则chkAll取消勾选
//add by zhaojg  2006-11-11
function SetchkAll_kp(chkVal,idVal,objName_sel,objName_all) 
{ 
	
    var thisfrm = document.forms[0];
    var bolFlag = true;
    var strName="";
    
    if (chkVal)
    {
     // 查找Forms里面所有的元素
		for (i = 0; i < thisfrm.length; i++) 
		{
			// 查找模板头中的CheckBox
			strName = thisfrm.elements[i].name;
			
			if (strName.indexOf (objName_sel) != -1) 
			{
				if (!thisfrm.elements[i].checked )
				{
					bolFlag=false;
				}
	            
			} // if       
		} // for
    }
    else
    {
		bolFlag=false;
    }    
   
	for (i = 0; i < thisfrm.length; i++) 
	{
		// 查找模板头中的chkAll
		strName = thisfrm.elements[i].name;
			
		if (strName.indexOf (objName_all) != -1) 
		{	
			if (bolFlag)
			{
				thisfrm.elements[i].checked = true; 			
	        }
	        else
	        {
				thisfrm.elements[i].checked = false; 
	        }
	        break;
		} // if       
	} // for
}


// heys added
function OpenModalDialogRefresh(url,w,h,refreshButtonName)
{
    if (url.indexOf('?') > -1 ) {
        url += "&";
    } else {
		url += "?";
    }

	url += 't=' + Math.random()

	o = 'dialogWidth=' + w + "px;dialogHeight=" + h + "px;scroll=yes;help=no";

	var ret = window.showModalDialog(url, '', o);

	if (ret)
	{
		//__doPostBack('','__RefreshDataGrid')
		
		__doPostBack(refreshButtonName,''); 
	}

}

// 浮动显示帮助 :
// 	<div id="win_info" style="position:absolute; visibility:hidden; left:0px; top:0px; width:15px; height:10px; background-color:#BBE2FF; z-index:1" onMouseOver="javascript:show_info('win_info','user_info','0')"; onMouseOut="hide_info('win_info');">
//		<textarea name="user_info" cols="30" rows="4"  readonly="readonly" style="font-size:12px; height:150px; width:150px; overflow:hidden;background-color:#BBE2FF;border:0px"></textarea>
//	</div>
function hide_info(layer_tmp)	
{
	layer_tmp=eval(layer_tmp);
	layer_tmp.style.visibility="hidden";
}

function show_info(layer_tmp,win_tmp,str_tmp)	
{
	var mou_x;
	var mou_y;
	layer_tmp=eval(layer_tmp);
	win_tmp=eval(win_tmp);
	if(event.clientX>=500)  mou_x=parseInt(event.clientX)-parseInt(win_tmp.style.width)+10;
	else
	mou_x=parseInt(event.clientX);
	
	mou_y=document.body.clientHeight-event.clientY
	if (mou_y<layer_tmp.offsetHeight)
		mou_y=document.body.scrollTop+event.clientY-layer_tmp.offsetHeight
	else
		mou_y=document.body.scrollTop+event.clientY

	layer_tmp.style.left=mou_x;
	layer_tmp.style.top=mou_y;
	layer_tmp.style.visibility="visible";
	
	if(str_tmp!="0")  win_tmp.value=str_tmp;
}

function OpenModalWindowNew(url)
{
	var vTmd=Math.random();
	var vReturn='';
	if(url.indexOf('?')>-1)
	{
		vReturn=window.showModalDialog(url+'&tmd='+vTmd,window,'status:0;scroll:0;help:0;');
	}
	else
	{
		vReturn=window.showModalDialog(url+'?tmd='+vTmd,window,'status:0;scroll:0;help:0;');
	}
	//if(typeof(vReturn)!="undefined" && vReturn!="")
	//{
	
	//	__doPostBack('','__RefreshDataGrid');
	//}
}

