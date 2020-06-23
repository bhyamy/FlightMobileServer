using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FlightMobileApp.Models
{
    public interface IClient
    {
        public Task<Result> Execute(Command cmd);
        public void Start();
        public void ProcessCommands();

        
    }
}
