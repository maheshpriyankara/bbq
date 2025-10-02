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
    public partial class download_user : System.Web.UI.Page
    {
        static SqlConnection conn = new SqlConnection(
     WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        static SqlConnection conn2 = new SqlConnection(
WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlDataReader reader, reader2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["userID"] != null)
                {
                    if (!Session["userType"].ToString().Equals("Admin"))
                    {
                        menu.Visible = false;
                        menu2.Visible = false;
                        menu3.Visible = false;
                        menu4.Visible = false;
                        menu5.Visible = false;
                        menu6.Visible = false;
                        menu7.Visible = false;
                        list_designation.Items.Clear();

                        conn.Open();
                        reader = new SqlCommand("SELECT a.month from empBackup as a,payslipmonthlist as b WHERE a.lock = 'true' and a.id = '" + Session["userID"] + "' and a.month=b.month ORDER BY b.id DESC", conn).ExecuteReader();
                        while (reader.Read())
                        {
                            list_designation.Items.Add(reader[0] + "");
                        }
                        conn.Close();
                        conn.Open();
                        reader = new SqlCommand("select firstname,lastname,line from emp where id='" + Session["userID"] + "'", conn).ExecuteReader();
                        if (reader.Read())
                        {
                            txt_name.InnerText = reader[0] + " " + reader[1] + "";
                            txt_line.InnerText = reader[2] + "";
                        }
                        conn.Close();
                        txt_name.InnerText = "";
                    }
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }


        }

        protected void btn_load_Click(object sender, EventArgs e)
        {
            var tt = "";
        }

        protected void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                db db = new db();
                var dir = new DirectoryInfo(Server.MapPath("/PDF/"));


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
                    var period = list_designation.SelectedItem;


                    idL = Session["userID"].ToString();
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
                    conn.Close();

                    ds.Tables.Add(dt);
                    payslip2 paySheet = new payslip2();
                    paySheet.SetDataSource(ds);

                    ExportOptions CrExportOptions;
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();

                    var down = DateTime.Now.ToString("yyyyMMss HHmmss");
                    CrDiskFileDestinationOptions.DiskFileName = dir + list_designation.Text.Split('/')[0] + list_designation.Text.Split('/')[1] + "_Payslip_" + Session["userID"] + ".pdf";
                    CrExportOptions = paySheet.ExportOptions;
                    {
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    }
                    paySheet.Export();

                    string filePath = dir + list_designation.Text.Split('/')[0] + list_designation.Text.Split('/')[1] + "_Payslip_" + Session["userID"] + ".pdf";
                    FileInfo file = new FileInfo(filePath);

                    paySheet.Close();
                    paySheet.Dispose();
                    paySheet = null;
                    ds.Clear();
                    ds.Dispose();


                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(file.FullName);
                    Response.End();
                }


            }
            catch (Exception a)
            {
            }
        }
    }
}