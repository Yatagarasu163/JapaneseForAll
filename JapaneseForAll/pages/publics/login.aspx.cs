using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace JapaneseForAll
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString);
                    con.Open();

                    string query = "SELECT COUNT(*) FROM USERTABLE WHERE USERNAME = @username AND PASSWORD = @password AND STATUS = 'ACTIVE'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username.Text);
                    cmd.Parameters.AddWithValue("@password", password.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                    if (count > 0)
                    {
                        SqlCommand cmdType = new SqlCommand("SELECT USER_ID, FNAME, ROLE FROM USERTABLE WHERE USERNAME=@username AND STATUS='ACTIVE'", con);
                        cmdType.Parameters.AddWithValue("@username", username.Text);
                        SqlDataReader dr = cmdType.ExecuteReader();

                        string role = "";
                        string name = "";
                        string uid = "";

                        while (dr.Read())
                        {
                            role = dr["ROLE"].ToString().Trim();
                            name = dr["FNAME"].ToString().Trim();
                            uid = dr["USER_ID"].ToString().Trim();
                        }

                        Session["fname"] = name;
                        Session["uid"] = uid;

                        if (role == "Member")
                        {
                            Response.Redirect("../member/dashboard.aspx");
                        } else if (role == "Instructor")
                        {
                            Response.Redirect("../instructor/dashboard.aspx");
                        } else if (role == "Admin")
                        {
                            Response.Redirect("../admin/dashboard.aspx");
                        }
                        else
                        {
                            Response.Redirect("login.aspx");
                        }
                    }
                    else
                    {
                        errMsg.Visible = true;
                        errMsg.ForeColor = System.Drawing.Color.Red;
                        errMsg.Text = "Username and password mismatch!";
                        return;
                    }

                    con.Close();

                }
                catch (Exception ex)
                {
                    errMsg.Visible = true;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Text = "Login not successful!" + ex.ToString();
                }
            }
        }
    }
}