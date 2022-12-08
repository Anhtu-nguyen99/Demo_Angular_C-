using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS_project.Models
{
    public class Attribute
    {
        public int Attribute_ID { get; set; }
        public string Attribute_Name { get; set; }
        public int Node_ID { get; set; }
        public int Deleted { get; set; }
        public string Node_Title { get; set; }
    }
}
