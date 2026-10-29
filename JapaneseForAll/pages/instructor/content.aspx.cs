using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Ajax.Utilities;

namespace JapaneseForAll.pages.instructor
{
    public partial class content : System.Web.UI.Page
    {

        public class Module
        {
            public string Title { get; set; }
            public string Type { get; set; }
            public string Level { get; set; }
            public string Status { get; set; }
            public string ModuleId { get; set; }
        }

        List<Module> modules = new List<Module>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["fname"] == null) { Response.Redirect("~/pages/publics/login.aspx"); }
            if (!IsPostBack)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
                {

                    string query = @"
                        SELECT
                            F.FLASHCARD_ID AS ModuleId,
                            F.TITLE AS Title,
                            'Flashcards' AS Type,
                            F.LEVEL AS Level,
                            F.STATUS AS Status,
                            F.DESCRIPTION AS Description,
                            COUNT(C.CARD_ID) AS CardCount
                        FROM dbo.FLASHCARDTABLE F
                        LEFT JOIN dbo.CARDTABLE C
                            ON C.FLASHCARD_ID = F.FLASHCARD_ID
                        WHERE F.STATUS <> 'DELETED'
                        GROUP BY
                            F.FLASHCARD_ID,
                            F.TITLE,
                            F.LEVEL,
                            F.STATUS,
                            F.DESCRIPTION
                        ORDER BY F.TITLE;
                        ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        da.Fill(dt);
                        foreach(DataRow row in dt.Rows)
                        {
                            if (row["Status"].ToString().Trim() != "DELETED")
                            {
                                modules.Add(new Module
                                {
                                    Title = row["Title"].ToString().Trim(),
                                    Type = row["Type"].ToString().Trim(),
                                    Level = row["Level"].ToString().Trim(),
                                    Status = row["Status"].ToString().Trim(),
                                    ModuleId = row["ModuleId"].ToString().Trim(),
                                });
                            }
                        }

                        con.Close();

                        moduleRepeater.DataSource = modules;
                        moduleRepeater.DataBind();
                    }
                }
            }
        }
        
        protected void DeleteModule_Command(object sender, CommandEventArgs e)
        {
            int moduleId = Convert.ToInt32(e.CommandArgument);

            using(SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString))
            {
                string query = "UPDATE FLASHCARDTABLE SET STATUS = 'DELETED' WHERE FLASHCARD_ID = @moduleId";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@moduleId", moduleId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if(rowsAffected > 0)
                    {
                        errMsg.Visible = true;
                        errMsg.ForeColor = System.Drawing.Color.Green;
                        errMsg.Text = "Successfully deleted flashcard!";
                    }
                }
            }
        }
    }
}