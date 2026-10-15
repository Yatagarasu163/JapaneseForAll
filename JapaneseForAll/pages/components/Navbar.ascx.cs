using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.components
{
    public partial class Navbar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["fname"] == null) { Response.Redirect("~/pages/publics/login.aspx"); }

                fname1.Text = Session["fname"].ToString();
                initial.Text = Session["fname"].ToString().Substring(0, 1).ToUpper();
            }

        }

        protected void LogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("~/pages/publics/login.aspx");

            return;
        }
    }
}