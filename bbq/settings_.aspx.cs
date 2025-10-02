using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bbq
{
    public partial class settings_ : System.Web.UI.Page
    {
        static SqlConnection conn = new SqlConnection(
    WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        static SqlConnection conn2 = new SqlConnection(
   WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);

        static SqlConnection conn_main;
        SqlDataReader reader2;
        static SqlDataReader reader;
        db db;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                list_calendarType.Items.Add("Poya Day");
                list_calendarType.Items.Add("Holiday");
                list_calendarType.Items.Add("Working Day");

                text_date.Text = new db().getSysDateTime().ToString();
            }
            if (Session["userID"] != null && Session["userType"] != null)
            {

            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void btn_exportText_Click(object sender, EventArgs e)
        {
            string comapny = "";
            double total = 0;
            //if (text_acno.Text.Length != 12)
            //{
            //    MessageBox.Show("AC No Sould be Length 12 Chara..");
            //    text_acno.Focus();
            //}
            //else if (text_bankCode.Text.Length != 4)
            //{
            //    MessageBox.Show("Bank Code Sould be Length 4 Chara..");
            //    text_bankCode.Focus();
            //}
            //else if (text_branchCode.Text.Length != 3)
            //{
            //    MessageBox.Show("Bank Code Sould be Length 3 Chara..");
            //    text_branchCode.Focus();
            //}
            //else
            //{
            //    try
            //    {

            //        string path = "C:\\Java Solution\\EVEMC\\" + "EVEMC" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Second + ".txt";
            //        string path2 = "C:\\Java Solution\\EVEMP\\" + "EVEMP" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Minute + DateTime.Now.Hour + DateTime.Now.Second + ".txt";



            //        db = new DB();
            //        db2 = new DB();
            //        db.setCursoerDefault();
            //        conn = db.createSqlConnection();
            //        conn2 = db2.createSqlConnection();
            //        conn.Close();
            //        conn2.Close();
            //        db.setCursoerWait();


            //        double employee = 0.0, member = 0.0, contribution = 0.0, gross = 0.0;
            //        var cc = 0;
            //        using (StreamWriter writer = new StreamWriter(path))
            //        {
            //            conn.Open();
            //            reader = new SqlCommand("select a.*,b.name,b.epfno,b.nic,b.name,b.initials,b.lastname,b.empid,b.line from paysheet as a,emp as b where a.month_2='" + ComboBoxYear.Value.Year + "/" + comboBox1.SelectedItem + "' and a.empID_1=b.id order by b.tempepfno", conn).ExecuteReader();

            //            while (reader.Read())
            //            {
            //                var tt = reader[57].ToString();
            //                if (reader[57].ToString().Equals("158"))
            //                {
            //                    var dd = "";
            //                }
            //                var grade = "";
            //                conn2.Open();
            //                reader2 = new SqlCommand("select grade from line where name='" + reader[57] + "'", conn2).ExecuteReader();
            //                if (reader2.Read())
            //                {
            //                    grade = reader2[0] + "";
            //                }
            //                conn2.Close();
            //                if (grade.Equals(""))
            //                {
            //                    grade = "0";
            //                }
            //                conn2.Open();
            //                reader2 = new SqlCommand("select line from empBackup where id='" + reader[1] + "' and month='" + ComboBoxYear.Value.Year + "/" + comboBox1.SelectedItem + "' and isepf='" + true + "'", conn2).ExecuteReader();
            //                if (reader2.Read())
            //                {
            //                    cc++;
            //                    employee = employee + reader.GetDouble(43);
            //                    member = member + reader.GetDouble(29);
            //                    contribution = contribution + (reader.GetDouble(29) + reader.GetDouble(43));
            //                    gross = gross + reader.GetDouble(22);
            //                    writer.WriteLine(setWordLengthFillWithAfterSpace(reader[53] + "", 20) + setWordLengthFillWithAfterSpace(reader[55] + "", 40) + setWordLengthFillWithAfterSpace(reader[56] + "", 20) + setWordLengthFillWithBeforeSpace(reader[52] + "", 6) + setNumberLengthFillWithBeforeSpace((reader.GetDouble(29) + reader.GetDouble(43)), 10) + setNumberLengthFillWithBeforeSpace((reader.GetDouble(43)), 10) + setNumberLengthFillWithBeforeSpace((reader.GetDouble(29)), 10) + setNumberLengthFillWithBeforeSpace((reader.GetDouble(22)), 12) + "EI" + setWordLengthFillWithBeforeSpace("25762", 6) + ComboBoxYear.Value.Year + "" + db.getMOnth(comboBox1.SelectedItem.ToString()) + " 1" + setNumberLengthFillWithBeforeSpace((reader.GetDouble(7)), 5) + setWordLengthFillWithBeforeSpace(grade, 3));

            //                }
            //                conn2.Close();


            //            }
            //            conn.Close();
            //        }

            //        using (StreamWriter writer2 = new StreamWriter(path2))
            //        {
            //            writer2.WriteLine("I" + setWordLengthFillWithBeforeSpace("25762", 6) + ComboBoxYear.Value.Year + db.getMOnth(comboBox1.SelectedItem.ToString()) + " 1" + setNumberLengthFillWithBeforeSpace((contribution), 12) + setWordLengthFillWithBeforeSpace(cc + "", 6) + "4" + text_bankCode.Text + text_branchCode.Text + text_acno.Text + "  " + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + "10");

            //        }
            //        label_employee.Text = "Total Employer's " + db.setAmountFormat(employee + "");
            //        label_employer.Text = "Total Member's " + db.setAmountFormat(member + "");
            //        label_contribution.Text = "Total Contribution's " + db.setAmountFormat(contribution + "");
            //        label_gross.Text = "Total Contribution's " + db.setAmountFormat(gross + "");
            //        label_count.Text = "Total Member Count " + db.setAmountFormat(cc + "");
            //        Process.Start("explorer.exe", "C:\\Java Solution\\EVEMC\\");
            //        Process.Start("explorer.exe", "C:\\Java Solution\\EVEMP\\");
            //        db.setCursoerDefault();

            //    }
            //    catch (Exception a)
            //    {
            //        MessageBox.Show(a.Message + "/" + a.StackTrace);
            //    }
            //  }
        }
        string tempValue;
        string setWordLengthFillWithAfterSpace(string value, int length)
        {
            tempValue = "";
            if (value.ToCharArray().Length < length)
            {
                tempValue = value;
                var a = length - value.ToCharArray().Length;
                for (int i = 0; i < a; i++)
                {
                    tempValue = tempValue + " ";
                }
            }
            else
            {
                tempValue = "";
                for (int i = 0; i < length; i++)
                {
                    tempValue = tempValue + value.ToCharArray()[i];
                }

            }

            return tempValue;
        }
        string setWordLengthFillWithBeforeSpace(string value, int length)
        {
            tempValue = "";
            if (value.ToCharArray().Length < length)
            {
                tempValue = value;
                var a = length - value.ToCharArray().Length;
                for (int i = 0; i < a; i++)
                {
                    tempValue = " " + tempValue;
                }
            }
            else
            {
                tempValue = "";
                for (int i = 0; i < length; i++)
                {
                    tempValue = tempValue + value.ToCharArray()[i];
                }

            }

            return tempValue;
        }
        string setNumberLengthFillWithBeforeSpace(double value, int length)
        {
            var temp_double = Math.Round(value, 2);
            tempValue = "";
            var tempValue2 = "";
            if (temp_double.ToString().Split('.').Length > 1)
            {
                tempValue = temp_double.ToString().Split('.')[0] + "." + temp_double.ToString().Split('.')[1];
            }
            else
            {
                tempValue = temp_double.ToString() + ".00";
            }

            if (tempValue.Length <= length)
            {
                tempValue2 = tempValue;
                var a = length - tempValue.ToCharArray().Length;
                for (int i = 0; i < a; i++)
                {
                    tempValue = " " + tempValue;
                }
            }
            else
            {
                tempValue = "";
                for (int i = 0; i < length; i++)
                {
                    tempValue = tempValue + "0";
                }

            }

            return tempValue;
        }
        protected void btn_export_Click(object sender, EventArgs e)
        {

        }

        protected void btn_exportText1_Click(object sender, EventArgs e)
        {
            var tt = "";

        }

        protected void btn_save_Click(object sender, EventArgs e)
        {


        }
    }
}