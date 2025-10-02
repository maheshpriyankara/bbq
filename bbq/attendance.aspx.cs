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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace bbq
{
    public partial class attendance : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] != null && Session["userType"] != null)
            {
                if (!Page.IsPostBack)
                {

                    loadList(list_search, conn, reader, "line", 1, "name");
                    loadList(year_from, conn, reader, "list_years", 0, "name");
                    loadList(date_search, conn, reader, "list_years", 0, "name");
                    loadList(year_to, conn, reader, "list_years", 0, "name");

                    Session["processPeriod"] = new db().getCurrentSalaryPeriod(conn, reader);
                    var temp = Session["processPeriod"].ToString();
                    date_search.Text = temp.Split('/')[0].ToString();
                    list_month.Value = temp.Split('/')[1].ToString();
                    year_from.Text = temp.Split('/')[0].ToString();
                    year_to.Text = temp.Split('/')[0].ToString();
                    month_from.Value = temp.Split('/')[1].ToString();
                    month_to.Value = temp.Split('/')[1].ToString();
                    sys_date = new db().getSysDateTime();
                    date_manual.Value = sys_date.ToString("yyyy-MM-dd");
                    time_manual.Value = sys_date.ToString("HH:mm:ss");


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



        protected void Previous_Click1(object sender, EventArgs e)
        {

        }

        protected async void Next_Click1Async(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            text_employee.Text = "";
            var epfNo = 0;
            db = new db();
            try
            {
                epfNo = Int32.Parse(text_epfNo_.Value);
            }
            catch (Exception)
            {
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44341/api/home/getall?empname=47&year=2023&month=March");
                //var responseContent = await response.Content.ReadAsStringAsync();
                //var joke = JObject.Parse(responseContent)[0].ToObject<DataModel>();
                //Console.WriteLine($"ID: {joke.InDate}");
                //Console.WriteLine($"Joke: {joke.ShiftOneInTime}");


                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // check if the JSON input is an array
                    if (responseContent.StartsWith("["))
                    {
                        // if the JSON input is an array, parse it as a JArray
                        JArray responseData = JArray.Parse(responseContent);

                        // access the elements of the array
                        foreach (JObject obj in responseData)
                        {

                            string property1 = obj["InDate"].ToString();
                            string property2 = obj["ShiftOneInTime"].ToString();
                            // Console.WriteLine($"Property 1: {property1}");
                            //    Console.WriteLine($"Property 2: {property2}");
                        }
                    }
                    else
                    {
                        // if the JSON input is an object, parse it as a JObject
                        JObject responseData = JObject.Parse(responseContent);
                        string property1 = responseData["InDate"].ToString();
                        string property2 = responseData["ShiftOneInTime"].ToString();
                        //  Console.WriteLine($"Property 1: {property1}");
                        // Console.WriteLine($"Property 2: {property2}");
                    }
                }
                else
                {
                    // Console.WriteLine($"API returned an error: {response.StatusCode}");
                }
            }
        }
        string reportCompanyName, reportName, reportLine, reportEpfNO, addDay_, addHDay_, hDay_, haldDayReprt;

        protected void export_timesheetAdvance_Click(object sender, EventArgs e)
        {
            db = new db();
            var date_from = new DateTime(Int32.Parse(year_from.Text), Int32.Parse(db.getMOnth(month_from.Value)), 1);
            var date_to = new DateTime(Int32.Parse(year_to.Text), Int32.Parse(db.getMOnth(month_to.Value)), 1);
            var search_tag = "";

            if (date_to.Date < date_from.Date)
            {
                Response.Write("<script>alert('Sorry, Invalied Year/Month Selection')</script>");
            }
            else if (list_search.SelectedIndex == 0 && text_epfNo_.Value.Equals(""))
            {
                Response.Write("<script>alert('Please Select Employee First')</script>");
            }
            else
            {
                try
                {
                    List<Tuple<int, int>> temp_yearmonthlist = db.getYearMonthDifference(date_from, date_to);
                    DataSet ds = new DataSet();
                    var dir = new DirectoryInfo(Server.MapPath("/PDF/"));
                    db = new db();
                    int epfNo = 0;
                    var idL = "";
                    DataTable dt = new DataTable();
                    {
                        dt.Columns.Add("CompanyName", typeof(string));
                        dt.Columns.Add("name", typeof(string));
                        dt.Columns.Add("epfNo", typeof(string));
                        dt.Columns.Add("line", typeof(string));
                        dt.Columns.Add("id", typeof(string));
                        dt.Columns.Add("month", typeof(string));
                        dt.Columns.Add("1", typeof(string));
                        dt.Columns.Add("1A", typeof(string));
                        dt.Columns.Add("1B", typeof(string));
                        dt.Columns.Add("1C", typeof(string));
                        dt.Columns.Add("1D", typeof(string));
                        dt.Columns.Add("1E", typeof(string));
                        dt.Columns.Add("1F", typeof(string));
                        dt.Columns.Add("1G", typeof(string));
                        dt.Columns.Add("1H", typeof(string));
                        dt.Columns.Add("1I", typeof(string));
                        dt.Columns.Add("2", typeof(string));
                        dt.Columns.Add("2A", typeof(string));
                        dt.Columns.Add("2B", typeof(string));
                        dt.Columns.Add("2C", typeof(string));
                        dt.Columns.Add("2D", typeof(string));
                        dt.Columns.Add("2E", typeof(string));
                        dt.Columns.Add("2F", typeof(string));
                        dt.Columns.Add("2G", typeof(string));
                        dt.Columns.Add("2H", typeof(string));
                        dt.Columns.Add("2I", typeof(string));
                        dt.Columns.Add("3", typeof(string));
                        dt.Columns.Add("3A", typeof(string));
                        dt.Columns.Add("3B", typeof(string));
                        dt.Columns.Add("3C", typeof(string));
                        dt.Columns.Add("3D", typeof(string));
                        dt.Columns.Add("3E", typeof(string));
                        dt.Columns.Add("3F", typeof(string));
                        dt.Columns.Add("3G", typeof(string));
                        dt.Columns.Add("3H", typeof(string));
                        dt.Columns.Add("3I", typeof(string));
                        dt.Columns.Add("4", typeof(string));
                        dt.Columns.Add("4A", typeof(string));
                        dt.Columns.Add("4B", typeof(string));
                        dt.Columns.Add("4C", typeof(string));
                        dt.Columns.Add("4D", typeof(string));
                        dt.Columns.Add("4E", typeof(string));
                        dt.Columns.Add("4F", typeof(string));
                        dt.Columns.Add("4G", typeof(string));
                        dt.Columns.Add("4H", typeof(string));
                        dt.Columns.Add("4I", typeof(string));
                        dt.Columns.Add("5", typeof(string));
                        dt.Columns.Add("5A", typeof(string));
                        dt.Columns.Add("5B", typeof(string));
                        dt.Columns.Add("5C", typeof(string));
                        dt.Columns.Add("5D", typeof(string));
                        dt.Columns.Add("5E", typeof(string));
                        dt.Columns.Add("5F", typeof(string));
                        dt.Columns.Add("5G", typeof(string));
                        dt.Columns.Add("5H", typeof(string));
                        dt.Columns.Add("5I", typeof(string));
                        dt.Columns.Add("6", typeof(string));
                        dt.Columns.Add("6A", typeof(string));
                        dt.Columns.Add("6B", typeof(string));
                        dt.Columns.Add("6C", typeof(string));
                        dt.Columns.Add("6D", typeof(string));
                        dt.Columns.Add("6E", typeof(string));
                        dt.Columns.Add("6F", typeof(string));
                        dt.Columns.Add("6G", typeof(string));
                        dt.Columns.Add("6H", typeof(string));
                        dt.Columns.Add("6I", typeof(string));
                        dt.Columns.Add("7", typeof(string));
                        dt.Columns.Add("7A", typeof(string));
                        dt.Columns.Add("7B", typeof(string));
                        dt.Columns.Add("7C", typeof(string));
                        dt.Columns.Add("7D", typeof(string));
                        dt.Columns.Add("7E", typeof(string));
                        dt.Columns.Add("7F", typeof(string));
                        dt.Columns.Add("7G", typeof(string));
                        dt.Columns.Add("7H", typeof(string));
                        dt.Columns.Add("7I", typeof(string));
                        dt.Columns.Add("8", typeof(string));
                        dt.Columns.Add("8A", typeof(string));
                        dt.Columns.Add("8B", typeof(string));
                        dt.Columns.Add("8C", typeof(string));
                        dt.Columns.Add("8D", typeof(string));
                        dt.Columns.Add("8E", typeof(string));
                        dt.Columns.Add("8F", typeof(string));
                        dt.Columns.Add("8G", typeof(string));
                        dt.Columns.Add("8H", typeof(string));
                        dt.Columns.Add("8I", typeof(string));
                        dt.Columns.Add("9", typeof(string));
                        dt.Columns.Add("9A", typeof(string));
                        dt.Columns.Add("9B", typeof(string));
                        dt.Columns.Add("9C", typeof(string));
                        dt.Columns.Add("9D", typeof(string));
                        dt.Columns.Add("9E", typeof(string));
                        dt.Columns.Add("9F", typeof(string));
                        dt.Columns.Add("9G", typeof(string));
                        dt.Columns.Add("9H", typeof(string));
                        dt.Columns.Add("9I", typeof(string));
                        dt.Columns.Add("10", typeof(string));
                        dt.Columns.Add("10A", typeof(string));
                        dt.Columns.Add("10B", typeof(string));
                        dt.Columns.Add("10C", typeof(string));
                        dt.Columns.Add("10D", typeof(string));
                        dt.Columns.Add("10E", typeof(string));
                        dt.Columns.Add("10F", typeof(string));
                        dt.Columns.Add("10G", typeof(string));
                        dt.Columns.Add("10H", typeof(string));
                        dt.Columns.Add("10I", typeof(string));
                        dt.Columns.Add("11", typeof(string));
                        dt.Columns.Add("11A", typeof(string));
                        dt.Columns.Add("11B", typeof(string));
                        dt.Columns.Add("11C", typeof(string));
                        dt.Columns.Add("11D", typeof(string));
                        dt.Columns.Add("11E", typeof(string));
                        dt.Columns.Add("11F", typeof(string));
                        dt.Columns.Add("11G", typeof(string));
                        dt.Columns.Add("11H", typeof(string));
                        dt.Columns.Add("11I", typeof(string));
                        dt.Columns.Add("12", typeof(string));
                        dt.Columns.Add("12A", typeof(string));
                        dt.Columns.Add("12B", typeof(string));
                        dt.Columns.Add("12C", typeof(string));
                        dt.Columns.Add("12D", typeof(string));
                        dt.Columns.Add("12E", typeof(string));
                        dt.Columns.Add("12F", typeof(string));
                        dt.Columns.Add("12G", typeof(string));
                        dt.Columns.Add("12H", typeof(string));
                        dt.Columns.Add("12I", typeof(string));
                        dt.Columns.Add("13", typeof(string));
                        dt.Columns.Add("13A", typeof(string));
                        dt.Columns.Add("13B", typeof(string));
                        dt.Columns.Add("13C", typeof(string));
                        dt.Columns.Add("13D", typeof(string));
                        dt.Columns.Add("13E", typeof(string));
                        dt.Columns.Add("13F", typeof(string));
                        dt.Columns.Add("13G", typeof(string));
                        dt.Columns.Add("13H", typeof(string));
                        dt.Columns.Add("13I", typeof(string));
                        dt.Columns.Add("14", typeof(string));
                        dt.Columns.Add("14A", typeof(string));
                        dt.Columns.Add("14B", typeof(string));
                        dt.Columns.Add("14C", typeof(string));
                        dt.Columns.Add("14D", typeof(string));
                        dt.Columns.Add("14E", typeof(string));
                        dt.Columns.Add("14F", typeof(string));
                        dt.Columns.Add("14G", typeof(string));
                        dt.Columns.Add("14H", typeof(string));
                        dt.Columns.Add("14I", typeof(string));
                        dt.Columns.Add("15", typeof(string));
                        dt.Columns.Add("15A", typeof(string));
                        dt.Columns.Add("15B", typeof(string));
                        dt.Columns.Add("15C", typeof(string));
                        dt.Columns.Add("15D", typeof(string));
                        dt.Columns.Add("15E", typeof(string));
                        dt.Columns.Add("15F", typeof(string));
                        dt.Columns.Add("15G", typeof(string));
                        dt.Columns.Add("15H", typeof(string));
                        dt.Columns.Add("15I", typeof(string));
                        dt.Columns.Add("16", typeof(string));
                        dt.Columns.Add("16A", typeof(string));
                        dt.Columns.Add("16B", typeof(string));
                        dt.Columns.Add("16C", typeof(string));
                        dt.Columns.Add("16D", typeof(string));
                        dt.Columns.Add("16E", typeof(string));
                        dt.Columns.Add("16F", typeof(string));
                        dt.Columns.Add("16G", typeof(string));
                        dt.Columns.Add("16H", typeof(string));
                        dt.Columns.Add("16I", typeof(string));
                        dt.Columns.Add("17", typeof(string));
                        dt.Columns.Add("17A", typeof(string));
                        dt.Columns.Add("17B", typeof(string));
                        dt.Columns.Add("17C", typeof(string));
                        dt.Columns.Add("17D", typeof(string));
                        dt.Columns.Add("17E", typeof(string));
                        dt.Columns.Add("17F", typeof(string));
                        dt.Columns.Add("17G", typeof(string));
                        dt.Columns.Add("17H", typeof(string));
                        dt.Columns.Add("17I", typeof(string));
                        dt.Columns.Add("18", typeof(string));
                        dt.Columns.Add("18A", typeof(string));
                        dt.Columns.Add("18B", typeof(string));
                        dt.Columns.Add("18C", typeof(string));
                        dt.Columns.Add("18D", typeof(string));
                        dt.Columns.Add("18E", typeof(string));
                        dt.Columns.Add("18F", typeof(string));
                        dt.Columns.Add("18G", typeof(string));
                        dt.Columns.Add("18H", typeof(string));
                        dt.Columns.Add("18I", typeof(string));
                        dt.Columns.Add("19", typeof(string));
                        dt.Columns.Add("19A", typeof(string));
                        dt.Columns.Add("19B", typeof(string));
                        dt.Columns.Add("19C", typeof(string));
                        dt.Columns.Add("19D", typeof(string));
                        dt.Columns.Add("19E", typeof(string));
                        dt.Columns.Add("19F", typeof(string));
                        dt.Columns.Add("19G", typeof(string));
                        dt.Columns.Add("19H", typeof(string));
                        dt.Columns.Add("19I", typeof(string));
                        dt.Columns.Add("20", typeof(string));
                        dt.Columns.Add("20A", typeof(string));
                        dt.Columns.Add("20B", typeof(string));
                        dt.Columns.Add("20C", typeof(string));
                        dt.Columns.Add("20D", typeof(string));
                        dt.Columns.Add("20E", typeof(string));
                        dt.Columns.Add("20F", typeof(string));
                        dt.Columns.Add("20G", typeof(string));
                        dt.Columns.Add("20H", typeof(string));
                        dt.Columns.Add("20I", typeof(string));
                        dt.Columns.Add("21", typeof(string));
                        dt.Columns.Add("21A", typeof(string));
                        dt.Columns.Add("21B", typeof(string));
                        dt.Columns.Add("21C", typeof(string));
                        dt.Columns.Add("21D", typeof(string));
                        dt.Columns.Add("21E", typeof(string));
                        dt.Columns.Add("21F", typeof(string));
                        dt.Columns.Add("21G", typeof(string));
                        dt.Columns.Add("21H", typeof(string));
                        dt.Columns.Add("21I", typeof(string));
                        dt.Columns.Add("22", typeof(string));
                        dt.Columns.Add("22A", typeof(string));
                        dt.Columns.Add("22B", typeof(string));
                        dt.Columns.Add("22C", typeof(string));
                        dt.Columns.Add("22D", typeof(string));
                        dt.Columns.Add("22E", typeof(string));
                        dt.Columns.Add("22F", typeof(string));
                        dt.Columns.Add("22G", typeof(string));
                        dt.Columns.Add("22H", typeof(string));
                        dt.Columns.Add("22I", typeof(string));
                        dt.Columns.Add("23", typeof(string));
                        dt.Columns.Add("23A", typeof(string));
                        dt.Columns.Add("23B", typeof(string));
                        dt.Columns.Add("23C", typeof(string));
                        dt.Columns.Add("23D", typeof(string));
                        dt.Columns.Add("23E", typeof(string));
                        dt.Columns.Add("23F", typeof(string));
                        dt.Columns.Add("23G", typeof(string));
                        dt.Columns.Add("23H", typeof(string));
                        dt.Columns.Add("23I", typeof(string));
                        dt.Columns.Add("24", typeof(string));
                        dt.Columns.Add("24A", typeof(string));
                        dt.Columns.Add("24B", typeof(string));
                        dt.Columns.Add("24C", typeof(string));
                        dt.Columns.Add("24D", typeof(string));
                        dt.Columns.Add("24E", typeof(string));
                        dt.Columns.Add("24F", typeof(string));
                        dt.Columns.Add("24G", typeof(string));
                        dt.Columns.Add("24H", typeof(string));
                        dt.Columns.Add("24I", typeof(string));
                        dt.Columns.Add("25", typeof(string));
                        dt.Columns.Add("25A", typeof(string));
                        dt.Columns.Add("25B", typeof(string));
                        dt.Columns.Add("25C", typeof(string));
                        dt.Columns.Add("25D", typeof(string));
                        dt.Columns.Add("25E", typeof(string));
                        dt.Columns.Add("25F", typeof(string));
                        dt.Columns.Add("25G", typeof(string));
                        dt.Columns.Add("25H", typeof(string));
                        dt.Columns.Add("25I", typeof(string));
                        dt.Columns.Add("26", typeof(string));
                        dt.Columns.Add("26A", typeof(string));
                        dt.Columns.Add("26B", typeof(string));
                        dt.Columns.Add("26C", typeof(string));
                        dt.Columns.Add("26D", typeof(string));
                        dt.Columns.Add("26E", typeof(string));
                        dt.Columns.Add("26F", typeof(string));
                        dt.Columns.Add("26G", typeof(string));
                        dt.Columns.Add("26H", typeof(string));
                        dt.Columns.Add("26I", typeof(string));
                        dt.Columns.Add("27", typeof(string));
                        dt.Columns.Add("27A", typeof(string));
                        dt.Columns.Add("27B", typeof(string));
                        dt.Columns.Add("27C", typeof(string));
                        dt.Columns.Add("27D", typeof(string));
                        dt.Columns.Add("27E", typeof(string));
                        dt.Columns.Add("27F", typeof(string));
                        dt.Columns.Add("27G", typeof(string));
                        dt.Columns.Add("27H", typeof(string));
                        dt.Columns.Add("27I", typeof(string));
                        dt.Columns.Add("28", typeof(string));
                        dt.Columns.Add("28A", typeof(string));
                        dt.Columns.Add("28B", typeof(string));
                        dt.Columns.Add("28C", typeof(string));
                        dt.Columns.Add("28D", typeof(string));
                        dt.Columns.Add("28E", typeof(string));
                        dt.Columns.Add("28F", typeof(string));
                        dt.Columns.Add("28G", typeof(string));
                        dt.Columns.Add("28H", typeof(string));
                        dt.Columns.Add("28I", typeof(string));
                        dt.Columns.Add("29", typeof(string));
                        dt.Columns.Add("29A", typeof(string));
                        dt.Columns.Add("29B", typeof(string));
                        dt.Columns.Add("29C", typeof(string));
                        dt.Columns.Add("29D", typeof(string));
                        dt.Columns.Add("29E", typeof(string));
                        dt.Columns.Add("29F", typeof(string));
                        dt.Columns.Add("29G", typeof(string));
                        dt.Columns.Add("29H", typeof(string));
                        dt.Columns.Add("29I", typeof(string));
                        dt.Columns.Add("30", typeof(string));
                        dt.Columns.Add("30A", typeof(string));
                        dt.Columns.Add("30B", typeof(string));
                        dt.Columns.Add("30C", typeof(string));
                        dt.Columns.Add("30D", typeof(string));
                        dt.Columns.Add("30E", typeof(string));
                        dt.Columns.Add("30F", typeof(string));
                        dt.Columns.Add("30G", typeof(string));
                        dt.Columns.Add("30H", typeof(string));
                        dt.Columns.Add("30I", typeof(string));
                        dt.Columns.Add("31", typeof(string));
                        dt.Columns.Add("31A", typeof(string));
                        dt.Columns.Add("31B", typeof(string));
                        dt.Columns.Add("31C", typeof(string));
                        dt.Columns.Add("31D", typeof(string));
                        dt.Columns.Add("31E", typeof(string));
                        dt.Columns.Add("31F", typeof(string));
                        dt.Columns.Add("31G", typeof(string));
                        dt.Columns.Add("31H", typeof(string));
                        dt.Columns.Add("31I", typeof(string));
                        dt.Columns.Add("TC", typeof(string));
                        dt.Columns.Add("TD", typeof(string));
                        dt.Columns.Add("TE", typeof(string));
                        dt.Columns.Add("TF", typeof(string));
                        dt.Columns.Add("TG", typeof(string));
                        dt.Columns.Add("TH", typeof(string));
                        dt.Columns.Add("TI", typeof(string));
                    }
                    conn2.Close();
                    try
                    {
                        epfNo = Int32.Parse(text_epfNo_.Value);
                    }
                    catch (Exception)
                    {
                    }
                    conn.Close();
                    conn.Open();
                    reader = new SqlCommand("select id from emp where epfno='" + epfNo + "'", conn).ExecuteReader();
                    if (reader.Read())
                    {
                        idL = reader[0] + "";
                    }
                    conn.Close();
                    var check_ = false;
                    if (list_search.SelectedIndex == 0)
                    {

                        foreach (Tuple<int, int> yearMonth in temp_yearmonthlist)
                        {
                            var period = yearMonth.Item1 + "/" + db.getMOnthName(yearMonth.Item2.ToString());
                            if (db.CheckEmployee(epfNo.ToString(), period, conn, reader))
                            {
                                check_ = true;
                            }

                        }
                    }
                    else if (list_search.SelectedIndex >= 1)
                    {
                        foreach (Tuple<int, int> yearMonth in temp_yearmonthlist)
                        {
                            var period = yearMonth.Item1 + "/" + db.getMOnthName(yearMonth.Item2.ToString());
                            var year = yearMonth.Item1.ToString();
                            var month = db.getMOnthName(yearMonth.Item2.ToString());

                            try
                            {

                                conn.Close();
                                conn2.Open();
                                if (list_search.SelectedIndex == 1)
                                {
                                    reader2 = new SqlCommand("select b.id,b.epfno from emp as b,empbackup as d where  b.tempepfno=d.epfno and b.resgin='" + false + "' and d.month='" + period + "'", conn2).ExecuteReader();

                                }
                                else
                                {
                                    reader2 = new SqlCommand("select b.id,b.epfno from emp as b,empbackup as d where  b.tempepfno=d.epfno and b.resgin='" + false + "' and d.month='" + period + "' and d.line='" + list_search.Text + "'", conn2).ExecuteReader();

                                }

                                while (reader2.Read())
                                {
                                    if (db.CheckEmployee(reader2[1].ToString(), period, conn, reader))
                                    {
                                        check_ = true;
                                    }
                                }
                                conn2.Close();
                            }
                            catch (Exception a)
                            {
                            }
                        }
                    }
                    if (check_)
                    {
                        Response.Write("<script>alert('Sorry, One or More Period still Processing.... , Can not Export Sucessfully.')</script>");
                    }
                    else
                    {

                        if (list_search.SelectedIndex == 0)
                        {
                            search_tag = year_from.Text + month_from.Value + "_to_" + year_to.Text + month_to.Value + "_singleemployee_" + text_epfNo_.Value;
                            conn.Open();

                            reader = new SqlCommand("select a.name,b.name,b.epfno,b.line from company as a, emp as b where b.id='" + idL + "' and b.company=a.id", conn).ExecuteReader();
                            if (reader.Read())
                            {
                                reportCompanyName = reader.GetString(0).ToUpper();
                                reportName = reader.GetString(1).ToUpper();
                                reportEpfNO = reader[2].ToString();
                                reportLine = reader.GetString(3).ToUpper();
                            }
                            conn.Close();

                            foreach (Tuple<int, int> yearMonth in temp_yearmonthlist)
                            {
                                var period = yearMonth.Item1 + "/" + db.getMOnthName(yearMonth.Item2.ToString());
                                var year = yearMonth.Item1.ToString();
                                var month = db.getMOnthName(yearMonth.Item2.ToString());
                                var dateList = db.getDateList(idL, db.getMOnth(month), year, conn, reader);
                                if (!db.CheckEmployee(epfNo.ToString(), period, conn, reader))
                                {
                                    {
                                        var dayteType = "";
                                        totalOTNOrmal_ = TimeSpan.Parse("00:00"); totalOtSunday_ = TimeSpan.Parse("00:00"); TotalExtraOt_ = TimeSpan.Parse("00:00"); totalLate_ = TimeSpan.Parse("00:00");
                                        TotaladddAY_ = 0; totalAddHalfDay_ = 0; totalHalfy_ = 0;
                                        totalAnnual_ = 0;
                                        totalCAshual_ = 0;
                                        totalSick_ = 0;


                                        ArrayList value = new ArrayList();
                                        for (int xi = 0; xi < dateList.Count; xi++)
                                        {
                                            dayteType = "";
                                            tempDate = xi;
                                            tempDate++;
                                            conn.Open();
                                            reader = new SqlCommand("select ispay,isPoya from calendar where date='" + Convert.ToDateTime(dateList[xi]) + "'", conn).ExecuteReader();
                                            if (reader.Read())
                                            {
                                                if (!reader.GetBoolean(0))
                                                {
                                                    if (reader.GetBoolean(1))
                                                    {
                                                        dayteType = "-P";
                                                    }
                                                    else
                                                    {
                                                        dayteType = "-H";
                                                    }

                                                }
                                                else
                                                {
                                                    if (Convert.ToDateTime(dateList[xi]).DayOfWeek == DayOfWeek.Sunday)
                                                    {
                                                        dayteType = "-S";
                                                    }

                                                }
                                            }
                                            conn.Close();
                                            conn.Open();
                                            reader = new SqlCommand("select * from timesheet where empid_1='" + idL + "' and inDate_3='" + dateList[xi] + "'", conn).ExecuteReader();
                                            if (reader.Read())
                                            {
                                                haldDayReprt = "";
                                                if (reader.GetBoolean(17))
                                                {
                                                    addDay_ = "1";
                                                    TotaladddAY_++;
                                                }
                                                else
                                                {
                                                    addDay_ = "";
                                                }
                                                if (reader.GetBoolean(16))
                                                {
                                                    addHDay_ = "1";
                                                    totalAddHalfDay_++;
                                                }
                                                else
                                                {
                                                    addHDay_ = "";
                                                }
                                                if (reader.GetInt32(13) == 1)
                                                {
                                                    hDay_ = "";
                                                    totalAnnual_++;
                                                }
                                                else if (reader.GetInt32(13) == 2)
                                                {
                                                    hDay_ = "";
                                                    totalCAshual_++;
                                                }
                                                else if (reader.GetInt32(13) == 3)
                                                {
                                                    hDay_ = "";
                                                    totalSick_++;
                                                }
                                                else if (reader.GetInt32(13) == 4)
                                                {
                                                    hDay_ = "1";
                                                    totalHalfy_++;
                                                }
                                                else if (reader.GetBoolean(15))
                                                {
                                                    hDay_ = "";
                                                    totalHalfy_ = totalHalfy_ + 0.5;
                                                }
                                                else
                                                {
                                                    hDay_ = "";
                                                }


                                                totalOTNOrmal_ = totalOTNOrmal_ + TimeSpan.Parse(reader.GetTimeSpan(10) + "");
                                                //  totalOtSunday_ = totalOtSunday_ + TimeSpan.Parse(reader.GetTimeSpan(20) + reader.GetTimeSpan(21) + "");
                                                TotalExtraOt_ = TotalExtraOt_ + reader.GetTimeSpan(22);
                                                totalLate_ = totalLate_ + reader.GetTimeSpan(6);
                                                //  MessageBox.Show(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0]+"");
                                                value.Add(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0] + dayteType + "/" + reader[2] + "/" + reader[4] + "/" + TimeSpan.Parse(reader.GetTimeSpan(18) + reader.GetTimeSpan(19) + "") + "/" + reader.GetTimeSpan(10) + "/" + reader[8] + "/" + reader[9] + "/" + reader[10] + "/" + reader[6] + "/" + hDay_);
                                            }
                                            else
                                            {
                                                // value.Add(xi + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/");
                                                value.Add(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0] + dayteType + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "");

                                            }
                                            conn.Close();
                                        }
                                        conn.Close();
                                        if (totalAnnual_ != 0)
                                        {
                                            haldDayReprt = "ANNUAL - " + totalAnnual_;
                                        }
                                        if (totalCAshual_ != 0)
                                        {
                                            if (haldDayReprt.Equals(""))
                                            {
                                                haldDayReprt = "CASHUAL - " + totalCAshual_;

                                            }
                                            else
                                            {
                                                haldDayReprt = haldDayReprt + " / CASHUAL - " + totalCAshual_;

                                            }
                                        }
                                        if (totalSick_ != 0)
                                        {
                                            if (haldDayReprt.Equals(""))
                                            {
                                                haldDayReprt = "SICK - " + totalSick_;

                                            }
                                            else
                                            {
                                                haldDayReprt = haldDayReprt + " / SICK - " + totalSick_;

                                            }
                                        }
                                        if (totalHalfy_ != 0)
                                        {
                                            if (haldDayReprt.Equals(""))
                                            {
                                                haldDayReprt = "NO PAY - " + totalHalfy_;
                                            }
                                            else
                                            {
                                                haldDayReprt = haldDayReprt + " / NO PAY - " + totalHalfy_;
                                            }

                                        }
                                        // MessageBox.Show(value.Count+"");
                                        if (value.Count == 31)
                                        {
                                            dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], value[29].ToString().Split('/')[0], value[29].ToString().Split('/')[1], value[29].ToString().Split('/')[2], value[29].ToString().Split('/')[3], value[29].ToString().Split('/')[4], value[29].ToString().Split('/')[5], value[29].ToString().Split('/')[6], value[29].ToString().Split('/')[7], value[29].ToString().Split('/')[8], value[29].ToString().Split('/')[9], value[30].ToString().Split('/')[0], value[30].ToString().Split('/')[1], value[30].ToString().Split('/')[2], value[30].ToString().Split('/')[3], value[30].ToString().Split('/')[4], value[30].ToString().Split('/')[5], value[30].ToString().Split('/')[6], value[30].ToString().Split('/')[7], value[30].ToString().Split('/')[8], value[30].ToString().Split('/')[9], (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                                        }
                                        else if (value.Count == 30)
                                        {
                                            dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], value[29].ToString().Split('/')[0], value[29].ToString().Split('/')[1], value[29].ToString().Split('/')[2], value[29].ToString().Split('/')[3], value[29].ToString().Split('/')[4], value[29].ToString().Split('/')[5], value[29].ToString().Split('/')[6], value[29].ToString().Split('/')[7], value[29].ToString().Split('/')[8], value[29].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                                        }
                                        else if (value.Count == 29)
                                        {
                                            dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                                        }
                                        else
                                        {

                                            // dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);
                                            dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                                        }
                                    }
                                }

                            }
                        }
                        else if (list_search.SelectedIndex >= 1)
                        {
                            foreach (Tuple<int, int> yearMonth in temp_yearmonthlist)
                            {
                                var period = yearMonth.Item1 + "/" + db.getMOnthName(yearMonth.Item2.ToString());
                                var year = yearMonth.Item1.ToString();
                                var month = db.getMOnthName(yearMonth.Item2.ToString());

                                try
                                {

                                    conn.Close();
                                    conn2.Open();
                                    if (list_search.SelectedIndex == 1)
                                    {
                                        search_tag = year_from.Text + month_from.Value + "_to_" + year_to.Text + month_to.Value + "_allemployee";
                                        reader2 = new SqlCommand("select b.id,b.epfno from emp as b,empbackup as d where  b.tempepfno=d.epfno and b.resgin='" + false + "' and d.month='" + period + "'", conn2).ExecuteReader();

                                    }
                                    else
                                    {
                                        search_tag = year_from.Text + month_from.Value + "_to_" + year_to.Text + month_to.Value + "_" + list_search.Text;
                                        reader2 = new SqlCommand("select b.id,b.epfno from emp as b,empbackup as d where  b.tempepfno=d.epfno and b.resgin='" + false + "' and d.month='" + period + "' and d.line='" + list_search.Text + "'", conn2).ExecuteReader();

                                    }

                                    while (reader2.Read())
                                    {
                                        idL = reader2[0] + "";
                                        if (!db.CheckEmployee(reader2[1].ToString(), period, conn, reader))
                                        {
                                            conn.Open();

                                            reader = new SqlCommand("select a.name,b.name,b.epfno,b.line from company as a, emp as b where b.id='" + idL + "' and b.company=a.id", conn).ExecuteReader();
                                            if (reader.Read())
                                            {
                                                reportCompanyName = reader.GetString(0).ToUpper();
                                                reportName = reader.GetString(1).ToUpper();
                                                reportEpfNO = reader[2].ToString();
                                                reportLine = reader.GetString(3).ToUpper();
                                            }
                                            conn.Close();
                                            var dateList = db.getDateList(idL, db.getMOnth(month), year, conn, reader);

                                            {
                                                var dayteType = "";
                                                totalOTNOrmal_ = TimeSpan.Parse("00:00"); totalOtSunday_ = TimeSpan.Parse("00:00"); TotalExtraOt_ = TimeSpan.Parse("00:00"); totalLate_ = TimeSpan.Parse("00:00");
                                                TotaladddAY_ = 0; totalAddHalfDay_ = 0; totalHalfy_ = 0;
                                                totalAnnual_ = 0;
                                                totalCAshual_ = 0;
                                                totalSick_ = 0;


                                                ArrayList value = new ArrayList();
                                                for (int xi = 0; xi < dateList.Count; xi++)
                                                {
                                                    dayteType = "";
                                                    tempDate = xi;
                                                    tempDate++;
                                                    conn.Open();
                                                    reader = new SqlCommand("select ispay,isPoya from calendar where date='" + Convert.ToDateTime(dateList[xi]) + "'", conn).ExecuteReader();
                                                    if (reader.Read())
                                                    {
                                                        if (!reader.GetBoolean(0))
                                                        {
                                                            if (reader.GetBoolean(1))
                                                            {
                                                                dayteType = "-P";
                                                            }
                                                            else
                                                            {
                                                                dayteType = "-H";
                                                            }

                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToDateTime(dateList[xi]).DayOfWeek == DayOfWeek.Sunday)
                                                            {
                                                                dayteType = "-S";
                                                            }

                                                        }
                                                    }
                                                    conn.Close();
                                                    conn.Open();
                                                    reader = new SqlCommand("select * from timesheet where empid_1='" + idL + "' and inDate_3='" + dateList[xi] + "'", conn).ExecuteReader();
                                                    if (reader.Read())
                                                    {
                                                        haldDayReprt = "";
                                                        if (reader.GetBoolean(17))
                                                        {
                                                            addDay_ = "1";
                                                            TotaladddAY_++;
                                                        }
                                                        else
                                                        {
                                                            addDay_ = "";
                                                        }
                                                        if (reader.GetBoolean(16))
                                                        {
                                                            addHDay_ = "1";
                                                            totalAddHalfDay_++;
                                                        }
                                                        else
                                                        {
                                                            addHDay_ = "";
                                                        }
                                                        if (reader.GetInt32(13) == 1)
                                                        {
                                                            hDay_ = "";
                                                            totalAnnual_++;
                                                        }
                                                        else if (reader.GetInt32(13) == 2)
                                                        {
                                                            hDay_ = "";
                                                            totalCAshual_++;
                                                        }
                                                        else if (reader.GetInt32(13) == 3)
                                                        {
                                                            hDay_ = "";
                                                            totalSick_++;
                                                        }
                                                        else if (reader.GetInt32(13) == 4)
                                                        {
                                                            hDay_ = "1";
                                                            totalHalfy_++;
                                                        }
                                                        else if (reader.GetBoolean(15))
                                                        {
                                                            hDay_ = "";
                                                            totalHalfy_ = totalHalfy_ + 0.5;
                                                        }
                                                        else
                                                        {
                                                            hDay_ = "";
                                                        }


                                                        totalOTNOrmal_ = totalOTNOrmal_ + TimeSpan.Parse(reader.GetTimeSpan(10) + "");
                                                        //  totalOtSunday_ = totalOtSunday_ + TimeSpan.Parse(reader.GetTimeSpan(20) + reader.GetTimeSpan(21) + "");
                                                        TotalExtraOt_ = TotalExtraOt_ + reader.GetTimeSpan(22);
                                                        totalLate_ = totalLate_ + reader.GetTimeSpan(6);
                                                        //  MessageBox.Show(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0]+"");
                                                        value.Add(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0] + dayteType + "/" + reader[2] + "/" + reader[4] + "/" + TimeSpan.Parse(reader.GetTimeSpan(18) + reader.GetTimeSpan(19) + "") + "/" + reader.GetTimeSpan(10) + "/" + reader[8] + "/" + reader[9] + "/" + reader[10] + "/" + reader[6] + "/" + hDay_);
                                                    }
                                                    else
                                                    {
                                                        // value.Add(xi + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/");
                                                        value.Add(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0] + dayteType + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "");

                                                    }
                                                    conn.Close();
                                                }
                                                conn.Close();
                                                if (totalAnnual_ != 0)
                                                {
                                                    haldDayReprt = "ANNUAL - " + totalAnnual_;
                                                }
                                                if (totalCAshual_ != 0)
                                                {
                                                    if (haldDayReprt.Equals(""))
                                                    {
                                                        haldDayReprt = "CASHUAL - " + totalCAshual_;

                                                    }
                                                    else
                                                    {
                                                        haldDayReprt = haldDayReprt + " / CASHUAL - " + totalCAshual_;

                                                    }
                                                }
                                                if (totalSick_ != 0)
                                                {
                                                    if (haldDayReprt.Equals(""))
                                                    {
                                                        haldDayReprt = "SICK - " + totalSick_;

                                                    }
                                                    else
                                                    {
                                                        haldDayReprt = haldDayReprt + " / SICK - " + totalSick_;

                                                    }
                                                }
                                                if (totalHalfy_ != 0)
                                                {
                                                    if (haldDayReprt.Equals(""))
                                                    {
                                                        haldDayReprt = "NO PAY - " + totalHalfy_;
                                                    }
                                                    else
                                                    {
                                                        haldDayReprt = haldDayReprt + " / NO PAY - " + totalHalfy_;
                                                    }

                                                }
                                                // MessageBox.Show(value.Count+"");
                                                if (value.Count == 31)
                                                {
                                                    dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], value[29].ToString().Split('/')[0], value[29].ToString().Split('/')[1], value[29].ToString().Split('/')[2], value[29].ToString().Split('/')[3], value[29].ToString().Split('/')[4], value[29].ToString().Split('/')[5], value[29].ToString().Split('/')[6], value[29].ToString().Split('/')[7], value[29].ToString().Split('/')[8], value[29].ToString().Split('/')[9], value[30].ToString().Split('/')[0], value[30].ToString().Split('/')[1], value[30].ToString().Split('/')[2], value[30].ToString().Split('/')[3], value[30].ToString().Split('/')[4], value[30].ToString().Split('/')[5], value[30].ToString().Split('/')[6], value[30].ToString().Split('/')[7], value[30].ToString().Split('/')[8], value[30].ToString().Split('/')[9], (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                                                }
                                                else if (value.Count == 30)
                                                {
                                                    dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], value[29].ToString().Split('/')[0], value[29].ToString().Split('/')[1], value[29].ToString().Split('/')[2], value[29].ToString().Split('/')[3], value[29].ToString().Split('/')[4], value[29].ToString().Split('/')[5], value[29].ToString().Split('/')[6], value[29].ToString().Split('/')[7], value[29].ToString().Split('/')[8], value[29].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                                                }
                                                else if (value.Count == 29)
                                                {
                                                    dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                                                }
                                                else
                                                {

                                                    // dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);
                                                    dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                                                }
                                            }
                                        }



                                    }
                                    conn2.Close();
                                }
                                catch (Exception a)
                                {
                                }
                            }
                        }

                        ds.Tables.Add(dt);
                        payslip2AAApparal2 paySheet = new payslip2AAApparal2();
                        paySheet.SetDataSource(ds);
                        ExportOptions CrExportOptions;
                        DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                        PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();

                        CrDiskFileDestinationOptions.DiskFileName = dir + "TimeSheet_AdvanceSearch" + "_" + search_tag + ".pdf";
                        CrExportOptions = paySheet.ExportOptions;
                        {
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        }
                        paySheet.Export();

                        string filePath = dir + "TimeSheet_AdvanceSearch" + "_" + search_tag + ".pdf";
                        FileInfo file = new FileInfo(filePath);

                        paySheet.Close();
                        paySheet.Dispose();
                        paySheet = null;
                        ds.Clear();
                        ds.Dispose();
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = "text/plain";
                        Response.Flush();
                        Response.TransmitFile(file.FullName);
                        Response.End();
                    }
                    

                }
                catch (Exception aa)
                {
                    var tt = aa.Message;
                }
            }
        }

        //string A_, AA_, AB_, AC_, AD_, AE_, AF_, AG_, AH_, AI_, B_, BA_, BB_, BC_, BD_, BE_, BF_, BG_, BH_, BI_, C_, CA_, CB_, CC_, CD_, CE_, CF_, CG_, CH_, CI_, D_, DA_, DB_, DC_, DE_, DF_, DG_, DH_, DI_, E_, EA_, EB_, EC_, ED_, EF_, EG_, EH_, EI_, F_, FA_, FB_, FC_, FD_, FE_, FF_, FG_, FH_, FI_, G_, GA_, GB_, GC_, GD_, GE_, GF_, GG_, GH_, GI, H, HA, HB, HC, HD, HE, HF, HG, HH, HI, I, IA, IB, IC, ID, IF, IG, IH, II, J, JA, JB, JC, JD, JE, JF, JG, JH, JI, K, KA, KB, KC, KD, KE, KF, KG, KH, KI, L, LA, LB, LC, LD, LE,LF,LG, LH, LI, M, MA, MB, MC, MD, ME, MF, MG, MH, MI, N, NA, NB, NC, ND, NE, NF, NG, NH, NI, O, OA, OB, OC, OD, OE, OF, OG, OH, OI;
        TimeSpan totalOTNOrmal_, totalOtSunday_, TotalExtraOt_, totalLate_;
        Int32 TotaladddAY_, totalAddHalfDay_, tempDate;
        double totalHalfy_, totalAnnual_, totalCAshual_, totalSick_;
        public void btn_print_Click(object sender, EventArgs e)
        {
            var epfNo = 0;
            var idL = "";
            var year = date_search.Text;
            var month = list_month.Value;
            db = new db();
            try
            {
                var dir = new DirectoryInfo(Server.MapPath("/PDF/"));
                try
                {
                    epfNo = Int32.Parse(text_epfNo_.Value);
                }
                catch (Exception)
                {
                }
                if (db.CheckEmployee(epfNo.ToString(), date_search.Text + "/" + list_month.Value,conn,reader))
                {
                    Response.Write("<script>alert('Sorry, Selected User Processing Salary above period........')</script>");
                }
                else
                {
                    conn.Open();
                    reader = new SqlCommand("select id from emp where epfno='" + epfNo + "'", conn).ExecuteReader();
                    if (reader.Read())
                    {
                        idL = reader[0] + "";
                    }
                    conn.Close();
                    db = new db();
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    {
                        dt.Columns.Add("CompanyName", typeof(string));
                        dt.Columns.Add("name", typeof(string));
                        dt.Columns.Add("epfNo", typeof(string));
                        dt.Columns.Add("line", typeof(string));
                        dt.Columns.Add("id", typeof(string));
                        dt.Columns.Add("month", typeof(string));
                        dt.Columns.Add("1", typeof(string));
                        dt.Columns.Add("1A", typeof(string));
                        dt.Columns.Add("1B", typeof(string));
                        dt.Columns.Add("1C", typeof(string));
                        dt.Columns.Add("1D", typeof(string));
                        dt.Columns.Add("1E", typeof(string));
                        dt.Columns.Add("1F", typeof(string));
                        dt.Columns.Add("1G", typeof(string));
                        dt.Columns.Add("1H", typeof(string));
                        dt.Columns.Add("1I", typeof(string));
                        dt.Columns.Add("2", typeof(string));
                        dt.Columns.Add("2A", typeof(string));
                        dt.Columns.Add("2B", typeof(string));
                        dt.Columns.Add("2C", typeof(string));
                        dt.Columns.Add("2D", typeof(string));
                        dt.Columns.Add("2E", typeof(string));
                        dt.Columns.Add("2F", typeof(string));
                        dt.Columns.Add("2G", typeof(string));
                        dt.Columns.Add("2H", typeof(string));
                        dt.Columns.Add("2I", typeof(string));
                        dt.Columns.Add("3", typeof(string));
                        dt.Columns.Add("3A", typeof(string));
                        dt.Columns.Add("3B", typeof(string));
                        dt.Columns.Add("3C", typeof(string));
                        dt.Columns.Add("3D", typeof(string));
                        dt.Columns.Add("3E", typeof(string));
                        dt.Columns.Add("3F", typeof(string));
                        dt.Columns.Add("3G", typeof(string));
                        dt.Columns.Add("3H", typeof(string));
                        dt.Columns.Add("3I", typeof(string));
                        dt.Columns.Add("4", typeof(string));
                        dt.Columns.Add("4A", typeof(string));
                        dt.Columns.Add("4B", typeof(string));
                        dt.Columns.Add("4C", typeof(string));
                        dt.Columns.Add("4D", typeof(string));
                        dt.Columns.Add("4E", typeof(string));
                        dt.Columns.Add("4F", typeof(string));
                        dt.Columns.Add("4G", typeof(string));
                        dt.Columns.Add("4H", typeof(string));
                        dt.Columns.Add("4I", typeof(string));
                        dt.Columns.Add("5", typeof(string));
                        dt.Columns.Add("5A", typeof(string));
                        dt.Columns.Add("5B", typeof(string));
                        dt.Columns.Add("5C", typeof(string));
                        dt.Columns.Add("5D", typeof(string));
                        dt.Columns.Add("5E", typeof(string));
                        dt.Columns.Add("5F", typeof(string));
                        dt.Columns.Add("5G", typeof(string));
                        dt.Columns.Add("5H", typeof(string));
                        dt.Columns.Add("5I", typeof(string));
                        dt.Columns.Add("6", typeof(string));
                        dt.Columns.Add("6A", typeof(string));
                        dt.Columns.Add("6B", typeof(string));
                        dt.Columns.Add("6C", typeof(string));
                        dt.Columns.Add("6D", typeof(string));
                        dt.Columns.Add("6E", typeof(string));
                        dt.Columns.Add("6F", typeof(string));
                        dt.Columns.Add("6G", typeof(string));
                        dt.Columns.Add("6H", typeof(string));
                        dt.Columns.Add("6I", typeof(string));
                        dt.Columns.Add("7", typeof(string));
                        dt.Columns.Add("7A", typeof(string));
                        dt.Columns.Add("7B", typeof(string));
                        dt.Columns.Add("7C", typeof(string));
                        dt.Columns.Add("7D", typeof(string));
                        dt.Columns.Add("7E", typeof(string));
                        dt.Columns.Add("7F", typeof(string));
                        dt.Columns.Add("7G", typeof(string));
                        dt.Columns.Add("7H", typeof(string));
                        dt.Columns.Add("7I", typeof(string));
                        dt.Columns.Add("8", typeof(string));
                        dt.Columns.Add("8A", typeof(string));
                        dt.Columns.Add("8B", typeof(string));
                        dt.Columns.Add("8C", typeof(string));
                        dt.Columns.Add("8D", typeof(string));
                        dt.Columns.Add("8E", typeof(string));
                        dt.Columns.Add("8F", typeof(string));
                        dt.Columns.Add("8G", typeof(string));
                        dt.Columns.Add("8H", typeof(string));
                        dt.Columns.Add("8I", typeof(string));
                        dt.Columns.Add("9", typeof(string));
                        dt.Columns.Add("9A", typeof(string));
                        dt.Columns.Add("9B", typeof(string));
                        dt.Columns.Add("9C", typeof(string));
                        dt.Columns.Add("9D", typeof(string));
                        dt.Columns.Add("9E", typeof(string));
                        dt.Columns.Add("9F", typeof(string));
                        dt.Columns.Add("9G", typeof(string));
                        dt.Columns.Add("9H", typeof(string));
                        dt.Columns.Add("9I", typeof(string));
                        dt.Columns.Add("10", typeof(string));
                        dt.Columns.Add("10A", typeof(string));
                        dt.Columns.Add("10B", typeof(string));
                        dt.Columns.Add("10C", typeof(string));
                        dt.Columns.Add("10D", typeof(string));
                        dt.Columns.Add("10E", typeof(string));
                        dt.Columns.Add("10F", typeof(string));
                        dt.Columns.Add("10G", typeof(string));
                        dt.Columns.Add("10H", typeof(string));
                        dt.Columns.Add("10I", typeof(string));
                        dt.Columns.Add("11", typeof(string));
                        dt.Columns.Add("11A", typeof(string));
                        dt.Columns.Add("11B", typeof(string));
                        dt.Columns.Add("11C", typeof(string));
                        dt.Columns.Add("11D", typeof(string));
                        dt.Columns.Add("11E", typeof(string));
                        dt.Columns.Add("11F", typeof(string));
                        dt.Columns.Add("11G", typeof(string));
                        dt.Columns.Add("11H", typeof(string));
                        dt.Columns.Add("11I", typeof(string));
                        dt.Columns.Add("12", typeof(string));
                        dt.Columns.Add("12A", typeof(string));
                        dt.Columns.Add("12B", typeof(string));
                        dt.Columns.Add("12C", typeof(string));
                        dt.Columns.Add("12D", typeof(string));
                        dt.Columns.Add("12E", typeof(string));
                        dt.Columns.Add("12F", typeof(string));
                        dt.Columns.Add("12G", typeof(string));
                        dt.Columns.Add("12H", typeof(string));
                        dt.Columns.Add("12I", typeof(string));
                        dt.Columns.Add("13", typeof(string));
                        dt.Columns.Add("13A", typeof(string));
                        dt.Columns.Add("13B", typeof(string));
                        dt.Columns.Add("13C", typeof(string));
                        dt.Columns.Add("13D", typeof(string));
                        dt.Columns.Add("13E", typeof(string));
                        dt.Columns.Add("13F", typeof(string));
                        dt.Columns.Add("13G", typeof(string));
                        dt.Columns.Add("13H", typeof(string));
                        dt.Columns.Add("13I", typeof(string));
                        dt.Columns.Add("14", typeof(string));
                        dt.Columns.Add("14A", typeof(string));
                        dt.Columns.Add("14B", typeof(string));
                        dt.Columns.Add("14C", typeof(string));
                        dt.Columns.Add("14D", typeof(string));
                        dt.Columns.Add("14E", typeof(string));
                        dt.Columns.Add("14F", typeof(string));
                        dt.Columns.Add("14G", typeof(string));
                        dt.Columns.Add("14H", typeof(string));
                        dt.Columns.Add("14I", typeof(string));
                        dt.Columns.Add("15", typeof(string));
                        dt.Columns.Add("15A", typeof(string));
                        dt.Columns.Add("15B", typeof(string));
                        dt.Columns.Add("15C", typeof(string));
                        dt.Columns.Add("15D", typeof(string));
                        dt.Columns.Add("15E", typeof(string));
                        dt.Columns.Add("15F", typeof(string));
                        dt.Columns.Add("15G", typeof(string));
                        dt.Columns.Add("15H", typeof(string));
                        dt.Columns.Add("15I", typeof(string));
                        dt.Columns.Add("16", typeof(string));
                        dt.Columns.Add("16A", typeof(string));
                        dt.Columns.Add("16B", typeof(string));
                        dt.Columns.Add("16C", typeof(string));
                        dt.Columns.Add("16D", typeof(string));
                        dt.Columns.Add("16E", typeof(string));
                        dt.Columns.Add("16F", typeof(string));
                        dt.Columns.Add("16G", typeof(string));
                        dt.Columns.Add("16H", typeof(string));
                        dt.Columns.Add("16I", typeof(string));
                        dt.Columns.Add("17", typeof(string));
                        dt.Columns.Add("17A", typeof(string));
                        dt.Columns.Add("17B", typeof(string));
                        dt.Columns.Add("17C", typeof(string));
                        dt.Columns.Add("17D", typeof(string));
                        dt.Columns.Add("17E", typeof(string));
                        dt.Columns.Add("17F", typeof(string));
                        dt.Columns.Add("17G", typeof(string));
                        dt.Columns.Add("17H", typeof(string));
                        dt.Columns.Add("17I", typeof(string));
                        dt.Columns.Add("18", typeof(string));
                        dt.Columns.Add("18A", typeof(string));
                        dt.Columns.Add("18B", typeof(string));
                        dt.Columns.Add("18C", typeof(string));
                        dt.Columns.Add("18D", typeof(string));
                        dt.Columns.Add("18E", typeof(string));
                        dt.Columns.Add("18F", typeof(string));
                        dt.Columns.Add("18G", typeof(string));
                        dt.Columns.Add("18H", typeof(string));
                        dt.Columns.Add("18I", typeof(string));
                        dt.Columns.Add("19", typeof(string));
                        dt.Columns.Add("19A", typeof(string));
                        dt.Columns.Add("19B", typeof(string));
                        dt.Columns.Add("19C", typeof(string));
                        dt.Columns.Add("19D", typeof(string));
                        dt.Columns.Add("19E", typeof(string));
                        dt.Columns.Add("19F", typeof(string));
                        dt.Columns.Add("19G", typeof(string));
                        dt.Columns.Add("19H", typeof(string));
                        dt.Columns.Add("19I", typeof(string));
                        dt.Columns.Add("20", typeof(string));
                        dt.Columns.Add("20A", typeof(string));
                        dt.Columns.Add("20B", typeof(string));
                        dt.Columns.Add("20C", typeof(string));
                        dt.Columns.Add("20D", typeof(string));
                        dt.Columns.Add("20E", typeof(string));
                        dt.Columns.Add("20F", typeof(string));
                        dt.Columns.Add("20G", typeof(string));
                        dt.Columns.Add("20H", typeof(string));
                        dt.Columns.Add("20I", typeof(string));
                        dt.Columns.Add("21", typeof(string));
                        dt.Columns.Add("21A", typeof(string));
                        dt.Columns.Add("21B", typeof(string));
                        dt.Columns.Add("21C", typeof(string));
                        dt.Columns.Add("21D", typeof(string));
                        dt.Columns.Add("21E", typeof(string));
                        dt.Columns.Add("21F", typeof(string));
                        dt.Columns.Add("21G", typeof(string));
                        dt.Columns.Add("21H", typeof(string));
                        dt.Columns.Add("21I", typeof(string));
                        dt.Columns.Add("22", typeof(string));
                        dt.Columns.Add("22A", typeof(string));
                        dt.Columns.Add("22B", typeof(string));
                        dt.Columns.Add("22C", typeof(string));
                        dt.Columns.Add("22D", typeof(string));
                        dt.Columns.Add("22E", typeof(string));
                        dt.Columns.Add("22F", typeof(string));
                        dt.Columns.Add("22G", typeof(string));
                        dt.Columns.Add("22H", typeof(string));
                        dt.Columns.Add("22I", typeof(string));
                        dt.Columns.Add("23", typeof(string));
                        dt.Columns.Add("23A", typeof(string));
                        dt.Columns.Add("23B", typeof(string));
                        dt.Columns.Add("23C", typeof(string));
                        dt.Columns.Add("23D", typeof(string));
                        dt.Columns.Add("23E", typeof(string));
                        dt.Columns.Add("23F", typeof(string));
                        dt.Columns.Add("23G", typeof(string));
                        dt.Columns.Add("23H", typeof(string));
                        dt.Columns.Add("23I", typeof(string));
                        dt.Columns.Add("24", typeof(string));
                        dt.Columns.Add("24A", typeof(string));
                        dt.Columns.Add("24B", typeof(string));
                        dt.Columns.Add("24C", typeof(string));
                        dt.Columns.Add("24D", typeof(string));
                        dt.Columns.Add("24E", typeof(string));
                        dt.Columns.Add("24F", typeof(string));
                        dt.Columns.Add("24G", typeof(string));
                        dt.Columns.Add("24H", typeof(string));
                        dt.Columns.Add("24I", typeof(string));
                        dt.Columns.Add("25", typeof(string));
                        dt.Columns.Add("25A", typeof(string));
                        dt.Columns.Add("25B", typeof(string));
                        dt.Columns.Add("25C", typeof(string));
                        dt.Columns.Add("25D", typeof(string));
                        dt.Columns.Add("25E", typeof(string));
                        dt.Columns.Add("25F", typeof(string));
                        dt.Columns.Add("25G", typeof(string));
                        dt.Columns.Add("25H", typeof(string));
                        dt.Columns.Add("25I", typeof(string));
                        dt.Columns.Add("26", typeof(string));
                        dt.Columns.Add("26A", typeof(string));
                        dt.Columns.Add("26B", typeof(string));
                        dt.Columns.Add("26C", typeof(string));
                        dt.Columns.Add("26D", typeof(string));
                        dt.Columns.Add("26E", typeof(string));
                        dt.Columns.Add("26F", typeof(string));
                        dt.Columns.Add("26G", typeof(string));
                        dt.Columns.Add("26H", typeof(string));
                        dt.Columns.Add("26I", typeof(string));
                        dt.Columns.Add("27", typeof(string));
                        dt.Columns.Add("27A", typeof(string));
                        dt.Columns.Add("27B", typeof(string));
                        dt.Columns.Add("27C", typeof(string));
                        dt.Columns.Add("27D", typeof(string));
                        dt.Columns.Add("27E", typeof(string));
                        dt.Columns.Add("27F", typeof(string));
                        dt.Columns.Add("27G", typeof(string));
                        dt.Columns.Add("27H", typeof(string));
                        dt.Columns.Add("27I", typeof(string));
                        dt.Columns.Add("28", typeof(string));
                        dt.Columns.Add("28A", typeof(string));
                        dt.Columns.Add("28B", typeof(string));
                        dt.Columns.Add("28C", typeof(string));
                        dt.Columns.Add("28D", typeof(string));
                        dt.Columns.Add("28E", typeof(string));
                        dt.Columns.Add("28F", typeof(string));
                        dt.Columns.Add("28G", typeof(string));
                        dt.Columns.Add("28H", typeof(string));
                        dt.Columns.Add("28I", typeof(string));
                        dt.Columns.Add("29", typeof(string));
                        dt.Columns.Add("29A", typeof(string));
                        dt.Columns.Add("29B", typeof(string));
                        dt.Columns.Add("29C", typeof(string));
                        dt.Columns.Add("29D", typeof(string));
                        dt.Columns.Add("29E", typeof(string));
                        dt.Columns.Add("29F", typeof(string));
                        dt.Columns.Add("29G", typeof(string));
                        dt.Columns.Add("29H", typeof(string));
                        dt.Columns.Add("29I", typeof(string));
                        dt.Columns.Add("30", typeof(string));
                        dt.Columns.Add("30A", typeof(string));
                        dt.Columns.Add("30B", typeof(string));
                        dt.Columns.Add("30C", typeof(string));
                        dt.Columns.Add("30D", typeof(string));
                        dt.Columns.Add("30E", typeof(string));
                        dt.Columns.Add("30F", typeof(string));
                        dt.Columns.Add("30G", typeof(string));
                        dt.Columns.Add("30H", typeof(string));
                        dt.Columns.Add("30I", typeof(string));
                        dt.Columns.Add("31", typeof(string));
                        dt.Columns.Add("31A", typeof(string));
                        dt.Columns.Add("31B", typeof(string));
                        dt.Columns.Add("31C", typeof(string));
                        dt.Columns.Add("31D", typeof(string));
                        dt.Columns.Add("31E", typeof(string));
                        dt.Columns.Add("31F", typeof(string));
                        dt.Columns.Add("31G", typeof(string));
                        dt.Columns.Add("31H", typeof(string));
                        dt.Columns.Add("31I", typeof(string));
                        dt.Columns.Add("TC", typeof(string));
                        dt.Columns.Add("TD", typeof(string));
                        dt.Columns.Add("TE", typeof(string));
                        dt.Columns.Add("TF", typeof(string));
                        dt.Columns.Add("TG", typeof(string));
                        dt.Columns.Add("TH", typeof(string));
                        dt.Columns.Add("TI", typeof(string));
                    }
                    var dayteType = "";
                    totalOTNOrmal_ = TimeSpan.Parse("00:00"); totalOtSunday_ = TimeSpan.Parse("00:00"); TotalExtraOt_ = TimeSpan.Parse("00:00"); totalLate_ = TimeSpan.Parse("00:00");
                    TotaladddAY_ = 0; totalAddHalfDay_ = 0; totalHalfy_ = 0;
                    totalAnnual_ = 0;
                    totalCAshual_ = 0;
                    totalSick_ = 0;
                    conn.Open();

                    reader = new SqlCommand("select a.name,b.name,b.epfno,b.line from company as a, emp as b where b.id='" + idL + "' and b.company=a.id", conn).ExecuteReader();
                    if (reader.Read())
                    {
                        reportCompanyName = reader.GetString(0).ToUpper();
                        reportName = reader.GetString(1).ToUpper();
                        reportEpfNO = reader[2].ToString();
                        reportLine = reader.GetString(3).ToUpper();
                    }
                    conn.Close();
                    var dateList = db.getDateList(idL, db.getMOnth(month), year, conn, reader);
                    ArrayList value = new ArrayList();
                    //       MessageBox.Show(dateList.Count+"");
                    for (int xi = 0; xi < dateList.Count; xi++)
                    {
                        dayteType = "";
                        tempDate = xi;
                        tempDate++;
                        conn.Open();
                        reader = new SqlCommand("select ispay,isPoya from calendar where date='" + Convert.ToDateTime(dateList[xi]) + "'", conn).ExecuteReader();
                        if (reader.Read())
                        {
                            if (!reader.GetBoolean(0))
                            {
                                if (reader.GetBoolean(1))
                                {
                                    dayteType = "-P";
                                }
                                else
                                {
                                    dayteType = "-H";
                                }

                            }
                            else
                            {
                                if (Convert.ToDateTime(dateList[xi]).DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dayteType = "-S";
                                }

                            }
                        }
                        conn.Close();
                        conn.Open();
                        reader = new SqlCommand("select * from timesheet where empid_1='" + idL + "' and inDate_3='" + dateList[xi] + "'", conn).ExecuteReader();
                        if (reader.Read())
                        {
                            haldDayReprt = "";
                            if (reader.GetBoolean(17))
                            {
                                addDay_ = "1";
                                TotaladddAY_++;
                            }
                            else
                            {
                                addDay_ = "";
                            }
                            if (reader.GetBoolean(16))
                            {
                                addHDay_ = "1";
                                totalAddHalfDay_++;
                            }
                            else
                            {
                                addHDay_ = "";
                            }
                            if (reader.GetInt32(13) == 1)
                            {
                                hDay_ = "";
                                totalAnnual_++;
                            }
                            else if (reader.GetInt32(13) == 2)
                            {
                                hDay_ = "";
                                totalCAshual_++;
                            }
                            else if (reader.GetInt32(13) == 3)
                            {
                                hDay_ = "";
                                totalSick_++;
                            }
                            else if (reader.GetInt32(13) == 4)
                            {
                                hDay_ = "1";
                                totalHalfy_++;
                            }
                            else if (reader.GetBoolean(15))
                            {
                                hDay_ = "";
                                totalHalfy_ = totalHalfy_ + 0.5;
                            }
                            else
                            {
                                hDay_ = "";
                            }


                            totalOTNOrmal_ = totalOTNOrmal_ + TimeSpan.Parse(reader.GetTimeSpan(10) + "");
                            //  totalOtSunday_ = totalOtSunday_ + TimeSpan.Parse(reader.GetTimeSpan(20) + reader.GetTimeSpan(21) + "");
                            TotalExtraOt_ = TotalExtraOt_ + reader.GetTimeSpan(22);
                            totalLate_ = totalLate_ + reader.GetTimeSpan(6);
                            //  MessageBox.Show(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0]+"");
                            value.Add(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0] + dayteType + "/" + reader[2] + "/" + reader[4] + "/" + TimeSpan.Parse(reader.GetTimeSpan(18) + reader.GetTimeSpan(19) + "") + "/" + reader.GetTimeSpan(10) + "/" + reader[8] + "/" + reader[9] + "/" + reader[10] + "/" + reader[6] + "/" + hDay_);
                        }
                        else
                        {
                            // value.Add(xi + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/" + "/");
                            value.Add(Convert.ToDateTime(dateList[xi]).ToString("dd-MM-yyyy").Split('-')[0] + dayteType + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "00:00:00" + "/" + "");

                        }
                        conn.Close();
                    }
                    conn.Close();
                    if (totalAnnual_ != 0)
                    {
                        haldDayReprt = "ANNUAL - " + totalAnnual_;
                    }
                    if (totalCAshual_ != 0)
                    {
                        if (haldDayReprt.Equals(""))
                        {
                            haldDayReprt = "CASHUAL - " + totalCAshual_;

                        }
                        else
                        {
                            haldDayReprt = haldDayReprt + " / CASHUAL - " + totalCAshual_;

                        }
                    }
                    if (totalSick_ != 0)
                    {
                        if (haldDayReprt.Equals(""))
                        {
                            haldDayReprt = "SICK - " + totalSick_;

                        }
                        else
                        {
                            haldDayReprt = haldDayReprt + " / SICK - " + totalSick_;

                        }
                    }
                    if (totalHalfy_ != 0)
                    {
                        if (haldDayReprt.Equals(""))
                        {
                            haldDayReprt = "NO PAY - " + totalHalfy_;
                        }
                        else
                        {
                            haldDayReprt = haldDayReprt + " / NO PAY - " + totalHalfy_;
                        }

                    }
                    // MessageBox.Show(value.Count+"");
                    if (value.Count == 31)
                    {
                        dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], value[29].ToString().Split('/')[0], value[29].ToString().Split('/')[1], value[29].ToString().Split('/')[2], value[29].ToString().Split('/')[3], value[29].ToString().Split('/')[4], value[29].ToString().Split('/')[5], value[29].ToString().Split('/')[6], value[29].ToString().Split('/')[7], value[29].ToString().Split('/')[8], value[29].ToString().Split('/')[9], value[30].ToString().Split('/')[0], value[30].ToString().Split('/')[1], value[30].ToString().Split('/')[2], value[30].ToString().Split('/')[3], value[30].ToString().Split('/')[4], value[30].ToString().Split('/')[5], value[30].ToString().Split('/')[6], value[30].ToString().Split('/')[7], value[30].ToString().Split('/')[8], value[30].ToString().Split('/')[9], (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                    }
                    else if (value.Count == 30)
                    {
                        dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], value[29].ToString().Split('/')[0], value[29].ToString().Split('/')[1], value[29].ToString().Split('/')[2], value[29].ToString().Split('/')[3], value[29].ToString().Split('/')[4], value[29].ToString().Split('/')[5], value[29].ToString().Split('/')[6], value[29].ToString().Split('/')[7], value[29].ToString().Split('/')[8], value[29].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                    }
                    else if (value.Count == 29)
                    {
                        dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                    }
                    else
                    {

                        // dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], value[28].ToString().Split('/')[0], value[28].ToString().Split('/')[1], value[28].ToString().Split('/')[2], value[28].ToString().Split('/')[3], value[28].ToString().Split('/')[4], value[28].ToString().Split('/')[5], value[28].ToString().Split('/')[6], value[28].ToString().Split('/')[7], value[28].ToString().Split('/')[8], value[28].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);
                        dt.Rows.Add(reportCompanyName, reportName, reportEpfNO, reportLine, reportEpfNO, year + "/" + month.ToUpper(), value[0].ToString().Split('/')[0], value[0].ToString().Split('/')[1], value[0].ToString().Split('/')[2], value[0].ToString().Split('/')[3], value[0].ToString().Split('/')[4], value[0].ToString().Split('/')[5], value[0].ToString().Split('/')[6], value[0].ToString().Split('/')[7], value[0].ToString().Split('/')[8], value[0].ToString().Split('/')[9], value[1].ToString().Split('/')[0], value[1].ToString().Split('/')[1], value[1].ToString().Split('/')[2], value[1].ToString().Split('/')[3], value[1].ToString().Split('/')[4], value[1].ToString().Split('/')[5], value[1].ToString().Split('/')[6], value[1].ToString().Split('/')[7], value[1].ToString().Split('/')[8], value[1].ToString().Split('/')[9], value[2].ToString().Split('/')[0], value[2].ToString().Split('/')[1], value[2].ToString().Split('/')[2], value[2].ToString().Split('/')[3], value[2].ToString().Split('/')[4], value[2].ToString().Split('/')[5], value[2].ToString().Split('/')[6], value[2].ToString().Split('/')[7], value[2].ToString().Split('/')[8], value[2].ToString().Split('/')[9], value[3].ToString().Split('/')[0], value[3].ToString().Split('/')[1], value[3].ToString().Split('/')[2], value[3].ToString().Split('/')[3], value[3].ToString().Split('/')[4], value[3].ToString().Split('/')[5], value[3].ToString().Split('/')[6], value[3].ToString().Split('/')[7], value[3].ToString().Split('/')[8], value[3].ToString().Split('/')[9], value[4].ToString().Split('/')[0], value[4].ToString().Split('/')[1], value[4].ToString().Split('/')[2], value[4].ToString().Split('/')[3], value[4].ToString().Split('/')[4], value[4].ToString().Split('/')[5], value[4].ToString().Split('/')[6], value[4].ToString().Split('/')[7], value[4].ToString().Split('/')[8], value[4].ToString().Split('/')[9], value[5].ToString().Split('/')[0], value[5].ToString().Split('/')[1], value[5].ToString().Split('/')[2], value[5].ToString().Split('/')[3], value[5].ToString().Split('/')[4], value[5].ToString().Split('/')[5], value[5].ToString().Split('/')[6], value[5].ToString().Split('/')[7], value[5].ToString().Split('/')[8], value[5].ToString().Split('/')[9], value[6].ToString().Split('/')[0], value[6].ToString().Split('/')[1], value[6].ToString().Split('/')[2], value[6].ToString().Split('/')[3], value[6].ToString().Split('/')[4], value[6].ToString().Split('/')[5], value[6].ToString().Split('/')[6], value[6].ToString().Split('/')[7], value[6].ToString().Split('/')[8], value[6].ToString().Split('/')[9], value[7].ToString().Split('/')[0], value[7].ToString().Split('/')[1], value[7].ToString().Split('/')[2], value[7].ToString().Split('/')[3], value[7].ToString().Split('/')[4], value[7].ToString().Split('/')[5], value[7].ToString().Split('/')[6], value[7].ToString().Split('/')[7], value[7].ToString().Split('/')[8], value[7].ToString().Split('/')[9], value[8].ToString().Split('/')[0], value[8].ToString().Split('/')[1], value[8].ToString().Split('/')[2], value[8].ToString().Split('/')[3], value[8].ToString().Split('/')[4], value[8].ToString().Split('/')[5], value[8].ToString().Split('/')[6], value[8].ToString().Split('/')[7], value[8].ToString().Split('/')[8], value[8].ToString().Split('/')[9], value[9].ToString().Split('/')[0], value[9].ToString().Split('/')[1], value[9].ToString().Split('/')[2], value[9].ToString().Split('/')[3], value[9].ToString().Split('/')[4], value[9].ToString().Split('/')[5], value[9].ToString().Split('/')[6], value[9].ToString().Split('/')[7], value[9].ToString().Split('/')[8], value[9].ToString().Split('/')[9], value[10].ToString().Split('/')[0], value[10].ToString().Split('/')[1], value[10].ToString().Split('/')[2], value[10].ToString().Split('/')[3], value[10].ToString().Split('/')[4], value[10].ToString().Split('/')[5], value[10].ToString().Split('/')[6], value[10].ToString().Split('/')[7], value[10].ToString().Split('/')[8], value[10].ToString().Split('/')[9], value[11].ToString().Split('/')[0], value[11].ToString().Split('/')[1], value[11].ToString().Split('/')[2], value[11].ToString().Split('/')[3], value[11].ToString().Split('/')[4], value[11].ToString().Split('/')[5], value[11].ToString().Split('/')[6], value[11].ToString().Split('/')[7], value[11].ToString().Split('/')[8], value[11].ToString().Split('/')[9], value[12].ToString().Split('/')[0], value[12].ToString().Split('/')[1], value[12].ToString().Split('/')[2], value[12].ToString().Split('/')[3], value[12].ToString().Split('/')[4], value[12].ToString().Split('/')[5], value[12].ToString().Split('/')[6], value[12].ToString().Split('/')[7], value[12].ToString().Split('/')[8], value[12].ToString().Split('/')[9], value[13].ToString().Split('/')[0], value[13].ToString().Split('/')[1], value[13].ToString().Split('/')[2], value[13].ToString().Split('/')[3], value[13].ToString().Split('/')[4], value[13].ToString().Split('/')[5], value[13].ToString().Split('/')[6], value[13].ToString().Split('/')[7], value[13].ToString().Split('/')[8], value[13].ToString().Split('/')[9], value[14].ToString().Split('/')[0], value[14].ToString().Split('/')[1], value[14].ToString().Split('/')[2], value[14].ToString().Split('/')[3], value[14].ToString().Split('/')[4], value[14].ToString().Split('/')[5], value[14].ToString().Split('/')[6], value[14].ToString().Split('/')[7], value[14].ToString().Split('/')[8], value[14].ToString().Split('/')[9], value[15].ToString().Split('/')[0], value[15].ToString().Split('/')[1], value[15].ToString().Split('/')[2], value[15].ToString().Split('/')[3], value[15].ToString().Split('/')[4], value[15].ToString().Split('/')[5], value[15].ToString().Split('/')[6], value[15].ToString().Split('/')[7], value[15].ToString().Split('/')[8], value[15].ToString().Split('/')[9], value[16].ToString().Split('/')[0], value[16].ToString().Split('/')[1], value[16].ToString().Split('/')[2], value[16].ToString().Split('/')[3], value[16].ToString().Split('/')[4], value[16].ToString().Split('/')[5], value[16].ToString().Split('/')[6], value[16].ToString().Split('/')[7], value[16].ToString().Split('/')[8], value[16].ToString().Split('/')[9], value[17].ToString().Split('/')[0], value[17].ToString().Split('/')[1], value[17].ToString().Split('/')[2], value[17].ToString().Split('/')[3], value[17].ToString().Split('/')[4], value[17].ToString().Split('/')[5], value[17].ToString().Split('/')[6], value[17].ToString().Split('/')[7], value[17].ToString().Split('/')[8], value[17].ToString().Split('/')[9], value[18].ToString().Split('/')[0], value[18].ToString().Split('/')[1], value[18].ToString().Split('/')[2], value[18].ToString().Split('/')[3], value[18].ToString().Split('/')[4], value[18].ToString().Split('/')[5], value[18].ToString().Split('/')[6], value[18].ToString().Split('/')[7], value[18].ToString().Split('/')[8], value[18].ToString().Split('/')[9], value[19].ToString().Split('/')[0], value[19].ToString().Split('/')[1], value[19].ToString().Split('/')[2], value[19].ToString().Split('/')[3], value[19].ToString().Split('/')[4], value[19].ToString().Split('/')[5], value[19].ToString().Split('/')[6], value[19].ToString().Split('/')[7], value[19].ToString().Split('/')[8], value[19].ToString().Split('/')[9], value[20].ToString().Split('/')[0], value[20].ToString().Split('/')[1], value[20].ToString().Split('/')[2], value[20].ToString().Split('/')[3], value[20].ToString().Split('/')[4], value[20].ToString().Split('/')[5], value[20].ToString().Split('/')[6], value[20].ToString().Split('/')[7], value[20].ToString().Split('/')[8], value[20].ToString().Split('/')[9], value[21].ToString().Split('/')[0], value[21].ToString().Split('/')[1], value[21].ToString().Split('/')[2], value[21].ToString().Split('/')[3], value[21].ToString().Split('/')[4], value[21].ToString().Split('/')[5], value[21].ToString().Split('/')[6], value[21].ToString().Split('/')[7], value[21].ToString().Split('/')[8], value[21].ToString().Split('/')[9], value[22].ToString().Split('/')[0], value[22].ToString().Split('/')[1], value[22].ToString().Split('/')[2], value[22].ToString().Split('/')[3], value[22].ToString().Split('/')[4], value[22].ToString().Split('/')[5], value[22].ToString().Split('/')[6], value[22].ToString().Split('/')[7], value[22].ToString().Split('/')[8], value[22].ToString().Split('/')[9], value[23].ToString().Split('/')[0], value[23].ToString().Split('/')[1], value[23].ToString().Split('/')[2], value[23].ToString().Split('/')[3], value[23].ToString().Split('/')[4], value[23].ToString().Split('/')[5], value[23].ToString().Split('/')[6], value[23].ToString().Split('/')[7], value[23].ToString().Split('/')[8], value[23].ToString().Split('/')[9], value[24].ToString().Split('/')[0], value[24].ToString().Split('/')[1], value[24].ToString().Split('/')[2], value[24].ToString().Split('/')[3], value[24].ToString().Split('/')[4], value[24].ToString().Split('/')[5], value[24].ToString().Split('/')[6], value[24].ToString().Split('/')[7], value[24].ToString().Split('/')[8], value[24].ToString().Split('/')[9], value[25].ToString().Split('/')[0], value[25].ToString().Split('/')[1], value[25].ToString().Split('/')[2], value[25].ToString().Split('/')[3], value[25].ToString().Split('/')[4], value[25].ToString().Split('/')[5], value[25].ToString().Split('/')[6], value[25].ToString().Split('/')[7], value[25].ToString().Split('/')[8], value[25].ToString().Split('/')[9], value[26].ToString().Split('/')[0], value[26].ToString().Split('/')[1], value[26].ToString().Split('/')[2], value[26].ToString().Split('/')[3], value[26].ToString().Split('/')[4], value[26].ToString().Split('/')[5], value[26].ToString().Split('/')[6], value[26].ToString().Split('/')[7], value[26].ToString().Split('/')[8], value[26].ToString().Split('/')[9], value[27].ToString().Split('/')[0], value[27].ToString().Split('/')[1], value[27].ToString().Split('/')[2], value[27].ToString().Split('/')[3], value[27].ToString().Split('/')[4], value[27].ToString().Split('/')[5], value[27].ToString().Split('/')[6], value[27].ToString().Split('/')[7], value[27].ToString().Split('/')[8], value[27].ToString().Split('/')[9], "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", (int)(totalOTNOrmal_.TotalMinutes / 60) + " HOURS : " + (int)(totalOTNOrmal_.TotalMinutes % 60) + " MINUTE", (int)(totalOtSunday_.TotalMinutes / 60) + " HOURS : " + (int)(totalOtSunday_.TotalMinutes % 60) + " MINUTE", TotaladddAY_, totalAddHalfDay_, (int)(TotalExtraOt_.TotalMinutes / 60) + " HOURS : " + (int)(TotalExtraOt_.TotalMinutes % 60) + " MINUTE", (int)(totalLate_.TotalMinutes / 60) + " HOURS : " + (int)(totalLate_.TotalMinutes % 60) + " MINUTE", haldDayReprt);

                    }

                    ds.Tables.Add(dt);
                    payslip2AAApparal2 paySheet = new payslip2AAApparal2();
                    paySheet.SetDataSource(ds);

                    CrystalDecisions.Shared.ExportOptions CrExportOptions;
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();

                    var down = DateTime.Now.ToString("yyyyMMss HHmmss");
                    CrDiskFileDestinationOptions.DiskFileName = dir + date_search.Text + "_" + list_month.Value + "_TimeSheet_" + epfNo + ".pdf";
                    CrExportOptions = paySheet.ExportOptions;
                    {
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    }
                    paySheet.Export();

                    string filePath = dir + date_search.Text + "_" + list_month.Value + "_TimeSheet_" + epfNo + ".pdf";
                    FileInfo file = new FileInfo(filePath);

                    paySheet.Close();
                    paySheet.Dispose();
                    paySheet = null;
                    ds.Clear();
                    ds.Dispose();

                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "text/plain";
                    Response.Flush();
                    Response.TransmitFile(file.FullName);
                    Response.End();

                }

            }
            catch (Exception a)
            {
                conn.Close();
                var tt = a.Message;
            }

        }

       
    }

    public class DataModel
    {
        public string InDate { get; set; }
        public string ShiftOneInTime { get; set; }
        public string ShiftOneOutTime { get; set; }
        public string ShiftTwoInTime { get; set; }
        public string ShiftTwoOutTime { get; set; }
        public string OutDate { get; set; }
        public string PayCut { get; set; }
        public string OverTime15 { get; set; }
        public string ExtraOT { get; set; }
        public string WorkHours { get; set; }
        public string NoPay { get; set; }

        public static implicit operator DataModel(HttpResponseMessage v)
        {
            throw new NotImplementedException();
        }
    }
}