using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DirectTeamBuniessReport : System.Web.UI.Page
{
    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["AStatus"] != null)
                {
                    Fillkit();
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
    private void Fillkit()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr, "sp_ddlPageSize");
            ddlPageSize.DataSource = Ds.Tables[0];
            ddlPageSize.DataValueField = "ddlPageSize";
            ddlPageSize.DataTextField = "ddlPageSize";
            ddlPageSize.DataBind();
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
            BindData(1);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void BindData(int PageIndex)
    {
        lblError.Text = "";
        try
        {
            string FromSessid = "";
            string ToSessid = "";
            string Idno = "";
            string DirectBusiness = "0";
            string TeamBusiness = "0";
            FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "";
            DirectBusiness = txtDirectbusiness.Text != "" ? txtDirectbusiness.Text : "0";
            TeamBusiness = TxtTeamBusiness.Text != "" ? TxtTeamBusiness.Text : "0";

            GvData1.DataSource = null;
            GvData1.DataBind();
            SqlParameter[] prms = new SqlParameter[9];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@StartDate", FromSessid);
            prms[2] = new SqlParameter("@EndDate", ToSessid);
            prms[3] = new SqlParameter("@DirectBusiness", DirectBusiness);
            prms[4] = new SqlParameter("@TeamBusiness", TeamBusiness);
            prms[5] = new SqlParameter("@PageIndex", PageIndex);
            prms[6] = new SqlParameter("@PageSize", 100000000);
            prms[7] = new SqlParameter("@IsExport", "N");
            prms[8] = new SqlParameter("@RecordCount", ParameterDirection.Output)
            { Direction = ParameterDirection.Output };
            Ds = SqlHelper.ExecuteDataset(constr1, "GetTeamDirectBunessReport", prms);
            if (Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                GvData1.DataSource = Ds.Tables[0];
                GvData1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                GvData1.DataBind();
                int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["Recordcount"].ToString());
                Session["InvestmentReport"] = Ds.Tables[0];
                ViewState["IdNo"] = Idno;
                ViewState["Sort_Order"] = "ASC";
                lblCount.Text = "Total Record: " + recordCount.ToString();
                GvData1.Visible = true;
            }
            else
            {
                lblError.Text = "No Record Found!!";
                GvData1.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string FromSessid = "0";
            string ToSessid = "0";
            string Idno = "";
            string DirectBusiness = "0";
            string TeamBusiness = "0";
            FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "";
            DirectBusiness = txtDirectbusiness.Text != "" ? txtDirectbusiness.Text : "0";
            TeamBusiness = TxtTeamBusiness.Text != "" ? TxtTeamBusiness.Text : "0";

            SqlParameter[] prms = new SqlParameter[9];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@StartDate", FromSessid);
            prms[2] = new SqlParameter("@EndDate", ToSessid);
            prms[3] = new SqlParameter("@DirectBusiness", DirectBusiness);
            prms[4] = new SqlParameter("@TeamBusiness", TeamBusiness);
            prms[5] = new SqlParameter("@PageIndex", 1);
            prms[6] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[7] = new SqlParameter("@IsExport", "Y");
            prms[8] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(constr1, "GetTeamDirectBunessReport", prms);
            Session["InvestmentReportExcel"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["InvestmentReportExcel"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "InvestmentReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=CheckBusinessReport.xlsx");
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData1.PageIndex = e.NewPageIndex;
            GvData1.DataSource = Session["InvestmentReport"];
            GvData1.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
