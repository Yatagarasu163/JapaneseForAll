<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navbar.ascx.cs" Inherits="JapaneseForAll.pages.components.Navbar" %>
        <header>
            <asp:HyperLink runat="server" ID="lnkHome" NavigateUrl="~/index.aspx" CssClass="logo">
                <b>JAPANESE</b> FOR ALL
            </asp:HyperLink>
            
            <nav>
                <asp:HyperLink runat="server" ID="lnkMemberDashboard" CssClass="" Text="Home" NavigateUrl="~/pages/member/dashboard.aspx"/>
                <asp:HyperLink runat="server" ID="lnkQuizzes" Text="Quiz"  NavigateUrl="~/pages/member/quizzes.aspx"/>
                <asp:HyperLink runat="server" ID="lnkFlashcards" Text ="Flashcards" NavigateUrl="~/pages/member/flashcard-list.aspx" />
                <asp:HyperLink runat="server" ID="lnkForum" Text="Forum" NavigateUrl="~/pages/member/forum.aspx"/>
                <asp:HyperLink runat="server" ID="lnkProfile" Text="Profile" NavigateUrl="~/pages/member/profile.aspx"/>

        


            </nav>
            <div class="rt">
                <span><asp:Label runat="server" ID="fname1"></asp:Label></span><span class="av"><asp:Label runat="server" ID="initial"></asp:Label></span>
                <asp:LinkButton runat="server" ID="LogOutLink" OnClick="LogOut_Click" CssClass="btn ghost sm">Log Out</asp:LinkButton>
            </div>
        </header>