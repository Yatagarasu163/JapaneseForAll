<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="JapaneseForAll.pages.member.WebForm1" %>
<%@ Register Src="~/pages/components/Navbar.ascx" TagPrefix="uc" TagName="Navbar" %>
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

        <uc:Navbar ID="Navbar1" runat="server" />

        <main>
            <div class="panel">
                <h2>Welcome back, <asp:Label runat="server" ID="fname2"></asp:Label> 👋</h2>
                <div class="grid3">
                <div class="card stat"><span class="mut">Modules Completed</span><b>12 / 30</b><span class="mut sm">+2 this week</span></div>
                <div class="card stat"><span class="mut">Average Quiz Score</span><b>86%</b><span class="mut sm">↑ 4% improvement</span></div>
                <div class="card stat"><span class="mut">Forum Posts</span><b>4</b><span class="mut sm">2 new replies</span></div>
                </div>

                <h3>Continue Learning</h3>
                <div class="grid3">
                <!-- REPEAT: one card per in-progress item -->
                <a class="card" href="flashcards.html"><span class="lvl N5">N5</span> <b>Basic Greetings</b><p class="mut">Flashcards 60% complete</p></a>
                <a class="card" href="quiz.html"><span class="lvl N5">N5</span> <b>Numbers &amp; Counting</b><p class="mut">Quiz · Not started</p></a>
                <a class="card" href="flashcards.html"><span class="lvl N4">N4</span> <b>Daily Conversations</b><p class="mut">Flashcards 20% complete</p></a>
                </div>
            
                <h3>Recommended for you</h3>
                <div class="grid3">
                <!-- REPEAT: one card per recommendation -->
                <div class="card"><span class="mut sm">Flashcards</span><h3>N5 Weather &amp; Seasons</h3><a class="lk" href="flashcards.html">Start lesson →</a></div>
                <div class="card"><span class="mut sm">Quiz</span><h3>N5 Shopping Phrases</h3><a class="lk" href="quiz.html">Start lesson →</a></div>
                <div class="card"><span class="mut sm">Flashcards</span><h3>N4 Train Station Talk</h3><a class="lk" href="flashcards.html">Start lesson →</a></div>
                </div>
            </div>
        </main>
    </form>
</body>
</html>
