using System.Security.Claims;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using SAPbobsCOM;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SAPCORE.Models;
using Microsoft.Data.Sqlite;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/// <summary>
namespace TokenBasedAPI.Controllers
{
    public class TestController : ControllerBase
    {

        public SAPbobsCOM.Company oCompany = null;
        private static int nErr = 0;
        string querystring = "";
        private static string erMsg = "";
        private readonly IConfigurationSection section;// = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
        //HttpResponseMessage response = null;
        string message = "";
        private static string userName;

        public static string Password;

        private static string Server;
        private static string DbServerType;
        private static string LicenseServer;
        private static string DbUserName;
        private static string DbPassword;
        private static string MessageFlag;
        private static string DBName;

        //private static string @"Database=" + DBName.Trim() + ";" = @"Database=" + DBName.Trim() + ";";
        private static string encrypt_decrypt_key = "b14ca5898a4e4133bbce2ea2315a1916";
        Recordset QueryObject = null;
        Recordset QueryObjectDocEntry = null;

        public IConfiguration Configuration { get; }

        public TestController(IConfiguration configuration)
        {
            section = configuration.GetSection("SAPConfig");
            var value = configuration["SAPConfig:userName"];
            using(var context = new SetupConfigContext())
            {
                userName = context.SetupConfig.Where(c=>c.Name=="userName").Select(c=>c.Value)?.FirstOrDefault()??section["userName"];// Password
                Password = context.SetupConfig.Where(c => c.Name == "password").Select(c => c.Value)?.FirstOrDefault() ?? section["password"];// Password
                Server = context.SetupConfig.Where(c => c.Name == "Server").Select(c => c.Value)?.FirstOrDefault() ?? section["Server"];
                DbServerType = context.SetupConfig.Where(c => c.Name == "DbServerType").Select(c => c.Value).FirstOrDefault() ?? section["DbServerType"]; ;
                LicenseServer = context.SetupConfig.Where(c => c.Name == "LicenseServer").Select(c => c.Value).FirstOrDefault() ?? section["LicenseServer"]; ; 
                DbUserName = context.SetupConfig.Where(c => c.Name == "DbUserName").Select(c => c.Value).FirstOrDefault() ?? section["DbUserName"]; ;
                DbPassword = context.SetupConfig.Where(c => c.Name == "DbPassword").Select(c => c.Value).FirstOrDefault() ?? section["DbPassword"]; ;
                MessageFlag = context.SetupConfig.Where(c => c.Name == "MessageFlag").Select(c => c.Value).FirstOrDefault() ?? section["MessageFlag"]; ;
                DBName = context.SetupConfig.Where(c => c.Name == "CompanyDB").Select(c => c.Value).FirstOrDefault() ?? section["CompanyDB"]; ;
            }
            
            Configuration = configuration;
        }


