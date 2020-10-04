using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB
{
    public class A
    {
        public string S { get; set; }
        public A()
        {
            using (var db = new LogServiceContext())
            {
                S = db.LogsTable.ToList().ToString();
            }
        }
    }
}
