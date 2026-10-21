<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="JapaneseForAll.pages.publics.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover"/>
    <title>Register · Japanese For All</title>
    <link rel="stylesheet" href="../../css/base.css"/>
    <link rel="stylesheet" href="../../css/layout.css"/>
    <link rel="stylesheet" href="../../css/components.css"/>
    <link rel="stylesheet" href="../../css/pages.css"/>
</head>
<body>
    <header>
        <a class="logo" href='<%= ResolveClientUrl("~/index.aspx") %>'><b>JAPANESE</b> FOR ALL</a>
        <nav></nav>
        <div class="rt"><a class="lk" href="login.aspx">Login</a><a class="btn sm" href="register.aspx">Register</a></div>
    </header>
    <main>
        <div class="panel narrow">
            <a href='<%= ResolveClientUrl("~/index.aspx") %>'>← Back to Home</a>
            <h2>Create your account</h2>
            
            <!-- Start of the form for registration -->
            <form class="reg" id="form1" runat="server">

                <asp:ValidationSummary 
                    ID="ValidationSummary1"
                    runat="server"
                    HeaderText="Please correct the following errors:"
                    CssClass="validation-summary"
                    ForeColor="Red"
                    DisplayMode="BulletList"
                    ValidationGroup="Register"
                    />
                <asp:Label runat="server" ID="errMsg" CssClass="validation-summary" ForeColor="Red" Visible="false"></asp:Label>

                <p class="mut">Join as a Member to start learning, or apply as an Instructor.
                        </p>
                    <!-- The certificate field below appears only while "Instructor" is selected (CSS :has) -->
                        <div class="chips">
                            <label class="chip">
                                <asp:RadioButton ID="roleMember" runat="server" GroupName="role" Text="" Checked="true" ClientIDMode="Static"/><span>Member</span>
                            </label>
                            <label class="chip">
                                <asp:RadioButton ID ="roleInstructor" runat="server" GroupName="role" Text="" ClientIDMode="Static"/><span>Instructor</span>
                            </label>
                        </div>

                        <div class="row">
                            <div class="col"><label>First Name<asp:TextBox runat="server" ID="fname" placeholder="Enter your first name"></asp:TextBox></label></div>
                            <asp:RequiredFieldValidator ID="fnameValidator" runat="server" ControlToValidate="fname" ErrorMessage="First name is required." Text="*" ForeColor="Red" ValidationGroup="Register"/>
                            <div class="col"><label>Middle Name (optional)<asp:TextBox runat="server" ID="mname" placeholder="Enter your middle name (optional)"></asp:TextBox></label></div>
                            <div class="col"><label>Last Name<asp:TextBox runat="server" ID="lname" placeholder="Enter your last name"></asp:TextBox></label></div>
                            <asp:RequiredFieldValidator ID="lnameValidator" runat="server" ControlToValidate="lname" ErrorMessage="Last name is required." Text="*" ForeColor="Red" ValidationGroup="Register"/>
                        </div>

                        <label>Username<asp:TextBox runat="server" ID="username" placeholder="Enter your username"></asp:TextBox></label>
                        <asp:RequiredFieldValidator ID="usernameValidator" runat="server" ControlToValidate="username" ErrorMessage="Username is required." Text="*" ForeColor="Red" ValidationGroup="Register"/>
                        <label>Email<asp:TextBox runat="server" ID="email" TextMode="Email" placeholder="abc@gmail.com"></asp:TextBox></label>
                        <asp:RequiredFieldValidator ID="emailValidator" runat="server" ControlToValidate="email" ErrorMessage="Email is required." Text="*" ForeColor="Red" ValidationGroup="Register" />

                        <div class="row">
                            <div class="col"><label>Password<asp:TextBox runat="server" ID="password" TextMode="Password"></asp:TextBox></label></div>
                            <div class="col"><label>Confirm Password<asp:TextBox runat="server" ID="confirm_password" TextMode="Password"></asp:TextBox></label></div>
                            <asp:RequiredFieldValidator ID="confirmPaswordRequired" runat="server" ControlToValidate="confirm_password" ErrorMessage="Please confirm your password." Text="*" ForeColor="Red" ValidationGroup="Register" />
                            <asp:CompareValidator ID="password_compare" runat="server" ControlToValidate="confirm_password" ControlToCompare="password" Operator="Equal" Type="String" ErrorMessage="Passwords do not match." Text="*" ForeColor="Red" ValidationGroup="Register" />
                        </div>

                        <label>Gender</label>

                        <div class="chips">
                        <label class="chip"><asp:RadioButton runat="server" ID="genderMale" GroupName="gender" Text="" Checked="true"/><span>Male</span></label>
                        <label class="chip"><asp:RadioButton runat="server" ID="genderFemale" GroupName="gender" Text="" /><span>Female</span></label>
                        <label class="chip"><asp:RadioButton runat="server" ID="genderOther" GroupName="gender" Text="" /><span>Other</span></label>
                        </div>

                        <div class="cert">
                            <label>Teaching Certificate (Instructors only)<asp:FileUpload runat="server" ID="cert" Enabled="false" accept=".pdf" ClientIDMode="Static"/></label>
                            <small class="mut">⬆ Upload PDF (max 5MB)</small>
                            <asp:CustomValidator ID="certValidator" runat="server" ErrorMessage="Instructors must submit a teaching certificate." Text="*" ForeColor="Red" ValidationGroup="Register" OnServerValidate="certValidator_ServerValidate" />
                        </div>
                        <div class="row"><asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="btn" Style="width:100%" ValidationGroup="Register" OnClick="btnRegister_Click"/></div>
                </form>
            <p class="c mut">Already have an account? <a href="login.aspx">Log in</a></p>
        </div>
    </main>
    <script>
const roleMemberId = '<%= roleMember.ClientID %>';
const roleInstructorId = '<%= roleInstructor.ClientID %>';
const certificateId = '<%= cert.ClientID %>';
    </script>
    <script src="../../js/register.js"></script>
</body>
</html>

