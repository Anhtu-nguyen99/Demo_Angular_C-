using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TMS_project.Business;
using TMS_project.Models;

namespace TMS_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        public NodeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _env = environment;
        }

        [Route("searchNode/{Node_Title}")]
        public JsonResult searchNode(string Node_Title)
        {
            NodeBusiness NodB = new(_configuration, _env);
            List<Node> Nod_List = NodB.search_Node(Node_Title);

            return new JsonResult(Nod_List);
        }

        [Route("GetNodeParent/{id}")]
        public JsonResult GetNodeParent(int id)
        {
            NodeBusiness NodB = new(_configuration, _env);
            List<Node> node_List = NodB.GetNodeParent(id);

            return new JsonResult(node_List);
        }

        [Route("GetNodeByApp/{id}")]
        public JsonResult GetNodeByApp(int id)
        {
            NodeBusiness NodB = new(_configuration, _env);
            List<Node> node_List = NodB.GetNodeApp(id);

            return new JsonResult(node_List);
        }

        [Route("OneNode/{id}")]
        public JsonResult GetNodeInfo(int id)
        {
            NodeBusiness NodB = new(_configuration, _env);
            Node nod = NodB.GetNodeInfo(id);

            return new JsonResult(nod);
        }

        [HttpPut]
        public JsonResult Put(Node nod)
        {
            NodeBusiness NodB = new(_configuration, _env);

            return NodB.UpdateNode(nod);
        }

        [Route("Delete")]
        public JsonResult Delete(Node nod)
        {
            NodeBusiness NodB = new(_configuration, _env);

            return NodB.Delete(nod);
        }

        [HttpGet]
        public JsonResult GetAllNode()
        {
            NodeBusiness NodB = new(_configuration, _env);

            return new JsonResult(NodB.GetAllNode());
        }

        [HttpPost]
        public JsonResult Post(Node nod)
        {
            NodeBusiness NodB = new(_configuration, _env);

            return NodB.Post(nod);
        }
    }
}
