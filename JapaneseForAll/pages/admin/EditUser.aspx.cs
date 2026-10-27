using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.admin
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {
                string userID = Request.QueryString["id"];

                if(!string.IsNullOrEmpty(userID))
                {
                    loadFormDetails(userID);
                }
            }
        }

        protected void loadFormDetails(string userID)
        {
            string connString = ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;
            string query = "SELECT FNAME, LNAME, EMAIL, ROLE FROM USERTABLE WHERE USER_ID = @UserID";

            using (SqlConnection conn = new SqlConnection(connString)) 
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            txtFirst.Value = reader["FNAME"].ToString();
                            txtLast.Value = reader["LNAME"].ToString();
                            txtEmail.Value = reader["EMAIL"].ToString();

                            string role = reader["ROLE"].ToString();
                            if (role == "Member") radioMember.Checked = true;
                            else if (role == "Instructor") radioInstructor.Checked = true;
                            else if (role == "Admin") radioAdmin.Checked = true;
                        }

                    }
                }
               

            }
        
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            string userID = Request.QueryString["id"];

            string firstname = txtFirst.Value;
            string lastname = txtLast.Value;
            string email = txtEmail.Value;

            string selectedRole = "Member";
            if (radioMember.Checked) selectedRole = "Member";
            else if (radioInstructor.Checked) selectedRole = "Instructor";
            else if (radioAdmin.Checked) selectedRole = "Admin";

            string connString = ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            string updateQuery = "UPDATE USERTABLE SET FNAME = @Fname, LNAME = @Lname, EMAIL = @Email, ROLE = @Role WHERE USER_ID = @UserId";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Fname", firstname);
                    cmd.Parameters.AddWithValue("@Lname", lastname);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Role", selectedRole);
                    cmd.Parameters.AddWithValue("@UserId", userID);


                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }

            Response.Redirect("management.aspx");
         

        }

    }
}