using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bbq
{
    public partial class report_employee : System.Web.UI.Page
    {
        static SqlConnection conn = new SqlConnection(
     WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        DataTable dt;
        ArrayList reportList;
        SqlDataReader reader;
        string query;
        String getColumnList(bool epfNo, bool attendanceNo, bool firstName, bool nic, bool drivingLicence, bool dob, bool gender, bool marital, bool blood, bool religion, bool nationality, bool race, bool shiftBlock, bool department, bool doa, bool address, bool contact, bool basic, bool budg, bool attendanceAllow, bool fixedAllow, bool epfPay, bool bankPay, bool acNo, bool bankCode, bool branchCode, bool resgin, bool resginDate, bool block, bool defaultRoaster, bool otType)
        {
            query = "";
            if (epfNo)
            {
                if (query.Equals(""))
                {
                    query = "tempepfNo as 'EPF No'";
                }
                else
                {
                    query = query+",tempepfNo as 'EPF No'";
                }
            }
            if (attendanceNo)
            {
                if (query.Equals(""))
                {
                    query = "empid as 'Attendance No'";
                }
                else
                {
                    query = query + ",empid as 'Attendance No'";
                }
            }
            if (firstName)
            {
                if (query.Equals(""))
                {
                    query = "name as 'Full Name'";
                }
                else
                {
                    query = query + ",name as 'Full Name'";
                }
            }
            if (nic)
            {
                if (query.Equals(""))
                {
                    query = "nic as 'NIC'";
                }
                else
                {
                    query = query + ",nic as 'NIC'";
                }
            }
            if (drivingLicence)
            {
                if (query.Equals(""))
                {
                    query = "drivingLicence as 'Driving Licence'";
                }
                else
                {
                    query = query + ",drivingLicence as 'Driving Licence'";
                }
            }
            if (dob)
            {
                if (query.Equals(""))
                {
                    query = "dob as 'DOB'";
                }
                else
                {
                    query = query + ",dob as 'DOB'";
                }
            }
            if (gender)
            {
                if (query.Equals(""))
                {
                    query = "gender as 'Gender'";
                }
                else
                {
                    query = query + ",gender as 'Gender'";
                }
            }
            if (marital)
            {
                if (query.Equals(""))
                {
                    query = "marital as 'Marital Status'";
                }
                else
                {
                    query = query + ",marital as 'Marital Status'";
                }
            }
            if (blood)
            {
                if (query.Equals(""))
                {
                    query = "blood as 'Blood Status'";
                }
                else
                {
                    query = query + ",blood as 'Blood Status'";
                }
            }
            if (religion)
            {
                if (query.Equals(""))
                {
                    query = "religion as 'Religion'";
                }
                else
                {
                    query = query + ",religion as 'Religion'";
                }
            }
            if (nationality)
            {
                if (query.Equals(""))
                {
                    query = "nationality as 'Nationality'";
                }
                else
                {
                    query = query + ",nationality as 'Nationality'";
                }
            }
            if (race)
            {
                if (query.Equals(""))
                {
                    query = "race as 'Race'";
                }
                else
                {
                    query = query + ",race as 'Race'";
                }
            }
            if (shiftBlock)
            {
                if (query.Equals(""))
                {
                    query = "jobCategory as 'Shift Block'";
                }
                else
                {
                    query = query + ",jobCategory as 'Shift Block'";
                }
            }
            if (department)
            {
                if (query.Equals(""))
                {
                    query = "line as 'Department'";
                }
                else
                {
                    query = query + ",line as 'Department'";
                }
            }
            if (doa)
            {
                if (query.Equals(""))
                {
                    query = "dateOfAppoinmant as 'Appoinmant Date'";
                }
                else
                {
                    query = query + ",dateOfAppoinmant as 'Appoinmant Date'";
                }
            }
            if (address)
            {
                if (query.Equals(""))
                {
                    query = "residentialAddress as 'Address'";
                }
                else
                {
                    query = query + ",residentialAddress as 'Address'";
                }
            }
            if (contact)
            {
                if (query.Equals(""))
                {
                    query = "mobileNUmber as 'Contact Number'";
                }
                else
                {
                    query = query + ",mobileNUmber as 'Contact Number'";
                }
            }
            if (basic)
            {
                if (query.Equals(""))
                {
                    query = "Basic as 'Basic Salary'";
                }
                else
                {
                    query = query + ",Basic as 'Basic Salary'";
                }
            }
            if (budg)
            {
                if (query.Equals(""))
                {
                    query = "budj as 'Budgetary Allowance'";
                }
                else
                {
                    query = query + ",budj as 'Budgetary Allowance'";
                }
            }
            if (fixedAllow)
            {
                if (query.Equals(""))
                {
                    query = "allowances as 'Fixed Allowance'";
                }
                else
                {
                    query = query + ",allowances as 'Fixed Allowance'";
                }
            }
            if (attendanceAllow)
            {
                if (query.Equals(""))
                {
                    query = "attendanceAllowance as 'Attendance Allowance'";
                }
                else
                {
                    query = query + ",attendanceAllowance as 'Attendance Allowance'";
                }
            }
            if (epfPay)
            {
                if (query.Equals(""))
                {
                    query = "isEpf as 'EPF Pay'";
                }
                else
                {
                    query = query + ",isEpf as 'EPF Pay'";
                }
            }
            if (bankPay)
            {
                if (query.Equals(""))
                {
                    query = "isBankPay as 'Bank Pay'";
                }
                else
                {
                    query = query + ",isBankPay as 'Bank Pay'";
                }
            }
            if (acNo)
            {
                if (query.Equals(""))
                {
                    query = "acno as 'Account Number'";
                }
                else
                {
                    query = query + ",acno as 'Account Number'";
                }
            }
            if (bankCode)
            {
                if (query.Equals(""))
                {
                    query = "bankNo as 'Bank Code'";
                }
                else
                {
                    query = query + ",bankNo as 'Bank Code'";
                }
            }
            if (branchCode)
            {
                if (query.Equals(""))
                {
                    query = "branchNo as 'Branch Code'";
                }
                else
                {
                    query = query + ",branchNo as 'Branch Code'";
                }
            }
            if (defaultRoaster)
            {
                if (query.Equals(""))
                {
                    query = "isDefaultRoaster as 'Default Roaster'";
                }
                else
                {
                    query = query + ",isDefaultRoaster as 'Default Roaster'";
                }
            }
            if (otType)
            {
                if (query.Equals(""))
                {
                    query = "otcircle as 'OT Circle Day (Yes), 180Hours (No)'";
                }
                else
                {
                    query = query + ",otcircle as 'OT Circle Day (Yes), 180Hours (No)'";
                }
            }
            if (resgin)
            {
                if (query.Equals(""))
                {
                    query = "RESGIN as 'Resgin'";
                }
                else
                {
                    query = query + ",RESGIN as 'Resgin'";
                }
            }
            if (resginDate)
            {
                if (query.Equals(""))
                {
                    query = "resginDate as 'Resgin Date'";
                }
                else
                {
                    query = query + ",resginDate as 'Resgin Date'";
                }
            }
            if (block)
            {
                if (query.Equals(""))
                {
                    query = "BLOCK as 'Block'";
                }
                else
                {
                    query = query + ",BLOCK as 'Block'";
                }
            }
            return query;

        }
        String getOrderByList(bool orderByEPFNo, bool orderByEPFNoASC, bool orderByAttendanceNo, bool orderByAttendanceNoASC, bool orderBySystemID, bool orderBySystemISASC, bool orderByName, bool orderByNameASC)
        {
            query = "";
            if (orderByEPFNo)
            {
                if (orderByEPFNoASC)
                {
                    query = "Order By tempepfno ASC";
                }
                else
                {
                    query = "Order By tempepfno DESC";
                }
            }
            else if (orderByAttendanceNo)
            {
                if (orderByAttendanceNoASC)
                {
                    query = "Order By empid ASC";
                }
                else
                {
                    query = "Order By empid DESC";
                }
            }
            else if (orderBySystemID)
            {
                if (orderBySystemISASC)
                {
                    query = "Order By id ASC";
                }
                else
                {
                    query = "Order By id DESC";
                }
            }
            else if (orderByName)
            {
                if (orderByNameASC)
                {
                    query = "Order By name ASC";
                }
                else
                {
                    query = "Order By name DESC";
                }
            }

            return query;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] != null && Session["userType"] != null)
            {
                if (!Page.IsPostBack)
                {
                    var quearyColumns = "";
                    var quearyOrderBy = "";
                    conn.Close();
                    conn.Open();
                    reader = new SqlCommand("select * from reportSettingsEmployee", conn).ExecuteReader();
                    if (reader.Read())
                    {
                        check_epfNo.Checked = reader.GetBoolean(0);
                        check_attendance.Checked = reader.GetBoolean(1);
                        check_name.Checked = reader.GetBoolean(2);
                        check_nic.Checked = reader.GetBoolean(3);
                        check_drivingLicence.Checked = reader.GetBoolean(4);
                        check_dob.Checked = reader.GetBoolean(5);
                        check_gender.Checked = reader.GetBoolean(6);
                        check_marital.Checked = reader.GetBoolean(7);
                        check_blood.Checked = reader.GetBoolean(8);
                        check_religion.Checked = reader.GetBoolean(9);
                        check_nationality.Checked = reader.GetBoolean(10);
                        check_race.Checked = reader.GetBoolean(11);
                        check_shiftBlock.Checked = reader.GetBoolean(12);
                        check_department.Checked = reader.GetBoolean(13);
                        check_doa.Checked = reader.GetBoolean(14);
                        check_address.Checked = reader.GetBoolean(15);
                        check_contact.Checked = reader.GetBoolean(16);
                        check_basic.Checked = reader.GetBoolean(17);
                        check_budj.Checked = reader.GetBoolean(18);
                        check_allow.Checked = reader.GetBoolean(19);
                        check_attendance.Checked = reader.GetBoolean(20);
                        check_epfPay_.Checked = reader.GetBoolean(21);
                        check_bankPay.Checked = reader.GetBoolean(22);
                        check_acNo.Checked = reader.GetBoolean(23);
                        check_bankCode.Checked = reader.GetBoolean(24);
                        check_branchCode.Checked = reader.GetBoolean(25);
                        check_resign.Checked = reader.GetBoolean(26);
                        check_resginDate.Checked = reader.GetBoolean(27);
                        check_block.Checked = reader.GetBoolean(28);
                        check_defaultRoaster.Checked = reader.GetBoolean(29);
                        check_otTYpe.Checked = reader.GetBoolean(30);
                        quearyColumns = getColumnList(reader.GetBoolean(0), reader.GetBoolean(1), reader.GetBoolean(2), reader.GetBoolean(3), reader.GetBoolean(4), reader.GetBoolean(5), reader.GetBoolean(6), reader.GetBoolean(7), reader.GetBoolean(8), reader.GetBoolean(9), reader.GetBoolean(10), reader.GetBoolean(11), reader.GetBoolean(12), reader.GetBoolean(13), reader.GetBoolean(14), reader.GetBoolean(15), reader.GetBoolean(16), reader.GetBoolean(17), reader.GetBoolean(18), reader.GetBoolean(20), reader.GetBoolean(19), reader.GetBoolean(21), reader.GetBoolean(22), reader.GetBoolean(23), reader.GetBoolean(24), reader.GetBoolean(25), reader.GetBoolean(26), reader.GetBoolean(27), reader.GetBoolean(28), reader.GetBoolean(29), reader.GetBoolean(30));
                        check_orderByEpfNo.Checked = reader.GetBoolean(31);
                        radio_orderByEpfNoASC.Checked = reader.GetBoolean(32);
                        check_orderByAttendanceId.Checked = reader.GetBoolean(33);
                        radio_orderByAttendanceIdASC.Checked = reader.GetBoolean(34);
                        check_orderBySystemID.Checked = reader.GetBoolean(35);
                        radio_orderBySystemIDASC.Checked = reader.GetBoolean(36);
                        check_orderByName.Checked = reader.GetBoolean(37);
                        radio_orderByNameASC.Checked = reader.GetBoolean(38);
                        quearyOrderBy = getOrderByList(reader.GetBoolean(31), reader.GetBoolean(32), reader.GetBoolean(33), reader.GetBoolean(34), reader.GetBoolean(35), reader.GetBoolean(36), reader.GetBoolean(37), reader.GetBoolean(38));
                    }
                    conn.Close();
                    if (quearyColumns.Equals(""))
                    {
                        quearyColumns = "*";
                    }
                    dt = new DataTable();
                    conn.Open();
                    reader = new SqlCommand("select " + quearyColumns + " from emp where resgin='false'" + quearyOrderBy, conn).ExecuteReader();
                    DataTable schema = reader.GetSchemaTable();

                    foreach (DataRow row in schema.Rows)
                    {
                        dt.Columns.Add(row["ColumnName"].ToString());
                    }
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        dt.Rows.Add(values);
                    }
                    conn.Close();
                    Grid_employee.DataSource = dt;
                    Grid_employee.DataBind();

                }

            }
            else
            {
                Response.Redirect("login.aspx");
            }
         
        }

        protected void btn_saveAdvanceSearch_Click(object sender, EventArgs e)
        {
            conn.Open();
            new SqlCommand("delete from reportSettingsEmployee", conn).ExecuteNonQuery();
            new SqlCommand("insert into reportSettingsEmployee values ('"+check_epfNo.Checked+ "','" + check_attendance.Checked + "','" + check_name.Checked + "','" + check_nic.Checked + "','" + check_drivingLicence.Checked + "','" + check_dob.Checked + "','" + check_gender.Checked + "','" + check_marital.Checked + "','" + check_blood.Checked + "','" + check_religion.Checked + "','" + check_nationality.Checked + "','" + check_race.Checked + "','" + check_shiftBlock.Checked + "','" + check_department.Checked + "','" + check_doa.Checked + "','" + check_address.Checked + "','" + check_contact.Checked + "','" + check_basic.Checked + "','" + check_budj.Checked + "','" + check_allow.Checked + "','" + check_attendance.Checked + "','" + check_epfPay_.Checked + "','" + check_bankPay.Checked + "','" + check_acNo.Checked + "','" + check_bankCode.Checked + "','" + check_branchCode.Checked + "','" + check_resign.Checked + "','" + check_resginDate.Checked + "','" + check_block.Checked + "','" + check_defaultRoaster.Checked + "','" + check_otTYpe.Checked + "','" + check_orderByEpfNo.Checked + "','" + radio_orderByEpfNoASC.Checked + "','" + check_orderByAttendanceId.Checked + "','" + radio_orderByAttendanceIdASC.Checked + "','" + check_orderBySystemID.Checked + "','" + radio_orderBySystemIDASC.Checked + "','" + check_orderByName.Checked + "','" + radio_orderByNameASC.Checked + "')", conn).ExecuteNonQuery();
            conn.Close();

            var quearyColumns = "";
            var quearyOrderBy = "";
            conn.Open();
            reader = new SqlCommand("select * from reportSettingsEmployee", conn).ExecuteReader();
            if (reader.Read())
            {
                check_epfNo.Checked = reader.GetBoolean(0);
                check_attendance.Checked = reader.GetBoolean(1);
                check_name.Checked = reader.GetBoolean(2);
                check_nic.Checked = reader.GetBoolean(3);
                check_drivingLicence.Checked = reader.GetBoolean(4);
                check_dob.Checked = reader.GetBoolean(5);
                check_gender.Checked = reader.GetBoolean(6);
                check_marital.Checked = reader.GetBoolean(7);
                check_blood.Checked = reader.GetBoolean(8);
                check_religion.Checked = reader.GetBoolean(9);
                check_nationality.Checked = reader.GetBoolean(10);
                check_race.Checked = reader.GetBoolean(11);
                check_shiftBlock.Checked = reader.GetBoolean(12);
                check_department.Checked = reader.GetBoolean(13);
                check_doa.Checked = reader.GetBoolean(14);
                check_address.Checked = reader.GetBoolean(15);
                check_contact.Checked = reader.GetBoolean(16);
                check_basic.Checked = reader.GetBoolean(17);
                check_budj.Checked = reader.GetBoolean(18);
                check_allow.Checked = reader.GetBoolean(19);
                check_attendance.Checked = reader.GetBoolean(20);
                check_epfPay_.Checked = reader.GetBoolean(21);
                check_bankPay.Checked = reader.GetBoolean(22);
                check_acNo.Checked = reader.GetBoolean(23);
                check_bankCode.Checked = reader.GetBoolean(24);
                check_branchCode.Checked = reader.GetBoolean(25);
                check_resign.Checked = reader.GetBoolean(26);
                check_resginDate.Checked = reader.GetBoolean(27);
                check_block.Checked = reader.GetBoolean(28);
                check_defaultRoaster.Checked = reader.GetBoolean(29);
                check_otTYpe.Checked = reader.GetBoolean(30);
                quearyColumns = getColumnList(reader.GetBoolean(0), reader.GetBoolean(1), reader.GetBoolean(2), reader.GetBoolean(3), reader.GetBoolean(4), reader.GetBoolean(5), reader.GetBoolean(6), reader.GetBoolean(7), reader.GetBoolean(8), reader.GetBoolean(9), reader.GetBoolean(10), reader.GetBoolean(11), reader.GetBoolean(12), reader.GetBoolean(13), reader.GetBoolean(14), reader.GetBoolean(15), reader.GetBoolean(16), reader.GetBoolean(17), reader.GetBoolean(18), reader.GetBoolean(20), reader.GetBoolean(19), reader.GetBoolean(21), reader.GetBoolean(22), reader.GetBoolean(23), reader.GetBoolean(24), reader.GetBoolean(25), reader.GetBoolean(26), reader.GetBoolean(27), reader.GetBoolean(28), reader.GetBoolean(29), reader.GetBoolean(30));
                check_orderByEpfNo.Checked = reader.GetBoolean(31);
                radio_orderByEpfNoASC.Checked = reader.GetBoolean(32);
                check_orderByAttendanceId.Checked = reader.GetBoolean(33);
                radio_orderByAttendanceIdASC.Checked = reader.GetBoolean(34);
                check_orderBySystemID.Checked = reader.GetBoolean(35);
                radio_orderBySystemIDASC.Checked = reader.GetBoolean(36);
                check_orderByName.Checked = reader.GetBoolean(37);
                radio_orderByNameASC.Checked = reader.GetBoolean(38);
                quearyOrderBy = getOrderByList(reader.GetBoolean(31), reader.GetBoolean(32), reader.GetBoolean(33), reader.GetBoolean(34), reader.GetBoolean(35), reader.GetBoolean(36), reader.GetBoolean(37), reader.GetBoolean(38));
            }
            conn.Close();
            if (quearyColumns.Equals(""))
            {
                quearyColumns = "*";
            }
            dt = new DataTable();
            conn.Open();
            reader = new SqlCommand("select " + quearyColumns + " from emp where resgin='false'" + quearyOrderBy, conn).ExecuteReader();
            DataTable schema = reader.GetSchemaTable();

            foreach (DataRow row in schema.Rows)
            {
                dt.Columns.Add(row["ColumnName"].ToString());
            }
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                reader.GetValues(values);
                dt.Rows.Add(values);
            }
            conn.Close();
            Grid_employee.DataSource = dt;
            Grid_employee.DataBind();
        }
    }
}