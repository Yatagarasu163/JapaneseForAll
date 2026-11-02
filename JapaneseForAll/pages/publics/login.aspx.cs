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
                    using (SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                    {
                        con.Open();

                        string query = @"
                SELECT USER_ID, FNAME, ROLE, PROFILE_PHOTO, STATUS
                FROM USERTABLE
                WHERE USERNAME = @username
                  AND PASSWORD = @password
                  AND STATUS = 'ACTIVE'";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@username", username.Text);
                            cmd.Parameters.AddWithValue("@password", password.Text);

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    string role = dr["ROLE"].ToString().Trim();
                                    string name = dr["FNAME"].ToString().Trim();
                                    string uid = dr["USER_ID"].ToString().Trim();
                                    string photo = Convert.ToString(dr["PROFILE_PHOTO"]);

                                    // Check instructor verification
                                    if (role == "Instructor")
                                    {
                                        string verificationStatus =
                                            Convert.ToString(dr["STATUS"]).Trim();

                                        if (verificationStatus != "VERIFIED")
                                        {
                                            errMsg.Visible = true;
                                            errMsg.ForeColor = System.Drawing.Color.Red;
                                            errMsg.Text = "Your instructor account has not been verified yet. Please wait for administrator approval.";
                                            return;
                                        }
                                    }

                                    Session["fname"] = name;
                                    Session["photo"] = photo;
                                    Session["uid"] = uid;
                                    Session["role"] = role;

                                    if (role == "Member")
                                    {
                                        Response.Redirect("../member/dashboard.aspx");
                                    }
                                    else if (role == "Instructor")
                                    {
                                        Response.Redirect("../instructor/content.aspx");
                                    }
                                    else if (role == "Admin")
                                    {
                                        Response.Redirect("../admin/management.aspx");
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
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errMsg.Visible = true;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Text = "Login not successful! " + ex.Message;
                }
            }

        }
    }
}
