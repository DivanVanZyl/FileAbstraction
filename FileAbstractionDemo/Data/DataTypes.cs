using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data
{
    internal class Computer
    {
        public Computer()
        {
            Name = Environment.MachineName;
            _description = RuntimeInformation.ProcessArchitecture;
        }
        public string Name { get; }
        private Architecture _description;
    }
}
