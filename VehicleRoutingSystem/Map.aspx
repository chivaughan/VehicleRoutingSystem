<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Map.aspx.cs" Inherits="VehicleRoutingSystem.Map" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vehicle Routing System using Ant Colony Optimisation</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="description" content="Place your description here" />
<meta name="keywords" content="put, your, keyword, here" />
<meta name="author" content="Templates.com - website templates provider" />
<link href="style.css" rel="stylesheet" type="text/css" />
<script src="Scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
<script src="Scripts/cufon-yui.js" type="text/javascript"></script>
<script src="Scripts/cufon-replace.js" type="text/javascript"></script>
<script src="Scripts/Myriad_Pro_400.font.js" type="text/javascript"></script>
<script src="Scripts/Myriad_Pro_600.font.js" type="text/javascript"></script>
<script src="Scripts/NewsGoth_BT_400.font.js" type="text/javascript"></script>
<script src="Scripts/NewsGoth_BT_700.font.js" type="text/javascript"></script>
<script src="Scripts/NewsGoth_Dm_BT_400.font.js" type="text/javascript"></script>
<script src="Scripts/script.js" type="text/javascript"></script>
<!--[if lt IE 7]>
	<script type="text/javascript" src="Scripts/ie_png.js"></script>
	<script type="text/javascript">
		 ie_png.fix('.png, #header .row-2 ul li a, .extra img, #search-form a, #search-form a em, #login-form .field1 a, #login-form .field1 a em, #login-form .field1 a b');
	</script>
	<link href="ie6.css" rel="stylesheet" type="text/css" />
<![endif]-->
<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
<script language="javascript" type="text/javascript">
    var directionsDisplay;
    var directionsService = new google.maps.DirectionsService();

    function InitializeMap() {
        directionsDisplay = new google.maps.DirectionsRenderer();
        var latlng = new google.maps.LatLng(6.4500, 3.3833);
        var myOptions =
        {
            zoom: 8,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("map"), myOptions);

        directionsDisplay.setMap(map);
        directionsDisplay.setPanel(document.getElementById('directionpanel'));

        var control = document.getElementById('control');
        control.style.display = 'block';


    }


    function calcRoute() {
        // Get the start and end points
        var start = document.getElementById('txtOrigin').value;
        var end = document.getElementById('txtDestination').value;

        // Get the selected mode of travel
        var selectedModeOfTravel = document.getElementById('ddlModeOfTravel').value;

        // Create the request
        var request = {
            origin: start,
            destination: end,
            travelMode: google.maps.DirectionsTravelMode[selectedModeOfTravel], // Set the selected mode of travel
            provideRouteAlternatives: chkShowAlternativeRoutes.checked, // Choose whether to provide alternative routes
            avoidTolls: chkAvoidTolls.checked, // Choose whether to avoid toll roads
            avoidHighways: chkAvoidHighways.checked, // Choose whether to avoid highways
            //optimizeWaypoints: chkOptimizeWayPoints.checked // Choose whether to optimize waypoints

        };
        directionsService.route(request, function (response, status) {
            if (status == google.maps.DirectionsStatus.OK) {
                directionsDisplay.setDirections(response);
            }
        });

    }



    function GetDirections_onclick() {
        calcRoute();
    }

    window.onload = InitializeMap;
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="main">
<!-- HEADER -->
	<div id="header">
		<div class="row-1">
			<div class="fleft"><a href="Default.aspx"><img src="images/logo.png" alt="" /></a></div>
			
		</div>
		<div class="row-2">
			<div class="left">
				<ul>
					<li><a href="Default.aspx"><span>Home</span></a></li>
					<li><a href="DefineCVRP.aspx"><span>Define Problem</span></a></li>
					<li><a href="Map.aspx"  class="active"><span>Map</span></a></li>
				</ul>
			</div>
		</div>
		<div class="row-3">
			<div class="inside">
				<h2>More Efficient<b>Routes</b></h2>
				<p>Produce better routes, necessitating fewer vehicles and drivers while enabling more products to be delivered on the same fleet</p>
				<a href="DefineCVRP.aspx" class="link1"><em><b>Start Defining Problem!</b></em></a>
			</div>
		</div>
		<div class="extra"><img src="images/header-img.png" alt="" /></div>
	</div>
<!-- CONTENT -->
	<div id="content">
		
		<div class="indent">
			<div class="wrapper"><br /><br /><br />
				<div>
					<h3><b>Welcome to</b> Map</h3>
					<p>This web application solves the Capacitated Vehicle Routing Problem using Ant Colony Optimisation</p>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td>
    <br />
    <label id="Label1" for ="txtOrigin">From: </label>
    <input type = "text" id="txtOrigin" class="txtMap"/>&nbsp;&nbsp;&nbsp;&nbsp;
    <label id="Label2" for="txtDestination">To: </label>
    <input type= "text" id="txtDestination" class="txtMap"/>
    <input type="button" id="btnGetDirections" value="Get Directions" class ="btnMap" onclick = "return GetDirections_onclick();"/><br /><br/>
    <label  id="Label3" for="ddlModeOfTravel">Mode of Travel: </label>
    <select id="ddlModeOfTravel">
    <option value= "DRIVING" selected="selected">Driving</option>
    <option value= "WALKING">Walking</option>
    <option value= "BICYCLING">Bicycling</option>
    </select>&nbsp;&nbsp;
    <input type="checkbox" id ="chkAvoidTolls" /> Avoid Tolls&nbsp;&nbsp;
     <input type="checkbox" id ="chkAvoidHighways" /> Avoid Highways&nbsp;&nbsp;
      <input type="checkbox" id ="chkShowAlternativeRoutes" /> Show Alternative Routes&nbsp;&nbsp;
      <br /><br />
      </td>
   </tr>  
   </table>
   <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
   <tr>
<td valign ="top">
<div id ="directionpanel"  style="height: 390px;overflow: auto;" ></div>
</td>
<td valign ="top">
<div id ="map" style="height: 390px; width: 489px;"></div>
</td>
</tr> 
</table>
				</div>
				
			</div>
		</div>
	</div>
<!-- FOOTER -->
	<div id="footer">
		
		<div class="bottom">Developed by OBI CHINEDU EMMANUEL<br/>
			</div>
	</div>
</div>
<script type="text/javascript">    Cufon.now(); </script>
    </form>
</body>
</html>
