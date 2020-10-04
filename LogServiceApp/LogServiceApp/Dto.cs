using System.Collections.Generic;
using DB.Models;

namespace LogServiceApp
{
    internal class Dto
    {
        public string Writing { get; set; }
        public List<Log> Logs { get; set; }
    }
}