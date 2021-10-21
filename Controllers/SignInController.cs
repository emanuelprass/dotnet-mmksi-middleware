using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Mvc;
using mmksi_middleware.Config;
using mmksi_middleware.Transport;

namespace mmksi_middleware.Controllers
{
    [ApiController]
    [Route("aws/signin")]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> SignIn(UserRequest user, [FromHeader] string company)
        {
            var aws = new AwsCognito
            {
                PoolId = Environment.GetEnvironmentVariable("ASPNETCORE_AWSPOOLID"),
                ClientId = Environment.GetEnvironmentVariable("ASPNETCORE_AWSCLIENTID"),
                ClientSecret = Environment.GetEnvironmentVariable("ASPNETCORE_AWSCLIENTSECRET"),
                Region = RegionEndpoint.USEast2
            };

            ResponseBadRequest resp = new();

            if (company is null) {    
                resp.StatusCode = 400;
                resp.Message = "Company can't be null";
                return BadRequest(resp);
            } else if (aws.PoolId is null ||  aws.ClientId is null || aws.ClientSecret is null){
                resp.StatusCode = 409;
                resp.Message = "Missing environment variable";
                return Conflict(resp);
            }

            var cognito = new AmazonCognitoIdentityProviderClient(aws.Region);

            byte[] keyByte = new ASCIIEncoding().GetBytes(aws.ClientSecret);
            byte[] messageBytes = new ASCIIEncoding().GetBytes(user.Username + aws.ClientId);
            byte[] hashmessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);
            var hash = Convert.ToBase64String(hashmessage);

            var request = new AdminInitiateAuthRequest
            {
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                UserPoolId = aws.PoolId,
                ClientId = aws.ClientId,
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