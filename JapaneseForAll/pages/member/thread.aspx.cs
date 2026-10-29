using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace JapaneseForAll.pages.member
{
    public partial class thread : System.Web.UI.Page
    {

        public class threadDetails
        {
            public string title { get; set; }
            public string description { get; set; }
            public string author { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public class threadMessage
        {
            public int UserId { get; set; }
            public bool IsInstructor { get; set; }
            public string Author { get; set; }
            public DateTime Timestamp { get; set; }
            public string message { get; set; }
        }

        List<threadMessage> messages = new List<threadMessage>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                string threadId = Request.QueryString["id"];

                if (!int.TryParse(threadId, out int id))
                {
                    Response.Redirect("~/pages/member/forum.aspx");
                    return;
                }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {
                    string query = @"SELECT
                                    T.TITLE,
                                    T.DESCRIPTION,
                                    CONCAT_WS(' ', U.FNAME, NULLIF(U.MNAME, ''), U.LNAME) AS AUTHOR,
                                    T.TIMESTAMP AS Timestamp
                                FROM THREADTABLE T
                                INNER JOIN USERTABLE U ON T.USER_ID = U.USER_ID
                                WHERE T.THREAD_ID = @id;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(threadId));

                        con.Open();

                        threadDetails details = new threadDetails();

                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            details.title = dr["TITLE"].ToString();
                            details.description = dr["DESCRIPTION"].ToString();
                            details.author = dr["AUTHOR"].ToString();
                            details.Timestamp = Convert.ToDateTime(dr["TIMESTAMP"]);
                        }

                        dr.Close();
                        con.Close();

                        threadDetailsRepeater.DataSource = new List<threadDetails> { details };
                        threadDetailsRepeater.DataBind();
                    }

                    query = @"
                    SELECT
                        R.USER_ID AS UserId,
                        CONCAT_WS(' ', U.FNAME, NULLIF(U.MNAME, ''), U.LNAME) AS Author,
                        R.CONTENT AS Message,
                        R.TIMESTAMP AS Timestamp,
                        U.ROLE AS Role
                    FROM REPLYTABLE R
                    INNER JOIN USERTABLE U
                        ON R.USER_ID = U.USER_ID
                    WHERE R.THREAD_ID = @threadId
                    ORDER BY R.TIMESTAMP DESC;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@threadId", Convert.ToInt32(threadId));

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            messages.Add(new threadMessage
                            {
                                UserId = Convert.ToInt32(row["UserId"]),
                                IsInstructor = row["Role"].ToString() == "Instructor",
                                Author = row["Author"].ToString(),
                                Timestamp = Convert.ToDateTime(row["Timestamp"]),
                                message = row["Message"].ToString()
                            });
                        }

                        con.Close();
                    }
                }

                threadRepeater.DataSource = messages;
                threadRepeater.DataBind();
            }
        }

        protected void OnReply_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["id"].ToString();

            if (Session["fname"] == null)
            {
                Response.Redirect("~/pages/publics/login.aspx");
                return;
            }

            if (!int.TryParse(Request.QueryString["id"], out int id))
            {
                Response.Redirect("~/pages/member/forum.aspx");
                return;
            }

            if (!string.IsNullOrWhiteSpace(reply.Text))
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {
                    string query = "INSERT INTO REPLYTABLE(USER_ID, THREAD_ID, CONTENT) VALUES(@uid, @threadId, @message)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@uid", Session["uid"]);
                        cmd.Parameters.AddWithValue("@threadId", Convert.ToInt32(threadId));
                        cmd.Parameters.AddWithValue("@message", reply.Text.Trim());

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        con.Close();

                        if(rowsAffected > 0)
                        {
                            Response.Redirect("~/pages/member/thread?id=" + threadId);
                            return;
                        }
                        else
                        {
                            errMsg.Visible = true;
                            errMsg.ForeColor = System.Drawing.Color.Red;
                            errMsg.Text = "Error posting reply! Please try again later.";
                            
                            return;
                        }
                    }
                }
            }
        }
    }
}