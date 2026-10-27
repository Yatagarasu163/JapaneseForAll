using System;
using System.Collections.Generic;
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

        }
    }
}