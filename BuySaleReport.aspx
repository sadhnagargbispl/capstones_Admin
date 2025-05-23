<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BuySaleReport.aspx.cs" Inherits="BuySaleReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Buy Sell Report</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-3">

                                        <asp:Label ID="Label1" runat="server" Text="Address: "></asp:Label>
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>
                                    <asp:HiddenField ID="HdnApiRate" runat="server" />
                                    <div class="col-md-3">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="col-md-3">
                                        <asp:Label ID="lblEndDate" runat="server" Text="End Date : "></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-3">
                                        Buy / Sell
                                      <asp:DropDownList ID="ddllist" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddllist_SelectedIndexChanged">
                                          <asp:ListItem Value="B" Selected="True">Buy</asp:ListItem>
                                          <asp:ListItem Value="S">Sell</asp:ListItem>
                                      </asp:DropDownList>
                                    </div>

                                    <div class="col-md-3">
                                        <br />
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" OnClick="BtnShow_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click" />
                                    </div>
                                </div>
                                <div id="doublescroll" class="col-md-12">
                                    <p>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblError" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblinv" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging" Visible="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="SNo.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TransactionId" Visible="false" SortExpression="TransactionId">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="HdnBHTSAmount" runat="server" Text='<%# Eval("BHTS Amount") %>'></asp:Label>
                                                                    <asp:Label ID="HdnBHTSTXNHash" runat="server" Text='<%# Eval("BHTS TXN Hash") %>'></asp:Label>
                                                                    <asp:Label ID="HdnFromAddress" runat="server" Text='<%# Eval("From Address") %>'></asp:Label>
                                                                    <asp:Label ID="HdnToAddress" runat="server" Text='<%# Eval("To Address") %>'></asp:Label>
                                                                    <asp:Label ID="HdnUSDTAmount" runat="server" Text='<%# Eval("USDT Amount") %>'></asp:Label>
                                                                    <asp:Label ID="HdnUSDTTXNHash" runat="server" Text='<%# Eval("USDT TXN Hash") %>'></asp:Label>
                                                                    <asp:Label ID="Hdnformno" runat="server" Text='<%# Eval("formno") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                                            <asp:BoundField DataField="From Address" HeaderText="From Address" SortExpression="From Address" />
                                                            <asp:BoundField DataField="To Address" HeaderText="To Address" SortExpression="To Address" />
                                                            <asp:BoundField DataField="BHTS Amount" HeaderText="BHTS Amount" SortExpression="BHTS Amount" />
                                                            <asp:BoundField DataField="BHTS TXN Hash" HeaderText="BHTS TXN Hash" SortExpression="BHTS TXN Hash" />
                                                            <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" />
                                                            <asp:BoundField DataField="USDT Amount" HeaderText="USDT Amount" SortExpression="USDT Amount" />
                                                            <asp:BoundField DataField="USDT TXN Hash" HeaderText="USDT TXN Hash" SortExpression="USDT TXN Hash" />
                                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                                            <asp:TemplateField HeaderText="Send" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"
                                                                        Visible='<%# Convert.ToBoolean(Eval("VisibleStatus")) %>'
                                                                        ForeColor="Black">
                                                                                 Click Here Transfer To USDT
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="85px"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GridView1_PageIndexChanging" Visible="False">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="SNo.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TransactionId" Visible="false" SortExpression="TransactionId">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="HdnBHTSAmount" runat="server" Text='<%# Eval("USDT Amount") %>'></asp:Label>
                                                                    <asp:Label ID="HdnBHTSTXNHash" runat="server" Text='<%# Eval("USDT TXN Hash") %>'></asp:Label>
                                                                    <asp:Label ID="HdnFromAddress" runat="server" Text='<%# Eval("From Address") %>'></asp:Label>
                                                                    <asp:Label ID="HdnToAddress" runat="server" Text='<%# Eval("To Address") %>'></asp:Label>
                                                                    <asp:Label ID="HdnUSDTAmount" runat="server" Text='<%# Eval("BHTS Amount") %>'></asp:Label>
                                                                    <asp:Label ID="HdnUSDTTXNHash" runat="server" Text='<%# Eval("BHTS TXN Hash") %>'></asp:Label>
                                                                    <asp:Label ID="Hdnformno" runat="server" Text='<%# Eval("formno") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                                            <asp:BoundField DataField="From Address" HeaderText="From Address" SortExpression="From Address" />
                                                            <asp:BoundField DataField="To Address" HeaderText="To Address" SortExpression="To Address" />
                                                            <asp:BoundField DataField="USDT Amount" HeaderText="USDT Amount" SortExpression="USDT Amount" />
                                                            <asp:BoundField DataField="USDT TXN Hash" HeaderText="USDT TXN Hash" SortExpression="USDT TXN Hash" />
                                                            <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" />
                                                            <asp:BoundField DataField="BHTS Amount" HeaderText="BHTS Amount" SortExpression="BHTS Amount" />
                                                            <asp:BoundField DataField="BHTS TXN Hash" HeaderText="BHTS TXN Hash" SortExpression="BHTS TXN Hash" />
                                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                                            <asp:TemplateField HeaderText="Send" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"
                                                                        Visible='<%# Convert.ToBoolean(Eval("VisibleStatus")) %>'
                                                                        ForeColor="Black">
                                                           Click Here Transfer To BHTS
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="85px"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
