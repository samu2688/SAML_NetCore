using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMLWebApp.Helper;
using System;


/*
 This version use AspNetSaml library code
 */
namespace SAMLWebApp.Controllers
{
    [AllowAnonymous]
    [Route("Samlv2")]
    public class Samlv2Controller : Controller
    {
        [Route("Samlv2Login")]
        //this example is an ASP.NET MVC action method
        public ActionResult Login()
        {
            //TODO: specify the SAML provider url here, aka "Endpoint"
            var samlEndpoint = "https://saml-idp-dev-jh1jd6g5.eu.auth0.com/samlp/x5H4CTynyk6RWkOjbYkxaE9hUzDVuskB";

            var request = new Samlv2AuthRequest(
                "urn:auth0:YOUR_TENANT:YOUR_CONNECTION_NAME", //TODO: put your app's "entity ID" here
                "https://localhost:44387/Samlv2/Samlv2Consumer" //TODO: put Assertion Consumer URL (where the provider should redirect users after authenticating)
                );

            //redirect the user to the SAML provider
            return Redirect(request.GetRedirectUrl(samlEndpoint, "https://www.corriere.it/"));
        }

        [Route("Samlv2Consumer")]
        //ASP.NET MVC action method... But you can easily modify the code for Web-forms etc.
        public ActionResult Samlv2Consumer()
        {
            // 1. TODO: specify the certificate that your SAML provider gave you
            string samlCertificate = @"-----BEGIN CERTIFICATE-----
MIIDHzCCAgegAwIBAgIJVfScYrlwOHbyMA0GCSqGSIb3DQEBCwUAMC0xKzApBgNVBAMTInNhbWwtaWRwLWRldi1qaDFqZDZnNS5ldS5hdXRoMC5jb20wHhcNMjIwODI5MTQ0MDIxWhcNMzYwNTA3MTQ0MDIxWjAtMSswKQYDVQQDEyJzYW1sLWlkcC1kZXYtamgxamQ2ZzUuZXUuYXV0aDAuY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5EOiIkaCH/cWD1JJhL1/QfVkfWXbHSDauL+Hr9RXFHgTEOsvURb3sK+ScA5owerihgL/QQ61Rx4CWU0BokYi69AsSexdRmuDonxUIWJ8csFRQ4FVOqOZUU5CYz1bghzkx/8MQqYy+TMo44tPfVLRg2c66jpyPDdgZAh/0nxdPT3ef+NkzGhaHAKAG5hchcHM6JEKHnm9iW/waIVJhrrN4krjrwiZbKEvsLnFgdpvBZO4vWAI7IoVOACFHyzYnNbBKoDBLBUlCdX+l0hvXA1/GeVY5MyGRR/Gy7nwQZPSTquqpWbPejR0F3qdkKtbnjMz6wvTHJ+Yq+U8Zfv7bzxjSQIDAQABo0IwQDAPBgNVHRMBAf8EBTADAQH/MB0GA1UdDgQWBBQ9mCQvGzGwNrzlWC1E6VqdGsPyxTAOBgNVHQ8BAf8EBAMCAoQwDQYJKoZIhvcNAQELBQADggEBAB5XEXp13nVLmIUEO3YC2Vc+O2UYN1J/oCnJa6pJ02+bq7njcSNqSE8V1YLff1lJCtzs8p32WBYQ1zDOt0WHXUHtZ1/z/so035IW/lbajwKhVbY8wLrJ/oncHB1ziiE9fpPBFdjgp8+nc6FZPEKdZ3uKxgc2PKYZbZOv0OLVY4mSZ3lyn4fByhwrD9LNAN3aT8KqeiZ1PhCuCnM7IsgK4eSdHTwmqBimdiuL4fJNdmNifB/OCZPNCL3mUL64UpeJcQ5BQdmQEyZwE9IbLA8Uc+wdDRvdjGg7l3REcTKRT8JW2rf9jYsFcMk5p5/Px5MUd4j2gQ/kIYYhswOxvthoNHE=
-----END CERTIFICATE-----";

            // 2. Let's read the data - SAML providers usually POST it into the "SAMLResponse" var
            var samlResponse = new Samlv2Response(samlCertificate, Request.Form["SAMLResponse"]);

            //for log purpose
            var samlTextResponse = samlResponse.GetSamlXmlResponse();

            // 3. We're done!
            if (samlResponse.IsValid())
            {
                //WOOHOO!!! user is logged in

                //Some more optional stuff for you
                //let's extract username/firstname etc
                string username, email, firstname, lastname;
                try
                {
                    username = samlResponse.GetNameID();
                    email = samlResponse.GetEmail();
                    firstname = samlResponse.GetFirstName();
                    lastname = samlResponse.GetLastName();
                }
                catch (Exception ex)
                {
                    //insert error handling code
                    //no, really, please do
                    return null;
                }

                //user has been authenticated, put your code here, like set a cookie or something...
                //or call FormsAuthentication.SetAuthCookie()
                //or call context.SignInAsync() in ASP.NET Core
                //or do something else
            }

            return null;
        }
    }
}