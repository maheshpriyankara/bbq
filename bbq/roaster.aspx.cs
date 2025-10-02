using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bbq
{
    public partial class roaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] != null && Session["userType"] != null)
            {
                if (!Page.IsPostBack)
                {

                    loadList(date_search, conn, reader, "list_years", 0, "name");

                    Session["processPeriod"] = new db().getCurrentSalaryPeriod(conn, reader);
                    var temp = Session["processPeriod"].ToString();
                    date_search.Text = temp.Split('/')[0].ToString();
                    list_month.Value = temp.Split('/')[1].ToString();
                    sys_date = new db().getSysDateTime();
                    list_lines.Items.Add("All");
                    conn.Open();
                    reader = new SqlCommand("select name from line order by name", conn).ExecuteReader();
                    while (reader.Read())
                    {
                        list_lines.Items.Add(reader.GetString(0));
                    }
                    conn.Close();
                }
            }
            else
            {
                Response.Redirect("login.aspx");
            }

        }
        void loadList(DropDownList list, SqlConnection conn, SqlDataReader reader, String table, int index, String orderBy)
        {
            try
            {
                conn.Open();
                reader = new SqlCommand("select * from " + table + " order by " + orderBy, conn).ExecuteReader();
                while (reader.Read())
                {
                    list.Items.Add(reader[index].ToString());
                }
                conn.Close();
            }
            catch (Exception a)
            {
                var tt = a.Message;
                conn.Close();
            }

        }


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
        public static List<string> getEmployee(string empName, string line)
        {
            empResult = new List<string>();
            try
            {
                conn.Close();

                conn.Open();
                if (!line.Equals("All"))
                {
                    if (empName.Equals("0"))
                    {
                        reader = new SqlCommand("select name,epfno,empid,type from emp where line='" + line + "' and resgin='false' order by name", conn).ExecuteReader();

                    }
                    else
                    {
                        reader = new SqlCommand("select name,epfno,empid,type from emp where line='" + line + "' and resgin='false' and id in ( select id from emp where name like '%" + empName + "%' or epfno like '" + empName + "%' or type like '" + empName + "%')", conn).ExecuteReader();

                    }
                    while (reader.Read())
                    {
                        empResult.Add(reader.GetString(0) + " (" + "EPF No-" + reader[1].ToString() + "," + "Employer No-" + reader[2].ToString() + "," + "Attendance No-" + reader[3].ToString() + ")");
                    }
                }
                else
                {
                    reader = new SqlCommand("select name,epfno,empid,type from emp where name like '%" + empName + "%' or epfno like '" + empName + "%' or type like '" + empName + "%'", conn).ExecuteReader();
                    while (reader.Read())
                    {
                        empResult.Add(reader.GetString(0) + " (" + "EPF No-" + reader[1].ToString() + "," + "Employer No-" + reader[2].ToString() + "," + "Attendance No-" + reader[3].ToString() + ")");
                    }
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
        public static string employeeSelected(string empName, string year, string month)
        {
            var temp = "";
            var result = "";
            var result_epfNo = "";
            var db_ = new db();
            var tempNoPay = 0;
            conn.Close();
            try
            {

                result = empName.Split('(')[1].ToString();
                result = result.Split(')')[0].ToString();
                result_epfNo = result.Split(',')[0].ToString().Split('-')[1];
            }
            catch (Exception)
            {
                result_epfNo = empName;
            }


            conn.Open();
            reader = new SqlCommand("select a.*,b.name,c.empid,c.line from paysheet as a,emp as b,empbackup as c where a.month_2='" + year + "/" + month + "' and a.empID_1=b.id and b.tempepfno='" + result_epfNo + "' and b.tempepfno=c.epfno  and c.month='" + year + "/" + month + "'", conn).ExecuteReader();
            if (reader.Read())
            {
                temp = result_epfNo + "_" + reader[52].ToString() + "_" + reader[51].ToString() + "_" + reader[53].ToString() + "_" + reader.GetDouble(7) + " Days" + "_" + reader.GetDouble(8) + " Days";
            }
            conn.Close();

            return temp;
        }
    }
}