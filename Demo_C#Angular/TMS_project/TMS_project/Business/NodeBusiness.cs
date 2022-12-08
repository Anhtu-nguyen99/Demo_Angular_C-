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
    public class NodeBusiness : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _env;

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(NodeBusiness));
        public NodeBusiness(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _env = environment;
        }

        private void Map_data(DataRow dr, Node nod)
        {
            DataTable dt = dr.Table;

            if (dt.Columns.Contains("Node_ID"))
                nod.Node_ID = Convert.ToInt32(dr["Node_ID"]);
            if (dt.Columns.Contains("Node_Title"))
                nod.Node_Title = Convert.ToString(dr["Node_Title"]);
            if (dt.Columns.Contains("Node_Type"))
                nod.Node_Type = Convert.ToString(dr["Node_Type"]);
            if (dt.Columns.Contains("Node_Parent_ID"))
                nod.Node_Parent_ID = Convert.ToInt32(dr["Node_Parent_ID"]);
            if (dt.Columns.Contains("Submission_Date"))
                nod.Submission_Date = Convert.ToDateTime(dr["Submission_Date"]);
            if (dt.Columns.Contains("Owner"))
                nod.Owner = Convert.ToString(dr["Owner"]);
            if (dt.Columns.Contains("Application_ID"))
                nod.Application_ID = Convert.ToInt32(dr["Application_ID"]);
            if (dt.Columns.Contains("Deleted"))
                nod.Deleted = Convert.ToInt32(dr["Deleted"]);
            if (dt.Columns.Contains("Application_Name"))
                nod.Application_Name = Convert.ToString(dr["Application_Name"]);
            if (dt.Columns.Contains("Node_Parents"))
                nod.Node_Parents = Convert.ToString(dr["Node_Parents"]);
        }

        public List<Node> search_Node(string Node_Title)
        {
            List<Node> arr_Nod = new();
            string query = @"
                    select * from view_Node
                    where Node_Title = '" + Node_Title + @"'";
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
            
            if (table.Rows.Count > 0)
            {
                Node ObjNod = new();
                Map_data(table.Rows[0], ObjNod);
                arr_Nod.Add(ObjNod);
            } 
            else
            {
                _log4net.Info("There is no node named: " + Node_Title);
            }

            

            //for (int i = 0; Nod != null; i++)
            //{
            //    Node temp = GetParentByNode(Nod.Node_Parent_ID, Nod.Application_ID);
            //    if (temp != null)
            //    {
            //        Node Obj = temp;
            //        arr_Nod.Add(Obj);
            //    }
            //    Nod = temp;
            //}

            return arr_Nod;
        }

        /// <summary>
        /// Get all parent nodes by application ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Node> GetNodeParent(int id) 
        {
            List<Node> arr_Nod = new();
            string query = @"
                    select * from view_Node
                    where Application_ID = " + id + @"";
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
                Node ObjNod = new();
                Map_data(dr, ObjNod);
                arr_Nod.Add(ObjNod);
            }

            List<Node> arr_NodParent = new();
            foreach (Node n in arr_Nod)
            {
                n.Node_List = GetNodeByParentToCheck(n.Node_ID);
                if (n.Node_List.Count > 0)
                {
                    arr_NodParent.Add(n);
                }
            }

            return arr_NodParent;
        }

        /// <summary>
        /// Get list nodes of an application and sons of those nodes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Node> GetNodeApp(int id) 
        {
            List<Node> arr_Nod = new();
            string query = @"
                    select * from view_Node
                    where Application_ID = " + id + @"";
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
                Node ObjNod = new();
                Map_data(dr, ObjNod);
                arr_Nod.Add(ObjNod);
            }

            List<Node> arr_NodApp = new();
            foreach (Node n in arr_Nod)
            {
                int count = 0;
                for (int i = 0; i < arr_Nod.Count(); i++)
                {
                    if (n.Node_Parent_ID == arr_Nod[i].Node_ID)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    n.Node_List = GetNodeByParent(n.Node_ID, n.Application_ID); // return list nodes follow treeview structure
                    arr_NodApp.Add(n);
                }
            }

            return arr_NodApp;
        }

        /// <summary>
        /// Get parent of a node
        /// </summary>
        /// <param name="Node_Parent_ID"></param>
        /// <param name="App_ID"></param>
        /// <returns></returns>
        public Node GetParentByNode(int Node_Parent_ID, int App_ID)
        {
            Node Nod = new();
            string query = @"
                    select * from view_Node
                    where Node_ID = " + Node_Parent_ID + @" 
                    and Application_ID = " + App_ID + @"";
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
            
            if (table.Rows.Count > 0)
            {
                Node ObjNod = new();
                Map_data(table.Rows[0], ObjNod);
                Nod = ObjNod;
                return Nod;
            }
            else
            {
                _log4net.Info("can not find node parent as ID: " + Node_Parent_ID);
            }

            return null;
        }

        public List<Node> GetNodeByParent(int Node_ID, int App_ID) // return list Nodes follow treeview structure
        {
            List<Node> arr_Nod = new();
            string query = @"
                    select * from view_Node
                    where Node_Parent_ID = " + Node_ID + @"
                    and Application_ID = " + App_ID + @"";
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
                Node ObjNod = new();
                Map_data(dr, ObjNod);
                arr_Nod.Add(ObjNod);
            }

            foreach (Node n in arr_Nod)
            {
                int i = 0;
                n.Node_List = GetNodeByParent(n.Node_ID, n.Application_ID);
                i++;
                if ((n.Node_List.Count() == 0) && (i == arr_Nod.Count()))
                    return arr_Nod;
                
            }

            return arr_Nod;
        }
        
        public Node GetNodeInfo(int id) // Get information of a node
        {
            Node Nod = new();
            string query = @"
                    select * from view_Node
                    where Node_ID = " + id + @"";
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

            if (table.Rows.Count > 0)
            {
                Node ObjNod = new();
                Map_data(table.Rows[0], ObjNod);
                Nod = ObjNod;
            }
            else
            {
                _log4net.Info("can not Get node information as ID: " + id);
            }

            return Nod;
        }

        public List<Node> GetAllNode()
        {
            List<Node> arr_Nod = new();
            string query = @"
                    select * from view_Node";
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
                Node ObjNod = new();
                Map_data(dr, ObjNod);
                arr_Nod.Add(ObjNod);
            }

            return arr_Nod;
        }

        public JsonResult UpdateNode(Node nod)
        {
            DateTime time = DateTime.Now;
            string query = @"
                    update view_Node set 
                    Node_Title = '" + nod.Node_Title + @"',
                    Node_Type = '" + nod.Node_Type + @"',
                    Submission_Date = convert(nvarchar,'" + time + @"', 126),
                    Owner = '" + nod.Owner + @"',
                    Application_ID = '" + nod.Application_ID + @"'

                    where Node_ID = " + nod.Node_ID + @"
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

            return new JsonResult("Updated Node Successfully");

        }

        public JsonResult Post(Node nod)
        {
            DateTime time = DateTime.Now;
            string query = @"
                    insert into dbo.Node(Node_Title, Node_Type, Submission_Date, Owner, Application_ID, Node_Parent_ID, Deleted) 
                    Values 
                    (
                    '" + nod.Node_Title + @"',
                    '" + nod.Node_Type + @"',
                    convert(nvarchar,'" + time + @"', 126),
                    '" + nod.Owner + @"',
                    " + nod.Application_ID + @",
                    " + nod.Node_Parent_ID + @",
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

            return new JsonResult("Added Node Successfully");
        }

        public JsonResult Delete(Node nod)
        {
            string query = @"
                    update view_Node set 
                    Deleted = 0

                    where Node_ID = " + nod.Node_ID + @"
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

            return new JsonResult("Deleted Node Successfully");

        }

        public List<Node> GetNodeByParentToCheck(int Node_ID)
        {
            List<Node> arr_Nod = new();
            string query = @"
                    select * from view_Node
                    where Node_Parent_ID = " + Node_ID + @"";
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
                Node ObjNod = new();
                Map_data(dr, ObjNod);
                arr_Nod.Add(ObjNod);
            }

            return arr_Nod;
        }
    }
}
