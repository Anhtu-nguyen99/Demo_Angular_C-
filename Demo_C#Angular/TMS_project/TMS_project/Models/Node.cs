using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS_project.Models
{
    public class Node
    {
        public int Node_ID { get; set; }
        public int Node_Parent_ID { get; set; }
        public string Node_Title { get; set; }
        public string Node_Type { get; set; }
        public DateTime Submission_Date { get; set; }
        public string Owner { get; set; }
        public int Deleted { get; set; }
        public int Application_ID { get; set; }
        public List<Node> Node_List { get; set; }
        public string Application_Name { get; set; }
        public string Node_Parents { get; set; }
    }
}
