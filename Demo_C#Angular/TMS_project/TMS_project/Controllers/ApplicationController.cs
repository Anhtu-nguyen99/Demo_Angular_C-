using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS_project.Business;
using TMS_project.Models;

namespace TMS_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        public ApplicationController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _env = environment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            ApplicationBusiness AppB = new(_configuration, _env);
            List<Application> app_List = AppB.getApplicationList();

            return new JsonResult(app_List);
        }

        [Route("Delete")]
        public JsonResult Delete(Application app)
        {
            ApplicationBusiness AppB = new(_configuration, _env);

            return AppB.Delete(app);
        }

        [HttpPost]
        public JsonResult Post(Application app)
        {
            ApplicationBusiness AppB = new(_configuration, _env);

            return AppB.Post(app);
        }

        
        [HttpPut]
        public JsonResult Update(Application app)
        {
            ApplicationBusiness AppB = new(_configuration, _env);

            return AppB.UpdateApp(app);
        }

    }
}
