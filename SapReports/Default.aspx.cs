using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;
using SapConnect.Reports.SapSalesService;
using SapConnect.Reports;
using System.Collections;
using System.Drawing;
using DotNet.Highcharts.Enums;

namespace SapReports
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SalesOrderDataAccess db = new SalesOrderDataAccess();
            var orders = db.GetAllOrders();
            var customers = orders.Select(order => order.CustomerName).ToArray();
            var netAmount = orders.Select(order => order.TotalSum).ToList();
            var netTax = orders.Select(order => order.Tax).ToList();
            List<object> amount = new List<object>();
            List<object> tax = new List<object>();
            List<object> categories = new List<object>();

            
            netAmount.ForEach(delegate(decimal net)
            {
                amount.Add((object)net);
            });

            netTax.ForEach(delegate(decimal net)
            {
                tax.Add((object)net);
            });


            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
            .InitChart(new Chart
            {
                Height = 300,
                Width = 600,
                DefaultSeriesType = ChartTypes.Bar,
                ClassName = "chart"
            })
            .SetTitle(new Title
                          {
                              Text = "Sales Orders By Company",
                              X = -20
                          })
            .SetXAxis(new XAxis
            {
                Categories = customers
            })
            .SetYAxis(new YAxis
            {
                Title = new YAxisTitle { Text = "In Dollars" },
            })
            .SetSeries(new Series[]
            {
                new Series { Name = "Amount", Data = new Data(amount.ToArray()) },
                new Series { Name = "Tax", Data = new Data(tax.ToArray()) },
            });

            ltrChart.Text = chart.ToHtmlString();

        }
    }
}
