using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS_project.Models
{
    public class Application
    {
        public int Application_ID { get; set; }
        public string Application_Name { get; set; }
        public string Owner { get; set; }
        public int Deleted { get; set; }
    }
}
