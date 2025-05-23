using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
public partial class FundWithdraw : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    public string formNo;
    string sql = "";
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //btnviapprove.Attributes.Add("onclick", DisableTheButton(Page, btnviapprove));
            //BtnApproveAll.Attributes.Add("onclick", DisableTheButton(Page, BtnApproveAll));
            //btnRejectAll.Attributes.Add("onclick", DisableTheButton(Page, btnRejectAll));
            if (!IsPostBack)
            {
                txtMemId.Text = "";
                GvData.Visible = false;
                btnExport.Enabled = false;
                if (Session["AStatus"] != null)
                {

                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox ChkSelectAll = (CheckBox)e.Row.FindControl("ChkSelectAll");
                ChkSelectAll.Attributes.Add("onclick", "javascript:SelectAll('" + ChkSelectAll.ClientID + "')");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please wait...';");
            sb.Append("this.disabled = true;");
            sb.Append(pge.Page.GetPostBackEventReference(btn));
            sb.Append(";");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillDetail()
    {
        try
        {
            string idno = "";
            string WalletAddress = "";
            string startDate;
            string endDate;
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");
            idno = !string.IsNullOrWhiteSpace(txtMemId.Text) ? txtMemId.Text.Trim() : "";
            WalletAddress = !string.IsNullOrWhiteSpace(TxtWalletAddress.Text) ? TxtWalletAddress.Text.Trim() : "";
            if (string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                startDate = "12-oct-2017";
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                endDate = formattedDate;
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            string sql = objDAL.IsoStart + "exec Sp_FundWithReportNew '" + idno + "','" + startDate + "','" + endDate + "','" + RbtStatus.SelectedValue + "','N','" + WalletAddress + "'" + objDAL.IsoEnd;
            dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = dtData;

            GvData.DataBind();
            Session["GData"] = dtData;
            GvData.Visible = true;
            if (dtData.Rows.Count > 0)
            {
                lblCount.Text = "Total: " + dtData.Rows.Count;
                Lblamount.Text = "Amount: " + dtData.Compute("Sum(Amount)", "");
                lbladmincharge.Text = "Admin charge: " + dtData.Compute("Sum(AdminCharge)", "");
                Lblnetamount.Text = "Net Amount: " + dtData.Compute("Sum(NetAmount)", "");
                btnExport.Enabled = true;
            }
            else
            {
                btnExport.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillDetail();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            string WalletAddress = "";
            string idno = "";
            string startDate;
            string endDate;
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");
            if (!string.IsNullOrWhiteSpace(txtMemId.Text))
            {
                idno = txtMemId.Text.Trim();
            }
            else
            {
                idno = "";
            }
            if (!string.IsNullOrWhiteSpace(TxtWalletAddress.Text))
            {
                WalletAddress = TxtWalletAddress.Text.Trim();
            }
            else
            {
                WalletAddress = "";
            }
            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = "12-oct-2017";
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = formattedDate;
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            string sql = objDAL.IsoStart + "exec Sp_FundWithReportNew '" + idno + "','" + startDate + "','" + endDate + "','" + RbtStatus.SelectedValue + "','Y','" + WalletAddress + "'" + objDAL.IsoEnd;
            dtTemp = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            dg.DataSource = dtTemp;
            dg.DataBind();
            ExportToExcel("WithdrawalReport.xls", dg);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }

    }
    private void ExportToExcel(string strFileName, DataGrid dg)
    {
        try
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.Charset = "";
            dg.EnableViewState = false;
            dg.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //protected void BtnApproveAll_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox Chk;
    //        int cnt = 0;
    //        int cnt1 = 0;
    //        int updateeffect;
    //        Label LblId = new Label();
    //        Label LblMobl = new Label();
    //        Label LblFromDate = new Label();
    //        Label LblTodate = new Label();
    //        TextBox txtRemark = new TextBox();
    //        foreach (GridViewRow Gvr in GvData.Rows)
    //        {
    //            Chk = (CheckBox)Gvr.FindControl("chkSelect");
    //            if (Chk.Checked)
    //            {
    //                LblId.Text = ((Label)Gvr.FindControl("LblID")).Text;
    //                TxtIDNo.Text = ((Label)Gvr.FindControl("LblIDNo")).Text;
    //                TxtName.Text = ((Label)Gvr.FindControl("LblPayeeName")).Text;
    //                TxtAmount.Text = ((Label)Gvr.FindControl("LblAmount")).Text;
    //                LblMobl.Text = ((Label)Gvr.FindControl("Lblmobl")).Text;
    //                LnblWeek.Text = ((Label)Gvr.FindControl("LblWeek")).Text;
    //                txtRemark.Text = ((TextBox)Gvr.FindControl("TxtRemarks")).Text;
    //                string Id = ((Label)Gvr.FindControl("LblID")).Text;
    //                string FormNo = ((Label)Gvr.FindControl("LblFormNo")).Text;
    //                string DateOn = ((Label)Gvr.FindControl("LblDate")).Text;
    //                string Remark = "";

    //                Remark = "Withdrawal Approved Of Idno " + TxtIDNo.Text + " For WeekNo:" + LnblWeek.Text + " By " + Session["UserName"];

    //                string Sql = "https://pay.cryptpayapi.com/PaymentResponse?withdrawl=" + Base64Encode(LblId.Text.Trim());
    //                string sResponseFromServer = string.Empty;

    //                HttpWebRequest tRequest;
    //                Stream dataStream;

    //                tRequest = (HttpWebRequest)WebRequest.Create(Sql);
    //                HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse();
    //                dataStream = tResponse.GetResponseStream();
    //                StreamReader tReader = new StreamReader(dataStream);
    //                sResponseFromServer = tReader.ReadToEnd();

    //                DataSet ds = new DataSet();
    //                ds = convertJsonStringToDataSet(sResponseFromServer);

    //                if (ds.Tables[0].Rows[0]["response"].ToString().ToUpper() == "SUCCESS")
    //                {
    //                    Sql = "Update FundWithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" + txtRemark.Text + "',UserId='" + Session["UserID"] + "',UserName='" + Session["UserName"] + "' Where ReqID=" + LblId.Text + ";" +
    //                          "Update TrnVoucher Set Userid='" + Session["UserId"] + "' Where DrTo='" + FormNo + "' And vtype='W' And VoucherDate='" + DateOn + "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " + DateOn + " with req. no." + Id + "';" +
    //                          " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
    //                          "('" + Session["UserID"] + "','" + Session["UserName"] + "','Fund WithDrawals','Fund Withdrawls Approve','" + Remark + "',Getdate(),'" + FormNo + "')";

    //                    cnt1++;
    //                }
    //                else
    //                {
    //                    Sql = " Update FundWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" + txtRemark.Text + "',UserId='" + Session["UserID"] + "',UserName='" + Session["UserName"] + "' Where ReqID=" + Id + ";" +
    //                          " INSERT INTO RejtVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID,Status,Rn) " +
    //                          "  SELECT 0,'" + DateTime.Now.ToString("dd-MMM-yyyy") + "',0, '" + FormNo + "',Amount," +
    //                          "  Replace(Replace(Narration,'Debited','Credited'),'with ','(Reject) with '),'" + Id + "/" + FormNo + "','M','C',convert(varchar,getdate(),112)," +
    //                          "  (Select Max(SessID) from M_SessnMaster),'R',Row_Number() Over(Order by voucherid) FROM TrnVoucher  Where Drto =  '" + FormNo + "' And refno = '" + Id + "/" + FormNo + "'";

    //                    cnt++;
    //                }
    //                updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
    //                cnt++;
    //                if (updateeffect != 0)
    //                {
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + cnt1 + "Withdrawal Approved and " + cnt + " Withdrawal Rejected Successfully.');", true);
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Server Timeout, Try After Some Time.');", true);
    //                }
    //                FillDetail();
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
    //    }
    //}
    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        try
        {
            XmlDocument xd = new XmlDocument();
            jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(xd));
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static string Base64Encode(string plainText)
    {
        try
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //protected void btnRejectAll_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox Chk;
    //        int cnt = 0;
    //        string msg = string.Empty;
    //        int updateeffect = 0;
    //        Label LblId = new Label();
    //        string sql = "";
    //        Label LblMobl = new Label();
    //        Label LblFromDate = new Label();
    //        Label LblTodate = new Label();
    //        string Remark = "";

    //        foreach (GridViewRow Gvr in GvData.Rows)
    //        {
    //            Chk = (CheckBox)Gvr.FindControl("chkSelect");
    //            if (Chk.Checked)
    //            {
    //                string Id = ((Label)Gvr.FindControl("LblID")).Text;
    //                string FormNo = ((Label)Gvr.FindControl("LblFormNo")).Text;
    //                string DateOn = ((Label)Gvr.FindControl("LblDate")).Text;
    //                string IdNo = ((Label)Gvr.FindControl("LblIDNo")).Text;
    //                string WeekNo = ((Label)Gvr.FindControl("LblWeek")).Text;
    //                string TxtRemark = ((TextBox)Gvr.FindControl("TxtRemarks")).Text;
    //                Remark = "Withdrawal Rejected Of Idno " + IdNo + " for WeekNo:" + WeekNo + " By " + Session["UserName"] + "";
    //                sql = "Update FundWithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" + TxtRemark + "',UserId='" + (Session["UserID"]) + "',UserName='" + Session["UserName"] + "' Where ReqID=" + Id + ";";
    //                sql += " INSERT INTO RejtVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID,Status,Rn) ";
    //                sql += "  SELECT 0,'" + DateTime.Now.ToString("dd-MMM-yyyy") + "',0, '" + FormNo + "',Amount,";
    //                sql += "  Replace(Replace(Narration,'Debited','Credited'),'with ','(Reject) with '),'" + Id + "/" + FormNo + "','M','C',convert(varchar,getdate(),112),";
    //                sql += "  (Select Max(SessID) from M_SessnMaster),'R',Row_Number() Over(Order by voucherid) FROM TrnVoucher  Where Drto =  '" + FormNo + "' And refno = '" + Id + "/" + FormNo + "';";

    //                updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql));
    //                cnt++;
    //            }
    //        }
    //        if (updateeffect != 0)
    //        {
    //            msg = cnt.ToString() + " Withdrawal Rejected Successfully.!";
    //            FillDetail();
    //        }
    //        else
    //        {
    //            msg = "Server Timeout, Try After Some Time.!";
    //        }
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + msg + "');", true);
    //        FillDetail();
    //        DivRemark.Visible = false;
    //    }
    //    catch (Exception Ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
    //    }
    //}
    //protected void btnviapprove_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ViAprvAction("Y", "A");
    //    }
    //    catch (Exception Ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
    //    }
    //}
    //private void ViAprvAction(string AprvType, string ApprvStatus)
    //{
    //    try
    //    {
    //        CheckBox Chk;
    //        string Msg = string.Empty;
    //        int cnt = 0;
    //        int cnt1 = 0;
    //        int updateeffect = 0;
    //        Label LblId = new Label();
    //        Label LblMobl = new Label();
    //        Label LblFromDate = new Label();
    //        Label LblTodate = new Label();
    //        TextBox txtRemark = new TextBox();

    //        foreach (GridViewRow Gvr in GvData.Rows)
    //        {
    //            Chk = (CheckBox)Gvr.FindControl("chkSelect");
    //            if (Chk.Checked)
    //            {
    //                LblId.Text = ((Label)Gvr.FindControl("LblID")).Text;
    //                TxtIDNo.Text = ((Label)Gvr.FindControl("LblIDNo")).Text;
    //                TxtName.Text = ((Label)Gvr.FindControl("LblPayeeName")).Text;
    //                TxtAmount.Text = ((Label)Gvr.FindControl("LblAmount")).Text;
    //                LblMobl.Text = ((Label)Gvr.FindControl("Lblmobl")).Text;
    //                LnblWeek.Text = ((Label)Gvr.FindControl("LblWeek")).Text;
    //                txtRemark.Text = ((TextBox)Gvr.FindControl("TxtRemarks")).Text;
    //                string Id = ((Label)Gvr.FindControl("LblID")).Text;
    //                string FormNo = ((Label)Gvr.FindControl("LblFormNo")).Text;
    //                string DateOn = ((Label)Gvr.FindControl("LblDate")).Text;
    //                string Remark = "Withdrawal Approved Of Idno " + TxtIDNo.Text + " For WeekNo:" + LnblWeek.Text + " By " + Session["UserName"];
    //                sql = "Update FundWithdrawls Set Status='" + ApprvStatus + "',IssueDate=GETDATE(),UserId='" + (Session["UserID"]) + "',UserName='" + Session["UserName"] + "' Where ReqID=" + LblId.Text + ";" +
    //                      " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
    //                      "('" + (Session["UserID"]) + "','" + Session["UserName"] + "','Fund WithDrawals','Fund Withdrawls Virtual Approve','" + Remark + "',Getdate(),'" + FormNo + "')";
    //                cnt1++;
    //                updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql));
    //                cnt++;
    //            }
    //        }
    //        if (updateeffect != 0)
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + cnt1 + "Withdrawal Virtual Approved Successfully.!');", true);
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Server Timeout, Try After Some Time.!');", true);
    //        }
    //        FillDetail();
    //    }
    //    catch (Exception Ex)
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
    //    }
    //}
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            // Rebind the data to the GridView
            FillDetail();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }


    protected void RbtStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDetail();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
}

