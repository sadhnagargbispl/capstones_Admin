using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class bankverified : System.Web.UI.Page
{
    string scrname = "";
    DataTable Dt = new DataTable();
    DAL Objdal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BtnVerifiy.Attributes.Add("onclick", DisableTheButton(Page, BtnVerifiy));
            BTnUnVerification.Attributes.Add("onclick", DisableTheButton(Page, BTnUnVerification));
           
            if (!Page.IsPostBack)
            { 
                if (Session["AStatus"] != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["key"]) && Request.QueryString["key"] != null)
                    {
                        if (Request.QueryString["key"] != "" && Request.QueryString["key"] != null)
                        {
                            txtMemId.Text = Request.QueryString["key"];
                            BindData(" AND IDNo='" + Request.QueryString["key"] + "'");
                        }
                    }
                    else
                    {
                        BindData();
                    }
                }
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
            sb.Append("this.value = 'Please Wait...';");
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
    public void BindData(string Condition = "")
    {
        try
        {
            {
                if (!string.IsNullOrEmpty(txtMemId.Text))
                {
                    Condition += " AND IDNo='" + txtMemId.Text.Trim() + "'";
                }
            }

            if (DDlVerify.SelectedValue != "S")
            {
                Condition += " And c.IsBankVerified='" + DDlVerify.SelectedValue + "'  ";
            }

            if (RbtSearch.SelectedValue != "A")
            {
                Condition += " And a.ActiveStatus='" + RbtSearch.SelectedValue + "'  ";
            }

            string sql = Objdal.IsoStart+"Select Cast(a.FormNo as varchar) as FormNo, Replace(CONVERT(varchar, Doj, 106), ' ', '-') as Doj, " +
                         "Case when a.Activestatus='Y' then Replace(CONVERT(varchar, UpgradeDate, 106), ' ', '-') Else '' End as ActivationDate, " +
                         "a.IDNo, RTRIM(a.MemFirstName + ' ' + a.MemLastName) as MemName, b.Bankname, a.Acno, a.Branchname, a.Ifscode, a.panno, " +
                         "CASE WHEN c.IsBankVerified='Y' THEN 'Verified' when c.IsBankVerified='R' then 'Rejected' Else 'Verification Due' END AS BankVerf, " +
                         "case when c.BankProof <> '' then Replace(CONVERT(varchar, c.BankProofDate, 106), ' ', '-') else '' end as BankProofDate, " +
                         "CASE WHEN c.AddrProof <> '' Then c.AddrProof ELSE '" + Session["CompWeb"].ToString() + "/Images/no_photo.jpg' END AS BankProofStatus, " +
                         "Case when c.IsBankVerified='N' then '' else Replace(convert(Varchar, c.BankVerifyDate, 106), ' ', '-') end as BankVerifyDate, " +
                         "Case when c.IsBankVerified='Y' then 'False' else 'True' end as EnableStatus, " +
                         "Case when c.IsBankVerified <> 'R' then '' else c.BankProofRemark end as RejectRemark, " +
                         "CASE WHEN c.IsAddrssVerified='Y' THEN 'Verified' when c.IsPanVerified='R' then 'Rejected' Else 'Verification Due' END AS PanVerf, " +
                         "case when c.PanImg <> '' then Replace(CONVERT(varchar, c.PanVerifyDate, 106), ' ', '-') else '' end as PanProofDate, " +
                         "CASE WHEN c.BackAddressProof <>'' Then c.BackAddressProof ELSE 'https://bighitterss.com//Images/no_photo.jpg' END AS PanProofStatus, " +
                         "Case when c.IsPanVerified='N' then '' else Replace(convert(Varchar, c.PanVerifyDate, 106), ' ', '-') end as PanVerifyDate, " +
                         "Case when c.IsPanVerified='Y' then 'False' else 'True' end as PanEnableStatus, " +
                         "Case when c.IsPanVerified <> 'R' then '' else c.PanRemarks end as PanRejectRemark, " +
                         "Isnull(e.UserName, ' ') as VerifyBy, Isnull(f.Reason, '') as RejectReason " +
                         "From " +Objdal.DBName + "..M_MemberMaster as a " +
                         "Inner Join " +Objdal.DBName + "..M_BAnkMaster as b On a.Bankid = b.Bankcode and b.Rowstatus = 'Y' " +
                         "Inner Join " +Objdal.DBName + "..KYCVerify as c On a.Formno = c.Formno " +
                         "Left Join " +Objdal.DBName + "..M_KycReject as f On c.BankRejectId = f.Kid " +
                         "Left Join " +Objdal.DBName + "..M_Usermaster as e On c.BankUserid = e.Userid and e.RowStatus = 'Y' " +
                         "Where 1 = 1   and c.AddrProof <>''" + Condition + " " + Objdal.IsoEnd;

            Dt =SqlHelper.ExecuteDataset(constr1,CommandType.Text, sql).Tables[0];
            GvData.DataSource = Dt;
            GvData.DataBind();
            Session["GData"] = Dt;

            if (Dt.Rows.Count > 0)
            {
              
                BTnUnVerification.Enabled = true;
                BtnVerifiy.Enabled = true;
                BtnExport.Enabled = true;
            }
            else
            {
              
                BtnVerifiy.Enabled = false;
                BTnUnVerification.Enabled = false;
                BtnExport.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GvData_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("ChkSelectAll");
            if (chkSelectAll != null)
            {
                chkSelectAll.Attributes.Add("onclick", "javascript:SelectAll('" + chkSelectAll.ClientID + "')");
            }
        }
    }
    protected void BtnVerifiy_Click(object sender, EventArgs e)
    {
        try
        {
            string str = "";
            string scrname;
            Label lbl;
            Label LblIdNo;
            CheckBox Chk;
            string Remark = "";
            foreach (GridViewRow Gvr in GvData.Rows)
            {
                Chk = (CheckBox)Gvr.FindControl("chkSelect");
                lbl = (Label)Gvr.FindControl("LblGrpID");
                LblIdNo = (Label)Gvr.FindControl("LblIdno");
                DataTable dtcheck = new DataTable();
                if (Chk.Checked)
                {
                    Remark = "Bank Proof Verify of IdNo:" + LblIdNo.Text;
                    str = "exec sp_BtnVerifiyNew '" + Session["UserID"] + "','" + Session["UserName"] + "','" + Remark + "','" + lbl.Text + "'";
                }
            }
            string Str_Sql =  " Begin Try   Begin Transaction " + str + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
            if (i > 0)
            {
                scrname = "<SCRIPT language='javascript'>alert(' Verified successfully. ');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Image", scrname, false);
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert(' Verified unsuccessfully. ');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Image", scrname, false);
            }

            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnUnVerify_Click(object sender, EventArgs e)
    {
        try
        {
             string str = "";
            string scrname;
            Label lbl;
            CheckBox Chk;
            string Remark = "";
            Label LblIdno;
            if ((TxtARemark.Text).Trim() == "")
            {

                scrname = "<SCRIPT language='javascript'>alert('Enter Remark.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Enter Remark.');", true);
                return;
            }
            foreach (GridViewRow Gvr in GvData.Rows)
            {
                Chk = (CheckBox)Gvr.FindControl("chkSelect");
                lbl = (Label)Gvr.FindControl("LblGrpID");
                LblIdno = (Label)Gvr.FindControl("LblIdno");
                if (Chk.Checked)
                {
                    Remark = "Kyc UnVerify of IdNo:" + LblIdno.Text;
                    str = "exec SP_BtnUnVerify '" + Session["UserID"] + "','" + Session["UserName"] + "','" + Remark + "','" + lbl.Text + "','" + DDlREason.SelectedValue + "'";
                }
            }
            string Str_Sql = "Begin Try   Begin Transaction " + str + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int X = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
            if (X > 0)
                scrname = "<SCRIPT language='javascript'>alert(' UnVerified successfuly. ');" + "</SCRIPT>";
            else
                scrname = "<SCRIPT language='javascript'>alert(' UnVerified unsuccessfuly. ');" + "</SCRIPT>";

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Image", scrname, false);

            DivRemark.Visible = false;
            TxtARemark.Text = "";

            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BTnUnVerification_Click(object sender, EventArgs e)
    {
        try
        {
            DivRemark.Visible = true;
            BtnVerifiy.Enabled = false;
            BTnUnVerification.Enabled = false;
            FillDetail();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void FillDetail()
    {
        try
        {
            string s = Objdal.IsoStart +"Select * from "+ Objdal.DBName +"..M_KycReject where activeStatus='Y'" + Objdal.IsoEnd ;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];
            if (Dt.Rows.Count > 0)
           
            {
                DDlREason.DataValueField = "kId";
                DDlREason.DataTextField = "reason";
                DDlREason.DataSource = Dt;
                DDlREason.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GData"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            string Condition = "";
            {
                if (!string.IsNullOrEmpty(txtMemId.Text))
                {
                    Condition += " AND IDNo='" + txtMemId.Text.Trim() + "'";
                }
            }

            if (DDlVerify.SelectedValue != "S")
            {
                Condition += " And c.IsBankVerified='" + DDlVerify.SelectedValue + "'  ";
            }

            if (RbtSearch.SelectedValue != "A")
            {
                Condition += " And a.ActiveStatus='" + RbtSearch.SelectedValue + "'  ";
            }

            string sql = Objdal.IsoStart + "Select Cast(a.FormNo as varchar) as FormNo, Replace(CONVERT(varchar, Doj, 106), ' ', '-') as Doj, " +
                         "Case when a.Activestatus='Y' then Replace(CONVERT(varchar, UpgradeDate, 106), ' ', '-') Else '' End as ActivationDate, " +
                         "a.IDNo, RTRIM(a.MemFirstName + ' ' + a.MemLastName) as MemName, b.Bankname, a.Acno, a.Branchname, a.Ifscode, a.panno, " +
                         "CASE WHEN c.IsBankVerified='Y' THEN 'Verified' when c.IsBankVerified='R' then 'Rejected' Else 'Verification Due' END AS BankVerf, " +
                         "case when c.BankProof <> '' then Replace(CONVERT(varchar, c.BankProofDate, 106), ' ', '-') else '' end as BankProofDate, " +
                         "CASE WHEN c.AddrProof <> '' Then c.AddrProof ELSE '" + Session["CompWeb"].ToString() + "/Images/no_photo.jpg' END AS BankProofStatus, " +
                         "Case when c.IsBankVerified='N' then '' else Replace(convert(Varchar, c.BankVerifyDate, 106), ' ', '-') end as BankVerifyDate, " +
                         "Case when c.IsBankVerified='Y' then 'False' else 'True' end as EnableStatus, " +
                         "Case when c.IsBankVerified <> 'R' then '' else c.BankProofRemark end as RejectRemark, " +
                         "CASE WHEN c.IsAddrssVerified='Y' THEN 'Verified' when c.IsPanVerified='R' then 'Rejected' Else 'Verification Due' END AS PanVerf, " +
                         "case when c.PanImg <> '' then Replace(CONVERT(varchar, c.PanVerifyDate, 106), ' ', '-') else '' end as PanProofDate, " +
                         "CASE WHEN c.BackAddressProof <>'' Then c.BackAddressProof ELSE 'https://bighitterss.com//Images/no_photo.jpg' END AS PanProofStatus, " +
                         "Case when c.IsPanVerified='N' then '' else Replace(convert(Varchar, c.PanVerifyDate, 106), ' ', '-') end as PanVerifyDate, " +
                         "Case when c.IsPanVerified='Y' then 'False' else 'True' end as PanEnableStatus, " +
                         "Case when c.IsPanVerified <> 'R' then '' else c.PanRemarks end as PanRejectRemark, " +
                         "Isnull(e.UserName, ' ') as VerifyBy, Isnull(f.Reason, '') as RejectReason " +
                         "From BigHit..M_MemberMaster as a " +
                         "Inner Join BigHit..M_BAnkMaster as b On a.Bankid = b.Bankcode and b.Rowstatus = 'Y' " +
                         "Inner Join BigHit..KYCVerify as c On a.Formno = c.Formno " +
                         "Left Join BigHit..M_KycReject as f On c.BankRejectId = f.Kid " +
                         "Left Join BigHit..M_Usermaster as e On c.BankUserid = e.Userid and e.RowStatus = 'Y' " +
                         "Where 1 = 1   and c.AddrProof <>''" + Condition + " " + Objdal.IsoEnd;

            dtTemp = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            dg.DataSource = dtTemp;
            dg.DataBind();

            ExportToExcel("BankProofKYC.xls", ref dg);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "Error In Exporting File");
        }
    }

    private void ExportToExcel(string strFileName, ref DataGrid dg)
    {
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Charset = "";
        dg.EnableViewState = false;

        dg.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
   
}