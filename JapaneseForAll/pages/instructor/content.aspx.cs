using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

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
                            Q.QUIZ_ID AS ModuleId,
                            Q.TITLE AS Title,
                            CASE
                                WHEN EXISTS (
                                    SELECT 1
                                    FROM FLASHCARDTABLE F
                                    WHERE F.QUIZ_ID = Q.QUIZ_ID
                                )
                                THEN 'Flashcards'
                                ELSE 'Quiz'
                            END AS Type,
                            Q.LEVEL AS Level,
                            Q.STATUS AS Status
                        FROM QUIZTABLE Q
                        ORDER BY Q.TITLE;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        da.Fill(dt);
                        foreach(DataRow row in dt.Rows)
                        {
                            if (row["Status"].ToString().Trim() == "DELETE")
                            {
                                modules.Add(new Module
                                {
                                    Title = row["Title"].ToString().Trim(),
                                    Type = row["Type"].ToString().Trim(),
                                    Level = row["Level"].ToString().Trim(),
                                    Status = row["Status"].ToString().Trim(),
                                    ModuleId = row["ModuleId"].ToString().Trim()
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
    }
}