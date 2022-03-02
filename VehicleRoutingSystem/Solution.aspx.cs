using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VehicleRoutingSystem
{
    public partial class Solution : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LengthOfBestTrailFound"] != null)
            {
                lblNoOfCities.Text = Session["NoOfCities"].ToString();
                lblNoOfAnts.Text = Session["NoOfAnts"].ToString();
                lblMaxTime.Text = Session["MaxTime"].ToString();
                lblAlpha.Text = Session["Alpha"].ToString();
                lblBeta.Text = Session["Beta"].ToString();
                lblRho.Text = Session["Rho"].ToString();
                lblQ.Text = Session["Q"].ToString();
                lblInitialRandomTrails.Text = "<br/>" + Session["InitialRandomTrails"].ToString();
                lblBestInitialTrailLength.Text = Session["BestInitialTrailLength"].ToString();
                lblBestTrailFound.Text = Session["BestTrailFound"].ToString();
                lblLengthOfBestTrailFound.Text = Session["LengthOfBestTrailFound"].ToString();
            }
            else
            {
                Response.Redirect("DefineCVRP.aspx");
            }
        }
    }
}