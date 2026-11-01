<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminNavbar.ascx.cs" Inherits="JapaneseForAll.pages.components.AdminNavbar" %>
<header>
    <asp:HyperLink runat="server" ID="lnkHome" NavigateUrl="~/index.aspx" CssClass="logo">
        <b>JAPANESE</b> FOR ALL
    </asp:HyperLink>
    
    <nav>
        <asp:HyperLink runat="server" ID="lnkManagement" Text="Home" NavigateUrl="~/pages/admin/management.aspx"/>
        <asp:HyperLink runat="server" ID="lnkVerification" Text="Verification" NavigateUrl="~/pages/admin/verification.aspx"/>
    </nav>
    
    <div class="rt">
        <span><asp:Label runat="server" ID="fname1"></asp:Label></span><span class="av"><asp:Label runat="server" ID="initial"></asp:Label><asp:Image runat="server" ID="photo" CssClass="av-img" Visible="false" AlternateText="Profile photo" Width="30" Height="30" style="object-fit:cover;border-radius:50%" /></span>
        <asp:LinkButton runat="server" ID="LogOutLink" OnClick="LogOut_Click" CssClass="btn ghost sm">Log Out</asp:LinkButton>
    </div>
</header>