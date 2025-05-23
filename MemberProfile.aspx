﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MemberProfile.aspx.cs" Inherits="MemberProfile" %>

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
                                <h3 class="card-title">Member Master</h3>
                            </div>
                                                        <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label2" runat="server" Text="Choose Date: "></asp:Label>
                                        <asp:DropDownList ID="CmbType" runat="server" class="form-control">
                                            <asp:ListItem Selected="True" Value="J">Joining Date</asp:ListItem>
                                            <asp:ListItem Value="A">Activation Date</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <label for="exampleInputEmail1">Choose Start Date :</label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <label for="exampleInputEmail1">Choose End Date:</label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package :" TextAlign="Left" />
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>

                                    <%--<div class="col-md-12">--%>
                                    <div class="col-md-3" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkBank" runat="server" Text="Choose Bank :" TextAlign="Left" />
                                        <asp:DropDownList ID="DdlBank" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="ChkMem" runat="server" Text="MemberId :" TextAlign="Left" />
                                        <asp:TextBox ID="txtMember" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        PageSize:
                                             <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                                 <asp:ListItem Text="10" Value="10" />
                                                 <asp:ListItem Text="20" Value="20" />
                                                 <asp:ListItem Text="50" Value="50" />
                                                 <asp:ListItem Text="100" Value="100" />
                                                 <asp:ListItem Text="200" Value="200" />
                                                 <asp:ListItem Text="300" Value="300" />
                                                 <asp:ListItem Text="400" Value="400" />
                                                 <asp:ListItem Text="500" Value="500" />
                                                 <asp:ListItem Text="1000" Value="1000" />
                                                 <asp:ListItem Text="2000" Value="2000" />
                                             </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                    <%-- <asp:Button ID="btnshowall" runat="server" class="btn btn-primary" Text="Show All" />--%>
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click" />
                                    <asp:Button ID="BtnExportCsv" runat="server" class="btn btn-primary" Text="Export To CSV"
                                        Visible="false" />
                                    <asp:Button ID="BtnBankDetail" runat="server" class="btn btn-primary" Text="View Bank Detail "
                                        Visible="false" />
                                    <asp:Button ID="BtnExportBank" runat="server" class="btn btn-primary" Text="Export Bank Detail"
                                        Visible="false" />
                                    <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary"
                                        Visible="false" />
                                    <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary"
                                        Visible="false" />
                                </div>

                                <div class="col-md-12">
                                </div>
                                <div id="doublescroll" class="col-md-12">
                                    <p>
                                        <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>--%>
                                                <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px; color: Red"></asp:Label>
                                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblactive" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lbldeactive" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray"></asp:Label>
                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GvData_PageIndexChanging">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>.
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                         <%--   </ContentTemplate>

                                        </asp:UpdatePanel>--%>
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

