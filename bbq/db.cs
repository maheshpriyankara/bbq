using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace bbq
{
    public class db
    {
        DateTime sys_date;
        public db()
        {
            sys_date = DateTime.Now;
            sys_date = DateTime.Now.AddMinutes(750);
        }
        public DateTime getSysDateTime()
        {
            return sys_date;
        }
        public async Task otp_requset(SqlConnection conn_2, string mobile, string empID)
        {
            try
            {
                var otp = (DateTime.Now.Hour * DateTime.Now.Minute) - DateTime.Now.Second + DateTime.Now.Year;
                conn_2.Open();
                new SqlCommand("insert into otp_request values('" + empID + "','" + mobile + "','" + GetLocalIPAddress() + "','" + otp + "','" + getSysDateTime() + "')", conn_2).ExecuteNonQuery();
                conn_2.Close();
                var body = "Hello, your One-Time Password (OTP) for HRIS login is " + otp + ". Please enter this OTP within 100 seconds to complete your login. Thank you!";
                string apiUrl = "https://msmsenterpriseapi.mobitel.lk/EnterpriseSMSV3/esmsproxy.php?m=" + body + "&r=" + mobile + "&a=LEESONS&u=esmsusr_o0KH7YwW&p=!Mahesh@2021&t=0";

                // Create HttpClient instance
                using (HttpClient client = new HttpClient())
                {
                    // Prepare the request parameters
                    var requestContent = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("m", body),
                new KeyValuePair<string, string>("r", mobile),
                new KeyValuePair<string, string>("a", "LEESONS"),
                new KeyValuePair<string, string>("u", "esmsusr_o0KH7YwW"),
                new KeyValuePair<string, string>("p", "!Mahesh@2021"),
                new KeyValuePair<string, string>("t", "0")
            });
                    await client.PostAsync(apiUrl, requestContent);
                    // Send the POST request
                    //HttpResponseMessage response = await client.PostAsync(apiUrl, requestContent);
                    //if (response != null)
                    //{
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        //  string responseContent = await response.Content.ReadAsStringAsync();
                    //        conn_2.Close();
                    //        conn_2.Open();
                    //        //    new SqlCommand("update sms_queue set processing='" + false + "',process_end='" + true + "',respone='"+response.StatusCode+"' where id='" + id_ + "'", conn_2).ExecuteNonQuery();
                    //        conn_2.Close();
                    //    }
                    //    else
                    //    {
                    //        conn_2.Close();
                    //        conn_2.Open();
                    //        //  new SqlCommand("update sms_queue set processing='" + false + "',process_end='" + true + "',respone='" + response.StatusCode + "' where id='" + id_ + "'", conn_2).ExecuteNonQuery();
                    //        conn_2.Close();
                    //    }
                    //}
                    //// Check if the response was successful

                }
            }
            catch (Exception ex)
            {
                conn_2.Close();
            }
        }
        public List<Tuple<int, int>> getYearMonthDifference(DateTime date1, DateTime date2)
        {
            int monthsDiff = (date2.Year - date1.Year) * 12 + (date2.Month - date1.Month) + 1;

            // Generate the list of year/month pairs
            List<Tuple<int, int>> yearMonthList = new List<Tuple<int, int>>();
            for (int i = 0; i < monthsDiff; i++)
            {
                DateTime currentMonth = date1.AddMonths(i);
                yearMonthList.Add(new Tuple<int, int>(currentMonth.Year, currentMonth.Month));
            }
            return yearMonthList;
        }
        public string getCurrentSalaryPeriod(SqlConnection conn, SqlDataReader reader)
        {
            var period = "";
            try
            {
                conn.Open();
                reader = new SqlCommand("select month from empbackup order by id desc", conn).ExecuteReader();
                if (reader.Read())
                {
                    var month = reader.GetString(0).Split('/')[1].ToString();
                    var date = new DateTime(Int32.Parse(reader.GetString(0).Split('/')[0].ToString()), Int32.Parse(getMOnth(month)), 01).AddMonths(1);
                    period = date.Year + "/" + getMOnthName(date.Month.ToString());
                }
                else
                {
                    period = DateTime.Now.Year + "/" + getMOnthName(DateTime.Now.Month.ToString());
                }
                conn.Close();
            }
            catch (Exception a)
            {
                var ddd = a.Message;
                conn.Close();
            }
            return period;
        }
        public string setAmountFormat(string amount)
        {
            string amountI = (int)Double.Parse(amount) + "";

            double amountD = Double.Parse(amount);
            if (amountI.Length == 1)
            {
                amount = String.Format("{0:0.00}", amountD);
            }
            else if (amountI.Length == 2)
            {
                amount = String.Format("{0:00.00}", amountD);
            }
            else if (amountI.Length == 3)
            {
                amount = String.Format("{0:000.00}", amountD);
            }
            else if (amountI.Length == 4)
            {
                amount = String.Format("{0:0,000.00}", amountD);
            }
            else if (amountI.Length == 5)
            {
                amount = String.Format("{0:00,000.00}", amountD);
                ///price = "hu";
            }
            else if (amountI.Length == 6)
            {
                amount = String.Format("{0:000,000.00}", amountD);
            }
            else if (amountI.Length == 7)
            {
                amount = String.Format("{0:0,000,000.00}", amountD);
            }
            else if (amountI.Length == 8)
            {
                amount = String.Format("{0:00,000,000.00}", amountD);
            }
            else if (amountI.Length == 9)
            {
                amount = String.Format("{0:000,000,000.00}", amountD);
            }
            else if (amountI.Length == 10)
            {
                amount = String.Format("{0:0,000,000,000.00}", amountD);
            }
            else if (amountI.Length == 11)
            {
                amount = String.Format("{0:00,000,000,000.00}", amountD);
            }
            else if (amountI.Length == 12)
            {
                amount = String.Format("{0:000,000,000,000.00}", amountD);
            }
            else if (amountI.Length == 13)
            {
                amount = String.Format("{0:0,000,000,000,000.00}", amountD);
            }

            return amount;
        }
        public string getMOnth(string y)
        {
            string month = "";
            if (y.Equals("January"))
            {
                month = "01";
            }
            else if (y.Equals("February"))
            {
                month = "02";
            }
            else if (y.Equals("March"))
            {
                month = "03";
            }
            else if (y.Equals("April"))
            {
                month = "04";
            }
            else if (y.Equals("May"))
            {
                month = "05";
            }
            else if (y.Equals("June"))
            {
                month = "06";
            }
            else if (y.Equals("July"))
            {
                month = "07";
            }
            else if (y.Equals("August"))
            {
                month = "08";
            }
            else if (y.Equals("September"))
            {
                month = "09";
            }
            if (y.Equals("October"))
            {
                month = "10";
            }
            if (y.Equals("November"))
            {
                month = "11";
            }
            if (y.Equals("December"))
            {
                month = "12";
            }

            return month;


        }
        public string getMOnthName2(string y)
        {
            string month = "";
            y = Int32.Parse(y) + "";
            if (y.Equals("01"))
            {
                month = "January";
            }
            else if (y.Equals("02"))
            {
                month = "February";
            }
            else if (y.Equals("03"))
            {
                month = "March";
            }
            else if (y.Equals("04"))
            {
                month = "April";
            }
            else if (y.Equals("05"))
            {
                month = "May";
            }
            else if (y.Equals("06"))
            {
                month = "June";
            }
            else if (y.Equals("07"))
            {
                month = "July";
            }
            else if (y.Equals("08"))
            {
                month = "August";
            }
            else if (y.Equals("09"))
            {
                month = "September";
            }
            if (y.Equals("10"))
            {
                month = "October";
            }
            if (y.Equals("11"))
            {
                month = "November";
            }
            if (y.Equals("12"))
            {
                month = "December";
            }

            return month;


        }

        public string getMOnthName(string y)
        {
            string month = "";
            y = Int32.Parse(y) + "";
            if (y.Equals("1"))
            {
                month = "January";
            }
            else if (y.Equals("2"))
            {
                month = "February";
            }
            else if (y.Equals("3"))
            {
                month = "March";
            }
            else if (y.Equals("4"))
            {
                month = "April";
            }
            else if (y.Equals("5"))
            {
                month = "May";
            }
            else if (y.Equals("6"))
            {
                month = "June";
            }
            else if (y.Equals("7"))
            {
                month = "July";
            }
            else if (y.Equals("8"))
            {
                month = "August";
            }
            else if (y.Equals("9"))
            {
                month = "September";
            }
            if (y.Equals("10"))
            {
                month = "October";
            }
            if (y.Equals("11"))
            {
                month = "November";
            }
            if (y.Equals("12"))
            {
                month = "December";
            }

            return month;


        }
        public string getBalance(SqlConnection con2, SqlDataReader reader2, string accountID)
        {
            var return_ = "";
            con2.Open();
            reader2 = new SqlCommand("select balance from sa_statment where account_id = '" + accountID + "' order by id desc", con2).ExecuteReader();
            if (reader2.Read())
            {

                return_ = new db().setAmountFormat(reader2.GetDouble(0) + "");


            }
            con2.Close();

            return return_;
        }
        public string getBalanceStatus(SqlConnection con2, SqlDataReader reader2, string accountID)
        {
            var return_ = "";

            try
            {
                con2.Open();
                reader2 = new SqlCommand("select * from sa_ledger where account_id='" + accountID + "' and updated='" + false + "' and checked='" + true + "' and approve_reject='" + true + "'", con2).ExecuteReader();
                if (reader2.Read())
                {
                    return_ = return_ + ", updating.....";
                }
                con2.Close();
            }
            catch (Exception)
            {
                con2.Close();
            }
            return return_;
        }
        public Boolean checkDateLockStatus(SqlConnection conn, SqlDataReader reader, DateTime date, string accountID)
        {
            var check_ = false;
            try
            {
                conn.Open();
                reader = new SqlCommand("select * from sa_date_lock where date>='" + date + "' and accountID='" + accountID + "' ", conn).ExecuteReader();
                if (reader.Read())
                {
                    check_ = true;
                }
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
            return check_;

        }
        public DateTime getLastRecordDate(SqlConnection conn, SqlDataReader reader, string accountID)
        {
            var date = DateTime.Now;
            try
            {
                conn.Open();
                if (accountID.Equals("18") || accountID.Equals("25"))
                {
                    reader = new SqlCommand("select max(date) from sa_ledger where account_id='" + accountID + "' and double_entry='" + false + "'", conn).ExecuteReader();
                }
                else
                {
                    reader = new SqlCommand("select max(date) from sa_statment where account_id='" + accountID + "'", conn).ExecuteReader();
                }
                if (reader.Read())
                {
                    date = reader.GetDateTime(0);
                }
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
            return date;
        }
        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public void updateLogs(SqlConnection conn, SqlConnection conn2, SqlDataReader reader, string category, string reference, DateTime created, string user)
        {
            try
            {
                conn.Open();
                new SqlCommand("insert into sa_logs_ values('" + category + "','" + getMaxID(conn2, reader, reference) + "','" + created + "','" + user + "','" + GetLocalIPAddress() + "')", conn).ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
        }
        public void updateLogsDeleted(SqlConnection conn, SqlConnection conn2, SqlDataReader reader, string category, string reference, DateTime created, string user, string id)
        {
            try
            {
                conn.Open();
                new SqlCommand("insert into sa_logs_ values('" + category + "','" + id + "','" + created + "','" + user + "','" + GetLocalIPAddress() + "')", conn).ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
        }

        double gapRent_ = 0, qtyState;
        TimeSpan difference;
        public double getrentGap(DateTime date)
        {
            //  MessageBox.Show(date+"");
            double gapRent = 0;
            difference = DateTime.Now.Subtract(date);
            // difference = DateTime.Parse("2019/10/31 09:04:00.0000000").Subtract(date);
            if (difference.TotalMinutes <= 1470)
            {
                gapRent = 1;

            }
            else
            {

                Int32 gapReal2 = (int)(difference.TotalMinutes / 1440);
                if ((difference.TotalMinutes - (gapReal2 * 1440)) <= 30)
                {
                    gapRent = gapReal2;
                }
                else if ((difference.TotalMinutes - (gapReal2 * 1440)) <= 270)
                {
                    gapRent = gapReal2 + 0.5;
                }
                else
                {
                    gapRent = gapReal2 + 1;
                }
            }
            //   MessageBox.Show(gapRent+"");
            return gapRent;

        }
        public int getMaxID(SqlConnection conn, SqlDataReader reader, string table)
        {
            var id = 0;
            try
            {
                conn.Open();
                reader = new SqlCommand("select max(id) from " + table + " ", conn).ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
            return id;
        }

        ArrayList dateList, value;
        bool oneToEnd, pastToPresent, tempBool;
        Int32 tempMonth;
        string lastDate;
        public string getLastDate(int month, int year)
        {
            var firstOftargetMonth = new DateTime(year, month, 1);
            var firstOfNextMonth = firstOftargetMonth.AddMonths(1);

            var allDates = new List<DateTime>();

            for (DateTime date = firstOftargetMonth; date < firstOfNextMonth; date = date.AddDays(1))
            {
                allDates.Add(date);
            }
            lastDate = allDates[allDates.Count - 1].ToString().Split(' ')[0].ToString().Split('/')[1];
            return lastDate;
        }


        public ArrayList getDateList(string empId, string month, string year, SqlConnection conn, SqlDataReader reader)
        {

            try
            {

                dateList = new ArrayList();
                oneToEnd = false;
                pastToPresent = false;
                conn.Open();
                reader = new SqlCommand("select a.*  from shiftsalaryPeriod  as a,emp as b where  b.id='" + empId + "' and b.jobCategory=a.name ", conn).ExecuteReader();
                if (reader.Read())
                {
                    oneToEnd = reader.GetBoolean(1);
                    pastToPresent = reader.GetBoolean(2);
                }
                conn.Close();

                if (oneToEnd)
                {
                    var lastDate_ = DateTime.DaysInMonth(Int32.Parse(year), Int32.Parse(month));

                    for (int i = 1; i <= lastDate_; i++)
                    {
                        dateList.Add(year + "-" + month + "-" + i);
                    }
                }
                else if (pastToPresent)
                {
                    tempMonth = new DateTime(Int32.Parse(year), Int32.Parse(month), 1).AddMonths(-1).Month;
                    string Year2 = new DateTime(Int32.Parse(year), Int32.Parse(month), 1).AddMonths(-1).Year + "";
                    //var lastDate = Int32.Parse(getLastDate(tempMonth, Int32.Parse(Year2)));
                    var lastDate_ = DateTime.DaysInMonth(new DateTime(Int32.Parse(year), Int32.Parse(month), 1).AddMonths(-1).Year, new DateTime(Int32.Parse(year), Int32.Parse(month), 1).AddMonths(-1).Month);
                    for (int i = 26; i <= lastDate_; i++)
                    {
                        dateList.Add(Year2 + "-" + tempMonth + "-" + i);
                    }
                    for (int i = 1; i <= 25; i++)
                    {
                        dateList.Add(year + "-" + month + "-" + i);
                    }
                }
            }
            catch (Exception a)
            {
            }
            return dateList;
        }

        public bool getNopay(string id, string date, SqlConnection conn, SqlDataReader reader)
        {
            try
            {


                tempBool = false;
                conn.Open();
                reader = new SqlCommand("select dayType_13 from timesheet where empId_1='" + id + "'  and inDate_3 = '" + date + "' ", conn).ExecuteReader();
                //  MessageBox.Show(ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-1" + " a " + ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-" + getLastDate(Int32.Parse(getMOnth(comboBox1.SelectedItem.ToString())), Int32.Parse(ComboBoxYear.Value.ToString("d").Split('/')[2])));
                if (reader.Read())
                {
                    if (reader.GetInt32(0) == 4)
                    {
                        tempBool = true;
                    }

                }

                conn.Close();
            }
            catch (Exception a)
            {
                conn.Close();
                tempBool = false;
            }
            return tempBool;
        }
        public bool getLeaveHalf(string id, string date, SqlConnection conn, SqlDataReader reader)
        {
            try
            {


                tempBool = false;
                conn.Open();
                reader = new SqlCommand("select * from leaveHalfDay where empid='" + id + "'  and date = '" + date + "' ", conn).ExecuteReader();
                //  MessageBox.Show(ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-1" + " a " + ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-" + getLastDate(Int32.Parse(getMOnth(comboBox1.SelectedItem.ToString())), Int32.Parse(ComboBoxYear.Value.ToString("d").Split('/')[2])));
                if (reader.Read())
                {

                    tempBool = true;
                }

                conn.Close();
            }
            catch (Exception a)
            {
                conn.Close();
                tempBool = false;
            }
            return tempBool;
        }
        public bool getSleeping(string id, string date, SqlConnection conn, SqlDataReader reader)
        {
            try
            {


                tempBool = false;
                conn.Open();
                reader = new SqlCommand("select * from sleping where empid='" + id + "'  and date = '" + date + "' ", conn).ExecuteReader();
                //  MessageBox.Show(ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-1" + " a " + ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-" + getLastDate(Int32.Parse(getMOnth(comboBox1.SelectedItem.ToString())), Int32.Parse(ComboBoxYear.Value.ToString("d").Split('/')[2])));
                if (reader.Read())
                {

                    tempBool = true;
                }

                conn.Close();
            }
            catch (Exception a)
            {
                conn.Close();
                tempBool = false;
            }
            return tempBool;
        }
        public bool getLeaveLeave(string id, string date, SqlConnection conn, SqlDataReader reader)
        {
            try
            {


                tempBool = false;
                conn.Open();
                reader = new SqlCommand("select * from leaveLeave where empid='" + id + "'  and date = '" + date + "' ", conn).ExecuteReader();
                //  MessageBox.Show(ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-1" + " a " + ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-" + getLastDate(Int32.Parse(getMOnth(comboBox1.SelectedItem.ToString())), Int32.Parse(ComboBoxYear.Value.ToString("d").Split('/')[2])));
                if (reader.Read())
                {

                    tempBool = true;
                }

                conn.Close();
            }
            catch (Exception a)
            {
                tempBool = false;
            }
            return tempBool;
        }
        public bool getMultiShift(string id, string date, SqlConnection conn, SqlDataReader reader)
        {
            try
            {


                tempBool = false;
                conn.Open();
                reader = new SqlCommand("select * from multiShift where empid='" + id + "'  and date = '" + date + "' ", conn).ExecuteReader();
                //  MessageBox.Show(ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-1" + " a " + ComboBoxYear.Value.ToString("d").Split('/')[2] + "-" + getMOnth(comboBox1.SelectedItem.ToString()) + "-" + getLastDate(Int32.Parse(getMOnth(comboBox1.SelectedItem.ToString())), Int32.Parse(ComboBoxYear.Value.ToString("d").Split('/')[2])));
                if (reader.Read())
                {

                    tempBool = true;
                }

                conn.Close();
            }
            catch (Exception a)
            {
                conn.Close();
                tempBool = false;
            }
            return tempBool;
        }
        public bool CheckEmployee(string epfno, string period, SqlConnection conn, SqlDataReader reader)
        {
            var status = false;

            try
            {
                conn.Open();
                reader = new SqlCommand("select id from emp where epfno='" + epfno + "'", conn).ExecuteReader();
                if (reader.Read())
                {
                    epfno = reader[0] + "";
                }
                conn.Close();
                conn.Open();
                reader = new SqlCommand("select lock from empbackup where id='" + epfno + "' and month='" + period + "'", conn).ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetBoolean(0))
                    {
                    }
                    else
                    {
                        conn.Close();
                        conn.Open();
                        reader = new SqlCommand("select id from process_queue where emp_systemID='" + epfno + "' and period_='" + period + "' and process_end='" + false + "'", conn).ExecuteReader();
                        if (reader.Read())
                        {
                            status = true;
                        }
                        conn.Close();
                    }


                }
                conn.Close();

            }
            catch (Exception)
            {
                conn.Close();
            }
            return status;
        }

        public void PushEmployee(string epfno, string period, string requestFrom, string user_, string ip)
        {
            int status = 0;
            SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            SqlDataReader reader = null;
            try
            {
                var system_id = "";
                var empid = "";

                conn.Open();
                reader = new SqlCommand("select id from process_queue where period_='" + period + "'", conn).ExecuteReader();
                if (!reader.Read())
                {
                    conn.Close();
                    var date = getSysDateTime();
                    conn.Open();
                    new SqlCommand("insert into process_request values ('" + period + "','" + false + "','" + false + "','" + date + "','" + date + "')", conn).ExecuteNonQuery();
                    conn.Close();
                }
                conn.Close();

                conn.Open();
                reader = new SqlCommand("select id,empid from emp where epfno='" + epfno + "'", conn).ExecuteReader();
                if (reader.Read())
                {
                    system_id = reader[0] + "";
                    empid = reader[1] + "";
                }
                conn.Close();
                var date_ = getSysDateTime();
                conn.Open();
                reader = new SqlCommand("select id from process_queue where emp_systemID='" + system_id + "' and period_='" + period + "' and process_end='" + false + "' and processing='" + false + "' and process_status='" + 0 + "'", conn).ExecuteReader();
                if (reader.Read())
                {
                    var id = reader[0] + "";
                    conn.Close();
                    conn.Open();
                    new SqlCommand("insert into logs_ values ('" + "Salary Process" + "','" + system_id + "','" + empid + "','" + "Process Request Override-" + id + "','" + period + "','" + date_ + "')", conn).ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    conn.Close();
                    conn.Open();
                    new SqlCommand("insert into process_queue values ('" + system_id + "','" + empid + "','" + requestFrom + "','" + date_ + "','" + user_ + "','" + ip + "','" + 0 + "','" + false + "','" + false + "','" + date_ + "','" + date_ + "','" + date_ + "','" + period + "')", conn).ExecuteNonQuery();
                    conn.Close();
                    conn.Open();
                    new SqlCommand("insert into logs_ values ('" + "Salary Process" + "','" + system_id + "','" + empid + "','" + "Process Request" + "','" + period + "','" + date_ + "')", conn).ExecuteNonQuery();
                    conn.Close();
                }
                conn.Close();

            }
            catch (Exception)
            {
                conn.Close();
            }
        }
    }
}