using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS_project.Business;

namespace TMS_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        public AttributeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _env = environment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            AttributeBusiness AttB = new(_configuration, _env);
            List<Models.Attribute> Attr_List = AttB.getAttributeList();

            return new JsonResult(Attr_List);
        }

        [Route("GetByNode/{id}")]
        public JsonResult Get(int id)
        {
            AttributeBusiness AttB = new(_configuration, _env);
            List<Models.Attribute> Attr_List = AttB.getAttributeList(id);

            return new JsonResult(Attr_List);
        }

        [HttpPost]
        public JsonResult Post(Models.Attribute att)
        {
            AttributeBusiness AttB = new(_configuration, _env);

            return AttB.Post(att);
        }

        [Route("UpdateAttribute")]
        public JsonResult UpdateAttribute(Models.Attribute att)
        {
            AttributeBusiness AttB = new(_configuration, _env);

            return AttB.UpdateAttribute(att);
        }

        [Route("Delete")]
        public JsonResult DeleteAttribute(Models.Attribute att)
        {
            AttributeBusiness AttB = new(_configuration, _env);

            return AttB.Delete(att);
        }
    }
}
