using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace JapaneseForAll.pages.member
{
    public partial class forum : System.Web.UI.Page
    {

        public class Thread
        {
            public int ThreadId {  get; set; }
            public string ThreadTitle { get; set; }
            public bool IsPinned { get; set; }
            public string Author { get; set; }
            public int ReplyCount { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();

            List<Thread> threads = new List<Thread>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
            {
                string query = @"
                    SELECT 
                        T.THREAD_ID AS THREAD_ID, 
                        T.TITLE AS TITLE,
                        T.STATUS AS STATUS,
                        U.FNAME + ' ' + U.LNAME AS AUTHOR,
                        COUNT(R.REPLY_ID) AS REPLYCOUNT
                    FROM THREADTABLE T
                    INNER JOIN USERTABLE U 
                        ON T.USER_ID = U.USER_ID
                    LEFT JOIN REPLYTABLE R
                        ON T.THREAD_ID = R.THREAD_ID
                    WHERE T.STATUS != 'DELETED'
                    GROUP BY 
                        T.THREAD_ID,
                        T.TITLE,
                        T.STATUS,
                        U.FNAME,
                        U.LNAME
                    ORDER BY T.THREAD_ID DESC
                    ;
                    ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using(SqlDataAdapter da  = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                threads.Add(new Thread
                {
                    ThreadId = Convert.ToInt32(row["THREAD_ID"]),
                    ThreadTitle = row["TITLE"].ToString(),
                    Author = row["AUTHOR"].ToString(),
                    ReplyCount = Convert.ToInt32(row["REPLYCOUNT"]),
                    IsPinned = row["STATUS"].ToString() == "PINNED",
                }); 
            }

            if (threads.Count < 1)
            {
                errMsg.Visible = true;
                errMsg.ForeColor = System.Drawing.Color.Red;
                errMsg.Text = "There are no threads at the moment! Come back later or make a new thread.";
                return;
            }

            threadRepeater.DataSource = threads;
            threadRepeater.DataBind();


        }
    }
}