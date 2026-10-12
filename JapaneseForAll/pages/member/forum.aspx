<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forum.aspx.cs" Inherits="JapaneseForAll.pages.member.forum" %>
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
                <div class="panel">
                  <div class="row sp" style="margin:0">
                    <div><h2>Discussion Forum</h2><p class="mut">Chat-style conversations with fellow learners and instructors</p></div>
                    <a class="btn" href="new-topic.aspx">+ New Topic</a>
                  </div>
                  <!-- REPEAT: one row per thread -->
                    <asp:Label runat="server" ID="errMsg" Visible="false"></asp:Label>
                    <asp:Repeater ID="threadRepeater" runat="server">
                        <ItemTemplate>
                            <a class="thread" href="thread.aspx?id=<%# Eval("ThreadId") %>">
                                <div>
                                    <b>
                                        <%#: Eval("ThreadTitle") %>
                                    </b>
                                    <asp:PlaceHolder runat="server" Visible='<%# Convert.ToBoolean(Eval("IsPinned")) %>'>
                                        <span class="pin">PINNED</span>
                                    </asp:PlaceHolder>
                                    <p class="mut">Started by <%#: Eval("Author") %></p>
                                </div>
                                <span class="mut"><%#: Eval("ReplyCount") %> replies</span>
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </main>
    </form>
</body>
</html>