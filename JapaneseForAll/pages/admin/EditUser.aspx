<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="JapaneseForAll.pages.admin.WebForm2" %>
<%@ Register Src="~/pages/components/AdminNavbar.ascx" TagPrefix="uc" TagName="AdminNavbar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="utf-8"/>
     <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover"/>
     <title>User Management · Japanese For All</title>
     <link rel="stylesheet" href="../../css/base.css"/>
     <link rel="stylesheet" href="../../css/layout.css"/>
     <link rel="stylesheet" href="../../css/components.css"/>
     <link rel="stylesheet" href="../../css/pages.css"/>
</head>
<body>
    <form id="form1" runat="server">

        <uc:AdminNavbar ID="AdminNavbar1" runat="server"/>

        <main>
          <div class="panel narrow">
              <a href="management.aspx">← Back to Users</a>
              <h2>Edit User Account</h2>
              <p class="mut">Update account details. Changes take effect immediately.</p>
             
              <div class="row">
                  <div class="col"><label>First Name<input type="text" runat="server" id="txtFirst" name="first"/></label></div>
                  <div class="col"><label>Last Name<input type="text" runat="server" id="txtLast" name="last"/></label></div>
              </div>
              
              <label>Email<input type="email" runat="server" id="txtEmail" name="email" value="wong.jh@example.com"/></label>
              
              <label>Role</label>
              <div class="chips">
                  <label class="chip"><input type="radio" runat="server" id="radioMember" name="role" value="Member" checked="true"/><span>Member</span></label>
                  <label class="chip"><input type="radio" runat="server" id="radioInstructor" name="role" value="Instructor"/><span>Instructor</span></label>
                  <label class="chip"><input type="radio" runat="server" id="radioAdmin" name="role" value="Admin"/><span>Admin</span></label>
              </div>
              
              <div class="row">
                  <a class="btn ghost" href="management.aspx">Cancel</a>
                  <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save Changes" OnClick="btnSave_Click" />
              </div>

          </div> 
       </main>
    </form>
</body>
</html>