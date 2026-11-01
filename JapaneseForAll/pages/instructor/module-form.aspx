<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="module-form.aspx.cs" Inherits="JapaneseForAll.pages.instructor.module_form" %><%@ Register Src="~/pages/components/InstructorNavbar.ascx" TagPrefix="uc" TagName="InstructorNavbar" %><!DOCTYPE html><html xmlns="http://www.w3.org/1999/xhtml"><head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover" />
    <title>Learning Module · Japanese For All</title>

    <link rel="stylesheet" href="../../css/base.css" />
    <link rel="stylesheet" href="../../css/layout.css" />
    <link rel="stylesheet" href="../../css/components.css" />
    <link rel="stylesheet" href="../../css/pages.css" /></head><body>
    <%-- enctype is required so the image files are sent to the server --%>
    <form id="form1" runat="server" enctype="multipart/form-data">

        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <uc:InstructorNavbar ID="Navbar1" runat="server" />

        <main>
            <div class="panel narrow">

                <asp:HyperLink ID="lnkBack" runat="server"
                    NavigateUrl="content.aspx">
                    ← Back to Learning Content
                </asp:HyperLink>

                <h2><%: HeadingText %></h2>
                <p class="mut">
                    Add a new flashcard set.
                </p>

                <label>
                    Module Title
                    <asp:TextBox ID="txtTitle" runat="server"
                        placeholder="e.g. N5 - Weather &amp; Seasons" />
                </label>

                <%-- Status chips (Published / Draft) --%>
                <label>Status</label>
                <asp:RadioButtonList ID="rblStatus" runat="server"
                    CssClass="chips"
                    RepeatLayout="Flow"
                    RepeatDirection="Horizontal">
                    <asp:ListItem Text="Published" Value="Published" />
                    <asp:ListItem Text="Draft" Value="Draft" Selected="True" />
                </asp:RadioButtonList>

                <%-- JLPT level chips --%>
                <label>JLPT Level</label>
                <asp:RadioButtonList ID="rblLevel" runat="server"
                    CssClass="chips"
                    RepeatLayout="Flow"
                    RepeatDirection="Horizontal">
                    <asp:ListItem Text="N5" Value="N5" Selected="True" />
                    <asp:ListItem Text="N4" Value="N4" />
                    <asp:ListItem Text="N3" Value="N3" />
                    <asp:ListItem Text="N2" Value="N2" />
                    <asp:ListItem Text="N1" Value="N1" />
                </asp:RadioButtonList>

                <asp:UpdatePanel ID="upCards" runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Repeater ID="rptCards" runat="server">
                            <ItemTemplate>

                                <div class="card qe">

                                    <div class="row sp" style="margin:0">
                                        <b>Card <%# Container.ItemIndex + 1 %></b>

                                        <span>
                                            <asp:LinkButton ID="btnRemove"
                                                runat="server"
                                                CssClass="lk"
                                                Text="Remove"
                                                CommandArgument='<%# Container.ItemIndex %>'
                                                OnClick="btnRemove_Click"
                                                CausesValidation="false"
                                                OnClientClick="return confirm('Remove this card?');" />
                                        </span>
                                    </div>

                                    <label>
                                        Front Text
                                        <asp:TextBox ID="txtFrontText"
                                            runat="server"
                                            Text='<%# Eval("FrontText") %>' />
                                    </label>

                                    <label>
                                        Front Romaji
                                        <asp:TextBox ID="txtFrontRomaji"
                                            runat="server"
                                            Text='<%# Eval("FrontRomaji") %>' />
                                    </label>

                                    <label>
                                        Back Text
                                        <asp:TextBox ID="txtBackText"
                                            runat="server"
                                            Text='<%# Eval("BackText") %>' />
                                    </label>

                                    <label>
                                        Back Note
                                        <asp:TextBox ID="txtBackNote"
                                            runat="server"
                                            Text='<%# Eval("BackNote") %>' />
                                    </label>

                                    <%-- Edit mode: shows the image that is already saved --%>
                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"
                                        Visible='<%# !string.IsNullOrEmpty(Eval("ImagePath") as string) %>'>
                                        <asp:Image ID="Image1" runat="server"
                                            ImageUrl='<%# Eval("ImagePath") %>'
                                            AlternateText="Current image"
                                            style="max-height:80px;border-radius:8px;display:block;margin:6px 0" />
                                        <small class="mut">Current image. Choose a file below to replace it.</small>
                                    </asp:PlaceHolder>

                                    <label>
                                        Image
                                        <%-- Hidden field to preserve path across partial postbacks --%>
                                        <asp:HiddenField ID="hfExistingImage" runat="server" Value='<%# Eval("ImagePath") %>' />
                                        
                                        <input type="file" id="fileImage" runat="server"
                                            accept="image/*" />
                                    </label>

                                </div>

                            </ItemTemplate>
                        </asp:Repeater>

                        <div class="row">
                            <asp:Button ID="btnAddCard"
                                runat="server"
                                Text="+ Add Card"
                                CssClass="btn ghost"
                                CausesValidation="false"
                                OnClick="btnAddCard_Click" />
                        </div>

                        <asp:Label ID="lblMessage" runat="server" CssClass="err" />

                        <div class="row sp">
                            <asp:HyperLink ID="lnkCancel"
                                runat="server"
                                NavigateUrl="content.aspx"
                                CssClass="btn ghost">
                                Cancel
                            </asp:HyperLink>

                            <asp:Button ID="btnSaveFlashcard"
                                runat="server"
                                Text="Save Flashcard"
                                CssClass="btn"
                                OnClick="btnSaveFlashcard_Click" />
                        </div>

                    </ContentTemplate>

                    <Triggers>
                        <%-- Saving must be a FULL postback, otherwise the image files are not sent --%>
                        <asp:PostBackTrigger ControlID="btnSaveFlashcard" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </main>

    </form></body></html>