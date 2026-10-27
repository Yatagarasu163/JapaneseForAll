using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace JapaneseForAll.pages.admin
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["JPDB"]?.ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPendingInstructors();
            }
        }

        private void LoadPendingInstructors()
        {
            if (string.IsNullOrEmpty(connString)) return;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Updated query to match your actual database columns: USER_ID, FNAME, MNAME, LNAME, EMAIL, CERTIFICATE, STATUS
                
                string query = "SELECT USER_ID, FNAME, MNAME, LNAME, EMAIL, CERTIFICATE, STATUS FROM USERTABLE WHERE CERTIFICATE IS NOT NULL AND UPPER(STATUS) = 'PENDING'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Combine FNAME, MNAME, and LNAME into a single FullName column for the Repeater
                        dt.Columns.Add("FullName", typeof(string));
                        dt.Columns.Add("CertificateFileName", typeof(string));
                        dt.Columns.Add("SubmittedText", typeof(string));

                        foreach (DataRow row in dt.Rows)
                        {
                            string fname = row["FNAME"]?.ToString() ?? "";
                            string mname = row["MNAME"] != DBNull.Value ? row["MNAME"].ToString() : "";
                            string lname = row["LNAME"]?.ToString() ?? "";

                            row["FullName"] = $"{fname} {(string.IsNullOrEmpty(mname) ? "" : mname + " ")}".Trim() + $" {lname}".Trim();

                            // Extract file name from the path stored in CERTIFICATE (e.g., "~/Certificates/file.pdf" -> "file.pdf")
                            string certPath = row["CERTIFICATE"]?.ToString() ?? "";
                        

                
                        }

                        rptPendingInstructors.DataSource = dt;
                        rptPendingInstructors.DataBind();

                        lblPendingCount.Text = $"{dt.Rows.Count} pending";
                        lblMessage.Visible = (dt.Rows.Count == 0);
                    }
                }
            }
        }

        protected void rptPendingInstructors_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "";
                if (e.CommandName == "Approve")
                {
                    // Update STATUS to Active/Verified and ensure ROLE is set to Instructor
                    query = "UPDATE USERTABLE SET STATUS = 'ACTIVE', ROLE = 'Instructor' WHERE USER_ID = @UserId";
                }
                else if (e.CommandName == "Reject")
                {
                    // Clear certificate or update status back to Member/Rejected
                    query = "UPDATE USERTABLE SET STATUS = 'REJECTED', CERTIFICATE = NULL WHERE USER_ID = @UserId";
                }

                if (!string.IsNullOrEmpty(query))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            LoadPendingInstructors();
        }
    }
}