using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.member
{
    public partial class flashcard_list : System.Web.UI.Page
    {

        public class Flashcard
        {
            public string Title { get; set; }
            public string Level { get; set; }
            public int FlashcardCount {  get; set; }
            public int FlashcardId { get; set; }
        }

        private string[] validLevels = { "N5", "N4", "N3", "N2", "N1" };

        List<Flashcard> flashcards = new List<Flashcard> { };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["fname"] == null) { Response.Redirect("~/pages/publics/login.aspx"); }
            if (!IsPostBack)
            {
                flashcards = new List<Flashcard>();

                    string level = Request.QueryString["level"];

                    if(!string.IsNullOrEmpty(level) && !validLevels.Contains(level))
                    {
                        errMsg.Visible = true;
                        errMsg.ForeColor = System.Drawing.Color.Red;
                        errMsg.Text = "That is not a valid level!";
                        return;
                    }

                    using(SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                    {
                    string query = @"
                        SELECT 
                            FLASHCARD_ID,
                            TITLE,
                            LEVEL,
                            FLASHCARD_COUNT
                        FROM FLASHCARDTABLE
                        WHERE STATUS = 'PUBLISHED'";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (!string.IsNullOrEmpty(level))
                        {
                            query += " AND LEVEL = @Level";
                            cmd.Parameters.AddWithValue("@Level", level);
                        }

                        con.Open();


                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        foreach(DataRow flashcard in dt.Rows)
                        {
                            flashcards.Add(new Flashcard
                            {
                                FlashcardId = Convert.ToInt32(flashcard["FLASHCARD_ID"]),
                                Title = flashcard["TITLE"].ToString(),
                                Level = flashcard["LEVEL"].ToString(),
                                FlashcardCount = Convert.ToInt32(flashcard["FLASHCARD_COUNT"])  
                            });
                        }

                        flashcardRepeater.DataSource = flashcards;
                        flashcardRepeater.DataBind();

                        con.Close();

                        if(flashcards.Count < 1)
                        {
                            errMsg.Visible = true;
                            errMsg.ForeColor = System.Drawing.Color.Red;
                            errMsg.Text = "There are no quizzes at the moment! Come back later.";
                        }
                    }
                }
            }
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();

            var filteredFlashcards = flashcards.Where(f => f.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            flashcardRepeater.DataSource = filteredFlashcards;
            flashcardRepeater.DataBind();
        }
    }
}