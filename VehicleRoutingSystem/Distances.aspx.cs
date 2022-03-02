using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

namespace VehicleRoutingSystem
{
    public partial class Distances : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CanLoadDistances"] == null) // Check whether the session variable is null
            {
                Response.Redirect("DefineCVRP.aspx"); // Redirect to the 'DefineCVRP' page
                return; // Stop every further action
            }
            DataTable dt = (DataTable)Session["NodeInformation"];
            string warehouseName = Session["WarehouseName"].ToString();
            for (int i = 0; i <= (dt.Rows.Count); i++)
            {
                for (int j = 0; j <= (dt.Rows.Count); j++)
                {
                    if (i == j) // Do not request the distance from a node to itself
                    {
                        if (i == 0 && j == 0) // Request for the distance from the warehouse to the first node
                        {
                            Label lblFromCityToCity = new Label();
                            lblFromCityToCity.Text = warehouseName + " Warehouse" + " to " + dt.Rows[j][0].ToString() + ": ";
                            TextBox txtFromCityToCityDistance = new TextBox();
                            txtFromCityToCityDistance.CssClass = "txtInput";
                            txtFromCityToCityDistance.MaxLength = 6;
                            txtFromCityToCityDistance.ID = "txt" + i + "" + j;
                            lblFromCityToCity.AssociatedControlID = txtFromCityToCityDistance.ID; //Set the label's associated control ID property
                            LiteralControl literalAlign = new LiteralControl("<p>");
                            LiteralControl literalAlign2 = new LiteralControl("</p>");
                            RequiredFieldValidator rfValid = new RequiredFieldValidator(); // The required field validator
                            rfValid.Text = "*";
                            rfValid.ControlToValidate = txtFromCityToCityDistance.ID;
                            RegularExpressionValidator regExpValid = new RegularExpressionValidator(); // The regular expression validator
                            regExpValid.ValidationExpression = @"^\d+$"; // The regular expression
                            regExpValid.ValidationGroup = "Distance";
                            regExpValid.Text = "Distance must be an integer";
                            regExpValid.ControlToValidate = txtFromCityToCityDistance.ID;
                            pnlDistances.Controls.Add(literalAlign);
                            pnlDistances.Controls.Add(lblFromCityToCity);
                            pnlDistances.Controls.Add(txtFromCityToCityDistance);
                            pnlDistances.Controls.Add(rfValid);
                            pnlDistances.Controls.Add(regExpValid);
                            pnlDistances.Controls.Add(literalAlign2);
                        }
                    }
                    else
                    {
                        if (i == 0) // Request for the distance from the Warehouse to the node 
                        {
                            if (j == dt.Rows.Count) continue;
                            Label lblFromCityToCity = new Label();
                            lblFromCityToCity.Text = warehouseName + " Warehouse" + " to " + dt.Rows[j][0].ToString() + ": ";
                            TextBox txtFromCityToCityDistance = new TextBox();
                            txtFromCityToCityDistance.CssClass = "txtInput";
                            txtFromCityToCityDistance.MaxLength = 6;
                            txtFromCityToCityDistance.ID = "txt" + i + "" + j;
                            lblFromCityToCity.AssociatedControlID = txtFromCityToCityDistance.ID; //Set the label's associated control ID property
                            LiteralControl literalAlign = new LiteralControl("<p>");
                            LiteralControl literalAlign2 = new LiteralControl("</p>");
                            RequiredFieldValidator rfValid = new RequiredFieldValidator(); // The required field validator
                            rfValid.Text = "*";
                            rfValid.ControlToValidate = txtFromCityToCityDistance.ID;
                            RegularExpressionValidator regExpValid = new RegularExpressionValidator(); // The regular expression validator
                            regExpValid.ValidationExpression = @"^\d+$"; // The regular expression
                            regExpValid.ValidationGroup = "Distance";
                            regExpValid.Text = "Distance must be an integer";
                            regExpValid.ControlToValidate = txtFromCityToCityDistance.ID;
                            pnlDistances.Controls.Add(literalAlign);
                            pnlDistances.Controls.Add(lblFromCityToCity);
                            pnlDistances.Controls.Add(txtFromCityToCityDistance);
                            pnlDistances.Controls.Add(rfValid);
                            pnlDistances.Controls.Add(regExpValid);
                            pnlDistances.Controls.Add(literalAlign2);
                        }
                        else //Request for the distance from the node to another node
                        {
                            //Check whether the distance has been requested. E.g (0,1) is the same as (1,0). Only request for it if it has not been requested
                            TextBox txt = (TextBox)pnlDistances.FindControl("txt" + j + "" + i);
                            if (txt == null)
                            {
                                if (j > 0)
                                {
                                    Label lblFromCityToCity = new Label();
                                    lblFromCityToCity.Text = dt.Rows[i - 1][0].ToString() + " to " + dt.Rows[j - 1][0].ToString() + ": ";
                                    TextBox txtFromCityToCityDistance = new TextBox();
                                    txtFromCityToCityDistance.CssClass = "txtInput";
                                    txtFromCityToCityDistance.MaxLength = 6;
                                    txtFromCityToCityDistance.ID = "txt" + i + "" + j;
                                    lblFromCityToCity.AssociatedControlID = txtFromCityToCityDistance.ID; //Set the label's associated control ID property
                                    LiteralControl literalAlign = new LiteralControl("<p>");
                                    LiteralControl literalAlign2 = new LiteralControl("</p>");
                                    RequiredFieldValidator rfValid = new RequiredFieldValidator(); // The required field validator
                                    rfValid.Text = "*";
                                    rfValid.ControlToValidate = txtFromCityToCityDistance.ID;
                                    RegularExpressionValidator regExpValid = new RegularExpressionValidator(); // The regular expression validator
                                    regExpValid.ValidationExpression = @"^\d+$"; // The regular expression
                                    regExpValid.ValidationGroup = "Distance";
                                    regExpValid.Text = "Distance must be an integer";
                                    regExpValid.ControlToValidate = txtFromCityToCityDistance.ID;
                                    pnlDistances.Controls.Add(literalAlign);
                                    pnlDistances.Controls.Add(lblFromCityToCity);
                                    pnlDistances.Controls.Add(txtFromCityToCityDistance);
                                    pnlDistances.Controls.Add(rfValid);
                                    pnlDistances.Controls.Add(regExpValid);
                                    pnlDistances.Controls.Add(literalAlign2);
                                }
                            }
                        }
                    }
                }
            }

