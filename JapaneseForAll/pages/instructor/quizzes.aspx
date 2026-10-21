<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quizzes.aspx.cs" Inherits="JapaneseForAll.quizzes" %>
<%@ Register Src ="~/pages/components/InstructorNavbar.ascx" TagPrefix="uc" TagName="InstructorNavbar" %>

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
        <a class="btn" href="quiz-editor.html">+ Create Quiz</a>
      </div>
      <div class="sc"><table>
        <tr><th>Quiz Title</th><th>Quiz ID</th><th>Level</th><th>Status</th><th>Actions</th></tr>
        <!-- REPEAT: one row per quiz -->
          <asp:Repeater ID="quizRepeater" runat="server">

          </asp:Repeater>
          <tr>
              <td> <%#: Eval("Title") %> </td>
              <td> <%#: Eval("QuizID") %></td>
              <td><span class='lvl <%#: Eval("Level") %>'> <%#: Eval("Level") %></span></td>
              <td><span class='badge <%#: Eval("Status") %>'><%#: Eval("Status") %></span></td>
              <td>
                  <a href='quiz-edit.aspx?id=<%#: Eval("QuizID") %>'>Edit</a>
                  <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="lk" CommandName="DeleteModule" CommandArgument='<%# Eval("QuizID")  %>' OnClientClick="return confirm('Are you sure you want to delete this quiz?');" OnCommand="DeleteQuiz_Command" />
              </td>
          </tr>

        <tr><td>Basic Greetings</td><td>501</td><td><span class="lvl N5">N5</span></td><td><span class="badge Published">Published</span></td><td><a href="quiz-editor.html">Edit</a> <a class="lk" href="#">Delete</a></td></tr>
        <tr><td>Numbers &amp; Counting</td><td>502</td><td><span class="lvl N5">N5</span></td><td><span class="badge Published">Published</span></td><td><a href="quiz-editor.html">Edit</a> <a class="lk" href="#">Delete</a></td></tr>
        <tr><td>Daily Conversations</td><td>401</td><td><span class="lvl N4">N4</span></td><td><span class="badge Draft">Draft</span></td><td><a href="quiz-editor.html">Edit</a> <a class="lk" href="#">Delete</a></td></tr>
        <tr><td>Train Station Talk</td><td>402</td><td><span class="lvl N4">N4</span></td><td><span class="badge Published">Published</span></td><td><a href="quiz-editor.html">Edit</a> <a class="lk" href="#">Delete</a></td></tr>
      </table></div>
    </div>
  </main>
    </form>
</body>
</html>
