using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class CoinMaster : System.Web.UI.Page
{
    DAL objDAL = new DAL();
    string IsoStart;
    string IsoEnd;
    ModuleFunction objModuleFun = new ModuleFunction();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!Page.IsPostBack)
            {
                btnShowRecord.Visible = false;
                if (Session["AStatus"] == "OK")
                {
                    BindData();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }

    public void BindData()
    {
        try
        {
            DataTable Dt = new DataTable();
            string sql = "Select CoinId,Cast(CoinId as varchar) as VCTypeId,gasfees as Coinrate,Replace(Convert(Varchar,rectimestamp,106),' ','-') as CoinDate," +
                         " CASE WHEN statusapi='Y' Then 'Active' ELSE 'DeActive' END AS Status," +
                         " Case when statusapi='N' then 'label label-danger' else 'label label-success' end as StatusClass From "+ objDAL.DBName +"..gasfeescheck " +
                         " Where 1=1 Order by CoinId Desc";
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = Dt;
            GvData.DataBind();
            Session["GData"] = Dt;
            ViewState["WithDrawDate"] = "CoinID";
            ViewState["Sort_Order"] = "ASC";

            Dt = new DataTable();
            Dt = objDAL.GetData(sql);
            GvData.DataSource = Dt;
            GvData.DataBind();
            Session["GData"] = Dt;

            if (Dt.Rows.Count > 0)
            {
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
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AddCoin.aspx", false);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void DeleteGroup(object sender, EventArgs e)
    {
        try
        {
            string GrpID, scrname;
            GridViewRow GVRw;

            GVRw = (GridViewRow)((Control)sender).Parent.Parent;
            GrpID = ((Label)GVRw.FindControl("LblGrpID")).Text;
            string sql = "Update gasfeescheck SET statusapi='N',LastModified='De-Activated by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "' WHERE CoinId='" + GrpID + "' ";
            int updateEffect = objDAL.UpdateData(sql);
            if (updateEffect != 0)
            {
                scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');</SCRIPT>";
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Not able to delete the Coin rate!');</SCRIPT>";
            }
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Complaint Type Deletion", scrname, false);
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["GData"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Users");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=CoinMaster-" + DateTime.Now.ToShortDateString() + ".xlsx");
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

}
