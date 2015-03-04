using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using DiscountInterface;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace MovieTicket
{
    public partial class Form1 : Form
    {
        Assembly asmb = Assembly.LoadFrom("DiscountClass.dll");

        private Dictionary<string, OrderType> dicOrderType = new Dictionary<string, OrderType>();

        public Form1()
        {
            InitializeComponent();
            AccessAppSettings();
            UpdateListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection lv1 = this.listView1.SelectedItems;
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                string kvp = item.Name;
                dicOrderType[kvp].qty = Convert.ToInt32(textBox1.Text);
                dicOrderType[kvp].total = dicOrderType[kvp].qty * dicOrderType[kvp].disprice;

                listView1.Items[kvp].SubItems[3].Text=(dicOrderType[kvp].qty.ToString());
                listView1.Items[kvp].SubItems[4].Text=(dicOrderType[kvp].total.ToString());
            }

            listView1.Refresh();

            int totalqty = 0;
            double totalamt = 0;

            foreach (string kvp in dicOrderType.Keys)
            {
                totalqty = totalqty + dicOrderType[kvp].qty;
                totalamt = totalamt + dicOrderType[kvp].total;
            }

            textBox2.Text = String.Format("{0:#,0}", totalqty);
            textBox3.Text = String.Format("{0:#,0}", totalamt);

        }

        private void UpdateListView()
        {
            listView1.BeginUpdate();
            listView1.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("身分別", 150, HorizontalAlignment.Center);
            listView1.Columns.Add("原價", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("折扣價", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("張數", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("小計", 100, HorizontalAlignment.Center);

            foreach (string kvp in dicOrderType.Keys)
            {
                listView1.Items.Add(kvp,dicOrderType[kvp].type_str ,0);                
                listView1.Items[kvp].SubItems.Add(dicOrderType[kvp].price.ToString());
                listView1.Items[kvp].SubItems.Add(dicOrderType[kvp].disprice.ToString());
                listView1.Items[kvp].SubItems.Add(dicOrderType[kvp].qty.ToString());
                listView1.Items[kvp].SubItems.Add(dicOrderType[kvp].total.ToString());
            }

            listView1.EndUpdate();
        }

        private void AccessAppSettings()
        {
            dicOrderType.Clear();

            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //根据Key读取<add>元素的Value
            foreach(string appkey in config.AppSettings.Settings.AllKeys)
            {
                string[] appvalue = config.AppSettings.Settings[appkey].Value.Split(';');
                string type_str = appvalue[0];
                double price = Convert.ToDouble(appvalue[1]);
                string distype_str = appvalue[2];
                double disprice = 0;

                if (distype_str.Length > 0)
                {
                    IDiscount obj = (IDiscount)asmb.CreateInstance(distype_str);
                    disprice = obj.Calculate(price);                
                }
                else
                    disprice = price;

                dicOrderType.Add(appkey, new OrderType
                {
                    type_str = type_str,
                    price = price,
                    distype_str = distype_str,
                    disprice = disprice,
                    qty = 0,
                    total = 0
                });

            }
        }
    }

    internal class OrderType
    {
        public string type_str;
        public double price;
        public string distype_str;
        public double disprice;
        public int qty;
        public double total;
    }
}
