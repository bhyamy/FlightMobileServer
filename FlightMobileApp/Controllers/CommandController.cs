using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Reflection;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightMobileApp.Controllers
{
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IClient _client;
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly string _ip;
        private readonly int _screenPort;
        


        public CommandController(IClient client, IConfiguration configuration)
        {
            _client = client;
            _ip = configuration.GetValue<string>("Logging:ServerInfo:ip");
            _screenPort = configuration.GetValue<int>("Logging:ServerInfo:screenshot_port");
        }

        // // GET api/<CommandController>
        // [Route("api/[controller]")]
        // [HttpGet]
        // public string Get()
        // {
        //     return "Hello World!";
        // }

        [Route("api/[controller]")]
        // POST api/<CommandController>
        [HttpPost]
        public ActionResult<dynamic> Post([FromBody] Command cmd)
        {
            Task<Result> task = _client.Execute(cmd);
            if (task.Result == Result.Ok)
                return Ok();
            return BadRequest();
        }

        // GET screenshot/
        [Route("screenshot")]
        [HttpGet]
        public async Task<IActionResult> GetScreenshot()
        {
            try
            {
                string address = "http://" + _ip + ":" + _screenPort + "/screenshot";
                HttpResponseMessage res = await HttpClient.GetAsync(address);
                byte[] data = await res.Content.ReadAsByteArrayAsync();
                return File(data, "image/png");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
