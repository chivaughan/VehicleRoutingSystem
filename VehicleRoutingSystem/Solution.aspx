<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Solution.aspx.cs" Inherits="VehicleRoutingSystem.Solution" %>

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
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
<script>
    var map;
    var geocoder;
    var bounds = new google.maps.LatLngBounds();
    var markersArray = [];

    var origin1;
    var destinationA;


    var destinationIcon = 'https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld=D|FF0000|000000';
    var originIcon = 'https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld=O|FFFF00|000000';

    function initialize() {
        var opts = {
            center: new google.maps.LatLng(6.4500, 3.3833),
            zoom: 10,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        map = new google.maps.Map(document.getElementById('map-canvas'), opts);
        geocoder = new google.maps.Geocoder();
    }

    function calculateDistances() {
        origin1 = document.getElementById("txtFrom").value;
        destinationA = document.getElementById("txtTo").value;
        var service = new google.maps.DistanceMatrixService();
        service.getDistanceMatrix(
    {
        origins: [origin1],
        destinations: [destinationA],
        travelMode: google.maps.TravelMode.DRIVING,
        unitSystem: google.maps.UnitSystem.METRIC,
        avoidHighways: false,
        avoidTolls: false
    }, callback);
    }

    function callback(response, status) {
        if (status != google.maps.DistanceMatrixStatus.OK) {
            alert('Error was: ' + status);
        } else {
            var origins = response.originAddresses;
            var destinations = response.destinationAddresses;
            var outputDiv = document.getElementById('outputDiv');
            outputDiv.innerHTML = '';
            deleteOverlays();

            for (var i = 0; i < origins.length; i++) {
                var results = response.rows[i].elements;
                addMarker(origins[i], false);
                for (var j = 0; j < results.length; j++) {
                    addMarker(destinations[j], true);
                    outputDiv.innerHTML += origins[i] + ' to ' + destinations[j]
            + ': <b>' + results[j].distance.text + ' in '
            + results[j].duration.text + '</b><br>';
                }
            }
        }
    }

    function addMarker(location, isDestination) {
        var icon;
        if (isDestination) {
            icon = destinationIcon;
        } else {
            icon = originIcon;
        }
        geocoder.geocode({ 'address': location }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                bounds.extend(results[0].geometry.location);
                map.fitBounds(bounds);
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location,
                    icon: icon
                });
                markersArray.push(marker);
            } else {
                alert('Geocode was not successful for the following reason: '
        + status);
            }
        });
    }

    function deleteOverlays() {
        for (var i = 0; i < markersArray.length; i++) {
            markersArray[i].setMap(null);
        }
        markersArray = [];
    }

    google.maps.event.addDomListener(window, 'load', initialize);

    </script></head>
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
					<li><a href="Default.aspx" class="active"><span>Home</span></a></li>
					<li><a href="DefineCVRP.aspx"><span>Define Problem</span></a></li>
					<li><a href="Map.aspx"><span>Map</span></a></li>
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
				<div class="col-1">
					<h3><b>Problem</b> Solved!</h3>
					<p>Now that was a piece of cake. The solution is displayed below</p>
                    <p>If you feel like solving another problem, feel free to restart me.</p>
                    <br />                    
                    <h3>Parameters</h3>
                    Number cities in problem = &nbsp;<strong><asp:Label runat="server" ID="lblNoOfCities"></asp:Label></strong>
                    <br /><br />
                    Number of Ants = &nbsp;<strong><asp:Label runat="server" ID="lblNoOfAnts"></asp:Label></strong>
                    <br /><br />
                    Maximum Time = &nbsp;<strong><asp:Label runat="server" ID="lblMaxTime"></asp:Label></strong>
                    <br /><br />
                    Alpha (pheromone influence) = &nbsp;<strong><asp:Label runat="server" ID="lblAlpha"></asp:Label></strong>
                    <br /><br />
                    Beta (local node influence) = &nbsp;<strong><asp:Label runat="server" ID="lblBeta"></asp:Label></strong>
                    <br /><br />
                    Rho (pheromone evaporation coefficient) = &nbsp;<strong><asp:Label runat="server" ID="lblRho"></asp:Label></strong>
                    <br /><br />
                    Q (pheromone deposit factor) = &nbsp;<strong><asp:Label runat="server" ID="lblQ"></asp:Label></strong>
                    <br /><br /><br />
                    <h3>Initial Random Trails</h3>
                    <strong><asp:Label runat="server" ID="lblInitialRandomTrails"></asp:Label></strong>
                    <br />
                    Best initial trail length: &nbsp;<strong><asp:Label runat="server" ID="lblBestInitialTrailLength"></asp:Label></strong>
                    <br /><br /><br />
                    <h3>Best Trail Found</h3>
                    <strong><asp:Label runat="server" ID="lblBestTrailFound"></asp:Label></strong>
                    <br /><br />
                    Length of Best Trail Found: &nbsp;<strong><asp:Label runat="server" ID="lblLengthOfBestTrailFound"></asp:Label></strong>
                    <br /><br />
                   

				</div>
				<div class="col-2">
					<div class="box2">
						<div class="border-top">
							<div class="left-top-corner">
								<div class="right-top-corner">
									<div class="inner">
										<h4><b>Calculate</b> Distance</h4>
										
<div id ="map-canvas" style="height: 300px; width: 100%;"></div>
<br />
<div style="color:#fff;" id="outputDiv"></div>
<br />
										
											<div><label style="color:#fff;">From: </label><input type="text" class="txt" id="txtFrom" name="txtFrom"/></div>
                                            <br />
											<div><label style="color:#fff;">To: &nbsp;&nbsp;&nbsp;</label><input type="text" class="txt" id="txtTo" name="txtTo"/></div>
                                            <br />
											<div><button type="button" id="btnCalculateDistance" onclick="calculateDistances();" style="float:right;">Calculate Distance</button></div>
											<br /><br /><br />
									</div>
								</div>
							</div>
						</div>
					</div>
					<div class="box3">
						<div class="right-bot-corner">
							<div class="left-bot-corner">
								
							</div>
						</div>
					</div>
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
