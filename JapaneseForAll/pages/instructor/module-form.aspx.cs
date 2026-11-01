using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace JapaneseForAll.pages.instructor
{
    public partial class module_form : System.Web.UI.Page
    {
        [Serializable]
        public class FlashCard
        {
            public string FrontText { get; set; } = "";
            public string FrontRomaji { get; set; } = "";
            public string BackText { get; set; } = "";
            public string BackNote { get; set; } = "";
            public string ImagePath { get; set; } = "";   // image already saved for this card (edit mode)
        }

        private const string UploadFolder = "~/uploads/flashcards/";
        private const int MaxImageBytes = 2 * 1024 * 1024;   // 2 MB per image
        private static readonly string[] AllowedImageTypes = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        protected string HeadingText { get; set; } = "Create Learning Module";

        private int EditingId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) && id > 0 ? id : 0;
            }
        }

        private List<FlashCard> Cards
        {
            get
            {
                if (ViewState["Cards"] == null)
                {
                    ViewState["Cards"] = new List<FlashCard>();
                }

                return (List<FlashCard>)ViewState["Cards"];
            }
            set
            {
                ViewState["Cards"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["fname"] == null)
            {
                Response.Redirect("~/pages/publics/login.aspx");
                return;
            }

            if (EditingId > 0)
                HeadingText = "Edit Learning Module";

            if (!IsPostBack)
            {
                if (EditingId > 0)
                {
                    if (!LoadFlashcard(EditingId))
                    {
                        Response.Redirect("content.aspx");
                        return;
                    }
                }
                else
                {
                    Cards = new List<FlashCard> { new FlashCard() };
                }

                BindCards();
            }
        }

        private bool LoadFlashcard(int flashcardId)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            List<FlashCard> cards = new List<FlashCard>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT TITLE, LEVEL, STATUS FROM FLASHCARDTABLE WHERE FLASHCARD_ID = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", flashcardId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return false;

                        txtTitle.Text = Convert.ToString(reader["TITLE"]);
                        SelectIgnoreCase(rblLevel, Convert.ToString(reader["LEVEL"]));
                        SelectIgnoreCase(rblStatus, Convert.ToString(reader["STATUS"]));
                    }
                }

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT FRONT_TEXT, FRONT_ROMAJI, BACK_TEXT, BACK_NOTE, [IMAGE]
                    FROM CARDTABLE
                    WHERE FLASHCARD_ID = @Id
                    ORDER BY CARD_ID", con))
                {
                    cmd.Parameters.AddWithValue("@Id", flashcardId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cards.Add(new FlashCard
                            {
                                FrontText = Convert.ToString(reader["FRONT_TEXT"]),
                                FrontRomaji = Convert.ToString(reader["FRONT_ROMAJI"]),
                                BackText = Convert.ToString(reader["BACK_TEXT"]),
                                BackNote = Convert.ToString(reader["BACK_NOTE"]),
                                ImagePath = Convert.ToString(reader["IMAGE"])
                            });
                        }
                    }
                }
            }

            if (cards.Count == 0)
                cards.Add(new FlashCard());

            Cards = cards;
            return true;
        }

        private void SelectIgnoreCase(ListControl list, string value)
        {
            list.ClearSelection();

            foreach (ListItem item in list.Items)
            {
                if (string.Equals(item.Value, value, StringComparison.OrdinalIgnoreCase))
                {
                    item.Selected = true;
                    return;
                }
            }
        }

        private void BindCards()
        {
            rptCards.DataSource = Cards;
            rptCards.DataBind();
        }

        private void SaveCurrentCards()
        {
            List<FlashCard> updated = new List<FlashCard>();
            List<string> dummySavedFiles = new List<string>();

            foreach (RepeaterItem item in rptCards.Items)
            {
                TextBox txtFrontText = (TextBox)item.FindControl("txtFrontText");
                TextBox txtFrontRomaji = (TextBox)item.FindControl("txtFrontRomaji");
                TextBox txtBackText = (TextBox)item.FindControl("txtBackText");
                TextBox txtBackNote = (TextBox)item.FindControl("txtBackNote");
                HtmlInputFile upload = (HtmlInputFile)item.FindControl("fileImage");
                HiddenField hfExistingImage = (HiddenField)item.FindControl("hfExistingImage");

                string imagePath = string.Empty;

                if (hfExistingImage != null)
                {
                    imagePath = hfExistingImage.Value;
                }

                // FIX: Immediately save the file to disk during postback so UpdatePanel doesn't wipe it out!
                if (upload != null && upload.PostedFile != null && upload.PostedFile.ContentLength > 0)
                {
                    string newPath = SaveImage(upload.PostedFile, dummySavedFiles);
                    if (!string.IsNullOrEmpty(newPath))
                    {
                        imagePath = newPath;
                    }
                }

                updated.Add(new FlashCard
                {
                    FrontText = txtFrontText != null ? txtFrontText.Text : "",
                    FrontRomaji = txtFrontRomaji != null ? txtFrontRomaji.Text : "",
                    BackText = txtBackText != null ? txtBackText.Text : "",
                    BackNote = txtBackNote != null ? txtBackNote.Text : "",
                    ImagePath = imagePath
                });
            }

            Cards = updated;
        }

        private List<HttpPostedFile> GetPostedImages()
        {
            List<HttpPostedFile> files = new List<HttpPostedFile>();

            foreach (RepeaterItem item in rptCards.Items)
            {
                HtmlInputFile upload = (HtmlInputFile)item.FindControl("fileImage");
                files.Add(upload != null ? upload.PostedFile : null);
            }

            return files;
        }

        protected void btnAddCard_Click(object sender, EventArgs e)
        {
            SaveCurrentCards();

            Cards.Add(new FlashCard());

            BindCards();
            upCards.Update();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            SaveCurrentCards();

            LinkButton btn = (LinkButton)sender;
            int index = Convert.ToInt32(btn.CommandArgument);

            if (Cards.Count > 1 && index >= 0 && index < Cards.Count)
            {
                Cards.RemoveAt(index);
            }

            BindCards();
            upCards.Update();
        }

        private string ValidateForm(List<HttpPostedFile> images)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
                return "Please enter a module title.";

            for (int i = 0; i < Cards.Count; i++)
            {
                FlashCard card = Cards[i];

                if (string.IsNullOrWhiteSpace(card.FrontText) ||
                    string.IsNullOrWhiteSpace(card.BackText))
                {
                    return "Card " + (i + 1) + " needs Front Text and Back Text.";
                }

                HttpPostedFile file = images[i];

                if (file != null && file.ContentLength > 0)
                {
                    string ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                    if (Array.IndexOf(AllowedImageTypes, ext) < 0)
                        return "Card " + (i + 1) + ": the image must be a JPG, PNG, GIF or WebP file.";

                    if (file.ContentLength > MaxImageBytes)
                        return "Card " + (i + 1) + ": the image must be smaller than 2 MB.";
                }
            }

            return null;
        }

        private string SaveImage(HttpPostedFile file, List<string> savedFiles)
        {
            if (file == null || file.ContentLength == 0)
                return null;

            string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            string fileName = Guid.NewGuid().ToString("N") + ext;

            string folder = Server.MapPath(UploadFolder);
            Directory.CreateDirectory(folder);

            string physicalPath = Path.Combine(folder, fileName);
            file.SaveAs(physicalPath);
            savedFiles.Add(physicalPath);

            return UploadFolder + fileName;
        }

        private void DeleteStoredImage(string virtualPath)
        {
            try
            {
                if (string.IsNullOrEmpty(virtualPath) ||
                    !virtualPath.StartsWith(UploadFolder, StringComparison.OrdinalIgnoreCase))
                    return;

                string root = Path.GetFullPath(Server.MapPath(UploadFolder));
                string full = Path.GetFullPath(Server.MapPath(virtualPath));

                if (full.StartsWith(root, StringComparison.OrdinalIgnoreCase) && File.Exists(full))
                    File.Delete(full);
            }
            catch
            {
            }
        }

        protected void btnSaveFlashcard_Click(object sender, EventArgs e)
        {
            SaveCurrentCards();
            List<HttpPostedFile> images = GetPostedImages();

            string error = ValidateForm(images);
            if (error != null)
            {
                lblMessage.Text = error;
                return;
            }

            string connectionString =
                ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString;

            int flashcardId = EditingId;
            bool isEditing = flashcardId > 0;

            string insertFlashcardQuery = @"
        INSERT INTO FLASHCARDTABLE (TITLE, DESCRIPTION, LEVEL, FLASHCARD_COUNT, STATUS)
        VALUES (@Title, @Description, @Level, @FlashcardCount, @Status);

        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            string updateFlashcardQuery = @"
        UPDATE FLASHCARDTABLE
        SET TITLE = @Title,
            LEVEL = @Level,
            FLASHCARD_COUNT = @FlashcardCount,
            STATUS = @Status
        WHERE FLASHCARD_ID = @FlashcardId;";

            string selectOldImagesQuery = @"
        SELECT [IMAGE] FROM CARDTABLE
        WHERE FLASHCARD_ID = @FlashcardId AND [IMAGE] IS NOT NULL;";

            string deleteCardsQuery = @"
        DELETE FROM CARDTABLE WHERE FLASHCARD_ID = @FlashcardId;";

            string insertCardQuery = @"
        INSERT INTO CARDTABLE
            (FLASHCARD_ID, FRONT_TEXT, FRONT_ROMAJI, BACK_TEXT, BACK_NOTE, [IMAGE])
        VALUES
            (@FlashcardId, @FrontText, @FrontRomaji, @BackText, @BackNote, @Image);";

            List<string> savedFiles = new List<string>();
            List<string> oldImages = new List<string>();
            List<string> keptImages = new List<string>();
            bool saved = false;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        if (isEditing)
                        {
                            using (SqlCommand cmd = new SqlCommand(
                                updateFlashcardQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                                cmd.Parameters.AddWithValue("@Level", rblLevel.SelectedValue);
                                cmd.Parameters.AddWithValue("@FlashcardCount", Cards.Count);
                                cmd.Parameters.AddWithValue("@Status", rblStatus.SelectedValue);
                                cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);

                                if (cmd.ExecuteNonQuery() == 0)
                                    throw new Exception("The flashcard set could not be found.");
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                selectOldImagesQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                        oldImages.Add(Convert.ToString(reader[0]));
                                }
                            }

                            using (SqlCommand cmd = new SqlCommand(
                                deleteCardsQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand(
                                insertFlashcardQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                                cmd.Parameters.AddWithValue("@Description", "");
                                cmd.Parameters.AddWithValue("@Level", rblLevel.SelectedValue);
                                cmd.Parameters.AddWithValue("@FlashcardCount", Cards.Count);
                                cmd.Parameters.AddWithValue("@Status", rblStatus.SelectedValue);

                                flashcardId = (int)cmd.ExecuteScalar();
                            }
                        }

                        for (int i = 0; i < Cards.Count; i++)
                        {
                            FlashCard card = Cards[i];
                            string imagePath = string.IsNullOrEmpty(card.ImagePath) ? null : card.ImagePath;

                            if (imagePath != null)
                                keptImages.Add(imagePath);

                            using (SqlCommand cmd = new SqlCommand(
                                insertCardQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);
                                cmd.Parameters.AddWithValue("@FrontText", card.FrontText.Trim());
                                cmd.Parameters.AddWithValue("@FrontRomaji", card.FrontRomaji.Trim());
                                cmd.Parameters.AddWithValue("@BackText", card.BackText.Trim());
                                cmd.Parameters.AddWithValue("@BackNote", card.BackNote.Trim());
                                cmd.Parameters.AddWithValue("@Image", (object)imagePath ?? DBNull.Value);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        saved = true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Flashcard save failed: " + ex.ToString());

                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }

                        foreach (string path in savedFiles)
                        {
                            try { File.Delete(path); } catch { }
                        }

                        lblMessage.Text = "Save failed: " + ex.GetType().Name + " - " + ex.Message;
                    }
                }
            }

            if (saved)
            {
                foreach (string oldImage in oldImages)
                {
                    if (!keptImages.Contains(oldImage))
                        DeleteStoredImage(oldImage);
                }

                Response.Redirect("content.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}