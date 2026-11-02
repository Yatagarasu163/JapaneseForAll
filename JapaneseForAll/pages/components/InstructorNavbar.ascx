<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstructorNavbar.ascx.cs" Inherits="JapaneseForAll.pages.components.InstructorNavbar" %>

  <header>
    <a class="logo" href="../../index.aspx"><b>JAPANESE</b> FOR ALL</a>
    <nav>
      <a href="content.aspx">Flashcards</a>
      <a href="quizzes.aspx">Quiz</a>
      <a href="forum.aspx">Forum</a>
      <asp:HyperLink runat="server" ID="lnkProfile" Text="Profile" NavigateUrl="~/pages/instructor/profile.aspx"/>
    </nav>
    <div class="rt">
      <span><asp:Label runat="server" ID="fname1"></asp:Label></span><span class="av"><asp:Label runat="server" ID="initial"></asp:Label><asp:Image runat="server" ID="photo" CssClass="av-img" Visible="false" AlternateText="Profile photo" Width="30" Height="30" style="object-fit:cover;border-radius:50%" /></span>
        <asp:HyperLink ID="LogOut" runat="server" CssClass="btn ghost sm" NavigateUrl="~/pages/publics/login.aspx" OnClick="OnLogOut_Click" Text="Log out" />
    </div>
  </header>