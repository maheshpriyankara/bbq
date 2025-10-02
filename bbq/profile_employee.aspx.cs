using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;

using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Collections;
using System.Drawing;
using System.Web.Services;
using System.IO;
using CrystalDecisions.Shared;
using bbq.reports_;
using System.Text.RegularExpressions;

namespace bbq
{
    public partial class profile_employee : System.Web.UI.Page
    {

        static SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        static SqlConnection conn2 = new SqlConnection(
   WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        static SqlConnection conn_main;
        SqlDataReader reader2;
        static SqlDataReader reader;
        db db;
        DateTime sys_date;
        string userID_;
        static ArrayList nicList;
        static List<string> empResult;
        int category_id;
        [WebMethod]
        public static List<string> getEmployee(string empName)
        {
            empResult = new List<string>();
            try
            {
                conn.Close();

                conn.Open();
                reader = new SqlCommand("select name,epfno,empid from emp where name like '%" + empName + "%' or epfno like '" + empName + "%' or empid like '" + empName + "%'", conn).ExecuteReader();
                while (reader.Read())
                {
                    //var epfNo = "<span style='color:red'>" + "EPF No-" + reader[1].ToString() + "</span>";
                    //var empNo = "<span style='color:green'>" + "Employer No-" + reader[2].ToString() + "</span>";
                    //string empResultText = reader.GetString(0) + " ( " + epfNo + ", " + empNo + ")";
                    //string encodedEmpResultText = HttpUtility.HtmlEncode(empResultText);
                    //  empResult.Add(empResultText);
                    empResult.Add(reader.GetString(0) + " (" + "EPF No-" + reader[1].ToString() + "," + "Employer No-" + reader[2].ToString() + ")");
                }
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }

            return empResult;
        }
        [WebMethod]
        public static string employeeSelected(string empName)
        {
            var result = "";
            var result_name = "";
            var result_epfNo = "";
            var result_empNo = "";



            try
            {

                result_name = empName.Split('(')[0].ToString().Remove(empName.Split('(')[0].ToString().Length - 1);
                result = empName.Split('(')[1].ToString();
                result = result.Split(')')[0].ToString();
                result_epfNo = result.Split(',')[0].ToString().Split('-')[1];
                result_empNo = result.Split(',')[1].ToString().Split('-')[1];
            }
            catch (Exception)
            {
            }
            //var str = "foo_bar_baz";
            //string[] parts = str.Split(new[] { "bar" }, StringSplitOptions.None);
            return result_name + "_" + result_epfNo + "_" + result_empNo;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          

        }
        void loadList(DropDownList list, SqlConnection conn, SqlDataReader reader, String table, int index, String orderBy)
        {
            try
            {
                list.Items.Clear();
                conn.Open();
                reader = new SqlCommand("select * from " + table + " order by " + orderBy, conn).ExecuteReader();
                while (reader.Read())
                {
                    list.Items.Add(reader[index].ToString());
                }
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }

        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            // Session["userID"] = null;
        }

        protected void grid_allowances_RowEditing(object sender, GridViewEditEventArgs e)
        {

            //  BindGridData();
        }

        string empNO = "";
        protected void btn_loadEmployee_Click(object sender, EventArgs e)
        {
           
        }

        protected void grid_allowances_RowEditing1(object sender, GridViewEditEventArgs e)
        {
        }

        protected void check_defaultRoaster_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
          
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
        }

        Int32 empIDDB;
        void getEmpID()
        {
            try
            {
                conn.Open();
                reader = new SqlCommand("select max(id) from emp", conn).ExecuteReader();
                if (reader.Read())
                {
                    empIDDB = reader.GetInt32(0);
                }
                conn.Close();
                empIDDB++;
                empNO = empIDDB + "";
            }
            catch (Exception)
            {
                conn.Close();
                empIDDB = 1;
                empNO = empIDDB + "";
            }
        }
    }
}