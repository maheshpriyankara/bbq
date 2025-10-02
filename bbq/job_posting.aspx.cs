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
    public partial class job_posting : System.Web.UI.Page
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
        protected void ValidateFileExtension(object source, ServerValidateEventArgs args)
        {

        }

        DataSet ds;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                db = new db();
                date_applicationdeadline.Text = db.getSysDateTime().ToString("MM/dd/yyyy");
                list_jobcategory.Items.Add("All Remote Jobs");
                list_jobtype_.Items.Add("All Job Types");
                try
                {
                    conn_job.Open();
                    reader = new SqlCommand("select * from job_Category order by name", conn_job).ExecuteReader();
                    while (reader.Read())
                    {
                        list_jobcategory.Items.Add(reader[0] + "");
                    }
                    conn_job.Close();


                }
                catch (Exception)
                {
                    conn.Close();
                }

                try
                {

                }
                catch (Exception)
                {

                    throw;
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
            try
            {
                if (text_jobtitle.Text.Equals(""))
                {
                    string script = "alert('Please Input Job Title to Publish');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
                }
                else if (text_description.Value.Equals(""))
                {
                    string script = "alert('Please Input Job Description to Publish');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
                }
                else
                {
                    db = new db();
                    conn_job.Open();
                    new SqlCommand("insert into job_published values ('" + false + "','" + 0 + "','" + list_jobcategory.SelectedItem + "','" + list_jobtype_.SelectedItem + "','" + "" + "','" + "" + "','" + text_jobtitle.Text + "','" + "Leesons Hospital Pvt Ltd" + "','" + "No 33 Tewatta Road Ragama" + "','" + "" + "','" + "N/A" + "','" + "Monthly" + "', '" + "N/A" + "','" + check_fulltime.Checked + "','" + check_parttime.Checked + "','" + list_jobtype.Value + "','" + "" + "','" + "" + "','" + db.getSysDateTime() + "','" + text_jobtitle.Text + "','" + "" + "','" + "" + "','" + "" + "','" + date_applicationdeadline.Text + "','" + text_email.Text + "','" + text_contact.Text + "','" + true + "')", conn_job).ExecuteNonQuery();

                    conn_job.Close();

                    var maxID = 0;
                    try
                    {
                        conn_job.Open();
                        reader = new SqlCommand("select max(id) from job_published", conn_job).ExecuteReader();
                        if (reader.Read())
                        {
                            maxID = reader.GetInt32(0);
                        }
                        conn_job.Close();
                    }
                    catch (Exception)
                    {
                        conn_job.Close();
                    }

                    conn_job.Open();
                    new SqlCommand("insert into about_company values ('" + maxID + "','" + "Leesons Hospital is multi-speciality tertiary care hospital in Sri Lanka and is one of the largest private hospitals in the country." + "')", conn_job).ExecuteNonQuery();
                    new SqlCommand("insert into about_company values ('" + maxID + "','" + "That is a subsidiary of Sri Lanka Insurance Corporation. The companys previously owned by Apollo Hospitals." + "')", conn_job).ExecuteNonQuery();

                    conn_job.Close();
                    try
                    {

                        if (!text_description.Value.ToString().Equals(""))
                        {
                            var value = text_description.Value.ToString().Split(',');
                            conn_job.Open();
                            for (int i = 0; i < value.Length; i++)
                            {
                                new SqlCommand("insert into responsibilities values ('" + maxID + "','" + value[i] + "')", conn_job).ExecuteNonQuery();

                            }
                            conn_job.Close();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {

                        if (!text_qulification.Value.ToString().Equals(""))
                        {
                            var value = text_qulification.Value.ToString().Split(',');
                            conn_job.Open();
                            for (int i = 0; i < value.Length; i++)
                            {
                                new SqlCommand("insert into requirements values ('" + maxID + "','" + value[i] + "')", conn_job).ExecuteNonQuery();

                            }
                            conn_job.Close();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {

                        if (!text_weoffer.Value.ToString().Equals(""))
                        {
                            var value = text_weoffer.Value.ToString().Split(',');
                            conn_job.Open();
                            for (int i = 0; i < value.Length; i++)
                            {
                                new SqlCommand("insert into we_offer values ('" + maxID + "','" + value[i] + "')", conn_job).ExecuteNonQuery();

                            }
                            conn_job.Close();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        if (FileUpload.HasFile)
                        {
                            FileUpload.SaveAs(HttpContext.Current.Server.MapPath("~/upload/upload_") + maxID + ".jpeg");
                           
                        }
                    }
                    catch (Exception aa)
                    {
                        var tt = aa.Message;
                    }
                   

                    string script = "alert('New Job listed Successfully');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script, true);
                    // Response.Write("<script>alert('New Job listed Successfully')</script>");
                    text_jobtitle.Text = string.Empty;
                    list_jobcategory.SelectedIndex = 0;
                    list_jobtype_.SelectedIndex = 0;
                    check_fulltime.Checked = false;
                    check_parttime.Checked = false;
                    text_description.Value = string.Empty;
                    list_jobtype.SelectedIndex = 0;
                    text_qulification.Value = string.Empty;
                    text_weoffer.Value = string.Empty;
                    date_applicationdeadline.Text = db.getSysDateTime().ToString();
                    text_email.Text = string.Empty;
                    text_contact.Text = string.Empty;
                    script = "<script type='text/javascript'>yourJavaScriptMethod();</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "loadJobList", script);
                }
            }
            catch (Exception ee)
            {
                var aaaa = ee.Message;
                conn_job.Close();
            }
        }

        protected void list_jobcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            list_jobtype_.Items.Clear();
            list_jobtype_.Items.Add("All Job Types");
            if (list_jobcategory.SelectedIndex != 0)
            {
                try
                {
                    conn_job.Open();
                    reader = new SqlCommand("select type from job_type where category='" + list_jobcategory.SelectedItem + "' order by type", conn_job).ExecuteReader();
                    while (reader.Read())
                    {
                        list_jobtype_.Items.Add(reader[0] + "");
                    }
                    conn_job.Close();
                }
                catch (Exception)
                {
                    conn_job.Close();
                }
            }
        }

        protected void btn_JobOpen_Click(object sender, EventArgs e)
        {

        }
    }
}