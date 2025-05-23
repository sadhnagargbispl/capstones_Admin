<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CoinMaster.aspx.cs" Inherits="CoinMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <%--   <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/Coin.css" rel="stylesheet" />
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Coin Master
                                </h3>
                            </div>
                            <div class="card-body">
                                <div align="center">
                                    <div class="col-md-12">
                                        <br />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click" />
                                        <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add Coin Rate" OnClick="BtnAdd_Click" />

                                        <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                            Visible="false" />
                                    </div>
                                    <br />
                                    <br />
                                    <div style="margin-bottom: 20px">


                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered"
                                            HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">

                                            <Columns>
                                                <asp:TemplateField HeaderText="GrpID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("CoinID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CoinRate" HeaderText="Coin Rate" ControlStyle-Width="50px"
                                                    SortExpression="Coinrate" />

                                                <asp:BoundField DataField="CoinDate" HeaderText="Coin Date" ControlStyle-Width="50px"
                                                    SortExpression="Coindate" />
                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("Status") %>' class='<%# Eval("StatusClass") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

