using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

public class GraphAPIs
{
    private static string token = string.Empty;
    private static string read = string.Empty;
    private static string responseData = string.Empty;

    public string GetToken()
    {
        string tmp = token;
        token = string.Empty;
        return tmp;
    }
    public string GetReadItem()
    {
        string tmp = read;
        read = string.Empty;
        return tmp;
    }

    public string GetResponseData()
    {
        string tmp = responseData;
        responseData = string.Empty;
        return tmp;
    }

    public GraphAPIs()
    {
        token = CreateToken();
    }
    public async void SendRequestUser(string user)
    {
        string apiUrl = "https://graph.microsoft.com/beta/";
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(apiUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Authorization
                     = new AuthenticationHeaderValue("Bearer", token);
        //var response = await client.GetAsync("users/contact@neoswit.fr");
        var response = await client.GetAsync("users/" + user + "/memberOf");
        responseData = await response.Content.ReadAsStringAsync();

    }

    #region Get an authentication access token
    public string CreateToken()
    {
        // TODO: Install-Package Microsoft.IdentityModel.Clients.ActiveDirectory -Version 2.21.301221612
        // and add using Microsoft.IdentityModel.Clients.ActiveDirectory

        //The client id that Azure AD created when you registered your client app.
        string clientID = "e74c9182-0dfc-4624-bb4a-42ba60cb969e";

        string TenantId = "44c8e8db-5402-457d-ae2b-61164369dc2b";
        string secret = "H*CbuY.b36L62adRrkvnnf?e+WXeey[[";

        //RedirectUri you used when you register your app.
        //For a client app, a redirect uri gives Azure AD more details on the application that it will authenticate.
        // You can use this redirect uri for your client app
        //string redirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

        //Resource Uri for Graph API
        string resourceUri = "https://graph.microsoft.com";

        //Get access token:
        // To call a Power BI REST operation, create an instance of AuthenticationContext and call AcquireToken
        // AuthenticationContext is part of the Active Directory Authentication Library NuGet package
        // To install the Active Directory Authentication Library NuGet package in Visual Studio,
        //  run "Install-Package Microsoft.IdentityModel.Clients.ActiveDirectory" from the nuget Package Manager Console.

        // AcquireToken will acquire an Azure access token
        // Call AcquireToken to get an Azure token from Azure Active Directory token issuance endpoint
        //string authority = string.Format(CultureInfo.InvariantCulture, AuthEndPoint, TenantId);

        string authority = "https://login.microsoftonline.com/{0}";
        authority = String.Format(authority, TenantId);
        AuthenticationContext authContext = new AuthenticationContext(authority);
        token = authContext.AcquireTokenAsync(resourceUri, new ClientCredential(clientID, secret)).Result.AccessToken;
        //string token = authContext.AcquireTokenAsync(resourceUri, new ClientCredential(clientID,secret)/*, new Uri(redirectUri), new PlatformParameters(PromptBehavior.Auto,)*/).Result.AccessToken;

        /*var app = DeviceCodeProvider.CreateClientApplication(clientID);
        //var authProvider = new DeviceCodeProvider(app);
        //var client = new GraphServiceClient(authContext);*/

        return token;
    }
    #endregion
}


