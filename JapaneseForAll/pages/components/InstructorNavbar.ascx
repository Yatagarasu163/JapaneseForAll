<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstructorNavbar.ascx.cs" Inherits="JapaneseForAll.pages.components.InstructorNavbar" %>

  <header>
    <a class="logo" href="~/pages/index.aspx"><b>JAPANESE</b> FOR ALL</a>
    <nav>
      <a href="~/pages/instructor/content.aspx" class="on">Learn</a>
      <a href="~/pages/instructor/quizzes.aspx">Quiz</a>
      <a href="~/pages/member/forum.aspx">Forum</a>
    </nav>
    <div class="rt">
      <span><asp:Label ID="fname" runat="server"/></span><span class="av"><asp:Label ID="initials" runat="server"/></span>
        <asp:HyperLink ID="LogOut" runat="server" CssClass="btn ghost sm" NavigateUrl="~/pages/publics/login.aspx" OnClick="OnLogOut_Click" Text="Log out" />
    </div>
  </header>