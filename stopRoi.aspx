<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="stopRoi.aspx.cs" Inherits="stopRoi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Stop Income</h3>
                            </div>
                            <div class="panel-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <strong>Member ID <span style="color: orangered;">*</span> :</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtIdno" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="txtIdno_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Idno."
                                            ControlToValidate="txtIdno" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <strong>Member Name <span style="color: orangered;">*</span> :</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="LblMobile" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblemail" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblstatus" runat="server" Visible="True"></asp:Label>
                                        <asp:TextBox ID="TxtMemberName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <strong>Select Income Name <span style="color: orangered;">*</span> :</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="DDlIncomeType" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlIncomeType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Please Select Income Name"
                                            ControlToValidate="DDlIncomeType" InitialValue="0" runat="server" ForeColor="Red"
                                            ValidationGroup="Save" />
                                    </div>
                                </div>
                                <%--<div class="col-md-12">
                                    <div class="col-md-2">
                                        <strong>Select <span style="color: orangered;">*</span> :</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="SelectRoi" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="SelectRoi_SelectedIndexChanged">
                                            <asp:ListItem Text="---Select Type---" Value="0" Selected="True">---Select Type---</asp:ListItem>
                                            <asp:ListItem Text="Self" Value="S">Self</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Please Select Any Type"
                                            ControlToValidate="SelectRoi" InitialValue="0" runat="server" ForeColor="Red"
                                            ValidationGroup="Save" />
                                    </div>
                                </div>--%>
                                <div class="col-md-12">
                                    <div class="col-md-2">
                                        <strong>Current Status <span style="color: orangered;">*</span> :</strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtstatus" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12" style="margin-bottom: 1%">
                                    <div class="col-md-2">
                                        <strong></strong>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:HiddenField ID="hdnFormno" runat="server" />
                                        <asp:Button ID="BtnSubmit" runat="server" Text="Save" class="btn btn-primary" ValidationGroup="Save" OnClick="BtnSubmit_Click" />
                                        <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" class="btn btn-danger" OnClick="Btn_Cancel_Click" />
                                    </div>
                                </div>
                                <div style="padding: 10px 10px 20px 10px" id="divDetail" runat="server">
                                    <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="false" GridLines="Both"
                                        class="table table-bordered" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                        EmptyDataText="No data to display." AutoGenerateColumns="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

