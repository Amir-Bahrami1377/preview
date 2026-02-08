using FormerUrban_Afta.DataAccess.Model.SMS;
using Newtonsoft.Json;

namespace FormerUrban_Afta.DataAccess.Services.Sms;

public class MeliPayamakRestClientAsync
{
    private const string endpoint = "https://rest.payamak-panel.com/api/SendSMS/";
    private const string sendOp = "SendSMS";
    private const string getDeliveryOp = "GetDeliveries2";
    private const string getMessagesOp = "GetMessages";
    private const string getCreditOp = "GetCredit";
    private const string getBasePriceOp = "GetBasePrice";
    private const string getUserNumbersOp = "GetUserNumbers";
    private const string sendByBaseNumberOp = "BaseServiceNumber";
    private string UserName;
    private string Password;

    public MeliPayamakRestClientAsync(string username, string password)
    {
        UserName = username;
        Password = password;
    }

    public MeliPayamakRestClientAsync()
    {
    }

    private async Task<MeliPayamakRestResponse> makeRequestAsync(Dictionary<string, string> values, string op)
    {
        var content = new FormUrlEncodedContent(values);

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsync(endpoint + op, content);
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MeliPayamakRestResponse>(responseString);
        }
    }

    public async Task<MeliPayamakRestResponse> SendAsync(string to, string from, string message, bool isflash)
    {
        var values = new Dictionary<string, string>
            {
                { "username", UserName },
                { "password", Password },
                { "to" , to },
                { "from" , from },
                { "text" , message },
                { "isFlash" , isflash.ToString() }
            };

        return await makeRequestAsync(values, sendOp);
    }

    public async Task<MeliPayamakRestResponse> SendByBaseNumberAsync(string text, string to, int bodyId)
    {
        var values = new Dictionary<string, string>
            {
                { "username", UserName },
                { "password", Password },
                { "text" , text },
                { "to" , to },
                { "bodyId" , bodyId.ToString() }
            };

        return await makeRequestAsync(values, sendByBaseNumberOp);
    }

    public async Task<MeliPayamakRestResponse> GetDeliveryAsync(long recid)
    {
        var values = new Dictionary<string, string>
            {
                { "UserName", UserName },
                { "PassWord", Password },
                { "recID" , recid.ToString() },
            };

        return await makeRequestAsync(values, getDeliveryOp);
    }
    public async Task<MeliPayamakRestResponse> GetMessagesAsync(int location, string from, string index, int count)
    {
        var values = new Dictionary<string, string>
            {
                { "UserName", UserName },
                { "PassWord", Password },
                { "Location" , location.ToString() },
                { "From" , from },
                { "Index" , index },
                { "Count" , count.ToString() }
            };

        return await makeRequestAsync(values, getMessagesOp);
    }
    public async Task<MeliPayamakRestResponse> GetCreditAsync()
    {
        var values = new Dictionary<string, string>
            {
                { "UserName", UserName },
                { "PassWord", Password },
            };

        return await makeRequestAsync(values, getCreditOp);
    }
    public async Task<MeliPayamakRestResponse> GetBasePriceAsync()
    {
        var values = new Dictionary<string, string>
            {
                { "UserName", UserName },
                { "PassWord", Password },
            };

        return await makeRequestAsync(values, getBasePriceOp);
    }
    public async Task<MeliPayamakRestResponse> GetUserNumbersAsync()
    {
        var values = new Dictionary<string, string>
            {
                { "UserName", UserName },
                { "PassWord", Password },
            };

        return await makeRequestAsync(values, getUserNumbersOp);
    }

}
