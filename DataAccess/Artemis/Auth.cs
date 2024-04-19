using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DataAccess.Artemis
{
    public class Auth
    {
        public class GetBody
        {
            public string username { get; set; }
            public string password { get; set; }
            public string clientId { get; set; }
            public string userPoolId { get; set; }
        }

        public class RefreshTokenParam
        {
            public string clientId { get; set; }
            public string refreshToken { get; set; }
        }

        //Return Result
        public class Result
        {
            public AuthenticationResult AuthenticationResult { get; set; }
            public ResponseMetadata ResponseMetadata { get; set; }
        }

        public class AuthenticationResult
        {
            public string AccessToken { get; set; }
            public string ExpiresIn { get; set; }
            public string TokenType { get; set; }
            public string RefreshToken { get; set; }
            public string IdToken { get; set; }
        }

        public class ResponseMetadata
        {
            public string RequestId { get; set; }
            public string HTTPStatusCode { get; set; }
            public HTTPHeaders HTTPHeaders { get; set; }
            public string RetryAttempts { get; set; }

        }

        public class HTTPHeaders
        {
            public string date { get; set; }
            [JsonProperty(PropertyName = "content-type")]
            public string content_type { get; set; }
            [JsonProperty(PropertyName = "content-length")]
            public string content_length { get; set; }
            public string connection { get; set; }
            [JsonProperty(PropertyName = "x-amzn-requestid")]
            public string x_amzn_requestid { get; set; }
        }

        public class Error
        { 
            public string detail { get; set; }
        }
    }
}