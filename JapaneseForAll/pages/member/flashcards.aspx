<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="flashcards.aspx.cs" Inherits="JapaneseForAll.pages.member.flashcards" %>
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
            <div class="panel narrow c">
                <p class="mut">
                    <asp:Label ID="lblSetTitle" runat="server" /> : Flashcards · Card
                    <asp:Label ID="lblCardNumber" runat="server" /> of 
                    <asp:Label ID="lblTotalCards" runat="server" />
                </p>

                <div class="bar">
                    <i id="progressBar" runat="server"></i>
                </div>

                <asp:Repeater ID="flashcardRepeater" runat="server">
                    <ItemTemplate> 
                        <label class="flip">
                            <input type="checkbox" />

                            <span class="front">
                                <span class="jp">
                                    <%#: Eval("FrontText") %>
                                </span>

                                <span class="mut">
                                    <%#: Eval("FrontRomaji")%>
                                </span>

                                <span class="back">
                                    <%#: Eval("BackText") %>
                                </span>

                                <span class="mut">
                                    <%#: Eval("BackNote") %>
                                </span>
                            </span>
                        </label>
                    </ItemTemplate>
                </asp:Repeater>

                <p class="mut sm">Tap the card to flip</p>

                <div class="row sp">
                    <asp:HyperLink ID="lnkPrevious" runat="server" CssClass="btn ghost" Text="← Previous" />

                    <asp:HyperLink ID="lnkNext" runat="server" CssClass="btn" Text="Next Card →" />
                </div>
            </div>
        </main>
    </form>
</body>
</html>
