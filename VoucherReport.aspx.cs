using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class VoucherReport : System.Web.UI.Page
{

    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    DataTable dtData = new DataTable();
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
           string sql = "Exec Sp_VoucherKit";
            dtData = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
            ddlstate.DataSource = dtData;
            ddlstate.DataTextField = "KitName";
            ddlstate.DataValueField = "KitID";
            ddlstate.DataBind();

            ddlstate.Items.Insert(0, new ListItem("--Select Kit Name--", ""));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void Filldate()
    {
        string str = "Select Replace(Convert(Varchar, GetDate(), 106), ' ', '-') as CurrentDate";
        DataTable dtData1 = new DataTable();
        dtData1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];

        if (dtData1.Rows.Count > 0)
        {
            txtStartDate.Text = dtData1.Rows[0]["CurrentDate"].ToString();
            txtEndDate.Text = dtData1.Rows[0]["CurrentDate"].ToString();
        }
    }
    private void FillReport()
    {
        string sql = string.Empty;
        string Idno = "0";
        string KitID = "0";
        string ddlUSeid = "0";

        // Get MemberID
        if (!string.IsNullOrWhiteSpace(txtMemberID.Text))
        {
            Idno = txtMemberID.Text.Trim();
        }
        else
        {
            Idno = "0";
        }

        // Get KitID
        if (ddlstate.SelectedValue == "--Select Kit Name--")
        {
            KitID = "0";
        }
        else
        {
            KitID = ddlstate.SelectedValue;
        }

        // Get Use Type
        if (ddlUSe.SelectedValue == "--Select Use Type--")
        {
            ddlUSeid = "0";
        }
        else
        {
            ddlUSeid = ddlUSe.SelectedValue;
        }

        // Compose SQL using string concatenation
        sql = "exec Sp_VoucherReport '" + Idno + "', '" + KitID + "', '" + txtStartDate.Text + "', '" + txtEndDate.Text + "', '" + ddlUSeid + "'";

        // Execute query
        DataTable dtData = new DataTable();
        dtData = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];

        // Bind to GridView
        GvData.DataSource = dtData;
        GvData.DataBind();

        // Store in Session
        Session["GData"] = dtData;
    }
        protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillReport();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        FillReport();
        ExportExcel();
    }

    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvData.PageIndex = e.NewPageIndex;
        FillReport();
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["GData"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "InvestmentReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=VoucherReport.xlsx");
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
}
