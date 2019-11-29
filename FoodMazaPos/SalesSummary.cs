using Foodies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodMazaPos
{
    public partial class SalesSummary : Form
    {
        /*Connection String*/
        SqlConnection con = new SqlConnection(Helper.con);

        /*Declaring product and cashier string,integerandimage type variable to be read by the dr and then saved in these
         declarations*/
        string Productname;
        int Productprice;
        int Productquantity;
        string Categoryname;

        /*Declaring runtime labels and pictureboxes*/
        Panel pnl;
        Panel pnl2;
        Label label;
        Label label2;

        /*DECLARING A INTEGER FOR PRODUCT TOTAL AMOUNT COUNT*/
        int totalAmount;
        int totalqty;

        /*Declaring integer totalsales for sales estimation of different days*/
        int TotalSales;
        public SalesSummary()
        {
            InitializeComponent();
        }

        private void SalesSummary_Load(object sender, EventArgs e)
        {
            SalesEstimation();
            Loadproducts();
        }

        public void SalesEstimation()
        {
            SqlDataAdapter adapter3 = new SqlDataAdapter("SELECT * from Bill", con);
            DataTable table3 = new DataTable();
            adapter3.Fill(table3);
            if (table3.Rows.Count > 0)
            {
                var firstdayofyear = DateTime.Now.Year;
                var currentdate = DateTime.Now.Date;

                string query = "select * from Bill where OrderDate between '" + firstdayofyear + "' and '" + currentdate + "' ";
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgv2.DataSource = dt;

                int TotalSales = 0;

                for (int i = 0; i < dgv2.Rows.Count; ++i)
                {
                    TotalSales += Convert.ToInt32(dgv2.Rows[i].Cells[7].Value);
                }

                annualsales.Text = TotalSales.ToString() + " Rs";
                dgv2.Refresh();
                dgv2.DataSource = null;
            }
            else
            {
                annualsales.Text = "0" + " Rs";
            }

            SqlDataAdapter adapter4 = new SqlDataAdapter("SELECT * from Bill", con);
            DataTable table4 = new DataTable();
            adapter4.Fill(table4);
            if (table4.Rows.Count > 0)
            {
                var firstdayofweek = DateTime.Now.AddDays(-6);
                var currentdate = DateTime.Now.Date;

                string query = "select * from Bill where OrderDate between '" + firstdayofweek + "' and '" + currentdate + "' ";
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgv2.DataSource = dt;

                int TotalSales = 0;

                for (int i = 0; i < dgv2.Rows.Count; ++i)
                {
                    TotalSales += Convert.ToInt32(dgv2.Rows[i].Cells[7].Value);
                }

                lastweeksales.Text = TotalSales.ToString() + " Rs";
                dgv2.Refresh();
                dgv2.DataSource = null;
            }
            else
            {
                lastweeksales.Text = "0" + " Rs";
            }

            SqlDataAdapter adapter5 = new SqlDataAdapter("SELECT * from Bill", con);
            DataTable table5 = new DataTable();
            adapter5.Fill(table5);
            if (table5.Rows.Count > 0)
            {
                var firstdayofweek = DateTime.Now.AddDays(-6);
                var currentdate = DateTime.Now.Date;

                string query = "select * from Bill where OrderDate = '" + currentdate + "' ";
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgv2.DataSource = dt;

                int TotalSales = 0;

                for (int i = 0; i < dgv2.Rows.Count; ++i)
                {
                    TotalSales += Convert.ToInt32(dgv2.Rows[i].Cells[7].Value);
                }

                todaysales.Text = TotalSales.ToString() + " Rs";
                dgv2.Refresh();
                dgv2.DataSource = null;
            }
            else
            {
                todaysales.Text = "0" + " Rs";
            }

            SqlDataAdapter adapter6 = new SqlDataAdapter("SELECT * from Bill", con);
            DataTable table6 = new DataTable();
            adapter6.Fill(table6);
            if (table6.Rows.Count > 0)
            {
                var firstdayofmonth = DateTime.Now.AddDays(-29);
                var currentdate = DateTime.Now.Date;

                string query = "Select * from Bill where OrderDate between '" + firstdayofmonth + "' and '" + currentdate + "' ";
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgv2.DataSource = dt;

                int TotalSales = 0;

                for (int i = 0; i < dgv2.Rows.Count; ++i)
                {
                    TotalSales += Convert.ToInt32(dgv2.Rows[i].Cells[7].Value);
                }

                lastmonthsales.Text = TotalSales.ToString() + " Rs";
                dgv2.Refresh();
                dgv2.DataSource = null;
            }
            else
            {
                lastmonthsales.Text = "0" + " Rs";
            }

        }

        public void Loadproducts()
        {
            using (SqlConnection con = new SqlConnection(Helper.con))
            {
                SqlConnection conn = new SqlConnection(Helper.con);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                SqlCommand cmd10 = new SqlCommand("SELECT * FROM Products", conn, tran);
                cmd10.ExecuteNonQuery();

                string CommandText = "SELECT * FROM Products";
                using (SqlCommand cmd2 = new SqlCommand(CommandText, con))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd2);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    using (SqlDataReader dr2 = cmd10.ExecuteReader())
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            while (dr2.Read())
                            {
                                Categoryname = Convert.ToString(dr2["ProductName"]);

                                SqlCommand cmd11 = new SqlCommand("SELECT * FROM Bill where ProductName = '" + Categoryname + "' ", conn, tran);
                                cmd11.ExecuteNonQuery();

                                dgv1.Rows.Clear();
                                dgv1.Refresh();
                                dgv1.DataSource = null;

                                string CommandText2 = "SELECT * FROM Products where ProductName = '" + Categoryname + "' ";
                                using (SqlCommand cmd3 = new SqlCommand(CommandText2, con))
                                {
                                    SqlDataAdapter sda2 = new SqlDataAdapter(cmd3);
                                    DataSet ds2 = new DataSet();
                                    sda2.Fill(ds2);

                                    using (SqlDataReader dr3 = cmd11.ExecuteReader())
                                    {
                                        foreach (DataRow dr4 in ds2.Tables[0].Rows)
                                        {
                                            while (dr3.Read())
                                            {
                                                totalAmount = 0;
                                                totalqty = 0;

                                                Productname = Convert.ToString(dr3["ProductName"]);
                                                Productprice = Convert.ToInt32(dr3["ProductAmount"]);
                                                Productquantity = Convert.ToInt32(dr3["ProductQuantity"]);

                                                dgv1.Rows.Add(Productname, Productprice, Productquantity);

                                                for (int j = 0; j < dgv1.Rows.Count; ++j)
                                                {
                                                    totalAmount += Convert.ToInt32(dgv1.Rows[j].Cells[1].Value);
                                                    totalqty += Convert.ToInt32(dgv1.Rows[j].Cells[2].Value);
                                                }
                                            }
                                        }
                                    }

                                    pnl = new Panel
                                    {
                                        Size = new System.Drawing.Size(890, 50),
                                        Location = new Point(-1, -1),
                                        BackColor = Color.White,
                                    };

                                    pnl2 = new Panel
                                    {
                                        Size = new System.Drawing.Size(450, 50),
                                        Location = new Point(-1, -1),
                                        BackColor = Color.SkyBlue,
                                    };

                                    label = new Label
                                    {
                                        Size = new System.Drawing.Size(450, 50),
                                        Font = new Font("Arial", 14, FontStyle.Bold),
                                        Location = new Point(10, 0),
                                        BackColor = Color.Transparent,
                                        ForeColor = Color.Black,
                                        Text = Productname,
                                        TextAlign = ContentAlignment.MiddleLeft,
                                    };

                                    label2 = new Label
                                    {
                                        Size = new System.Drawing.Size(440, 50),
                                        Font = new Font("Arial", 14, FontStyle.Bold),
                                        Location = new Point(460, 0),
                                        BackColor = Color.White,
                                        ForeColor = Color.Black,
                                        Text = totalAmount + " Rs",
                                        TextAlign = ContentAlignment.MiddleLeft,
                                    };

                                    flowLayoutPanel1.Controls.Add(pnl);
                                    pnl.Controls.Add(pnl2);
                                    pnl2.Controls.Add(label);
                                    pnl.Controls.Add(label2);
                                    /*Total becomes 0 otherwise it will add this totalamount into the next total which will makes it addition*/
                                    totalAmount = 0;
                                    totalqty = 0;
                                }
                            }
                        }

                    }
                }
            }

            /*To clear the dgv1 after all the calcualtion done*/
            dgv1.Rows.Clear();
            dgv1.Refresh();
            dgv1.DataSource = null;

            /*Method to check the dynamic total and total amount label total on dynamic panles*/
            //abondenedcheckingcodeForTotalAmount();
        }
    }
}
