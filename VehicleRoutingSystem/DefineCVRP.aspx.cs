using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace VehicleRoutingSystem
{
    public partial class DefineCVRP : System.Web.UI.Page
    {
        static Random random = new Random(0);

        static int alpha; // influence of pheromone on direction
        static int beta;  // influence of adjacent node distance

        static double rho; // pheromone decrease factor
        static double Q;   // pheromone increase factor

        protected void Page_Load(object sender, EventArgs e)
        {
            bool CanSolve = false; 
            //this.AddKeepAlive();
            if (!Page.IsPostBack) // do this the very first time the page loads
            {
                fuUploadProblem.Visible = false;
                if (Session["Panel"] != null)
                {
                    string panel = Session["Panel"].ToString();
                    switch (panel)
                    {
                        case "Panel1":
                            Panel1.Visible = true;
                            Panel2.Visible = false;
                            Panel3.Visible = false;
                            Panel4.Visible = false;
                            Panel5.Visible = false;
                            Panel7.Visible = false;
                            break;
                        case "Panel2":
                            Panel1.Visible = false;
                            Panel2.Visible = true;
                            Panel3.Visible = false;
                            Panel4.Visible = false;
                            Panel5.Visible = false;
                            Panel7.Visible = false;
                            break;
                        case "Panel3":
                            Panel1.Visible = false;
                            Panel2.Visible = false;
                            Panel3.Visible = true;
                            Panel4.Visible = false;
                            Panel5.Visible = false;
                            Panel7.Visible = false;
                            break;
                        case "Panel4":
                            Panel1.Visible = false;
                            Panel2.Visible = false;
                            Panel3.Visible = false;
                            Panel4.Visible = true;
                            Panel5.Visible = false;
                            Panel7.Visible = false;
                            break;
                        case "Panel5":
                            Panel1.Visible = false;
                            Panel2.Visible = false;
                            Panel3.Visible = false;
                            Panel4.Visible = false;
                            Panel5.Visible = true;
                            Panel7.Visible = false;
                            break;
                        case "Panel7":
                            Panel1.Visible = false;
                            Panel2.Visible = false;
                            Panel3.Visible = false;
                            Panel4.Visible = false;
                            Panel5.Visible = false;
                            Panel7.Visible = true;
                            CanSolve = true;
                            break;
                    }
                }
                else
                {
                    // Display the first panel
                    Panel1.Visible = true;
                    Panel2.Visible = false;
                    Panel3.Visible = false;
                    Panel4.Visible = false;
                    Panel5.Visible = false;
                    Panel7.Visible = false;
                }
                DataTable dtNodeInformation = new DataTable("Node_Information");
                
                //Create columns for the datatable
                DataColumn nodeColumn = new DataColumn("Node", typeof(string));
                DataColumn demandColumn = new DataColumn("Demand", typeof(float));

                //Add the columns
                dtNodeInformation.Columns.Add(nodeColumn);
                dtNodeInformation.Columns.Add(demandColumn);

                //Set the unique column
                nodeColumn.Unique = true;

                // Create primary key on this field
                DataColumn[] pk = new DataColumn[1];
                pk[0] = nodeColumn;
                dtNodeInformation.PrimaryKey = pk;

                dtNodeInformation.AcceptChanges();
                
                

                if (CanSolve == true) // Check whether this page was called from the distance page
                {
                    DataTable dt = (DataTable)Session["NodeInformation"];
                    Session["NodeInformation"] = dt;
                    
                }
                else
                {
                    Session["NodeInformation"] = dtNodeInformation;
                    // Assign the data source of the gridview
                    grvNodeInformation2.DataSource = dtNodeInformation;
                    grvNodeInformation2.DataBind();                    
                }

            }
            if (Session["SavedNodeInformation"] != null)
            {
                DataTable dt = (DataTable)Session["SavedNodeInformation"];
                Session["NodeInformation"] = dt;
                // Assign the data source of the gridview
                grvNodeInformation2.DataSource = dt;
                grvNodeInformation2.DataBind();
                Session.Remove("SavedNodeInformation"); // Remove the saved node information since its value has been copied
            }
        }

        protected void grvNodeInformation_InsertCommand(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            
        }

        protected void grvNodeInformation2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void lnkAddNode_Click(object sender, EventArgs e)
        {
            double maxTruckCapacity = double.Parse(Session["MaxTruckCapacity"].ToString());
            if (double.Parse(txtNewDemand.Text) <= maxTruckCapacity)
            {
                DataTable dt = (DataTable)Session["NodeInformation"];
                dt.Rows.Add(txtNewNode.Text, int.Parse(txtNewDemand.Text));
                dt.AcceptChanges();
                grvNodeInformation2.DataSource = dt;
                grvNodeInformation2.DataBind();
                lblMessage.Text = "";
                Session["NodeInformation"] = dt;
            }
            else
            {
                // Tell the user that customer demand cannot exceed max truck capacity
                lblMessage.Text = "Customer demand cannot exceed maximum truck capacity";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void grvNodeInformation2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grvNodeInformation2.EditIndex = e.NewEditIndex;
            DataTable dt = (DataTable)Session["NodeInformation"];
            grvNodeInformation2.DataSource = dt;
            grvNodeInformation2.DataBind();
        }

        protected void grvNodeInformation2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)grvNodeInformation2.DataSource;
            GridViewRow row = grvNodeInformation2.Rows[e.RowIndex];
            string node = ((TextBox)row.FindControl(dt.Columns[0].ColumnName)).Text;
            string demand = ((TextBox)row.FindControl(dt.Columns[1].ColumnName)).Text;
            dt.Rows[e.RowIndex]["Node"] = node;
            dt.Rows[e.RowIndex]["Demand"] = demand;
            dt.AcceptChanges();
            grvNodeInformation2.EditIndex = -1;
            grvNodeInformation2.DataSource = dt;
            grvNodeInformation2.DataBind(); 
        }

        protected void grvNodeInformation2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvNodeInformation2.EditIndex = -1;
            DataTable dt = (DataTable)Session["NodeInformation"];
            grvNodeInformation2.DataSource = dt;
            grvNodeInformation2.DataBind();
        }

        

        protected void btnNext5_Click(object sender, EventArgs e)
        {
            if (grvNodeInformation2.Rows.Count >= 4) // Ensure that the user has entered up to 4 nodes
            {
                Session["WarehouseName"] = txtWarehouse.Text;
                Session["NoOfAnts"] = txtNoOfAnts.Text;
                Session["MaxTime"] = txtMaxTime.Text;
                Session["Alpha"] = txtAlpha.Text;
                Session["Beta"] = txtBeta.Text;
                Session["Rho"] = txtRho.Text;
                Session["Q"] = txtQ.Text;
                Session["MaxTruckCapacity"] = txtTruckCapacity.Text;
                DataTable dt = (DataTable)Session["NodeInformation"];
                Session["NodeInformation"] = dt;
                Session["CanLoadDistances"] = "YES";
                lblMessage.Text = "";
                Server.Transfer("Distances.aspx");
            }
            else
            {
                lblMessage.Text = "You must enter at least four nodes";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnNext1_Click(object sender, EventArgs e)
        {
            if (rbtnCreateProblem.Checked == true) // Check whether the 'Create Problem' radio button is checked
            {
                // Set the visible panel
                Panel1.Visible = false;
                Panel2.Visible = true;
                Panel3.Visible = false;
                Panel4.Visible = false;
                Panel5.Visible = false;
                Panel7.Visible = false;
            }
            else // Fetch problem from XML file
            {
                if (fuUploadProblem.HasFile == true) // Check whether a file has been selected
                {
                    HttpPostedFile problem = fuUploadProblem.PostedFile; // Fetch the posted file
                    if (problem.ContentType == "text/xml") // Check whether the uploaded file is an XML file
                    {
                        try
                        {
                            // Load and process the uploaded problem
                            XmlDocument doc = new XmlDocument();
                            doc.Load(problem.InputStream);

                            // Load the ACO Parameters
                            XmlNode acoParameters = doc.SelectSingleNode("/Problem/ACO_Parameters");
                            txtAlpha.Text = acoParameters.SelectSingleNode("Alpha").InnerText;
                            txtBeta.Text = acoParameters.SelectSingleNode("Beta").InnerText;
                            txtRho.Text = acoParameters.SelectSingleNode("Rho").InnerText;
                            txtQ.Text = acoParameters.SelectSingleNode("Q").InnerText;
                            txtNoOfAnts.Text = acoParameters.SelectSingleNode("NoOfAnts").InnerText;
                            txtMaxTime.Text = acoParameters.SelectSingleNode("MaxTime").InnerText;

                            // Load the Warehouse name and Max Truck Capacity
                            XmlNode warehouse = doc.SelectSingleNode("/Problem/WareHouse");
                            txtWarehouse.Text = warehouse.InnerText;
                            XmlNode maxTruckCapacity = doc.SelectSingleNode("/Problem/MaxTruckCapacity");
                            txtTruckCapacity.Text = maxTruckCapacity.InnerText;

                            // Load the Node Names and Demands
                            XmlNode nodeInformation = doc.SelectSingleNode("/Problem/NodeInformation");
                            DataTable dtNodeInformation = new DataTable("Node_Information");
                            //Create columns for the datatable
                            DataColumn nodeColumn = new DataColumn("Node", typeof(string));
                            DataColumn demandColumn = new DataColumn("Demand", typeof(float));
                            //Add the columns
                            dtNodeInformation.Columns.Add(nodeColumn);
                            dtNodeInformation.Columns.Add(demandColumn);
                            //Set the unique column
                            nodeColumn.Unique = true;
                            // Create primary key on this field
                            DataColumn[] pk = new DataColumn[1];
                            pk[0] = nodeColumn;
                            dtNodeInformation.PrimaryKey = pk;
                            foreach (XmlNode node in nodeInformation.SelectNodes("Node")) // Loop through all nodes
                            {
                                dtNodeInformation.Rows.Add(node.Attributes["Name"].Value, int.Parse(node.Attributes["Demand"].Value));
                                dtNodeInformation.AcceptChanges();
                            }

                            // Load the Node Distances
                            XmlNode nodeDistances = doc.SelectSingleNode("/Problem/Distances");
                            int savedNodeDistanceCount = 0;
                            int[] savedNodeDistanceArray = new int[savedNodeDistanceCount]; // Array to hold the saved node distances
                            string[] savedNodeDistanceDescriptionArray = new string[savedNodeDistanceCount];
                            foreach (XmlNode node in nodeDistances.SelectNodes("NodeDistance"))
                            {
                                savedNodeDistanceCount += 1; // Increment the node distance count
                                Array.Resize(ref savedNodeDistanceArray, savedNodeDistanceCount);
                                Array.Resize(ref savedNodeDistanceDescriptionArray, savedNodeDistanceCount);
                                // Fetch the node distances and their description
                                savedNodeDistanceArray[savedNodeDistanceCount - 1] = int.Parse(node.Attributes["Distance"].Value);
                                savedNodeDistanceDescriptionArray[savedNodeDistanceCount - 1] = node.Attributes["Description"].Value;
                            }

                            // Set the session variables
                            Session["SavedNodeInformation"] = dtNodeInformation;
                            Session["SavedNodeDistanceArray"] = savedNodeDistanceArray;
                            Session["SavedNodeDistanceDescriptionArray"] = savedNodeDistanceDescriptionArray;

                            // Set the visible panel
                            Panel1.Visible = false;
                            Panel2.Visible = true;
                            Panel3.Visible = false;
                            Panel4.Visible = false;
                            Panel5.Visible = false;
                            Panel7.Visible = false;
                            lblMessage.Text = "";
                        }
                        catch (Exception) // Catch every error
                        {
                            lblMessage.Text = "Oops! an error occurred. The file is either damaged or in a bad format"; 
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "File must be in XML Format";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblMessage.Text = "Please Upload a Previously Saved Problem";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnPrevious2_Click(object sender, EventArgs e)
        {
            // Set the visible panel
            Panel1.Visible = true;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel5.Visible = false;
            Panel7.Visible = false;
        }

        protected void btnNext2_Click(object sender, EventArgs e)
        {
            // Set the visible panel
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = true;
            Panel4.Visible = false;
            Panel5.Visible = false;
            Panel7.Visible = false;
        }

        protected void btnPrevious3_Click(object sender, EventArgs e)
        {
            // Set the visible panel
            Panel1.Visible = false;
            Panel2.Visible = true;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel5.Visible = false;
            Panel7.Visible = false;
        }

        protected void btnNext3_Click(object sender, EventArgs e)
        {
            // Set the visible panel
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = true;
            Panel5.Visible = false;
            Panel7.Visible = false;
        }

        protected void btnPrevious4_Click(object sender, EventArgs e)
        {
            // Set the visible panel
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = true;
            Panel4.Visible = false;
            Panel5.Visible = false;
            Panel7.Visible = false;
        }

        protected void btnNext4_Click(object sender, EventArgs e)
        {
            Session["MaxTruckCapacity"] = txtTruckCapacity.Text;
            // Set the visible panel
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel5.Visible = true;
            Panel7.Visible = false;
        }

        protected void btnPrevious5_Click(object sender, EventArgs e)
        {
            // Set the visible panel
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = true;
            Panel5.Visible = false;
            Panel7.Visible = false;
        }

        

        protected void btnPrevious7_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnSolve_Click(object sender, EventArgs e)
        {
            alpha = int.Parse(Session["Alpha"].ToString()); // influence of pheromone on direction
            beta = int.Parse(Session["Beta"].ToString());  // influence of adjacent node distance

            rho = double.Parse(Session["Rho"].ToString()); // pheromone decrease factor
            Q = double.Parse(Session["Q"].ToString());   // pheromone increase factor
            DataTable dt = (DataTable)HttpContext.Current.Session["NodeInformation"];
            double maxTruckCapacity = double.Parse(Session["MaxTruckCapacity"].ToString());
            string[,] nodeArray = FetchNodeAndDemand(); // Fetch the nodes and demands
            string[] nodeNames = new string[dt.Rows.Count];
            string[] nodeDemands = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nodeNames[i] = nodeArray[i, 0]; //Fetch the node names
                nodeDemands[i] = nodeArray[i, 1]; // Fetch the node demands
            }
            
            
                        
            try
            {
                //Begin Ant Colony Optimization;
                Session["NodeInformation"] = dt;
                int numCities = dt.Rows.Count;
                int numAnts = 4;
                int maxTime = 1000;

                Session["NoOfCities"] = numCities;

                Session["NoOfAnts"] = numAnts;
                Session["MaxTime"] = maxTime;

                Session["Alpha"] = alpha;
                Session["Beta"] = beta;
                Session["Rho"] = rho.ToString("F2");
                Session["Q"] = Q.ToString("F2");

                //Initializing graph distances;
                int[][] dists = (int[][])Session["NodeDistances"];
                
                //Initializing ants to random trails;
                int[][] ants = InitAnts(numAnts, numCities); // initialize ants to random trails
                Session["InitialRandomTrails"] = ShowAnts(ants, dists);
                int[] bestTrail = BestTrail(ants, dists); // determine the best initial trail
                double bestLength = double.Parse(Session["BestInitialTrailLength"].ToString());
                
                //Initializing pheromones on trails;
                double[][] pheromones = InitPheromones(numCities);

                int time = 0;
                //Entering UpdateAnts - UpdatePheromones loop;
                while (time < maxTime)
                {
                    UpdateAnts(ants, pheromones, dists);
                    UpdatePheromones(pheromones, ants, dists);

                    int[] currBestTrail = BestTrail(ants, dists);
                    double totalDemands = 0.0; // Reset the total demand
                    double trailDistance = 0.0; // Reset the trail distance
                    trailDistance += dists[0][currBestTrail[0] + 1]; // Add the distance from the warehouse to the first node                        
                    for (int i = 0; i < currBestTrail.Length; i++)
                    {
                        if (totalDemands + double.Parse(nodeDemands[currBestTrail[i]]) <= maxTruckCapacity) //Check that the total demand does not exceed the truck capacity
                        {
                            totalDemands += double.Parse(nodeDemands[currBestTrail[i]]); // increment the total demand
                            if (i != 0)
                            {
                                // Add the distance between nodes
                                trailDistance += dists[currBestTrail[i - 1] + 1][currBestTrail[i] + 1]; //The +1 is added because the dists array contains node distances from the warehouse too
                            }
                        }
                        else
                        {
                            trailDistance += dists[currBestTrail[i - 1] + 1][0]; // Add the distance from the previous node to the warehouse
                            totalDemands = 0.0; // Reset the total demand since we are starting a new trip
                            totalDemands += double.Parse(nodeDemands[currBestTrail[i]]); // increment the total demand
                            trailDistance += dists[0][currBestTrail[i] + 1]; // Add the distance from the warehouse to the current node
                        }
                    }
                    trailDistance += dists[currBestTrail[currBestTrail.Length - 1] + 1][0]; // Add the distance from the last node to the warehouse
                    double currBestLength = trailDistance; //Assign the trail distance
                    if (currBestLength < bestLength)
                    {
                        bestLength = currBestLength;
                        bestTrail = currBestTrail;
                    }
                    ++time;
                }

                //Time complete";

                //Best trail found;
                Session["BestTrailFound"] = Display(bestTrail);
                
                //End Ant Colony Optimization
                //Show the result on the solution page
                Server.Transfer("Solution.aspx");

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        // Ant Colony Optimisation Methods
        // --------------------------------------------------------------------------------------------

        IEnumerable<Control> GetControls(Control parent)
        {
            List<Control> ret = new List<Control>();
            foreach (Control c in parent.Controls)
            {
                ret.Add(c);
                ret.AddRange(GetControls(c));
            }
            return (IEnumerable<Control>)ret;
        }

        static string[,] FetchNodeAndDemand() // Fetch an array of nodes and demands
        {
            // Fetch the nodes and their demands            
            DataTable dt = (DataTable)HttpContext.Current.Session["NodeInformation"];
            string[,] nodeArray = new string[dt.Rows.Count, 2];

            for (int i = 0; i <= (dt.Rows.Count - 1); i++)
            {
                nodeArray[i, 0] = dt.Rows[i][0].ToString(); //Fetch the node's name
                nodeArray[i, 1] = dt.Rows[i][1].ToString(); //Fetch the node's demand
            }
            return nodeArray;
        }

        static int[][] InitAnts(int numAnts, int numCities)
        {
            int[][] ants = new int[numAnts][];
            for (int k = 0; k < numAnts; ++k)
            {
                int start = random.Next(0, numCities);
                ants[k] = RandomTrail(start, numCities);
            }
            return ants;
        }

        static int[] RandomTrail(int start, int numCities) // helper for InitAnts
        {
            int[] trail = new int[numCities];

            for (int i = 0; i < numCities; ++i) { trail[i] = i; } // sequential

            for (int i = 0; i < numCities; ++i) // Fisher-Yates shuffle
            {
                int r = random.Next(i, numCities);
                int tmp = trail[r]; trail[r] = trail[i]; trail[i] = tmp;
            }

            int idx = IndexOfTarget(trail, start); // put start at [0]
            int temp = trail[0];
            trail[0] = trail[idx];
            trail[idx] = temp;

            return trail;
        }

        static int IndexOfTarget(int[] trail, int target) // helper for RandomTrail
        {
            for (int i = 0; i < trail.Length; ++i)
            {
                if (trail[i] == target)
                    return i;
            }
            throw new Exception("Target not found in IndexOfTarget");
        }

        static double Length(int[] trail, int[][] dists) // total length of a trail
        {
            double result = 0.0;
            for (int i = 0; i < trail.Length - 1; ++i)
                result += Distance(trail[i] + 1, trail[i + 1] + 1, dists); // The + 1 is cuz the dists array contains distances 
                                                                           //from warehouse and is 1 larger that trail array
            result += Distance(0, trail[0] + 1, dists); // Compute the distance from the warehouse to the first node
            result += Distance(trail[trail.Length - 1] + 1, 0, dists); // Compute the distance from the last node to the warehouse
            return result;
        }

        // -------------------------------------------------------------------------------------------- 

        static int[] BestTrail(int[][] ants, int[][] dists) // best trail has shortest total length
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["NodeInformation"];
            double maxTruckCapacity = double.Parse(HttpContext.Current.Session["MaxTruckCapacity"].ToString());
            string[,] nodeArray = FetchNodeAndDemand(); // Fetch the nodes and demands
            string[] nodeNames = new string[dt.Rows.Count];
            string[] nodeDemands = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nodeNames[i] = nodeArray[i, 0]; //Fetch the node names
                nodeDemands[i] = nodeArray[i, 1]; // Fetch the node demands
            }
            // Compute the distance of the first trail
            double totalDemands = 0.0; // Reset the total demand
            double trailDistance = 0.0; // Reset the trail distance
            trailDistance += dists[0][ants[0][0] + 1]; // Add the distance from the warehouse to the first node                        
            for (int i = 0; i < ants[0].Length; i++)
            {
                if (totalDemands + double.Parse(nodeDemands[ants[0][i]]) <= maxTruckCapacity) //Check that the total demand does not exceed the truck capacity
                {
                    totalDemands += double.Parse(nodeDemands[ants[0][i]]); // increment the total demand
                    if (i != 0)
                    {
                        // Add the distance between nodes
                        trailDistance += dists[ants[0][i - 1] + 1][ants[0][i] + 1]; //The +1 is added because the dists array contains node distances from the warehouse too
                    }
                }
                else
                {
                    trailDistance += dists[ants[0][i - 1] + 1][0]; // Add the distance from the previous node to the warehouse
                    totalDemands = 0.0; // Reset the total demand since we are starting a new trip
                    totalDemands += double.Parse(nodeDemands[ants[0][i]]); // increment the total demand
                    trailDistance += dists[0][ants[0][i] + 1]; // Add the distance from the warehouse to the current node
                }
            }
            trailDistance += dists[ants[0][ants[0].Length - 1] + 1][0]; // Add the distance from the last node to the warehouse
            double bestLength = trailDistance; //set the distance of the first trail as the initial best length
            trailDistance = 0.0; // Reset the trail distance
            totalDemands = 0.0; // Reset the total demands
            int idxBestLength = 0;
            // Compute the length of other trails
            for (int k = 1; k < ants.Length; ++k)
            {
                totalDemands = 0.0; // Reset the total demand
                trailDistance = 0.0; // Reset the trail distance
                trailDistance += dists[0][ants[k][0] + 1]; // Add the distance from the warehouse to the first node                        
                for (int i = 0; i < ants[k].Length; i++)
                {
                    if (totalDemands + double.Parse(nodeDemands[ants[k][i]]) <= maxTruckCapacity) //Check that the total demand does not exceed the truck capacity
                    {
                        totalDemands += double.Parse(nodeDemands[ants[k][i]]); // increment the total demand
                        if (i != 0)
                        {
                            // Add the distance between nodes
                            trailDistance += dists[ants[k][i - 1] + 1][ants[k][i] + 1]; //The +1 is added because the dists array contains node distances from the warehouse too
                        }
                    }
                    else
                    {
                        trailDistance += dists[ants[k][i - 1] + 1][0]; // Add the distance from the previous node to the warehouse
                        totalDemands = 0.0; // Reset the total demand since we are starting a new trip
                        totalDemands += double.Parse(nodeDemands[ants[k][i]]); // increment the total demand
                        trailDistance += dists[0][ants[k][i] + 1]; // Add the distance from the warehouse to the current node
                    }
                }
                trailDistance += dists[ants[k][ants[k].Length - 1] + 1][0]; // Add the distance from the last node to the warehouse
                double len = trailDistance; // Assign the length of the trail
                if (len < bestLength)
                {
                    bestLength = len;
                    idxBestLength = k;
                }
            }
            int numCities = ants[0].Length;
            int[] bestTrail = new int[numCities];
            ants[idxBestLength].CopyTo(bestTrail, 0);
            return bestTrail;
        }

        // --------------------------------------------------------------------------------------------

        static double[][] InitPheromones(int numCities)
        {
            double[][] pheromones = new double[numCities][];
            for (int i = 0; i < numCities; ++i)
                pheromones[i] = new double[numCities];
            for (int i = 0; i < pheromones.Length; ++i)
                for (int j = 0; j < pheromones[i].Length; ++j)
                    pheromones[i][j] = 0.01; // otherwise first call to UpdateAnts -> BuiuldTrail -> NextNode -> MoveProbs => all 0.0 => throws
            return pheromones;
        }

        // --------------------------------------------------------------------------------------------

        static void UpdateAnts(int[][] ants, double[][] pheromones, int[][] dists)
        {
            int numCities = pheromones.Length;
            for (int k = 0; k < ants.Length; ++k)
            {
                int start = random.Next(0, numCities);
                int[] newTrail = BuildTrail(k, start, pheromones, dists);
                ants[k] = newTrail;
            }
        }

        static int[] BuildTrail(int k, int start, double[][] pheromones, int[][] dists)
        {
            int numCities = pheromones.Length;
            int[] trail = new int[numCities];
            bool[] visited = new bool[numCities];
            trail[0] = start;
            visited[start] = true;
            for (int i = 0; i < numCities - 1; ++i)
            {
                int cityX = trail[i];
                int next = NextCity(k, cityX, visited, pheromones, dists);
                trail[i + 1] = next;
                visited[next] = true;
            }
            return trail;
        }

        static int NextCity(int k, int cityX, bool[] visited, double[][] pheromones, int[][] dists)
        {
            // for ant k (with visited[]), at nodeX, what is next node in trail?
            double[] probs = MoveProbs(k, cityX, visited, pheromones, dists);

            double[] cumul = new double[probs.Length + 1];
            for (int i = 0; i < probs.Length; ++i)
                cumul[i + 1] = cumul[i] + probs[i]; // consider setting cumul[cuml.Length-1] to 1.00

            double p = random.NextDouble();

            for (int i = 0; i < cumul.Length - 1; ++i)
                if (p >= cumul[i] && p < cumul[i + 1])
                    return i;
            throw new Exception("Failure to return valid city in NextCity");
        }

        static double[] MoveProbs(int k, int cityX, bool[] visited, double[][] pheromones, int[][] dists)
        {
            // for ant k, located at nodeX, with visited[], return the prob of moving to each city
            int numCities = pheromones.Length;
            double[] taueta = new double[numCities]; // inclues cityX and visited cities
            double sum = 0.0; // sum of all tauetas
            for (int i = 0; i < taueta.Length; ++i) // i is the adjacent city
            {
                if (i == cityX)
                    taueta[i] = 0.0; // prob of moving to self is 0
                else if (visited[i] == true)
                    taueta[i] = 0.0; // prob of moving to a visited city is 0
                else
                {
                    taueta[i] = Math.Pow(pheromones[cityX][i], alpha) * Math.Pow((1.0 / Distance(cityX, i, dists)), beta); // could be huge when pheromone[][] is big
                    if (taueta[i] < 0.0001)
                        taueta[i] = 0.0001;
                    else if (taueta[i] > (double.MaxValue / (numCities * 100)))
                        taueta[i] = double.MaxValue / (numCities * 100);
                }
                sum += taueta[i];
            }

            double[] probs = new double[numCities];
            for (int i = 0; i < probs.Length; ++i)
                probs[i] = taueta[i] / sum; // big trouble if sum = 0.0
            return probs;
        }

        // --------------------------------------------------------------------------------------------

        static void UpdatePheromones(double[][] pheromones, int[][] ants, int[][] dists)
        {
            for (int i = 0; i < pheromones.Length; ++i)
            {
                for (int j = i + 1; j < pheromones[i].Length; ++j)
                {
                    for (int k = 0; k < ants.Length; ++k)
                    {
                        double length = Length(ants[k], dists); // length of ant k trail
                        double decrease = (1.0 - rho) * pheromones[i][j];
                        double increase = 0.0;
                        if (EdgeInTrail(i, j, ants[k]) == true) increase = (Q / length);

                        pheromones[i][j] = decrease + increase;

                        if (pheromones[i][j] < 0.0001)
                            pheromones[i][j] = 0.0001;
                        else if (pheromones[i][j] > 100000.0)
                            pheromones[i][j] = 100000.0;

                        pheromones[j][i] = pheromones[i][j];
                    }
                }
            }
        }

        static bool EdgeInTrail(int cityX, int cityY, int[] trail)
        {
            // are cityX and cityY adjacent to each other in trail[]?
            int lastIndex = trail.Length - 1;
            int idx = IndexOfTarget(trail, cityX);

            if (idx == 0 && trail[1] == cityY) return true;
            else if (idx == 0 && trail[lastIndex] == cityY) return true;
            else if (idx == 0) return false;
            else if (idx == lastIndex && trail[lastIndex - 1] == cityY) return true;
            else if (idx == lastIndex && trail[0] == cityY) return true;
            else if (idx == lastIndex) return false;
            else if (trail[idx - 1] == cityY) return true;
            else if (trail[idx + 1] == cityY) return true;
            else return false;
        }


        // --------------------------------------------------------------------------------------------

        static int[][] MakeGraphDistances(int numCities)
        {
            int[][] dists = new int[numCities][];
            for (int i = 0; i < dists.Length; i++)
                dists[i] = new int[numCities];
            for (int i = 0; i < numCities; i++)
                for (int j = 0; j < numCities; j++)
                {
                    if (j >= i)
                    {
                        dists[i][j] = 0;
                    }
                    else
                    {
                        int d = random.Next(1, 9); // [1,8]
                        dists[i][j] = d;
                        //dists[j][i] = d;
                    }
                }
            return dists;
        }

        static double Distance(int cityX, int cityY, int[][] dists)
        {
            return dists[cityX][cityY];
        }

        // --------------------------------------------------------------------------------------------

        static string Display(int[] trail)
        {
            StringBuilder strDisplay = new StringBuilder();
            strDisplay.Append("<br/>[ " + HttpContext.Current.Session["WarehouseName"] + " Warehouse to ");
            DataTable dt = (DataTable)HttpContext.Current.Session["NodeInformation"];
            double maxTruckCapacity = double.Parse(HttpContext.Current.Session["MaxTruckCapacity"].ToString());
            //Initializing graph distances;
            int[][] dists = (int[][])HttpContext.Current.Session["NodeDistances"];
            double totalQuantity = double.Parse(HttpContext.Current.Session["TotalQuantity"].ToString()); // Fetch the total quantity demanded
            string[,] nodeArray = FetchNodeAndDemand(); // Fetch the nodes and demands
            string[] nodeNames = new string[dt.Rows.Count];
            string[] nodeDemands = new string[dt.Rows.Count];
            double totalDemands = 0.0;
            double trailDistance = 0.0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nodeNames[i] = nodeArray[i, 0]; //Fetch the node names
                nodeDemands[i] = nodeArray[i, 1]; // Fetch the node demands
            }
            trailDistance += dists[0][trail[0] + 1]; // Add the distance from the warehouse to the first node
            for (int i = 0; i < trail.Length; ++i)
            {
                if (totalDemands + double.Parse(nodeDemands[trail[i]]) <= maxTruckCapacity) //Check that the total demand does not exceed the truck capacity
                {
                    strDisplay.Append(nodeNames[trail[i]] + " to "); //Display the node names
                    totalDemands += double.Parse(nodeDemands[trail[i]]); // increment the total demand
                    if (i > 0 && i % 20 == 0) strDisplay.Append("");
                    if (i != 0)
                    {
                        // Add the distance between nodes
                        trailDistance += dists[trail[i - 1] + 1][trail[i] + 1]; //The +1 is added because the dists array contains node distances from the warehouse too
                    }
                }
                else
                {
                    trailDistance += dists[trail[i - 1] + 1][0]; // Add the distance from the previous node to the warehouse
                    strDisplay.Append(HttpContext.Current.Session["WarehouseName"] + " Warehouse]<br/>"); // Go back to the warehouse
                    totalDemands = 0.0; // Reset the total demand since we are starting a new trip
                    totalDemands += double.Parse(nodeDemands[trail[i]]); // increment the total demand
                    trailDistance += dists[0][trail[i] + 1]; // Add the distance from the warehouse to the current node
                    strDisplay.Append("[" + HttpContext.Current.Session["WarehouseName"] + " Warehouse to ");
                    strDisplay.Append(nodeNames[trail[i]] + " to "); //Display the node names
                    if (i > 0 && i % 20 == 0) strDisplay.Append("");
                }
            }
            trailDistance += dists[trail[trail.Length - 1] + 1][0]; // Add the distance from the last node to the warehouse
            strDisplay.Append("");
            strDisplay.Append(HttpContext.Current.Session["WarehouseName"] + " Warehouse]");
            strDisplay.Append("");
            strDisplay.Append("<br/><br/>Total Quantity Distributed = " + totalQuantity);
            HttpContext.Current.Session["LengthOfBestTrailFound"] = trailDistance;
            return strDisplay.ToString();
        }


        static string ShowAnts(int[][] ants, int[][] dists)
        {
            StringBuilder strAnts = new StringBuilder();
            DataTable dt = (DataTable)HttpContext.Current.Session["NodeInformation"];
            double maxTruckCapacity = double.Parse(HttpContext.Current.Session["MaxTruckCapacity"].ToString());
            string[,] nodeArray = FetchNodeAndDemand(); // Fetch the nodes and demands
            string[] nodeNames = new string[dt.Rows.Count];
            string[] nodeDemands = new string[dt.Rows.Count];
            double totalDemands = 0.0;
            double trailDistance = 0.0;
            double totalQuantity = 0.0;
            double[] initialTrailLengths = new double[ants.Length]; // Array to hold the initial trail lengths
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nodeNames[i] = nodeArray[i, 0]; //Fetch the node names
                nodeDemands[i] = nodeArray[i, 1]; // Fetch the node demands
            }
            for (int m = 0; m < nodeDemands.Length; m++)
            {
                totalQuantity += double.Parse(nodeDemands[m]); // Sum the quantity demanded
            }
            HttpContext.Current.Session["TotalQuantity"] = totalQuantity;
            for (int i = 0; i < ants.Length; i++)
            {
                strAnts.Append("<br/>Trail " + (i + 1) + " <br/>----------------------------------------------------" + 
                    "<br/>[ " + HttpContext.Current.Session["WarehouseName"] + " Warehouse to ");
                totalDemands = 0.0; // Reset the total demand since we are starting a new trip
                trailDistance = 0.0; // Reset the trail distance since we are starting a new trip
                trailDistance += dists[0][ants[i][0] + 1]; // Add the distance from the warehouse to the first node
                for (int j = 0; j < ants[i].Length; j++)
                {

                    if (totalDemands + double.Parse(nodeDemands[ants[i][j]]) <= maxTruckCapacity) //Check that the total demand does not exceed the truck capacity
                    {
                        strAnts.Append(nodeNames[ants[i][j]] + " to ");
                        totalDemands += double.Parse(nodeDemands[ants[i][j]]); // increment the total demand
                        if (j != 0)
                        {
                            // Add the distance between nodes
                            trailDistance += dists[ants[i][j - 1] + 1][ants[i][j] + 1]; //The +1 is added because the dists array contains node distances from the warehouse too
                        }
                    }
                    else
                    {
                        trailDistance += dists[ants[i][j-1] + 1][0]; // Add the distance from the previous node to the warehouse
                        strAnts.Append(HttpContext.Current.Session["WarehouseName"] + " Warehouse]<br/>"); // Go back to the warehouse
                        totalDemands = 0.0; // Reset the total demand since we are starting a new trip
                        totalDemands += double.Parse(nodeDemands[ants[i][j]]); // increment the total demand
                        trailDistance += dists[0][ants[i][j] + 1]; // Add the distance from the warehouse to the current node
                        strAnts.Append("[" +HttpContext.Current.Session["WarehouseName"] + " Warehouse to ");
                        strAnts.Append(nodeNames[ants[i][j]] + " to ");
                    }
                    
                }
                trailDistance += dists[ants[i][(ants[i].Length - 1)] + 1][0]; // Add the distance from the last node to the warehouse
                initialTrailLengths[i] = trailDistance; // Add the trail distance to the initialTrailLengths array
                strAnts.Append(HttpContext.Current.Session["WarehouseName"] + " Warehouse]");
                double len = Length(ants[i], dists);
                //strAnts.Append(len.ToString("F1"));
                strAnts.Append("<br/><br/>Total Distance Covered = " + trailDistance);
                strAnts.Append("<br/><br/>Total Quantity Distributed = " + totalQuantity);
                strAnts.Append("<br/><br/>");
            }
            // Look for the trail with shortest distance
            double bestInitialLength = initialTrailLengths[0]; // Initially set the first trail distance as the best
            for (int n = 1; n < initialTrailLengths.Length; n++)
            {
                if (initialTrailLengths[n] < bestInitialLength)
                {
                    bestInitialLength = initialTrailLengths[n]; // If a smaller distance id found, set to be the best
                }
            }
            HttpContext.Current.Session["BestInitialTrailLength"] = bestInitialLength.ToString("F1");
            return strAnts.ToString();
        }

        static void Display(double[][] pheromones)
        {
            for (int i = 0; i < pheromones.Length; ++i)
            {
                Console.Write(i + ": ");
                for (int j = 0; j < pheromones[i].Length; ++j)
                {
                    Console.Write(pheromones[i][j].ToString("F4").PadLeft(8) + " ");
                }
                Console.WriteLine("");
            }

        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            ExportXML(); // Save the problem and prompt the user to download it 
        }

        public void ExportXML()
        {
            // Fetch every parameter and data
            alpha = int.Parse(Session["Alpha"].ToString()); // influence of pheromone on direction
            beta = int.Parse(Session["Beta"].ToString());  // influence of adjacent node distance
            rho = double.Parse(Session["Rho"].ToString()); // pheromone decrease factor
            Q = double.Parse(Session["Q"].ToString());   // pheromone increase factor
            DataTable dt = (DataTable)HttpContext.Current.Session["NodeInformation"];
            string warehouseName = Session["WarehouseName"].ToString();
            double maxTruckCapacity = double.Parse(Session["MaxTruckCapacity"].ToString());
            int noOfAnts = int.Parse(Session["NoOfAnts"].ToString());
            int maxTime = int.Parse(Session["MaxTime"].ToString());
            string[,] nodeArray = FetchNodeAndDemand(); // Fetch the nodes and demands
            string[] nodeNames = new string[dt.Rows.Count];
            string[] nodeDemands = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nodeNames[i] = nodeArray[i, 0]; //Fetch the node names
                nodeDemands[i] = nodeArray[i, 1]; // Fetch the node demands
            }

            // Create the XML document
            XmlDocument doc = new XmlDocument();
            //XML declaration
            XmlNode declaration = doc.CreateNode(XmlNodeType.XmlDeclaration, null, null);
            doc.AppendChild(declaration);
            // Root element = Catalog
            XmlElement root = doc.CreateElement("Problem");
            doc.AppendChild(root);

            // Parameters
            XmlElement acoParameters = doc.CreateElement("ACO_Parameters");
            // Alpha
            XmlElement alphaNode = doc.CreateElement("Alpha");
            alphaNode.InnerText = alpha.ToString();
            acoParameters.AppendChild(alphaNode);
            // Beta
            XmlElement betaNode = doc.CreateElement("Beta");
            betaNode.InnerText = beta.ToString();
            acoParameters.AppendChild(betaNode);
            // Rho
            XmlElement rhoNode = doc.CreateElement("Rho");
            rhoNode.InnerText = rho.ToString();
            acoParameters.AppendChild(rhoNode);
            // Q
            XmlElement Q_Node = doc.CreateElement("Q");
            Q_Node.InnerText = Q.ToString();
            acoParameters.AppendChild(Q_Node);
            // Number of Ants
            XmlElement numberOfAnts = doc.CreateElement("NoOfAnts");
            numberOfAnts.InnerText = noOfAnts.ToString();
            acoParameters.AppendChild(numberOfAnts);
            // Maximum Time
            XmlElement maximumTime = doc.CreateElement("MaxTime");
            maximumTime.InnerText = maxTime.ToString();
            acoParameters.AppendChild(maximumTime);
            root.AppendChild(acoParameters); // Append the parameters
            // Warehouse
            XmlElement warehouse = doc.CreateElement("WareHouse");
            warehouse.InnerText = warehouseName.ToString();
            root.AppendChild(warehouse);
            // Maximum Truck Capacity
            XmlElement maximumTruckCapacity = doc.CreateElement("MaxTruckCapacity");
            maximumTruckCapacity.InnerText = maxTruckCapacity.ToString();
            root.AppendChild(maximumTruckCapacity);
            

            // Node Information (i.e: Node Names and Demands)
            XmlElement nodeInformation = doc.CreateElement("NodeInformation");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlElement node = doc.CreateElement("Node");
                XmlAttribute nodeName = doc.CreateAttribute("Name");
                nodeName.Value = nodeArray[i, 0]; //Fetch the node names
                XmlAttribute nodeDemand = doc.CreateAttribute("Demand");
                nodeDemand.Value = nodeArray[i, 1]; //Fetch the node demands
                node.Attributes.Append(nodeName);
                node.Attributes.Append(nodeDemand);
                nodeInformation.AppendChild(node);
            }
            root.AppendChild(nodeInformation); // Append the node information

            // Distances (Distances from the warehouse and between nodes)
            XmlElement distances = doc.CreateElement("Distances");
            int[] enteredNodeDistances = (int[])Session["EnteredNodeDistances"]; // Fetch the distances entered in the textboxes on the 'Distances' page
            string[] nodeDistanceDescription = (string[])Session["NodeDistanceDescription"]; // Fetch the node distance description
            // Note that enteredNodeDistances and nodeDistanceDescription are of the same size since a label describes a textbox
            for (int i = 0; i < enteredNodeDistances.Length; i++)
            {
                XmlElement nodeDistance = doc.CreateElement("NodeDistance");
                XmlAttribute description = doc.CreateAttribute("Description");
                description.Value = nodeDistanceDescription[i];
                XmlAttribute distance = doc.CreateAttribute("Distance");
                distance.Value = enteredNodeDistances[i].ToString();
                nodeDistance.Attributes.Append(description);
                nodeDistance.Attributes.Append(distance);
                distances.AppendChild(nodeDistance);
            }
            root.AppendChild(distances); // Append the node distances

            // Write the XML file to the memory stream and prompt the user to download it
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);

            doc.WriteTo(writer);
            writer.Flush();
            Response.Clear();
            byte[] byteArray = stream.ToArray();
            Response.AppendHeader("Content-Disposition", "filename=CVRP.xml");
            Response.AppendHeader("Content-Length", byteArray.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(byteArray);
            writer.Close();
        }

        protected void rbtnLoadProblem_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnLoadProblem.Checked == true)
            {
                fuUploadProblem.Visible = true; // Display the file upload control
            }
        }

        protected void rbtnCreateProblem_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnCreateProblem.Checked == true)
            {
                fuUploadProblem.Visible = false; // Hide the file upload control
            }
        }

        }
    }
