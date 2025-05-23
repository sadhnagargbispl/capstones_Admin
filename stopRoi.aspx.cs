using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stopRoi : System.Web.UI.Page
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
                    GetIncome();
                    FillData();
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }

    }
    private void GetIncome()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr, "Sp_GetIncomeName");
            DDlIncomeType.DataSource = Ds.Tables[0];
            DDlIncomeType.DataValueField = "Value";
            DDlIncomeType.DataTextField = "Name";
            DDlIncomeType.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
            str = ObjDAl.IsoStart + "exec Sp_GetNameROI '" + txtIdno.Text + "' " + ObjDAl.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count == 0)
            {
                txtIdno.Text = "";
                TxtMemberName.Text = "";
                txtstatus.Text = "";
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
    protected void FillData()
    {
        try
        {
            string qry1 = "";
            DataTable dtData = new DataTable();

            qry1 = ObjDAl.IsoStart + "exec Sp_GetROIData" + ObjDAl.IsoEnd;
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry1).Tables[0];

            if (dtData.Rows.Count > 0)
            {
                GvData.DataSource = dtData;
                GvData.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string Rstatus = "";
            string str = ObjDAl.IsoStart + "Select top 1 Rstatus from " + ObjDAl.DBName + "..Stoproi where formno='" + hdnFormno.Value + "' order by rectimestamp desc" + ObjDAl.IsoEnd;
            DataTable dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];

            if (dtData.Rows.Count > 0)
            {
                if (dtData.Rows[0]["Rstatus"].ToString() == "N")
                {
                    str = "exec SP_SAVEROI '" + hdnFormno.Value + "','S','Y','" + DDlIncomeType.SelectedItem.Text + "'";
                }
                else if (dtData.Rows[0]["Rstatus"].ToString() == "Y")
                {
                    str = "exec SP_SAVEROI '" + hdnFormno.Value + "','S','N','" + DDlIncomeType.SelectedItem.Text + "'";
                }
            }
            else
            {
                str = "exec SP_SAVEROI '" + hdnFormno.Value + "','S','Y','" + DDlIncomeType.SelectedItem.Text + "'";
            }

            int x = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str));

            if (x > 0)
            {
                Clear();
                scrname = "<SCRIPT language='javascript'>alert('Record Save Successfully..!');</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                DDlIncomeType.SelectedValue = "0";
                FillData();
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
            txtstatus.Text = "";
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
            txtstatus.Text = "";
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
            GetName();
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
    //protected void SelectRoi_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (hdnFormno.Value.ToString() == "")
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Please Enter Member ID.!')", true);
    //        SelectRoi.SelectedValue = "0";
    //        return;
    //    }
    //    else
    //    {
    //        DataTable dt = new DataTable();
    //        string Str = ObjDAl.IsoStart + " exec Sp_GetROIBind '" + SelectRoi.SelectedValue + "','" + hdnFormno.Value + "','" + DDlIncomeType.SelectedItem.Text + "'" + ObjDAl.IsoEnd;
    //        dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Str).Tables[0];
    //        if (dt.Rows.Count == 0)
    //        {
    //            txtstatus.Text = "Start";
    //            BtnSubmit.Text = "Stop";
    //        }
    //        else
    //        {
    //            txtstatus.Text = dt.Rows[0]["Rstatus"].ToString();

    //            if (dt.Rows[0]["Rstatus"].ToString().ToUpper() == "START")
    //            {
    //                BtnSubmit.Text = "Stop";
    //            }
    //            else if (dt.Rows[0]["Rstatus"].ToString().ToUpper() == "STOP")
    //            {
    //                BtnSubmit.Text = "Start";
    //            }
    //        }
    //    }

    //}
    protected void DDlIncomeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (hdnFormno.Value.ToString() == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Please Enter Member ID.!')", true);
            DDlIncomeType.SelectedValue = "0";
            return;
        }
        else
        {
            DataTable dt = new DataTable();
            string Str = ObjDAl.IsoStart + " exec Sp_GetROIBind 'S','" + hdnFormno.Value + "','" + DDlIncomeType.SelectedItem.Text + "'" + ObjDAl.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Str).Tables[0];
            if (dt.Rows.Count == 0)
            {
                txtstatus.Text = "Start";
                BtnSubmit.Text = "Stop";
            }
            else
            {
                txtstatus.Text = dt.Rows[0]["Rstatus"].ToString();

                if (dt.Rows[0]["Rstatus"].ToString().ToUpper() == "START")
                {
                    BtnSubmit.Text = "Stop";
                }
                else if (dt.Rows[0]["Rstatus"].ToString().ToUpper() == "STOP")
                {
                    BtnSubmit.Text = "Start";
                }
            }
        }

    }
}

