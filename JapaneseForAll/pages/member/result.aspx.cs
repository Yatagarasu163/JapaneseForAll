using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace JapaneseForAll.pages.member
{
    public partial class result : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("~/pages/publics/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (!int.TryParse(
                    Request.QueryString["resultId"],
                    out int resultId))
                {
                    Response.Redirect("~/pages/member/quizzes.aspx");
                    return;
                }

                LoadResult(resultId);
            }
        }

        private void LoadResult(int resultId)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Load the overall quiz result and title.
                string resultQuery = @"
                    SELECT
                        Q.TITLE,
                        R.SCORE,
                        R.TOTAL_QUESTIONS
                    FROM RESULTTABLE R
                    INNER JOIN QUIZTABLE Q
                        ON Q.QUIZ_ID = R.QUIZ_ID
                    WHERE R.RESULT_ID = @ResultID
                      AND R.USER_ID = @UserID";

                int totalQuestions;
                int finalScore;

                using (SqlCommand cmd = new SqlCommand(resultQuery, con))
                {
                    cmd.Parameters.AddWithValue("@ResultID", resultId);
                    cmd.Parameters.AddWithValue(
                        "@UserID", Convert.ToInt32(Session["uid"]));

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            Response.Redirect(
                                "~/pages/member/quizzes.aspx");
                            return;
                        }

                        quizTitle.Text = Server.HtmlEncode(
                            dr["TITLE"].ToString());

                        finalScore = Convert.ToInt32(dr["SCORE"]);
                        totalQuestions =
                            Convert.ToInt32(dr["TOTAL_QUESTIONS"]);
                    }
                }

                // Display the score.
                score.Text = finalScore + " / " + totalQuestions;

                double percentage = totalQuestions > 0
                    ? (double)finalScore / totalQuestions * 100
                    : 0;

                scorePercent.Text =
                    percentage.ToString("0.#") + "% correct";

                // Load individual question results.
                string detailQuery = @"
                    SELECT
                        Q.QUESTION_STRING AS Question,
                        D.IS_CORRECT AS IsCorrect,
                        D.IS_ANSWERED AS IsAnswered
                    FROM RESULTDETAILTABLE D
                    INNER JOIN QUESTIONTABLE Q
                        ON Q.QUESTION_ID = D.QUESTION_ID
                    WHERE D.RESULT_ID = @ResultID
                    ORDER BY D.RESULT_DETAIL_ID";

                using (SqlCommand cmd =
                    new SqlCommand(detailQuery, con))
                {
                    cmd.Parameters.AddWithValue("@ResultID", resultId);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Add question numbering for display.
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["Question"] =
                                "Q" + (i + 1) + ". " +
                                dt.Rows[i]["Question"];
                        }

                        resultRepeater.DataSource = dt;
                        resultRepeater.DataBind();
                    }
                }
            }
        }
    }
}