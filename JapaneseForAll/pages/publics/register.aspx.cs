using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace JapaneseForAll.pages.publics
{

    public partial class WebForm1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //Registration Process here.

                try
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["JPDB"].ConnectionString);
                    con.Open();

                    string query = "SELECT COUNT(*) FROM USERTABLE WHERE USERNAME = @username AND STATUS = 'ACTIVE'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username.Text);
                    int check = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                    if(check > 0)
                    {
                        errMsg.Visible = true;
                        errMsg.ForeColor = System.Drawing.Color.Red;
                        errMsg.Text = "Username has been taken!";
                    }
                    else
                    {

                        string gender = "";

                        if (genderMale.Checked)
                        {
                            gender = "Male";
                        } else if (genderFemale.Checked)
                        {
                            gender = "Female";
                        }
                        else
                        {
                            gender = "Other";
                        }

                        string photoPath = SaveProfilePhoto();

                        if (roleMember.Checked)
                        {

                            string profilePicFolderPath = Server.MapPath("~/photo/");

                            System.Diagnostics.Debug.WriteLine("Photo folder: " + profilePicFolderPath);

                            if (!Directory.Exists(profilePicFolderPath))
                            {
                                Directory.CreateDirectory(profilePicFolderPath);
                            }

                            string profilePicPath = "";

                            if (photo.HasFile)
                            {
                                string photoName = Path.GetFileName(photo.FileName);
                                string fullPath = Path.Combine(profilePicFolderPath, photoName);

                                System.Diagnostics.Debug.WriteLine("Uploaded filename: " + photoName);
                                System.Diagnostics.Debug.WriteLine("Full save path: " + fullPath);

                                photo.SaveAs(fullPath);

                                profilePicPath = "~/photo/" + photoName;

                                //certPath = "~/Certificates/" + cert.FileName.ToString();
                                //cert.SaveAs(folderPath + Path.GetFileName(cert.FileName));
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("No photo file was uploaded.");
                            }

                            //Create a row for a member 
                            string query1 = "INSERT INTO USERTABLE (FNAME, MNAME, LNAME, USERNAME, EMAIL, PASSWORD, GENDER, ROLE, PROFILE_PHOTO) VALUES (@fname, @mname, @lname, @username, @email, @password, @gender, @role, @photo) ";

                            SqlCommand cmd2 = new SqlCommand(query1, con);
                            cmd2.Parameters.AddWithValue("@fname", fname.Text);
                            cmd2.Parameters.AddWithValue("@mname", string.IsNullOrWhiteSpace(mname.Text) ? (object)DBNull.Value: mname.Text.Trim());
                            cmd2.Parameters.AddWithValue("@lname", lname.Text);
                            cmd2.Parameters.AddWithValue("@username", username.Text);
                            cmd2.Parameters.AddWithValue("@email", email.Text);
                            cmd2.Parameters.AddWithValue("@password", password.Text);
                            cmd2.Parameters.AddWithValue("@gender", gender);
                            cmd2.Parameters.AddWithValue("@role", "Member");
                            cmd2.Parameters.AddWithValue("@photo", (object)photoPath ?? DBNull.Value);
                            cmd2.ExecuteNonQuery();
                            Response.Redirect("login.aspx");
                        }
                        else
                        {
                            string folderPath = Server.MapPath("~/Certificates/");

                            System.Diagnostics.Debug.WriteLine("Certificate folder: " + folderPath);

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            string certPath = "";

                            if (cert.HasFile)
                            {
                                string fileName = Path.GetFileName(cert.FileName);
                                string fullPath = Path.Combine(folderPath, fileName);

                                System.Diagnostics.Debug.WriteLine("Uploaded filename: " + fileName);
                                System.Diagnostics.Debug.WriteLine("Full save path: " + fullPath);

                                cert.SaveAs(fullPath);

                                certPath = "~/Certificates/" + fileName;

                                //certPath = "~/Certificates/" + cert.FileName.ToString();
                                //cert.SaveAs(folderPath + Path.GetFileName(cert.FileName));
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("No certificate file was uploaded.");
                            }


                            string profilePicFolderPath = Server.MapPath("~/photo/");

                            System.Diagnostics.Debug.WriteLine("Photo folder: " + profilePicFolderPath);

                            if (!Directory.Exists(profilePicFolderPath))
                            {
                                Directory.CreateDirectory(profilePicFolderPath);
                            }

                            string profilePicPath = "";

                            if (photo.HasFile)
                            {
                                string photoName = Path.GetFileName(photo.FileName);
                                string fullPath = Path.Combine(profilePicFolderPath, photoName);

                                System.Diagnostics.Debug.WriteLine("Uploaded filename: " + photoName);
                                System.Diagnostics.Debug.WriteLine("Full save path: " + fullPath);

                                photo.SaveAs(fullPath);

                                profilePicPath = "~/photo/" + photoName;

                                //certPath = "~/Certificates/" + cert.FileName.ToString();
                                //cert.SaveAs(folderPath + Path.GetFileName(cert.FileName));
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("No photo file was uploaded.");
                            }







                            //Create a row for an instructor
                            string query1 = "INSERT INTO USERTABLE(FNAME, MNAME, LNAME, USERNAME, EMAIL, PASSWORD, GENDER, ROLE, CERTIFICATE, STATUS, PROFILE_PHOTO) VALUES (@fname, @mname, @lname, @username, @email, @password, @gender, @role, @cert, 'PENDING', @photo) ";
                            SqlCommand cmd1 = new SqlCommand(query1, con);
                            cmd1.Parameters.AddWithValue("@fname", fname.Text);
                            cmd1.Parameters.AddWithValue("@mname", string.IsNullOrWhiteSpace(mname.Text) ? (object)DBNull.Value: mname.Text.Trim());
                            cmd1.Parameters.AddWithValue("@lname", lname.Text);
                            cmd1.Parameters.AddWithValue("@username", username.Text);
                            cmd1.Parameters.AddWithValue("@email", email.Text);
                            cmd1.Parameters.AddWithValue("@password", password.Text);
                            cmd1.Parameters.AddWithValue("@gender", gender);
                            cmd1.Parameters.AddWithValue("@role", "Instructor");
                            cmd1.Parameters.AddWithValue("@cert", certPath);
                            cmd1.Parameters.AddWithValue("@photo", (object)profilePicPath ?? DBNull.Value);
                            cmd1.ExecuteNonQuery();
                            Response.Redirect("login.aspx");
                        }
                        con.Close();
                    }
                } catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());

                    errMsg.Visible = true;
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    errMsg.Text = "Registration not successful!" + ex.ToString();
                }
            }
        }

    protected void certValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (roleInstructor.Checked)
            {
                args.IsValid = cert.HasFile;
            }
            else
            {
                args.IsValid = true;
            }
        }


        private const string PhotoFolder = "~/uploads/profiles/";
        private const int MaxPhotoBytes = 50 * 1024 * 1024;   // 2 MB

        // Returns an error message, or null if there is no photo / the photo is fine.
        private string ValidatePhoto()
        {
            if (!photo.HasFile)
                return null;                                   // the photo is optional

            string ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (ext != ".jpg" && ext != ".jpeg" &&  ext != ".png")
                return "The profile photo must be a JPG or JPEG or PNG file.";

            if (photo.PostedFile.ContentLength > MaxPhotoBytes)
                return "The profile photo must be smaller than 50 MB.";

            // a real JPEG always starts with the bytes FF D8 FF (this catches renamed files)
            if (ext == ".jpeg")
            {
                byte[] header = new byte[3];
                int read = photo.PostedFile.InputStream.Read(header, 0, 3);
                photo.PostedFile.InputStream.Position = 0;
                if (read < 3 || header[0] != 0xFF || header[1] != 0xD8 || header[2] != 0xFF)
                    return "That file is not a valid JPEG image.";
            }
                return null;
        }

        protected void photoValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string error = ValidatePhoto();
            args.IsValid = error == null;
            if (error != null)
                photoValidator.ErrorMessage = error;
        }

        // Saves the photo under a random name. Returns "~/uploads/profiles/xxxx.jpg", or null if none was chosen.
        private string SaveProfilePhoto()
        {
            if (!photo.HasFile)
                return null;

            string folder = Server.MapPath(PhotoFolder);
            Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid().ToString("N") + ".jpg";
            photo.SaveAs(Path.Combine(folder, fileName));

            return PhotoFolder + fileName;
        }
    }
}