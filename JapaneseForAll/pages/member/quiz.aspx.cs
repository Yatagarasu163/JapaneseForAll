using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.member
{
    public partial class quiz : System.Web.UI.Page
    {

        public class Question
        {
            public int QuestionID { get; set; }
            public string QuestionDescription { get; set; }
            public string CorrectAnswer { get; set; }
            public List<Answer> Answers { get; set; } = new List<Answer>();
        }

        public class Answer
        {
            public string AnswerText { get; set; }
        }

        public class AnswerResult
        {
            public int QuestionId {  get; set; }
            public string SelectedAnswer { get; set; }
            public string CorrectAnswer { get; set; }
            public bool IsCorrect {  get; set; }
        }

        private int quizId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("~/pages/publics/login.aspx");
            }
            if (!IsPostBack) {
                if (!int.TryParse(Request.QueryString["id"], out int quizId))
                {
                    Response.Redirect("~/pages/member/quizzes.aspx");
                    return;
                }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {
                    string query = "SELECT TITLE FROM QUIZTABLE WHERE QUIZ_ID=@id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", quizId);

                        con.Open();

                        object result = cmd.ExecuteScalar();

                        if(result == null)
                        {
                            Response.Redirect("~/pages/member/quizzes.aspx");
                            return;
                        }

                        quizTitle.Text = Server.HtmlEncode(result.ToString());
                    }

                    questionRepeater.DataSource = GetQuestions(quizId);
                    questionRepeater.DataBind();

                }


            }


        }

        private List<Question> GetQuestions(int quizId)
        {
            List<Question> questions = new List<Question>();

            string query = @"
                SELECT QUESTION_ID, QUESTION_STRING, ANSWER_1, ANSWER_2, ANSWER_3, ANSWER_4, CORRECT_ANSWER
                FROM QUESTIONTABLE
                WHERE QUIZ_ID = @QuizId
                ORDER BY QUESTION_ID";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@QuizId", quizId);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Question question = new Question
                            {
                                QuestionID = Convert.ToInt32(dr["QUESTION_ID"]),
                                QuestionDescription = dr["QUESTION_STRING"].ToString(),
                                CorrectAnswer = dr["CORRECT_ANSWER"].ToString()
                            };

                            for (int i = 1; i <= 4; i++)
                            {
                                question.Answers.Add(new Answer
                                {
                                    AnswerText = dr["ANSWER_" + i].ToString()
                                });
                            }

                            questions.Add(question);
                        }
                    }
                }
            }

            return questions;
        }

        protected void questionRepeater_ItemDataBound(object sendder, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Question question = (Question)e.Item.DataItem;

                RadioButtonList answerList = (RadioButtonList)e.Item.FindControl("answerList");

                answerList.DataSource = question.Answers;
                answerList.DataTextField = "AnswerText";
                answerList.DataValueField = "AnswerText";
                answerList.DataBind();
            }
        }

        protected void submitQuiz_Click(object sender, EventArgs e)
        {
            errMsg.Visible = false;

            // Get the quiz ID from the URL.
            if (!int.TryParse(Request.QueryString["id"], out int quizId))
            {
                Response.Redirect("~/pages/member/quizzes.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["uid"]);

            List<AnswerResult> answerResults = new List<AnswerResult>();

            int score = 0;
            int total = 0;

            // Validate that every question has been answered.
            foreach (RepeaterItem item in questionRepeater.Items)
            {
                RadioButtonList answerList =
                    (RadioButtonList)item.FindControl("answerList");

                if (string.IsNullOrEmpty(answerList.SelectedValue))
                {
                    errMsg.Visible = true;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Text = "Please answer every question before submitting.";
                    return;
                }
            }

            string connectionString =
                ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            // Collect the selected and correct answers.
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (RepeaterItem item in questionRepeater.Items)
                {
                    HiddenField questionIdField =
                        (HiddenField)item.FindControl("questionId");

                    RadioButtonList answerList =
                        (RadioButtonList)item.FindControl("answerList");

                    int questionId = Convert.ToInt32(questionIdField.Value);
                    string selectedAnswer = answerList.SelectedValue;

                    string query = @"
                SELECT CORRECT_ANSWER
                FROM QUESTIONTABLE
                WHERE QUESTION_ID = @QuestionID
                  AND QUIZ_ID = @QuizID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@QuestionID", questionId);
                        cmd.Parameters.AddWithValue("@QuizID", quizId);

                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            throw new Exception(
                                "Could not find question " + questionId +
                                " for quiz " + quizId + ".");
                        }

                        string correctAnswer = result.ToString();

                        bool isCorrect = string.Equals(
                            selectedAnswer,
                            correctAnswer,
                            StringComparison.Ordinal);

                        answerResults.Add(new AnswerResult
                        {
                            QuestionId = questionId,
                            SelectedAnswer = selectedAnswer,
                            CorrectAnswer = correctAnswer,
                            IsCorrect = isCorrect
                        });

                        total++;

                        if (isCorrect)
                        {
                            score++;
                        }
                    }
                }
            }

            // Save the overall result and its details atomically.
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int resultId;

                        string resultQuery = @"
                    INSERT INTO RESULTTABLE
                        (USER_ID, QUIZ_ID, SCORE, TOTAL_QUESTIONS)
                    VALUES
                        (@UserID, @QuizID, @Score, @TotalQuestions);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        using (SqlCommand cmd =
                            new SqlCommand(resultQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@QuizID", quizId);
                            cmd.Parameters.AddWithValue("@Score", score);
                            cmd.Parameters.AddWithValue("@TotalQuestions", total);

                            resultId = (int)cmd.ExecuteScalar();
                        }

                        string detailQuery = @"
                    INSERT INTO RESULTDETAILTABLE
                    (
                        RESULT_ID,
                        QUESTION_ID,
                        SELECTED_ANSWER,
                        CORRECT_ANSWER,
                        IS_ANSWERED,
                        IS_CORRECT
                    )
                    VALUES
                    (
                        @ResultID,
                        @QuestionID,
                        @SelectedAnswer,
                        @CorrectAnswer,
                        @IsAnswered,
                        @IsCorrect
                    )";

                        foreach (AnswerResult answer in answerResults)
                        {
                            using (SqlCommand cmd =
                                new SqlCommand(detailQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ResultID", resultId);
                                cmd.Parameters.AddWithValue(
                                    "@QuestionID", answer.QuestionId);
                                cmd.Parameters.AddWithValue(
                                    "@SelectedAnswer", answer.SelectedAnswer);
                                cmd.Parameters.AddWithValue(
                                    "@CorrectAnswer", answer.CorrectAnswer);
                                cmd.Parameters.AddWithValue("@IsAnswered", true);
                                cmd.Parameters.AddWithValue(
                                    "@IsCorrect", answer.IsCorrect);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        // Open the result page for this specific attempt.
                        Response.Redirect(
                            "~/pages/member/result.aspx?resultId=" + resultId,
                            false);

                        Context.ApplicationInstance.CompleteRequest();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}