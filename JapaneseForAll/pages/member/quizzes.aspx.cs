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
    public partial class WebForm2 : System.Web.UI.Page
    {

        public class Quiz
        {
            public string Title { get; set; }
            public string Level { get; set; }
            public int QuestionCount { get; set; }
            public int QuizId { get; set; }
        }

        private string[] validLevels = { "N5", "N4", "N3", "N2", "N1" };

        List<Quiz> quizzes = new List<Quiz> { };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["fname"] == null) { Response.Redirect("~/pages/publics/login.aspx"); }
            if (!IsPostBack)
            {
                quizzes = new List<Quiz>();

                try
                {
                    string level = Request.QueryString["level"];

                    if (!string.IsNullOrEmpty(level) && !validLevels.Contains(level))
                    {
                        errMsg.Visible = true;
                        errMsg.ForeColor = System.Drawing.Color.Red;
                        errMsg.Text = "That is not a valid level!";
                        return;
                    }

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString);
                    con.Open();

                    string query = @"
                    SELECT
                        q.QUIZ_ID,
                        q.TITLE,
                        q.LEVEL,
                        q.QUESTION_COUNT
                    FROM QUIZTABLE q
                    WHERE q.STATUS = 'PUBLISHED'";

                    SqlCommand cmd = new SqlCommand(query, con);

                    if (!string.IsNullOrEmpty(level))
                    {
                        query += " AND q.LEVEL = @LEVEL";
                        cmd.Parameters.AddWithValue("@LEVEL", level);
                    }

                    

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow quiz in dt.Rows)
                    {
                        quizzes.Add(new Quiz
                        {
                            QuizId = Convert.ToInt32(quiz["QUIZ_ID"]),
                            Title = quiz["TITLE"].ToString(),
                            Level = quiz["LEVEL"].ToString(),
                            QuestionCount = Convert.ToInt32(quiz["QUESTION_COUNT"])
                        });
                    }

                    quizRepeater.DataSource = quizzes;
                    quizRepeater.DataBind();

                    con.Close();

                    if (quizzes.Count < 1)
                    {
                        errMsg.Visible = true;
                        errMsg.ForeColor = System.Drawing.Color.Red;
                        errMsg.Text = "There are no quizzes at the moment! Come back later.";
                    }

                }
                catch (Exception ex)
                {
                    errMsg.Visible = true;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Text = "Database error: " + ex.Message;

                    System.Diagnostics.Trace.TraceError(ex.ToString());
                }
            }
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();

            var filteredQuizzes = quizzes.Where(q => q.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            quizRepeater.DataSource = filteredQuizzes;
            quizRepeater.DataBind();
        }
    }
}