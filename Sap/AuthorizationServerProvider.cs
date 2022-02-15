using Microsoft.Owin.Security.OAuth;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using TokenBasedAPI.Controllers;

namespace TokenBasedAPI
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {


        public SAPbobsCOM.Company oCompany = null;
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserAuthentication OBJ = new UserAuthentication())
            {

                string SAPDBName = "";
                SAPDBName = context.Scope[0];
                TestController.AddUpdateAppSettings("CompanyDB", SAPDBName);
                NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
                string UserName = section["UserName"];
                string Password = section["Password"];
                // SAPDBName = section["CompanyDB"];

                var user = OBJ.ValidateUser(context.UserName, context.Password);
                if (user == "false")
                {
                    context.SetError("invalid_grant", "Username or password is incorrect");
                    return;
                }
                string connected_to_sap = Connected_SAP(SAPDBName, UserName, Password);
                if (connected_to_sap == "true")
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Role, "SuperAdmin"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, UserName));
                    //identity.AddClaim(new Claim("Email", user.UserEmailID));

                    context.Validated(identity);
                }
            }
        }



        public string Connected_SAP(string DBName, string SAPUserName, string SAPPassword)
        {

            int nErr = 0;
            string erMsg = "";
            string connected = "";
            TestController.AddUpdateAppSettings("CompanyDB", DBName);
            TestController.AddUpdateAppSettings("manager", SAPUserName);
            TestController.AddUpdateAppSettings("Password", SAPPassword);

            TestController.Connect_To_SAP connect = new TestController.Connect_To_SAP();
            oCompany = connect.ConnectSAPDB(DBName, SAPUserName, SAPPassword);
            if ((0 == oCompany.Connect()))
            {
                connected = "true";
            }
            //  Interaction.MsgBox("Connected to Licence server successfully");
            else

            {
                oCompany.GetLastError(out nErr, out erMsg);
                if (erMsg.Contains("connected"))
                {
                    connected = "true";
                }
                else
                {
                    connected = "false";
                }


            }

            return connected;
        }
    }

}