
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace JapaneseForAll.pages.member
{
    public partial class WebForm1 : Page
    {
        public class Flashcard
        {
            public string FlashcardTitle { get; set; }
            public int FlashcardID { get; set; }
        }

        public class Quiz
        {
            public string QuizTitle { get; set; }
            public int QuizID { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("~/pages/publics/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                fname2.Text = Server.HtmlEncode(
                    Convert.ToString(Session["fname"]));

                LoadDashboard();
            }
        }

        private void LoadDashboard()
        {
            int userId = Convert.ToInt32(Session["uid"]);

            string connectionString =
                ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Count published quizzes and those completed by this user.
                string completionQuery = @"
                            SELECT
                                COUNT(DISTINCT Q.QUIZ_ID) AS TotalQuizzes,
                                COUNT(DISTINCT CASE
                                    WHEN R.QUIZ_ID IS NOT NULL THEN Q.QUIZ_ID
                                END) AS CompletedQuizzes
                            FROM QUIZTABLE Q
                            LEFT JOIN RESULTTABLE R
                                ON R.QUIZ_ID = Q.QUIZ_ID
                                AND R.USER_ID = @UserID
                            WHERE Q.STATUS = 'PUBLISHED'";

                using (SqlCommand cmd = new SqlCommand(completionQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            int total = Convert.ToInt32(dr["TotalQuizzes"]);

                            int completed = dr["CompletedQuizzes"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(dr["CompletedQuizzes"]);

                            completedModules.Text = completed + " / " + total;
                        }
                    }
                }

                // Average percentage across the user's quiz attempts.
                string averageQuery = @"
                    SELECT AVG(
                        CAST(SCORE AS FLOAT) * 100.0 /
                        NULLIF(TOTAL_QUESTIONS, 0)
                    )
                    FROM RESULTTABLE
                    WHERE USER_ID = @UserID";

                using (SqlCommand cmd =
                    new SqlCommand(averageQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    object average = cmd.ExecuteScalar();

                    quizAverage.Text =
                        average == DBNull.Value
                            ? "N/A"
                            : Convert.ToDouble(average).ToString("0.0") + "%";
                }

                // Count the user's non-deleted forum threads.
                string forumQuery = @"
                    SELECT COUNT(*)
                    FROM THREADTABLE
                    WHERE USER_ID = @UserID
                      AND (STATUS IS NULL OR STATUS <> 'DELETED')";

                using (SqlCommand cmd =
                    new SqlCommand(forumQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    forumPosts.Text = cmd.ExecuteScalar().ToString();
                }

                // Load published quizzes the user has not completed.
                string quizQuery = @"
                    SELECT Q.QUIZ_ID, Q.TITLE
                    FROM QUIZTABLE Q
                    WHERE Q.STATUS = 'PUBLISHED'
                      AND NOT EXISTS (
                          SELECT 1
                          FROM RESULTTABLE R
                          WHERE R.QUIZ_ID = Q.QUIZ_ID
                            AND R.USER_ID = @UserID
                      )
                    ORDER BY Q.TITLE";

                using (SqlCommand cmd =
                    new SqlCommand(quizQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        quizRepeater.DataSource = dt;
                        quizRepeater.DataBind();
                    }
                }
            }

            // Flashcards are loaded separately once their completion
            // tracking table and column names are confirmed.
        }
    }
}
