using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WalletReport : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    string IsoStart;
    string IsoEnd;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnShow.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnShow));
            string searchtext = Session["Search"] as string;
            if (!IsPostBack)
            {
                if (Session["AStatus"] != null)
                {
                    GvData.Visible = false;
                    gvContainer.Visible = false;
                    Session["WalletData"] = null;
                    Filldate();
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
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
    private void Filldate()
    {
        try
        {

            string Str = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate ";
            dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Str).Tables[0];
            if (dtData.Rows.Count > 0)
            {
                txtStartDate.Text = dtData.Rows[0]["CurrentDate"].ToString();
                txtEndDate.Text = dtData.Rows[0]["CurrentDate"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string GetFormNo()
    {
        string formno = "";
        try
        {

            string idNo = txtMemberId.Text.Trim();
            string qry = objDAL.IsoStart + "Select FormNo from " + objDAL.DBName + "..M_MemberMaster where IdNo='" + idNo + "'" + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                formno = Dt.Rows[0]["FormNo"].ToString();
            }
            else
            {
                lblErr.Text = "Member Id does not exist. Please check it once and then enter it again.";
                lblErr.Visible = true;
                txtMemberId.Text = "";
            }
        }
        catch (Exception ex)
        {
            return "";
            throw new Exception(ex.Message);

        }
        return formno;
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["WalletData1"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "WalletBalance");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=WalletBalanceReport.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string Idno = "0";
            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                Idno = txtMemberId.Text;
            }
            else
            {
                Idno = "0";
            }
            string str = objDAL.IsoStart + " exec sp_GetWalletBalanceDetail '" + Idno.ToLower() + "','1',10,'Y','" + txtStartDate.Text + "', '" + txtEndDate.Text + "',0" + objDAL.IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str);
            Session["WalletData1"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void BindData(int PageIndex)
    {
        try
        {
            string Idno = "0";
            lblErr.Text = "";
            lblCount.Text = "";
            GvData.DataSource = null;
            GvData.DataBind();

            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                Idno = txtMemberId.Text;
            }
            else
            {
                Idno = "0";
            }
            string str = objDAL.IsoStart + " exec sp_GetWalletBalanceDetail '" + Convert.ToString(Idno).ToLower() + "','" + PageIndex + "',10,'N','" + txtStartDate.Text + "', '" + txtEndDate.Text + "',0" + objDAL.IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str);
            GvData.DataSource = Ds.Tables[0];
            GvData.DataBind();

            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
           

            Session["WalletData"] = Ds.Tables[0];
            ViewState["Sno"] = "Formno";
            ViewState["Sort_Order"] = "ASC";
            if (recordCount > 0)
            {
                int LabelWorking = Convert.ToInt32(Ds.Tables[2].Rows[0]["FundWalletBal"]);
                //int LabelProduct = Convert.ToInt32(Ds.Tables[2].Rows[0]["ProductWalletBal"]);
                int LabelEarning = Convert.ToInt32(Ds.Tables[2].Rows[0]["EarningWalletBal"]);
                //int LabelPoint = Convert.ToInt32(Ds.Tables[2].Rows[0]["PointWalletBal1"]);
                for (int i = 0; i < GvData.Columns.Count; i++)
                {
                    TableCell tableCell = GvData.HeaderRow.Cells[i];
                    Image img = new Image();
                    img.ImageUrl = "~/Images/Uparrow.png";
                    tableCell.Controls.Add(new LiteralControl("&nbsp;"));
                    tableCell.Controls.Add(img);
                }
                GvData.Visible = true;
                gvContainer.Visible = true;
                lblCount.Text = "Total : " + recordCount;

                LabelFundWallet.Text = "Fund Wallet Balance : " + LabelWorking;
                //LabelProductWallet.Text = "Product Wallet Balance  : " + LabelProduct;
                LabelEarningWallet.Text = "Earning Wallet Balance   : " + LabelEarning;
                //LabelPointWallet.Text = "Point Wallet Balance   : " + LabelEarning;
            }
            else
            {
                GvData.Visible = false;
                gvContainer.Visible = false;
                lblErr.Text = "No Record Found!!";
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lblErr.Text = "";
            lblCount.Text = "";
            BindData(1);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            BindData(1);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindData(1);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}
