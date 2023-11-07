using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Postex.Payment.Application.Contracts.Jibit.Models
{
    public class GenerateTokenResponseModel
    {
        [Newtonsoft.Json.JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [Newtonsoft.Json.JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
