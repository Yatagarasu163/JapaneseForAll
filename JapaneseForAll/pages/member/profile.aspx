<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="JapaneseForAll.pages.member.profile" %>
<%@ Register Src="~/pages/components/Navbar.ascx" TagPrefix="uc" TagName="Navbar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
            <meta charset="utf-8"/>
            <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover"/>
            <title>Quizzes · Japanese For All</title>
            <link rel="stylesheet" href="../../css/base.css"/>
            <link rel="stylesheet" href="../../css/layout.css"/>
            <link rel="stylesheet" href="../../css/components.css"/>
            <link rel="stylesheet" href="../../css/pages.css"/>
    </head>
<body>
    <form id="form1" runat="server">

        <uc:Navbar ID="Navbar1" runat="server" />

        <main>
            <div class="panel narrow">
              <h2>Profile &amp; Settings</h2>
              <h3>Profile Details</h3>
                <div class="row">
                  <div class="col"><label>First Name<asp:TextBox ID="fname" runat="server" /></label></div>
                  <div class="col"><label>Middle Name<asp:TextBox ID="mname" runat="server" /></label></div>
                  <div class="col"><label>Last Name<asp:TextBox ID="lname" runat="server"/></label></div>
                </div>
                <label>Email<asp:TextBox ID="email" runat="server" TextMode="Email" /></label>
                <div class="row"> 
                <asp:Button ID="submit" runat="server" OnClick="OnSubmit_Click" CssClass="btn" Text="Save Changes" />
                </div>
              <h3>Change Password</h3>
                
                <label>Current Password<asp:TextBox ID="password" runat="server" TextMode="Password" /></label>
                <label>New Password<asp:TextBox ID="newpassword" runat="server" TextMode="Password" /></label>
                <label>Confirm New Password<asp:TextBox ID="confirmpassword" runat="server" TextMode="Password" /></label>
                <asp:Label ID="errMsg" runat="server" Visible="false"></asp:Label>
                <div class="row"><asp:Button ID="update" runat="server" OnClick="OnUpdate_Click" CssClass="btn" Text="Update Password" /></div>
            </div>
        </main>
    </form>
</body>
</html>
