<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="new-topic.aspx.cs" Inherits="JapaneseForAll.pages.instructor.new_topic" %>
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
                  <h2>Start a New Topic</h2>
                  <p class="mut">Share a question or start a conversation with the community.</p>
                    <asp:ValidationSummary 
                        ID="ValidationSummary1" 
                        runat="server"
                        HeaderText="Please ensure the following fields are populated:"
                        CssClass="validation-summary"
                        ForeColor="Red"
                        DisplayMode="BulletList"
                        ValidationGroup="Forum"
                        />
                    <label>Topic Title<asp:TextBox ID="title" runat="server" placeholder="e.g. Best resources for learning Kanji"></asp:TextBox></label>
                    <asp:RequiredFieldValidator ID="titleValidator" runat="server" ControlToValidate="title" ErrorMessage="Title is missing." Text="*" ForeColor="Red" ValidationGroup="Forum" />
                    <label>Category</label>
                    <div class="chips">

                      <label class="chip"><asp:RadioButton ID="rbGrammar" runat="server" GroupName="category" Text="" Checked="true"/><span>Grammar</span></label>
                      <label class="chip"><asp:RadioButton ID="rbVocabulary" runat="server" GroupName="category" Text=""/><span>Vocabulary</span></label>
                      <label class="chip"><asp:RadioButton ID="rbStudy" runat="server" GroupName="category" Text=""/><span>Study Tips</span></label>
                      <label class="chip"><asp:RadioButton ID="rbSpeaking" runat="server" GroupName="category" Text=""/><span>Speaking</span></label>
                      <label class="chip"><asp:RadioButton ID="rbGeneral" runat="server" GroupName="category" Text=""/><span>General</span></label>
                    </div>
                        <label>Message<asp:TextBox ID="msg" runat="server" Placeholder="Write your thoughts here!" TextMode="MultiLine" Rows="5" CssClass="textarea" ></asp:TextBox></label>
                        <asp:RequiredFieldValidator ID="msgValidator" runat="server" ControlToValidate="msg" ErrorMessage="Message is missing." Text="*" ForeColor="Red" ValidationGroup="Forum" />
                            <div class="row"><asp:Button ID="cancel" CssClass="btn ghost" runat="server" OnClick="OnCancel_Click" Text="Cancel"/><asp:Button ID="submit" CssClass="btn" runat="server" OnClick="OnSubmit_Click" Text="Publish Topic" /></div>       
                </div>
          </main>
    </form>
</body>
</html>