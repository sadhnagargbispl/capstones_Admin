<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <div class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1 class="m-0">Dashboard</h1>
                    </div>
                    <!-- /.col -->
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="Home.aspx">Home</a></li>

                        </ol>
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->
            </div>
            <!-- /.container-fluid -->
        </div>
        <!-- /.content-header -->

        <!-- Main content -->
        <section class="content">


            <div class="container-fluid">
                <!-- Small boxes (Stat box) -->
                <div class="row">
                    <div class="col-md-8">
                        <div class="row">
                            <asp:Repeater runat="server" ID="RepMemberData">
                                <ItemTemplate>
                                    <div class="col-lg-4 col-12">
                                        <!-- small box -->
                                        <div class="small-box bg-primary">
                                            <div class="inner">
                                                <h3><%#Eval("cnt")%></h3>

                                                <p><%#Eval("Name")%></p>
                                            </div>
                                            <div class="icon">
                                                <i class="ion ion-person-add"></i>
                                            </div>
                                            <a href="#" class="small-box-footer">&nbsp;</a>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-8">
                        <div class="row">
                            <asp:Repeater runat="server" ID="RepWallet">
                                <ItemTemplate>
                                    <div class="col-lg-4 col-12">
                                        <!-- small box -->
                                        <div class="small-box bg-info">
                                            <div class="inner">
                                                <h3><%#Eval("Amount")%></h3>

                                                <p><%#Eval("Name")%></p>
                                            </div>
                                            <div class="icon">
                                                <i class="ion ion-stats-bars"></i>
                                            </div>
                                            <a href="#" class="small-box-footer">&nbsp;</a>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-8">
                        <div class="row">
                            <asp:Repeater runat="server" ID="RepFundWithrawal">
                                <ItemTemplate>
                                    <div class="col-lg-4 col-12">
                                        <!-- small box -->
                                       <div class="small-box bg-danger">
                                            <div class="inner">
                                                <h3><%#Eval("Amount")%></h3>

                                                <p><%#Eval("Name")%></p>
                                            </div>
                                            <div class="icon">
                                                <i class="ion ion-stats-bars"></i>
                                            </div>
                                            <a href="#" class="small-box-footer">&nbsp;</a>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <!-- /.row -->
                <!-- Main row -->

            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
</asp:Content>

