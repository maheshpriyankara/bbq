using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
               WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlDataReader reader;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                reader = new SqlCommand("select leaveType from leave where userId='" + "1" + "' and date='" + "19" + "-" + "02" + "-" + "2019" + "' ", con).ExecuteReader();
                if (reader.Read())
                {
                    Response.Write("<script>alert('Hy')</script>");


                }
                else {
                    Response.Write("<script>alert('Hy2')</script>");
                }
                con.Close();
            }
            catch (Exception a)
            {
                Response.Write("<script>alert('"+a.Message+"/"+a.StackTrace+"')</script>");
            }
        }
    }
}