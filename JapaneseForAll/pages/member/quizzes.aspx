<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quizzes.aspx.cs" Inherits="JapaneseForAll.pages.member.WebForm2" %>
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
        
        <uc:Navbar ID="Navbar1" runat="server"/>

        <main>
            <div class="panel">
                <h2>Quizzes</h2>
                <p class="mut">Log in to track your scores.</p>
                    <a class="fb on" href="quizzes.aspx">All Levels</a>
                    <a class="fb" href="quizzes.aspx?level=N5">N5</a>
                    <a class="fb" href="quizzes.aspx?level=N4">N4</a>
                    <a class="fb" href="quizzes.aspx?level=N3">N3</a>
                    <a class="fb" href="quizzes.aspx?level=N2">N2</a>
                    <a class="fb" href="quizzes.aspx?level=N1">N1</a>
                    <asp:TextBox runat="server" ID="searchTextBox" Style="max-width:220px" placeholder="🔍 Search quizzes…"></asp:TextBox>
                    <asp:Button  runat="server" ID="searchButton" CssClass="btn sm" Text="Search" OnClick="searchButton_Click"/>
                <div class="grid3">
                    <asp:Label runat="server" ID="errMsg" ForeColor="White" Text="There are no quizzes at the moment! Come back later." Visible="false"></asp:Label>
                    <asp:Repeater ID="quizRepeater" runat="server">
                        <ItemTemplate>
                            <div class="card">
                                <div class="top">
                                    <span class='lvl <%# Eval("Level") %>'>
                                        <%# Eval("Level") %>
                                    </span>

                                    <span class='st <%# Eval("StatusCss") %>'>
                                        <%# Eval("ProgressStatus") %>
                                    </span>
                                </div>

                                <h3><%# Eval("Title") %></h3>

                                <p class="mut">
                                    <%# Eval("QuestionCount") %> Qs · Score:
                                    <%# Eval("Score") == DBNull.Value
                                        ? "—"
                                        : Eval("Score").ToString() + " / " +
                                          Eval("QuestionCount").ToString() %>
                                </p>

                                <a class="btn sm"
                                   href='quiz.aspx?id=<%# Eval("QuizId") %>'>
                                    <%# Eval("ActionText") %>
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </main>
    </form>
</body>
</html>
