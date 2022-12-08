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
    public class AttributeBusiness : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        public AttributeBusiness(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _env = environment;
        }

        private void Map_data(DataRow dr, Models.Attribute att)
        {
            DataTable dt = dr.Table;

            if (dt.Columns.Contains("Node_ID"))
                att.Node_ID = Convert.ToInt32(dr["Node_ID"]);
            if (dt.Columns.Contains("Attribute_ID"))
                att.Attribute_ID = Convert.ToInt32(dr["Attribute_ID"]);
            if (dt.Columns.Contains("Attribute_Name"))
                att.Attribute_Name = Convert.ToString(dr["Attribute_Name"]);
            if (dt.Columns.Contains("Deleted"))
                att.Deleted = Convert.ToInt32(dr["Deleted"]);
            if (dt.Columns.Contains("Node_Title"))
                att.Node_Title = Convert.ToString(dr["Node_Title"]);
        }

        public List<Models.Attribute> getAttributeList()
        {
            List<Models.Attribute> arr_Att = new();
            string query = @"
                    select * from view_Attribute";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
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

            foreach (DataRow dr in table.Rows)
            {
                Models.Attribute ObjAtt = new();
                Map_data(dr, ObjAtt);
                arr_Att.Add(ObjAtt);
            }

            return arr_Att;
        }

        public List<Models.Attribute> getAttributeList(int id)
        {
            List<Models.Attribute> arr_Att = new();
            string query = @"
                    select * from view_Attribute
                    where Node_ID = " + id + @"";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
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

            foreach (DataRow dr in table.Rows)
            {
                Models.Attribute ObjAtt = new();
                Map_data(dr, ObjAtt);
                arr_Att.Add(ObjAtt);
            }

            return arr_Att;
        }

        public JsonResult Post(Models.Attribute att)
        {
            string query = @"
                    insert into dbo.Attribute(Node_ID, Attribute_Name, Deleted) 
                    Values 
                    (
                    '" + att.Node_ID + @"',
                    '" + att.Attribute_Name + @"',
                    1
                    )
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
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

            return new JsonResult("Added Attribute Successfully");
        }

        public JsonResult UpdateAttribute(Models.Attribute att)
        {
            string query = @"
                    update view_Attribute set 
                    Attribute_Name = '" + att.Attribute_Name + @"',
                    Node_ID = " + att.Node_ID + @"

                    where Attribute_ID = " + att.Attribute_ID + @"
                    ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
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

            return new JsonResult("Updated Attribute Successfully");

        }

        public JsonResult Delete(Models.Attribute att)
        {
            string query = @"
                    update view_Attribute set
                    Deleted = 0
                    where Attribute_ID = " + att.Attribute_ID + @"";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TMSAppCon");
            SqlDataReader myReader;
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

            return new JsonResult("Deleted Attribute Successfully");
        }
    }
}
