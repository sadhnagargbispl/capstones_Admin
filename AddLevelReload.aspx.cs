using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddLevelReload : System.Web.UI.Page
{
    string scrname;
    string formno;
    DAL ObjDAl = new DAL();
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            BtnSubmit.Attributes.Add("onclick", DisableTheButton(Page, BtnSubmit));
            if (Session["AStatus"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    Session["PageName"] = "Master / Create Task";
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
    protected string GetName()
    {
        try
        {

            string str = "";
            DataTable dt = new DataTable();
            str = ObjDAl.IsoStart + "exec Sp_GetName '" + txtIdno.Text + "' " + ObjDAl.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count == 0)
            {
                txtIdno.Text = "";
                TxtMemberName.Text = "";
                scrname = "<SCRIPT language='javascript'>alert('Invalid ID');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            }
            else
            {
                if (dt.Rows[0]["Isblock"].ToString() == "Y")
                {
                    txtIdno.Text = "";
                    TxtMemberName.Text = "";
                    lblstatus.Text = "";
                    scrname = "<SCRIPT language='javascript'>alert('This Id  is block Please Contact To Admin.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return "";
                }
                else
                {
                    TxtMemberName.Text = dt.Rows[0]["memname"].ToString();
                    lblemail.Text = dt.Rows[0]["email"].ToString();
                    hdnFormno.Value = dt.Rows[0]["Formno"].ToString();
                    LblMobile.Text = "";
                    return "OK";
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            ObjDAl.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
        return ""; // Default return statement in case of any exception or error
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string str = "insert Into LevelReload(Formno, RectimeStamp)Values('" + hdnFormno.Value + "', getdate())";
            int x = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str));
            if (x > 0)
            {
                Clear();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Record Save Successfully.!');location.replace('LevelReload.aspx');", true);
                BtnSubmit.Text = "Save";
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Record Not Saved, Please Try Again!');</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            txtIdno.Text = "";
            TxtMemberName.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void Clear()
    {
        try
        {
            txtIdno.Text = "";
            TxtMemberName.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void txtIdno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (GetName() == "OK")
            {
                string str1 = "";
                DataTable dt1 = new DataTable();
                str1 = ObjDAl.IsoStart + "select * from " + ObjDAl.DBName + "..LevelReload where formno = '" + hdnFormno.Value + "' " + ObjDAl.IsoEnd;
                dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str1).Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    scrname = "<SCRIPT language='javascript'>alert('This Id is Already Add.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    txtIdno.Text = "";
                    TxtMemberName.Text = "";
                }

            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private string GetFormNo()
    {
        try
        {
            string qry = ObjDAl.IsoStart + "Select FormNo from " + ObjDAl.DBName + "..M_MemberMaster where IdNo='" + txtIdno.Text.Trim() + "'" + ObjDAl.IsoEnd;
            DataTable dt = new DataTable();

            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];

            if (dt.Rows.Count > 0)
            {
                formno = dt.Rows[0]["FormNo"].ToString();
            }

            return formno;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

