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
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IClient _client;

        public CommandController(IClient client)
        {
            _client = client;
            _client.Start();//todo check where i should start
        }

        // GET api/<CommandController>
        [Route("api/[controller]")]
        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }

        [Route("api/[controller]")]
        // POST api/<CommandController>
        [HttpPost]
        public void Post([FromBody] Command cmd)
        {
            Task<Result> task = _client.Execute(cmd);
        }

        // GET screenshot/
        [Route("screenshot")]
        [HttpGet]
        public List<byte> GetScreenshot()
        {
            //todo get screen shot from simulator
            Console.Out.WriteLine("in screenshot");
            List<byte> pixels = new List<byte>();
            pixels.Add(2);
            pixels.Add(1);
            return pixels;
        }
    }
}
