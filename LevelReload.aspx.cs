using ClosedXML.Excel;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class LevelReload : System.Web.UI.Page
{
    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        AddLevel.Attributes.Add("onclick", DisableTheButton(Page, AddLevel));
        try
        {
            if (!IsPostBack)
            {
                if (Session["AStatus"] != null)
                {
                    BindData();
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
    protected void BtnShow_Click(object sender, EventArgs e)
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
    public void BindData()
    {
        lblError.Text = "";
        try
        {
            string Idno = "0";
            Idno = txtMemId.Text != "" ? txtMemId.Text : "0";
            string qry1 = "";
            DataTable dtData = new DataTable();
            qry1 = objDAL.IsoStart + "exec Sp_GetDataReLoad '" + Idno.ToString() + "'" + objDAL.IsoEnd;
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry1).Tables[0];
            if (dtData.Rows.Count > 0)
            {
                GvData.DataSource = dtData;
                GvData.DataBind();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void AddLevel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AddLevelReload.aspx", false);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }
}
