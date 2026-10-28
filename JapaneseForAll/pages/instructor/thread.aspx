<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="thread.aspx.cs" Inherits="JapaneseForAll.pages.instructor.thread" %>
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
                <div class="panel narrow">
                  <a href="forum.aspx">← Back to Forum</a>
                    <asp:Repeater ID="threadDetailsRepeater" runat="server">
                        <ItemTemplate>
                            <h2><%#: Eval("title") %></h2>
                            <p><%#: Eval("description") %></p>
                            <p class="mut">Started by <%#: Eval("author") %> · <%#: Eval("Timestamp", "{0:dd MMM yyyy, hh:mm tt}") %></p>
                        </ItemTemplate>

                    </asp:Repeater>
                  <div class="chat">
                    <!-- REPEAT: one bubble per post (add class "mine" for the signed-in user's posts) -->
                      <asp:Repeater ID="threadRepeater" runat="server">
                          <ItemTemplate>
                              <div class='<%# Convert.ToInt32(Eval("UserId")) == Convert.ToInt32(Session["uid"]) ? "msg mine" : "msg" %>'>
                                  <b>
                                      <%# Convert.ToBoolean(Eval("IsInstructor")) ? "<span class=\"tag\">Instructor</span> " : "" %><%#: Eval("Author") %>
                                  </b>
                                  <small class="mut">
                                      <%#: Eval("Timestamp", "{0:dd MMM yyy, hh:mm tt}") %>
                                  </small>

                                  <p><%#: Eval("message") %></p>
                              </div>
                          </ItemTemplate>
                      </asp:Repeater>
                  </div>
                    <asp:TextBox ID="reply" TextMode="MultiLine" Rows="2" Placeholder="Write a reply..." runat="server" CssClass="input"/>
                    <asp:Button CssClass="btn" ID="btnReply" runat="server" Text="Reply" OnClick="OnReply_Click"/>
                    <asp:Button
                    ID="btnDeleteThread"
                    runat="server"
                    Text="Delete Thread"
                    CssClass="btn"
                    OnClick="OnDeleteThread_Click"
                    OnClientClick="return confirm('Are you sure you want to delete this thread?');" />
                    <asp:Label ID="errMsg" runat="server" Visible="false"/>
                </div>
            </main>

    </form>
</body>
</html>

