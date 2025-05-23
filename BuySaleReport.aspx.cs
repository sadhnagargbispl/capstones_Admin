using AjaxControlToolkit;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class BuySaleReport : System.Web.UI.Page
{
    DAL obj = new DAL();
    string isoStart;
    string isoEnd;
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                Fun_Sp_GetCryptoAPIFor_FundWithdraw_Cpanel();
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
    private void Fun_Sp_GetCryptoAPIFor_FundWithdraw_Cpanel()
    {
        try
        {
            string sql = string.Empty;
            DataTable dt_API_MAster = new DataTable();
            sql = obj.IsoStart + " Exec Sp_GetCryptoAPIFor_FundWithdraw_Cpanel " + obj.IsoEnd;
            dt_API_MAster = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            Session["BHTSSINGLEPAYOUT"] = dt_API_MAster.Rows[2]["APIURL"];
            Session["BHTSBALANCE"] = dt_API_MAster.Rows[4]["APIURL"];
            Session["STATUSCHECK"] = dt_API_MAster.Rows[5]["APIURL"];
            Session["BUYSIGLEPAYOUT"] = dt_API_MAster.Rows[6]["APIURL"];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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

            DataTable Dt_GetApi = new DataTable();
            string sql = "exec sp_GetBuySellReport '" + ID + "','" + startDate + "', '" + endDate + "','" + ddllist.SelectedValue + "'";
            Dt_GetApi = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (ddllist.SelectedValue == "B")
            {
                GridView1.DataSource = Dt_GetApi;
                GridView1.DataBind();
                GvData.Visible = false;
                GridView1.Visible = true;
            }
            else
            {
                GvData.DataSource = Dt_GetApi;
                GvData.DataBind();
                GvData.Visible = true;
                GridView1.Visible = false;
            }
            Session["GData"] = Dt_GetApi;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
            DataTable Dt_GetApi = new DataTable();
            string sql = "exec sp_GetBuySellReport '" + ID + "','" + startDate + "', '" + endDate + "','" + ddllist.SelectedValue + "'";
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
                Response.AddHeader("content-disposition", "attachment;filename=BuySellTransactionReport.xlsx");
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
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = Session["GData"];
            GridView1.DataBind();
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
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GData"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void ddllist_SelectedIndexChanged(object sender, EventArgs e)
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
    protected void DeleteGroup(object sender, EventArgs e)
    {
        string sqlStr = "";
        string sqlRes = "";
        string HdnBHTSAmount, Hdnformno1, HdnFromAddress, hdnTxnHash, scrName;
        try
        {
            GridViewRow gvRw = (GridViewRow)((System.Web.UI.Control)sender).NamingContainer;
            HdnBHTSAmount = ((Label)gvRw.FindControl("HdnBHTSAmount")).Text;
            Hdnformno1 = ((Label)gvRw.FindControl("Hdnformno")).Text;
            HdnFromAddress = ((Label)gvRw.FindControl("HdnFromAddress")).Text;
            hdnTxnHash = ((Label)gvRw.FindControl("HdnBHTSTXNHash")).Text;
            string sResult = "";
            string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random_number = new Random().Next(0, 999);
            string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
            sResult = formatted_datetime;
            DataTable dt = new DataTable();
            if (dt.Rows.Count == 0)
            {
                //string tokenResponse = Fund_Token_Send(hdnWalletAddress, hdnPrivateKey, hdnAmount, hdnFormNo);
                string sqlResToken = string.Empty;

                decimal apiTotalAmount = CalculateTotalRate(decimal.Parse(HdnBHTSAmount, System.Globalization.NumberStyles.Float));
                string tokenResponse = DeductWalletApi(Hdnformno1, apiTotalAmount, HdnFromAddress, sResult, decimal.Parse(HdnBHTSAmount, System.Globalization.NumberStyles.Float), hdnTxnHash);
                if (tokenResponse.ToUpper().Trim() == "SUCCESS")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Sell Transaction Successfully submitted, Please Check Your Wallet.!');location.replace('BuySaleReport.aspx');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Payment Failed.!');location.replace('BuySaleReport.aspx');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Already Clear.!');location.replace('BuySaleReport.aspx');", true);
            }
        }
        catch (Exception ex)
        {
            string errorQry = "INSERT INTO TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData)VALUES('" + ex.Message + "',GETDATE(), '','" + sqlStr.Trim() + "','" + sqlRes + "')";
            int x1 = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry);
        }

        BindData(1);
    }
    public string DeductWalletApi(string Formno_V, decimal withdrawlAmount, string WalletAddress, string ReqNo, decimal BHTSAmount, string BHTShash)
    {
        DataTable dtQuery = new DataTable();
        string sResponseFromServer1 = string.Empty;
        string StatusApi = "";
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        int i = 0;
        string hash_ = "";
        string postData = "";
        string bamount = BHTSAmount.ToString();
        string amount = withdrawlAmount.ToString();
        decimal apiTotalAmount = decimal.Parse(amount, System.Globalization.NumberStyles.Float);
        string URL;
        try
        {
            DataSet dslogin = new DataSet();
            string str = string.Empty;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                if (ddllist.SelectedValue == "B")
                {
                    URL = Session["BUYSIGLEPAYOUT"].ToString();
                }
                else
                {
                    URL = Session["BHTSSINGLEPAYOUT"].ToString();
                }

                WebRequest tRequest = WebRequest.Create(URL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                postData = $"{{\"to\":\"{WalletAddress.Trim()}\",\"amount\":\"{apiTotalAmount.ToString()}\",\"txId\":\"{sResult.Trim()}\"}}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Formno, Request, postdata, Req_From, Rate, BhtsAmount, BhtsHash) ";
                sql_req += "VALUES('" + ReqNo.Trim() + "', '" + Formno_V.Trim() + "', '" + URL.Trim() + "', '" + postData.Trim() + "', 'SinglePayoutUSDT', '" + HdnApiRate.Value + "', '" + bamount.Trim() + "', '" + BHTShash.Trim() + "')";
                int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStream = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStream))
                        {
                            str = tReader.ReadToEnd();
                        }
                    }
                }
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + str.Trim() + "' WHERE ReqID = '" + ReqNo.Trim() + "' AND Req_From = 'SinglePayoutUSDT'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res));
                dslogin = ConvertJsonStringToDataSet(str);
                string resultStatus = str != "" ? dslogin.Tables[0].Rows[0]["Status"].ToString() : "failed";
                if (resultStatus.ToUpper() == "FAILED")
                {
                    try
                    {
                        hash_ = dslogin.Tables[0].Rows[0]["txhash"].ToString();
                    }
                    catch { }
                }
                else if (resultStatus.ToUpper() == "SUCCESS")
                {
                    hash_ = dslogin.Tables[0].Rows[0]["txhash"].ToString();
                }

                string strs = "INSERT INTO ApiReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response, ApiStatus, RectimeStamp, ApiType, TxnHash, AMount, PostData, TypeB) ";
                strs += "VALUES('" + Formno_V + "', '" + WalletAddress + "', '" + WalletAddress + "', '" + WalletAddress + "', '" + URL + "', '" + str + "', '" + StatusApi + "', GETDATE(), 'Token Payout', '" + hash_ + "', ";
                strs += "'" + apiTotalAmount.ToString() + "', '" + postData + "', 'QrCode')";
                string Query = "BEGIN TRY BEGIN TRANSACTION " + strs + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Query));

                if (i > 0 && resultStatus.ToUpper() == "SUCCESS")
                {
                    StatusApi = resultStatus;
                }
            }
            catch (Exception ex)
            {
                string errorQry = "";
                string ErrorMsg = ex.Message;
                errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url,WalletAddress,PostData,formno) ";
                errorQry += "VALUES('" + ErrorMsg + "', GETDATE(), '" + Session["BHTSSINGLEPAYOUT"].ToString() + "', '" + WalletAddress.Trim() + "', '" + postData + "', '" + Formno_V + "'); ";
                errorQry += "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + ErrorMsg + "' WHERE ReqID = '" + ReqNo.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, errorQry));

                if (x_res > 0)
                {
                    StatusApi = "failed";
                }
            }
        }
        catch
        {
            StatusApi = "failed";
        }

        return StatusApi;
    }
    public decimal CalculateTotalRate(decimal amount)
    {
        decimal totalRate = 0m;
        decimal rate = 0m;

        try
        {
            string sql = "";
            DataTable dtApiMaster = new DataTable();
            sql = obj.IsoStart + "SELECT Gasfees FROM " + obj.DBName + "..GasFeesCheck WHERE statusapi = 'Y' " + obj.IsoEnd;
            dtApiMaster = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (dtApiMaster.Rows.Count > 0)
            {
                rate = Convert.ToDecimal(dtApiMaster.Rows[0]["Gasfees"]);
                HdnApiRate.Value = rate.ToString();
                if (ddllist.SelectedValue == "B")
                {
                    totalRate = amount / rate;
                }
                else
                {
                    totalRate = amount * rate;
                }

            }

            return totalRate;
        }
        catch (Exception)
        {
            return totalRate;
        }
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = JsonConvert.DeserializeXmlNode(jsonString);

        DataSet ds = new DataSet();
        using (XmlNodeReader xmlNodeReader = new XmlNodeReader(xd))
        {
            ds.ReadXml(xmlNodeReader);
        }

        return ds;
    }
}
