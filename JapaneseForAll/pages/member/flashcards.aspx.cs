 using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;

namespace JapaneseForAll.pages.member
{
    public partial class flashcards : System.Web.UI.Page
    {
        public class Flashcard
        {
            public int CardId { get; set; }
            public string FrontText { get; set; }
            public string FrontRomaji { get; set; }
            public string BackText { get; set; }
            public string BackNote { get; set; }
            public string ImagePath { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // ?id=7  -> which flashcard set;  ?card=3 -> which card inside it (starts at 1)
            int flashcardId;
            if (!int.TryParse(Request.QueryString["id"], out flashcardId) || flashcardId < 1)
            {
                Response.Redirect("flashcard-list.aspx");
                return;
            }

            int cardNumber;
            if (!int.TryParse(Request.QueryString["card"], out cardNumber) || cardNumber < 1)
            {
                cardNumber = 1;
            }

            List<Flashcard> cards = new List<Flashcard>();
            string setTitle = "";

            try
            {
                using (SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {
                    con.Open();

                    // the set's title (FLASHCARDTABLE, not QUIZTABLE)
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT TITLE FROM FLASHCARDTABLE WHERE FLASHCARD_ID = @FlashcardId", con))
                    {
                        cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            setTitle = result.ToString();
                    }

                    // the cards live in CARDTABLE and are linked by FLASHCARD_ID
                    using (SqlCommand cmd = new SqlCommand(@"
                        SELECT CARD_ID, FRONT_TEXT, FRONT_ROMAJI, BACK_TEXT, BACK_NOTE, [IMAGE]
                        FROM CARDTABLE
                        WHERE FLASHCARD_ID = @FlashcardId
                        ORDER BY CARD_ID", con))
                    {
                        cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                cards.Add(new Flashcard
                                {
                                    CardId = Convert.ToInt32(dr["CARD_ID"]),
                                    FrontText = dr["FRONT_TEXT"].ToString(),
                                    FrontRomaji = dr["FRONT_ROMAJI"].ToString(),
                                    BackText = dr["BACK_TEXT"].ToString(),
                                    BackNote = dr["BACK_NOTE"].ToString(),
                                    ImagePath = dr["IMAGE"] != DBNull.Value ? dr["IMAGE"].ToString() : string.Empty
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // shows up in Visual Studio: View > Output > Show output from: Debug
                System.Diagnostics.Debug.WriteLine("Loading flashcards failed: " + ex.ToString());
            }

            if (cards.Count == 0)
            {
                lblSetTitle.Text = string.IsNullOrEmpty(setTitle) ? "No flashcards available" : setTitle;
                lblCardNumber.Text = "0";
                lblTotalCards.Text = "0";

                flashcardRepeater.Visible = false;
                lnkPrevious.Visible = false;
                lnkNext.Visible = false;

                return;
            }

            if (cardNumber > cards.Count)
            {
                cardNumber = cards.Count;
            }

            Flashcard currentCard = cards[cardNumber - 1];

            flashcardRepeater.DataSource = new List<Flashcard> { currentCard };
            flashcardRepeater.DataBind();

            lblSetTitle.Text = setTitle;
            lblCardNumber.Text = cardNumber.ToString();
            lblTotalCards.Text = cards.Count.ToString();

            // InvariantCulture keeps the decimal point a "." on every PC (CSS needs "33.3%", not "33,3%")
            double progress = (double)cardNumber / cards.Count * 100;
            progressBar.Style["width"] = progress.ToString("0.#", CultureInfo.InvariantCulture) + "%";

            lnkPrevious.NavigateUrl = "flashcards.aspx?id=" + flashcardId + "&card=" + Math.Max(1, cardNumber - 1);
            lnkPrevious.Enabled = cardNumber > 1;

            if (cardNumber == cards.Count)
            {
                // last card: the Next button becomes the Complete button
                lnkNext.Text = "Complete ✓";
                lnkNext.NavigateUrl = "flashcard-list.aspx";
            }
            else
            {
                lnkNext.Text = "Next Card →";
                lnkNext.NavigateUrl = "flashcards.aspx?id=" + flashcardId + "&card=" + (cardNumber + 1);
            }
        }
    }
}