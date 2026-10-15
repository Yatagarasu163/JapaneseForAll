using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.components
{
    public partial class InstructorNavbar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["fname"] == null) { Response.Redirect("~/pages/publics/login.aspx"); }

                fname.Text = Session["fname"].ToString();
                initials.Text = Session["fname"].ToString().Substring(0, 1).ToUpper();
            }
        }

        protected void OnLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("~/pages/publics/login.aspx");

            return;
        }
    }
}