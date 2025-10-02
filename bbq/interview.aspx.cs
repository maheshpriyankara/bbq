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
    public partial class interview : System.Web.UI.Page
    {

        static SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        static SqlConnection conn2 = new SqlConnection(
   WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        static SqlConnection conn_job = new SqlConnection(
   WebConfigurationManager.ConnectionStrings["conn_job"].ConnectionString);
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

            Session["applicantID"] = "";
            if (Request.QueryString["applicantID"] == null)
            {
                panel_assgin.Visible = false;
            }
            else
            {
                Session["applicantID"] = Request.QueryString["applicantID"];
            }
            if (IsPostBack != true)
            {
                db = new db();
                var date_ = db.getSysDateTime();
                date_applied.Text = date_.ToString("yyyy-MM-dd");
                date_deadline.Text = date_.ToString("yyyy-MM-dd");
                date_assigninterview.Text = date_.ToString("yyyy-MM-dd");
                try
                {
                    var tt = Session["applicantID"];
                    conn_job.Open();
                    reader = new SqlCommand("select a.job_title,a.job_category,b.applicant_name,a.date_expire,b.applied,b.select_,b.rejetc_,b.applicant_contact from job_published as a,job_applicant as b where b.id='" + Session["applicantID"] + "' and b.job_id=a.id", conn_job).ExecuteReader();
                    if (reader.Read())
                    {
                        text_jobtitle.Text = reader.GetString(0);
                        text_department.Text = reader.GetString(1);
                        text_applicantname.Text = reader.GetString(2);
                        date_deadline.Text = reader.GetDateTime(3).ToString("yyyy-MM-dd");
                        date_applied.Text = reader.GetDateTime(4).ToString("yyyy-MM-dd");
                        Session["applicantContact"] = reader[5] + "";
                    }
                    conn_job.Close();
                }
                catch (Exception)
                {
                    conn_job.Close();
                }
            }






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

        protected void btn_save_Click1(object sender, EventArgs e)
        {

        }

        protected void btn_save_Click2(object sender, EventArgs e)
        {
            db = new db();
            try
            {
                if (DateTime.Parse(date_assigninterview.Text).Date <= db.getSysDateTime().Date)
                {
                    string script = "alert('Please Select Valied Date to Assign to Interview');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
                }
                else if (TimeSpan.Parse(time_assigninterview.Text).TotalMinutes == 0)
                {
                    string script = "alert('Please Input Valied Interview Time');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
                }
                else if (Session["applicantID"] == null)
                {
                    string script = "alert('Sorry Applicant Loading Error. Please Go to Applicant Dashboard and select again');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
                }
                else
                {
                    var conatct = "";
                    conn_job.Open();
                    reader = new SqlCommand("select applicant_contact from job_applicant where id='"+ Session["applicantID"] + "'", conn_job).ExecuteReader();
                    if (reader.Read())
                    {
                        conatct = reader[0] + "";
                    }
                    conn_job.Close();

                    conn_job.Open();
                    new SqlCommand("insert into interview values ('" + Session["applicantID"] + "','" + date_assigninterview.Text + "','" + time_assigninterview.Text + "','" + date_assigninterview.Text + " " + time_assigninterview.Text + "','" + db.getSysDateTime() + "','false','false','')", conn_job).ExecuteNonQuery();
                    conn_job.Close();
                    conn_job.Open();
                    new SqlCommand("update job_applicant set check_='true',select_='true' where id='" + Session["applicantID"] + "'", conn_job).ExecuteNonQuery();
                    conn_job.Close();
                    conn.Open();
                    var body = "Dear " + text_applicantname.Text + ", Congratulations, You have been assigned to an interview for the position of " + text_jobtitle.Text + ". The interview is scheduled on " + date_assigninterview.Text + " at " + time_assigninterview.Text + ". Please prepare and make sure to attend the interview on time.";
                    new SqlCommand("insert into sms_queue values('" + Session["applicantID"] + "','" + conatct + "','" + body + "','" + db.getSysDateTime() + "','" + db.GetLocalIPAddress() + "','" + Session["applicantID"] + "','" + false + "','" + false + "','" + db.getSysDateTime() + "','" + db.getSysDateTime() + "','" + "" + "')", conn).ExecuteNonQuery();

                    conn.Close();
                    string script = "alert('" + text_applicantname.Text + " Assigned to Interview Successfully" + "');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
                    panel_assgin.Visible = false;
                }
            }
            catch (Exception ex)
            {
                conn_job.Close();
                conn.Close();
                var sss = ex.Message;
                string script = "alert('Interview Date/ Time Selection Invalied');";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
            }

        }
    }
}