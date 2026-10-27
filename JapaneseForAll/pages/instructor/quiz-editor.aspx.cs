
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using static JapaneseForAll.pages.instructor.quiz_editor;

namespace JapaneseForAll.pages.instructor
{
    public partial class quiz_editor : System.Web.UI.Page
    {
        [Serializable]
        public class QuizQuestion
        {
            public string Question { get; set; } = "";
            public string Option1 { get; set; } = "";
            public string Option2 { get; set; } = "";
            public string Option3 { get; set; } = "";
            public string Option4 { get; set; } = "";
            public string CorrectAnswer { get; set; } = "1";
        }

        private List<QuizQuestion> Questions
        {
            get
            {
                if (ViewState["Questions"] == null)
                {
                    ViewState["Questions"] =
                        new List<QuizQuestion>();
                }

                return (List<QuizQuestion>)ViewState["Questions"];
            }
            set
            {
                ViewState["Questions"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["fname"] == null)
            {
                Response.Redirect("~/pages/publics/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string type = Request.QueryString["type"];
                string idValue = Request.QueryString["id"];

                if (type == "new")
                {
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion()
                    };

                    title.Text = "";
                    ddlLevel.SelectedValue = "N5";
                    ddlStatus.SelectedValue = "Draft";

                    BindQuestions();
                }
                else if (!string.IsNullOrWhiteSpace(idValue))
                {
                    if (int.TryParse(idValue, out int quizId) && quizId > 0)
                    {
                        LoadQuiz(quizId);
                    }
                    else
                    {
                        Response.Redirect("quizzes.aspx");
                    }
                }
                else
                {
                    Response.Redirect("quizzes.aspx");
                }
            }
        }

        private void LoadQuiz(int quizId)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT
                Q.TITLE,
                Q.LEVEL,
                Q.STATUS,
                QT.QUESTION_ID,
                QT.QUESTION_STRING,
                QT.ANSWER_1,
                QT.ANSWER_2,
                QT.ANSWER_3,
                QT.ANSWER_4,
                QT.CORRECT_ANSWER
            FROM QUIZTABLE Q
            INNER JOIN QUESTIONTABLE QT
                ON Q.QUIZ_ID = QT.QUIZ_ID
            WHERE Q.QUIZ_ID = @QuizId
            ORDER BY QT.QUESTION_ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@QuizId", SqlDbType.Int).Value = quizId;

                    conn.Open();

                    List<QuizQuestion> questions = new List<QuizQuestion>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            title.Text = reader["TITLE"].ToString();

                            string level = reader["LEVEL"].ToString();
                            if (ddlLevel.Items.FindByValue(level) != null)
                                ddlLevel.SelectedValue = level;

                            string status = reader["STATUS"].ToString();
                            if (ddlStatus.Items.FindByValue(status) != null)
                                ddlStatus.SelectedValue = status;

                            questions.Add(new QuizQuestion
                            {
                                Question = reader["QUESTION_STRING"].ToString(),
                                Option1 = reader["ANSWER_1"].ToString(),
                                Option2 = reader["ANSWER_2"].ToString(),
                                Option3 = reader["ANSWER_3"].ToString(),
                                Option4 = reader["ANSWER_4"].ToString(),
                                CorrectAnswer = reader["CORRECT_ANSWER"].ToString()
                            });
                        }
                    }

                    if (questions.Count == 0)
                    {
                        questions.Add(new QuizQuestion());
                    }

