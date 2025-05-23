<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FundWithdraw.aspx.cs" Inherits="FundWithdraw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <script type="text/javascript">
        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=GvData.ClientID%>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell 
                            //checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>
    <style>
        /* Define a class for adding space between buttons */
        .button-space {
            margin-right: 10px; /* Adjust this value to set the desired space */
        }

        /* Define media query for mobile devices */
        @media only screen and (max-width: 500px) {
            /* Adjust button style for mobile view */
            .btn {
                margin-bottom: 5px; /* Add some space between buttons vertically */
                width: 45%; /* Make buttons full-width on mobile devices */
            }

            .button-space {
                margin-right: 0; /* Remove right margin for buttons on mobile devices */
            }
        }
    </style>
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
                                <h3 class="card-title">Bank Withdrawal </h3>
                            </div>

                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        Member ID :
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        Wallet Address :
     <asp:TextBox ID="TxtWalletAddress" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        From Date :
                                <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-3">
                                        To Date :
                              <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="col-md-3">
                                        Status :
                                        <asp:DropDownList ID="RbtStatus" runat="server" class="form-control" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="RbtStatus_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="N" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Approve" Value="A"></asp:ListItem>
                                            <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div id="DivTopup" runat="server" visible="false" style="width: 100%">
                                        <center>
                                            <table align="center" border="1px" style="background-color: #394a59; color: #d0d8df;">
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtRemark"
                                                            ErrorMessage="Remark Not Found, Check?" Font-Bold="True" ForeColor="Maroon" ValidationGroup="confirm"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <strong>IDNo</strong>*
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="TxtIDNo" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                                                        <asp:Label ID="lblID" runat="server" Visible="false"></asp:Label>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <strong>Name</strong>*
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="TxtName" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                                                        <asp:Label ID="LblFDate" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="LblTdate" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="LblMobil" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="LnblWeek" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="LblForm1" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="LabelDate1" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <strong>Amount</strong>*
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="TxtAmount" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <strong>Payment Type</strong>
                                                    </td>
                                                    <td align="left">
                                                        <asp:RadioButtonList ID="RbtPayment" runat="server" RepeatDirection="Horizontal"
                                                            RepeatLayout="Flow" AutoPostBack="true">
                                                            <asp:ListItem Text="Cash" Value="C" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Cheque" Value="Q"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <div id="BankDetail" runat="server" visible="false">
                                                    <tr>
                                                        <td align="left">
                                                            <strong>Bank Name</strong>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="DDlBank" runat="server" Width="200px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <strong>Branch Name</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtBranchName" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <strong>IFSC Code</strong>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TxtIFSCode" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <strong>Account Number</strong>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtAccount" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <strong>Cheque Number </strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtCheque" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <strong>Cheque Date</strong>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtChequeDate" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtChequeDate"
                                                                Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtChequeDate"
                                                                ErrorMessage="Invalid Cheque Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                </div>
                                                <tr>
                                                    <td align="left">
                                                        <strong>Remark</strong>*
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="TxtRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trcommand" runat="server">
                                                    <td style="height: 26px"></td>
                                                    <td style="height: 26px" align="left">
                                                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" class="buttonBG" Style="height: 24px; width: 64px;"
                                                            ValidationGroup="confirm" />
                                                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" class="buttonBG" Style="height: 24px; width: 64px;" />
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                    <div id="DivRemark" runat="server" visible="false">
                                        <table id="TblRemark" runat="server" align="center" style="background-color: #394a59; color: #d0d8df; border-color: Black; border-width: 1px; margin-top: -10px;">
                                            <tr>
                                                <td align="left">
                                                    <strong>Remark</strong>*
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td>
                                                    <asp:Button ID="btnApprove" runat="server" class="buttonBG" Text="Approve" Visible="false" />
                                                    <asp:Button ID="btnReject" runat="server" Text="Reject " class="ButtonBG" Visible="false" />
                                                    <asp:Button ID="btnRejectSingle" runat="server" Text="Reject" runtat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblRejIdNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="LblRejWeek" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRejFormno" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="LblRejDateOn" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="LblRejReqNo" runat="server" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-md-3" style="padding: 18px;">
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" OnClick="BtnShow_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click" />
                                    </div>
                                </div>
                                <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px; color: Red"></asp:Label>

                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <div id="gvContainers" runat="server">
                                        <div class="table-responsive">

                                            <div style="margin-top: 20px; margin-bottom: 20px;">

                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <br />
                                                <asp:Label ID="Lblamount" runat="server" Style="font-weight: bold; font-size: 18px; color: Black; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="lbladmincharge" runat="server" Style="font-weight: bold; font-size: 18px; color: Black; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="lbltdscharge" runat="server" Style="font-weight: bold; font-size: 18px; color: Black; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="Lblnetamount" runat="server" Style="font-weight: bold; font-size: 18px; color: Black; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="LblCredit" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="lblDebit" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="LblBalance" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                            </div>
                                            <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px; margin-bottom: 25px;">
                                                <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="true" GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                    EmptyDataText="No data to display." AutoGenerateColumns="false" AllowSorting="true" PageSize="10" OnPageIndexChanging="GvData_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="CheckAll">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# Convert.ToBoolean(Eval("IsVisible")) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SNo.">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblWeek" runat="server" Text='<%# Eval("WeekNo") %>'></asp:Label>
                                                                <asp:Label ID="LblID" runat="server" Text='<%# Eval("ReqID") %>'></asp:Label>
                                                                <asp:Label ID="LblMobl" runat="server" Visible="false" Text='<%# Eval("Mobl") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FormNo" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblFormNo" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Request ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblDated" runat="server" Text='<%# Eval("ReqID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Withdrawal Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblDate" runat="server" Text='<%# Eval("WithDrawDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblIDNo" runat="server" Text='<%# Eval("MemberID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Member Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Lblmembername" runat="server" Text='<%# Eval("MemFirstName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Payee Name" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblPayeeName" runat="server" Text='<%# Eval("PayeeName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="Requested BHTS">--%>
                                                        <asp:TemplateField HeaderText="Requested USDT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="BHTS Rate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdminCharge" runat="server" Text='<%# Eval("CoinRate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      <%--  <asp:TemplateField HeaderText="USDT">--%>
                                                          <asp:TemplateField HeaderText="BHTS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNetAmount" runat="server" Text='<%# Eval("DeductForProductWalletFinal") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="PanNo" HeaderText="BHTS Address" />
                                                        <asp:BoundField DataField="txnhash" HeaderText="Txn Hash" />
                                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtRemarks" runat="server" Style="display: inline" Text='<%# Eval("Remark") %>'
                                                                    Enabled='<%# Convert.ToBoolean(Eval("IsVisible")) %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <%-- <asp:BoundField DataField="ProcessedBy" HeaderText="Processed By" />
                                                        <asp:BoundField DataField="ProcessedDate" HeaderText="Processed Date" />
                                                        <asp:BoundField DataField="WeekNo" HeaderText="Week No." Visible="false" />--%>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" />
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>
                                            </div>
                                        </div>
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

