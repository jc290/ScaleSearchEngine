using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.Intrinsics.Arm;

namespace LoadBalancer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private LoadBalancer _loadBalancer;

        public SearchController(LoadBalancer loadBalancer) { _loadBalancer = loadBalancer; }

        [HttpGet]
        [HttpPost]
        [Route("/Search")]
        public async Task<ActionResult> Search()
        {

            HttpClient httpClient = new HttpClient();
            var service = _loadBalancer.NextService();
            if (service == "")
            {
                return StatusCode(500);
            }
            Console.WriteLine("Connecting to host: " + service);

            httpClient.BaseAddress = new Uri(service);
            HttpResponseMessage resp;
            try
            {

                if (Request.Method == "GET")
                {
                    Console.WriteLine("Sending GET request to host: " + service + " with path " + Request.Path + Request.QueryString);
                    resp = await httpClient.GetAsync(Request.Path + Request.QueryString);

                }
                else if (Request.Method == "POST")
                {
                    Console.WriteLine("Sending POST request to host: " + service + " with path " + Request.Path + Request.QueryString);
                    resp = await httpClient.PostAsync(Request.Path + Request.QueryString, new StringContent(""));
                    if (!resp.IsSuccessStatusCode)
                    {
                        if (resp.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            _loadBalancer.RemoveService(service);
                            return StatusCode(500);
                        }
                    }
                }
                else
                {
                    return StatusCode(500);
                }

                Console.WriteLine("Host " + service + " returned status " + resp.StatusCode);

                var response = await resp.Content.ReadAsStringAsync();

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to host " + service + " it will be removed from server list");
                _loadBalancer.RemoveService(service);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [HttpPost]
        [Route("/Strategy")]
        public ActionResult Strategy(string type)
        {
            if(type == "roundrobin")
            {
                _loadBalancer = new LoadBalancer();
                _loadBalancer.SetActiveStrategy(new LoadBalancerRoundRobinStrategy());
            }
            return new OkObjectResult("");
        }
    }

    
}