                    Questions = questions;
                    BindQuestions();
                }
            }
        }

        private void BindQuestions()
        {
            rptQuestions.DataSource = Questions;
            rptQuestions.DataBind();
        }

        private void SaveCurrentQuestions()
        {
            List<QuizQuestion> updated =
                new List<QuizQuestion>();

            foreach (RepeaterItem item in rptQuestions.Items)
            {
                QuizQuestion q = new QuizQuestion();

                q.Question =
                    ((TextBox)item.FindControl("txtQuestion")).Text;

                q.Option1 =
                    ((TextBox)item.FindControl("txtOption1")).Text;

                q.Option2 =
                    ((TextBox)item.FindControl("txtOption2")).Text;

                q.Option3 =
                    ((TextBox)item.FindControl("txtOption3")).Text;

                q.Option4 =
                    ((TextBox)item.FindControl("txtOption4")).Text;

                if (((RadioButton)item.FindControl("rbCorrect1")).Checked)
                    q.CorrectAnswer = "1";
                else if (((RadioButton)item.FindControl("rbCorrect2")).Checked)
                    q.CorrectAnswer = "2";
                else if (((RadioButton)item.FindControl("rbCorrect3")).Checked)
                    q.CorrectAnswer = "3";
                else if (((RadioButton)item.FindControl("rbCorrect4")).Checked)
                    q.CorrectAnswer = "4";

                updated.Add(q);
            }

            Questions = updated;
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            SaveCurrentQuestions();

            Questions.Add(new QuizQuestion());

            BindQuestions();
            upQuestions.Update();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            SaveCurrentQuestions();

            LinkButton btn = (LinkButton)sender;
            int index = Convert.ToInt32(btn.CommandArgument);

            if (Questions.Count > 1 &&
                index >= 0 &&
                index < Questions.Count)
            {
                Questions.RemoveAt(index);
            }

            BindQuestions();
            upQuestions.Update();
        }


        protected void btnSaveQuiz_Click(object sender, EventArgs e)
        {
            SaveCurrentQuestions();

            List<QuizQuestion> questions = Questions;

            // Validate quiz title
            if (string.IsNullOrWhiteSpace(title.Text))
                return;

            // Validate questions
            foreach (QuizQuestion q in questions)
            {
                if (string.IsNullOrWhiteSpace(q.Question) ||
                    string.IsNullOrWhiteSpace(q.Option1) ||
                    string.IsNullOrWhiteSpace(q.Option2) ||
                    string.IsNullOrWhiteSpace(q.Option3) ||
                    string.IsNullOrWhiteSpace(q.Option4))
                {
                    return;
                }
            }

            // Determine whether this is a new or existing quiz
            string idValue = Request.QueryString["id"];
            bool isEditing = int.TryParse(idValue, out int quizId)
                             && quizId > 0;

            string connectionString =
                ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            string insertQuizQuery = @"
        INSERT INTO QUIZTABLE (TITLE, LEVEL, QUESTION_COUNT, STATUS)
        VALUES (@Title, @Level, @Question_Count, @Status);

        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            string updateQuizQuery = @"
        UPDATE QUIZTABLE
        SET TITLE = @Title,
            LEVEL = @Level,
            QUESTION_COUNT = @Question_Count,
            STATUS = @Status
        WHERE QUIZ_ID = @QuizId;";

            string deleteQuestionsQuery = @"
        DELETE FROM QUESTIONTABLE
        WHERE QUIZ_ID = @QuizId;";

            string insertQuestionQuery = @"
        INSERT INTO QUESTIONTABLE
            (QUIZ_ID, QUESTION_STRING, ANSWER_1, ANSWER_2,
             ANSWER_3, ANSWER_4, CORRECT_ANSWER)
        VALUES
            (@QuizId, @QuestionText, @Answer1, @Answer2,
             @Answer3, @Answer4, @CorrectAnswer);";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        if (isEditing)
                        {
                            // Update the existing quiz
                            using (SqlCommand cmd = new SqlCommand(
                                updateQuizQuery, con, transaction))
                            {
                                string status = ddlStatus.SelectedValue.ToString().ToUpper();
                                cmd.Parameters.AddWithValue("@Title", title.Text.Trim());
                                cmd.Parameters.AddWithValue("@Level", ddlLevel.SelectedValue);
                                cmd.Parameters.AddWithValue("@Question_Count", questions.Count);
                                cmd.Parameters.AddWithValue("@Status", status);
                                cmd.Parameters.AddWithValue("@QuizId", quizId);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected == 0)
                                    throw new Exception("The quiz could not be found.");
                            }

                            // Remove the existing questions for this quiz
                            using (SqlCommand cmd = new SqlCommand(
                                deleteQuestionsQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@QuizId", quizId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Create a new quiz and retrieve its ID
                            using (SqlCommand cmd = new SqlCommand(
                                insertQuizQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Title", title.Text.Trim());
                                cmd.Parameters.AddWithValue("@Level", ddlLevel.SelectedValue);
                                cmd.Parameters.AddWithValue("@Question_Count", questions.Count);
                                cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);

                                quizId = (int)cmd.ExecuteScalar();
                            }
                        }

                        // Insert the current questions
                        foreach (QuizQuestion question in questions)
                        {
                            using (SqlCommand cmd = new SqlCommand(
                                insertQuestionQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@QuizId", quizId);
                                cmd.Parameters.AddWithValue("@QuestionText", question.Question);
                                cmd.Parameters.AddWithValue("@Answer1", question.Option1);
                                cmd.Parameters.AddWithValue("@Answer2", question.Option2);
                                cmd.Parameters.AddWithValue("@Answer3", question.Option3);
                                cmd.Parameters.AddWithValue("@Answer4", question.Option4);
                                cmd.Parameters.AddWithValue("@CorrectAnswer", question.CorrectAnswer);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        Response.Redirect("quizzes.aspx");
                    }
                    catch(Exception ex) 
                    {

                        System.Diagnostics.Debug.WriteLine("Quiz save failed: " + ex.ToString());

                        if(transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }

                        throw;
                    }
                }
            }
        }
    }
}
