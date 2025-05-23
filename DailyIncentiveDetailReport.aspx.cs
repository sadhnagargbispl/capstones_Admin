using ClosedXML.Excel;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class DailyIncentiveDetailReport : System.Web.UI.Page
{
     DataTable Dt = new DataTable();
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private int CurrentPageIndex;
    private readonly object pagerDataList;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

          
                if (Session["AStatus"] != null)
                {
                    BindFromDate();
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
    public void BindFromDate()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr, "sp_GetDailySessionDetail");
            DDlFromDate.DataSource = Ds.Tables[0];
            DDlFromDate.DataValueField = "SessID";
            DDlFromDate.DataTextField = "Fromdate";
            DDlFromDate.DataBind();
            DDltodate.DataSource = Ds.Tables[0];
            DDltodate.DataValueField = "SessID";
            DDltodate.DataTextField = "Todate";
            DDltodate.DataBind();
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
            string FromSessid = "0";
            string ToSessid = "0";
            string Idno = "0";

            FromSessid = DDlFromDate.Text != "" ? DDlFromDate.Text : Session["CompDate"].ToString();
            ToSessid = DDltodate.Text != "" ? DDltodate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "0";

            GvData.DataSource = null;
            GvData.DataBind();
            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@FromSessid", FromSessid);
            prms[2] = new SqlParameter("@ToSessid", ToSessid);
            prms[3] = new SqlParameter("@PageIndex", PageIndex);
            prms[4] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[5] = new SqlParameter("@IsExport", "N");
            prms[6] = new SqlParameter("@RecordCount", SqlDbType.Int);
            prms[6].Direction = ParameterDirection.Output;
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetDailyPayoutDetail", prms);
            GvData.DataSource = Ds.Tables[0];
            GvData.DataBind();
            int recordCount = (int)Ds.Tables[1].Rows[0]["RecordCount"];
            Session["GData"] = Ds.Tables[0];
            if (Ds.Tables[0].Rows.Count > 0)
            {
                lblCount.Text = "Total Record: " + Ds.Tables[1].Rows[0]["RecordCount"];
                lblinv.Text = "Total Income: " + Ds.Tables[1].Rows[0]["TotalIncome"].ToString();
                GvData.Visible = true;
            }
            else
            {
                lblError.Text = "No Record Found!!";
                GvData.Visible = false;
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
            string FromSessid = "0";
            string ToSessid = "0";
            string Idno = "0";

            FromSessid = DDlFromDate.Text != "" ? DDlFromDate.Text : Session["CompDate"].ToString();
            ToSessid = DDltodate.Text != "" ? DDltodate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "0";

            GvData.DataSource = null;
            GvData.DataBind();
            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@FromSessid", FromSessid);
            prms[2] = new SqlParameter("@ToSessid", ToSessid);
            prms[3] = new SqlParameter("@PageIndex", 1);
            prms[4] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[5] = new SqlParameter("@IsExport", "Y");
            prms[6] = new SqlParameter("@RecordCount", SqlDbType.Int);
            prms[6].Direction = ParameterDirection.Output;
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetDailyPayoutDetail", prms);
            Session["GData1"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + " Error In Exporting File " + "')", true);
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["GData1"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "DailyIncentiveDetailReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=DailyIncentiveDetailReport.xlsx");
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
}
