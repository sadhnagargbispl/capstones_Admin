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

public partial class PoolReport : System.Web.UI.Page
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
                    Fillpool();
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

    private void Fillpool()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr1, "Sp_GetSelectPoolTable");
            ddltype.DataSource = Ds.Tables[0];
            ddltype.DataValueField = "Rid";
            ddltype.DataTextField = "PoolName";
            ddltype.DataBind();
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
            string Idno = "0";

            FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "0";

            GvData1.DataSource = null;
            GvData1.DataBind();
            SqlParameter[] prms = new SqlParameter[9];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@FromSessid", FromSessid);
            prms[2] = new SqlParameter("@ToSessid", ToSessid);
            prms[3] = new SqlParameter("@PageIndex", PageIndex);
            prms[4] = new SqlParameter("@PageSize", 100000000);
            prms[5] = new SqlParameter("@IsExport", "N");
            prms[6] = new SqlParameter("@Type", ddltype.SelectedItem.ToString());
            prms[7] = new SqlParameter("@Rid", ddltype.SelectedValue.ToString());
            prms[8] = new SqlParameter("@RecordCount", ParameterDirection.Output)
            { Direction = ParameterDirection.Output };
            
             
            Ds = SqlHelper.ExecuteDataset(constr1, "Sp_GetPoolTableDetailadmin", prms);
            if (Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                GvData1.DataSource = Ds.Tables[0];
                GvData1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                GvData1.DataBind();
                int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["Recordcount"].ToString());
                Session["PoolReport"] = Ds.Tables[0];
                ViewState["IdNo"] = Idno;
                ViewState["Sort_Order"] = "ASC";

                lblCount.Text = "Total Record: " + recordCount.ToString();
                lblCount.Visible = true;
                lblinv.Text = "Total Amount: " + Ds.Tables[1].Rows[0]["Total"].ToString();
                lblinv.Visible = true;
                GvData1.Visible = true;
            }
            else
            {
                lblError.Text = "No Record Found!!";
                GvData1.Visible = false;
                lblCount.Visible = false;
                lblinv.Visible = false;
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
            string Idno = "0";
            FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "0";
            SqlParameter[] prms = new SqlParameter[9];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@FromSessid", FromSessid);
            prms[2] = new SqlParameter("@ToSessid", ToSessid);
            prms[3] = new SqlParameter("@PageIndex", 1);
            prms[4] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[5] = new SqlParameter("@IsExport", "Y");
            prms[6] = new SqlParameter("@Type", ddltype.SelectedValue.ToString());
            prms[7] = new SqlParameter("@Rid", ddltype.SelectedValue.ToString());
            prms[8] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(constr1, "Sp_GetPoolTableDetailadmin", prms);
            Session["PoolReportExcel"] = Ds.Tables[0];
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
            DataTable dt = (DataTable)Session["PoolReportExcel"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "PoolReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=PoolReport.xlsx");
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
            GvData1.DataSource = Session["PoolReport"];
            GvData1.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
