using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.instructor
{
    public partial class new_topic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/pages/member/forum.aspx");
        }

        protected void OnSubmit_Click(object sender, EventArgs e)
        {

            string category = "";

            if (rbGrammar.Checked) { category = "Grammar"; }
            else if (rbVocabulary.Checked) { category = "Vocabulary"; }
            else if (rbStudy.Checked) { category = "Study Tips"; }
            else if (rbSpeaking.Checked) { category = "Speaking"; }
            else if (rbGeneral.Checked) { category = "General"; }
            else { return; }



            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
            {
                string query = "INSERT INTO THREADTABLE(USER_ID, TITLE, STATUS, CATEGORY, DESCRIPTION) VALUES(@uid, @title, 'ACTIVE', @category, @description);";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@uid", Session["uid"]);
                    cmd.Parameters.AddWithValue("@title", title.Text);
                    cmd.Parameters.AddWithValue("@description", msg.Text);
                    cmd.Parameters.AddWithValue("@category", category);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            Response.Redirect("~/pages/instructor/forum.aspx");
        }


    }
}