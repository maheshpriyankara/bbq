using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bbq
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
            WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlDataReader reader;
        protected void Page_Load(object sender, EventArgs e)
        {
            string myDate = "31-12-2019";
            // DateTime dt1 = DateTime.ParseExact(myDate, "dd-MM-yyyy",CultureInfo.InvariantCulture);
            con.Open();
            new SqlCommand("insert into leave values ('" + "1" + "','" + "ANNUAL" + "','" + myDate + "','" + DateTime.Now + "','" + DateTime.Now + "','" + "1" + "','" + "1" + "','" + "1" + "','" + false + "')", con).ExecuteNonQuery();
            reader = new SqlCommand("select leaveType,approved,date,dateRequst from leave where userId='" + "1" + "' and date between '" + "01-01-" + DateTime.Now.Year + "' and '" + "31-12-" + DateTime.Now.Year + "'", con).ExecuteReader();

            while (reader.Read())
            {
               // Label1.Text = reader.GetDateTime(2).ToShortDateString();
            }
            con.Close();
        }
    }
}