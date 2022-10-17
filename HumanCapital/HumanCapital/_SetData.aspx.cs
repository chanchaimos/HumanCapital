using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _SetData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetControl();
        }
    }

    public void SetControl()
    {
        string[] userindname = HttpContext.Current.User.Identity.Name.Split('\\');//new string[] { "", "26007098" };
        txtResult.Text = SystemFunction.GetSysCon() + Environment.NewLine + "AD :" + (userindname.Length >= 2 ? userindname[1] + "" : "");
    }
}