using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bbq
{
    public partial class applicant_dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                list_remarkList.Items.Add("All Profiles");
                list_remarkList.Items.Add("Remarked Profiles");
                list_remarkList.Items.Add("Not Remarked Profiles");
            }
           
        }
       
    }
}