```aspx
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quiz-editor.aspx.cs" Inherits="JapaneseForAll.pages.instructor.quiz_editor" %>
<%@ Register Src="~/pages/components/InstructorNavbar.ascx" TagPrefix="uc" TagName="InstructorNavbar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover" />
    <title>Quiz Editor · Japanese For All</title>

    <link rel="stylesheet" href="../../css/base.css" />
    <link rel="stylesheet" href="../../css/layout.css" />
    <link rel="stylesheet" href="../../css/components.css" />
    <link rel="stylesheet" href="../../css/pages.css" />
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <uc:InstructorNavbar ID="Navbar1" runat="server" />

        <main>
            <div class="panel narrow">

                <asp:HyperLink ID="lnkBack" runat="server"
                    NavigateUrl="quizzes.aspx">
                    ← Back to Quizzes
                </asp:HyperLink>

                <h2>Quiz Editor</h2>
                <p class="mut">
                    Add, edit or remove questions. Changes apply to future attempts.
                </p>

                <label>
                    Title
                    <asp:TextBox ID="title" runat="server" />
                </label>

                <div class="row">
                    <div class="col">
                        <asp:Label ID="lblLevel" runat="server"
                            AssociatedControlID="ddlLevel" Text="Level" />

                        <asp:DropDownList ID="ddlLevel" runat="server">
                            <asp:ListItem Selected="True">N5</asp:ListItem>
                            <asp:ListItem>N4</asp:ListItem>
                            <asp:ListItem>N3</asp:ListItem>
                            <asp:ListItem>N2</asp:ListItem>
                            <asp:ListItem>N1</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col">
                        <asp:Label ID="lblStatus" runat="server"
                            AssociatedControlID="ddlStatus" Text="Status" />

                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem Selected="True">Published</asp:ListItem>
                            <asp:ListItem>Draft</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <asp:UpdatePanel ID="upQuestions" runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Repeater ID="rptQuestions" runat="server">
                            <ItemTemplate>

                                <div class="card qe">

                                    <div class="row sp" style="margin:0">
                                        <b>Question <%# Container.ItemIndex + 1 %></b>

                                        <span>
                                            <asp:LinkButton ID="btnRemove"
                                                runat="server"
                                                CssClass="lk"
                                                Text="Remove"
                                                CommandArgument='<%# Container.ItemIndex %>'
                                                OnClick="btnRemove_Click"
                                                CausesValidation="false"
                                                OnClientClick="return confirm('Remove this question?');" />
                                        </span>
                                    </div>

                                    <label>
                                        Question
                                        <asp:TextBox ID="txtQuestion"
                                            runat="server"
                                            Text='<%# Eval("Question") %>' />
                                    </label>

                                    <label>
                                        Options — select the correct answer
                                    </label>

                                    <div class="orow">
                                        <asp:RadioButton ID="rbCorrect1"
                                            runat="server"
                                            GroupName="CorrectAnswer"
                                            Checked='<%# Eval("CorrectAnswer").ToString() == "1" %>' />

                                        <asp:TextBox ID="txtOption1"
                                            runat="server"
                                            Text='<%# Eval("Option1") %>' />
                                    </div>

                                    <div class="orow">
                                        <asp:RadioButton ID="rbCorrect2"
                                            runat="server"
                                            GroupName="CorrectAnswer"
                                            Checked='<%# Eval("CorrectAnswer").ToString() == "2" %>' />

                                        <asp:TextBox ID="txtOption2"
                                            runat="server"
                                            Text='<%# Eval("Option2") %>' />
                                    </div>

                                    <div class="orow">
                                        <asp:RadioButton ID="rbCorrect3"
                                            runat="server"
                                            GroupName="CorrectAnswer"
                                            Checked='<%# Eval("CorrectAnswer").ToString() == "3" %>' />

                                        <asp:TextBox ID="txtOption3"
                                            runat="server"
                                            Text='<%# Eval("Option3") %>' />
                                    </div>

                                    <div class="orow">
                                        <asp:RadioButton ID="rbCorrect4"
                                            runat="server"
                                            GroupName="CorrectAnswer"
                                            Checked='<%# Eval("CorrectAnswer").ToString() == "4" %>' />

                                        <asp:TextBox ID="txtOption4"
                                            runat="server"
                                            Text='<%# Eval("Option4") %>' />
                                    </div>

                                </div>

                            </ItemTemplate>
                        </asp:Repeater>

                        <div class="row">
                            <asp:Button ID="btnAddQuestion"
                                runat="server"
                                Text="+ Add Question"
                                CssClass="btn ghost"
                                CausesValidation="false"
                                OnClick="btnAddQuestion_Click" />
                        </div>

                        <div class="row sp">
                            <asp:HyperLink ID="lnkCancel"
                                runat="server"
                                NavigateUrl="quizzes.aspx"
                                CssClass="btn ghost">
                                Cancel
                            </asp:HyperLink>

                            <asp:Button ID="btnSaveQuiz"
                                runat="server"
                                Text="Save Quiz"
                                CssClass="btn"
                                OnClick="btnSaveQuiz_Click" />
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </main>

    </form>
</body>
</html>
```