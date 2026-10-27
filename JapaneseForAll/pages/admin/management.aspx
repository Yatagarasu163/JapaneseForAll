<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="management.aspx.cs" Inherits="JapaneseForAll.pages.admin.WebForm1" %>
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

        <!-- Uses your project's shared Navbar component -->
        <uc:AdminNavbar ID="AdminNavbar1" runat="server" />

        <main>
            <div class="panel">
              <div class="row sp" style="margin:0">
                <div><h2>User Management</h2><p class="mut">Create, view, update and delete user accounts</p></div>
                <a class="btn" href="../publics/register.aspx">+ Create User Account</a>
              </div>
              <div class="grid3">
                <div class="card stat"><span class="mut">Total Users</span><b runat="server" id="TotalUsers"></b><span class="mut sm">Members, Instructors, Admins</span></div>
                <div class="card stat"><span class="mut">Pending Verifications</span><b runat="server" id="Status"></b><span class="mut sm">Instructor certificates</span></div>
                <div class="card stat"><span class="mut">Active This Week</span><b runat="server" id="Active"></b><span class="mut sm" runat="server" id="ActivePercentage"> of total users</span></div>
              </div>
              <div class="sc">
    <table>
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Role</th>
            <th>Status</th>
            <th>Actions</th>
        </tr>
        
        <!-- Repeater -->
        <asp:Repeater ID="RepeatUsers" runat="server" OnItemCommand="BtnDelete">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("USERNAME") %></td>
                    <td><%# Eval("EMAIL") %></td>
                    <td><%# Eval("ROLE") %></td>
                    <td><span class="badge <%# Eval("STATUS") %>"><%# Eval("STATUS") %></span></td>
                    <td>
                        <a href='EditUser.aspx?id=<%# Eval("USER_ID") %>'>Edit</a> 

                        <asp:LinkButton runat="server" ID="BtnDelete" CssClass="lk" 
                            CommandName="DeleteUser" 
                            CommandArgument='<%# Eval("USER_ID") %>' 
                            OnClientClick="return confirm('Are you sure you want to delete this user?');">Delete</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>
            </div>
        </main>
    </form>
</body>
</html>