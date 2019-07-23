using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

public class SkypeOnlineHelper
{
    private HttpClient client;
    public string responseData { get; set; }

    public SkypeOnlineHelper()
    {
        client = new HttpClient();

    }

    public async void getUserInfo(string upn)
    {
        string apiUrl = "https://testneoswitv2v1.azurewebsites.net/api/HttpTriggerPowerShell1";
        client.BaseAddress = new Uri(apiUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Authorization
        //             = new AuthenticationHeaderValue("Bearer", token);
        //var response = await client.GetAsync("users/contact@neoswit.fr");
        var response = await client.GetAsync("?upn=" + upn);
        this.responseData = await response.Content.ReadAsStringAsync();
    }
}
