using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TokenBasedAPI
{
    public class UserAuthentication : IDisposable
    {
        public string ValidateUser(string username, string password)
        {

            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
            string TokenUserName = section["TokenUserName"];
            string TokenPassword = section["TokenPassword"];

            string Name = username == TokenUserName ? "Valid" : "InValid";
            string Pass = password == TokenPassword ? "Valid" : "InValid";

            if (Name == "Valid" && Pass == "Valid")
                return "true";
            else
                return "false";
        }
        public void Dispose()
        {
            //Dispose();
        }
    }
}