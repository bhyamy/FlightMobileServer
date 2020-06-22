using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IClient _client;

        CommandController(IClient client)
        {
            _client = client;
            _client.Start();//todo check where i should start
        }

        // GET api/<CommandController>
        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }

        // POST api/<CommandController>
        [HttpPost]
        public void Post([FromBody] Command cmd)
        {
            Task<Result> task = _client.Execute(cmd);
        }

        // GET screenshot/
        [Route("screenshot"), HttpGet]
        public void GetScreenshot()
        {
            //todo get screen shot from simulator
        }
    }
}
