using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Runtime.InteropServices;

namespace JapaneseForAll.pages.member
{
    public partial class flashcards : System.Web.UI.Page
    {

        public class Flashcard
        {
            public int CardId {  get; set; }
            public string FrontText { get; set; }
            public string FrontRomaji { get; set; }
            public string BackText { get; set; }
            public string BackNote { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int quizId = Convert.ToInt32(Request.QueryString["id"]);

            int cardNumber = 1;

            string quiz_title = "";

            if (!int.TryParse(Request.QueryString["card"], out cardNumber) || cardNumber < 1)
            {
                cardNumber = 1;
            }

            List<Flashcard> flashcards = new List<Flashcard>();

            string query = @"
                SELECT CARD_ID, FRONT_TEXT, FRONT_ROMAJI, BACK_TEXT, BACK_NOTE
                FROM FLASHCARDTABLE
                WHERE QUIZ_ID = @quizId
                ORDER BY CARD_ORDER";
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@quizId", quizId);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    flashcards.Add(new Flashcard
                    {
                        CardId = Convert.ToInt32(dr["CARD_ID"]),
                        FrontText = dr["FRONT_TEXT"].ToString(),
                        FrontRomaji = dr["FRONT_ROMAJI"].ToString(),
                        BackText = dr["BACK_TEXT"].ToString(),
                        BackNote = dr["BACK_NOTE"].ToString()
                    });
                }

                dr.Close();

                query = "SELECT TITLE FROM QUIZTABLE WHERE QUIZ_ID = @quizId";
                cmd = new SqlCommand(query, con);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    quiz_title = dr["TITLE"].ToString();
                }
                dr.Close();

                SqlCommand cmd1 = new SqlCommand();


                con.Close();


            } catch (Exception ex) {
                System.Diagnostics.Trace.Write(ex.ToString());
            }

            if(flashcards.Count == 0)
            {
                lblSetTitle.Text = "No flashcards available";
                lblCardNumber.Text = "0";
                lblTotalCards.Text = "0";

                flashcardRepeater.Visible = false;
                lnkPrevious.Visible = false;
                lnkNext.Visible = false;

                return;
            }

            if(cardNumber > flashcards.Count)
            {
                cardNumber = flashcards.Count;
            }

            Flashcard currentCard = flashcards[cardNumber - 1];

            flashcardRepeater.DataSource = new List<Flashcard> { currentCard };
            flashcardRepeater.DataBind();

            lblSetTitle.Text = quiz_title;
            lblCardNumber.Text = cardNumber.ToString();
            lblTotalCards.Text = flashcards.Count.ToString();

            double progress = (double)cardNumber / flashcards.Count * 100;
            progressBar.Style["width"] = progress + "%";

            lnkPrevious.NavigateUrl = "flashcards.aspx?id=" + quizId + "&card=" + Math.Min(flashcards.Count, cardNumber - 1);

            lnkNext.NavigateUrl = "flashcards.aspx?id=" + quizId + "&card=" + Math.Min(flashcards.Count, cardNumber + 1);

            lnkPrevious.Enabled = cardNumber > 1;
            lnkNext.Enabled = cardNumber < flashcards.Count; 
        }
    }
}