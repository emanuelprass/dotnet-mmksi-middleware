
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Mvc;
using mmksi_middleware.Transport;

namespace mmksi_middleware.Controllers
{
    [ApiController]
    [Route("aws/signin")]
    public class AuthenticationController : ControllerBase
    {
        private readonly string _poolId = "us-east-2_p74zOrZvg";
        private readonly string _clientId = "469cct1mnf5ja0difbo7sk6fkj";
        private readonly string _clientSecret = "n0i4r6g75g7i4vi4jc8uu011gg681aglgarkd4fmafhu0atln3n";
        private readonly RegionEndpoint _region = RegionEndpoint.USEast2;
        [HttpPost]
        public async Task<ActionResult<string>> SignIn(UserRequest user, [FromHeader] string company)
        {
            if (company is null) {
                ResponseBadRequest resp = new();
                resp.StatusCode = 400;
                resp.Message = "Company can't be null";
                return BadRequest(resp);
            }

            var cognito = new AmazonCognitoIdentityProviderClient(_region);

            byte[] keyByte = new ASCIIEncoding().GetBytes(_clientSecret);
            byte[] messageBytes = new ASCIIEncoding().GetBytes(user.Username + _clientId);
            byte[] hashmessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);
            var hash = Convert.ToBase64String(hashmessage);

            var request = new AdminInitiateAuthRequest
            {
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                UserPoolId = _poolId,
                ClientId = _clientId,
            };

            request.AuthParameters.Add("USERNAME", user.Username);
            request.AuthParameters.Add("PASSWORD", user.Password);
            request.AuthParameters.Add("SECRET_HASH", hash);

            AdminInitiateAuthResponse authResponse = await cognito.AdminInitiateAuthAsync(request).ConfigureAwait(continueOnCapturedContext:false);

            UserResponse token = new();
            token.IdToken = authResponse.AuthenticationResult.IdToken;            
            token.RefreshToken = authResponse.AuthenticationResult.RefreshToken;

            return Ok(token);
        }
    }
}