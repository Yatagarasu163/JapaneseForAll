<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="JapaneseForAll.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover"/>
    <title>Login · Japanese For All</title>
    <link rel="stylesheet" href="../../css/base.css"/>
    <link rel="stylesheet" href="../../css/layout.css"/>
    <link rel="stylesheet" href="../../css/components.css"/>
    <link rel="stylesheet" href="../../css/pages.css"/>
</head>
<body>
    <header>
        <a class="logo" href='<%= ResolveClientUrl("~/index.aspx") %>'><b>JAPANESE</b> FOR ALL</a>
        <nav></nav>
        <div class="rt"><a class="lk" href="login.aspx">Login</a><a class="btn sm" href="register.aspx">Register</a></div>
    </header>

    <main>
        <div class="panel narrow">
            <a href='<%= ResolveClientUrl("~/index.aspx") %>'>← Back to Home</a>
            <h2>Welcome back</h2>
            <p class="mut">Log in to continue your Japanese learning journey.</p>
            <form id="form1" runat="server">

                    <asp:ValidationSummary 
                    ID="ValidationSummary1"
                    runat="server"
                    HeaderText="Please correct the following errors:"
                    CssClass="validation-summary"
                    ForeColor="Red"
                    DisplayMode="BulletList"
                    ValidationGroup="Login"
                    />
                <asp:Label runat="server" ID="errMsg" CssClass="validation-summary" ForeColor="Red" Visible="false"></asp:Label>

                <label>Username<asp:TextBox ID="username" runat="server" placeholder="Enter your name"></asp:TextBox></label>
                <asp:RequiredFieldValidator ID="usernameValidator" runat="server" ControlToValidate="username" ErrorMessage="Username is missing." Text="*" ForeColor="Red" ValidationGroup="Login"/>
                <label>Password<asp:TextBox ID="password" runat="server" TextMode="Password"></asp:TextBox></label>
                <asp:RequiredFieldValidator ID="passwordValidator" runat="server" ControlToValidate="password" ErrorMessage="Pasword is missing." Text="*" ForeColor="Red" ValidationGroup="Login"/>
                <p class="sm"><a href="#">Forgot password?</a></p>
                <div class="row"><asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" Style="width:100%" ValidationGroup="Login" OnClick="btnLogin_Click"/></div>
            </form>
            <p class="c mut">New to Japanese for All? <a href="register.aspx">Register</a></p>
        </div>
    </main>

</body>
</html>
