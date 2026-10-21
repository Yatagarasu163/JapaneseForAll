<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="content.aspx.cs" Inherits="JapaneseForAll.pages.instructor.content" %>
<%@ Register Src="~/pages/components/InstructorNavbar.ascx" TagPrefix="uc" TagName="InstructorNavbar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover"/>
    <title>Dashboard · Japanese For All</title>
    <link rel="stylesheet" href="../../css/base.css"/>
    <link rel="stylesheet" href="../../css/layout.css"/>
    <link rel="stylesheet" href="../../css/components.css"/>
    <link rel="stylesheet" href="../../css/pages.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <uc:InstructorNavbar ID="Navbar1" runat="server" />

        <main>
            <div class="panel">
                <div class="row sp" style="margin:0">
                <div><h2>Learning Content Management</h2><p class="mut">Create and manage flashcards, quizzes and audio visual modules</p></div>
                <a class="btn" href="module-form.aspx">+ Create Content</a>
                </div>
                <div class="sc"><table>
                <tr><th>Module</th><th>Type</th><th>Level</th><th>Status</th><th>Actions</th></tr>
                <!-- REPEAT: one row per module -->
                <asp:Repeater ID="moduleRepeater" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%#: Eval("Title") %></td>
                            <td><%#: Eval("Type") %></td>
                            <td><span class='lvl <%#: Eval("Level") %>'> <%#: Eval("Level") %></span></td>
                            <td><span class='badge <%#: Eval("Status") %>'><%#: Eval("Status") %></span></td>
                            <td>
                                <a href='module-form.aspx?id=<%#: Eval("ModuleId") %>'>Edit</a>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="lk" CommandName="DeleteModule" CommandArgument='<%# Eval("ModuleId") %>' OnClientClick="return confirm('Are you sure you want to delete this module?');" OnCommand="DeleteModule_Command"/>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </table></div>
                <h3>Recent Quiz Submissions to Review</h3>
                <div class="sc"><table>
                <tr><th>Member</th><th>Quiz</th><th>Score</th><th>Status</th></tr>
                <!-- REPEAT: one row per submission -->
                <tr><td>Wong Jun Hao</td><td>N5 Basic Greetings</td><td>9 / 10</td><td><span class="badge Reviewed">Reviewed</span></td></tr>
                <tr><td>Narish Kumar Dey</td><td>N5 Numbers &amp; Counting</td><td>7 / 10</td><td><span class="badge Pending">Pending</span></td></tr>
                </table></div>
            </div>
        </main>
    </form>
</body>
</html>
