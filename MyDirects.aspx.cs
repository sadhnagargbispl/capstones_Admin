﻿
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class MyDirects : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DataSet Ds = new DataSet();
    DAL ObjDal = new DAL();
    DAL Obj = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["AStatus"] != null)
            {
                if (!Page.IsPostBack)
                {
                    FillLevel();                   
                    DdlLevel.SelectedValue = "0";
                    LevelDetail(1);
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void FillLevel()
    { 
        try
        {
            string Formno = "";
            if (string.IsNullOrEmpty(txtMember.Text))
            {
                Formno = "0";
            }
            else
            {
                Formno = GetFormNo().ToString();
            }
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@FormNo", Formno);
            prms[1] = new SqlParameter("@type", "N");

            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetLevel", prms);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                DdlLevel.DataSource = Ds.Tables[0];
                DdlLevel.DataTextField = "LevelName";
                DdlLevel.DataValueField = "MLevel";
                DdlLevel.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public void LevelDetail(int pageIndex)
    {
        try
        {
            string legno = "";
            string level = "";
            if (rbtnsearch.SelectedValue == "L")
            {
                legno = "0";
                level = DdlLevel.SelectedValue;
            }
            else
            {
                legno = rbtnsearch.SelectedValue;
                level = "1";
            }

            string Formno = "";
            if (string.IsNullOrEmpty(txtMember.Text))
            {
                Formno = "0";
            }
            else
            {
                Formno = GetFormNo();
            }

        
            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@MLevel", level);
            prms[1] = new SqlParameter("@Legno", legno);
            prms[2] = new SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue);
            prms[3] = new SqlParameter("@FormNo", Formno);
            prms[4] = new SqlParameter("@PageIndex", pageIndex);
            prms[5] = new SqlParameter("@PageSize", 150000000);
            prms[6] = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

            DataSet ds = SqlHelper.ExecuteDataset(constr, "sp_GetLevelDetail", prms);
            GvData.DataSource = ds.Tables[0];
            GvData.DataBind();
            Session["IssuedPinValue"] = ds.Tables[0];
            DataTable dt = ds.Tables[0];
            int recordCount = (int)ds.Tables[1].Rows[0]["RecordCount"];
            lbltotal.Text = recordCount.ToString();

            if (recordCount > 0)
            {
                GvData.DataSource = dt;
                GvData.DataBind();
            }
            else
            {
                GvData.DataSource = dt;
                GvData.DataBind();
                lbltotal.Text = recordCount.ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void FillData()
    {
        try
        {
            string Formno = GetFormNo();

            string qry = "Select * from V#ReferalDownlineinfo where Formno=" + Formno;
            DataTable dt = Obj.GetData(qry);
            if (dt.Rows.Count > 0)
            {
                tdDirectleft.InnerText = dt.Rows[0]["RegisterLeft"].ToString();
                tdDirectright.InnerText = dt.Rows[0]["RegisterRight"].ToString();
                TotalDirect.InnerText = (Convert.ToInt32(dt.Rows[0]["RegisterLeft"]) + Convert.ToInt32(dt.Rows[0]["RegisterRight"])).ToString();
                tddirectActive.InnerText = dt.Rows[0]["ConfirmLeft"].ToString();
                tdindirectActive.InnerText = dt.Rows[0]["ConfirmRight"].ToString();
                TotalActive.InnerText = (Convert.ToInt32(dt.Rows[0]["ConfirmLeft"]) + Convert.ToInt32(dt.Rows[0]["ConfirmRight"])).ToString();
                Directunit.InnerText = dt.Rows[0]["LeftBv"].ToString();
                indirectunit.InnerText = dt.Rows[0]["RightBv"].ToString();
                totalunit.InnerText = (Convert.ToInt32(dt.Rows[0]["LeftBv"]) + Convert.ToInt32(dt.Rows[0]["RightBv"])).ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private string GetFormNo()
    {
        try
        {  
            string idNo = txtMember.Text.Trim();
            string formno = "0";
            string qry = ObjDal.IsoStart + " Select FormNo from "+ ObjDal.DBName  +"..M_MemberMaster  where IdNo='" + idNo + "' " +  ObjDal.IsoEnd;
            DataTable dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
            if (dt.Rows.Count > 0)
            {
                formno = dt.Rows[0]["FormNo"].ToString();
                lblErr.Text = "";
                lblErr.Visible = false;
            }
            else
            {
                lblErr.Text = "Member Id does not exist.";
                lblErr.Visible = true;
                txtMember.Text = "";
            }
            return formno;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void txtMember_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string Formno = GetFormNo();
            if (Formno != "0")
            {
                FillLevel();
                FillData();
                LevelDetail(1);
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            FillData();
            LevelDetail(1);
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["IssuedPinValue"];
                GvData.PageIndex = e.NewPageIndex;
                GvData.DataSource = dt;
                GvData.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            }
        }
    }

  
}
