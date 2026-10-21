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

                        if (roleMember.Checked)
                        {

                            //Create a row for a member 
                            string query1 = "INSERT INTO USERTABLE (FNAME, MNAME, LNAME, USERNAME, EMAIL, PASSWORD, GENDER, ROLE) VALUES (@fname, @mname, @lname, @username, @email, @password, @gender, @role) ";

                            SqlCommand cmd2 = new SqlCommand(query1, con);
                            cmd2.Parameters.AddWithValue("@fname", fname.Text);
                            cmd2.Parameters.AddWithValue("@mname", string.IsNullOrWhiteSpace(mname.Text) ? (object)DBNull.Value: mname.Text.Trim());
                            cmd2.Parameters.AddWithValue("@lname", lname.Text);
                            cmd2.Parameters.AddWithValue("@username", username.Text);
                            cmd2.Parameters.AddWithValue("@email", email.Text);
                            cmd2.Parameters.AddWithValue("@password", password.Text);
                            cmd2.Parameters.AddWithValue("@gender", gender);
                            cmd2.Parameters.AddWithValue("@role", "Member");
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
                            

                            //Create a row for an instructor
                            string query1 = "INSERT INTO USERTABLE(FNAME, MNAME, LNAME, USERNAME, EMAIL, PASSWORD, GENDER, ROLE, CERTIFICATE, STATUS) VALUES (@fname, @mname, @lname, @username, @email, @password, @gender, @role, @cert, 'PENDING') ";
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
}
}