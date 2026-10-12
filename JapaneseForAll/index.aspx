<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="JapaneseForAll.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover"/>
    <title>Home · Japanese For All</title>
    <link rel="stylesheet" href="css/base.css"/>
    <link rel="stylesheet" href="css/layout.css"/>
    <link rel="stylesheet" href="css/components.css"/>
    <link rel="stylesheet" href="css/pages.css"/>
</head>
<body>
    <header>
        <a class="logo" href="index.aspx"><b>JAPANESE</b> FOR ALL</a>
        <nav></nav>
        <div class="rt"><a class="lk" href="pages/publics/login.aspx">Login</a><a class="btn sm" href="pages/publics/register.aspx">Register</a></div>
    </header>
    <main>
        <section class="hero">
        <div class="kanji">日本語</div>
            <span class="petal p1"></span><span class="petal p2"></span><span class="petal p3"></span><span class="petal p4"></span>
            <span class="petal p5"></span><span class="petal p6"></span><span class="petal p7"></span><span class="petal p8"></span>
            <h1>Learn Japanese The Fun<br/><em>Effective Way</em></h1>
            <p class="mut">Flashcards and quizzes — plus a community of learners and instructors to keep you motivated.</p>
            <div class="row c"><a class="btn" href="pages/publics/register.aspx">Get Started — It's Free</a><a class="btn ghost" href="pages/publics/quizzes.aspx">Browse as Guest</a></div>
            </section>
            <h2 class="c">Choose your path</h2>
            <div class="grid3 wide">
            <a class="card path" href="pages/publics/quizzes.aspx"><div class="ico">🌸</div><h3>Guest</h3><p class="mut">Explore lessons and forums freely — no account needed.</p><b class="lk">Browse Now →</b></a>
            <a class="card path" href="pages/publics/register.aspx"><div class="ico">⛩️</div><h3>Member</h3><p class="mut">Full access to flashcards, quizzes, progress tracking and forums.</p><b class="lk">Join as Member →</b></a>
            <a class="card path" href="pages/publics/register.aspx"><div class="ico">🎌</div><h3>Instructor</h3><p class="mut">Create content, manage quizzes and mentor learners.</p><b class="lk">Apply to Teach →</b></a>
            </div>
    </main>
</body>
</html>

