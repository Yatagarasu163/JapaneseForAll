using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.admin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Repeater RptUsers;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Security Check
            if (Session["role"] == null || Session["role"].ToString() != "Admin")
            {
                Response.Redirect("~/pages/publics/login.aspx");
            }

            // 2. Load Stats and Table Data (Only on first page load, not postbacks)
            if (!IsPostBack)
            {
                LoadDashboardStats();
                BindUserTable(); 
            }
        }

        private void LoadDashboardStats()
        {
            string connString = ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string totalQuery = "SELECT COUNT(*) FROM USERTABLE";
                int totalUsers = 0;
                using (SqlCommand cmd = new SqlCommand(totalQuery, conn))
                {
                    totalUsers = (int)cmd.ExecuteScalar();
                    TotalUsers.InnerText = totalUsers.ToString();
                }

                string statusQuery = "SELECT COUNT(*) FROM USERTABLE WHERE CERTIFICATE = 'Pending'";
                using (SqlCommand cmd = new SqlCommand(statusQuery, conn))
                {
                    int totalPending = (int)cmd.ExecuteScalar();
                    Status.InnerText = totalPending.ToString();
                }

                string activeQuery = "SELECT COUNT(*) FROM USERTABLE WHERE STATUS = 'ACTIVE'";
                int totalActive = 0;
                using (SqlCommand cmd = new SqlCommand(activeQuery, conn))
                {
                    totalActive = (int)cmd.ExecuteScalar();
                    Active.InnerText = totalActive.ToString();
                }

                if (totalUsers > 0)
                {
                    double percentage = ((double)totalActive / totalUsers) * 100;
                    ActivePercentage.InnerText = Math.Round(percentage, 0).ToString() + "% of total users";
                }
                else
                {
                    ActivePercentage.InnerText = "0% of total users";
                }
            }
        }

        
        private void BindUserTable()
        {
            string connString = ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT USER_ID, USERNAME, EMAIL, ROLE, STATUS FROM USERTABLE";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        // Binds the database rows to your ASP:Repeater control in the HTML
                        RepeatUsers.DataSource = dt;
                        RepeatUsers.DataBind();
                    }
                }
            }
        }

        protected void BtnDelete(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                string connString = ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    //delete replies
                    string deleteReplies = "DELETE FROM REPLYTABLE WHERE THREAD_ID IN (SELECT THREAD_ID FROM THREADTABLE WHERE USER_ID = @Id)";
                    using (SqlCommand cmdReplies = new SqlCommand(deleteReplies, conn))
                    {
                        cmdReplies.Parameters.AddWithValue("@Id", userId);
                        cmdReplies.ExecuteNonQuery();
                    }

                    // Step 1: Delete their posts/threads first so the database doesn't block us
                    string deleteThreads = "DELETE FROM THREADTABLE WHERE USER_ID = @Id";
                    using (SqlCommand cmdThreads = new SqlCommand(deleteThreads, conn))
                    {
                        cmdThreads.Parameters.AddWithValue("@Id", userId);
                        cmdThreads.ExecuteNonQuery();
                    }

                    // Step 2: Now delete the user safely
                    string deleteUser = "DELETE FROM USERTABLE WHERE USER_ID = @Id";
                    using (SqlCommand cmdUser = new SqlCommand(deleteUser, conn))
                    {
                        cmdUser.Parameters.AddWithValue("@Id", userId);
                        cmdUser.ExecuteNonQuery();
                    }
                }

                // Refresh the table so the user disappears from the screen
                BindUserTable();
            }
        }
    }
}