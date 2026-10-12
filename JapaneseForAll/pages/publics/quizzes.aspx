<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quizzes.aspx.cs" Inherits="JapaneseForAll.pages.publics.WebForm2" %>

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
     <header>
   <a class="logo" href="../../index.aspx"><b>JAPANESE</b> FOR ALL</a>
   <nav></nav>
   <div class="rt"><a class="lk" href="../publics/login.aspx">Login</a><a class="btn sm" href="../publics/register.aspx">Register</a></div>
 </header>
    <main>
        <form runat="server" id="form1">
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
                                    <span class='lvl <%#Eval("Level")%>'>
                                        <%#Eval("Level") %>
                                    </span>
                                </div>

                                <h3><%#Eval("Title") %></h3>

                                <p class="mut">
                                    <%#Eval("QuestionCount") %> Qs
                                </p>

                                <a class="btn sm" href='<%#Eval("Url") %>'>
                                    Start →
                                </a>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </form>
    </main>
</body>
</html>