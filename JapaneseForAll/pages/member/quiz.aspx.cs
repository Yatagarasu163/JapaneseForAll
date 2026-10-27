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

            foreach (RepeaterItem item in questionRepeater.Items)
            {
                RadioButtonList answerList = (RadioButtonList)item.FindControl("answerList");

                if (string.IsNullOrEmpty(answerList.SelectedValue))
                {
                    errMsg.Visible = true;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Text = "Please answer every question before submitting.";

                    return;
                }
            }


            int score = 0;
            int total = 0;

            string query = "";

            foreach (RepeaterItem item in questionRepeater.Items)
            {
                HiddenField questionIdField = (HiddenField)item.FindControl("questionId");

                RadioButtonList answerList = (RadioButtonList)item.FindControl("answerList");

                int questionId = Convert.ToInt32(questionIdField.Value);

                if (string.IsNullOrEmpty(answerList.SelectedValue))
                {
                    continue;
                }

                string selectedAnswer = answerList.SelectedValue;

                query = @"SELECT CORRECT_ANSWER FROM QUESTIONTABLE WHERE QUESTION_ID = @QuestionID";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@QuestionID", questionId);

                        con.Open();

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            total++;

                            if (string.Equals(selectedAnswer, result.ToString(), StringComparison.Ordinal))
                            {
                                score++;
                            }
                        }
                    }
                }
            }

            query = @"INSERT INTO RESULTTABLE (USER_ID, QUIZ_ID, SCORE, TOTAL_QUESTIONS) VALUES (@UserID, @QuizID, @Score, @TotalQuestions)";

            using(SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["uid"]));
                    cmd.Parameters.AddWithValue("@QuizID", quizId);
                    cmd.Parameters.AddWithValue("@Score", score);
                    cmd.Parameters.AddWithValue("@TotalQuestions", total);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            Response.Redirect("~/pages/member/quizzes.aspx");
        }
    }
}