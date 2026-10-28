<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="result.aspx.cs" Inherits="JapaneseForAll.pages.member.result" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
          <main>
            <div class="panel narrow c">
              <div class="medal">🎉</div>
              <h2>Quiz Complete!</h2>
                <asp:Label ID="quizTitle" runat="server" CssClass="mut"></asp:Label>
              <div class="big"><asp:Label ID="score" runat="server"></asp:Label></div>
              <p>
                  <asp:Label Style="color:var(--ok)" ID="scorePercent" runat="server">Great job - 90% correct</asp:Label>
              </p>
              <div class="res">
                <asp:Repeater ID="resultRepeater" runat="server">
                    <ItemTemplate>
                        <div class='<%# Convert.ToBoolean(Eval("IsCorrect")) ? "ok" : "bad" %>'>
                            <span>
                                <%# Eval("Question") %>
                            </span>

                            <b>
                                <%# Convert.ToBoolean(Eval("IsCorrect"))
                                    ? "✓ Correct"
                                    : (Convert.ToBoolean(Eval("IsAnswered"))
                                        ? "✕ Incorrect"
                                        : "— Unanswered") %>
                            </b>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
              </div>
              <div class="row c"><a class="btn" href="dashboard.aspx">Back to Dashboard</a></div>
            </div>
          </main>
    </form>
</body>
</html>
