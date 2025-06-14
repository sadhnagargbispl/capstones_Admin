<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddBvpoint.aspx.cs" Inherits="AddBvpoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <%-- <style type="text/css">
        p {
            font-weight: bold;
            color: #666666;
            margin: 0px;
            line-height: 25px;
            width: 400px;
            padding-bottom: 8px;
            text-align: left;
        }
    </style>--%>
    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Add Virtual Power</h3>
                            </div>
                            <div class="card-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                     <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                </div>
                                <div class="row">
                                    <%--<div class="col-md-4">--%>
                                    <div class="form-group">
                                        <asp:Label ID="LblAmount" runat="server" CssClass="label-text"></asp:Label>
                                        <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;"></span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                    <div class="col-md-6">
                                        <label>Member ID : </label>
                                        <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" OnTextChanged="TxtIDNo_TextChanged" AutoPostBack="true"></asp:TextBox><br />
                                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                         <asp:Label ID="TxtFormNo" runat="server" CssClass="label-text" Visible="false"></asp:Label>
                                        <%--<asp:TextBox ID="TxtFormNo" runat="server" CssClass="TxtBox" Visible="false"></asp:TextBox>--%>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                            ControlToValidate="TxtIDNo" ValidationGroup="Save" Display="Dynamic" ></asp:RequiredFieldValidator>
                                    </div>
                               
                                    <div class="col-md-6" style=" display: none;" >
                                        <label>Income Type:</label>
                                        <asp:DropDownList ID="rbtranktype" RepeatLayout="Flow" runat="server" class="form-control" ValidationGroup="Save" AutoPostBack="true" OnSelectedIndexChanged="rbranktype_SelectedIndexChanged">
                                            <asp:ListItem Value="Rank" >Rank</asp:ListItem>
                                            <asp:ListItem Value="Reward" Selected="True" >Reward</asp:ListItem>
                                            <asp:ListItem Value="Salary">Salary</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6">
                                        <label>BV Type:</label>
                                        <asp:DropDownList ID="RbtType" RepeatLayout="Flow" runat="server" class="form-control" ValidationGroup="Save" AutoPostBack="true"  OnSelectedIndexChanged="RbtType_SelectedIndexChanged">
                                            <asp:ListItem Text="Self" Value="S" Selected="True">Self</asp:ListItem>
                                            <asp:ListItem Text="Tree" Value="V">Tree</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6" style="padding: 10px;">
                                        <label>Leg No:</label>
                                        <asp:DropDownList ID="RbtLeg" RepeatLayout="Flow" runat="server" class="form-control">
                                            <asp:ListItem Text="Left" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6">
                                        <label>BV Point:</label>
                                        <asp:TextBox ID="TxtFund" runat="server" class="form-control" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter BV Point."
                                            ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                        <label>Remarks :</label>
                                        <asp:TextBox ID="TxtRemarks" runat="server" class="form-control" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remrks"
                                            ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Add BV" OnClientClick="return confirmation();"
                                            class="btn btn-primary" ValidationGroup="Save" OnClick="BtnFundTransfer_Click" />
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

