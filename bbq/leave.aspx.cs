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
    public partial class leave : System.Web.UI.Page
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
                    date_manual.Value = sys_date.ToString("yyyy-MM-dd");
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
        public static List<string> getEmployee(string empName)
        {
            empResult = new List<string>();
            try
            {
                conn.Close();

                conn.Open();
                reader = new SqlCommand("select name,epfno,empid from emp where name like '%" + empName + "%' or epfno like '" + empName + "%'", conn).ExecuteReader();
                while (reader.Read())
                {
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
        public static string employeeSelected(string empName, string year, string month)
        {
            var temp = "";
            var result = "";
            var result_epfNo = "";
            var db_ = new db();
            var id = "";
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
                id = reader[1] + "";
                temp = result_epfNo + "_" + reader[52].ToString() + "_" + reader[51].ToString() + "_" + reader[53].ToString();
            }
            conn.Close();
            var deduct = 0.0;
            var earning = 0.0;
            try
            {
                conn.Open();
                reader = new SqlCommand("select * fixedValue where id='" + id + "' and month='" + year + "/" + month + "'", conn).ExecuteReader();
                if (reader.Read())
                {
                    deduct = reader.GetDouble(2);
                    deduct = deduct + reader.GetDouble(3);
                    deduct = deduct + reader.GetDouble(4);
                    earning = reader.GetDouble(5);
                    earning = earning + reader.GetDouble(6);
                    deduct = deduct + reader.GetDouble(7);
                }
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
            try
            {
                conn.Open();
                reader = new SqlCommand("select sum(amount) from deduct where id='" + id + "'  and month='" + year + "/" + month + "'", conn).ExecuteReader();
                if (reader.Read())
                {
                    temp = temp + "_" + db_.setAmountFormat((reader.GetDouble(0) + deduct).ToString());
                }
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
                temp = temp + "_0.00";
            }

            try
            {
                conn.Open();
                reader = new SqlCommand("select sum(amount) from earning where id='" + id + "' and month='" + year + "/" + month + "'", conn).ExecuteReader();
                if (reader.Read())
                {
                    temp = temp + "_" + db_.setAmountFormat((reader.GetDouble(0) + earning).ToString());
                }
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
                temp = temp + "_0.00";
            }

            return temp;
        }

    }
}