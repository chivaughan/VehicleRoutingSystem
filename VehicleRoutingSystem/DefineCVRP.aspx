<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefineCVRP.aspx.cs" Inherits="VehicleRoutingSystem.DefineCVRP" %>



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
					<li><a href="DefineCVRP.aspx" class="active"><span>Define Problem</span></a></li>
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
                <p><asp:Label runat="server" ID="lblMessage"></asp:Label></p>					
					<asp:Panel runat="server" ID="Panel1">
                    <h3><b>Define</b> Problem</h3>
                    <p>Here's where you can create a new problem for me to solve or if you have an already saved one, tell me to load it. Select from the two options given below.</p>
                    
					<p>
                    <asp:RadioButton runat="server" ID="rbtnCreateProblem" Text ="Create Problem" 
                            Checked="true" GroupName="Problem" AutoPostBack="True" 
                            oncheckedchanged="rbtnCreateProblem_CheckedChanged"/> <br /><br />
                    <asp:RadioButton runat="server" ID="rbtnLoadProblem" Text ="Load Problem" 
                            GroupName="Problem" AutoPostBack="True" 
                            oncheckedchanged="rbtnLoadProblem_CheckedChanged"/>
                    <asp:FileUpload runat="server" ID="fuUploadProblem"/>
                    </p>
                    <div>
							<asp:Button runat="server" ID="btnNext1" Text ="Next" CssClass="btnNext" 
                                Width="125px" Height="35px" onclick="btnNext1_Click" />
					</div>
					</asp:Panel>
                    <asp:Panel runat="server" ID="Panel2">
                    <h3><b>Enter/Edit</b> ACO Parameters</h3>
					<p>Please specify the Ant Colony Optimisation Parameters</p>
                    
					<p>
                    Alpha (Pheromone Influence): &nbsp;<asp:TextBox runat="server" ID="txtAlpha" CssClass="txtInput" Text="3"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ReqiredFieldValidator1" runat="server" ControlToValidate="txtAlpha" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="Parameters">*</asp:RequiredFieldValidator>
                                   <asp:RegularExpressionValidator ID = "RegularExpressionValidator1" runat="server" ControlToValidate="txtAlpha" ValidationExpression="^\d+(\.\d+)?$" Text="*" ValidationGroup="Parameters"></asp:RegularExpressionValidator>
                    <br /><br />
                    Beta (Local Node Influence): &nbsp;<asp:TextBox runat="server" ID="txtBeta" CssClass="txtInput" Text="2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBeta" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="Parameters">*</asp:RequiredFieldValidator>
                                                                      <asp:RegularExpressionValidator ID = "RegularExpressionValidator2" runat="server" ValidationGroup="Parameters" ControlToValidate="txtBeta" ValidationExpression="^\d+(\.\d+)?$" Text="*"></asp:RegularExpressionValidator>

                    <br /><br />
                    Rho (Pheromone Evaporation Coefficient): &nbsp;<asp:TextBox runat="server" ID="txtRho" CssClass="txtInput" Text="0.01"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRho" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="Parameters">*</asp:RequiredFieldValidator>
                                                                                                         <asp:RegularExpressionValidator ID = "RegularExpressionValidator3" ValidationGroup="Parameters" runat="server" ControlToValidate="txtRho" ValidationExpression="^\d+(\.\d+)?$" Text="*"></asp:RegularExpressionValidator>

                    <br /><br />
                    Q (Pheromone Deposit Factor): &nbsp;<asp:TextBox runat="server" ID="txtQ" CssClass="txtInput" Text="2.00"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtQ" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="Parameters">*</asp:RequiredFieldValidator>
                                                                                                                                            <asp:RegularExpressionValidator ID = "RegularExpressionValidator4" ValidationGroup="Parameters" runat="server" ControlToValidate="txtQ" ValidationExpression="^\d+(\.\d+)?$" Text="*"></asp:RegularExpressionValidator>

                                   <br /><br />
                    Number of Ants: &nbsp;<asp:TextBox runat="server" ID="txtNoOfAnts" CssClass="txtInput" Text="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNoOfAnts" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="Parameters">*</asp:RequiredFieldValidator>
                                                                                                                                            <asp:RegularExpressionValidator ID = "RegularExpressionValidator5" ValidationGroup="Parameters" runat="server" ControlToValidate="txtNoOfAnts" ValidationExpression="^\d+$" Text="*"></asp:RegularExpressionValidator>

                                   <br /><br />
                    Maximum Time: &nbsp;<asp:TextBox runat="server" ID="txtMaxTime" CssClass="txtInput" Text="1000"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtMaxTime" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="Parameters">*</asp:RequiredFieldValidator>
                                                                                                                                                                                 <asp:RegularExpressionValidator ID = "RegularExpressionValidator7" ValidationGroup="Parameters" runat="server" ControlToValidate="txtNoOfAnts" ValidationExpression="^\d+$" Text="*"></asp:RegularExpressionValidator>
                                                                                                                                          <asp:RegularExpressionValidator ID = "RegularExpressionValidator6" ValidationGroup="Parameters" runat="server" ControlToValidate="txtMaxTime" ValidationExpression="^\d+$" Text="*"></asp:RegularExpressionValidator>

                    </p>
                    <div>
                    <asp:Button runat="server" ID="btnPrevious2" Text ="Previous" CssClass="btn" 
                            Width="125px" Height="35px" onclick="btnPrevious2_Click" />
							<asp:Button runat="server" ID="btnNext2" Text ="Next" CssClass="btnNext" 
                            Width="125px" Height="35px" onclick="btnNext2_Click" ValidationGroup="Parameters"/>
					</div>
					</asp:Panel>
                    <asp:Panel runat="server" ID="Panel3">
                    <h3><b>Name the</b> Warehouse</h3>
					<p>You can type the name of the warehouse location below</p>
                    
					<p>
                    <asp:TextBox runat="server" ID="txtWarehouse" CssClass="txt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtWarehouse" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="Warehouse">*</asp:RequiredFieldValidator>
                    </p>
                    <div>
                    <asp:Button runat="server" ID="btnPrevious3" Text ="Previous" CssClass="btn" 
                            Width="125px" Height="35px" onclick="btnPrevious3_Click" />
							<asp:Button runat="server" ID="btnNext3" Text ="Next" CssClass="btnNext" 
                            Width="125px" Height="35px" onclick="btnNext3_Click" ValidationGroup="Warehouse"/>
					</div>
					</asp:Panel>
                    <asp:Panel runat="server" ID="Panel4">
                    <h3><b>Enter/Edit</b> Truck Capacity</h3>
					<p>Please specify the maximum amount of units a truck can carry below</p>
                    
					<p>
                    <asp:TextBox runat="server" ID="txtTruckCapacity" CssClass="txt"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtTruckCapacity" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="TruckCapacity">*</asp:RequiredFieldValidator>
                                                                                                                                                                               <asp:RegularExpressionValidator ID = "RegularExpressionValidator8" ValidationGroup="TruckCapacity" runat="server" ControlToValidate="txtTruckCapacity" ValidationExpression="^\d+(\.\d+)?$" Text="*"></asp:RegularExpressionValidator>

                    </p>
                    <div>
                    <asp:Button runat="server" ID="btnPrevious4" Text ="Previous" CssClass="btn" 
                            Width="125px" Height="35px" onclick="btnPrevious4_Click" />
							<asp:Button runat="server" ID="btnNext4" Text ="Next" CssClass="btnNext" 
                            Width="125px" Height="35px" onclick="btnNext4_Click" ValidationGroup="TruckCapacity"/>
					</div>
					</asp:Panel>
                    <asp:Panel runat="server" ID="Panel5">
                    <h3><b>Enter/Edit</b> Node Information</h3>
					<p>You can enter the locations one after the other along with their demands below.</p>
                    
					<p>
                    Node: <asp:TextBox runat="server" ID="txtNewNode" CssClass="txt" Width="125px"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtNewNode" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="NodeInformation">*</asp:RequiredFieldValidator>
                      &nbsp; Demand: <asp:TextBox runat="server" ID="txtNewDemand" CssClass="txt" Width="50px"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtNewDemand" ErrorMessage="*" 
                                   ForeColor="Red" ValidationGroup ="NodeInformation">*</asp:RequiredFieldValidator>
                                                                                                                                                                               <asp:RegularExpressionValidator ID = "RegularExpressionValidator9" ValidationGroup="NodeInformation" runat="server" ControlToValidate="txtNewDemand" ValidationExpression="^\d+(\.\d+)?$" Text="*"></asp:RegularExpressionValidator>

                      &nbsp;
                      <asp:LinkButton runat="server" Text = "Add Node" ID="lnkAddNode" 
                            onclick="lnkAddNode_Click" ValidationGroup ="NodeInformation"></asp:LinkButton>
                    </p>
                       
                         <asp:GridView runat="server" ID= "grvNodeInformation2" ShowFooter="true" GridLines="None" ForeColor="#333333" Width="100%"
                            onrowcommand="grvNodeInformation2_RowCommand" 
                            onrowcancelingedit="grvNodeInformation2_RowCancelingEdit" 
                            onrowediting="grvNodeInformation2_RowEditing" 
                            onrowupdating="grvNodeInformation2_RowUpdating" AllowPaging="true" PageSize="100">
                            <RowStyle ForeColor="#333333" BackColor="#F7F6F3"/>
                            <PagerStyle BackColor="#1f7dd2" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#1f7dd2" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
                            
                         </asp:GridView>
                      
                      
                            <br /><br /><br />
                    <div>
                    <asp:Button runat="server" ID="btnPrevious5" Text ="Previous" CssClass="btn" 
                            Width="125px" Height="35px" onclick="btnPrevious5_Click" />
							<asp:Button runat="server" ID="btnNext5" Text ="Next" CssClass="btnNext" 
                            Width="125px" Height="35px" onclick="btnNext5_Click"/>
					</div>
					</asp:Panel>
                    
                    <asp:Panel runat="server" ID="Panel7">
                    <h3><b>Solve</b> It</h3>
					<p>Yippeee. Now I can solve this problem for you. Please click "Next" to view the solution. Solving the problem may take some time depending on the number of locations. You may also want to save your progress upto this point using the 'Save Problem' button below.</p>
                    
					<p align="center">
                    <asp:LinkButton runat="server" ID="lnkSave" Text = "Save Problem" 
                            onclick="lnkSave_Click"></asp:LinkButton>
                    </p>
                    <div>
                    <asp:Button runat="server" ID="btnPrevious7" Text ="Previous" CssClass="btn" 
                            Width="125px" Height="35px" OnClientClick="javascript:history.go(-1);return false;" />
							<asp:Button runat="server" ID="btnSolve" Text ="Solve" CssClass="btnNext" 
                            Width="125px" Height="35px" onclick="btnSolve_Click"/>
					</div>
					</asp:Panel>
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
