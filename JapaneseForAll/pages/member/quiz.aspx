<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quiz.aspx.cs" Inherits="JapaneseForAll.pages.member.quiz" %>
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
            <asp:Label ID="quizTitle" runat="server" CssClass="mut"/>
            <asp:Repeater ID="questionRepeater" runat="server" OnItemDataBound="questionRepeater_ItemDataBound">
                <ItemTemplate>
                    <div class="panel narrow">
                        <div class="bar"><i style="width:30%"></i></div>
                        <h2><%#: Eval("QuestionDescription") %></h2>
                        <asp:HiddenField ID="questionId" runat="server" Value='<%# Eval("QuestionID") %>' />
                        <asp:RadioButtonList
                            ID="answerList"
                            runat="server"
                            CssClass="answer-list">
                        </asp:RadioButtonList>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Label ID="errMsg" runat="server" Visible="false" />
            <asp:Button 
                ID="submitQuiz"
                runat="server"
                Text="Submit Quiz"
                CssClass="btn"
                OnClick="submitQuiz_Click" />    
        </main>
    </form>
</body>
</html>
