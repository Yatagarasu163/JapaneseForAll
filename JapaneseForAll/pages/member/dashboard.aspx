<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="JapaneseForAll.pages.member.WebForm1" %>
<%@ Register Src="~/pages/components/Navbar.ascx" TagPrefix="uc" TagName="Navbar" %>
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

        <uc:Navbar ID="Navbar1" runat="server" />

        <main>
            <div class="panel">
                <h2>Welcome back, <asp:Label runat="server" ID="fname2"></asp:Label> 👋</h2>
                <div class="grid3">
                    <div class="card stat">
                        <span class="mut">Quizzes Completed</span>
                        <b><asp:Label ID="completedModules" runat="server" /></b>
                    </div>

                    <div class="card stat">
                        <span class="mut">Average Quiz Score</span>
                        <b><asp:Label ID="quizAverage" runat="server" /></b>
                    </div>

                    <div class="card stat">
                        <span class="mut">Forum Posts</span>
                        <b><asp:Label ID="forumPosts" runat="server" /></b>
                    </div>
                </div>

                <h3>Flashcards</h3>
                <asp:Repeater ID="flashcardRepeater" runat="server">
                    <ItemTemplate>
                        <div class="card">
                            <span class="mut sm"><h3><%# Eval("FlashcardTitle") %></h3><a class="lk" href='flashcards.aspx?id=<%# Eval("FlashcardID") %>'>Start lesson →</a></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            
                <h3>Quizzes</h3>
                <div class="grid3">
                    <asp:Repeater ID="quizRepeater" runat="server">
                        <ItemTemplate>
                            <div class="card">
                                <span class="mut sm"><h3><%# Eval("QuizTitle") %></h3><a class="lk" href='quiz.aspx?id=<%# Eval("QuizID") %>'>Start lesson →</a></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </main>
    </form>
</body>
</html>