        // //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetCurrencies")]
        public IActionResult GetCurrencies(string DBName)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".OCRN\"  ";

                    }
                    else
                    {
                        querystring = "select * FROM   " + Sanitize(DBName) + ".[dbo]." + "OCRN  ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                //return dt;
                                
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Problem( ex.Message);
            }

        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetMarketingDocumentOwner")]
        public IActionResult GetMarketingDocumentOwner(string DBName)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".OHEM\"  ";

                    }
                    else
                    {
                        querystring = "select * FROM   " + Sanitize(DBName) + ".[dbo]." + "OHEM  ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                //return dt;
                                
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetMarketingDocumentSalesPerson")]
        public IActionResult GetMarketingDocumentSalesPerson(string DBName)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".OSLP\"  ";

                    }
                    else
                    {
                        querystring = "select * FROM   " + Sanitize(DBName) + ".[dbo]." + "OSLP  ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }
        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetBusinessPartnersGroupCode")]
        public IActionResult GetBusinessPartnersGroupCode(string DBName)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select T1.CardType,T0.* FROM   \"" + Sanitize(DBName) + "\" + \".OCRG\"  T0  INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OCRD T1 ON T0.GroupCode = T0.GroupCode ";

                    }
                    else
                    {
                        querystring = "select T1.CardType,T0.*  FROM   " + Sanitize(DBName) + ".[dbo]." + "OCRG T0  INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OCRD T1 ON T0.GroupCode = T0.GroupCode ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                //return dt;
                                
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetBusinessPartnerPaymentTerms")]
        public IActionResult GetBusinessPartnerPaymentTerms(string DBName)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".OCTG\"  ";

                    }
                    else
                    {
                        querystring = "select * FROM   " + Sanitize(DBName) + ".[dbo]." + "OCTG  ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }

        [HttpGet]
        [Route("api/SAP/{DBName}/GetHouseBanks")]
        public IActionResult GetHouseBanks(string DBName)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".DSC1\"  ";

                    }
                    else
                    {
                        querystring = "select * FROM   " + Sanitize(DBName) + ".[dbo]." + "DSC1  ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }
        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/GetSAPCompanies")]
        public IActionResult GetSAPCompanies()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                var con = new SqlConnection(constr);
                {

                    var DBNameString = "[SBO-COMMON]";
                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select CompDbNam FROM   \"" + Sanitize(DBNameString.Replace("[", "").Replace("]", "")) + "\" + \".SRGC\"  ";

                    }
                    else
                    {
                        querystring = "select * FROM   " + Sanitize(DBNameString) + ".[dbo]." + "SRGC  ";

                    }
                    var cmd = new SqlCommand(querystring, con);
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        {
                            //cmd.Connection = con;
                            //sda.SelectCommand = cmd;
                            DataTable dt = new DataTable();
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = new HttpResponseMessage();
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }



        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetIncomingPayments")]
        public IActionResult GetIncomingPayments(string DBName)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM  " + "\"" + Sanitize(DBName) + "\"." + "\"ORCT\"  ";

                    }
                    else
                    {
                        querystring = "SELECT T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "ORCT T0 ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Payments";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }



        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetOutgoingPayments")]
        public IActionResult GetOutgoingPayments(string DBName)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM  " + "\"" + Sanitize(DBName) + "\"." + "\"OVPM\"  ";

                    }
                    else
                    {
                        querystring = "SELECT T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "OVPM T0 ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Payments";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetIncomingPaymentsPaginated/{CardCode}/{Page}/{RequestLimit}")]
        public IActionResult GetIncomingPaymentsPaginated(string DBName, string CardCode, string Page, string RequestLimit)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM  " + "\"" + Sanitize(DBName) + "\"." + "\"ORCT\"  ";

                    }
                    else
                    {
                        querystring = "SELECT T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "ORCT T0 " +
                             " WHERE   T0.CardCode = '" + Sanitize(CardCode) + "' ORDER BY T0.CardCode asc OFFSET " + Sanitize(Page) + "  ROWS FETCH NEXT " + Sanitize(RequestLimit) + " ROWS ONLY ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Payments";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }



        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetOutgoingPaymentsPaginated/{CardCode}/{Page}/{RequestLimit}")]
        public IActionResult GetOutgoingPaymentsPaginated(string DBName, string CardCode, string Page, string RequestLimit)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM  " + "\"" + Sanitize(DBName) + "\"." + "\"OVPM\"  ";

                    }
                    else
                    {
                        querystring = "SELECT T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "OVPM T0 " +
                            " WHERE   T0.CardCode = '" + Sanitize(CardCode) + "' ORDER BY T0.CardCode asc OFFSET " + Sanitize(Page) + "  ROWS FETCH NEXT " + Sanitize(RequestLimit) + " ROWS ONLY ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Payments";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }

        [HttpGet]
        [Route("api/SAP/{DBName}/GetIncomingPaymentsPaginated/{Page}/{RequestLimit}")]
        public IActionResult GetIncomingPaymentsPaginated(string DBName, string Page, string RequestLimit)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM  " + "\"" + Sanitize(DBName) + "\"." + "\"ORCT\"  ";

                    }
                    else
                    {
                        querystring = "SELECT T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "ORCT T0 " +
                             " ORDER BY T0.CardCode asc OFFSET " + Sanitize(Page) + "  ROWS FETCH NEXT " + Sanitize(RequestLimit) + " ROWS ONLY ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Payments";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }



        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetOutgoingPaymentsPaginated/{Page}/{RequestLimit}")]
        public IActionResult GetOutgoingPaymentsPaginated(string DBName, string Page, string RequestLimit)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM  " + "\"" + Sanitize(DBName) + "\"." + "\"OVPM\"  ";

                    }
                    else
                    {
                        querystring = "SELECT T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "OVPM T0 " +
                            " ORDER BY T0.CardCode asc OFFSET " + Sanitize(Page) + "  ROWS FETCH NEXT " + Sanitize(RequestLimit) + " ROWS ONLY ";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Payments";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetCompanyDetails")]
        // [Route("api/SAP/GetIncomingPayments")]
        public IActionResult GetCompanyDetails(string DBName)
        {
            //try
            //{
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

            // string constr = (ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            //string constr = (DecryptString(encrypt_decrypt_key, "ics4rk5x7PdAhKGpSaI9EQ7QnVLudfbvP4/KbNqyZkPkdNy7+oxOCjueMpEDCpVjzFqWuH/kencobsf/QAIJOef+fFD+VLplL8ba96ECFeLmJaeg8hyK+ginTQRH5LTf"));
            // string constr = @"Driver ={ SQL Server}; Server = DESKTOP - NL6OLU8; Database = SBODEMOGB; Uid = sap; Pwd = sekonda1; MultipleActiveResultSets = True;";
            using (SqlConnection con = new SqlConnection(constr))
            {


                if (DbServerType == "SAPHANA")
                {
                    querystring = "SELECT T0.CompnyName, T0.CompnyAddr, T0.Country, T0.Phone1, T0.Phone2, T0.E_Mail, T0.MainCurncy, T0.SysCurrncy, T0.TaxIdNum FROM " + Sanitize(DBName) + ".[dbo]." + "OADM T0";

                }
                else
                {
                    querystring = "SELECT T0.CompnyName, T0.CompnyAddr, T0.Country, T0.Phone1, T0.Phone2, T0.E_Mail, T0.MainCurncy, T0.SysCurrncy, T0.TaxIdNum FROM " + Sanitize(DBName) + ".[dbo]." + "OADM T0";


                }
                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Payments";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                            // return Request.CreateResponse(HttpStatusCode.Created, customers);
                        }
                    }
                }


            }
            //}
            //catch (Exception ex)
            //{

            //    HttpResponseMessage exeption_response = null;
            //    exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            //    return Problem(ex.Message);
            //}

        }





        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetIncomingPaymentsByDate/{FromDocDate}/{ToDocDate}")]
        public IActionResult GetIncomingPaymentsByDate(string DBName, string FromDocDate, string ToDocDate)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".ORCT\"  " +
                                     " WHERE  \"DocDate\" BETWEEN '" + Sanitize(FromDocDate) + "' and  '" + Sanitize(ToDocDate) + "'";

                    }
                    else
                    {
                        querystring = "select  T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                                    " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                                    "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                                    "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                                    " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                                    " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                                    " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                                    "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                                    " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                                    "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "ORCT T0 " +
                                    " WHERE  DocDate  BETWEEN '" + Sanitize(FromDocDate) + "' and  '" + Sanitize(ToDocDate) + "'";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Payments";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetOutgoingPaymentsByDate/{FromDocDate}/{ToDocDate}")]
        public IActionResult GetOutgoingPaymentsByDate(string DBName, string FromDocDate, string ToDocDate)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".OVPM\"  " +
                                     " WHERE  \"DocDate\" BETWEEN '" + Sanitize(FromDocDate) + "' and  '" + Sanitize(ToDocDate) + "'";

                    }
                    else
                    {
                        querystring = "select  T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "OVPM T0 " +
                            " WHERE  DocDate  BETWEEN '" + Sanitize(FromDocDate) + "' and  '" + Sanitize(ToDocDate) + "'";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Payments";
                                sda.Fill(dt);
                                //return dt;
                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetIncomingPaymentsByDocEntry/{DocEntry}")]
        public IActionResult GetIncomingPaymentsByDocEntry(string DBName, string DocEntry)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".ORCT\"  " +
                                     " WHERE  \"DocEntry\" = '" + Sanitize(DocEntry) + "'";

                    }
                    else
                    {
                        querystring = "select  T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "ORCT T0 " +
                            " WHERE  T0.DocEntry = '" + Sanitize(DocEntry) + "'";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                //return dt;
                                string results = DataTableToJSONWithStringBuilder(dt);

                                return Ok(dt);
                                // return Request.CreateResponse(HttpStatusCode.Created, customers);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetOutgoingPaymentsByDocEntry/{DocEntry}")]
        public IActionResult GetOutgoingPaymentsByDocEntry(string DBName, string DocEntry)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var roles = identity.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);
                string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                using (SqlConnection con = new SqlConnection(constr))
                {


                    if (DbServerType == "SAPHANA")
                    {
                        querystring = "select * FROM   \"" + Sanitize(DBName) + "\" + \".OVPM\"  " +
                                     " WHERE  \"DocEntry\" = '" + Sanitize(DocEntry) + "'";

                    }
                    else
                    {
                        querystring = "select  T0.DocEntry,T0.DocNum, T0.DocType,T0.Canceled, T0.Handwrtten, T0.Printed, T0.DocDate," +
                            " T0.DocDueDate, T0.CardCode, T0.CardName, T0.DdctPrcnt, T0.DdctSum, T0.DdctSumFC, T0.CashAcct, " +
                            "T0.CashSum, T0.CashSumFC, T0.CreditSum, T0.CredSumFC, T0.CheckAcct, T0.CheckSum, T0.CheckSumFC, T0.TrsfrAcct, " +
                            "T0.TrsfrSum, T0.TrsfrSumFC, T0.TrsfrDate, T0.TrsfrRef, T0.PayNoDoc, T0.NoDocSum, T0.NoDocSumFC, T0.DocCurr," +
                            " T0.DocRate, T0.SysRate, T0.DocTotal, T0.DocTotalFC, T0.Ref1, T0.Ref2, T0.CounterRef, T0.Comments, T0.JrnlMemo," +
                            " T0.TransId, T0.DocTime, T0.ShowAtCard, T0.CntctCode, T0.DdctSumSy, T0.CashSumSy, T0.CredSumSy, T0.CheckSumSy," +
                            " T0.TrsfrSumSy, T0.NoDocSumSy, T0.DocTotalSy, T0.StornoRate, T0.UpdateDate, T0.CreateDate, T0.TaxDate, T0.Series, " +
                            "T0.BankCode, T0.BankAcct, T0.VatGroup, T0.VatSum, T0.VatSumFC, T0.VatSumSy, T0.FinncPriod, T0.VatPrcnt, T0.Dcount," +
                            " T0.DcntSum, T0.DcntSumFC, T0.WtCode, T0.WtSum, T0.WtSumFrgn, T0.WtSumSys, T0.Proforma, T0.BpAct, T0.BcgSum, " +
                            "T0.BcgSumFC, T0.BcgSumSy, T0.PayToCode, T0.IsPaytoBnk, T0.PBnkCnt, T0.PBnkCode, T0.PBnkAccnt FROM " + Sanitize(DBName) + ".[dbo]." + "OVPM T0 " +
                            " WHERE  T0.DocEntry = '" + Sanitize(DocEntry) + "'";

                    }
                    using (SqlCommand cmd = new SqlCommand(querystring))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                dt.TableName = "Dataset";
                                sda.Fill(dt);
                                return Ok(dt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }

        }
        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetAccounts")]
        public IActionResult GetAccounts(string DBName)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
            using (SqlConnection con = new SqlConnection(constr))
            {

                if (DbServerType == "SAPHANA")
                {
                    querystring = "select T0.\"AcctCode\", T0.\"AcctName\" , T0.\"Postable\"  from  \"" + Sanitize(DBName) + "\" + \".OACT\" T0 ";
                }
                else
                {
                    querystring = "select T0.AcctCode, T0.AcctName,T0.Postable from  " + Sanitize(DBName) + ".[dbo]." + "oact T0(nolock)";

                }
                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 4000000;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetTaxes")]
        public IActionResult GetTaxes(string DBName)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
            using (SqlConnection con = new SqlConnection(constr))
            {


                if (DbServerType == "SAPHANA")
                {
                    querystring = "select T0.* from  \"" + Sanitize(DBName) + "\" + \".OVTG\" T0  where T0.\"Inactive\"= 'N'";
                }
                else
                {
                    querystring = "select T0.* from  " + Sanitize(DBName) + ".[dbo]." + "OVTG t0 (nolock) where  T0.Inactive='N'";

                }

                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }
        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetWitholdingTaxes")]
        public IActionResult GetWitholdingTaxes(string DBName)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
            using (SqlConnection con = new SqlConnection(constr))
            {


                if (DbServerType == "SAPHANA")
                {
                    querystring = "select T0.* from  \"" + Sanitize(DBName) + "\" + \".OWHT\" T0  ";
                }
                else
                {
                    querystring = "select T0.* from  " + Sanitize(DBName) + ".[dbo]." + "OWHT t0 (nolock) ";

                }

                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetWarehouses")]
        public IActionResult GetWarehouses(string DBName)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
            using (SqlConnection con = new SqlConnection(constr))
            {

                if (DbServerType == "SAPHANA")
                {
                    querystring = "select T0.\"WhsCode\", T0.\"WhsName\" from  \"" + Sanitize(DBName) + "\" + \".OWHS\" T0  where T0.\"Locked\"= 'N'";
                }
                else
                {
                    querystring = "select T0.WhsCode, T0.WhsName from  " + Sanitize(DBName) + ".[dbo]." + "OWHS t0 (nolock) where  T0.Locked='N'";

                }




                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetPriceslist")]
        public IActionResult GetPriceslist(string DBName)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
            using (SqlConnection con = new SqlConnection(constr))
            {

                if (DbServerType == "SAPHANA")
                {
                    querystring = "select *  from  \"" + Sanitize(DBName) + "\" + \".OPLN\"    ";
                }
                else
                {
                    querystring = "select * from  " + Sanitize(DBName) + ".[dbo]." + "OPLN (nolock) ";

                }

                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetItems")]
        public IActionResult GetItems(string DBName)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
            using (SqlConnection con = new SqlConnection(constr))
            {

                if (DbServerType == "SAPHANA")
                {
                    querystring = "SELECT T0.*,T1.\"Price\",T1.\"PriceList\" FROM   \"" + Sanitize(DBName) + "\" + \".OITM\" T0 ";

                }
                else
                {
                    querystring = "SELECT T0.ItemCode, T0.ItemName, T0.ItemType,T1.Price,T1.PriceList FROM   " + Sanitize(DBName) + ".[dbo]." + "OITM T0(nolock) INNER JOIN   " + Sanitize(DBName) + ".[dbo]." + "ITM1 T1 ON T0.ItemCode = T1.ItemCode ";

                }
                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 4000000;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }
        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/IsDBConnected")]
        public IActionResult Post39()
        {
            // try { 
            string message = "";
            //dynamic message_ = null;
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                if (con.State == ConnectionState.Open)
                {
                    erMsg = "Database connection was sucessfull";
                    message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"" + erMsg + "\",\"Connection Status\": \"Company\"}}";
                    //  message_ = JsonConvert.DeserializeObject(message);
                }
                //  Interaction.MsgBox("Connected to Licence server successfully");
                else
                {
                    erMsg = "Database connection was not sucessfull";

                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Connection Status\": \"Company\"}}";

                }
                con.Close();
                con.Dispose();
                // }
                message = message;
                return Ok(System.Text.Json.JsonSerializer.Deserialize<object>( message));
                MarshallObject(oCompany);
                //return response;
                //}

                //catch (Exception ex)
                //{
                //    // Console.WriteLine("Error writing app settings");
                //    message = ex.Message;
            }
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        // [Route("api/SAP/{DBName}/CheckGetPostStatus")]
        [Route("api/SAP/{DBName}/CheckGetPostStatus/{SAPUserName}/{SAPPassword}")]
        public IActionResult CheckGetPostStatus(string DBName, string SAPUserName, string SAPPassword)
        {
            //  try { 

            string message = "";
            dynamic dbstatus = null;


            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // , string SAPUserName, string SAPPassword
                AddUpdateAppSettings("CompanyDB", DBName);
                AddUpdateAppSettings("manager", SAPUserName);
                AddUpdateAppSettings("Password", SAPPassword);
                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                if ((0 == oCompany.Connect() && (con.State == ConnectionState.Open)))
                {
                    erMsg = "Both POST and GET method are ready ";
                    message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"" + erMsg + "\",\"Connection Status\": \"Company\"}}";

                }

                else if (0 != oCompany.Connect())
                {
                    int errcode;
                    oCompany.GetLastError(out nErr, out erMsg);
                    if (!erMsg.Contains("already connected"))
                    {

                        message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Connection Status\": \"Company\"}}";
                    }
                    else if (erMsg.Contains("already connected") && (con.State == ConnectionState.Open))
                    {

                        erMsg = "Both POST and GET method are ready ";
                        message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"" + erMsg + "\",\"Connection Status\": \"Company\"}}";


                    }

                }
                else
                {
                    erMsg = "GET method is not ready there is a problem with connection string";
                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Connection Status\": \"Company\"}}";

                }
            }
            return Ok(System.Text.Json.JsonSerializer.Deserialize<object>(message));


        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]

        [Route("api/SAP/{DBName}/IsCompanyConnected/{SAPUserName}/{SAPPassword}")]
        public IActionResult IsCompanyConnected(string DBName, string SAPUserName, string SAPPassword)
        {

            try
            {
                //var response =null;

                // dynamic message_ = null;


                AddUpdateAppSettings("CompanyDB", DBName);
                AddUpdateAppSettings("manager", SAPUserName);
                AddUpdateAppSettings("Password", SAPPassword);

                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                ApiResponse message = new ApiResponse();
                if ((0 == oCompany.Connect()))
                {
                    erMsg = "You have successfully  connected to Company " + oCompany.CompanyName;
                    message = new ApiResponse { Message=new SAPCORE.Models.Message { MessageType="Success", Description=erMsg } };
                    //  message_ = JsonConvert.DeserializeObject(message);
                }
                //  Interaction.MsgBox("Connected to Licence server successfully");
                else
                {
                    // Console.WriteLine("Error");
                    //  Interaction.MsgBox("failed  to connect  to to Licence server ");
                    int errcode;
                    oCompany.GetLastError(out nErr, out erMsg); erMsg = Sanitize_Errors(erMsg);
                    if (erMsg.Contains("already connected"))
                    {
                        message = new ApiResponse { Message = new SAPCORE.Models.Message { MessageType = "Success", Description = oCompany.CompanyName } };
                    }
                    else
                    {
                        message = new ApiResponse { Message = new SAPCORE.Models.Message { MessageType = "Error", Description = erMsg } };
                    }

                }
                return Ok(message);
                MarshallObject(oCompany);

            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error writing app settings");
                return Problem(ex.Message);
            }
        }

        public static string AddUpdateAppSettings(string key, string value)
        {
            string message = "";
            try
            {
                using(var context = new SetupConfigContext())
                {
                    var config = context.SetupConfig.FirstOrDefault(c => c.Name == key);
                    
                    if(config==null){
                        context.SetupConfig.Add(new SetupConfig { Name = key, Value = value });
                    }
                    else
                    {
                        config.Value = value;
                    }
                    context.SaveChanges();
                    
                }
               // Configuration configFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
               // var settings = configFile.AppSettings.Settings;

               // //value = EncryptString(encrypt_decrypt_key, value);
               // // var settings_1 = configFile.ConnectionStrings.Settings;
               // ConnectionStringSettings settings_ =
               //ConfigurationManager.ConnectionStrings["name"];

               // if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
               // {
               //     //if (key.Contains("Password"))
               //     //    {
               //     //    //value = EncryptString(encrypt_decrypt_key,value);
               //     //    value =  value;
               //     //}
               //     if (settings[key] == null)
               //     {
               //         settings.Add(key, value);
               //     }
               //     else
               //     {
               //         settings[key].Value = value;
               //     }
               //     configFile.Save(ConfigurationSaveMode.Modified);
               //     ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

               // }
               // else
               // {

               //     message = "Key does not Exist";
               // }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error writing app settings");
                message = ex.Message;
            }

            return message;
        }


        static string AddUpdateConfigurationSettings(string Name, string value)
        {
            string message = "";
            try
            {
                using (var context = new SetupConfigContext())
                {
                    var config = context.SetupConfig.FirstOrDefault(c => c.Name == Name);

                    if (config == null)
                    {
                        context.SetupConfig.Add(new SetupConfig { Name = Name, Value = value });
                    }
                    else
                    {
                        config.Value = value;
                    }
                    context.SaveChanges();

                }
                //Configuration configFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                //var settings = configFile.AppSettings.Settings;

                //ConnectionStringSettings settings_ = ConfigurationManager.ConnectionStrings[Name];

                //if (!(settings_ == null))
                //{

                //    // value = EncryptString(encrypt_decrypt_key, value);
                //    // configFile.ConnectionStrings.ConnectionStrings.Clear(new ConnectionStringSettings(Name, value))
                //    configFile.ConnectionStrings.ConnectionStrings["constr"].ConnectionString = value;
                //    configFile.Save(ConfigurationSaveMode.Modified, true);
                //    ConfigurationManager.RefreshSection("connectionStrings");
                //}
                //else
                //{
                //    //settings_[Name].Value = value;

                //    message = "Key does not Exist";
                //}

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error writing app settings");
                message = ex.Message;
            }

            return message;
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            var string_value = streamReader.ReadToEnd();
                            return string_value;
                        }
                    }
                }
            }
        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/UpdateWebConfig/{type}/{key}/{value}")]
        public IActionResult UpdateWebConfig(string type, string key, string value)
        {
            string message = "";
            if (type == "connectionStrings")
            {

                message = AddUpdateConfigurationSettings(key, value);

            }
            else if (type == "appSettings")
            {

                message = AddUpdateAppSettings(key, value);

            }


            if (string.IsNullOrEmpty(message))
            {
                message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"Successfully updated configuration file\",\"Document Type\": \"configuration\"}}";

            }
            else
            {
                message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"Key does not exist in the configuration file\",\"Document Type\": \"configuration\"}}";


            }
            return Ok(System.Text.Json.JsonSerializer.Deserialize<object>(message));
            // MarshallObject(oCompany);
            //return response;
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetAvailableQuantity/{ItemCode}/{WarehouseCode}/{Quantity}")]
        public IActionResult GetAvailableQuantity(string DBName, string ItemCode, string WarehouseCode, string Quantity)
        {

            //ItemCode = ItemCode.Replace(";", "");
            //WarehouseCode = WarehouseCode.Replace(";", "");
            bool is_Numeric = IsNumeric(Quantity);
            //if (is_Numeric == true)
            //{ 


            if (DbServerType == "SAPHANA")
            {
                querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from  \"" + Sanitize(DBName) + "\" + \".OITM\" T0   INNER JOIN  \"" + Sanitize(DBName) + "\" + \".OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";

            }
            else
            {
                querystring = "select case WHEN T1.OnHand  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS QuantityOk from " + Sanitize(DBName) + ".[dbo]." + "OITM T0  (nolock) INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OITW  T1  ON T0.ItemCode = T1.ItemCode INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OWHS T2 ON T2.WhsCode = T1.WhsCode WHERE  T0.ItemCode = '" + Sanitize(ItemCode) + "' and T2.WhsCode = '" + Sanitize(WarehouseCode) + "'";

            }
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

            //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString  + @"Database=" + DBName.Trim() + ";" ;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }
        }



        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetAvailableQuantityBatch/{ItemCode}/{WarehouseCode}/{Quantity}")]
        public IActionResult GetAvailableQuantityBatch(string DBName, string ItemCode, string WarehouseCode, string Quantity)
        {

            //ItemCode = ItemCode.Replace(";", "");
            //WarehouseCode = WarehouseCode.Replace(";", "");
            bool is_Numeric = IsNumeric(Quantity);
            //if (is_Numeric == true)
            //{ 


            if (DbServerType == "SAPHANA")
            {
                querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from  \"" + Sanitize(DBName) + "\" + \".OITM\" T0   INNER JOIN  \"" + Sanitize(DBName) + "\" + \".OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";

            }
            else
            {
                //querystring = "select case WHEN T1.OnHand  >= '" + Sanitize(Quantity) + "' THEN " +
                //    " 'Y' ELSE 'N' END AS QuantityOk from " + Sanitize(DBName) + ".[dbo]." + "OITM T0  (nolock) INNER JOIN "
                //    + Sanitize(DBName) + ".[dbo]." + "OITW  T1  ON T0.ItemCode = T1.ItemCode INNER JOIN " + Sanitize(DBName) + ".[dbo]." + 
                //    "OWHS T2 ON T2.WhsCode = T1.WhsCode WHERE  T0.ItemCode = '" + Sanitize(ItemCode) + "' and T2.WhsCode = '" + Sanitize(WarehouseCode) + "'";

                querystring = "SELECT distinct case WHEN T3.OnHand  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS  QuantityOk " +
                " FROM " + Sanitize(DBName) + ".[dbo]." + "OITM T0  INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OBTN T1 ON T0.ItemCode = T1.ItemCode " +
                " INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "IBT1 T5 ON T0.ItemCode = T5.ItemCode" +
                " INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OITW T3 ON T0.ItemCode = T3.ItemCode" +
                " INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OWHS T4 ON T3.WhsCode = T4.WhsCode WHERE T0.ItemCode = '" + Sanitize(ItemCode) + "'" +
                " AND T4.WhsCode = '" + Sanitize(WarehouseCode) + "'";


                //querystring = "SELECT DISTINCT T0.ItemCode,  T3.OnHand 'ItemAvailableQuantiy', T1.DistNumber, T5.Quantity 'QuantityPerBatch', T4.WhsCode " +
                // " FROM " + Sanitize(DBName) + ".[dbo]." + "OITM T0  INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OBTN T1 ON T0.ItemCode = T1.ItemCode " +
                // " INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "IBT1 T5 ON T0.ItemCode = T5.ItemCode" +
                // " INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OITW T3 ON T0.ItemCode = T3.ItemCode" +
                // " INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OWHS T4 ON T3.WhsCode = T4.WhsCode WHERE T0.ItemCode = '" + Sanitize(ItemCode) + "'" +
                // " AND T4.WhsCode = = '" + Sanitize(WarehouseCode) + "'";
            }
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

            //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString  + @"Database=" + DBName.Trim() + ";" ;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);

                        }
                    }
                }
            }
        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetItemBatchesAndQuantities/{ItemCode}/{WarehouseCode}")]
        public IActionResult GetItemBatchesAndQuantities(string DBName, string ItemCode, string WarehouseCode)
        {

            //ItemCode = ItemCode.Replace(";", "");
            //WarehouseCode = WarehouseCode.Replace(";", "");
            //bool is_Numeric = IsNumeric(Quantity);
            //if (is_Numeric == true)
            //{ 


            if (DbServerType == "SAPHANA")
            {
                querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(ItemCode) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from  \"" + Sanitize(DBName) + "\" + \".OITM\" T0   INNER JOIN  \"" + Sanitize(DBName) + "\" + \".OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";

            }
            else
            {


                //querystring = "SELECT distinct case WHEN T3.OnHand  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS  QuantityOk " +
                //" FROM " + Sanitize(DBName) + ".[dbo]." + "OITM T0  INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OBTN T1 ON T0.ItemCode = T1.ItemCode " +
                //" INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "IBT1 T5 ON T0.ItemCode = T5.ItemCode" +
                //" INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OITW T3 ON T0.ItemCode = T3.ItemCode" +
                //" INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OWHS T4 ON T3.WhsCode = T4.WhsCode WHERE T0.ItemCode = '" + Sanitize(ItemCode) + "'" +
                //" AND T4.WhsCode = '" + Sanitize(WarehouseCode) + "'";


                querystring = "select M.BatchNum,sum(M.Quantity)'Quantity' from " +
                " (SELECT BatchNum, -Quantity 'Quantity'   FROM " + Sanitize(DBName) + ".[dbo]." + "IBT1  T0 WHERE  T0.ItemCode = '" + Sanitize(ItemCode) + "'  AND  T0.WhsCode = '" + Sanitize(WarehouseCode) + "'   and  Direction = '1' " +
                "  union all    SELECT BatchNum, Quantity 'Quantity'   FROM " + Sanitize(DBName) + ".[dbo]." + "IBT1  T0 WHERE  T0.ItemCode = '" + Sanitize(ItemCode) + "'  AND  T0.WhsCode = '" + Sanitize(WarehouseCode) + "'   and  Direction = '0' " +
                "   )M  group by M.BatchNum " +
                 "  order by Quantity desc";
            }
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";

            //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString  + @"Database=" + DBName.Trim() + ";" ;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetItemPrice/{ItemCode}/{CardCode}")]
        public IActionResult GetItemPrice(string DBName, string ItemCode, string CardCode)
        {



            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "SELECT T0.\"ItemCode\", T0.\"Price\" ,T0.\"PriceList\" FROM  \"" + Sanitize(DBName) + "\" + \".ITM1\" T0 INNER JOIN  \"" + Sanitize(DBName) + "\" + \".OCRD\" T1 ON T0.\"PriceList\" = T1.\"ListNum\"  WHERE T1.\"CardCode\" = '" + Sanitize(CardCode) + "' AND  T0.\"ItemCode\"='" + Sanitize(ItemCode) + "'AND T0.\"Price\" != 0";

            }
            else
            {
                querystring = "SELECT T0.ItemCode, T0.Price ,T0.PriceList FROM  " + Sanitize(DBName) + ".[dbo]." + "ITM1 T0 (nolock)INNER JOIN  " + Sanitize(DBName) + ".[dbo]." + "OCRD T1 ON T0.PriceList = T1.ListNum  WHERE T1.CardCode = '" + Sanitize(CardCode) + "' AND  T0.ItemCode='" + Sanitize(ItemCode) + "'AND T0.Price <> 0";

            }

            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            string constr = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(querystring))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Dataset";
                            sda.Fill(dt);
                            //return dt;
                            return Ok(dt);
                        }
                    }
                }
            }

        }


        public class Connect_To_SAP
        {
            public SAPbobsCOM.Company vCompany = null;
            private static int errcode = 0;
            private static string erMsg = "";
            // public sap
            public SAPbobsCOM.Company ConnectSAPDB(string DBName, string SAPUserName, string SAPPassword)
            {


                vCompany = new SAPbobsCOM.Company();


                string UserName = SAPUserName;
                string Password = SAPPassword;
                //section["Password"];
                //DecryptString(encrypt_decrypt_key, section["Password"]);
                string CompanyDB = DBName;
                //section["CompanyDB"];

                //DecryptString(encrypt_decrypt_key, section["DbPassword"]);
                // string DbPassword = "sekonda";
                vCompany.Server = Server;
                // "DESKTOP-NL6OLU8";
                if (DbServerType == "MSSQL2012")
                { vCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012; }
                else if (DbServerType == "MSSQL2014")
                { vCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014; }
                else if (DbServerType == "MSSQL2016")
                { vCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016; }
                else if (DbServerType == "MSSQL2017")
                { vCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017; }
                else if (DbServerType == "MSSQL2019")
                { vCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019; }
                else if (DbServerType == "SAPHANA")
                {
                    //vCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                    vCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                }

                vCompany.LicenseServer = LicenseServer;
                //"DESKTOP-NL6OLU8:30000";
                vCompany.UserName = UserName;
                // "manager";
                vCompany.Password = Password;
                //"sekonda";
                //'SBODEMOGB
                //P@ssw0r#?
                vCompany.CompanyDB = CompanyDB;
                //"SBODEMOGB";
                vCompany.language = SAPbobsCOM.BoSuppLangs.ln_English;
                vCompany.DbUserName = DbUserName;
                //"sa";
                vCompany.DbPassword = DbPassword;
                //"sekonda";


                if ((0 == vCompany.Connect()))
                {
                    Console.WriteLine("success");
                }
                //  Interaction.MsgBox("Connected to Licence server successfully");
                else
                {
                    // Console.WriteLine("Error");
                    //  Interaction.MsgBox("failed  to connect  to to Licence server ");

                    string message = "";
                    vCompany.GetLastError(out nErr, out erMsg);
                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": " + erMsg + ",\"Document Type\": \"Customer\"}}";
                    //  message_ = JsonConvert.DeserializeObject(message);
                }
                return vCompany;
            }

        }
        public class MarketingDocument_Rows
        {
            public string ItemCode { get; set; }
            public string Dscription { get; set; }
            public string Quantity { get; set; }
            public string WhsCode { get; set; }
            public string LineTotal { get; set; }
            public string PriceBefDi { get; set; }
            public string Price { get; set; }
            public string VatGroup { get; set; }
            public string VatSum { get; set; }
            public string DocEntry { get; set; }
        }

        public class Payment_Rows
        {
            public string DocEntry { get; set; }
            public string DocNum { get; set; }
            public string DocTotal { get; set; }
            public string PaymentMode { get; set; }



        }

        public class MarketingDocument_Details
        {
            public int RecordCount { get; set; }
            public int RecordLimit { get; set; }
            public int CurrentPage { get; set; }

            public int NumberofPages { get; set; }

        }

        public class MarketingDocumentHeader
        {
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string MarketingDocument { get; set; }
            public string DocDate { get; set; }
            public string DocDueDate { get; set; }
            public string TaxDate { get; set; }

            public string DocType { get; set; }
            public string CANCELED { get; set; }
            public string DocStatus { get; set; }
            public string DocTotal { get; set; }
            public string VatSum { get; set; }
            public string DocDiscount { get; set; }
            public string DocNum { get; set; }
            public string DocCur { get; set; }
            public string DocEntry { get; set; }
            public string Remarks { get; set; }
            public List<MarketingDocument_Rows> MarketingDocument_Rows { get; set; }
            public List<Payment_Rows> Payment_Rows { get; set; }

        }

        [HttpGet]
        [Route("api/SAP/{DBName}/GetMarketingDocumentsByCardCode/{ObjectType}/{CardCode}/{Page}/{RequestLimit}/")]
        public IActionResult GetMarketingDocumentsByCardCode(string DBName, string ObjectType, string CardCode, string Page, string RequestLimit)


        {
            string HeaderTable = "";
            string RowsTable = "";
            if (ObjectType == "SALESQOUTATION")
            {
                HeaderTable = "OQUT";
                RowsTable = "QUT1";
            }
            else if (ObjectType == "SALESORDER")
            {

                HeaderTable = "ORDR";
                RowsTable = "RDR1";
            }
            else if (ObjectType == "SALESCREDITNOTE")
            {


                HeaderTable = "ORIN";
                RowsTable = "RIN1";
            }
            else if (ObjectType == "SALESINVOICE")
            {

                HeaderTable = "OINV";
                RowsTable = "INV1";

            }
            else if (ObjectType == "PURCHASEREQUEST")
            {

                HeaderTable = "OPRQ";
                RowsTable = "PRQ1";

            }
            else if (ObjectType == "PURCHASEQOUTATION")
            {

                HeaderTable = "OPQT";
                RowsTable = "PQT1";
            }
            else if (ObjectType == "PURCHASEORDER")
            {

                HeaderTable = "OPOR";
                RowsTable = "POR1";
            }
            else if (ObjectType == "PURCHASECREDITNOTE")
            {


                HeaderTable = "ORPC";
                RowsTable = "RPC1";
            }
            else if (ObjectType == "GRPO")
            {

                HeaderTable = "OPDN";
                RowsTable = "PDN1";
            }
            else if (ObjectType == "GOODSRETURN")
            {


                HeaderTable = "ORPD";
                RowsTable = "RPD1";
            }
            else if (ObjectType == "PURCHASEINVOICE")
            {

                HeaderTable = "OPCH";
                RowsTable = "PCH1";
            }

            List<MarketingDocumentHeader> invoices = new List<MarketingDocumentHeader>();


            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "select T0.\"DocEntry\",T0.\"DocNum\",T0.\"CardCode\", T0.\"CardName\", T0.\"DocDate\", T0.\"DocType\", T0.\"CANCELED\", T0.\"DocStatus\", T0.\"DocTotal\", T0.\"VatSum\" from  \"" + Sanitize(DBName) + "\" + \".OINV\" T0 ";

            }
            else
            {
                querystring = "select ISNULL(T0.DocEntry,'')'DocEntry',ISNULL(T0.DocNum,'') 'DocNum',ISNULL(T0.DocCur,'') 'DocCur',ISNULL(T0.CardCode,'') 'CardCode', ISNULL(T0.CardName,'') 'CardName'," +
                   " ISNULL( T0.DocDate,'') 'DocDate', ISNULL( T0.DocDueDate,'') 'DocDueDate', ISNULL( T0.TaxDate,'') 'TaxDate',ISNULL(T0.DocType,'') 'DocType', ISNULL(T0.CANCELED,'') 'CANCELED', ISNULL(T0.DocStatus ,'') 'DocStatus'," +
                   " ISNULL(T0.DocTotal,0)'DocTotal', ISNULL(T0.VatSum,0)'VatSum',ISNULL(T0.DiscSum,0) 'DiscSum' ,ISNULL( T0.Comments,'') 'Comments' from  " + Sanitize(DBName) + ".[dbo]." + HeaderTable + " t0 (nolock) " +
                   " WHERE   CardCode = '" + Sanitize(CardCode) + "' ORDER BY CardCode asc OFFSET " + Sanitize(Page) + "  ROWS FETCH NEXT " + Sanitize(RequestLimit) + " ROWS ONLY ";

                // querystring = "select  T0.DocEntry,T0.DocNum,T0.CardCode, T0.CardName, T0.DocDate, T0.DocType, T0.CANCELED, T0.DocStatus, ISNULL(T0.DocTotal,0), T0.VatSum ,ISNULL(T0.DiscSum,0) from  "  + Sanitize(DBName) + ".[dbo]." + "OINV t0 (nolock) ";


            }
            DataTable dt = GetData(DBName, querystring);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((ObjectType.EndsWith("INVOICE")))
                {

                    MarketingDocumentHeader document = new MarketingDocumentHeader
                    {


                        DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                ,
                        DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                ,
                        DocCur = Convert.ToString(dt.Rows[i]["DocCur"])
                ,
                        CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                ,
                        CardName = Convert.ToString(dt.Rows[i]["CardName"])
                ,
                        DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                ,
                        MarketingDocument = ObjectType,
                        DocDueDate = Convert.ToString(dt.Rows[i]["DocDueDate"])

                ,
                        TaxDate = Convert.ToString(dt.Rows[i]["TaxDate"])

                ,
                        DocType = Convert.ToString(dt.Rows[i]["DocType"])
                ,
                        CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                ,
                        DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                ,
                        DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                ,
                        VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                ,
                        DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                ,
                        Remarks = Convert.ToString(dt.Rows[i]["Comments"])
                ,
                        MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                        Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))

                    };
                    invoices.Add(document);
                }



                else
                {
                    MarketingDocumentHeader document = new MarketingDocumentHeader
                    {


                        DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                       ,
                        DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                       ,
                        DocCur = Convert.ToString(dt.Rows[i]["DocCur"])
                       ,
                        CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                       ,
                        CardName = Convert.ToString(dt.Rows[i]["CardName"])
                       ,
                        DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                       ,
                        MarketingDocument = ObjectType,
                        DocDueDate = Convert.ToString(dt.Rows[i]["DocDueDate"])

                       ,
                        TaxDate = Convert.ToString(dt.Rows[i]["TaxDate"])

                       ,
                        DocType = Convert.ToString(dt.Rows[i]["DocType"])
                       ,
                        CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                       ,
                        DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                       ,
                        DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                       ,
                        VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                       ,
                        DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                       ,
                        Remarks = Convert.ToString(dt.Rows[i]["Comments"])
                       ,
                        MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                        // Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))

                    };
                    invoices.Add(document);

                }

            }
            querystring = "select count(T0.DocEntry)/  " + RequestLimit + " as NumberofPages FROM  " + Sanitize(DBName) + ".[dbo]." + HeaderTable + " t0(nolock)";
            DataTable records = GetData(DBName, querystring);
            var json = DataTableToJSONWithStringBuilder(records);
            string record_details = "{\"RequestLimit\": \"" + RequestLimit + "\",\"CurrentPage\": \"" + Page + "\"," + json.Replace("[{", "").Replace("]", "");


            string jObject1 = JsonConvert.SerializeObject(invoices, Newtonsoft.Json.Formatting.Indented);
            // string final_data =  jObject1 + record_details;
            var json_final = new[] { "MarketingDocument",JsonConvert.DeserializeObject(jObject1),
                                            "Record Details",JsonConvert.DeserializeObject(record_details) };


            //  var result = new JObject();

            // jObject1.Merge(jObject2);
            //var json1 = JsonConvert.SerializeObject(record_details, Newtonsoft.Json.Formatting.Indented);
            // var json_ = JsonConvert.SerializeObject("{"Success":"test"}", Newtonsoft.Json.Formatting.Indented);

            return Ok(json_final);
        }

        [HttpGet]
        [Route("api/SAP/{DBName}/GetMarketingDocumentsPaginated/{ObjectType}/{Page}/{RequestLimit}")]
        public IActionResult GetMarketingDocumentsPaginated(string DBName, string ObjectType, int Page, int RequestLimit,string dateFrom=null, string dateTo=null)
         {
            if (string.IsNullOrEmpty(dateFrom))
                dateFrom = "1900-01-01";
            if (string.IsNullOrEmpty(dateTo))
                dateTo = DateTimeOffset.Now.ToString("yyyy-MM-dd");
            int offset = 0;
            if (Page < 2)
            {
                offset = 0;
            }
            else
                offset = Page*RequestLimit;
            string HeaderTable = "";
            string RowsTable = "";
            if (ObjectType == "SALESQOUTATION")
            {
                HeaderTable = "OQUT";
                RowsTable = "QUT1";
            }
            else if (ObjectType == "SALESORDER")
            {

                HeaderTable = "ORDR";
                RowsTable = "RDR1";
            }
            else if (ObjectType == "SALESCREDITNOTE")
            {


                HeaderTable = "ORIN";
                RowsTable = "RIN1";
            }
            else if (ObjectType == "SALESINVOICE")
            {

                HeaderTable = "OINV";
                RowsTable = "INV1";

            }
            else if (ObjectType == "PURCHASEREQUEST")
            {

                HeaderTable = "OPRQ";
                RowsTable = "PRQ1";

            }
            else if (ObjectType == "PURCHASEQOUTATION")
            {

                HeaderTable = "OPQT";
                RowsTable = "PQT1";
            }
            else if (ObjectType == "PURCHASEORDER")
            {

                HeaderTable = "OPOR";
                RowsTable = "POR1";
            }
            else if (ObjectType == "PURCHASECREDITNOTE")
            {


                HeaderTable = "ORPC";
                RowsTable = "RPC1";
            }
            else if (ObjectType == "GRPO")
            {

                HeaderTable = "OPDN";
                RowsTable = "PDN1";
            }
            else if (ObjectType == "GOODSRETURN")
            {


                HeaderTable = "ORPD";
                RowsTable = "RPD1";
            }
            else if (ObjectType == "PURCHASEINVOICE")
            {

                HeaderTable = "OPCH";
                RowsTable = "PCH1";
            }

            List<MarketingDocumentHeader> invoices = new List<MarketingDocumentHeader>();


            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "select T0.\"DocEntry\",T0.\"DocNum\",T0.\"CardCode\", T0.\"CardName\", T0.\"DocDate\", T0.\"DocType\", T0.\"CANCELED\", T0.\"DocStatus\", T0.\"DocTotal\", T0.\"VatSum\" from  \"" + Sanitize(DBName) + "\" + \".OINV\" T0 ";

            }
            else
            {
                querystring = "select ISNULL(T0.DocEntry,'')'DocEntry',ISNULL(T0.DocNum,'') 'DocNum',ISNULL(T0.DocCur,'') 'DocCur',ISNULL(T0.CardCode,'') 'CardCode', ISNULL(T0.CardName,'') 'CardName'," +
                   " ISNULL( T0.DocDate,'') 'DocDate', ISNULL( T0.DocDueDate,'') 'DocDueDate', ISNULL( T0.TaxDate,'') 'TaxDate',ISNULL(T0.DocType,'') 'DocType', ISNULL(T0.CANCELED,'') 'CANCELED', ISNULL(T0.DocStatus ,'') 'DocStatus'," +
                   " ISNULL(T0.DocTotal,0)'DocTotal', ISNULL(T0.VatSum,0)'VatSum',ISNULL(T0.DiscSum,0) 'DiscSum' ,ISNULL( T0.Comments,'') 'Comments' from  " + Sanitize(DBName) + ".[dbo]." + HeaderTable + $" t0 (nolock) where T0.DocDate > '{dateFrom}' and T0.DocDate < '{dateTo}' ORDER BY T0.DocDate DESC OFFSET {offset} ROWS FETCH NEXT {RequestLimit} ROWS ONLY;";
                // querystring = "select  T0.DocEntry,T0.DocNum,T0.CardCode, T0.CardName, T0.DocDate, T0.DocType, T0.CANCELED, T0.DocStatus, ISNULL(T0.DocTotal,0), T0.VatSum ,ISNULL(T0.DiscSum,0) from  "  + Sanitize(DBName) + ".[dbo]." + "OINV t0 (nolock) ";


            }
            DataTable dt = GetData(DBName, querystring);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((ObjectType.EndsWith("INVOICE")))
                {

                    MarketingDocumentHeader document = new MarketingDocumentHeader
                    {


                        DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                ,
                        DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                ,
                        DocCur = Convert.ToString(dt.Rows[i]["DocCur"])
                ,
                        CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                ,
                        CardName = Convert.ToString(dt.Rows[i]["CardName"])
                ,
                        DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                ,
                        MarketingDocument = ObjectType,
                        DocDueDate = Convert.ToString(dt.Rows[i]["DocDueDate"])

                ,
                        TaxDate = Convert.ToString(dt.Rows[i]["TaxDate"])

                ,
                        DocType = Convert.ToString(dt.Rows[i]["DocType"])
                ,
                        CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                ,
                        DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                ,
                        DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                ,
                        VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                ,
                        DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                ,
                        Remarks = Convert.ToString(dt.Rows[i]["Comments"])
                ,
                        MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                        Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))

                    };
                    invoices.Add(document);
                }



                else
                {
                    MarketingDocumentHeader document = new MarketingDocumentHeader
                    {


                        DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                       ,
                        DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                       ,
                        DocCur = Convert.ToString(dt.Rows[i]["DocCur"])
                       ,
                        CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                       ,
                        CardName = Convert.ToString(dt.Rows[i]["CardName"])
                       ,
                        DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                       ,
                        MarketingDocument = ObjectType,
                        DocDueDate = Convert.ToString(dt.Rows[i]["DocDueDate"])

                       ,
                        TaxDate = Convert.ToString(dt.Rows[i]["TaxDate"])

                       ,
                        DocType = Convert.ToString(dt.Rows[i]["DocType"])
                       ,
                        CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                       ,
                        DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                       ,
                        DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                       ,
                        VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                       ,
                        DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                       ,
                        Remarks = Convert.ToString(dt.Rows[i]["Comments"])
                       ,
                        MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                        // Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))

                    };
                    invoices.Add(document);

                }

            }

            return Ok(invoices);
        }


        //[Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetMarketingDocuments/{ObjectType}")]
        public IActionResult GetMarketingDocuments(string DBName, string ObjectType)


        {
            string HeaderTable = "";
            string RowsTable = "";
            if (ObjectType == "SALESQOUTATION")
            {
                HeaderTable = "OQUT";
                RowsTable = "QUT1";
            }
            else if (ObjectType == "SALESORDER")
            {

                HeaderTable = "ORDR";
                RowsTable = "RDR1";
            }
            else if (ObjectType == "SALESCREDITNOTE")
            {


                HeaderTable = "ORIN";
                RowsTable = "RIN1";
            }
            else if (ObjectType == "SALESINVOICE")
            {

                HeaderTable = "OINV";
                RowsTable = "INV1";

            }
            else if (ObjectType == "PURCHASEREQUEST")
            {

                HeaderTable = "OPRQ";
                RowsTable = "PRQ1";

            }
            else if (ObjectType == "PURCHASEQOUTATION")
            {

                HeaderTable = "OPQT";
                RowsTable = "PQT1";
            }
            else if (ObjectType == "PURCHASEORDER")
            {

                HeaderTable = "OPOR";
                RowsTable = "POR1";
            }
            else if (ObjectType == "PURCHASECREDITNOTE")
            {


                HeaderTable = "ORPC";
                RowsTable = "RPC1";
            }
            else if (ObjectType == "GRPO")
            {

                HeaderTable = "OPDN";
                RowsTable = "PDN1";
            }
            else if (ObjectType == "GOODSRETURN")
            {


                HeaderTable = "ORPD";
                RowsTable = "RPD1";
            }
            else if (ObjectType == "PURCHASEINVOICE")
            {

                HeaderTable = "OPCH";
                RowsTable = "PCH1";
            }

            List<MarketingDocumentHeader> invoices = new List<MarketingDocumentHeader>();


            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "select T0.\"DocEntry\",T0.\"DocNum\",T0.\"CardCode\", T0.\"CardName\", T0.\"DocDate\", T0.\"DocType\", T0.\"CANCELED\", T0.\"DocStatus\", T0.\"DocTotal\", T0.\"VatSum\" from  \"" + Sanitize(DBName) + "\" + \".OINV\" T0 ";

            }
            else
            {
                querystring = "select ISNULL(T0.DocEntry,'')'DocEntry',ISNULL(T0.DocNum,'') 'DocNum',ISNULL(T0.DocCur,'') 'DocCur',ISNULL(T0.CardCode,'') 'CardCode', ISNULL(T0.CardName,'') 'CardName'," +
                   " ISNULL( T0.DocDate,'') 'DocDate', ISNULL( T0.DocDueDate,'') 'DocDueDate', ISNULL( T0.TaxDate,'') 'TaxDate',ISNULL(T0.DocType,'') 'DocType', ISNULL(T0.CANCELED,'') 'CANCELED', ISNULL(T0.DocStatus ,'') 'DocStatus'," +
                   " ISNULL(T0.DocTotal,0)'DocTotal', ISNULL(T0.VatSum,0)'VatSum',ISNULL(T0.DiscSum,0) 'DiscSum' ,ISNULL( T0.Comments,'') 'Comments' from  " + Sanitize(DBName) + ".[dbo]." + HeaderTable + " t0 (nolock) ORDER BY T0.DocDate DESC OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY;";
                // querystring = "select  T0.DocEntry,T0.DocNum,T0.CardCode, T0.CardName, T0.DocDate, T0.DocType, T0.CANCELED, T0.DocStatus, ISNULL(T0.DocTotal,0), T0.VatSum ,ISNULL(T0.DiscSum,0) from  "  + Sanitize(DBName) + ".[dbo]." + "OINV t0 (nolock) ";


            }
            DataTable dt = GetData(DBName, querystring);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((ObjectType.EndsWith("INVOICE")))
                {

                    MarketingDocumentHeader document = new MarketingDocumentHeader
                    {


                        DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                ,
                        DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                ,
                        DocCur = Convert.ToString(dt.Rows[i]["DocCur"])
                ,
                        CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                ,
                        CardName = Convert.ToString(dt.Rows[i]["CardName"])
                ,
                        DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                ,
                        MarketingDocument = ObjectType,
                        DocDueDate = Convert.ToString(dt.Rows[i]["DocDueDate"])

                ,
                        TaxDate = Convert.ToString(dt.Rows[i]["TaxDate"])

                ,
                        DocType = Convert.ToString(dt.Rows[i]["DocType"])
                ,
                        CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                ,
                        DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                ,
                        DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                ,
                        VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                ,
                        DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                ,
                        Remarks = Convert.ToString(dt.Rows[i]["Comments"])
                ,
                        MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                        Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))

                    };
                    invoices.Add(document);
                }



                else
                {
                    MarketingDocumentHeader document = new MarketingDocumentHeader
                    {


                        DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                       ,
                        DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                       ,
                        DocCur = Convert.ToString(dt.Rows[i]["DocCur"])
                       ,
                        CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                       ,
                        CardName = Convert.ToString(dt.Rows[i]["CardName"])
                       ,
                        DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                       ,
                        MarketingDocument = ObjectType,
                        DocDueDate = Convert.ToString(dt.Rows[i]["DocDueDate"])

                       ,
                        TaxDate = Convert.ToString(dt.Rows[i]["TaxDate"])

                       ,
                        DocType = Convert.ToString(dt.Rows[i]["DocType"])
                       ,
                        CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                       ,
                        DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                       ,
                        DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                       ,
                        VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                       ,
                        DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                       ,
                        Remarks = Convert.ToString(dt.Rows[i]["Comments"])
                       ,
                        MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                        // Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))

                    };
                    invoices.Add(document);

                }

            }

            return Ok(invoices);
        }
        //  [Authorize(Roles = "SuperAdmin, Admin, User")]

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetMarketingDocumentByDate/{ObjectType}/{FromDocDate}/{ToDocDate}")]
        public IActionResult GetMarketingDocumentByDate(string DBName, string ObjectType, string FromDocDate, string ToDocDate)


        {
            string HeaderTable = "";
            string RowsTable = "";
            if (ObjectType == "SALESQOUTATION")
            {
                HeaderTable = "OQUT";
                RowsTable = "QUT1";
            }
            else if (ObjectType == "SALESORDER")
            {

                HeaderTable = "ORDR";
                RowsTable = "RDR1";
            }
            else if (ObjectType == "SALESCREDITNOTE")
            {


                HeaderTable = "ORIN";
                RowsTable = "RIN1";
            }
            else if (ObjectType == "SALESINVOICE")
            {

                HeaderTable = "OINV";
                RowsTable = "INV1";

            }
            else if (ObjectType == "PURCHASEREQUEST")
            {

                HeaderTable = "OPRQ";
                RowsTable = "PRQ1";

            }
            else if (ObjectType == "PURCHASEQOUTATION")
            {

                HeaderTable = "OPQT";
                RowsTable = "PQT1";
            }
            else if (ObjectType == "PURCHASEORDER")
            {

                HeaderTable = "OPOR";
                RowsTable = "POR1";
            }
            else if (ObjectType == "PURCHASECREDITNOTE")
            {


                HeaderTable = "ORPC";
                RowsTable = "RPC1";
            }
            else if (ObjectType == "GRPO")
            {

                HeaderTable = "OPDN";
                RowsTable = "PDN1";
            }
            else if (ObjectType == "GOODSRETURN")
            {


                HeaderTable = "ORPD";
                RowsTable = "RPD1";
            }
            else if (ObjectType == "PURCHASEINVOICE")
            {

                HeaderTable = "OPCH";
                RowsTable = "PCH1";
            }

            List<MarketingDocumentHeader> invoices = new List<MarketingDocumentHeader>();


            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "select T0.\"DocEntry\",T0.\"DocNum\",T0.\"CardCode\", T0.\"CardName\", T0.\"DocDate\", T0.\"DocType\", T0.\"CANCELED\", T0.\"DocStatus\", T0.\"DocTotal\", T0.\"VatSum\" from  \"" + Sanitize(DBName) + "\" + \".OINV\" T0 ";

            }
            else
            {
                querystring = "select ISNULL(T0.DocEntry,'')'DocEntry',ISNULL(T0.DocNum,'') 'DocNum',ISNULL(T0.DocCur,'') 'DocCur',ISNULL(T0.CardCode,'') 'CardCode', ISNULL(T0.CardName,'') 'CardName'," +
                   " ISNULL( T0.DocDate,'') 'DocDate', ISNULL( T0.DocDueDate,'') 'DocDueDate', ISNULL( T0.TaxDate,'') 'TaxDate',ISNULL(T0.DocType,'') 'DocType', ISNULL(T0.CANCELED,'') 'CANCELED', ISNULL(T0.DocStatus ,'') 'DocStatus'," +
                   " ISNULL(T0.DocTotal,0)'DocTotal', ISNULL(T0.VatSum,0)'VatSum',ISNULL(T0.DiscSum,0) 'DiscSum' ,ISNULL( T0.Comments,'') 'Comments' from  " + Sanitize(DBName) + ".[dbo]." + HeaderTable + " t0 (nolock) " +
                   " WHERE  T0.DocDate  BETWEEN '" + Sanitize(FromDocDate) + "' and  '" + Sanitize(ToDocDate) + "'";


            }
            DataTable dt = GetData(DBName, querystring);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                MarketingDocumentHeader document = new MarketingDocumentHeader
                {


                    DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                    ,
                    DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                    ,
                    DocCur = Convert.ToString(dt.Rows[i]["DocCur"])
                    ,
                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                    ,
                    MarketingDocument = ObjectType,
                    DocDueDate = Convert.ToString(dt.Rows[i]["DocDueDate"])

                    ,
                    TaxDate = Convert.ToString(dt.Rows[i]["TaxDate"])

                    ,
                    DocType = Convert.ToString(dt.Rows[i]["DocType"])
                    ,
                    CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                    ,
                    DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                    ,
                    DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                    ,
                    VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                    ,
                    DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                    ,
                    Remarks = Convert.ToString(dt.Rows[i]["Comments"])
                    ,
                    MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                    Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))
                };
                invoices.Add(document);
            }



            return Ok(invoices);
        }







        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetMarketingDocByDocEntry/{ObjectType}/{DocEntry}")]
        public IActionResult GetMarketingDocByDocEntry(string DBName, string ObjectType, string DocEntry)


        {
            string HeaderTable = "";
            string RowsTable = "";
            ObjectType = ObjectType.ToUpper();
            if (ObjectType == "SALESQOUTATION")
            {
                HeaderTable = "OQUT";
                RowsTable = "QUT1";
            }
            else if (ObjectType == "SALESORDER")
            {

                HeaderTable = "ORDR";
                RowsTable = "RDR1";
            }
            else if (ObjectType == "SALESCREDITNOTE")
            {


                HeaderTable = "ORIN";
                RowsTable = "RIN1";
            }
            else if (ObjectType == "SALESINVOICE")
            {

                HeaderTable = "OINV";
                RowsTable = "INV1";

            }
            else if (ObjectType == "PURCHASEREQUEST")
            {

                HeaderTable = "OPRQ";
                RowsTable = "PRQ1";

            }
            else if (ObjectType == "PURCHASEQOUTATION")
            {

                HeaderTable = "OPQT";
                RowsTable = "PQT1";
            }
            else if (ObjectType == "PURCHASEORDER")
            {

                HeaderTable = "OPOR";
                RowsTable = "POR1";
            }
            else if (ObjectType == "PURCHASECREDITNOTE")
            {


                HeaderTable = "ORPC";
                RowsTable = "RPC1";
            }
            else if (ObjectType == "GRPO")
            {

                HeaderTable = "OPDN";
                RowsTable = "PDN1";
            }
            else if (ObjectType == "GOODSRETURN")
            {


                HeaderTable = "ORPD";
                RowsTable = "RPD1";
            }
            else if (ObjectType == "PURCHASEINVOICE")
            {

                HeaderTable = "OPCH";
                RowsTable = "PCH1";
            }

            List<MarketingDocumentHeader> invoices = new List<MarketingDocumentHeader>();


            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "select T0.\"DocEntry\",T0.\"DocNum\",T0.\"CardCode\", T0.\"CardName\", T0.\"DocDate\", T0.\"DocType\", T0.\"CANCELED\", T0.\"DocStatus\", T0.\"DocTotal\", T0.\"VatSum\" from  \"" + Sanitize(DBName) + "\" + \".OINV\" T0 ";

            }
            else
            {
                querystring = "select ISNULL(T0.DocEntry,'')'DocEntry',ISNULL(T0.DocNum,'') 'DocNum',ISNULL(T0.DocCur,'') 'DocCur',ISNULL(T0.CardCode,'') 'CardCode', ISNULL(T0.CardName,'') 'CardName'," +
                   " ISNULL( T0.DocDate,'') 'DocDate', ISNULL( T0.DocDueDate,'') 'DocDueDate', ISNULL( T0.TaxDate,'') 'TaxDate',ISNULL(T0.DocType,'') 'DocType', ISNULL(T0.CANCELED,'') 'CANCELED', ISNULL(T0.DocStatus ,'') 'DocStatus'," +
                   " ISNULL(T0.DocTotal,0)'DocTotal', ISNULL(T0.VatSum,0)'VatSum',ISNULL(T0.DiscSum,0) 'DiscSum' ,ISNULL( T0.Comments,'') 'Comments' from  " + Sanitize(DBName) + ".[dbo]." + HeaderTable + " t0 (nolock) " +
                   " WHERE  T0.DocEntry  = '" + Sanitize(DocEntry) + "'";
            }

            DataTable dt = GetData(DBName, querystring);
            MarketingDocumentHeader invoice = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                invoice = new MarketingDocumentHeader
                {

                    DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                    ,
                    DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                    ,
                    DocCur = Convert.ToString(dt.Rows[i]["DocCur"])
                    ,
                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                    ,
                    MarketingDocument = ObjectType,
                    DocDueDate = Convert.ToString(dt.Rows[i]["DocDueDate"])

                    ,
                    TaxDate = Convert.ToString(dt.Rows[i]["TaxDate"])

                    ,
                    DocType = Convert.ToString(dt.Rows[i]["DocType"])
                    ,
                    CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                    ,
                    DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                    ,
                    DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                    ,
                    VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                    ,
                    DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                    ,
                    Remarks = Convert.ToString(dt.Rows[i]["Comments"])
                    ,
                    MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                    Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))
                };
                break;
            }
            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoices);
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetInvoicesById/{DocEntry}")]
        public IActionResult GetInvoicesById(string DBName, string DocEntry, string Table)

        {

            string HeaderTable = "";
            string RowsTable = "";


            HeaderTable = "OINV";
            RowsTable = "INV1";


            List<MarketingDocumentHeader> invoices = new List<MarketingDocumentHeader>();


            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "select T0.\"DocEntry\",T0.\"DocNum\",T0.\"CardCode\", T0.\"CardName\", T0.\"DocDate\", T0.\"DocType\", T0.\"CANCELED\", T0.\"DocStatus\", T0.\"DocTotal\", T0.\"VatSum\" from  \"" + Sanitize(DBName) + "\" + \".OINV\" T0 ";

            }
            else
            {
                querystring = "select ISNULL(T0.DocEntry,'')'DocEntry',ISNULL(T0.DocNum,'') 'DocNum',ISNULL(T0.CardCode,'') 'CardCode', ISNULL(T0.CardName,'') 'CardName'," +
                   " ISNULL( T0.DocDate,'') 'DocDate', ISNULL( T0.DocDueDate,'') 'DocDueDate', ISNULL( T0.TaxDate,'') 'TaxDate',ISNULL(T0.DocType,'') 'DocType', ISNULL(T0.CANCELED,'') 'CANCELED', ISNULL(T0.DocStatus ,'') 'DocStatus'," +
                   " ISNULL(T0.DocTotal,0)'DocTotal', ISNULL(T0.VatSum,0)'VatSum',ISNULL(T0.DiscSum,0) 'DiscSum' from  " + Sanitize(DBName) + ".[dbo]." + HeaderTable + " t0 (nolock) ";
                // querystring = "select  T0.DocEntry,T0.DocNum,T0.CardCode, T0.CardName, T0.DocDate, T0.DocType, T0.CANCELED, T0.DocStatus, ISNULL(T0.DocTotal,0), T0.VatSum ,ISNULL(T0.DiscSum,0) from  "  + Sanitize(DBName) + ".[dbo]." + "OINV t0 (nolock) ";


            }


            DataTable dt = GetData(DBName, querystring);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                MarketingDocumentHeader invoice = new MarketingDocumentHeader
                {


                    DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                    ,
                    DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                    ,

                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    DocDate = Convert.ToString(dt.Rows[i]["DocDate"])

                    ,
                    DocType = Convert.ToString(dt.Rows[i]["DocType"])
                    ,
                    CANCELED = Convert.ToString(dt.Rows[i]["CANCELED"])
                    ,
                    DocStatus = Convert.ToString(dt.Rows[i]["DocStatus"])
                    ,
                    DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                    ,
                    VatSum = Convert.ToString(dt.Rows[i]["VatSum"])
                    ,
                    DocDiscount = Convert.ToString(dt.Rows[i]["DiscSum"])
                    ,
                    MarketingDocument_Rows = GetMarketingDocumentRows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"])),
                    Payment_Rows = GetPayment_Rows(DBName, HeaderTable, RowsTable, Convert.ToString(dt.Rows[i]["DocEntry"]))
                };
                invoices.Add(invoice);
            }

            return Ok(invoices);
        }
        public List<MarketingDocument_Rows> GetMarketingDocumentRows(string DBName, string HeaderTable, string RowsTable, string DocEntry)
        {



            List<MarketingDocument_Rows> invoicerows = new List<MarketingDocument_Rows>();
            DataTable dt = null;
            bool is_Numeric = IsNumeric(DocEntry);
            if (is_Numeric == true)
            {


                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "select  T1.\"DocEntry\" ,T1.\"ItemCode\", T1.\"ItemCode\", T1.\"Dscription\", T1.\"Quantity\", T1.\"WhsCode\", T1.\"LineTotal\", T1.\"PriceBefDi\",T1.\"Price\", T1.\"VatGroup\", T1.\"VatSum\" from  \"" + Sanitize(DBName) + "\" + \".INV1\" T1  INNER JOIN" +
                              "   \"" + Sanitize(DBName) + "\" + \".OINV\"  T0  ON  T0.\"DocEntry\" =T1.\"DocEntry\"  Where T0.\"DocEntry\" ='" + DocEntry + "'";
                }
                else
                {
                    querystring = "select  ISNULL(T1.DocEntry,'') 'DocEntry' , ISNULL(T1.ItemCode,'') 'ItemCode' , " +
                        " ISNULL(T1.ItemCode,'') 'ItemCode', ISNULL(T1.Dscription ,'') 'Dscription', ISNULL(T1.Quantity ,0) 'Quantity', ISNULL(T1.WhsCode ,'') 'WhsCode', " +
                        " ISNULL(T1.LineTotal,0) 'LineTotal',ISNULL( T1.PriceBefDi,0) 'PriceBefDi',ISNULL(T1.Price,0) 'Price'," +
                        " ISNULL(T1.VatGroup,'') 'VatGroup', ISNULL(T1.VatSum,0) 'VatSum' from " + Sanitize(DBName) + ".[dbo]." + RowsTable + " T1 (nolock) INNER JOIN" +
                              " " + Sanitize(DBName) + ".[dbo]." + HeaderTable + " T0  ON  T0.DocEntry =T1.DocEntry  Where T0.DocEntry ='" + DocEntry + "'";
                }




                dt = GetData(DBName, querystring);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                invoicerows.Add(new MarketingDocument_Rows
                {


                    DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                     ,
                    ItemCode = Convert.ToString(dt.Rows[i]["ItemCode"])
                    ,
                    Dscription = Convert.ToString(dt.Rows[i]["Dscription"])
                    ,
                    Quantity = Convert.ToString(dt.Rows[i]["Quantity"])
                    ,
                    WhsCode = Convert.ToString(dt.Rows[i]["WhsCode"])
                    ,
                    LineTotal = Convert.ToString(dt.Rows[i]["LineTotal"])
                    ,
                    PriceBefDi = Convert.ToString(dt.Rows[i]["PriceBefDi"])
                      ,
                    Price = Convert.ToString(dt.Rows[i]["Price"])
                      ,
                    VatGroup = Convert.ToString(dt.Rows[i]["VatGroup"])
                    ,
                    VatSum = Convert.ToString(dt.Rows[i]["VatSum"])

                });
            }
            return invoicerows;
        }


        public List<Payment_Rows> GetPayment_Rows(string DBName, string HeaderTable, string RowsTable, string DocEntry)
        {



            List<Payment_Rows> paymentrows = new List<Payment_Rows>();
            DataTable dt = null;
            bool is_Numeric = IsNumeric(DocEntry);
            if (is_Numeric == true)
            {


                if (DbServerType == "SAPHANA")
                {
                    querystring = "SELECT isnull(T0.DocEntry, 0) 'DocEntry',isnull(T0.DocNum, '') 'DocNum',isnull(T0.DocTotal, 0) 'DocTotal'," +
                       "isnull(T0.TrsfrAcct, '') 'TrsfrAcct',isnull(T3.AcctName, '') 'AcctName' FROM  "
                       + Sanitize(DBName) + ".[dbo]." + "ORCT T0 LEFT JOIN " + Sanitize(DBName) + ".[dbo]." + "RCT2 T1 ON T1.DocNum = T0.DocNum " +
                       " LEFT JOIN" + Sanitize(DBName) + ".[dbo]." + "OINV T2 ON T2.DocEntry = T1.DocEntry LEFT JOIN"
                       + Sanitize(DBName) + ".[dbo]." + "OACT T3 ON T3.AcctCode = TrsfrAcct " +
                       "  Where T2.DocEntry ='" + DocEntry + "'  AND  T0.Canceled='N'";
                }
                else
                {
                    querystring = "SELECT isnull(T0.DocEntry, 0) 'DocEntry',isnull(T0.DocNum, '') 'DocNum',isnull(T0.DocTotal, 0) 'DocTotal'," +
                        " case  when T0.TrsfrSum > 0 then 'Transfer' when T0.CashSum > 0 then 'Cash' when T0.CheckSum > 0 then 'Cheque' when T0.CreditSum > 0 then 'Credit Card' end 'PaymentMode' FROM  "
                        + Sanitize(DBName) + ".[dbo]." + "ORCT T0 LEFT JOIN " + Sanitize(DBName) + ".[dbo]." + "RCT2 T1 ON T1.DocNum = T0.DocNum " +
                        " LEFT JOIN " + Sanitize(DBName) + ".[dbo]." + "OINV T2 ON T2.DocEntry = T1.DocEntry LEFT JOIN "
                         + Sanitize(DBName) + ".[dbo]." + "OACT T3 ON T3.AcctCode = TrsfrAcct " +
                        "  Where T2.DocEntry ='" + DocEntry + "' AND  T0.Canceled='N'";
                }




                dt = GetData(DBName, querystring);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                paymentrows.Add(new Payment_Rows
                {


                    DocEntry = Convert.ToString(dt.Rows[i]["DocEntry"])
                     ,
                    DocNum = Convert.ToString(dt.Rows[i]["DocNum"])
                    ,
                    DocTotal = Convert.ToString(dt.Rows[i]["DocTotal"])
                    ,
                    PaymentMode = Convert.ToString(dt.Rows[i]["PaymentMode"])


                });
            }
            return paymentrows;
        }



        public class BusinessPartner_Master
        {
            public string CardCode { get; set; }
            public string CardName { get; set; }
            public string CardType { get; set; }
            public double Balance { get; set; }
            public string Currency { get; set; }
            public double CreditLine { get; set; }
            public double DebtLine { get; set; }

            public string GroupCode { get; set; }
            public string Telephone1 { get; set; }
            public string Telephone2 { get; set; }
            public string MobilePhone { get; set; }
            public string Pin { get; set; }
            public string Email { get; set; }
            public string Fax { get; set; }
            public string FrozenFor { get; set; }
            public List<BillToAddress> Bill_To_Address { get; set; }
        }

        public class BillToAddress
        {
            public string CardCode { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public string Position { get; set; }
            public string Address { get; set; }

            public string Tel1 { get; set; }

            public string Cellolar { get; set; }
            public string E_MailL { get; set; }

            public string Active { get; set; }
            // T1.[Name], T1.[Title], T1.[Position], T1.[Address], T1.[Tel1], T1.[Cellolar], T1.[E_MailL], T1.[Active]
        }
        ////  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetBusinessPartners")]
        public IActionResult GetBusinessPartners(string DBName)
        {
            List<BusinessPartner_Master> customers = new List<BusinessPartner_Master>();


            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = querystring = "select  T0.\"CardCode\", T0.\"CardName\", T0.\"Balance\", T0.\"CreditLine\", T0.\"DebtLine\", T0.\"Currency\" from  \"" + Sanitize(DBName) + "\" + \".OCRD\" T0  WHERE  T0.\"frozenFor\"='N'";

            }
            else
            {
                querystring = "select T0.CardCode, T0.CardName,CardType, T0.Balance, T0.Currency ,T0.GroupCode, T0.Phone1, T0.Phone2, T0.Cellular, T0.VatIdUnCmp, T0.E_Mail, T0.Fax ,T0.frozenFor from  " + Sanitize(DBName) + ".[dbo]." + "ocrd t0 (nolock)  WHERE  T0.frozenFor ='N'";
            }

            DataTable dt = GetData(DBName, querystring);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                BusinessPartner_Master customer = new BusinessPartner_Master
                {

                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    CardType = Convert.ToString(dt.Rows[i]["CardType"])
                        ,
                    Balance = Convert.ToDouble(dt.Rows[i]["Balance"])
                    ,
                    Currency = Convert.ToString(dt.Rows[i]["Currency"])
                    ,
                    GroupCode = Convert.ToString(dt.Rows[i]["GroupCode"])
                    ,
                    Telephone1 = Convert.ToString(dt.Rows[i]["Phone1"])
                    ,
                    Telephone2 = Convert.ToString(dt.Rows[i]["Phone2"])
                    ,
                    MobilePhone = Convert.ToString(dt.Rows[i]["Cellular"])
                   ,
                    Pin = Convert.ToString(dt.Rows[i]["VatIdUnCmp"])
                    ,
                    Email = Convert.ToString(dt.Rows[i]["E_Mail"])
                    ,
                    Fax = Convert.ToString(dt.Rows[i]["Fax"])
                    ,
                    FrozenFor = Convert.ToString(dt.Rows[i]["frozenFor"])
                    ,

                    Bill_To_Address = GetContact(DBName, Convert.ToString(dt.Rows[i]["CardCode"]))
                };
                customers.Add(customer);
            }


            //  var json = new JavaScriptSerializer().Serialize(customers);
            return Ok(customers);
        }


        ////  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetBusinessPartnersPaginated/{Page}/{RequestLimit}")]
        public IActionResult GetBusinessPartnersPaginated(string DBName,int Page, int RequestLimit)
        {
            List<BusinessPartner_Master> customers = new List<BusinessPartner_Master>();
            //if (string.IsNullOrEmpty(dateFrom))
            //    dateFrom = "1900-01-01";
            //if (string.IsNullOrEmpty(dateTo))
            //    dateTo = DateTimeOffset.Now.ToString("yyyy-MM-dd");
            int offset = 0;
            if (Page < 2)
            {
                offset = 0;
            }
            else
                offset = Page * RequestLimit;

            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = querystring = "select  T0.\"CardCode\", T0.\"CardName\", T0.\"Balance\", T0.\"CreditLine\", T0.\"DebtLine\", T0.\"Currency\" from  \"" + Sanitize(DBName) + "\" + \".OCRD\" T0  WHERE  T0.\"frozenFor\"='N'";

            }
            else
            {
                querystring = "select T0.CardCode, T0.CardName,CardType, T0.Balance, T0.Currency ,T0.GroupCode, T0.Phone1, T0.Phone2, T0.Cellular, T0.VatIdUnCmp, T0.E_Mail, T0.Fax ,T0.frozenFor from  " + Sanitize(DBName) + ".[dbo]." + "ocrd t0 (nolock)  WHERE  T0.frozenFor ='N'" + $" ORDER BY T0.CardCode desc OFFSET {offset} ROWS FETCH NEXT {RequestLimit} ROWS ONLY;"; ;
            }

            DataTable dt = GetData(DBName, querystring);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                BusinessPartner_Master customer = new BusinessPartner_Master
                {

                    CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                    ,
                    CardName = Convert.ToString(dt.Rows[i]["CardName"])
                    ,
                    CardType = Convert.ToString(dt.Rows[i]["CardType"])
                        ,
                    Balance = Convert.ToDouble(dt.Rows[i]["Balance"])
                    ,
                    Currency = Convert.ToString(dt.Rows[i]["Currency"])
                    ,
                    GroupCode = Convert.ToString(dt.Rows[i]["GroupCode"])
                    ,
                    Telephone1 = Convert.ToString(dt.Rows[i]["Phone1"])
                    ,
                    Telephone2 = Convert.ToString(dt.Rows[i]["Phone2"])
                    ,
                    MobilePhone = Convert.ToString(dt.Rows[i]["Cellular"])
                   ,
                    Pin = Convert.ToString(dt.Rows[i]["VatIdUnCmp"])
                    ,
                    Email = Convert.ToString(dt.Rows[i]["E_Mail"])
                    ,
                    Fax = Convert.ToString(dt.Rows[i]["Fax"])
                    ,
                    FrozenFor = Convert.ToString(dt.Rows[i]["frozenFor"])
                    ,

                    Bill_To_Address = GetContact(DBName, Convert.ToString(dt.Rows[i]["CardCode"]))
                };
                customers.Add(customer);
            }


            //  var json = new JavaScriptSerializer().Serialize(customers);
            return Ok(customers);
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/DeleteBusinessPartner/{SAPUserName}/{SAPPassword}")]
        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult>  DeleteBusinessPartner(HttpRequestMessage request, string DBName, string SAPUserName, string SAPPassword)
        {
            //try
            //{
            var jsonString = await request.Content.ReadAsStringAsync();
            string bsl = @"\";
            Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
            var response = "";
            dynamic message_ = null;
            var message = "";
            string CardCode, CardName, CardType, Action;
            //Header Section 

            CardCode = (string)json.SelectToken("BPInformation").SelectToken("CardCode");
            CardName = (string)json.SelectToken("BPInformation").SelectToken("CardName");
            CardType = (string)json.SelectToken("BPInformation").SelectToken("CardType").ToString().ToUpper();
            Action = (string)json.SelectToken("BPInformation").SelectToken("Action");

            Connect_To_SAP connect = new Connect_To_SAP();
            oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
            SAPbobsCOM.BusinessPartners sboBP = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
            bool check_card_code = CheckIfExists(DBName, CardCode, CardType);
            if (check_card_code == true && Action == "DELETE")
            {
                sboBP.GetByKey(CardCode);



                if (sboBP.Remove() != 0)
                {
                    string dqt = @"""";
                    oCompany.GetLastError(out nErr, out erMsg);

                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": " + dqt + erMsg.Replace("-", "") + dqt + ", \"Customer Number\": " + dqt + CardCode + dqt + ",\"Document Type\": \"Customer\"}}";
                    message_ = JsonConvert.DeserializeObject(message);

                    MarshallObject(sboBP);
                    MarshallObject(oCompany);
                }
                else
                {


                    message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"Successfully Removed Business Partner\",\"Business Partner Number\": \"" + CardCode + "\",\"Document Type\": \"Business Partner\"}}";
                    message_ = JsonConvert.DeserializeObject(message);

                    SAP_SEND_MESSAGE("Business Partner Removed", "Business Partner Removed from API", "Business Partner Name", CardCode, "2", CardCode, "", "", "", "");

                    MarshallObject(sboBP);
                    MarshallObject(oCompany);


                }

            }
            else
            {

                message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"Customer Does not Exists \",\"Business Partner Number\": \"" + CardCode + "\" ,\"Document Type\": \"Business Partner\"}}";
                message_ = JsonConvert.DeserializeObject(message);
            }
            return Ok(message_);


            //}
            //catch (Exception ex)
            //{

            //    HttpResponseMessage exeption_response = null;
            //    exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
            //    return Problem(ex.Message);
            //}




        }
        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/CreateBusinessPartner/")]
        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult> CreateBusinessPartner([FromBody]CreateBusinessPartnerRequest request, string DBName)
        {
            try
            {
                //var jsonString = await request.Content.ReadAsStringAsync();
                string bsl = @"\";
                //JObject json = JObject.Parse(jsonString);
                var response = "";
                dynamic message_ = null;
                var message = "";
                //string CardCode, CardName, CardType, Currency, GroupCode, Action, Telephone1, Telephone2, MobilePhone, PayTermsGrpCode, KRAPIN, Email, Fax, Active;
                ////Header Section 

                //CardCode = request.BPInformation.CardCode (string)json.SelectToken("BPInformation").SelectToken("CardCode");
                //CardName = (string)json.SelectToken("BPInformation").SelectToken("CardName");
                //CardType = (string)json.SelectToken("BPInformation").SelectToken("CardType").ToString().ToUpper();
                //Currency = (string)json.SelectToken("BPInformation").SelectToken("Currency");
                //GroupCode = (string)json.SelectToken("BPInformation").SelectToken("GroupCode");
                //Telephone1 = (string)json.SelectToken("BPInformation").SelectToken("Telephone1");
                //Telephone2 = (string)json.SelectToken("BPInformation").SelectToken("Telephone2");
                //Action = (string)json.SelectToken("BPInformation").SelectToken("Action");
                //MobilePhone = (string)json.SelectToken("BPInformation").SelectToken("MobilePhone");
                //PayTermsGrpCode = (string)json.SelectToken("BPInformation").SelectToken("PayTermsGrpCode");
                //KRAPIN = (string)json.SelectToken("BPInformation").SelectToken("KRAPIN");
                //Email = (string)json.SelectToken("BPInformation").SelectToken("Email");
                //Fax = (string)json.SelectToken("BPInformation").SelectToken("Fax");
                //Active = (string)json.SelectToken("BPInformation").SelectToken("Active");

                ////End of Header UDF  Declaration Section 
                ////Header Section 


                //string S_AddressName1, S_AddressName2, S_POBox, S_Code, S_City;
                //string B_AddressName1, B_AddressName2, B_POBox, B_Code, B_City;

                //B_AddressName1 = (string)json.SelectToken("BilltoAdress").SelectToken("AddressName1");
                //B_AddressName2 = (string)json.SelectToken("BilltoAdress").SelectToken("AddressName2");
                //B_POBox = (string)json.SelectToken("BilltoAdress").SelectToken("POBox");
                //B_Code = (string)json.SelectToken("BilltoAdress").SelectToken("Code");
                //B_City = (string)json.SelectToken("BilltoAdress").SelectToken("City");

                //S_AddressName1 = (string)json.SelectToken("ShiptoAdress").SelectToken("AddressName1");
                //S_AddressName2 = (string)json.SelectToken("ShiptoAdress").SelectToken("AddressName2");
                //S_POBox = (string)json.SelectToken("ShiptoAdress").SelectToken("POBox");
                //S_Code = (string)json.SelectToken("ShiptoAdress").SelectToken("Code");
                //S_City = (string)json.SelectToken("ShiptoAdress").SelectToken("City");

                //string BrokerCode, BrokerName, BrokerGroupCode;
                //BrokerCode = (string)json.SelectToken("Accounting").SelectToken("BrokerCode");
                //BrokerName = (stri;ng)json.SelectToken("Accounting").SelectToken("BrokerName");
                //BrokerGroupCode = (string)json.SelectToken("Accounting").SelectToken("BrokerGroupCode");
                string SAPUserName = userName;
                string SAPPassword =  Password;

                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                //oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                SAPbobsCOM.BusinessPartners sboBP = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                bool check_card_code = CheckIfExists(DBName, request.BPInformation.CardCode, request.BPInformation.CardType);
                if (check_card_code == false && request.BPInformation.Action == "ADD")
                {

                    sboBP.CardCode = request.BPInformation.CardCode;
                    sboBP.CardName = request.BPInformation.CardName;
                    request.BPInformation.CardType = request.BPInformation.CardType.ToUpper();
                    if (request.BPInformation.CardType == "CUSTOMER")
                    {
                        sboBP.CardType = BoCardTypes.cCustomer;
                    }
                    else if (request.BPInformation.CardType == "SUPPLIER")
                    {
                        sboBP.CardType = BoCardTypes.cSupplier;
                        Insert_WithholdingTax(oCompany.CompanyDB, request.BPInformation.CardCode);
                        sboBP.SubjectToWithholdingTax = SAPbobsCOM.BoYesNoNoneEnum.boYES;


                    }
                    else if (request.BPInformation.CardType == "LEAD")
                    {
                        sboBP.CardType = BoCardTypes.cLid;
                    }
                    if (!string.IsNullOrEmpty(request.BPInformation.GroupCode))
                    {
                        sboBP.GroupCode = Convert.ToInt32(request.BPInformation.GroupCode);
                    }




                    if (string.IsNullOrEmpty(request.BPInformation.Currency))
                    {
                        request.BPInformation.Currency = LocalCurrency(DBName, SAPUserName, SAPPassword);
                        sboBP.Currency = request.BPInformation.Currency;
                    }
                    else
                    {
                        sboBP.Currency = request.BPInformation.Currency;
                    }
                    sboBP.Phone1 = request.BPInformation.Telephone1;
                    sboBP.Phone2 = request.BPInformation.Telephone2;
                    sboBP.Cellular = request.BPInformation.MobilePhone;
                    sboBP.UnifiedFederalTaxID = request.BPInformation.KRAPIN;
                    // sboBP.FederalTaxID = KRAPIN;
                    sboBP.EmailAddress = request.BPInformation.Email;
                    sboBP.Fax = request.BPInformation.Fax;

                    if (request.BPInformation.Active == "Y")
                    {


                        sboBP.Frozen = SAPbobsCOM.BoYesNoEnum.tNO;
                        sboBP.FrozenFrom = DateTime.Today;
                        sboBP.Valid = SAPbobsCOM.BoYesNoEnum.tYES;
                    }
                    else

                    {
                        sboBP.Frozen = SAPbobsCOM.BoYesNoEnum.tYES;
                        sboBP.FrozenFrom = DateTime.Today;
                        sboBP.Valid = SAPbobsCOM.BoYesNoEnum.tNO;
                        // sboBP.Valid = SAPbobsCOM.BoYesNoEnum.tNO;
                    }

                    sboBP.Addresses.TypeOfAddress = Convert.ToString(BoAddressType.bo_BillTo);
                    //sboBP.Addresses.TypeOfAddress = Convert.ToString(BoAddressType.bo_ShipTo);
                    sboBP.Addresses.AddressName = "Address1";
                    sboBP.Addresses.AddressName2 = request.BilltoAdress.AddressName2;// S_AddressName2;
                    sboBP.Addresses.Street = request.BilltoAdress.POBox;
                    sboBP.Addresses.ZipCode = request.BilltoAdress.POBox;
                    sboBP.Addresses.City = request.BilltoAdress.City;
                    sboBP.Addresses.Add();


                    //sboBP.Addresses.AddressName = "Address1";
                    //sboBP.Addresses.AddressName2 = S_AddressName2;
                    //sboBP.Addresses.Street = S_POBox;
                    //sboBP.Addresses.ZipCode = S_POBox;
                    //sboBP.Addresses.City = S_City;
                    //sboBP.Addresses.Add();




                    // 
                    if (!string.IsNullOrEmpty(request.BPInformation.PayTermsGrpCode))
                    {

                        sboBP.PayTermsGrpCode = Convert.ToInt32(request.BPInformation.PayTermsGrpCode); ;
                    }
                    //if (string.IsNullOrEmpty(BrokerCode))
                    //{
                    //    sboBP.WTCode = "WT05";
                    //}
                    //else
                    //{
                    //    sboBP.WTCode = "WT15";
                    //    sboBP.FatherCard = BrokerCode;
                    //    sboBP.FatherType = BoFatherCardTypes.cPayments_sum;
                    //}


                    if (sboBP.Add() != 0)
                    {
                        string dqt = @"""";
                        oCompany.GetLastError(out nErr, out erMsg);
                        message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": " + erMsg + ", \"Business Partner Number\": " + dqt + request.BPInformation.CardCode + dqt + ",\"Document Type\": \"Business Partner\"}}";
                        message_ = JsonConvert.DeserializeObject(message);

                        MarshallObject(sboBP);
                        MarshallObject(oCompany);
                    }
                    else
                    {

                        //message = "{" + bsl + "Message" + bsl + ": {" + bsl + "MessageType" + bsl + ": " + bsl + "Success" + bsl + "," + bsl + "Description" + bsl + ": " + bsl + "Successfully Created  Customer " + bsl + "," + bsl + "Customer Number" + bsl + ": " + bsl + CardCode.ToString() + bsl + " ," + bsl + "Document Type" + bsl + ": " + bsl + "Customer" + bsl + "}}";
                        // message = message.Replace(bsl, dqt);
                        // var json_response = JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented);

                        message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"Successfully Created Business Partner\",\"Business Partner Number\": \"" + request.BPInformation.CardCode + "\",\"Document Type\": \"Business Partner\"}}";
                        message_ = JsonConvert.DeserializeObject(message);

                        SAP_SEND_MESSAGE("Customer Added", "Business Partner Added from API", "Business Partner Name", request.BPInformation.CardCode, "2", request.BPInformation.CardCode, "", "", "", "");

                        MarshallObject(sboBP);
                        MarshallObject(oCompany);


                    }

                }
                else if (check_card_code == true && request.BPInformation.Action == "UPDATE")
                {
                    sboBP.GetByKey(request.BPInformation.CardCode);
                    // sboBP.CardCode = CardCode;
                    sboBP.CardName = request.BPInformation.CardName;
                    request.BPInformation.CardType = request.BPInformation.CardType.ToUpper();
                    if (request.BPInformation.CardType == "CUSTOMER")
                    {
                        sboBP.CardType = BoCardTypes.cCustomer;
                    }
                    else if (request.BPInformation.CardType == "SUPPLIER")
                    {
                        sboBP.CardType = BoCardTypes.cSupplier;
                        Insert_WithholdingTax(oCompany.CompanyDB, request.BPInformation.CardCode);


                    }
                    else if (request.BPInformation.CardType == "LEAD")
                    {
                        sboBP.CardType = BoCardTypes.cLid;
                    }
                    if (!string.IsNullOrEmpty(request.BPInformation.GroupCode))
                    {
                        sboBP.GroupCode = Convert.ToInt32(request.BPInformation.GroupCode);
                    }




                    if (string.IsNullOrEmpty(request.BPInformation.Currency))
                    {
                        request.BPInformation.Currency = LocalCurrency(DBName, SAPUserName, SAPPassword);
                        sboBP.Currency = request.BPInformation.Currency;
                    }
                    else
                    {
                        sboBP.Currency = request.BPInformation.Currency;
                    }

                    if (request.BPInformation.Active == "Y")
                    {


                        sboBP.Frozen = SAPbobsCOM.BoYesNoEnum.tNO;
                        sboBP.FrozenFrom = DateTime.Today;
                        sboBP.Valid = SAPbobsCOM.BoYesNoEnum.tYES;
                    }
                    else

                    {
                        sboBP.Frozen = SAPbobsCOM.BoYesNoEnum.tYES;
                        sboBP.FrozenFrom = DateTime.Today;
                        sboBP.Valid = SAPbobsCOM.BoYesNoEnum.tNO;
                        // sboBP.Valid = SAPbobsCOM.BoYesNoEnum.tNO;
                    }


                    sboBP.Phone1 = request.BPInformation.Telephone1;
                    sboBP.Phone2 = request.BPInformation.Telephone2;
                    sboBP.Cellular = request.BPInformation.MobilePhone;
                    sboBP.UnifiedFederalTaxID = request.BPInformation.KRAPIN;
                    // sboBP.FederalTaxID = KRAPIN;
                    sboBP.EmailAddress = request.BPInformation.Email;
                    sboBP.Fax = request.BPInformation.Fax;

                    sboBP.Addresses.TypeOfAddress = Convert.ToString(BoAddressType.bo_BillTo);
                    //sboBP.Addresses.TypeOfAddress = Convert.ToString(BoAddressType.bo_ShipTo);
                    sboBP.Addresses.AddressName = request.BilltoAdress.AddressName1;
                    sboBP.Addresses.AddressName2 = request.BilltoAdress.AddressName2;
                    sboBP.Addresses.Street = request.BilltoAdress.POBox;
                    sboBP.Addresses.ZipCode = request.BilltoAdress.POBox; 
                    sboBP.Addresses.City = request.BilltoAdress.City;
                    sboBP.Addresses.Add();


                    //sboBP.Addresses.AddressName = "Address1";
                    //sboBP.Addresses.AddressName2 = S_AddressName2;
                    //sboBP.Addresses.Street = S_POBox;
                    //sboBP.Addresses.ZipCode = S_POBox;
                    //sboBP.Addresses.City = S_City;
                    //sboBP.Addresses.Add();



                    //sboBP.SubjectToWithholdingTax = BoYesNoEnum.tYES;
                    // 
                    if (!string.IsNullOrEmpty(request.BPInformation.PayTermsGrpCode))
                    {

                        sboBP.PayTermsGrpCode = Convert.ToInt32(request.BPInformation.PayTermsGrpCode); ;
                    }
                    //if (string.IsNullOrEmpty(BrokerCode))
                    //{
                    //    sboBP.WTCode = "WT05";
                    //}
                    //else
                    //{
                    //    sboBP.WTCode = "WT15";
                    //    sboBP.FatherCard = BrokerCode;
                    //    sboBP.FatherType = BoFatherCardTypes.cPayments_sum;
                    //}


                    if (sboBP.Update() != 0)
                    {
                        string dqt = @"""";
                        oCompany.GetLastError(out nErr, out erMsg);
                        message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": " + erMsg + ", \"Business Partner Number\": " + dqt + request.BPInformation.CardCode + dqt + ",\"Document Type\": \"Business Partner\"}}";
                        message_ = JsonConvert.DeserializeObject(message);

                        MarshallObject(sboBP);
                        MarshallObject(oCompany);
                    }
                    else
                    {

                        //message = "{" + bsl + "Message" + bsl + ": {" + bsl + "MessageType" + bsl + ": " + bsl + "Success" + bsl + "," + bsl + "Description" + bsl + ": " + bsl + "Successfully Created  Customer " + bsl + "," + bsl + "Customer Number" + bsl + ": " + bsl + CardCode.ToString() + bsl + " ," + bsl + "Document Type" + bsl + ": " + bsl + "Customer" + bsl + "}}";
                        // message = message.Replace(bsl, dqt);
                        // var json_response = JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented);

                        message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"Successfully Updated Business Partner\",\"Business Partner Number\": \"" + request.BPInformation.CardCode + "\",\"Document Type\": \"Business Partner\"}}";
                        message_ = JsonConvert.DeserializeObject(message);

                        SAP_SEND_MESSAGE("Customer Updated", "Business Partner Updated from API", "Business Partner Name", request.BPInformation.CardCode, "2", request.BPInformation.CardCode, "", "", "", "");

                        MarshallObject(sboBP);
                        MarshallObject(oCompany);

                    }
                }
                else
                {

                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"Business Partner  Already nt Exists \",\"Business Partner Number\": \"" + request.BPInformation.CardCode + "\" ,\"Document Type\": \"Business Partner\"}}";
                    message_ = JsonConvert.DeserializeObject(message);
                }
                return Ok(message_);


            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }




        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/ReverseDocument/{SAPUserName}/{SAPPassword}")]
        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult>  ReverseDocument(HttpRequestMessage request, string DBName, string SAPUserName, string SAPPassword)
        {


            string ObjectType = "";
            string DocEntry = "";
            string DocType = "";
            string CardCode = "";
            dynamic message_ = null;
            var jsonString = await request.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(jsonString);

            AddUpdateAppSettings("CompanyDB", DBName);
            AddUpdateAppSettings("manager", SAPUserName);
            AddUpdateAppSettings("Password", SAPPassword);


            ObjectType = (string)json.SelectToken("Header").SelectToken("ObjectType");
            DocEntry = (string)json.SelectToken("Header").SelectToken("DocEntry");
            DocType = (string)json.SelectToken("Header").SelectToken("DocType");
            CardCode = (string)json.SelectToken("Header").SelectToken("CardCode");
            oCompany = new Connect_To_SAP().ConnectSAPDB(DBName, SAPUserName, SAPPassword);



            SAPbobsCOM.Documents oDoc = null;
            string Table = "";

            string ObjectTypeNum = "";
            ObjectType = ObjectType.ToUpper();
            if (ObjectType == "SALESINVOICE")
            {

                oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oCreditNotes);
                oDoc.GetByKey(Convert.ToInt32(DocEntry));

                Table = "OINV";
                ObjectTypeNum = "14";
            }

            else if (ObjectType == "PURCHASEINVOICE")
            {

                oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes);
                oDoc.GetByKey(Convert.ToInt32(DocEntry));


                Table = "ORPC";
                ObjectTypeNum = "19";
            }

            JArray jarr = (JArray)json["Rows"];
            if (DocType == "S")
            {
                foreach (var item in jarr)
                {
                    string Description, AcctCode, VatGroup, UnitPrice, LineTotal;

                    LineTotal = item.SelectToken("LineTotal").ToString();
                    Description = item.SelectToken("Description").ToString();
                    AcctCode = item.SelectToken("AcctCode").ToString();
                    UnitPrice = item.SelectToken("UnitPrice").ToString();
                    VatGroup = item.SelectToken("VatGroup").ToString();


                    //

                    if (ObjectType == "SALESINVOICE")
                    {
                        oDoc.Lines.BaseEntry = Convert.ToInt32(DocEntry);
                        oDoc.Lines.BaseLine = (Int32)item[0];
                        // oDoc.Lines.BaseType = SAPbobsCOM.BoObjectTypes.oCreditNotes;
                        oDoc.Lines.BaseType = (Int32)SAPbobsCOM.BoObjectTypes.oInvoices;
                    }
                    else if (ObjectType == "PURCHASEINVOICE")
                    {
                        oDoc.Lines.BaseEntry = Convert.ToInt32(DocEntry);
                        oDoc.Lines.BaseLine = (Int32)item[0];
                        oDoc.Lines.BaseType = (Int32)SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    }

                    oDoc.Lines.ItemDescription = Description;
                    oDoc.Lines.AccountCode = AcctCode;
                    oDoc.Lines.VatGroup = VatGroup;
                    oDoc.Lines.UnitPrice = Convert.ToDouble(UnitPrice);
                    oDoc.Lines.LineTotal = Convert.ToDouble(LineTotal);
                    oDoc.Lines.PriceAfterVAT = Convert.ToDouble(LineTotal);
                    oDoc.Lines.Add();

                }
            }
            else
            {

                foreach (var item in jarr)
                {
                    string ItemCode, Description, Quantity, UnitPrice, VatGroup, LineTotal, WarehouseCode;

                    ItemCode = item.SelectToken("ItemCode").ToString();
                    Description = item.SelectToken("Description").ToString();
                    Quantity = item.SelectToken("Quantity").ToString();
                    UnitPrice = item.SelectToken("UnitPrice").ToString();
                    VatGroup = item.SelectToken("VatGroup").ToString();
                    LineTotal = item.SelectToken("LineTotal").ToString();
                    WarehouseCode = item.SelectToken("WarehouseCode").ToString();


                    if (ObjectType == "SALESINVOICE")
                    {
                        oDoc.Lines.BaseEntry = Convert.ToInt32(DocEntry);
                        //oDoc.Lines.BaseLine = (Int32)item[0];
                        // oDoc.Lines.BaseType = SAPbobsCOM.BoObjectTypes.oCreditNotes;
                        oDoc.Lines.BaseType = (Int32)SAPbobsCOM.BoObjectTypes.oInvoices;
                    }
                    else if (ObjectType == "PURCHASEINVOICE")
                    {
                        oDoc.Lines.BaseEntry = Convert.ToInt32(DocEntry);
                        oDoc.Lines.BaseLine = (Int32)item[0];
                        oDoc.Lines.BaseType = (Int32)SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    }

                    oDoc.Lines.ItemCode = ItemCode;
                    oDoc.Lines.Quantity = Convert.ToDouble(Quantity);
                    oDoc.Lines.VatGroup = VatGroup;
                    oDoc.Lines.UnitPrice = Convert.ToDouble(UnitPrice);
                    //oDoc.Lines.LineTotal = Convert.ToDouble(LineTotal);
                    //oDoc.Lines.PriceAfterVAT = Convert.ToDouble(LineTotal);
                    oDoc.Lines.WarehouseCode = WarehouseCode;

                    oDoc.Lines.Add();

                }






            }
            if (oDoc.Add() != 0)
            {

                oCompany.GetLastError(out nErr, out erMsg);

                erMsg = Sanitize_Errors(erMsg);
                //.Replace(dbqt, "");
                message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Destination Number\": \"0\",\"Document Type\": \"Invoice\"}}";
                message_ = JsonConvert.DeserializeObject(message);
                MarshallObject(oDoc);
                MarshallObject(oCompany);
            }
            else
            {
                //int snum = Int32.Parse(oCompany.GetNewObjectKey());
                // oDoc.GetByKey(snum);
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(oDoc.GetAsXML());
                //string jsonText = JsonConvert.SerializeXmlNode(doc);
                //message = jsonText;

                string add_data = Get_DocData(DBName, Table, CardCode);
                if (add_data.Length > 1)
                {

                    message = "{\"Message\": {\"MessageType\": \"Success\",\"ObjectType\": \"" + ObjectType + "\",\"Description\": \"Successfully Created\"" + add_data;

                    JObject json_data = JObject.Parse(message);
                    string Doc_Entry = json_data.SelectToken("Message").SelectToken("DocEntry").ToString();
                    message_ = JsonConvert.DeserializeObject(message);
                    SAP_SEND_MESSAGE("Document  Added", "Document Added from API", "Business Partner Name", CardCode, "2", CardCode, "Document Number", Doc_Entry, ObjectTypeNum, DocEntry);
                }
                else
                {
                    string SourceNumber = "0";
                    erMsg = "Customer Code  or ItemCode was not found in this  Company " + DBName;
                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Source Number\": \"" + SourceNumber + "\",\"Destination Number\": \"0\",\"Document Type\": \"Invoice\"}}";
                    message_ = JsonConvert.DeserializeObject(message);
                    MarshallObject(oDoc);
                    MarshallObject(oCompany);
                }
            }

            return Ok(message_);
        }





        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/CancelDocument/{SAPUserName}/{SAPPassword}")]
        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult> CancelDocument(HttpRequestMessage request, string DBName, string SAPUserName, string SAPPassword)
        {
            try
            {
                string ObjectType = "";
                string DocEntry = "";
                string DocType = "";
                string CardCode = "";
                dynamic message_ = null;
                var jsonString = await request.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(jsonString);

                AddUpdateAppSettings("CompanyDB", DBName);
                AddUpdateAppSettings("manager", SAPUserName);
                AddUpdateAppSettings("Password", SAPPassword);


                ObjectType = (string)json.SelectToken("Header").SelectToken("ObjectType");
                DocEntry = (string)json.SelectToken("Header").SelectToken("DocEntry");
                DocType = (string)json.SelectToken("Header").SelectToken("DocType");
                CardCode = (string)json.SelectToken("Header").SelectToken("CardCode");
                oCompany = new Connect_To_SAP().ConnectSAPDB(DBName, SAPUserName, SAPPassword);



                SAPbobsCOM.Documents oDoc = null;
                SAPbobsCOM.Documents oDocCancellation = null;
                SAPbobsCOM.Documents FinalDoc = null;

                //Dim document As SAPbobsCOM.Documents = DirectCast(vCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices), Documents)
                //Dim InvoiceDocNum As Integer = CInt(ListBox1.Items(i).ToString())
                //document.GetByKey(InvoiceDocNum)
                //Dim cancel_doc = document.CreateCancellationDocument()
                //''cancel_doc.UserFields.Fields.Item("U_XMLNAME").Value = "cancellation"
                //retcode = cancel_doc.Add()
                string Table = "";

                string ObjectTypeNum = "";


                if (ObjectType == "SALESQOUTATION")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oQuotations);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "OQUT";
                    ObjectTypeNum = "23";
                }
                else if (ObjectType == "SALESORDER")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "ORDR";
                    ObjectTypeNum = "17";
                }
                else if (ObjectType == "SALESRETURN")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oReturns);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "ORDN";
                    ObjectTypeNum = "16";
                }
                else if (ObjectType == "SALESCREDITNOTE")
                {

                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oCreditNotes);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "ORIN";
                    ObjectTypeNum = "14";
                }
                else if (ObjectType == "SALESINVOICE")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "OINV";
                    ObjectTypeNum = "13";

                }
                else if (ObjectType == "PURCHASEREQUEST")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseRequest);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    ObjectTypeNum = "14700001";
                    Table = "OPRQ";
                    ObjectTypeNum = "13";
                }
                else if (ObjectType == "PURCHASEQOUTATION")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseQuotations);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "OPQT";
                    ObjectTypeNum = "540000006";

                }
                else if (ObjectType == "PURCHASEORDER")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "OPOR";
                    ObjectTypeNum = "22";
                }
                else if (ObjectType == "PURCHASECREDITNOTE")
                {

                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "ORPC";
                    ObjectTypeNum = "19";
                }
                else if (ObjectType == "GRPO")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "OPDN";
                    ObjectTypeNum = "20";
                }
                else if (ObjectType == "GOODSRETURN")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseReturns);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "ORPD";
                    ObjectTypeNum = "21";
                }
                else if (ObjectType == "PURCHASEINVOICE")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                    oDoc.GetByKey(Convert.ToInt32(DocEntry));
                    Table = "OPCH";
                    ObjectTypeNum = "18";
                }
                int retcode = 0;
                if (ObjectType.EndsWith("INVOICE") || ObjectType.EndsWith("RETURN") || ObjectType.EndsWith("PO"))
                {
                    oDocCancellation = oDoc.CreateCancellationDocument();
                    retcode = oDocCancellation.Add();
                }

                else
                {
                    retcode = oDoc.Cancel();
                }


                if (retcode != 0)
                {

                    oCompany.GetLastError(out nErr, out erMsg);

                    erMsg = Sanitize_Errors(erMsg);
                    //.Replace(dbqt, "");
                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Destination Number\": \"0\",\"Document Type\": \"" + ObjectType + "\"}}";
                    message_ = JsonConvert.DeserializeObject(message);
                    MarshallObject(oDoc);
                    MarshallObject(oCompany);
                }
                else
                {
                    //int snum = Int32.Parse(oCompany.GetNewObjectKey());
                    // oDoc.GetByKey(snum);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(oDoc.GetAsXML());
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);
                    //message = jsonText;

                    string add_data = Get_DocData(DBName, Table, CardCode);
                    if (add_data.Length > 1)
                    {

                        message = "{\"Message\": {\"MessageType\": \"Success\",\"ObjectType\": \"" + ObjectType + "\",\"Description\": \"Successfully Cancelled\"" + add_data;

                        JObject json_data = JObject.Parse(message);
                        string Doc_Entry = json_data.SelectToken("Message").SelectToken("DocEntry").ToString();
                        message_ = JsonConvert.DeserializeObject(message);
                        SAP_SEND_MESSAGE("Document  Cancellation", "Document Cancelled from API", "Business Partner Name", CardCode, "2", CardCode, "Document Number", Doc_Entry, ObjectTypeNum, DocEntry);
                    }
                    else
                    {
                        string SourceNumber = "0";
                        erMsg = "Customer Code  or ItemCode was not found in this  Company " + DBName;
                        message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Source Number\": \"" + SourceNumber + "\",\"Destination Number\": \"0\",\"Document Type\": \"" + ObjectType + "\"}}";
                        message_ = JsonConvert.DeserializeObject(message);
                        MarshallObject(oDoc);
                        MarshallObject(oCompany);
                    }
                }

                return Ok(message_);
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }
        }



        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/SetExchangeRate/{SAPUserName}/{SAPPassword}")]
        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult>  SetExchangeRate(HttpRequestMessage request, string DBName, string SAPUserName, string SAPPassword)
        {
            try
            {

                var jsonString = await request.Content.ReadAsStringAsync();
                //context.Response.ContentType = "text/JSON";
                string output = "";
                string message = "";
                dynamic message_ = null;

                JObject json = JObject.Parse(jsonString);

                // string SourceNumber = "";
                // string PaymentReference;

                string Date = "";
                string Currency = "";
                string Rate = "";

                AddUpdateAppSettings("CompanyDB", DBName);
                AddUpdateAppSettings("manager", SAPUserName);
                AddUpdateAppSettings("Password", SAPPassword);


                Date = (string)json.SelectToken("Header").SelectToken("Date");
                Currency = (string)json.SelectToken("Header").SelectToken("Currency");
                Rate = (string)json.SelectToken("Header").SelectToken("Rate");

                oCompany = new Connect_To_SAP().ConnectSAPDB(DBName, SAPUserName, SAPPassword);



                SAPbobsCOM.SBObob oSBObob = (SAPbobsCOM.SBObob)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);

                oSBObob.SetCurrencyRate(Currency, Convert.ToDateTime(Date), Convert.ToDouble(Rate), true);



                message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"Successfully Updated " + Currency + " Exchange Rate to " + Rate + " For Date " + Date + "\"}}";


                message_ = JsonConvert.DeserializeObject(message);


                return Ok(message_);
                MarshallObject(oSBObob);
                MarshallObject(oCompany);


            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }


        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/CreateInvoice/{SAPUserName}/{SAPPassword}")]
        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult>  CreateInvoice(HttpRequestMessage request, string DBName, string SAPUserName, string SAPPassword)
        {
            try
            {

                var jsonString = await request.Content.ReadAsStringAsync();
                //context.Response.ContentType = "text/JSON";
                string output = "";
                string message = "";
                dynamic message_ = null;

                JObject json = JObject.Parse(jsonString);

                // string SourceNumber = "";
                // string PaymentReference;

                string DocDate = "";
                string PostingDate = "";
                string CardCode = "";
                string CardName = "";
                string InvoiceType = "";
                string SourceNumber = "";
                string GroupCode = "";
                string Action = "";
                string Rounding = "";
                string Reference = "";
                string Remarks = "";


                AddUpdateAppSettings("CompanyDB", DBName);
                AddUpdateAppSettings("manager", SAPUserName);
                AddUpdateAppSettings("Password", SAPPassword);

                DocDate = (string)json.SelectToken("Header").SelectToken("DocDate");
                PostingDate = (string)json.SelectToken("Header").SelectToken("PostingDate");
                CardCode = (string)json.SelectToken("Header").SelectToken("CardCode");
                CardName = (string)json.SelectToken("Header").SelectToken("CardName");
                InvoiceType = (string)json.SelectToken("Header").SelectToken("InvoiceType");
                SourceNumber = (string)json.SelectToken("Header").SelectToken("SourceNumber");
                // GroupCode = (string) json.SelectToken("Header").SelectToken("GroupCode");
                Action = (string)json.SelectToken("Header").SelectToken("Action");
                Rounding = (string)json.SelectToken("Header").SelectToken("Rounding");
                Reference = (string)json.SelectToken("Header").SelectToken("Reference");
                Remarks = (string)json.SelectToken("Header").SelectToken("Remarks");
                oCompany = new Connect_To_SAP().ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                //int checkif_customer_exist = 0;



                //checkif_customer_exist = Check_If_Customer_Exists(DBName, CardCode,SAPUserName,  SAPPassword);
                //if (checkif_customer_exist == 0)
                //{

                //    Create_Customer(CardCode, CardName);

                //}



                // string Tagged_Get_FatherCard = Get_FatherCard(BrokerCode);

                //SAPbobsCOM.BusinessPartners sboBP = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                SAPbobsCOM.Documents oInvoice = null;

                oInvoice = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                oInvoice.Rounding = BoYesNoEnum.tYES;
                oInvoice.DocDate = Convert.ToDateTime(DocDate);
                oInvoice.CardCode = CardCode;
                // oInvoice.SalesPersonCode = get_salespersoncode;
                oInvoice.NumAtCard = SourceNumber;
                //oInvoice.UserFields.Fields.Item("U_SourceNumber").Value = SourceNumber;
                oInvoice.Comments = Remarks + " " + "SourceNumber :" + SourceNumber;
                // oInvoice.BPL_IDAssignedToInvoice = Convert.ToInt32(Branch);

                JArray jarr = (JArray)json["Rows"];
                if (InvoiceType == "S")
                {
                    foreach (var item in jarr)
                    {
                        string Description, AcctCode, VatGroup, UnitPrice, LineTotal;

                        LineTotal = item.SelectToken("LineTotal").ToString();
                        Description = item.SelectToken("Description").ToString();
                        AcctCode = item.SelectToken("AcctCode").ToString();
                        UnitPrice = item.SelectToken("UnitPrice").ToString();
                        VatGroup = item.SelectToken("VatGroup").ToString();


                        oInvoice.Lines.ItemDescription = Description;
                        oInvoice.Lines.AccountCode = AcctCode;
                        oInvoice.Lines.VatGroup = VatGroup;
                        oInvoice.Lines.UnitPrice = Convert.ToDouble(UnitPrice);
                        oInvoice.Lines.LineTotal = Convert.ToDouble(LineTotal);
                        oInvoice.Lines.PriceAfterVAT = Convert.ToDouble(LineTotal);
                        oInvoice.Lines.Add();

                    }
                }
                else
                {

                    foreach (var item in jarr)
                    {
                        string ItemCode, Description, Quantity, UnitPrice, VatGroup, LineTotal, WarehouseCode;

                        ItemCode = item.SelectToken("ItemCode").ToString();
                        Description = item.SelectToken("Description").ToString();
                        Quantity = item.SelectToken("Quantity").ToString();
                        UnitPrice = item.SelectToken("UnitPrice").ToString();
                        VatGroup = item.SelectToken("VatGroup").ToString();
                        LineTotal = item.SelectToken("LineTotal").ToString();
                        WarehouseCode = item.SelectToken("WarehouseCode").ToString();


                        oInvoice.Lines.ItemCode = ItemCode;
                        oInvoice.Lines.Quantity = Convert.ToDouble(Quantity);
                        oInvoice.Lines.VatGroup = VatGroup;
                        oInvoice.Lines.UnitPrice = Convert.ToDouble(UnitPrice);
                        //oInvoice.Lines.LineTotal = Convert.ToDouble(LineTotal);
                        //oInvoice.Lines.PriceAfterVAT = Convert.ToDouble(LineTotal);
                        oInvoice.Lines.WarehouseCode = WarehouseCode;

                        oInvoice.Lines.Add();

                    }






                }
                if (oInvoice.Add() != 0)
                {

                    oCompany.GetLastError(out nErr, out erMsg);

                    erMsg = Sanitize_Errors(erMsg);
                    //.Replace(dbqt, "");
                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Source Number\": \"" + SourceNumber + "\",\"Destination Number\": \"0\",\"Document Type\": \"Invoice\"}}";
                    message_ = JsonConvert.DeserializeObject(message);
                    MarshallObject(oInvoice);
                    MarshallObject(oCompany);
                }
                else
                {
                    //int snum = Int32.Parse(oCompany.GetNewObjectKey());
                    // oInvoice.GetByKey(snum);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(oInvoice.GetAsXML());
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);
                    //message = jsonText;

                    string add_data = Get_InvoiceData(DBName, CardCode);
                    if (add_data.Length > 1)
                    {

                        message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"Successfully Created\"" + add_data;
                        // {\"Message\": {\"MessageType\": \"Success\",\"Description\": \"Successfully Created\",\"DocTotal\": \"3000\",\"VatSum\": \"0\",\"DiscSum\": \"0\"}}
                        //   message = message;
                        JObject json_data = JObject.Parse(message);
                        string DocEntry = json_data.SelectToken("Message").SelectToken("DocEntry").ToString();
                        message_ = JsonConvert.DeserializeObject(message);
                        SAP_SEND_MESSAGE("Invoice Added", "Invoice Added from API", "Customer Name", CardCode, "2", CardCode, "Invoice Number", DocEntry, "13", DocEntry);
                    }
                    else
                    {
                        erMsg = "Customer Code  or ItemCode was not found in this  Company " + DBName;
                        message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Source Number\": \"" + SourceNumber + "\",\"Destination Number\": \"0\",\"Document Type\": \"Invoice\"}}";
                        message_ = JsonConvert.DeserializeObject(message);
                        MarshallObject(oInvoice);
                        MarshallObject(oCompany);
                    }

                }
                
                MarshallObject(oInvoice);
                MarshallObject(oCompany);
                MarshallObject(QueryObject);
                MarshallObject(QueryObjectDocEntry);
                return Ok(message_);

            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }


        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/CreateMarketingDocument/")]
        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult>  CreateMarketingDocument(HttpRequestMessage request, string DBName)
        {
            try
            {

                var jsonString = await request.Content.ReadAsStringAsync();
                //context.Response.ContentType = "text/JSON";
                string output = "";
                string message = "";
                dynamic message_ = null;

                JObject json = JObject.Parse(jsonString);

                // string SourceNumber = "";
                // string PaymentReference;

                string DocDate = "";
                string PostingDate = "";
                string DocDueDate = "";
                string RequiredDate = "";
                string ValidUntil = "";
                string CardCode = "";
                string CardName = "";
                string ObjectType = "";
                string DocType = "";
                string SourceNumber = "";
                string Action = "";
                string Rounding = "";
                string Reference = "";
                string Remarks = "";
                string SalesPersonCode = "";
                string DocumentOwnerCode = "";
                string DocCurrency = "";
                string WVAT = "";
                string TaxbleAmnt = "";
                string WTAmnt = "";
                //  AddUpdateAppSettings("CompanyDB", DBName);
                //AddUpdateAppSettings("manager", SAPUserName);
                //AddUpdateAppSettings("Password", SAPPassword);

                DocDate = (string)json.SelectToken("Header").SelectToken("DocDate");
                PostingDate = (string)json.SelectToken("Header").SelectToken("PostingDate");
                DocDueDate = (string)json.SelectToken("Header").SelectToken("DocDueDate");
                DocCurrency = (string)json.SelectToken("Header").SelectToken("DocCurrency");
                RequiredDate = (string)json.SelectToken("Header").SelectToken("RequiredDate");
                ValidUntil = (string)json.SelectToken("Header").SelectToken("ValidUntil");
                ObjectType = (string)json.SelectToken("Header").SelectToken("ObjectType");
                CardCode = (string)json.SelectToken("Header").SelectToken("CardCode");
                CardName = (string)json.SelectToken("Header").SelectToken("CardName");
                Action = (string)json.SelectToken("Header").SelectToken("Action");
                SalesPersonCode = (string)json.SelectToken("Header").SelectToken("SalesPersonCode");
                DocumentOwnerCode = (string)json.SelectToken("Header").SelectToken("DocumentOwnerCode");



                DocType = (string)json.SelectToken("Header").SelectToken("DocType");
                SourceNumber = (string)json.SelectToken("Header").SelectToken("SourceNumber");
                Action = (string)json.SelectToken("Header").SelectToken("Action");
                Rounding = (string)json.SelectToken("Header").SelectToken("Rounding");
                Reference = (string)json.SelectToken("Header").SelectToken("Reference");
                Remarks = (string)json.SelectToken("Header").SelectToken("Remarks");
                string SAPUserName = userName;
                string SAPPassword = section["Password"];

                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);

                string Table = "";

                SAPbobsCOM.Documents oDoc = null;

                string ObjectTypeNum = "";
                string DraftsObjectTypeNum = "";
                if (ObjectType == "SALESQOUTATION")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oQuotations);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "OQUT";
                    ObjectTypeNum = "23";
                }
                else if (ObjectType == "SALESORDER")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                    oDoc.CardCode = CardCode;
                    oDoc.DocDueDate = Convert.ToDateTime(DocDueDate);
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ORDR";
                    ObjectTypeNum = "17";
                }
                else if (ObjectType == "SALESCREDITNOTE")
                {

                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oCreditNotes);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ORIN";
                    ObjectTypeNum = "14";
                }
                else if (ObjectType == "SALESRETURN")
                {

                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oReturns);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ORDN";
                    ObjectTypeNum = "16";
                }


                else if (ObjectType == "SALESINVOICE")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "OINV";
                    ObjectTypeNum = "13";

                }



                else if (ObjectType == "SALESQOUTATIONDRAFT")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oInvoices;
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "23";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "SALESORDERDRAFT")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders;
                    oDoc.CardCode = CardCode;
                    oDoc.DocDueDate = Convert.ToDateTime(DocDueDate);
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "17";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "SALESCREDITNOTEDRAFT")
                {

                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oCreditNotes;

                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "14";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "SALESRETURNDRAFT")
                {

                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oReturns;

                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "16";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "SALESINVOICEDRAFT")
                {
                    //oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oInvoices;
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "13";
                    DraftsObjectTypeNum = "112";

                }

                else if (ObjectType == "SALESDELIVERYNOTEDRAFT")
                {
                    //  oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oDeliveryNotes;
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "15";
                    DraftsObjectTypeNum = "112";

                }
                else if (ObjectType == "PURCHASEREQUESTDRAFT")
                {
                    // oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseRequest);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseRequest;

                    oDoc.RequriedDate = Convert.ToDateTime(RequiredDate);
                    oDoc.ClosingDate = Convert.ToDateTime(ValidUntil);
                    Table = "ODRF";
                    ObjectTypeNum = "1470000113";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "PURCHASEQOUTATIONDRAFT")
                {
                    // oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseQuotations);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseQuotations;
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "540000006";
                    DraftsObjectTypeNum = "112";
                    oDoc.RequriedDate = Convert.ToDateTime(RequiredDate);
                }
                else if (ObjectType == "PURCHASEORDERDRAFT")
                {
                    // oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseOrders;
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "22";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "PURCHASECREDITNOTEDRAFT")
                {

                    //   oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes;
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "19";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "GRPODRAFT")
                {
                    //  oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes;

                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "20";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "GOODSRETURNDRAFT")
                {
                    //oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseReturns);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseReturns;

                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    ObjectTypeNum = "21";
                    DraftsObjectTypeNum = "112";
                }
                else if (ObjectType == "PURCHASEINVOICEDRAFT")
                {
                    //  oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);

                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODRF";
                    DraftsObjectTypeNum = "112";
                    ObjectTypeNum = "18";


                }




                else if (ObjectType == "SALESDELIVERYNOTE")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ODLN";
                    ObjectTypeNum = "15";

                }
                else if (ObjectType == "PURCHASEREQUEST")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseRequest);
                    oDoc.RequriedDate = Convert.ToDateTime(RequiredDate);
                    oDoc.ClosingDate = Convert.ToDateTime(ValidUntil);
                    Table = "OPRQ";
                    ObjectTypeNum = "1470000113";
                }
                else if (ObjectType == "PURCHASEQOUTATION")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseQuotations);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "OPQT";
                    ObjectTypeNum = "540000006";
                    oDoc.RequriedDate = Convert.ToDateTime(RequiredDate);
                }
                else if (ObjectType == "PURCHASEORDER")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "OPOR";
                    ObjectTypeNum = "22";
                }
                else if (ObjectType == "PURCHASECREDITNOTE")
                {

                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ORPC";
                    ObjectTypeNum = "19";
                }
                else if (ObjectType == "GRPO")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "OPDN";
                    ObjectTypeNum = "20";
                }
                else if (ObjectType == "GOODSRETURN")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseReturns);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "ORPD";
                    ObjectTypeNum = "21";
                }
                else if (ObjectType == "PURCHASEINVOICE")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "OPCH";
                    ObjectTypeNum = "18";


                }


                oDoc.Rounding = BoYesNoEnum.tYES;

                oDoc.TaxDate = Convert.ToDateTime(PostingDate);
                oDoc.DocDate = Convert.ToDateTime(DocDate);
                // oDoc.SalesPersonCode = get_salespersoncode;

                if (!string.IsNullOrEmpty(SalesPersonCode))
                {
                    oDoc.SalesPersonCode = Convert.ToInt32(SalesPersonCode);
                }

                if (!string.IsNullOrEmpty(DocumentOwnerCode))
                {
                    oDoc.DocumentsOwner = Convert.ToInt32(DocumentOwnerCode);
                }
                oDoc.Comments = Remarks + " " + "SourceNumber :" + SourceNumber;

                if (string.IsNullOrEmpty(DocCurrency))
                {
                    DocCurrency = LocalCurrency(DBName, SAPUserName, SAPPassword);
                    oDoc.DocCurrency = DocCurrency;
                }
                else
                {
                    oDoc.DocCurrency = DocCurrency;
                }

                // oDoc.BPL_IDAssignedToDoc = Convert.ToInt32(Branch);

                JArray jarr = (JArray)json["Rows"];

                if (DocType == "S")
                {
                    foreach (var item in jarr)
                    {
                        string Description, AcctCode, VatGroup, UnitPrice, LineTotal;

                        LineTotal = item.SelectToken("LineTotal").ToString();
                        Description = item.SelectToken("Description").ToString();
                        AcctCode = item.SelectToken("AcctCode").ToString();
                        UnitPrice = item.SelectToken("UnitPrice").ToString();
                        VatGroup = item.SelectToken("VatGroup").ToString();

                        oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                        oDoc.Lines.ItemDescription = Description;
                        oDoc.Lines.AccountCode = AcctCode;
                        oDoc.Lines.VatGroup = VatGroup;
                        oDoc.Lines.UnitPrice = Convert.ToDouble(UnitPrice);
                        oDoc.Lines.LineTotal = Convert.ToDouble(LineTotal);
                        oDoc.Lines.PriceAfterVAT = Convert.ToDouble(LineTotal);
                        oDoc.Lines.Add();

                    }
                }
                else
                {

                    JToken token = JToken.Parse(jsonString);


                    for (int i = 0; i <= jarr.Count() - 1; i++)
                    {
                        string ItemCode, Description, Quantity, UnitPrice, VatGroup, LineTotal, WarehouseCode;

                        ItemCode = token.SelectToken("Rows[" + i + "]").SelectToken("ItemCode").ToString();
                        Description = token.SelectToken("Rows[" + i + "]").SelectToken("Description").ToString();
                        Quantity = token.SelectToken("Rows[" + i + "]").SelectToken("Quantity").ToString();
                        UnitPrice = token.SelectToken("Rows[" + i + "]").SelectToken("UnitPrice").ToString();
                        VatGroup = token.SelectToken("Rows[" + i + "]").SelectToken("VatGroup").ToString();
                        LineTotal = token.SelectToken("Rows[" + i + "]").SelectToken("LineTotal").ToString();
                        WarehouseCode = token.SelectToken("Rows[" + i + "]").SelectToken("WarehouseCode").ToString();

                        oDoc.Lines.ItemCode = ItemCode;
                        oDoc.Lines.Quantity = Convert.ToDouble(Quantity);
                        oDoc.Lines.VatGroup = VatGroup;
                        oDoc.Lines.WarehouseCode = WarehouseCode;
                        oDoc.Lines.Add();
                        // oDoc.Lines.WTLiable = BoYesNoEnum.tYES;
                        // oDoc.Lines.TaxLiable = BoYesNoEnum.tYES;
                        //    string batchmanageditem = Check_If_Item_BatchManaged(DBName, ItemCode, SAPUserName, SAPPassword);
                        //if (batchmanageditem == "Y")
                        //{
                        //    int count = token.SelectToken("Rows[" + i + "]").SelectToken("Batch").Count();
                        //    for (int CountBatChRows = 0; CountBatChRows <= count - 1; CountBatChRows++)
                        //    {
                        //        var jBatchObject = (JObject)token.SelectToken("Rows[" + i + "].Batch[" + CountBatChRows + "]");

                        //        string BatchNumber = jBatchObject["BatchNum"].ToString();
                        //        string BatchQuantity = jBatchObject["BatchQuantity"].ToString();

                        //        oDoc.Lines.BatchNumbers.BatchNumber = BatchNumber.ToString();

                        //        oDoc.Lines.BatchNumbers.Quantity = Convert.ToInt32(BatchQuantity);
                        //        oDoc.Lines.BatchNumbers.Add();

                        //    }
                        //    oDoc.Lines.Add();
                        //}
                        //else
                        //{
                        //    oDoc.Lines.Add();
                        //}

                    }




                }



                if (oDoc.Add() != 0)
                {

                    oCompany.GetLastError(out nErr, out erMsg);
                    //if (string.IsNullOrEmpty(erMsg))
                    //{

                    //    erMsg = "Missing or wrong Customer Code/or Tax Code or Missing  Item Code or Batch Number";
                    //}
                    //else
                    //{
                    erMsg = Sanitize_Errors(erMsg);
                    //}


                    //.Replace(dbqt, "");
                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Source Number\": \"" + SourceNumber + "\",\"Destination Number\": \"0\",\"Document Type\": \"" + ObjectType + "\"}}";
                    message_ = JsonConvert.DeserializeObject(message);
                    MarshallObject(oDoc);
                    MarshallObject(oCompany);
                }
                else
                {
                    //int snum = Int32.Parse(oCompany.GetNewObjectKey());
                    // oDoc.GetByKey(snum);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(oDoc.GetAsXML());
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);
                    //message = jsonText;

                    string add_data = Get_DocData(DBName, Table, CardCode);
                    if (add_data.Length > 1)
                    {

                        message = "{\"Message\": {\"MessageType\": \"Success\",\"ObjectType\": \"" + ObjectType + "\",\"Description\": \"Successfully Created\"" + add_data;

                        JObject json_data = JObject.Parse(message);
                        string DocEntry = json_data.SelectToken("Message").SelectToken("DocEntry").ToString();
                        message_ = JsonConvert.DeserializeObject(message);
                        if (!ObjectType.EndsWith("DRAFT"))
                        {
                            SAP_SEND_MESSAGE("Document " + ObjectType.ToLower() + " Added", "Document Added  " + ObjectType.ToLower() + " from API", "Business Partner Name", CardCode, "2", CardCode, ObjectType.ToLower() + " Number", DocEntry, ObjectTypeNum, DocEntry);
                        }
                        else
                        {

                            SAP_SEND_MESSAGE("Document " + ObjectType.ToLower() + " Added", "Document Added from " + ObjectType.ToLower() + " API", "Business Partner Name", CardCode, "2", CardCode, ObjectType.ToLower() + " Number", DocEntry, DraftsObjectTypeNum, DocEntry);

                        }
                    }
                    else
                    {
                        erMsg = "Customer Code  or ItemCode was not found in this  Company " + DBName;
                        message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Source Number\": \"" + SourceNumber + "\",\"Destination Number\": \"0\",\"Document Type\": \"" + ObjectType + "\"}}";
                        message_ = JsonConvert.DeserializeObject(message);
                        MarshallObject(oDoc);
                        MarshallObject(oCompany);
                    }

                }
                MarshallObject(QueryObjectDocEntry);
                MarshallObject(QueryObject);
                MarshallObject(oDoc);
                MarshallObject(oCompany);
                return Ok(message_);

            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }


        }



        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/CreatePurchaseDocuments/{SAPUserName}/{SAPPassword}")]
        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult>  CreatePurchaseDocuments(HttpRequestMessage request, string DBName, string SAPUserName, string SAPPassword)
        {
            try
            {

                var jsonString = await request.Content.ReadAsStringAsync();
                //context.Response.ContentType = "text/JSON";
                string output = "";
                string message = "";
                dynamic message_ = null;

                JObject json = JObject.Parse(jsonString);

                // string SourceNumber = "";
                // string PaymentReference;

                string DocDate = "";
                string PostingDate = "";
                string DocDueDate = "";
                string RequiredDate = "";
                string ValidUntil = "";
                string CardCode = "";
                string CardName = "";
                string ObjectType = "";
                string DocType = "";
                string SourceNumber = "";
                string Action = "";
                string Rounding = "";
                string Reference = "";
                string Remarks = "";
                string SalesPersonCode = "";
                string DocumentOwnerCode = "";
                string DocCurrency = "";
                string WVAT = "";
                string TaxbleAmnt = "";
                string WTAmnt = "";
                AddUpdateAppSettings("CompanyDB", DBName);
                AddUpdateAppSettings("manager", SAPUserName);
                AddUpdateAppSettings("Password", SAPPassword);

                DocDate = (string)json.SelectToken("Header").SelectToken("DocDate");
                PostingDate = (string)json.SelectToken("Header").SelectToken("PostingDate");
                DocDueDate = (string)json.SelectToken("Header").SelectToken("DocDueDate");
                DocCurrency = (string)json.SelectToken("Header").SelectToken("DocCurrency");
                RequiredDate = (string)json.SelectToken("Header").SelectToken("RequiredDate");
                ValidUntil = (string)json.SelectToken("Header").SelectToken("ValidUntil");
                ObjectType = (string)json.SelectToken("Header").SelectToken("ObjectType");
                CardCode = (string)json.SelectToken("Header").SelectToken("CardCode");
                CardName = (string)json.SelectToken("Header").SelectToken("CardName");
                Action = (string)json.SelectToken("Header").SelectToken("Action");
                SalesPersonCode = (string)json.SelectToken("Header").SelectToken("SalesPersonCode");
                DocumentOwnerCode = (string)json.SelectToken("Header").SelectToken("DocumentOwnerCode");



                DocType = (string)json.SelectToken("Header").SelectToken("DocType");
                SourceNumber = (string)json.SelectToken("Header").SelectToken("SourceNumber");
                Action = (string)json.SelectToken("Header").SelectToken("Action");
                Rounding = (string)json.SelectToken("Header").SelectToken("Rounding");
                Reference = (string)json.SelectToken("Header").SelectToken("Reference");
                Remarks = (string)json.SelectToken("Header").SelectToken("Remarks");
                oCompany = new Connect_To_SAP().ConnectSAPDB(DBName, SAPUserName, SAPPassword);

                string Table = "";

                SAPbobsCOM.Documents oDoc = null;

                string ObjectTypeNum = "";
                string DraftsObjectTypeNum = "";
                if (ObjectType == "PURCHASEINVOICE")
                {
                    oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                    oDoc.CardCode = CardCode;
                    oDoc.NumAtCard = SourceNumber;
                    Table = "OPCH";
                    ObjectTypeNum = "18";

                    WVAT = (string)json.SelectToken("WithholdingTax").SelectToken("WTCode");
                    TaxbleAmnt = (string)json.SelectToken("WithholdingTax").SelectToken("TaxableAmount");
                    WTAmnt = (string)json.SelectToken("WithholdingTax").SelectToken("WTAmount");

                }


                oDoc.Rounding = BoYesNoEnum.tYES;

                oDoc.TaxDate = Convert.ToDateTime(PostingDate);
                oDoc.DocDate = Convert.ToDateTime(DocDate);
                // oDoc.SalesPersonCode = get_salespersoncode;

                if (!string.IsNullOrEmpty(SalesPersonCode))
                {
                    oDoc.SalesPersonCode = Convert.ToInt32(SalesPersonCode);
                }

                if (!string.IsNullOrEmpty(DocumentOwnerCode))
                {
                    oDoc.DocumentsOwner = Convert.ToInt32(DocumentOwnerCode);
                }
                oDoc.Comments = Remarks + " " + "SourceNumber :" + SourceNumber;

                if (string.IsNullOrEmpty(DocCurrency))
                {
                    DocCurrency = LocalCurrency(DBName, SAPUserName, SAPPassword);
                    oDoc.DocCurrency = DocCurrency;
                }
                else
                {
                    oDoc.DocCurrency = DocCurrency;
                }

                // oDoc.BPL_IDAssignedToDoc = Convert.ToInt32(Branch);

                JArray jarr = (JArray)json["Rows"];

                if (DocType == "S")
                {
                    foreach (var item in jarr)
                    {
                        string Description, AcctCode, VatGroup, UnitPrice, LineTotal;

                        LineTotal = item.SelectToken("LineTotal").ToString();
                        Description = item.SelectToken("Description").ToString();
                        AcctCode = item.SelectToken("AcctCode").ToString();
                        UnitPrice = item.SelectToken("UnitPrice").ToString();
                        VatGroup = item.SelectToken("VatGroup").ToString();

                        oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                        oDoc.Lines.ItemDescription = Description;
                        oDoc.Lines.AccountCode = AcctCode;
                        oDoc.Lines.VatGroup = VatGroup;
                        oDoc.Lines.UnitPrice = Convert.ToDouble(UnitPrice);
                        oDoc.Lines.LineTotal = Convert.ToDouble(LineTotal);
                        oDoc.Lines.PriceAfterVAT = Convert.ToDouble(LineTotal);
                        oDoc.Lines.Add();

                    }
                }
                else
                {

                    JToken token = JToken.Parse(jsonString);


                    for (int i = 0; i <= jarr.Count() - 1; i++)
                    {
                        string ItemCode, Description, Quantity, UnitPrice, VatGroup, LineTotal, WarehouseCode;

                        ItemCode = token.SelectToken("Rows[" + i + "]").SelectToken("ItemCode").ToString();
                        Description = token.SelectToken("Rows[" + i + "]").SelectToken("Description").ToString();
                        Quantity = token.SelectToken("Rows[" + i + "]").SelectToken("Quantity").ToString();
                        UnitPrice = token.SelectToken("Rows[" + i + "]").SelectToken("UnitPrice").ToString();
                        VatGroup = token.SelectToken("Rows[" + i + "]").SelectToken("VatGroup").ToString();
                        LineTotal = token.SelectToken("Rows[" + i + "]").SelectToken("LineTotal").ToString();
                        WarehouseCode = token.SelectToken("Rows[" + i + "]").SelectToken("WarehouseCode").ToString();

                        oDoc.Lines.ItemCode = ItemCode;
                        oDoc.Lines.Quantity = Convert.ToDouble(Quantity);
                        oDoc.Lines.VatGroup = VatGroup;
                        oDoc.Lines.WarehouseCode = WarehouseCode;

                        oDoc.Lines.WTLiable = BoYesNoEnum.tYES;
                        oDoc.Lines.TaxLiable = BoYesNoEnum.tYES;
                        string batchmanageditem = Check_If_Item_BatchManaged(DBName, ItemCode, SAPUserName, SAPPassword);
                        if (batchmanageditem == "Y")
                        {
                            int count = token.SelectToken("Rows[" + i + "]").SelectToken("Batch").Count();
                            for (int CountBatChRows = 0; CountBatChRows <= count - 1; CountBatChRows++)
                            {
                                var jBatchObject = (JObject)token.SelectToken("Rows[" + i + "].Batch[" + CountBatChRows + "]");

                                string BatchNumber = jBatchObject["BatchNum"].ToString();
                                string BatchQuantity = jBatchObject["BatchQuantity"].ToString();

                                oDoc.Lines.BatchNumbers.BatchNumber = BatchNumber.ToString();

                                oDoc.Lines.BatchNumbers.Quantity = Convert.ToInt32(BatchQuantity);
                                oDoc.Lines.BatchNumbers.Add();

                            }
                            oDoc.Lines.Add();
                        }
                        else
                        {
                            oDoc.Lines.Add();
                        }

                    }




                }
                oDoc.WithholdingTaxData.WTCode = WVAT;
                oDoc.WithholdingTaxData.TaxableAmount = Convert.ToDouble(TaxbleAmnt);
                //  oDoc.WithholdingTaxData.WTAmount = Convert.ToDouble(WTAmnt);
                oDoc.WithholdingTaxData.Add();
                if (oDoc.Add() != 0)
                {

                    oCompany.GetLastError(out nErr, out erMsg);
                    //if (string.IsNullOrEmpty(erMsg))
                    //{

                    //    erMsg = "Missing or wrong Customer Code/or Tax Code or Missing  Item Code or Batch Number";
                    //}
                    //else
                    //{
                    //    erMsg = Sanitize_Errors(erMsg);
                    //}


                    //.Replace(dbqt, "");
                    message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Source Number\": \"" + SourceNumber + "\",\"Destination Number\": \"0\",\"Document Type\": \"" + ObjectType + "\"}}";
                    message_ = JsonConvert.DeserializeObject(message);
                    MarshallObject(oDoc);
                    MarshallObject(oCompany);
                }
                else
                {
                    //int snum = Int32.Parse(oCompany.GetNewObjectKey());
                    // oDoc.GetByKey(snum);
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(oDoc.GetAsXML());
                    //string jsonText = JsonConvert.SerializeXmlNode(doc);
                    //message = jsonText;

                    string add_data = Get_DocData(DBName, Table, CardCode);
                    if (add_data.Length > 1)
                    {

                        message = "{\"Message\": {\"MessageType\": \"Success\",\"ObjectType\": \"" + ObjectType + "\",\"Description\": \"Successfully Created\"" + add_data;

                        JObject json_data = JObject.Parse(message);
                        string DocEntry = json_data.SelectToken("Message").SelectToken("DocEntry").ToString();
                        message_ = JsonConvert.DeserializeObject(message);
                        if (!ObjectType.EndsWith("DRAFT"))
                        {
                            SAP_SEND_MESSAGE("Document " + ObjectType.ToLower() + " Added", "Document Added  " + ObjectType.ToLower() + " from API", "Business Partner Name", CardCode, "2", CardCode, ObjectType.ToLower() + " Number", DocEntry, ObjectTypeNum, DocEntry);
                        }
                        else
                        {

                            SAP_SEND_MESSAGE("Document " + ObjectType.ToLower() + " Added", "Document Added from " + ObjectType.ToLower() + " API", "Business Partner Name", CardCode, "2", CardCode, ObjectType.ToLower() + " Number", DocEntry, DraftsObjectTypeNum, DocEntry);

                        }
                    }


                }
                
                MarshallObject(QueryObjectDocEntry);
                MarshallObject(QueryObject);
                MarshallObject(oDoc);
                MarshallObject(oCompany);
                return Ok(message_);

            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }


        }
        private void Write_Variable(string Message)
        {
            try
            {
                string appPath = @"C:\test\Values.txt";
                using (StreamWriter writer = new StreamWriter(appPath, true))
                {
                    writer.WriteLine(Message);
                }
            }
            catch (Exception ex)
            {
                //  Get_specific_errorline(ex, "write_Error_to_file");
            }
        }

        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("api/SAP/{DBName}/CreatePayment/")]

        //public IActionResult Post([FromBody]string value)
        //{
        public async Task<IActionResult>  CreatePayment(HttpRequestMessage request, string DBName)
        {
            try
            {
                string message = "";
                dynamic message_ = null;
                var jsonString = await request.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(jsonString);


                string DocDate = "";
                string PostingDate = "";
                string CardCode = "";
                string CardType = "";
                string Action = "";
                string Rounding = "";
                string SourceNumber = "";
                string InvoiceDocEntry = "";
                string ReceiptNo = "";
                int DocEntry = 0;
                string Type = "";
                string DocCurrency = "";


                DocDate = (string)json.SelectToken("Header").SelectToken("DocDate");
                PostingDate = (string)json.SelectToken("Header").SelectToken("PostingDate");
                CardCode = (string)json.SelectToken("Header").SelectToken("CardCode");
                CardType = (string)json.SelectToken("Header").SelectToken("CardType");
                Action = (string)json.SelectToken("Header").SelectToken("Action");
                Rounding = (string)json.SelectToken("Header").SelectToken("Rounding");
                SourceNumber = (string)json.SelectToken("Header").SelectToken("SourceNumber");
                InvoiceDocEntry = (string)json.SelectToken("Header").SelectToken("InvoiceDocEntry");
                ReceiptNo = (string)json.SelectToken("Header").SelectToken("ReceiptNo");
                Type = (string)json.SelectToken("Header").SelectToken("Type");
                DocCurrency = (string)json.SelectToken("Header").SelectToken("DocCurrency");


                SAPbobsCOM.Payments oPayment = null;

                string SAPUserName = userName;
                string SAPPassword = section["Password"];

                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                if (CardType == "C" && Type == "INCOMING")
                {
                    oPayment = (SAPbobsCOM.Payments)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                    oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments;
                    // oPmt.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments
                    oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                    if (string.IsNullOrEmpty(InvoiceDocEntry))
                    {
                        DocEntry = Get_DocEntry(DBName, SourceNumber, CardCode);
                    }
                    else
                    {
                        DocEntry = Convert.ToInt32(InvoiceDocEntry);
                    }


                }
                else if (CardType == "S" && Type == "INCOMING")
                {
                    oPayment = (SAPbobsCOM.Payments)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                    oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments;
                    oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                    if (string.IsNullOrEmpty(InvoiceDocEntry))
                    {
                        DocEntry = Get_DocEntry(DBName, SourceNumber, CardCode);
                    }
                    else
                    {
                        DocEntry = Convert.ToInt32(InvoiceDocEntry);
                    }

                }

                else if (CardType == "S" && Type == "OUTGOING")
                {
                    oPayment = (SAPbobsCOM.Payments)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                    oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
                    oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;

                }

                else if (CardType == "C" && Type == "OUTGOING")
                {
                    oPayment = (SAPbobsCOM.Payments)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                    oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
                    oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                }


                //if (string.IsNullOrEmpty(DocCurrency))
                //{
                //    DocCurrency = LocalCurrency(DBName, SAPUserName, SAPPassword);
                //    oPayment.DocCurrency = DocCurrency;
                //}
                //else
                //{
                //    oPayment.DocCurrency = DocCurrency;
                //}



                JArray jPayments = (JArray)json["Payments"];
                foreach (var Payment_item in jPayments)
                {
                    string PaymentDate, PaymentReference, PaymentType, Account, Amount;

                    PaymentDate = Payment_item.SelectToken("PaymentDate").ToString();
                    PaymentReference = Payment_item.SelectToken("PaymentReference").ToString();
                    PaymentType = Payment_item.SelectToken("PaymentType").ToString().ToUpper();
                    Account = Payment_item.SelectToken("Account").ToString();
                    Amount = Payment_item.SelectToken("Amount").ToString();

                    if (!string.IsNullOrEmpty(CardCode))
                    {




                        if (PaymentType == "CHEQUE")
                        {

                            if (DocEntry != 0)
                            {

                                oPayment.Invoices.DocEntry = DocEntry;
                                oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.CardCode = CardCode;
                                //oPayment.BPLID = Branchid;
                                oPayment.Checks.CheckAccount = Account;
                                oPayment.Remarks = PaymentReference;
                                oPayment.Checks.CheckNumber = Convert.ToInt32(PaymentReference);
                                oPayment.Checks.CheckSum = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;
                                string CountryCode = Get_HouseBankCountry(DBName, Account);
                                oPayment.Checks.CountryCode = CountryCode;
                                string BankCode = Get_HouseBankCode(DBName, Account);
                                oPayment.Checks.BankCode = BankCode;
                                oPayment.Checks.ManualCheck = SAPbobsCOM.BoYesNoEnum.tNO;
                                oPayment.Checks.Trnsfrable = SAPbobsCOM.BoYesNoEnum.tNO;
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;
                            }
                            else
                            {
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                // // oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.CardCode = CardCode;
                                //oPayment.BPLID = Branchid;
                                oPayment.Checks.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.Remarks = PaymentReference;
                                oPayment.Checks.CheckNumber = Convert.ToInt32(PaymentReference);
                                oPayment.Checks.CheckSum = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;
                                oPayment.Checks.CheckAccount = Account;
                                string CountryCode = Get_HouseBankCountry(DBName, Account);
                                oPayment.Checks.CountryCode = CountryCode;
                                string BankCode = Get_HouseBankCode(DBName, Account);
                                oPayment.Checks.BankCode = BankCode;
                                oPayment.Checks.ManualCheck = SAPbobsCOM.BoYesNoEnum.tNO;

                                oPayment.Checks.Trnsfrable = SAPbobsCOM.BoYesNoEnum.tNO;
                                // // // oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);

                                oPayment.ApplyVAT = BoYesNoEnum.tNO;

                            }

                        }

                        else if (PaymentType == "MOBILEPAYMENT")
                        {
                            //oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                            if (DocEntry != 0)
                            {

                                oPayment.Invoices.DocEntry = DocEntry;
                                oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                                oPayment.CardCode = CardCode;
                                // oPayment.BPLID = Branchid;
                                oPayment.TransferAccount = Account;
                                oPayment.Remarks = PaymentReference;
                                oPayment.TransferDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TransferReference = (PaymentReference);
                                oPayment.TransferSum = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;





                            }
                            else
                            {
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TransferAccount = Account;
                                oPayment.CardCode = CardCode;
                                //   oPayment.BPLID = Branchid;
                                oPayment.TransferDate = Convert.ToDateTime(PaymentDate);
                                oPayment.Remarks = PaymentReference;
                                oPayment.TransferReference = (PaymentReference);
                                oPayment.TransferSum = Convert.ToDouble(Amount);
                                // // oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;

                            }

                        }

                        else if (PaymentType == "CASH")
                        {
                            // oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                            if (DocEntry != 0)
                            {

                                oPayment.Invoices.DocEntry = DocEntry;
                                oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.CashSum = Convert.ToDouble(Amount);
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.CashAccount = Account;
                                oPayment.CardCode = CardCode;
                                //   oPayment.BPLID = Branchid;
                                oPayment.Remarks = PaymentReference;
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;

                            }
                            else
                            {
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.CashAccount = Account;
                                oPayment.CardCode = CardCode;
                                //   oPayment.BPLID = Branchid;
                                oPayment.Remarks = PaymentReference;
                                oPayment.CashSum = Convert.ToDouble(Amount);
                                // // oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;

                            }

                        }
                        else if (PaymentType == "BANKTRANSFER")
                        {
                            // oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                            if (DocEntry != 0)
                            {

                                oPayment.Invoices.DocEntry = DocEntry;
                                oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                                oPayment.CardCode = CardCode;
                                // oPayment.BPLID = Branchid;
                                oPayment.TransferAccount = Account;
                                oPayment.Remarks = PaymentReference;
                                oPayment.TransferDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TransferReference = (PaymentReference);
                                oPayment.TransferSum = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;
                            }
                            else
                            {
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TransferAccount = Account;
                                oPayment.CardCode = CardCode;
                                //   oPayment.BPLID = Branchid;
                                oPayment.TransferDate = Convert.ToDateTime(PaymentDate);
                                oPayment.Remarks = PaymentReference;
                                oPayment.TransferReference = (PaymentReference);
                                oPayment.TransferSum = Convert.ToDouble(Amount);
                                //// oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;

                            }

                        }

                        else if (PaymentType == "CREDITCARD")
                        {

                            if (DocEntry != 0)
                            {

                                oPayment.Invoices.DocEntry = DocEntry;
                                oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.CardCode = CardCode;

                                oPayment.TransferDate = Convert.ToDateTime(PaymentDate);
                                oPayment.Remarks = PaymentReference;
                                oPayment.CreditCards.CreditSum = Convert.ToDouble(Amount);
                                //DateTime cr_year = DateTime.Today.ToString("yyyy-MM-dd"));
                                //DateTime cr_month = (DateTime.Today.ToString("yyyy-MM-dd"));
                                oPayment.CreditCards.CardValidUntil = DateTime.Now;
                                oPayment.CreditCards.CreditAcct = Account;
                                oPayment.CreditCards.CreditCard = Get_CreditCard(DBName, Account);
                                oPayment.CreditCards.CreditCardNumber = PaymentReference;
                                oPayment.CreditCards.VoucherNum = PaymentReference;
                                oPayment.CreditCards.FirstPaymentDue = Convert.ToDateTime(PaymentDate);
                                oPayment.CreditCards.NumOfPayments = 1;
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;


                            }
                            else
                            {
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.CardCode = CardCode;
                                //   oPayment.BPLID = Branchid;
                                oPayment.TransferDate = Convert.ToDateTime(PaymentDate);
                                oPayment.Remarks = PaymentReference;
                                oPayment.CreditCards.CreditSum = Convert.ToDouble(Amount);
                                //DateTime cr_year = DateTime.Today.ToString("yyyy-MM-dd"));
                                //DateTime cr_month = (DateTime.Today.ToString("yyyy-MM-dd"));
                                oPayment.CreditCards.CardValidUntil = DateTime.Now;
                                oPayment.CreditCards.CreditAcct = Account;
                                oPayment.CreditCards.CreditCard = Get_CreditCard(DBName, Account);
                                oPayment.CreditCards.CreditCardNumber = PaymentReference;
                                oPayment.CreditCards.VoucherNum = PaymentReference;
                                oPayment.CreditCards.FirstPaymentDue = Convert.ToDateTime(PaymentDate);
                                oPayment.CreditCards.NumOfPayments = 1;
                                // // oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;

                            }

                        }


                        else if (PaymentType == "OTHER")
                        {
                            //oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                            if (DocEntry != 0)
                            {

                                oPayment.Invoices.DocEntry = DocEntry;
                                oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                                oPayment.CardCode = CardCode;
                                // oPayment.BPLID = Branchid;
                                oPayment.TransferAccount = Account;
                                oPayment.Remarks = PaymentReference;
                                oPayment.TransferDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TransferReference = (PaymentReference);
                                oPayment.TransferSum = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;
                            }
                            else
                            {
                                oPayment.DocDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TaxDate = Convert.ToDateTime(PaymentDate);
                                oPayment.DueDate = Convert.ToDateTime(PaymentDate);
                                oPayment.TransferAccount = Account;
                                oPayment.CardCode = CardCode;
                                //   oPayment.BPLID = Branchid;
                                oPayment.TransferDate = Convert.ToDateTime(PaymentDate);
                                oPayment.Remarks = PaymentReference;
                                oPayment.TransferReference = (PaymentReference);
                                oPayment.TransferSum = Convert.ToDouble(Amount);
                                //// oPayment.Invoices.SumApplied = Convert.ToDouble(Amount);
                                oPayment.ApplyVAT = BoYesNoEnum.tNO;

                            }

                        }



                    }

                    if (oPayment.Add() != 0)
                    {

                        oCompany.GetLastError(out nErr, out erMsg);

                        message = "{\"Message\": {\"MessageType\": \"Error\",\"Description\": \"" + erMsg + "\",\"Document Type \": \"Payment\"}}";
                        message_ = JsonConvert.DeserializeObject(message);
                    }
                    else
                    {
                        string DocNum = GetPaymentDocEntry(DBName, CardCode, CardType, PaymentType, Convert.ToDouble(Amount), SAPUserName, SAPPassword);

                        message = "{\"Message\": {\"MessageType\": \"Success\",\"Description\": \"Successfully Created\",\"Doc Entry\": \"" + DocNum + "\",\"Document Type\": \"Payment\"}}";
                        message_ = JsonConvert.DeserializeObject(message);
                    }
                }
                MarshallObject(oPayment);
                MarshallObject(oCompany);
                return Ok(message_);
            }

            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }
        }

        public string Get_InvoiceData(string DBName, string CardCode)
        {

            {
                string results = "";
                string DocTotal = "";
                string VatSum = "";
                string DiscSum = "";
                QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                QueryObjectDocEntry = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                string querystringDocEntry = "";
                string DocEntry = "";
                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystringDocEntry = "SELECT  Top 1 \"DocEntry\"  FROM \"OINV\"   ORDER BY  DocEntry DESC";
                    QueryObjectDocEntry.DoQuery(querystringDocEntry);
                    DocEntry = (QueryObjectDocEntry.Fields.Item(0).Value.ToString());
                    querystring = "SELECT   \"DocTotal\" ,\"VatSum\" ,\"DiscSum\"  FROM \"OINV\"   WHERE \"DocEntry\" = '" + DocEntry + "'";


                }
                else
                {
                    querystringDocEntry = "SELECT  Top 1 DocEntry  FROM   " + Sanitize(DBName) + ".[dbo]." + "OINV   (nolock) where CardCode ='" + Sanitize(CardCode) + "'  ORDER BY  DocEntry DESC";
                    QueryObjectDocEntry.DoQuery(querystringDocEntry);
                    DocEntry = (QueryObjectDocEntry.Fields.Item(0).Value.ToString());
                    querystring = "SELECT   DocTotal ,VatSum,DiscSum FROM " + Sanitize(DBName) + ".[dbo]." + "OINV  (nolock) WHERE DocEntry = '" + DocEntry + "' and CardCode ='" + Sanitize(CardCode) + "'";

                }


                QueryObject.DoQuery(querystring);
                DocTotal = (QueryObject.Fields.Item(0).Value.ToString());
                VatSum = (QueryObject.Fields.Item(1).Value.ToString());
                DiscSum = (QueryObject.Fields.Item(2).Value.ToString());


                results = ",\"DocEntry\": \"" + DocEntry + "\",\"DocTotal\": \"" + DocTotal + "\",\"VatSum\": \"" + VatSum + "\",\"DiscSum\": \"" + DiscSum + "\"}}";

                //results = @"\""DocTotal\"": \""results1\""""
                return results;

            }
        }


        public string Get_DocData(string DBName, string Table, string CardCode)
        {

            {
                string results = "";
                string DocNum = "";
                string DocTotal = "";
                string VatSum = "";
                string DiscSum = "";
                QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                QueryObjectDocEntry = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                string querystringDocEntry = "";
                string DocEntry = "";
                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystringDocEntry = "SELECT  Top 1 \"DocEntry\"  FROM \"+ Table +\"   ORDER BY  DocEntry DESC";
                    QueryObjectDocEntry.DoQuery(querystringDocEntry);
                    DocEntry = (QueryObjectDocEntry.Fields.Item(0).Value.ToString());
                    querystring = "SELECT \"DocNum\" ,  \"DocTotal\" ,\"VatSum\" ,\"DiscSum\"  FROM \"OINV\"   WHERE \"DocEntry\" = '" + DocEntry + "'";


                }
                else
                {
                    if (!string.IsNullOrEmpty(CardCode))
                    {

                        querystringDocEntry = "SELECT  Top 1 DocEntry  FROM   " + Sanitize(DBName) + ".[dbo]. " + Table + "   (nolock) where CardCode ='" + Sanitize(CardCode) + "'  ORDER BY  DocEntry DESC";
                        QueryObjectDocEntry.DoQuery(querystringDocEntry);
                        DocEntry = (QueryObjectDocEntry.Fields.Item(0).Value.ToString());
                        querystring = "SELECT  DocNum, DocTotal ,VatSum,DiscSum FROM " + Sanitize(DBName) + ".[dbo]. " + Table + "(nolock) WHERE DocEntry = '" + DocEntry + "' and CardCode ='" + Sanitize(CardCode) + "'";


                    }
                    else
                    {
                        querystringDocEntry = "SELECT  Top 1 DocEntry  FROM   " + Sanitize(DBName) + ".[dbo]. " + Table + "   (nolock)   ORDER BY  DocEntry DESC";
                        QueryObjectDocEntry.DoQuery(querystringDocEntry);
                        DocEntry = (QueryObjectDocEntry.Fields.Item(0).Value.ToString());
                        querystring = "SELECT   DocNum ,DocTotal ,VatSum,DiscSum FROM " + Sanitize(DBName) + ".[dbo]. " + Table + "(nolock) WHERE DocEntry = '" + DocEntry + "'";


                    }

                }


                QueryObject.DoQuery(querystring);
                DocNum = (QueryObject.Fields.Item(0).Value.ToString());
                DocTotal = (QueryObject.Fields.Item(1).Value.ToString());
                VatSum = (QueryObject.Fields.Item(2).Value.ToString());
                DiscSum = (QueryObject.Fields.Item(3).Value.ToString());


                results = ",\"DocEntry\": \"" + DocEntry + "\",\"DocNum\": \"" + DocNum + "\",\"DocTotal\": \"" + DocTotal + "\",\"VatSum\": \"" + VatSum + "\",\"DiscSum\": \"" + DiscSum + "\"}}";

                //results = @"\""DocTotal\"": \""results1\""""
                return results;

            }
        }

        public string Get_HouseBankCountry(string DBName, string Account)
        {

            {
                string results = "";
                Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                //string querystring = "SELECT   DocEntry FROM OINV  (nolock) WHERE NumAtCard = '" + Sanitize(InvoiceReference) + "'  and  CardCode ='" + Sanitize(CardCode) + "'";

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "SELECT  TOP 1  \"Country\" FROM  \"" + (DBName) + "\" + \".DSC1\"   WHERE \"GLAccount\" = '" + (Account) + "'";

                }
                else
                {
                    querystring = "SELECT   TOP 1  Country FROM  " + (DBName) + ".[dbo]." + "DSC1  (nolock) WHERE GLAccount = '" + (Account) + "'";

                }


                QueryObject.DoQuery(querystring);
                results = QueryObject.Fields.Item(0).Value.ToString();
                if (QueryObject != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(QueryObject);
                    QueryObject = null;
                }
                return results;

            }
        }

        public string Get_HouseBankCode(string DBName, string Account)
        {

            {
                string results = "";
                Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                //string querystring = "SELECT   DocEntry FROM OINV  (nolock) WHERE NumAtCard = '" + Sanitize(InvoiceReference) + "'  and  CardCode ='" + Sanitize(CardCode) + "'";

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "SELECT  TOP 1 \"BankCode\" FROM  \"" + Sanitize(DBName) + "\" + \".DSC1\"   WHERE \"GLAccount\" = '" + Sanitize(Account) + "'";

                }
                else
                {
                    querystring = "SELECT  TOP 1   BankCode FROM  " + Sanitize(DBName) + ".[dbo]." + "DSC1  (nolock) WHERE GLAccount = '" + Sanitize(Account) + "'";

                }


                QueryObject.DoQuery(querystring);
                results = QueryObject.Fields.Item(0).Value.ToString();
                if (QueryObject != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(QueryObject);
                    QueryObject = null;
                }
                return results;

            }
        }


        public int Get_CreditCard(string DBName, string Account)
        {

            {
                int results = 1;
                Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                //string querystring = "SELECT   DocEntry FROM OINV  (nolock) WHERE NumAtCard = '" + Sanitize(InvoiceReference) + "'  and  CardCode ='" + Sanitize(CardCode) + "'";

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "SELECT TOP 1  \"CreditCard\" FROM  \"" + Sanitize(DBName) + "\" + \".OCRC\"   WHERE \"AcctCode\" = '" + (Account) + "'";

                }
                else
                {
                    querystring = "SELECT TOP 1  CreditCard FROM  " + Sanitize(DBName) + ".[dbo]." + "OCRC  (nolock) WHERE AcctCode = '" + (Account) + "'";

                }


                QueryObject.DoQuery(querystring);
                results = Convert.ToInt32(QueryObject.Fields.Item(0).Value.ToString());
                if (QueryObject != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(QueryObject);
                    QueryObject = null;
                }
                return results;

            }
        }
        public int Get_DocEntry(string DBName, string InvoiceReference, string CardCode)
        {

            {
                int results = 0;
                Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                //string querystring = "SELECT   DocEntry FROM OINV  (nolock) WHERE NumAtCard = '" + Sanitize(InvoiceReference) + "'  and  CardCode ='" + Sanitize(CardCode) + "'";

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "SELECT TOP 1   \"DocEntry\" FROM  \"" + Sanitize(DBName) + "\" + \".OINV\"   WHERE \"NumAtCard\" = '" + Sanitize(InvoiceReference) + "'  and  \"CardCode\" ='" + Sanitize(CardCode) + "'";

                }
                else
                {
                    querystring = "SELECT TOP 1  DocEntry FROM  " + Sanitize(DBName) + ".[dbo]." + "OINV  (nolock) WHERE NumAtCard = '" + Sanitize(InvoiceReference) + "'  and  CardCode ='" + Sanitize(CardCode) + "'";

                }


                QueryObject.DoQuery(querystring);
                results = Convert.ToInt32(QueryObject.Fields.Item(0).Value.ToString());
                if (QueryObject != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(QueryObject);
                    QueryObject = null;
                }
                return results;

            }
        }
        public void MarshallObject(object Object)
        {
            try
            {
                if (Object != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(Object);
                    Object = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void SAP_SEND_MESSAGE(string subject, string message, string ColumnOneName, string ColumnOneValue, string ColumnOneObject, string ColumnOneObjectKey, string ColumnTwoName, string ColumnTwoValue, string ColumnTwoObject, string ColumnTwoObjectKey)
        {
            try
            {
                if (MessageFlag == "ON")
                {

                    SAPbobsCOM.CompanyService oCmpSrv;
                    MessagesService oMessageService;

                    //get company service
                    oCmpSrv = oCompany.GetCompanyService();

                    oMessageService = (SAPbobsCOM.MessagesService)oCmpSrv.GetBusinessService(ServiceTypes.MessagesService);

                    SAPbobsCOM.Message oMessage = null;
                    MessageDataColumns pMessageDataColumns = null;
                    MessageDataColumn pMessageDataColumn = null;
                    MessageDataLines oLines = null;
                    MessageDataLine oLine = null;
                    RecipientCollection oRecipientCollection = null;

                    // get the data interface for the new message
                    oMessage = ((SAPbobsCOM.Message)(oMessageService.GetDataInterface(MessagesServiceDataInterfaces.msdiMessage)));

                    // fill subject
                    oMessage.Subject = subject;
                    oMessage.Text = message;

                    // Add Recipient 
                    oRecipientCollection = oMessage.RecipientCollection;

                    // se agregan dos usuarios a los cuales les llegara el mensaje/alerta
                    oRecipientCollection.Add();
                    oRecipientCollection.Item(0).SendInternal = BoYesNoEnum.tYES; // send internal message
                    oRecipientCollection.Item(0).UserCode = "manager"; // add existing user name
                                                                       //oRecipientCollection.Add();
                                                                       //oRecipientCollection.Item(1).SendInternal = BoYesNoEnum.tYES; // send internal message
                                                                       //oRecipientCollection.Item(1).UserCode = "ventas"; // add existing user name

                    // agregamos nuestro listado de documento o documentos que se mostrara en el mensaje/alertas
                    pMessageDataColumns = oMessage.MessageDataColumns; // get columns data

                    if ((ColumnOneValue.Length > 0) && (ColumnOneObject.Length > 0))
                    {
                        pMessageDataColumn = pMessageDataColumns.Add(); // get column
                        pMessageDataColumn.ColumnName = ColumnOneName; // set column name
                        pMessageDataColumn.Link = BoYesNoEnum.tYES; // set link to a real object in the application
                                                                    // agregamos las partidas

                        oLines = pMessageDataColumn.MessageDataLines; // get lines
                        oLine = oLines.Add(); // add new line
                        oLine.Value = ColumnOneValue; // set the line value
                        oLine.Object = ColumnOneObject; // set the link to object Document 
                        oLine.ObjectKey = ColumnOneObjectKey; // set the Document code (DocEntry)

                        pMessageDataColumn = pMessageDataColumns.Add(); // get column
                        pMessageDataColumn.ColumnName = ColumnTwoName; // set column name
                        pMessageDataColumn.Link = BoYesNoEnum.tYES; // set link to a real object in the application
                                                                    // agregamos las partidas
                        oLines = pMessageDataColumn.MessageDataLines; // get lines
                        oLine = oLines.Add(); // add new line
                        oLine.Value = ColumnTwoValue; // set the line value
                        oLine.Object = ColumnTwoObject; // set the link to object Document 
                        oLine.ObjectKey = ColumnTwoObjectKey; // set the Document code (DocEntry)
                    }
                    else
                    {

                        pMessageDataColumn = pMessageDataColumns.Add(); // get column
                        pMessageDataColumn.ColumnName = ColumnOneName; // set column name
                        pMessageDataColumn.Link = BoYesNoEnum.tYES; // set link to a real object in the application
                                                                    // agregamos las partidas

                        oLines = pMessageDataColumn.MessageDataLines; // get lines
                        oLine = oLines.Add(); // add new line
                        oLine.Value = ColumnOneValue; // set the line value
                        oLine.Object = ColumnOneObject; // set the link to object Document 
                        oLine.ObjectKey = ColumnOneObjectKey; // set the Document code (DocEntry)

                        pMessageDataColumn = pMessageDataColumns.Add(); // get column
                        pMessageDataColumn.ColumnName = ColumnTwoName; // set column name
                        pMessageDataColumn.Link = BoYesNoEnum.tYES; // set link to a real object in the application
                                                                    // agregamos las partidas
                        oLines = pMessageDataColumn.MessageDataLines; // get lines
                        oLine = oLines.Add(); // add new line
                        oLine.Value = ColumnTwoValue; // set the line value
                        oLine.Object = ColumnTwoObject; // set the link to object Document 
                        oLine.ObjectKey = ColumnTwoObjectKey; // set the Document code (DocEntry)
                    }
                    // send the message
                    //oMessage.Add();
                    oMessageService.SendMessage(oMessage);
                    //resul = true;
                    MarshallObject(oMessage);
                    GC.Collect();
                    //return resul;
                }
            }
            catch (Exception ex)
            {
                //_message_error += "warning: " + ex.Message;
                // return false;
            }

        }
        public void SendSAPMessage(string subject, string message)
        {

            // SAPbobsCOM.Messages oMsg = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages);
            try
            {

                //Connect_To_SAP connect = new Connect_To_SAP();
                //oCompany = connect.ConnectSAPDB();
                //oMsg.Subject = subject;
                //oMsg.MessageText = message;
                //oMsg.Recipients.SetCurrentLine(0);
                //oMsg.Recipients.UserCode = "manager";
                //oMsg.Recipients.NameTo = "manager";
                //oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;
                //oMsg.Recipients.SendEmail = SAPbobsCOM.BoYesNoEnum.tNO;
                //oMsg.Priority = SAPbobsCOM.BoMsgPriorities.pr_High;


                //if (oMsg.Add() != 0)
                //{
                //   // Console.WriteLine("failed");
                //}
                //else
                //{
                //    //Console.WriteLine("added");
                //}
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // MarshallObject(oMsg);
            }
        }



        public int Check_If_Customer_Exists(string DBName, string CardCode, string SAPUserName, string SAPPassword)
        {

            {
                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                int results = 0;
                Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //  string querystring = "SELECT   FatherCard FROM OCRD WHERE FatherCard = '" + FatherCard + "' and  CardCode ='"+ CardCode +"'";

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "SELECT  COUNT(\"CardCode\") FROM \"" + DBName + "\" + \".OCRD\"  WHERE  \"CardCode\" ='" + Sanitize(CardCode) + "'";
                }
                else
                {
                    querystring = "SELECT  COUNT(CardCode)FROM  " + Sanitize(DBName) + ".[dbo]." + "OCRD (nolock) WHERE  CardCode ='" + Sanitize(CardCode) + "'";
                }
                // string querystring = 
                QueryObject.DoQuery(querystring);
                results = Convert.ToInt32(QueryObject.Fields.Item(0).Value);
                return results;
            }
        }


        public string GetPaymentDocEntry(string DBName, string CardCode, string CardType, string PaymentType, double DocToTal, string SAPUserName, string SAPPassword)
        {

            {
                string PaymentTable = "";
                if (CardType == "C")
                {
                    PaymentTable = "ORCT";
                }
                else
                {
                    PaymentTable = "OVPM";

                }
                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                string results = "";
                Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //  string querystring = "SELECT   FatherCard FROM OCRD WHERE FatherCard = '" + FatherCard + "' and  CardCode ='"+ CardCode +"'";

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "SELECT  Top 1 \"DocEntry\" FROM \"" + DBName + "\" + \"." + PaymentTable + "\"  WHERE  \"CardCode\" ='" + Sanitize(CardCode) + "'  and  DocToTal ='" + DocToTal + "' order by  DocEntry desc";
                }
                else
                {
                    querystring = "SELECT Top 1 DocEntry FROM  " + Sanitize(DBName) + ".[dbo]." + PaymentTable + "(nolock) WHERE  CardCode ='" + Sanitize(CardCode) + "'  and  DocToTal ='" + DocToTal + "' order by  DocEntry desc";
                }
                // string querystring = 
                QueryObject.DoQuery(querystring);
                results = QueryObject.Fields.Item(0).Value.ToString();
                //if (string.IsNullOrEmpty(results))
                //{
                //    results = PaymentType;
                //}
                return results;
            }
        }

        public string Check_If_Item_BatchManaged(string DBName, string ItemCode, string SAPUserName, string SAPPassword)
        {

            {
                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                string results = "N";
                Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //  string querystring = "SELECT   FatherCard FROM OCRD WHERE FatherCard = '" + FatherCard + "' and  CardCode ='"+ CardCode +"'";

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "SELECT \"ManBtchNum\" FROM \"" + DBName + "\" + \".OITM\"  WHERE  \"ItemCode\" ='" + Sanitize(ItemCode) + "'";
                }
                else
                {
                    querystring = "SELECT  ManBtchNum FROM  " + Sanitize(DBName) + ".[dbo]." + "OITM (nolock) WHERE  ItemCode ='" + Sanitize(ItemCode) + "' ";
                }
                // string querystring = 
                // QueryObject.DoQuery(querystring);
                // results = QueryObject.Fields.Item(0).Value;
                return results;
            }
        }

        public string LocalCurrency(string DBName, string SAPUserName, string SAPPassword)
        {

            {
                Connect_To_SAP connect = new Connect_To_SAP();
                oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
                string results = "N";
                Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //  string querystring = "SELECT   FatherCard FROM OCRD WHERE FatherCard = '" + FatherCard + "' and  CardCode ='"+ CardCode +"'";

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "SELECT \"MainCurncy\" FROM \"" + DBName + "\" + \".OADM\" ";
                }
                else
                {
                    querystring = "SELECT  MainCurncy FROM  " + Sanitize(DBName) + ".[dbo]." + "OADM (nolock) ";
                }
                // string querystring = 
                QueryObject.DoQuery(querystring);
                results = "";
                // QueryObject.Fields.Item(0).Value;
                return results;
            }
        }
        public void Create_Customer(string CardCode, string CardName)
        {
            try
            {
                SAPbobsCOM.BusinessPartners sboBP = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                sboBP.CardCode = CardCode;
                sboBP.CardName = CardName;
                sboBP.CardType = BoCardTypes.cCustomer;



                if (sboBP.Add() != 0)
                {

                    oCompany.GetLastError(out nErr, out erMsg);
                    //SAP_SEND_MESSAGE("Customer Added", "Customer Added from API", "Customer Name", CardCode, "2", CardCode, "", "", "", "");
                    //SendSAPMessage("Customer Added", "A Customer with  code " + CardCode + " and  name  " + CardName + " was created by API kindly update Customer Group and Payment Terms Accordingly ");

                }
                else
                {
                    SAP_SEND_MESSAGE("Customer Added", "Customer Added from API Kindly update Customer Group and other relevant fields ", "Customer Name", CardCode, "2", CardCode, "", "", "", "");
                    //SendSAPMessage("Customer Added", "A Customer with  code " + CardCode + " and  name  " + CardName + " was created by API kindly update Customer Group and Payment Terms Accordingly ");
                    // MarshallObject(sboBP);
                }
                MarshallObject(sboBP);

            }
            catch (Exception ex)
            {


            }


        }
        private bool CheckIfExists(string DBName, string CardCode, string CardType)

        {
            bool output = false;

            int results = 0;
            //if (CardType == "CUSTOMER")
            //{
            //    CardType = "C";
            //}
            //else {

            //    CardType = "S";

            //}
            Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            // string querystring = "SELECT  Count(CardCode)  as CardCode_Count  FROM OCRD (nolock)  WHERE CardCode = '" + Sanitize(CardCode) + "'   and  CardType ='" + Sanitize(CardType) + "'";
            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "SELECT  Count(\"CardCode\")  AS \"CardCode_Count\"   \"" + Sanitize(DBName) + "\" + \".OCRD\"  WHERE \"CardCode\" = '" + Sanitize(CardCode) + "'   and  \"CardType\" ='" + Sanitize(CardType) + "'";

            }
            else
            {
                querystring = "SELECT  Count(CardCode)FROM  " + Sanitize(DBName) + ".[dbo]." + "OCRD (nolock) WHERE  CardCode ='" + Sanitize(CardCode) + "'";
            }

            QueryObject.DoQuery(querystring);
            results = Convert.ToInt32(QueryObject.Fields.Item(0).Value.ToString());

            if (results == 0)
            {
                output = false;
            }
            else
            {
                output = true;
            }

            return output;
        }


        //  [Authorize(Roles = "SuperAdmin, Admin, User")]
        //[HttpPost]
        [HttpGet]
        [Route("api/SAP/{DBName}/GetBusinessPartnerByCode/{CardCode}")]
        public IActionResult GetBusinessPartnerByCode(string DBName, string CardCode)
        {
            try
            {
                List<BusinessPartner_Master> customers = new List<BusinessPartner_Master>();
                //SqlParameter[] myCardCode = new SqlParameter[1];
                //myCardCode[0] = new SqlParameter("@CardCode", CardCode);

                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = "select top 1 T0.\"CardCode\", T0.\"CardName\", T0.\"Balance\", T0.\"Currency\"  FROM \"" + Sanitize(DBName) + "\" + \".OCRD\" t0  where  T0.\"CardCode\"='" + Sanitize(CardCode) + "'";

                }
                else
                {
                    querystring = "select top 1 T0.CardCode, T0.CardName, T0.CardType ,T0.Balance, T0.Currency ,T0.GroupCode, T0.Phone1, T0.Phone2, T0.Cellular, T0.VatIdUnCmp, T0.E_Mail, T0.Fax ,T0.frozenFor from  " + Sanitize(DBName) + ".[dbo]." + "ocrd t0 (nolock) where  T0.CardCode='" + Sanitize(CardCode) + "'";
                }

                DataTable dt = GetData(DBName, querystring);
                BusinessPartner_Master customer = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    customer = new BusinessPartner_Master
                    {

                        CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                        ,
                        CardName = Convert.ToString(dt.Rows[i]["CardName"])
                        ,
                        CardType = Convert.ToString(dt.Rows[i]["CardType"])
                        ,
                        Balance = Convert.ToDouble(dt.Rows[i]["Balance"])
                        ,
                        Currency = Convert.ToString(dt.Rows[i]["Currency"])
                        ,
                        GroupCode = Convert.ToString(dt.Rows[i]["GroupCode"])
                        ,
                        Telephone1 = Convert.ToString(dt.Rows[i]["Phone1"])
                        ,
                        Telephone2 = Convert.ToString(dt.Rows[i]["Phone2"])
                        ,
                        MobilePhone = Convert.ToString(dt.Rows[i]["Cellular"])
                       ,
                        Pin = Convert.ToString(dt.Rows[i]["VatIdUnCmp"])
                        ,
                        Email = Convert.ToString(dt.Rows[i]["E_Mail"])
                        ,
                        Fax = Convert.ToString(dt.Rows[i]["Fax"])
                        ,
                        FrozenFor = Convert.ToString(dt.Rows[i]["frozenFor"])
                        ,

                        Bill_To_Address = GetContact(DBName, Convert.ToString(dt.Rows[i]["CardCode"]))
                    };
                    break;
                }
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {

                HttpResponseMessage exeption_response = null;
                exeption_response.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
                return Problem(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/SAP/{DBName}/GetBusinessPartnerByCode1/{CardCode}")]
        public IActionResult GetBusinessPartnerByCode1(string DBName, string CardCode)
        {
            SAPbobsCOM.BusinessPartners oBP;
            //string forward_slash = "\"";
            string ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;

            ApplicationPath = ApplicationPath + @"\" + CardCode + ".xml";
            //"+ ".xml";
            Connect_To_SAP connect = new Connect_To_SAP();

            oCompany = connect.ConnectSAPDB(DBName, userName, section["Password"]);
            oBP = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

            oBP.GetByKey(CardCode);
            if (System.IO.File.Exists(ApplicationPath))
            {
                System.IO.File.Delete(ApplicationPath);
                oBP.SaveXML(ApplicationPath);
            }
            else
            {
                oBP.SaveXML(ApplicationPath);
            }


            XmlDocument doc = new XmlDocument();
            doc.Load(ApplicationPath);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            jsonText.Replace(@"\", "");
            return Ok(jsonText);
        }

        [HttpGet]
        [Route("api/SAP/{DBName}/GetInvoiceByDocEntry/{DocEntry}")]
        public IActionResult GetInvoiceByDocEntry(string DBName, int DocEntry)
        {

            // string forward_slash = "\"";
            string ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;
            //System.IO.Directory.GetParent(System.Windows.Forms.Application.StartupPath).ToString() ;
            ApplicationPath = ApplicationPath + @"\" + DocEntry + ".xml";
            //"+ ".xml";
            Connect_To_SAP connect = new Connect_To_SAP();
            oCompany = connect.ConnectSAPDB(DBName, userName, section["Password"]);
            SAPbobsCOM.Documents oDoc = null;


            oDoc = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
            oDoc.GetByKey(DocEntry);
            if (System.IO.File.Exists(ApplicationPath))
            {
                System.IO.File.Delete(ApplicationPath);
                oDoc.SaveXML(ApplicationPath);
            }
            else
            {
                oDoc.SaveXML(ApplicationPath);
            }


            XmlDocument doc = new XmlDocument();
            doc.Load(ApplicationPath);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            // jsonText = jsonText
            return Ok(jsonText);
        }

        public List<BillToAddress> GetContact(string DBName, string customerId)
        {

            List<BillToAddress> billtoaddress = new List<BillToAddress>();

            if (DbServerType == "SAPHANA")
            {
                //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                querystring = "select  T1.\"CardCode\", T1.\"Name\", T1.\"Title\", T1.\"Position\", T1.\"Address\", T1.\"Tel1\", T1.\"Cellolar\", T1.\"E_MailL\", T1.\"Active\" from  \"" + Sanitize(DBName) + "\" + \".OCPR\" T1 INNER JOIN" +
                           "  \"" + Sanitize(DBName) + "\" + \".OCRD\"  T0  ON  T0.\"CardCode\" =T1.\"CardCode\"  Where T0.\"CardCode\" ='" + (customerId) + "'";
            }
            else
            {
                querystring = "select  ISNULL(T0.CardCode,'')'CardCode', ISNULL(T1.Name,'')'Name', ISNULL(T1.Title,'') 'Title'," +
                   " ISNULL(T1.Position,'') 'Position', ISNULL(T1.Address,'') 'Address' ,ISNULL( T1.Tel1,'') 'Tel1',ISNULL( T1.Cellolar,'') 'Cellolar'," +
                    " ISNULL(T1.E_MailL,'') 'E_MailL',ISNULL( T1.Active,'') 'Active' from  " + Sanitize(DBName) + ".[dbo]." + "OCPR T1(nolock) " +
                   " INNER JOIN " + Sanitize(DBName) + ".[dbo]." + "OCRD  T0  ON  T0.CardCode =T1.CardCode  Where T0.CardCode ='" + (customerId) + "'";
            }
            if (customerId.Length > 1)
            {
                //querystring= querystring
                DataTable dt = GetData(DBName, querystring);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    billtoaddress.Add(new BillToAddress
                    {
                        CardCode = Convert.ToString(dt.Rows[i]["CardCode"])
                        ,
                        Name = Convert.ToString(dt.Rows[i]["Name"])
                        ,
                        Title = Convert.ToString(dt.Rows[i]["Title"])
                        ,
                        Position = Convert.ToString(dt.Rows[i]["Position"])
                        ,
                        Address = Convert.ToString(dt.Rows[i]["Address"])
                        ,
                        Tel1 = Convert.ToString(dt.Rows[i]["Tel1"])
                          ,
                        Cellolar = Convert.ToString(dt.Rows[i]["Cellolar"])
                        ,
                        E_MailL = Convert.ToString(dt.Rows[i]["E_MailL"])
                        ,
                        Active = Convert.ToString(dt.Rows[i]["Active"])
                    });
                }
            }
            return billtoaddress;
        }

        private DataTable GetData(string DBName, string query)
        {

            DataTable dt = null;
            try
            {


                string conString = Configuration.GetConnectionString("constr") + @"Database=" + DBName.Trim() + ";";
                if (!string.IsNullOrEmpty(query))
                {
                    query = query;
                }
                else
                {
                    query = "";
                }

                //SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //oRecordSet.DoQuery(query);

                SqlCommand cmd = new SqlCommand(query);
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 4000000;
                        sda.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);


                        }
                    }
                }

            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        public void Insert_WithholdingTax(string DBname, string CardCode)
        {

            try
            {
                string LogInstanc = "1";
                if (DbServerType == "SAPHANA")
                {
                    //querystring = "select case WHEN T1.\"OnHand\"  >= '" + Sanitize(Quantity) + "' THEN  'Y' ELSE 'N' END AS \"QuantityOk\" from \"OITM\" T0   INNER JOIN \"OITW\"  T1  ON T0.\"ItemCode\" = T1.\"ItemCode\" INNER JOIN \"OWHS\" T2 ON T2.\"WhsCode\" = T1.\"WhsCode\" WHERE  T0.\"ItemCode\" = '" + Sanitize(ItemCode) + "' and T2.\"WhsCode\" = '" + Sanitize(WarehouseCode) + "'";
                    querystring = querystring = "select  T0.\"CardCode\", T0.\"CardName\", T0.\"Balance\", T0.\"CreditLine\", T0.\"DebtLine\", T0.\"Currency\" from  \"" + Sanitize(DBName) + "\" + \".OCRD\" T0  WHERE  T0.\"frozenFor\"='N'";

                }
                else
                {
                    querystring = "select  T0.WTCode  from  " + Sanitize(DBName) + ".[dbo]." + "OWHT T0 (nolock) WHERE  Inactive='N' ";
                }

                DataTable dt = GetData(DBName, querystring);
                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    {

                        string WTCode = Convert.ToString(dt.Rows[i]["WTCode"]);
                        Recordset QueryObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                        string querystring = "Insert  into  CRD4  (CardCode, WTCode ,LogInstanc )values ('" + CardCode + "', '" + WTCode + "', '" + LogInstanc + "')";
                        QueryObject.DoQuery(querystring);
                        MarshallObject(QueryObject);
                    }
                }



            }
            catch { }
        }
        public string Sanitize_Errors(string value)
        {
            List<string> words = new List<string> { "--", ";", ",", @"\\", @"//", "[", "]", "\"", ":" };
            // .Replace("[", "").Replace("\\", "").Replace("//", "").Replace("]", "").Replace(",", "").Replace(";", "");


            foreach (string _mystring in words)
            {
                if (value.Contains(_mystring))
                {
                    value = value.Replace(_mystring, "");
                }
            }
            return value;
        }

        public string Sanitize(string value)
        {
            List<string> words = new List<string> { "--", ";", "drop", "truncate", "create", "call", "alter", "exec", "execute", "," };
            // .Replace("[", "").Replace("\\", "").Replace("//", "").Replace("]", "").Replace(",", "").Replace(";", "");


            foreach (string _mystring in words)
            {
                if (value.Contains(_mystring))
                {
                    value = value.ToLower().Replace(_mystring, "");
                }
            }
            return value;
        }




        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            string dbqt = @"""";
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)

                    {
                        bool is_Numeric = IsNumeric(table.Rows[i][j].ToString());
                        if (j < table.Columns.Count - 1)
                        {

                            if (is_Numeric == true)
                            {

                                JSONString.Append("" + dbqt + table.Columns[j].ColumnName.ToString() + dbqt + ":" + "" + table.Rows[i][j].ToString().Replace(dbqt, "").Replace("'", "").Trim() + ",");
                            }
                            else
                            {
                                JSONString.Append("" + dbqt + table.Columns[j].ColumnName.ToString() + dbqt + ":" + "" + dbqt + table.Rows[i][j].ToString().Replace(dbqt, "").Replace("'", "").Trim() + dbqt + ",");

                            }

                            // JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            if (is_Numeric == true)
                            {

                                JSONString.Append("" + dbqt + table.Columns[j].ColumnName.ToString() + dbqt + ":" + "" + table.Rows[i][j].ToString().Replace(dbqt, "").Replace("'", "").Trim() + "");
                            }
                            else
                            {
                                JSONString.Append("" + dbqt + table.Columns[j].ColumnName.ToString() + dbqt + ":" + "" + dbqt + table.Rows[i][j].ToString().Replace(dbqt, "").Replace("'", "").Trim() + dbqt + "");

                            }
                            // JSONString.Append("\"" + dbqt+ table.Columns[j].ColumnName.ToString() + dbqt + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        private bool IsNumeric(string str)
        {
            float f;
            return float.TryParse(str, out f);
        }



    }
}
