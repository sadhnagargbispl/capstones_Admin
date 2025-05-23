<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DailyIncentiveDetailReport.aspx.cs" Inherits="DailyIncentiveDetailReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function openPopup(element) {
            var url = element.href;
            hs.htmlExpand(element, {
                objectType: 'iframe',
                width: 620,
                height: 450,
                marginTop: 0
            });
            return false;
        }
    </script>


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">DailyIncentive Detail  Report</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        Member ID :
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        FromDate :
                                        <asp:DropDownList ID="DDlFromDate" runat="server" class="form-control" Style="display: inline">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Todate :
                                        <asp:DropDownList ID="DDltodate" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        PageSize:
                                       <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" onclick="ddlPageSize_SelectedIndexChanged">
                                           <asp:ListItem Text="10" Value="10" />
                                           <asp:ListItem Text="20" Value="20" />
                                           <asp:ListItem Text="50" Value="50" />
                                           <asp:ListItem Text="100" Value="100" />
                                           <asp:ListItem Text="200" Value="200" />
                                           <asp:ListItem Text="300" Value="300" />
                                           <asp:ListItem Text="400" Value="400" />
                                           <asp:ListItem Text="500" Value="500" />
                                           <asp:ListItem Text="600" Value="600" />
                                           <asp:ListItem Text="2000" Value="2000" />
                                       </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <br />

                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" OnClick="BtnShow_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click " />
                                    </div>
                                </div>

                                <div id="doublescroll" class="col-md-12">
                                    <p>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblinv" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>

                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">

                                                        <Columns>
                                                            <%--<asp:BoundField DataField="SNo" HeaderText="SNo." SortExpression="SNo" />--%>
                                                            <asp:TemplateField HeaderText="SNo.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Payout Date" SortExpression="payoutdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="StartDate" runat="server" Text='<%# Eval("payoutdate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Member ID" SortExpression="Member ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMemberId" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Member Name" SortExpression="Member Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMemberName" runat="server" Text='<%# Eval("mem_Name") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Daily Stacking Bonus" SortExpression="Daily Stacking Bonus">
                                                                <ItemTemplate>
                                                                    <a href='<%# "ViewSelfIncome.aspx?formno=" + Eval("FormNo") + "&SessId=" + Eval("SessId") %>'
                                                                        onclick="return openPopup(this)">
                                                                        <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text='<%# Eval("SelfIncome") %>'></asp:Label>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Direct Bonus" SortExpression="Direct Bonus">
                                                                <ItemTemplate>
                                                                    <a href='<%# "ViewdirectIncome.aspx?formno=" + Eval("FormNo") + "&SessId=" + Eval("SessId") %>'
                                                                        onclick="return openPopup(this)">
                                                                        <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text='<%# Eval("PairIncentive") %>'></asp:Label>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Level Bonus" SortExpression="Level Bonus">
                                                                <ItemTemplate>
                                                                    <a href='<%# "ViewLevelIncome.aspx?formno=" + Eval("FormNo") + "&SessId=" + Eval("SessId") %>'
                                                                        onclick="return openPopup(this)">
                                                                        <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text='<%# Eval("LevelIncome") %>'></asp:Label>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Award & Reward Bonus" SortExpression="Rank Bonus">
                                                                <ItemTemplate>
                                                                    <a href='<%# "ViewRankIncome.aspx?formno=" + Eval("FormNo") + "&SessId=" + Eval("SessId") %>'
                                                                        onclick="return openPopup(this)">
                                                                        <asp:Label ID="Label12" runat="server" ForeColor="Blue" Text='<%# Eval("RewardInc") %>'></asp:Label>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Salary Bonus" SortExpression="Salary Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("RoyaltyIncome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Team Turnover Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("BusinessDevelopment")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Bonus" SortExpression="Total Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("Total")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Admin Charge" SortExpression="Admin Charge">
                                                                <ItemTemplate>
                                                                    <%#Eval("AdminCharge")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Product wallet deduction" SortExpression="Product wallet deduction">
                                                                <ItemTemplate>
                                                                    <%#Eval("CouponsAmt")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Net Bonus" SortExpression="Net Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("ChqAmt")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Token Rate" SortExpression="Token Rate">
                                                                <ItemTemplate>
                                                                    <%#Eval("Rate")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Token Value" SortExpression="Token Value">
                                                                <ItemTemplate>
                                                                    <%#Eval("TokenVal")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>