            if (Session["SavedNodeDistanceArray"] != null) // Check whether the saved node distance array exists
            {
                int[] savedNodeDistanceArray = (int[])Session["SavedNodeDistanceArray"]; // Fetch the saved node distances
                string[] savedNodeDistanceDescriptionArray = (string[])Session["SavedNodeDistanceDescriptionArray"]; // Fetch the saved node distances description
                foreach (Control ctrl in GetControls(pnlDistances))
                {
                    if (ctrl is Label)
                    {
                        Label lbl = (Label)ctrl;
                        foreach (string description in savedNodeDistanceDescriptionArray)
                        {
                            // Check whether the label's text matches the description
                            if (lbl.Text == description)
                            {
                                // fetch the distance that corresponds to the label
                                TextBox txt = (TextBox)pnlDistances.FindControl(lbl.AssociatedControlID);
                                txt.Text = savedNodeDistanceArray[Array.IndexOf(savedNodeDistanceDescriptionArray, description)].ToString();
                            }
                        }
                        
                    }
                }
            }
        }

        protected void btnNext6_Click(object sender, EventArgs e)
        {
            // Fetch the distances entered in the textboxes on this page and save them in an array 
            int noOfTextBoxes = 0;
            int noOfLabels = 0;
            int[] enteredNodeDistances = new int[noOfTextBoxes]; // Array to hold the distance values entered on this page
            string[] nodeDistanceDescription = new string[noOfLabels]; // Array to hold the number of labels describing node distances
            foreach (Control ctrl in GetControls(pnlDistances))
            {
                if (ctrl is TextBox)
                {
                    TextBox txt = (TextBox)ctrl;
                    noOfTextBoxes += 1; // Count the no of textboxes on the page
                    Array.Resize(ref enteredNodeDistances, noOfTextBoxes); // Resize the array
                    enteredNodeDistances[noOfTextBoxes - 1] = int.Parse(txt.Text); // Fetch the text in the textbox
                }
                if (ctrl is Label)
                {
                    Label lbl = (Label)ctrl;
                    if (lbl.Text != "*" && lbl.Text != "Distance must be an integer") // Ensure that it is not the validator's label
                    {
                        noOfLabels += 1; // Count the no of labels on the page
                        Array.Resize(ref nodeDistanceDescription, noOfLabels); // Resize the array
                        nodeDistanceDescription[noOfLabels - 1] = lbl.Text; // Fetch the text of the label
                    }
                }
            }
            

            DataTable dt = (DataTable)Session["NodeInformation"];
            int[][] distances = new int[dt.Rows.Count + 1][]; //Include the warehouse in the distance matrix
            for (int i = 0; i < distances.Length; i++)
                distances[i] = new int[dt.Rows.Count + 1];
            for (int i = 0; i <= (dt.Rows.Count); i++)
            {
                for (int j = 0; j <= (dt.Rows.Count); j++)
                {
                    if (i == j) // The distance from a node to itself is zero
                    {
                        distances[i][j] = 0;
                    }
                    else
                    {
                        if (i > j)
                        {
                            distances[i][j] = distances[j][i]; // Node distances are symmetric i.e [1][0] = [0][1]
                        }
                        else
                        {
                            if (i == 0)
                            {
                                // Get the node distance from the warehouse
                                TextBox txt = (TextBox)Page.FindControl("txt" + i + "" + (j - 1));
                                distances[i][j] = int.Parse(txt.Text);
                            }
                            else
                            {
                                // Get the node distance from another node
                                TextBox txt = (TextBox)Page.FindControl("txt" + i + "" + j);
                                distances[i][j] = int.Parse(txt.Text);
                            }
                        }
                    }
                }
            }
            
            Session["NodeDistances"] = distances;
            Session["NodeInformation"] = dt;
            Session["EnteredNodeDistances"] = enteredNodeDistances;
            Session["NodeDistanceDescription"] = nodeDistanceDescription;
            Session["Panel"] = "Panel7";
            Server.Transfer("DefineCVRP.aspx");
        }

        protected void btnPrevious6_Click(object sender, EventArgs e)
        {
            //Session["Panel"] = "Panel5";
            //Server.Transfer("DefineCVRP.aspx");
        }

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
    }
}