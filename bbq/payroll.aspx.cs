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

namespace bbq
{
    public partial class payroll : System.Web.UI.Page
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
            var result_name = "";
            var result_epfNo = "";
            var result_empNo = "";
            var db_ = new db();


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

            conn.Open();
            reader = new SqlCommand("select a.*,b.name,c.empid,c.line from paysheet as a,emp as b,empbackup as c where a.month_2='" + year + "/" + month + "' and a.empID_1=b.id and b.tempepfno='" + result_epfNo + "' and b.tempepfno=c.epfno  and c.month='" + year + "/" + month + "'", conn).ExecuteReader();
            if (reader.Read())
            {
                var otNormalHours = (int)(reader.GetDouble(9) / 60 / 60) + " HH: " + (int)((reader.GetDouble(9) / 60) % 60) + " MM";
                var extraOtHours = (int)(reader.GetDouble(13) / 60 / 60) + " HH: " + (int)((reader.GetDouble(13) / 60) % 60) + " MM";
                var lateHours = (int)(reader.GetDouble(23) / 60 / 60) + " HH: " + (int)((reader.GetDouble(23) / 60) % 60) + " MM";
                temp = result_epfNo + "_" + reader[52].ToString() + "_" + reader[51].ToString() + "_" + reader[53].ToString() + "_" + reader.GetDouble(7) + " Days" + "_" + reader.GetDouble(8) + " Days" + "_" + db_.setAmountFormat(reader[3] + "") + "_" + db_.setAmountFormat(reader[4] + "") + "_" + db_.setAmountFormat(reader[5] + "") + "_" + db_.setAmountFormat(reader[6] + "") + "_" + db_.setAmountFormat(reader[11] + "") + "_" + otNormalHours + "_" + db_.setAmountFormat(reader[14] + "") + "_" + extraOtHours + "_" + db_.setAmountFormat(reader[16] + "") + "_" + db_.setAmountFormat(reader[19] + "") + "_" + db_.setAmountFormat(reader[17] + "") + "_" + db_.setAmountFormat(reader[18] + "") + "_" + db_.setAmountFormat(reader[36] + "") + "_" + db_.setAmountFormat(reader[32] + "") + "_" + db_.setAmountFormat(reader[30] + "") + "_" + db_.setAmountFormat(reader[20] + "") + "_" + db_.setAmountFormat(reader[31] + "") + "_" + db_.setAmountFormat(reader[47] + "") + "_" + db_.setAmountFormat(reader[48] + "") + "_" + db_.setAmountFormat(reader[49] + "") + "_" + db_.setAmountFormat(reader[50] + "") + "_" + db_.setAmountFormat(reader[40] + "") + "_" + db_.setAmountFormat(reader[21] + "") + "_" + db_.setAmountFormat(reader[24] + "") + "_" + lateHours + "_" + db_.setAmountFormat(reader[25] + "") + "_" + db_.setAmountFormat(reader[29] + "") + "_" + db_.setAmountFormat(reader[26] + "") + "_" + db_.setAmountFormat(reader[46] + "") + "_" + db_.setAmountFormat(reader[27] + "") + "_" + db_.setAmountFormat(reader[33] + "") + "_" + db_.setAmountFormat(reader[34] + "") + "_" + db_.setAmountFormat(reader[35] + "") + "_" + db_.setAmountFormat(reader[45] + "") + "_" + db_.setAmountFormat(reader[38] + "") + "_" + db_.setAmountFormat(reader[22] + "") + "_" + db_.setAmountFormat(reader[39] + "") + "_" + db_.setAmountFormat(reader[41] + "") + "_" + db_.setAmountFormat(reader[43] + "") + "_" + db_.setAmountFormat(reader[44] + "");
            }
            conn.Close();
            return temp;
        }

        protected void Previous_Click(object sender, EventArgs e)
        {
            text_employee.Text = "";
            var epfNo = 0;
            try
            {
                epfNo = Int32.Parse(text_epfNo_.Value);
            }
            catch (Exception)
            {
            }
            try
            {
                db = new db();
                epfNo--;
                if (epfNo > 0)
                {
                    conn.Open();
                    reader = new SqlCommand("select a.*,b.name,d.empid,d.line,b.tempEpfNo from paysheet as a,emp as b,empbackup as d where a.month_2='" + date_search.Text + "/" + list_month.Value + "' and a.empID_1=b.id and b.tempepfno<='" + epfNo + "' and b.resgin='" + false + "' and b.tempepfno=d.epfno and d.month='" + date_search.Text + "/" + list_month.Value + "' and a.id not in (select emp_systemID from process_queue where process_end='" + false + "') order by b.tempepfno ", conn).ExecuteReader();

                    if (reader.Read())
                    {
                        text_epfNo_.Value = reader.GetInt32(54).ToString();
                        text_attendanceId.Value = reader[52].ToString();
                        text_employeeName.Value = reader[51].ToString();
                        text_department.Value = reader[53].ToString();
                        text_workDays.Value = reader[7].ToString() + " Days";
                        text_nopayDays.Value = reader[8].ToString() + " Days";
                        text_basic.Value = db.setAmountFormat(reader[3].ToString());
                        text_budj.Value = db.setAmountFormat(reader[4].ToString());
                        text_totalBasic.Value = db.setAmountFormat(reader[5].ToString());
                        text_salaryForEPF.Value = db.setAmountFormat(reader[6].ToString());
                        text_OT15.Value = db.setAmountFormat(reader[11].ToString());
                        text_OT15Hours.Value = (int)(reader.GetDouble(9) / 60 / 60) + " HH: " + (int)((reader.GetDouble(9) / 60) % 60) + " MM";
                        text_extraOT.Value = db.setAmountFormat(reader[14].ToString());
                        text_extraOTHours.Value = (int)(reader.GetDouble(13) / 60 / 60) + " HH: " + (int)((reader.GetDouble(13) / 60) % 60) + " MM";
                        text_allowICU.Value = db.setAmountFormat(reader[16].ToString());
                        text_allowCases.Value = db.setAmountFormat(reader[19].ToString());
                        text_allowNight.Value = db.setAmountFormat(reader[17].ToString());
                        text_allowFixed.Value = db.setAmountFormat(reader[18].ToString());
                        text_allowMeal.Value = db.setAmountFormat(reader[36].ToString());
                        text_allowSpecial.Value = db.setAmountFormat(reader[32].ToString());
                        text_allowTheater.Value = db.setAmountFormat(reader[30].ToString());
                        text_allowOther.Value = db.setAmountFormat(reader[20].ToString());
                        text_allowPH.Value = db.setAmountFormat(reader[31].ToString());
                        text_allowTransport.Value = db.setAmountFormat(reader[47].ToString());
                        text_allowAccommodation.Value = db.setAmountFormat(reader[48].ToString());
                        text_allowFuel.Value = db.setAmountFormat(reader[49].ToString());
                        text_allowAllowances02.Value = db.setAmountFormat(reader[50].ToString());
                        text_coinBF.Value = db.setAmountFormat(reader[40].ToString());
                        text_totalEarning.Value = db.setAmountFormat(reader[21].ToString());
                        text_payCut.Value = db.setAmountFormat(reader[24].ToString());
                        text_payCutHours.Value = (int)(reader.GetDouble(23) / 60 / 60) + " HH: " + (int)((reader.GetDouble(23) / 60) % 60) + " MM";
                        text_noPay.Value = db.setAmountFormat(reader[25].ToString());
                        text_epf8.Value = db.setAmountFormat(reader[29].ToString());
                        text_deduAdvanced.Value = db.setAmountFormat(reader[26].ToString());
                        text_deduPayee.Value = db.setAmountFormat(reader[46].ToString());
                        text_deduOther.Value = db.setAmountFormat(reader[27].ToString());
                        text_deduRDB.Value = db.setAmountFormat(reader[33].ToString());
                        text_deduStaff.Value = db.setAmountFormat(reader[34].ToString());
                        text_deduCashShort.Value = db.setAmountFormat(reader[35].ToString());
                        text_coinCF.Value = db.setAmountFormat(reader[45].ToString());
                        text_totalDeduction.Value = db.setAmountFormat(reader[38].ToString());
                        text_grossPayment.Value = db.setAmountFormat(reader[22].ToString());
                        text_netSalary.Value = db.setAmountFormat(reader[39].ToString());
                        text_totalPayble.Value = db.setAmountFormat(reader[41].ToString());
                        text_epf12.Value = db.setAmountFormat(reader[43].ToString());
                        text_etf3.Value = db.setAmountFormat(reader[44].ToString());
                    }
                    conn.Close();
                }

            }
            catch (Exception)
            {
                conn.Close();
            }
        }

        protected void Next_Click(object sender, EventArgs e)
        {
            text_employee.Text = "";
            var epfNo = 0;
            try
            {
                epfNo = Int32.Parse(text_epfNo_.Value);
            }
            catch (Exception)
            {
            }
            try
            {
                db = new db();
                epfNo++;
                conn.Open();

                reader = new SqlCommand("select a.*,b.name,d.empid,d.line,b.tempEpfNo from paysheet as a,emp as b,empbackup as d where a.month_2='" + date_search.Text + "/" + list_month.Value + "' and a.empID_1=b.id and b.tempepfno>='" + epfNo + "' and b.resgin='" + false + "' and b.tempepfno=d.epfno and d.month='" + date_search.Text + "/" + list_month.Value + "' and a.id not in (select emp_systemID from process_queue where process_end='" + false + "') order by b.tempepfno ", conn).ExecuteReader();
                if (reader.Read())
                {
                    text_epfNo_.Value = reader.GetInt32(54).ToString();
                    text_attendanceId.Value = reader[52].ToString();
                    text_employeeName.Value = reader[51].ToString();
                    text_department.Value = reader[53].ToString();
                    text_workDays.Value = reader[7].ToString() + " Days";
                    text_nopayDays.Value = reader[8].ToString() + " Days";
                    text_basic.Value = db.setAmountFormat(reader[3].ToString());
                    text_budj.Value = db.setAmountFormat(reader[4].ToString());
                    text_totalBasic.Value = db.setAmountFormat(reader[5].ToString());
                    text_salaryForEPF.Value = db.setAmountFormat(reader[6].ToString());
                    text_OT15.Value = db.setAmountFormat(reader[11].ToString());
                    text_OT15Hours.Value = (int)(reader.GetDouble(9) / 60 / 60) + " HH: " + (int)((reader.GetDouble(9) / 60) % 60) + " MM";
                    text_extraOT.Value = db.setAmountFormat(reader[14].ToString());
                    text_extraOTHours.Value = (int)(reader.GetDouble(13) / 60 / 60) + " HH: " + (int)((reader.GetDouble(13) / 60) % 60) + " MM";
                    text_allowICU.Value = db.setAmountFormat(reader[16].ToString());
                    text_allowCases.Value = db.setAmountFormat(reader[19].ToString());
                    text_allowNight.Value = db.setAmountFormat(reader[17].ToString());
                    text_allowFixed.Value = db.setAmountFormat(reader[18].ToString());
                    text_allowMeal.Value = db.setAmountFormat(reader[36].ToString());
                    text_allowSpecial.Value = db.setAmountFormat(reader[32].ToString());
                    text_allowTheater.Value = db.setAmountFormat(reader[30].ToString());
                    text_allowOther.Value = db.setAmountFormat(reader[20].ToString());
                    text_allowPH.Value = db.setAmountFormat(reader[31].ToString());
                    text_allowTransport.Value = db.setAmountFormat(reader[47].ToString());
                    text_allowAccommodation.Value = db.setAmountFormat(reader[48].ToString());
                    text_allowFuel.Value = db.setAmountFormat(reader[49].ToString());
                    text_allowAllowances02.Value = db.setAmountFormat(reader[50].ToString());
                    text_coinBF.Value = db.setAmountFormat(reader[40].ToString());
                    text_totalEarning.Value = db.setAmountFormat(reader[21].ToString());
                    text_payCut.Value = db.setAmountFormat(reader[24].ToString());
                    text_payCutHours.Value = (int)(reader.GetDouble(23) / 60 / 60) + " HH: " + (int)((reader.GetDouble(23) / 60) % 60) + " MM";
                    text_noPay.Value = db.setAmountFormat(reader[25].ToString());
                    text_epf8.Value = db.setAmountFormat(reader[29].ToString());
                    text_deduAdvanced.Value = db.setAmountFormat(reader[26].ToString());
                    text_deduPayee.Value = db.setAmountFormat(reader[46].ToString());
                    text_deduOther.Value = db.setAmountFormat(reader[27].ToString());
                    text_deduRDB.Value = db.setAmountFormat(reader[33].ToString());
                    text_deduStaff.Value = db.setAmountFormat(reader[34].ToString());
                    text_deduCashShort.Value = db.setAmountFormat(reader[35].ToString());
                    text_coinCF.Value = db.setAmountFormat(reader[45].ToString());
                    text_totalDeduction.Value = db.setAmountFormat(reader[38].ToString());
                    text_grossPayment.Value = db.setAmountFormat(reader[22].ToString());
                    text_netSalary.Value = db.setAmountFormat(reader[39].ToString());
                    text_totalPayble.Value = db.setAmountFormat(reader[41].ToString());
                    text_epf12.Value = db.setAmountFormat(reader[43].ToString());
                    text_etf3.Value = db.setAmountFormat(reader[44].ToString());
                }
                conn.Close();
            }
            catch (Exception aa)
            {
                var bdg = aa.Message;
                conn.Close();
            }
        }

        protected void btn_print_Click(object sender, EventArgs e)
        {
            var epfNo = 0;
            try
            {
                db = new db();
                var dir = new DirectoryInfo(Server.MapPath("/PDF/"));
                try
                {
                    epfNo = Int32.Parse(text_epfNo_.Value);
                }
                catch (Exception)
                {
                }
                if (db.CheckEmployee(epfNo.ToString(), date_search.Text + "/" + list_month.Value, conn, reader))
                {
                    Response.Write("<script>alert('Sorry, Selected User Processing Salary above period........')</script>");
                }
                else
                {
                    db = new db();
                    DataSet ds = new DataSet();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("CompanyName", typeof(string));
                    dt.Columns.Add("month", typeof(string));
                    dt.Columns.Add("epfNo", typeof(int));
                    dt.Columns.Add("name", typeof(string));
                    dt.Columns.Add("line", typeof(string));
                    dt.Columns.Add("Designation", typeof(string));
                    dt.Columns.Add("rate", typeof(string));
                    dt.Columns.Add("otHours", typeof(string));
                    dt.Columns.Add("otHours2", typeof(string));
                    dt.Columns.Add("additionalDay", typeof(string));
                    dt.Columns.Add("extraOtHours", typeof(string));
                    dt.Columns.Add("noPayDays", typeof(string));
                    dt.Columns.Add("lateHours", typeof(string));
                    dt.Columns.Add("basic", typeof(string));
                    dt.Columns.Add("position", typeof(string));
                    dt.Columns.Add("grading", typeof(string));
                    dt.Columns.Add("otherAllowanc", typeof(string));
                    dt.Columns.Add("attendaceAllowance", typeof(string));
                    dt.Columns.Add("otPay", typeof(string));
                    dt.Columns.Add("otPay2", typeof(string));
                    dt.Columns.Add("additionalDayPay", typeof(string));
                    dt.Columns.Add("extraOtPay", typeof(string));
                    dt.Columns.Add("coinBF", typeof(string));
                    dt.Columns.Add("noPay", typeof(string));
                    dt.Columns.Add("payCut", typeof(string));
                    dt.Columns.Add("epf8", typeof(string));
                    dt.Columns.Add("payeStapDuty", typeof(string));
                    dt.Columns.Add("welfareFees", typeof(string));
                    dt.Columns.Add("detathDonation", typeof(string));
                    dt.Columns.Add("welfareLoan", typeof(string));
                    dt.Columns.Add("easyPay1", typeof(string));
                    dt.Columns.Add("easypa2", typeof(string));
                    dt.Columns.Add("easyPay3", typeof(string));
                    dt.Columns.Add("OtherLoan", typeof(string));
                    dt.Columns.Add("salaryAdvacne", typeof(string));
                    dt.Columns.Add("Donetion", typeof(string));
                    dt.Columns.Add("coinC/F", typeof(string));
                    dt.Columns.Add("totalDeductionF", typeof(string));
                    dt.Columns.Add("totalForEPF", typeof(string));
                    dt.Columns.Add("netSalary", typeof(string));
                    dt.Columns.Add("epf12", typeof(string));
                    dt.Columns.Add("epf3", typeof(string));
                    dt.Columns.Add("test1", typeof(string));
                    dt.Columns.Add("test2", typeof(string));
                    string companyName, ot, otSunday, extraOtValue, addDayValue, coinsBF, pay, basicSa, totalBasic, salaryForEpf, budj
                    , gradeAll, Position, attendanceAllowance, otherAllowance, totalEarnigs, grossPayment, lateTime, offDayDeduct,
                    advancedValue, otherDeduction, loanText, epf_8, welfareMember, welfareDead, totalDeductionValue,
                    netSalaryValue, totalPayble, epf_12, etf_3, line, desgination, empID, rate, otHoursCount, OtHours2Count, lateHoursCount, ExtraHoursCount, addDAyCount,
                    payeStampDuty, EasyPay1, easyPay2, easyPay3, welfareLoan, donation, coinCF, workingDays, name, absentDays, month;
                    rate = "";
                    var idL = "";
                    var period = date_search.Text + "/" + list_month.Value;


                    conn.Open();
                    reader = new SqlCommand("select id from emp where epfno='" + epfNo + "'", conn).ExecuteReader();
                    if (reader.Read())
                    {
                        idL = reader[0] + "";
                    }
                    conn.Close();
                    conn.Open();
                    reader = new SqlCommand("select a.*,c.name,b.desgination,d.line,b.name,d.empid,b.tempepfNo from paysheet as a,emp as b,company as c,empbackup as d where  a.empID_1=b.id and b.id='" + idL + "' and a.month_2='" + period + "' and b.tempepfno=d.epfno  and d.month='" + period + "'", conn).ExecuteReader();
                    if (reader.Read())
                    {
                        conn2.Open();
                        reader2 = new SqlCommand("select a.* from shiftValueRates as a, emp as b where b.id='" + idL + "' and b.jobCategory=a.name_0", conn2).ExecuteReader();
                        if (reader2.Read())
                        {
                            rate = db.setAmountFormat(((reader.GetDouble(3) + reader.GetDouble(4)) / reader2.GetDouble(5)) * reader2.GetDouble(6) + "");

                        }
                        conn2.Close();

                        payeStampDuty = db.setAmountFormat(reader.GetDouble(6) + "");
                        empID = reader[55] + "";
                        name = reader[54] + "";
                        line = reader[53] + "";
                        desgination = reader[52] + "";
                        basicSa = db.setAmountFormat(reader.GetDouble(3) + "");
                        budj = db.setAmountFormat(reader.GetDouble(4) + "");
                        totalBasic = db.setAmountFormat(reader.GetDouble(5) + "");
                        salaryForEpf = db.setAmountFormat(reader.GetDouble(6) + "");
                        workingDays = reader[7].ToString() + " Days";
                        absentDays = reader[8].ToString() + " Days";

                        otHoursCount = (int)((reader.GetDouble(9) + reader.GetDouble(13)) / 60 / 60) + " Hour " + (int)(((reader.GetDouble(9) + reader.GetDouble(13)) / 60) % 60) + "Min";
                        ot = db.setAmountFormat((reader.GetDouble(11) + reader.GetDouble(14)) + "");
                        otSunday = db.setAmountFormat(reader[49] + "");
                        OtHours2Count = (int)(reader.GetDouble(10) / 60 / 60) + "Hour" + (int)((reader.GetDouble(10) / 60) % 60) + "Min";
                        extraOtValue = db.setAmountFormat(reader[50] + "");

                        ExtraHoursCount = db.setAmountFormat(reader[48].ToString());
                        addDayValue = db.setAmountFormat(reader[16] + "");
                        addDAyCount = db.setAmountFormat(reader[47].ToString());
                        gradeAll = db.setAmountFormat(reader[17] + "");
                        Position = db.setAmountFormat(reader[18] + "");

                        attendanceAllowance = db.setAmountFormat(reader[19] + "");
                        otherAllowance = db.setAmountFormat(reader[20] + "");
                        totalEarnigs = db.setAmountFormat(reader[21].ToString());

                        grossPayment = db.setAmountFormat(reader[22].ToString());

                        lateTime = db.setAmountFormat(reader[24] + "");
                        lateHoursCount = (int)(reader.GetDouble(23) / 60 / 60) + "Hour" + (int)((reader.GetDouble(23) / 60) % 60) + "Min";
                        offDayDeduct = db.setAmountFormat(reader[25].ToString());
                        advancedValue = db.setAmountFormat(reader.GetDouble(26) + "");
                        otherDeduction = db.setAmountFormat(reader.GetDouble(27) + "");
                        loanText = db.setAmountFormat(reader.GetDouble(46) + "");
                        epf_8 = db.setAmountFormat(reader.GetDouble(29) + "");
                        welfareMember = db.setAmountFormat(reader.GetDouble(30) + "");
                        welfareDead = db.setAmountFormat(reader.GetDouble(31) + "");

                        payeStampDuty = db.setAmountFormat(reader.GetDouble(32) + "");
                        EasyPay1 = db.setAmountFormat(reader.GetDouble(33) + "");
                        easyPay2 = db.setAmountFormat(reader.GetDouble(34) + "");
                        easyPay3 = db.setAmountFormat(reader.GetDouble(35) + "");
                        welfareLoan = db.setAmountFormat(reader.GetDouble(36) + "");
                        donation = db.setAmountFormat(reader.GetDouble(37) + "");
                        totalDeductionValue = db.setAmountFormat(reader.GetDouble(38) + "");

                        netSalaryValue = db.setAmountFormat(reader.GetDouble(39) + "");
                        coinsBF = db.setAmountFormat(reader.GetDouble(40) + "");
                        totalPayble = db.setAmountFormat(reader.GetDouble(41) + "");
                        pay = db.setAmountFormat(reader.GetDouble(42) + "");
                        epf_12 = db.setAmountFormat(reader.GetDouble(43) + "");
                        etf_3 = db.setAmountFormat(reader.GetDouble(44) + "");
                        coinCF = db.setAmountFormat(reader.GetDouble(45) + "");
                        companyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[51].ToString().ToLower());
                        month = reader[2].ToString();
                        dt.Rows.Add(empID, month, reader[56], name, line, desgination, rate, otHoursCount, budj, addDAyCount, ExtraHoursCount, absentDays, lateHoursCount, basicSa, Position, gradeAll, otherAllowance, attendanceAllowance, ot, otSunday, addDayValue, extraOtValue, coinsBF, offDayDeduct, lateTime, epf_8, payeStampDuty, welfareMember, welfareDead, welfareLoan, EasyPay1, easyPay2, easyPay3, loanText, advancedValue, otherDeduction, coinCF, totalDeductionValue, salaryForEpf, netSalaryValue, epf_12, etf_3, grossPayment, pay);
                    }


                    ds.Tables.Add(dt);
                    payslip2 paySheet = new payslip2();
                    paySheet.SetDataSource(ds);

                    ExportOptions CrExportOptions;
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();

                    var down = DateTime.Now.ToString("yyyyMMss HHmmss");
                    CrDiskFileDestinationOptions.DiskFileName = dir + date_search.Text + "_" + list_month.Value + "_Payslip_" + epfNo + ".pdf";
                    CrExportOptions = paySheet.ExportOptions;
                    {
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    }
                    paySheet.Export();

                    string filePath = dir + date_search.Text + "_" + list_month.Value + "_Payslip_" + epfNo + ".pdf";
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
            }
        }

        protected void export_paysheetAdvance_Click(object sender, EventArgs e)
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
                    DataTable dt = new DataTable();
                    dt.Columns.Add("CompanyName", typeof(string));
                    dt.Columns.Add("month", typeof(string));
                    dt.Columns.Add("epfNo", typeof(int));
                    dt.Columns.Add("name", typeof(string));
                    dt.Columns.Add("line", typeof(string));
                    dt.Columns.Add("Designation", typeof(string));
                    dt.Columns.Add("rate", typeof(string));
                    dt.Columns.Add("otHours", typeof(string));
                    dt.Columns.Add("otHours2", typeof(string));
                    dt.Columns.Add("additionalDay", typeof(string));
                    dt.Columns.Add("extraOtHours", typeof(string));
                    dt.Columns.Add("noPayDays", typeof(string));
                    dt.Columns.Add("lateHours", typeof(string));
                    dt.Columns.Add("basic", typeof(string));
                    dt.Columns.Add("position", typeof(string));
                    dt.Columns.Add("grading", typeof(string));
                    dt.Columns.Add("otherAllowanc", typeof(string));
                    dt.Columns.Add("attendaceAllowance", typeof(string));
                    dt.Columns.Add("otPay", typeof(string));
                    dt.Columns.Add("otPay2", typeof(string));
                    dt.Columns.Add("additionalDayPay", typeof(string));
                    dt.Columns.Add("extraOtPay", typeof(string));
                    dt.Columns.Add("coinBF", typeof(string));
                    dt.Columns.Add("noPay", typeof(string));
                    dt.Columns.Add("payCut", typeof(string));
                    dt.Columns.Add("epf8", typeof(string));
                    dt.Columns.Add("payeStapDuty", typeof(string));
                    dt.Columns.Add("welfareFees", typeof(string));
                    dt.Columns.Add("detathDonation", typeof(string));
                    dt.Columns.Add("welfareLoan", typeof(string));
                    dt.Columns.Add("easyPay1", typeof(string));
                    dt.Columns.Add("easypa2", typeof(string));
                    dt.Columns.Add("easyPay3", typeof(string));
                    dt.Columns.Add("OtherLoan", typeof(string));
                    dt.Columns.Add("salaryAdvacne", typeof(string));
                    dt.Columns.Add("Donetion", typeof(string));
                    dt.Columns.Add("coinC/F", typeof(string));
                    dt.Columns.Add("totalDeductionF", typeof(string));
                    dt.Columns.Add("totalForEPF", typeof(string));
                    dt.Columns.Add("netSalary", typeof(string));
                    dt.Columns.Add("epf12", typeof(string));
                    dt.Columns.Add("epf3", typeof(string));
                    dt.Columns.Add("test1", typeof(string));
                    dt.Columns.Add("test2", typeof(string));
                    string companyName, ot, otSunday, extraOtValue, addDayValue, coinsBF, pay, basicSa, totalBasic, salaryForEpf, budj
                    , gradeAll, Position, attendanceAllowance, otherAllowance, totalEarnigs, grossPayment, lateTime, offDayDeduct,
                    advancedValue, otherDeduction, loanText, epf_8, welfareMember, welfareDead, totalDeductionValue,
                    netSalaryValue, totalPayble, epf_12, etf_3, line, desgination, empID, rate, otHoursCount, OtHours2Count, lateHoursCount, ExtraHoursCount, addDAyCount,
                    payeStampDuty, EasyPay1, easyPay2, easyPay3, welfareLoan, donation, coinCF, workingDays, name, absentDays, month;
                    rate = "";
                    var idL = "";
                    var check_ = false;
                    var epfNo = 0;
                    try
                    {
                        epfNo = Int32.Parse(text_epfNo_.Value);
                    }
                    catch (Exception)
                    {
                    }
                    conn2.Close();
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

                            conn.Close();
                            conn.Open();
                            reader = new SqlCommand("select id from emp where epfno='" + epfNo + "'", conn).ExecuteReader();
                            if (reader.Read())
                            {
                                idL = reader[0] + "";
                            }
                            conn.Close();
                            foreach (Tuple<int, int> yearMonth in temp_yearmonthlist)
                            {
                                var period = yearMonth.Item1 + "/" + db.getMOnthName(yearMonth.Item2.ToString());
                                if (!db.CheckEmployee(epfNo.ToString(), period, conn, reader))
                                {

                                    try
                                    {

                                        conn.Close();
                                        conn.Open();
                                        reader = new SqlCommand("select a.*,c.name,b.desgination,d.line,b.name,d.empid,b.tempepfNo from paysheet as a,emp as b,company as c,empbackup as d where  a.empID_1=b.id and b.id='" + idL + "' and a.month_2='" + period + "' and b.tempepfno=d.epfno  and d.month='" + period + "'", conn).ExecuteReader();
                                        if (reader.Read())
                                        {
                                            conn2.Open();
                                            reader2 = new SqlCommand("select a.* from shiftValueRates as a, emp as b where b.id='" + idL + "' and b.jobCategory=a.name_0", conn2).ExecuteReader();
                                            if (reader2.Read())
                                            {
                                                rate = db.setAmountFormat(((reader.GetDouble(3) + reader.GetDouble(4)) / reader2.GetDouble(5)) * reader2.GetDouble(6) + "");

                                            }
                                            conn2.Close();

                                            payeStampDuty = db.setAmountFormat(reader.GetDouble(6) + "");
                                            empID = reader[55] + "";
                                            name = reader[54] + "";
                                            line = reader[53] + "";
                                            desgination = reader[52] + "";
                                            basicSa = db.setAmountFormat(reader.GetDouble(3) + "");
                                            budj = db.setAmountFormat(reader.GetDouble(4) + "");
                                            totalBasic = db.setAmountFormat(reader.GetDouble(5) + "");
                                            salaryForEpf = db.setAmountFormat(reader.GetDouble(6) + "");
                                            workingDays = reader[7].ToString() + " Days";
                                            absentDays = reader[8].ToString() + " Days";

                                            otHoursCount = (int)((reader.GetDouble(9) + reader.GetDouble(13)) / 60 / 60) + " Hour " + (int)(((reader.GetDouble(9) + reader.GetDouble(13)) / 60) % 60) + "Min";
                                            ot = db.setAmountFormat((reader.GetDouble(11) + reader.GetDouble(14)) + "");
                                            otSunday = db.setAmountFormat(reader[49] + "");
                                            OtHours2Count = (int)(reader.GetDouble(10) / 60 / 60) + "Hour" + (int)((reader.GetDouble(10) / 60) % 60) + "Min";
                                            extraOtValue = db.setAmountFormat(reader[50] + "");

                                            ExtraHoursCount = db.setAmountFormat(reader[48].ToString());
                                            addDayValue = db.setAmountFormat(reader[16] + "");
                                            addDAyCount = db.setAmountFormat(reader[47].ToString());
                                            gradeAll = db.setAmountFormat(reader[17] + "");
                                            Position = db.setAmountFormat(reader[18] + "");

                                            attendanceAllowance = db.setAmountFormat(reader[19] + "");
                                            otherAllowance = db.setAmountFormat(reader[20] + "");
                                            totalEarnigs = db.setAmountFormat(reader[21].ToString());

                                            grossPayment = db.setAmountFormat(reader[22].ToString());

                                            lateTime = db.setAmountFormat(reader[24] + "");
                                            lateHoursCount = (int)(reader.GetDouble(23) / 60 / 60) + "Hour" + (int)((reader.GetDouble(23) / 60) % 60) + "Min";
                                            offDayDeduct = db.setAmountFormat(reader[25].ToString());
                                            advancedValue = db.setAmountFormat(reader.GetDouble(26) + "");
                                            otherDeduction = db.setAmountFormat(reader.GetDouble(27) + "");
                                            loanText = db.setAmountFormat(reader.GetDouble(46) + "");
                                            epf_8 = db.setAmountFormat(reader.GetDouble(29) + "");
                                            welfareMember = db.setAmountFormat(reader.GetDouble(30) + "");
                                            welfareDead = db.setAmountFormat(reader.GetDouble(31) + "");

                                            payeStampDuty = db.setAmountFormat(reader.GetDouble(32) + "");
                                            EasyPay1 = db.setAmountFormat(reader.GetDouble(33) + "");
                                            easyPay2 = db.setAmountFormat(reader.GetDouble(34) + "");
                                            easyPay3 = db.setAmountFormat(reader.GetDouble(35) + "");
                                            welfareLoan = db.setAmountFormat(reader.GetDouble(36) + "");
                                            donation = db.setAmountFormat(reader.GetDouble(37) + "");
                                            totalDeductionValue = db.setAmountFormat(reader.GetDouble(38) + "");

                                            netSalaryValue = db.setAmountFormat(reader.GetDouble(39) + "");
                                            coinsBF = db.setAmountFormat(reader.GetDouble(40) + "");
                                            totalPayble = db.setAmountFormat(reader.GetDouble(41) + "");
                                            pay = db.setAmountFormat(reader.GetDouble(42) + "");
                                            epf_12 = db.setAmountFormat(reader.GetDouble(43) + "");
                                            etf_3 = db.setAmountFormat(reader.GetDouble(44) + "");
                                            coinCF = db.setAmountFormat(reader.GetDouble(45) + "");
                                            companyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[51].ToString().ToLower());
                                            month = reader[2].ToString();
                                            dt.Rows.Add(empID, month, reader[56], name, line, desgination, rate, otHoursCount, budj, addDAyCount, ExtraHoursCount, absentDays, lateHoursCount, basicSa, Position, gradeAll, otherAllowance, attendanceAllowance, ot, otSunday, addDayValue, extraOtValue, coinsBF, offDayDeduct, lateTime, epf_8, payeStampDuty, welfareMember, welfareDead, welfareLoan, EasyPay1, easyPay2, easyPay3, loanText, advancedValue, otherDeduction, coinCF, totalDeductionValue, salaryForEpf, netSalaryValue, epf_12, etf_3, grossPayment, pay);
                                        }
                                    }
                                    catch (Exception a)
                                    {
                                    }
                                }
                            }
                        }
                        else if (list_search.SelectedIndex >= 1)
                        {
                            foreach (Tuple<int, int> yearMonth in temp_yearmonthlist)
                            {
                                var period = yearMonth.Item1 + "/" + db.getMOnthName(yearMonth.Item2.ToString());

                                try
                                {

                                    conn.Close();
                                    conn.Open();
                                    if (list_search.SelectedIndex == 1)
                                    {
                                        search_tag = year_from.Text + month_from.Value + "_to_" + year_to.Text + month_to.Value + "_allemployee";
                                        reader = new SqlCommand("select a.*,c.name,b.desgination,d.line,b.name,d.empid,b.tempepfNo from paysheet as a,emp as b,company as c,empbackup as d where  a.empID_1=b.id and b.resgin='" + false + "' and a.month_2='" + period + "' and b.tempepfno=d.epfno and d.month='" + period + "'", conn).ExecuteReader();

                                    }
                                    else
                                    {
                                        search_tag = year_from.Text + month_from.Value + "_to_" + year_to.Text + month_to.Value + "_" + list_search.Text;
                                        reader = new SqlCommand("select a.*,c.name,b.desgination,d.line,b.name,d.empid,b.tempepfNo from paysheet as a,emp as b,company as c,empbackup as d where  a.empID_1=b.id and b.resgin='" + false + "' and a.month_2='" + period + "' and b.tempepfno=d.epfno and d.month='" + period + "' and d.line='" + list_search.Text + "'", conn).ExecuteReader();

                                    }

                                    while (reader.Read())
                                    {
                                        if (!db.CheckEmployee(reader[56].ToString(), period, conn2, reader2))
                                        {
                                            conn2.Open();
                                            reader2 = new SqlCommand("select a.* from shiftValueRates as a, emp as b where b.id='" + reader[1] + "' and b.jobCategory=a.name_0", conn2).ExecuteReader();
                                            if (reader2.Read())
                                            {
                                                rate = db.setAmountFormat(((reader.GetDouble(3) + reader.GetDouble(4)) / reader2.GetDouble(5)) * reader2.GetDouble(6) + "");

                                            }
                                            conn2.Close();

                                            payeStampDuty = db.setAmountFormat(reader.GetDouble(6) + "");
                                            empID = reader[55] + "";
                                            name = reader[54] + "";
                                            line = reader[53] + "";
                                            desgination = reader[52] + "";
                                            basicSa = db.setAmountFormat(reader.GetDouble(3) + "");
                                            budj = db.setAmountFormat(reader.GetDouble(4) + "");
                                            totalBasic = db.setAmountFormat(reader.GetDouble(5) + "");
                                            salaryForEpf = db.setAmountFormat(reader.GetDouble(6) + "");
                                            workingDays = reader[7].ToString() + " Days";
                                            absentDays = reader[8].ToString() + " Days";

                                            otHoursCount = (int)((reader.GetDouble(9) + reader.GetDouble(13)) / 60 / 60) + " Hour " + (int)(((reader.GetDouble(9) + reader.GetDouble(13)) / 60) % 60) + "Min";
                                            ot = db.setAmountFormat((reader.GetDouble(11) + reader.GetDouble(14)) + "");
                                            otSunday = db.setAmountFormat(reader[49] + "");
                                            OtHours2Count = (int)(reader.GetDouble(10) / 60 / 60) + "Hour" + (int)((reader.GetDouble(10) / 60) % 60) + "Min";
                                            extraOtValue = db.setAmountFormat(reader[50] + "");

                                            ExtraHoursCount = db.setAmountFormat(reader[48].ToString());
                                            addDayValue = db.setAmountFormat(reader[16] + "");
                                            addDAyCount = db.setAmountFormat(reader[47].ToString());
                                            gradeAll = db.setAmountFormat(reader[17] + "");
                                            Position = db.setAmountFormat(reader[18] + "");

                                            attendanceAllowance = db.setAmountFormat(reader[19] + "");
                                            otherAllowance = db.setAmountFormat(reader[20] + "");
                                            totalEarnigs = db.setAmountFormat(reader[21].ToString());

                                            grossPayment = db.setAmountFormat(reader[22].ToString());

                                            lateTime = db.setAmountFormat(reader[24] + "");
                                            lateHoursCount = (int)(reader.GetDouble(23) / 60 / 60) + "Hour" + (int)((reader.GetDouble(23) / 60) % 60) + "Min";
                                            offDayDeduct = db.setAmountFormat(reader[25].ToString());
                                            advancedValue = db.setAmountFormat(reader.GetDouble(26) + "");
                                            otherDeduction = db.setAmountFormat(reader.GetDouble(27) + "");
                                            loanText = db.setAmountFormat(reader.GetDouble(46) + "");
                                            epf_8 = db.setAmountFormat(reader.GetDouble(29) + "");
                                            welfareMember = db.setAmountFormat(reader.GetDouble(30) + "");
                                            welfareDead = db.setAmountFormat(reader.GetDouble(31) + "");

                                            payeStampDuty = db.setAmountFormat(reader.GetDouble(32) + "");
                                            EasyPay1 = db.setAmountFormat(reader.GetDouble(33) + "");
                                            easyPay2 = db.setAmountFormat(reader.GetDouble(34) + "");
                                            easyPay3 = db.setAmountFormat(reader.GetDouble(35) + "");
                                            welfareLoan = db.setAmountFormat(reader.GetDouble(36) + "");
                                            donation = db.setAmountFormat(reader.GetDouble(37) + "");
                                            totalDeductionValue = db.setAmountFormat(reader.GetDouble(38) + "");

                                            netSalaryValue = db.setAmountFormat(reader.GetDouble(39) + "");
                                            coinsBF = db.setAmountFormat(reader.GetDouble(40) + "");
                                            totalPayble = db.setAmountFormat(reader.GetDouble(41) + "");
                                            pay = db.setAmountFormat(reader.GetDouble(42) + "");
                                            epf_12 = db.setAmountFormat(reader.GetDouble(43) + "");
                                            etf_3 = db.setAmountFormat(reader.GetDouble(44) + "");
                                            coinCF = db.setAmountFormat(reader.GetDouble(45) + "");
                                            companyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reader[51].ToString().ToLower());
                                            month = reader[2].ToString();
                                            dt.Rows.Add(empID, month, reader[56], name, line, desgination, rate, otHoursCount, budj, addDAyCount, ExtraHoursCount, absentDays, lateHoursCount, basicSa, Position, gradeAll, otherAllowance, attendanceAllowance, ot, otSunday, addDayValue, extraOtValue, coinsBF, offDayDeduct, lateTime, epf_8, payeStampDuty, welfareMember, welfareDead, welfareLoan, EasyPay1, easyPay2, easyPay3, loanText, advancedValue, otherDeduction, coinCF, totalDeductionValue, salaryForEpf, netSalaryValue, epf_12, etf_3, grossPayment, pay);

                                        }
                                    }
                                }
                                catch (Exception a)
                                {
                                }
                            }
                        }

                        ds.Tables.Add(dt);
                        payslip2 paySheet = new payslip2();
                        paySheet.SetDataSource(ds);
                        ExportOptions CrExportOptions;
                        DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                        PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();

                        CrDiskFileDestinationOptions.DiskFileName = dir + "Payslip_AdvanceSearch" + "_" + search_tag + ".pdf";
                        CrExportOptions = paySheet.ExportOptions;
                        {
                            CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            CrExportOptions.FormatOptions = CrFormatTypeOptions;
                        }
                        paySheet.Export();

                        string filePath = dir + "Payslip_AdvanceSearch" + "_" + search_tag + ".pdf";
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
    }
}