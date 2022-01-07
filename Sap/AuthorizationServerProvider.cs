using Microsoft.Owin.Security.OAuth;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenBasedAPI
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserAuthentication OBJ = new UserAuthentication())
            {

                NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
                string TokenUserName = section["TokenUserName"];
               

                var user = OBJ.ValidateUser(context.UserName, context.Password);
                if (user == "false")
                {
                    context.SetError("invalid_grant", "Username or password is incorrect");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, "SuperAdmin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, TokenUserName));
                //identity.AddClaim(new Claim("Email", user.UserEmailID));

                context.Validated(identity);
            }
        }
    }
}