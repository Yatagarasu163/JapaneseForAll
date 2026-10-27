<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="verification.aspx.cs" Inherits="JapaneseForAll.pages.admin.WebForm3" %>
<%@ Register Src="~/pages/components/AdminNavbar.ascx" TagPrefix="uc" TagName="AdminNavbar" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover">
    <title>Instructor Verification · Japanese For All</title>
    <link rel="stylesheet" href="../../css/base.css">
    <link rel="stylesheet" href="../../css/layout.css">
    <link rel="stylesheet" href="../../css/components.css">
    <link rel="stylesheet" href="../../css/pages.css">
</head>
<body>
    <form id="form1" runat="server">
        <!-- Shared Navigation Component -->
        <uc:AdminNavbar ID="AdminNavbar1" runat="server" />

        <main>
            <div class="panel">
                <div class="row sp" style="margin:0">
                    <div>
                        <h2>Instructor Verification</h2>
                        <p class="mut">Review teaching certificates before approving instructor access</p>
                    </div>
                    <asp:Label ID="lblPendingCount" runat="server" CssClass="badge Pending" Text="0 pending" />
                </div>

                <!-- Dynamic list of pending instructors -->
                <asp:Repeater ID="rptPendingInstructors" runat="server" OnItemCommand="rptPendingInstructors_ItemCommand">
                    <ItemTemplate>
                        <div class="card row sp">
                            <div>
                                <b><%# Eval("FullName") %></b><br>
                                <span class="mut"><%# Eval("Email") %></span><br>
                                📄 <%# Eval("CertificateFileName") %> · Submitted <%# Eval("SubmittedText") %>
                            </div>
                            <div>
                                <asp:HyperLink ID="lnkViewCert" runat="server" CssClass="lk" NavigateUrl='<%# Eval("Certificate") %>' Target="_blank">View Certificate</asp:HyperLink>
                                <asp:Button ID="btnReject" runat="server" CssClass="btn ghost sm" Text="Reject" CommandName="Reject" CommandArgument='<%# Eval("USER_ID") %>' />
                                <asp:Button ID="btnApprove" runat="server" CssClass="btn sm" Text="Approve" CommandName="Approve" CommandArgument='<%# Eval("USER_ID") %>' />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Label ID="lblMessage" runat="server" CssClass="mut" Visible="false" Text="No pending instructor verifications found." />
            </div>
        </main>
    </form>
</body>
</html>