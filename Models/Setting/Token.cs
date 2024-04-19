namespace GreatEastForex.Models
{
    public class Token
    {        
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }

    public class Api
    {
        public static string baseAddress = "https://api1.artemisuat.cynopsis.co";
        public static string tokenAddress = "https://crm-demo.cynopsis.co/oauth/token";
    }

    public class Resp
    {
        public string status { get; set; }
        public int id { get; set; }
        public string access_token { get; set; }
    }
}