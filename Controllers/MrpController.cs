using System.Net;
using System.Web;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using mmksi_middleware.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using mmksi_middleware.Transport;

namespace mmksi_middleware.Controllers
{
    [ApiController]
    [Route("mrp/vehicle")]
    public class VehicleController : ControllerBase
    {
        public Vehicle vehicle;
        
        [Authorize]
        [HttpGet]

        public async Task<IActionResult> GetVehicle(string  brand, string model, [FromHeader] string Authorization)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("ApiKey", Environment.GetEnvironmentVariable("ASPNETCORE_MRPAPIKEY"));
            var url = Environment.GetEnvironmentVariable("ASPNETCORE_MRPBASEURL");
            
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["brand"] = brand;
            query["model"] = model;
            uriBuilder.Query = query.ToString();

            ResponseBadRequest resp = new();
            if (Authorization is null) {
                resp.StatusCode = 400;
                resp.Message = "Authorization can't be empty";
                return BadRequest(resp);
            }

            try {
                var response = await httpClient.GetAsync(uriBuilder.ToString()).ConfigureAwait(false);

                string apiResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                vehicle = JsonConvert.DeserializeObject<Vehicle>(apiResponse);
                return Ok(vehicle);
            }
            catch {
                resp.StatusCode = 400;
                resp.Message = "Error from DSF, please try again later.";
                return BadRequest(resp);
            }
        }
    }
}