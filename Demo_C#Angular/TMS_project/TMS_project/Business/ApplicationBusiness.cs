using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TMS_project.Models;

namespace TMS_project.Business
{
    public class ApplicationBusiness : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(ApplicationBusiness));
        public ApplicationBusiness(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _env = environment;
        }

        private void Map_data(DataRow dr, Application app)
        {
            DataTable dt = dr.Table;

            if (dt.Columns.Contains("Application_ID"))
                app.Application_ID = Convert.ToInt32(dr["Application_ID"]);
            if (dt.Columns.Contains("Application_Name"))
                app.Application_Name = Convert.ToString(dr["Application_Name"]);
            if (dt.Columns.Contains("Owner"))
                app.Owner = Convert.ToString(dr["Owner"]);
            if (dt.Columns.Contains("Deleted"))
                app.Deleted = Convert.ToInt32(dr["Deleted"]);
        }
        public List<Application> getApplicationList()
        {
            List<Application> arr_App = new();
            string query = @"
                    select * from view_Application
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _log4net.Error("Error: " + ex);
            }

            foreach (DataRow dr in table.Rows)
            {
                Application ObjApp = new();
                Map_data(dr, ObjApp);
                arr_App.Add(ObjApp);
            }

            return arr_App;
        }

        public JsonResult Post(Application app)
        {
            string query = @"
                    insert into dbo.Application(Application_Name, Owner, Deleted) 
                    Values 
                    (
                    '" + app.Application_Name + @"',
                    '" + app.Owner + @"',
                    1
                    )
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _log4net.Error("Error: " + ex);
            }

            return new JsonResult("Added Successfully");
        }

        public JsonResult Delete(Application app)
        {
            string query = @"
                    update view_Application set
                    Deleted = 0
                    where Application_ID = " + app.Application_ID + @"";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _log4net.Error("Error: " + ex);
            }

            return new JsonResult("Deleted Application Successfully");
        }

        public JsonResult UpdateApp(Application app)
        {
            string query = @"
                    update view_Application set 
                    Application_Name = '" + app.Application_Name + @"',
                    Owner = '" + app.Owner + @"'
                    
                    where Application_ID = " + app.Application_ID + @"
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _log4net.Error("Error: " + ex);
            }

            return new JsonResult("Updated Application Successfully");

        }
    }
}
