using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PackageActivate : System.Web.UI.Page
{
    DAL Objdal = new DAL();
    string Sql = "";
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string scrName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["AStatus"]?.ToString() == "OK")
            {
                Session["PageName"] = "Member / ID Activate ";
                if (!IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringAdmin(6);
                    //FillKit();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    //public void FillKit(string condition = "")
    //{
    //    try
    //    {
    //        Sql = Objdal.IsoStart + " Select kitId,KitName From " + Objdal.DBName + "..M_KitMaster" +
    //              " Where  RowStatus='Y' and KitId<>4 " + condition + "  Order By kitName" + Objdal.IsoEnd;

    //        DataTable Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];

    //        DDlKit.DataSource = Dt;
    //        DDlKit.DataTextField = "KitName";
    //        DDlKit.DataValueField = "KitId";
    //        DDlKit.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

    public string GenerateRandomStringAdmin( int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < iLength; i++)
        {
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        }
        return sResult;
    }
    private bool Check_IdNo()
    {
        try
        {
            DataTable dt1 = new DataTable();
            Sql = "EXEC sP_GetRepurchdataNew  '" + TxtIDNo.Text + "'";
          
            DataTable Dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];

            if (Dt_.Rows.Count == 0)
            {
                lblError.Text = " Please enter correct Member ID.";
                lblError.ForeColor = System.Drawing.Color.Red;
                TxtIDNo.Text = "";
                LblMemName.Text = "";
                LblKitName.Text = "";
                LblNewKitid.Text = "";
                GrdDirects1.Visible = false;
                BtnUpgrade.Enabled = false;
                return false;
            }
            else
            {
                LblKitId.Text = Dt_.Rows[0]["KitId"].ToString();
                lblError.Text = "";
                LblMemName.Text = Dt_.Rows[0]["MemName"].ToString();
                LblFormno.Text = Dt_.Rows[0]["Formno"].ToString();
                LblKitName.Text = Dt_.Rows[0]["KitName"].ToString();

                //if (Dt_.Rows[0]["ActiveStatus"].ToString() == "Y" && Dt_.Rows[0]["Kitid"].ToString()=="4")
                if (Dt_.Rows[0]["ActiveStatus"].ToString() == "Y" || Dt_.Rows[0]["ActiveStatus"].ToString() == "N")
                {
                    LblCondition.Text = " and TopupSeq>='" + Dt_.Rows[0]["TopupSeq"].ToString() + "'";

                    //scrName = "<SCRIPT language='javascript'>alert('This Id already activate 0 Pin Activation.');location.replace('PackageActivate.aspx');</SCRIPT>";
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                    //return false;
                }
                else
                {
                    LblCondition.Text = "";
                }
                string s = " exec sP_GetRepurchdata '" + Dt_.Rows[0]["Formno"].ToString() + "'";

             
                dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    GrdDirects1.DataSource = dt1;
                    GrdDirects1.DataBind();
                    Session["gridData"] = dt1;
                    GrdDirects1.Visible = true;
                }
                else
                {
                    GrdDirects1.Visible = false;
                }

                LblMemName.ForeColor = System.Drawing.Color.Black;
                BtnUpgrade.Enabled = true;

                return true;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
            return false;
        }
    }
    protected void BtnUpgrade_Click(object sender, EventArgs e)
    {
        try
        {
            int updateEffect;
            string strSql = "Insert into Trnactivecadmin (Transid, Rectimestamp) values(" + HdnCheckTrnns.Value + ", getdate())";
            int updateEffect1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strSql));
              if (updateEffect1 > 0)
            {
                string scrname;
                string Remark = " Package Upgrade of Idno:" + TxtIDNo.Text + "";

                try
                {
                    lblError.Text = "";

                    if (string.IsNullOrWhiteSpace(TxtIDNo.Text))
                    {
                        lblError.Text = "Enter Member ID.";
                        return;
                    }
                    else
                    {
                        if (!Check_IdNo())
                        {
                            lblError.Text = "Invalid Member ID.";
                            return;
                        }
                        if (Convert.ToInt32(txtAmount.Text) >= 49)
                        {
                            //if (Convert.ToInt32(txtAmount.Text) >= 50 && Convert.ToInt32(txtAmount.Text) <= 950)
                            //{
                            //    Sql = "Exec Sp_packageActivateid '" + TxtIDNo.Text.Trim() + "',2," + Convert.ToInt32(txtAmount.Text) + ";";
                            //}
                            //if (Convert.ToInt32(txtAmount.Text) >= 1000 && Convert.ToInt32(txtAmount.Text) <= 100000000)
                            //{
                            //    Sql = "Exec Sp_packageActivateid '" + TxtIDNo.Text.Trim() + "',3," + Convert.ToInt32(txtAmount.Text) + ";";
                            //}

                            if (Convert.ToInt32(txtAmount.Text) >= 50 && Convert.ToInt32(txtAmount.Text) <= 100000000)
                            {
                                Sql = "Exec Sp_packageActivateid '" + TxtIDNo.Text.Trim() + "',4," + Convert.ToInt32(txtAmount.Text) + ";";
                            }
                          

                            if (Objdal.SaveData(Sql) != 0)
                            {
                                Objdal = new DAL();
                                Sql = "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,memberId)Values" +
                                    "('" + Convert.ToInt32(Session["UserID"]) + "','" + Session["UserName"] + "','Upgrade Package ','Upgrade Package','" + Remark + "',Getdate(),'" + LblFormno.Text + "')";
                               
                                int updateEffect2 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('ID Activated Successfully.!');location.replace('PackageActivate.aspx');", true);
                               
                                lblError.Text = " ID Activated successfully.";
                                TxtIDNo.Text = "";
                                LblMemName.Text = "";
                                lblError.Text = "";
                                BtnUpgrade.Enabled = false;
                                GrdDirects1.Visible = false;
                                LblFormno.Text = "";
                                LblKitId.Text = "";
                                LblKitName.Text = "";
                                LblNewKitid.Text = "";
                                txtAmount.Text = "";
                        
                            }
                        }
                        else
                        {
                            scrname = "<SCRIPT language='javascript'>alert('The investment should be more than 50 !!');</SCRIPT>";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('" + ex.Message + "');</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Something Want Wrong');location.replace('PackageActivate.aspx');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            TxtIDNo.Text = "";
            LblMemName.Text = "";
            lblError.Text = "";
            BtnUpgrade.Enabled = false;
            GrdDirects1.Visible = false;
            LblFormno.Text = "";
            LblKitId.Text = "";
            LblKitName.Text = "";
            LblNewKitid.Text = "";
            txtAmount.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    protected void TxtIDNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Check_IdNo();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //string str = "exec sp_checkinv '" + TxtIDNo.Text + "' ";
            //DataTable DT = new DataTable();
            //DT = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];
            //if (DT.Rows.Count > 0)
            //{
            //    if ((Convert.ToInt32 (txtAmount.Text) < Convert.ToInt32(DT.Rows[0]["repurch"])))
            //    {
            //        scrName = "<SCRIPT language='javascript'>alert('The investment should be more than last investment!!');" + "</SCRIPT>";
            //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
            //        txtAmount.Text = "";
            //        return;
            //    }
            //}
            if (Convert.ToInt32(txtAmount.Text) <= 49)
            {
                scrName = "<SCRIPT language='javascript'>alert('The investment should be more than 50 !!');" + "</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                txtAmount.Text = "";
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    protected void GrdDirects1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdDirects1.PageIndex = e.NewPageIndex;
        GrdDirects1.DataSource = Session["gridData"];
        GrdDirects1.DataBind();
    }
}
