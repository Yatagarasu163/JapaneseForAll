using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

namespace JapaneseForAll.pages.member
{
    public partial class profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["fname"] == null)
            {
                Response.Redirect("~/pages/publics/login.aspx");
            }

            if (!IsPostBack)
            {
                errMsg.Visible = false;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {
                    string query = "SELECT FNAME, MNAME, LNAME, EMAIL, PASSWORD FROM USERTABLE WHERE USER_ID=@uid";

                    using(SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@uid", Session["uid"]);

                        con.Open();

                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            fname.Text = dr["FNAME"].ToString().Trim();
                            mname.Text = dr["MNAME"] == DBNull.Value ? "" : dr["MNAME"].ToString().Trim();
                            lname.Text = dr["LNAME"].ToString().Trim();
                            email.Text = dr["EMAIL"].ToString().Trim();
                        }

                        dr.Close();
                        con.Close();
                    }
                }
            }
        }

        protected void OnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
            {
                string query = "UPDATE USERTABLE SET FNAME=@fname, MNAME=@mname, LNAME=@lname, EMAIL=@email WHERE USER_ID=@uid";

                using(SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@fname", fname.Text.Trim());
                    cmd.Parameters.AddWithValue("@mname", string.IsNullOrEmpty(mname.Text.Trim()) ? (object)DBNull.Value : mname.Text.Trim());
                    cmd.Parameters.AddWithValue("@lname", lname.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", email.Text.Trim());
                    cmd.Parameters.AddWithValue("@uid", Session["uid"]);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    errMsg.Visible = true;

                    if(rowsAffected > 0)
                    {
                        errMsg.ForeColor = System.Drawing.Color.Green;
                        errMsg.Text = "Profile updated successfully!";
                    }
                    else
                    {
                        errMsg.ForeColor = System.Drawing.Color.Red;
                        errMsg.Text = "Unable to update profile.";
                    }
                }
            }
        }

        protected void OnUpdate_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(password.Text) || string.IsNullOrWhiteSpace(newpassword.Text) || string.IsNullOrWhiteSpace(confirmpassword.Text))
            {
                errMsg.Visible = true;
                errMsg.ForeColor = System.Drawing.Color.Red;
                errMsg.Text = "Please fill in all password fields!";
                return;
            }

            if (password.Text.Trim() != newpassword.Text.Trim())
            {

                using(SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {
                    con.Open();

                    string query = "SELECT PASSWORD FROM USERTABLE WHERE USER_ID = @uid";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@uid", Session["uid"]);

                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value || result.ToString() != password.Text)
                        {
                            errMsg.Visible = true;
                            errMsg.ForeColor = System.Drawing.Color.Red;
                            errMsg.Text = "Current password is incorrect!";
                            return;
                        }
                    }
                }


                if(newpassword.Text.Trim() == confirmpassword.Text.Trim())
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                    {
                        string query = "UPDATE USERTABLE SET PASSWORD=@confirmpassword WHERE USER_ID=@uid";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@confirmpassword", confirmpassword.Text.Trim());
                            cmd.Parameters.AddWithValue("@uid", Session["uid"]);

                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                errMsg.Visible = true;
                                errMsg.ForeColor = System.Drawing.Color.Green;
                                errMsg.Text = "Password updated successfully!";

                                password.Text = "";
                                newpassword.Text = "";
                                confirmpassword.Text = "";
                            }
                            else
                            {
                                errMsg.Visible = true;
                                errMsg.ForeColor = System.Drawing.Color.Red;
                                errMsg.Text = "Unable to update password!";

                            }
                        }
                    }
                }
                else
                {
                    errMsg.Visible = true;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Text = "New password and confirm password are not the same!";
                    return;
                }
            }
            else
            {
                errMsg.Visible = true;
                errMsg.ForeColor = System.Drawing.Color.Red;
                errMsg.Text = "The new password cannot be the same as the old password!";
                return;
            }
        }
    }
}