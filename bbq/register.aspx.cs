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

namespace bbq
{
    public partial class register : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
            WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlDataReader reader;
        SqlConnection con2 = new SqlConnection(
           WebConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlDataReader reader2;

        ArrayList profile_list, accounts_list, accounts_list_sub, accounts_list_sub_;
        string userID_, cc_;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] != null && Session["userType"] != null)
            {
                try
                {
                    con.Open();
                    reader = new SqlCommand("select account_type from sa_login where user_name='" + Session["userID"] + "'", con).ExecuteReader();
                    if (reader.Read())
                    {
                        var a = reader.GetString(0);
                        if (reader.GetString(0).Equals("user"))
                        {
                            menu_administartion_head.Visible = false;
                            menu_authorization.Visible = false;
                            menu_verification.Visible = false;
                        }
                        else if (reader.GetString(0).Equals("authorize"))
                        {
                            menu_finance_head.Visible = false;
                            menu_data_entry.Visible = false;
                            menu_settings.Visible = false;
                            menu_verification.Visible = false;
                            menu_administartion_head.Visible = false;

                        }
                        else if (reader.GetString(0).Equals("verify"))
                        {
                            menu_administartion_head.Visible = false;
                            menu_finance_head.Visible = false;
                            menu_data_entry.Visible = false;
                            menu_reports.Visible = false;
                            menu_settings.Visible = false;
                            menu_authorization.Visible = false;
                        }
                    }
                    con.Close();
                }
                catch (Exception)
                {

                }
                Page.MaintainScrollPositionOnPostBack = true;

                profile_list = new ArrayList();
                loadAccounts();
                if (!IsPostBack)
                {



                    // dateTextBox1.Text = DateTime.Now.ToShortDateString();
                    // dateTextBox2.Text = DateTime.Now.ToShortDateString();


                    if (Session["userID"] != null)
                    {
                        userID_ = Session["userID"].ToString();
                        // radio_cash.Checked = true;
                        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:yesnoCheck(); ", true);
                    }
                    else
                    {
                        Response.Write("<script>alert('Sorry Time Out Expired, Please Login Agian')</script>");
                        Response.Redirect("~/default.aspx");
                    }
                }

            }
            else
            {
                Response.Redirect("login.aspx");
            }
          

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Session["userID"] = null;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Session["userID"] = null;
            Response.Redirect("~/default.aspx");

        }
        DataTable dt;
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Session["userID"] != null)
            {

            }
            else
            {
                Response.Write("<script>alert('Sorry Time Out Expired, Please Login Agian')</script>");
                Response.Redirect("~/default.aspx");
            }
        }
        protected void Button1_Click1(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                reader = new SqlCommand("select name from sa_profile where name='" + name_company.Text + "' ", con).ExecuteReader();
                if (reader.Read())
                {
                    Response.Write("<script>alert('" + "You Have Enterd Duplicate Customer / Supplier Name" + "')</script>");
                }
                else
                {
                    con.Close();
                    con.Open();
                    new SqlCommand("insert into sa_profile values ('"+profile_type.Text+"','"+name_company.Text+"','"+address.Text+"','"+contact_number.Text+"','"+true+"')",con).ExecuteNonQuery();
                    con.Close();
                    new db().updateLogs(con, con2, reader, "Profile Create", "sa_profile", new db().getSysDateTime(), Session["userID"] + "");
                    name_company.Text = "";
                    address.Text = "";
                    contact_number.Text = "";
                    Response.Redirect("~/register.aspx");
                    loadAccounts();
                }
                con.Close();
            }
            catch (Exception)
            {
                con.Close();
            }

        }
        public void loadAccounts()
        {
            try
            {
                accounts_list = new ArrayList();
                dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Supplier Description"), new DataColumn("Name"), new DataColumn("Cheque Writer"), new DataColumn("Contact Number") });

                con.Open();
                reader = new SqlCommand("select a.* from sa_profile as a,sa_logs_ as b where b.category='" + "Profile Create" + "' and b.user_='" + Session["userID"] + "' and b.reference=a.id and b.created between '" + new db().getSysDateTime().ToShortDateString() + " 00:00" + "' and '" + new db().getSysDateTime().ToShortDateString()+ " 23:59" + "' order by a.id desc", con).ExecuteReader();
                while (reader.Read())
                {
                    dt.Rows.Add(reader[1], reader[2], reader[3], reader[4]);
                    accounts_list.Add(reader[0] + "");
                }
                con.Close();

                grid_accounts.DataSource = dt;
                grid_accounts.DataBind();

            }
            catch (Exception)
            {
                con.Close();
            }
        }
        public void loadAccountsCategory()
        {

        }
        protected void account_type_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void grid_accounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Supplier Description"), new DataColumn("Name"), new DataColumn("Address"), new DataColumn("Contact Number") });
            string deleteID = accounts_list[e.RowIndex] + "";
            con.Open();
            new SqlCommand("delete from sa_profile where id='" + deleteID + "'", con).ExecuteNonQuery();
            con.Close();
            new db().updateLogs(con, con2, reader, "Profile Delete", "sa_profile", new db().getSysDateTime(), Session["userID"] + "");
            accounts_list = new ArrayList();
            reader = new SqlCommand("select a.* from sa_profile as a,sa_logs_ as b where b.category='" + "Profile Create" + "' and b.user_='" + Session["userID"] + "' and b.reference=a.id and b.created between '" + new db().getSysDateTime() + " 00:00" + "' and '" + new db().getSysDateTime() + " 23:59" + "' order by a.id desc", con).ExecuteReader();
              
            while (reader.Read())
            {
                dt.Rows.Add(reader[1], reader[2], reader[3], reader[4]);
                accounts_list.Add(reader[0] + "");
            }
            con.Close();

            grid_accounts.DataSource = dt;
            grid_accounts.DataBind();


        }


        protected void sub_Accounts_grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {




        }

        protected void sub_Account_submit_Click(object sender, EventArgs e)
        {

        }

        protected void account_type_sub_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadAccountsCategory();
        }

    }
}