using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace JapaneseForAll.pages.publics
{
    public partial class WebForm2 : System.Web.UI.Page
    {

        public class Quiz
        {
            public string Title { get; set; }
            public string Level { get; set; }
            public int QuestionCount {  get; set; }
            public string Url {  get; set; }

        }

        private string[] validLevels = { "N5", "N4", "N3", "N2", "N1" };

        List<Quiz> quizzes = new List<Quiz>
                {
                    new Quiz
                    {
                        Title = "Basic Greetings",
                        Level = "N5",
                        QuestionCount = 10,
                        Url = "quiz.aspx?id=1"
                    }

                };

        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                return;
            }

            quizzes = new List<Quiz>();

            try
            {
                string level = Request.QueryString["level"];

                if(!string.IsNullOrEmpty(level) && !validLevels.Contains(level))
                {
                    errMsg.Visible = true;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Text = "That is not a valid level!";
                }

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString);
                con.Open();

                string query = "SELECT TITLE, LEVEL, QUESTION_COUNT, URL FROM QUIZTABLE WHERE STATUS='ACTIVE'";

                if (!string.IsNullOrEmpty(level))
                {
                    query += " AND LEVEL = @LEVEL";
                }

                SqlCommand cmd = new SqlCommand(query, con);

                if (!string.IsNullOrEmpty(level))
                {
                    cmd.Parameters.AddWithValue("@LEVEL", level);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow quiz in dt.Rows)
                {
                    quizzes.Add(new Quiz
                    {
                        Title = quiz["TITLE"].ToString(),
                        Level = quiz["LEVEL"].ToString(),
                        QuestionCount = Convert.ToInt32(quiz["QUESTION_COUNT"]),
                        Url = quiz["URL"].ToString()
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
                System.Diagnostics.Trace.TraceError(ex.ToString());
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