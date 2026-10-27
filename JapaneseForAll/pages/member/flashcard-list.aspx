<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="flashcard-list.aspx.cs" Inherits="JapaneseForAll.pages.member.flashcard_list" %>
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

        <uc:Navbar ID="Navbar1" runat="server"/>

        <main>
        <div class="panel">
            <h2>Flashcards</h2>
            <a class="fb" href="flashcard-list.aspx">All Levels</a>
            <a class="fb" href="flashcard-list.aspx?level=N5">N5</a>
            <a class="fb" href="flashcard-list.aspx?level=N4">N4</a>
            <a class="fb" href="flashcard-list.aspx?level=N3">N3</a>
            <a class="fb" href="flashcard-list.aspx?level=N2">N2</a>
            <a class="fb" href="flashcard-list.aspx?level=N1">N1</a>
            <asp:TextBox runat="server" ID="searchTextBox" Style="max-width:220px" placeholder="🔍 Search flashcards…"></asp:TextBox>
            <div class="grid3">
           <asp:Label runat="server" ID="errMsg" ForeColor="White" Text="There are no flashcards at the moment! Come back later." Visible="false"></asp:Label>
            <asp:Repeater ID="flashcardRepeater" runat="server">
               <ItemTemplate>
                    <div class="card">
                        <div class="top">
                            <span class='lvl <%# Eval("Level") %>'>
                                <%# Eval("Level") %>
                            </span>
                        </div>

                        <h3><%# Eval("Title") %></h3>

                        <p class="mut">
                            <%# Eval("FlashcardCount") %> Qs
                        </p>

                        <a class="btn sm"
                           href='flashcard.aspx?id=<%# Eval("FlashcardId") %>'>
                            Start →
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
