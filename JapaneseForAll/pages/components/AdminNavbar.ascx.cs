using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.components
{
    public partial class AdminNavbar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // Checks whichever session variable is available
                string displayName = "";

                if (Session["fname"] != null)
                {
                    displayName = Session["fname"].ToString();
                }
                else if (Session["username"] != null)
                {
                    displayName = Session["username"].ToString();
                }

                if (!string.IsNullOrEmpty(displayName))
                {
                    // for name
                    fname1.Text = displayName;

                    // for circle
                    initial.Text = displayName.Substring(0, 1).ToUpper();
                }
            }
        }

        protected void LogOut_Click(object sender, EventArgs e)
        {
            
            Session.Clear();
            Response.Redirect("~/pages/publics/login.aspx");
        }
    }
}