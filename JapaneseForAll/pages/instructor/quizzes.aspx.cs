using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll
{
    public partial class quizzes : System.Web.UI.Page
    {

        public class Quiz
        {
            public string Title { get; set; }
            public string Level { get; set; }
            public string Status { get; set; }
            public string QuizID { get; set; }
        }

        List<Quiz> quizList = new List<Quiz>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["fname"] == null) { Response.Redirect("~/pages/publics/login.aspx"); }
            if (!IsPostBack)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {
                    string query = @"
                                    SELECT 
                                        Q.QUIZ_ID AS QuizID,
                                        Q.TITLE AS Title,
                                        Q.LEVEL AS Level,
                                        Q.STATUS AS Status
                                    FROM QUIZTABLE Q
                                    ORDER BY Q.TITLE;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        foreach(DataRow row in dt.Rows)
                        {
                            if (row["Status"].ToString().Trim() != "DELETED")
                            {
                                quizList.Add(new Quiz
                                {
                                    Title = row["Title"].ToString().Trim(),
                                    Level = row["Level"].ToString().Trim(),
                                    Status = row["Status"].ToString().Trim(),
                                    QuizID = row["QuizID"].ToString().Trim()
                                }); 
                            }
                        }

                        con.Close();
                        quizRepeater.DataSource = quizList;
                        quizRepeater.DataBind();
                    }
                }
            }
        }

        protected void DeleteQuiz_Command(object sender, CommandEventArgs e)
        {
            int quizID = Convert.ToInt32(e.CommandArgument);
            
            using(SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
            {
                string query = "UPDATE QUIZTABLE SET STATUS = 'DELETED' WHERE QUIZ_ID = @quizID";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@quizID", quizID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if(rowsAffected > 0)
                    {
                        errMsg.Visible = true;
                        errMsg.ForeColor = System.Drawing.Color.Green;
                        errMsg.Text = "Successfully deleted quiz!";

                        Response.Redirect("~/pages/instructor/quizzes.aspx");
                    }
                }
            }
        }
    }
}