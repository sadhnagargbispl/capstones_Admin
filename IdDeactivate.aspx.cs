using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Activities.Expressions;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class IdDeactivate : System.Web.UI.Page
{
    DAL objDal = new DAL();
    string Sql = "";
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
            //this.BtnDedactive.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnDedactive));
            lblrecordcount.Text = "";

            if (!IsPostBack)
            {
                txtMemberId.Text = "";

                //if (Request.QueryString.HasKeys)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["key"]) && Request.QueryString["key"] != null)
                    {
                        txtMemberId.Text = Request.QueryString["key"].ToString();
                        //ShowDetail();

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void ShowDetail()
    {
        try
        {
            string idNo;
            lblError.Text = "";
            DataTable Dt = new DataTable();
            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                idNo = ClearInject(txtMemberId.Text);
                string qry = objDal.IsoStart + "Select FormNo from " + objDal.DBName + "..M_MemberMaster WHERE IDNO='" + idNo + "'" + objDal.IsoEnd;
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    TxtFormNo.Text = Dt.Rows[0][0].ToString();
                }
                else
                {
                    lblError.Text = "Member ID not exist. Please provide correct member ID.";
                    lblError.Visible = true;
                    return;
                }
            }
            else
            {
                lblError.Text = "Member Id can not be blank. Please provide member ID to proceed.";
                lblError.Visible = true;
            }
        }
        catch (Exception EX)
        {
            throw new Exception(EX.Message);
        }
    }
   
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //GvData.PageIndex = e.NewPageIndex;
            //GvData.DataSource = Session["GData"];
            //GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    //private string DisableTheButton(Control pge, Control btn)
    //{
    //    try
    //    {
    //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
    //        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
    //        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
    //        sb.Append("this.value = 'Please wait...';");
    //        sb.Append("this.disabled = true;");
    //        sb.Append(pge.Page.GetPostBackEventReference(btn));
    //        sb.Append(";");
    //        return sb.ToString();

    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }

    //}
   
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            StrObj = StrObj.Trim();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return StrObj;
    }

   
    protected void txtMemberId_TextChanged(object sender, EventArgs e)
    {
        string idNo;
        lblError.Text = "";
        DataTable Dt = new DataTable();
        idNo = ClearInject(txtMemberId.Text);
        string qry = objDal.IsoStart + "Select FormNo,Activestatus from " + objDal.DBName + "..M_MemberMaster WHERE IDNO='" + idNo + "'" + objDal.IsoEnd;
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
        if (Dt.Rows.Count == 0)
        {
            lblError.Text = "Member ID not exist. Please provide correct member ID.";
            lblError.Visible = true;
            BtnDedactive.Enabled = false;
            txtMemberId.Text = "";
            return;
        }
        else if (Dt.Rows[0]["Activestatus"].ToString() == "N")
        {
            lblError.Text = "This Member ID already De-Active.";
            lblError.Visible = true;
            BtnDedactive.Enabled = false;
            txtMemberId.Text = "";
            return;
        }
        else if (Dt.Rows[0]["Activestatus"].ToString() == "Y")
        {
            TxtFormNo.Text = Dt.Rows[0]["FormNo"].ToString();
            BtnDedactive.Enabled = true;
        }
        else
        {
            //TxtFormNo.Text = Dt.Rows[0]["FormNo"].ToString();
            //BtnDedactive.Enabled = true;
        }
        //try
        //{
        //    DataTable dt1 = new DataTable();

        //    Sql = objDal.IsoStart + " Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName,IsNull(c.Idno,'') as SponsorId," +
        //          " isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName,a.activeStatus,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq " +
        //          " ,a.LegNo,B.KitName,Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') else '' end as UpgradeDate," +
        //          " a.FLD1,a.Planid " +
        //          " from " + objDal.DBName + "..M_KitMaster as b," + objDal.DBName + "..M_MemberMaster as a Left Join " + objDal.DBName + "..M_MemberMaster as c on a.RefFormno=c.Formno  " +
        //          " where a.KitId=b.KitId and  (b.RowStatus='Y')  and a.idno='" + txtMemberId.Text + "' and a.IsBlock='N'" + objDal.IsoEnd;

        //    DataTable Dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];

        //    if (Dt_.Rows.Count == 0)
        //    {
        //        lblError.Text = " Please enter correct Member ID.";
        //        lblError.ForeColor = System.Drawing.Color.Red;
        //        txtMemberId.Text = "";
        //        //LblMemName.Text = "";
        //        //LblKitName.Text = "";
        //        //LblNewKitid.Text = "";
        //        GvData.Visible = false;
        //        BtnDedactive.Enabled = false;
        //        //return false;
        //    }
        //    else
        //    {


        //        string s = objDal.IsoStart + " select c.idno,(c.memfirstName+C.MemlastName) as MemName,b.KitName,Replace(Convert(Varchar,BillDate,106),' ','-')as UpgradeDate " +
        //                   " from " + objDal.DBName + "..Repurchincome as a," + objDal.DBName + "..M_kitMaster as B ," + objDal.DBName + "..m_membermaster as c  " +
        //                   " where a.formno=c.formno and a.kitid=b.kitid and b.RowsTatus='Y' and a.Formno='" + Dt_.Rows[0]["Formno"].ToString() + "' " +
        //                   " Union all " +
        //                   " select c.idno,(c.memfirstName+C.MemlastName) as MemName,b.KitName,''as UpgradeDate " +
        //                   " from " + objDal.DBName + "..M_kitMaster as B ," + objDal.DBName + "..m_membermaster as c " +
        //                   " where c.kitid=b.kitid and b.RowsTatus='Y' and c.Formno='" + Dt_.Rows[0]["Formno"].ToString() + "' and c.ActiveStatus='N' " + objDal.IsoEnd;

        //        dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];

        //        if (dt1.Rows.Count > 0)
        //        {
        //            GvData.DataSource = dt1;
        //            GvData.DataBind();
        //            GvData.Visible = true;
        //        }
        //        else
        //        {
        //            GvData.Visible = false;
        //        }

        //        //LblMemName.ForeColor = System.Drawing.Color.Black;
        //        BtnDedactive.Enabled = true;

        //        //return true;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    // Handle the exception
        //    //return false;
        //}
    }

    protected void BtnDedactive_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql, scrname;
            //string Remark = "";

            Sql = "exec Sp_IddeactivateAdmin '" + ClearInject(TxtFormNo.Text) + "'";

            scrname = "ID";
            string Str_Sql = string.Empty;
            Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            //int updateEffect = 0;
            //updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
            DataTable dtCheck = new DataTable();
            dtCheck = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str_Sql).Tables[0];
            if (Convert.ToInt32(dtCheck.Rows[0]["Result"]) == 1)
            //if (updateEffect > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + scrname + " Roll Back Successfully.!')", true);

                TxtFormNo.Text = "";
                txtMemberId.Text = "";
                BtnDedactive.Visible = false;
                //GvData.Visible = false;
                lblrecordcount.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Id do not Roll Back!! ')", true);

            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
