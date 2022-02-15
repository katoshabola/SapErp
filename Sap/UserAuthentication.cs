using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using TokenBasedAPI.Controllers;

namespace TokenBasedAPI
{
    public class UserAuthentication : IDisposable
    {

        public string ValidateUser(string username, string password)
        {

            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("appSettings");
            string UserName = section["UserName"];
            string Password = section["Password"];

            string Name = username == UserName ? "Valid" : "InValid";
            string Pass = password == Password ? "Valid" : "InValid";



            if (Name == "Valid" && Pass == "Valid")
                return "true";
            else
                return "false";
        }
        public void Dispose()
        {
            //Dispose();
        }
        //public void SAPDBName(string DBName)
        //{
        //    string test = DBName;
        //    TestController.AddUpdateAppSettings("CompanyDB", DBName);
        //}

    }
}