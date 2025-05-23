using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OnlineTrasction : System.Web.UI.Page
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void Filldate()
    {
        try
        {
            DAL objDAL = new DAL();
            string query = "SELECT REPLACE(CONVERT(VARCHAR, GETDATE(), 106), ' ', '-') AS Date, REPLACE(CONVERT(VARCHAR, GETDATE(), 106), ' ', '-') AS CurrentDate";
            DataTable dtData = new DataTable();
            dtData = objDAL.GetData(query);

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
            string startDate;
            string endDate;
            string ID;
            string WalletAddress;
            string ThnHash;
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");

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
            if (string.IsNullOrEmpty(txtMemId.Text))
            {
                ID = "";
            }
            else
            {
                ID = txtMemId.Text;
            }
            if (string.IsNullOrEmpty(TxtWalletAddress.Text))
            {
                WalletAddress = "";
            }
            else
            {
                WalletAddress = TxtWalletAddress.Text;
            }

            if (string.IsNullOrEmpty(TxtHash.Text))
            {
                ThnHash = "";
            }
            else
            {
                ThnHash = TxtHash.Text;
            }

            DataTable Dt_GetApi = new DataTable();
            string sql = "exec sp_GetApiResponse '" + ID + "','" + WalletAddress + "','" + ThnHash + "','" + startDate + "', '" + endDate + "'";
            Dt_GetApi = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = Dt_GetApi;
            GvData.DataBind();
            Session["GData"] = Dt_GetApi;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string startDate;
            string endDate;
            string ID;
            string WalletAddress;
            string ThnHash;
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");

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
            if (string.IsNullOrEmpty(txtMemId.Text))
            {
                ID = "";
            }
            else
            {
                ID = txtMemId.Text;
            }

            if (string.IsNullOrEmpty(TxtWalletAddress.Text))
            {
                WalletAddress = "";
            }
            else
            {
                WalletAddress = TxtWalletAddress.Text;
            }

            if (string.IsNullOrEmpty(TxtHash.Text))
            {
                ThnHash = "";
            }
            else
            {
                ThnHash = TxtHash.Text;
            }




            DataTable Dt_GetApi = new DataTable();
            string sql = "exec sp_GetApiResponse '" + ID + "','" + WalletAddress + "','" + ThnHash + "','" + startDate + "', '" + endDate + "'";
            Dt_GetApi = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            Session["OnlineTrasctionReport"] = Dt_GetApi;
            ExportExcel();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["OnlineTrasctionReport"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "OnlineTrasctionReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=USDTBEP20TransactionReport.xlsx");
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
            GvData.DataSource = Session["OnlineTrasctionReport"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
