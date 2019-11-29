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
    public partial class Stocks : Form
    {
        int StockID; int stockweigth;
        string stockname, stockcompany, stockcategory;

        public Stocks()
        {
            InitializeComponent();
        }

        private void Stocks_Load(object sender, EventArgs e)
        {
            dgv_1();
            LoadGridView1();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();

                if (StockName.Text == string.Empty)
                {
                    MessageBox.Show("Stock name is required");
                }
                else if (StockWeight.Text == string.Empty)
                {
                    MessageBox.Show("Stock weight is required");
                }
                else if (StockCategory.Text == string.Empty)
                {
                    MessageBox.Show("Stock category is required");
                }
                else if (StockCompany.Text == string.Empty)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT stockname,stockcategory from Stock where stockname = '" + StockName.Text.ToLower() + "' and stockcategory = '" + StockCategory.Text.ToLower() + "' ", con);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        DialogResult result = MessageBox.Show("Stock already present do you want to add more?\nPress Yes for adding more stock\nPress No to create new stock record." +
                            "By creating a new record your previous stock data will be deleted and new record will be created.", "Stock Already Exist", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            SqlTransaction tran = con.BeginTransaction();

                            SqlCommand cmd1 = new SqlCommand("select * from Stock where stockname = '" + StockName.Text.ToLower() + "' and stockcategory = '" + StockCategory.Text.ToLower() + "' ", con, tran);
                            cmd1.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd1.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);

                                    stockweigth = stockweigth + Convert.ToInt32(StockWeight.Text);
                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + stockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + DateTime.Now.ToShortDateString() + "', stocktime = '" + DateTime.Now.ToShortTimeString() + "' where stockname = '" + StockName.Text + "' and stockcategory = '" + StockCategory.Text + "'  ", con, tran);
                            cmd2.ExecuteNonQuery();

                            StockCompany.Text = string.Empty;
                            StockCategory.Text = string.Empty;
                            StockName.Text = string.Empty;
                            StockWeight.Text = string.Empty;
                            MessageBox.Show("Stock updated successfully");
                            tran.Commit();
                            LoadGridView1();
                        }
                        else if (result == DialogResult.No)
                        {
                            string companyname = "local company";
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "Delete from Stock where stockname = '" + StockName.Text.ToLower() + "' and stockcategory = '" + StockCategory.Text.ToLower() + "' ";
                            cmd.ExecuteNonQuery();
                            SqlCommand cmd2 = con.CreateCommand();
                            cmd2.CommandType = CommandType.Text;
                            cmd2.CommandText = "insert into Stock (stockname,stockweigth,stockcompany,stockcategory,stockdate,stocktime) values ('" + StockName.Text.ToLower() + "','" + StockWeight.Text + "','" + companyname.ToString().ToLower() + "','" + StockCategory.Text.ToLower() + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "')";
                            cmd2.ExecuteNonQuery();
                            StockCompany.Text = string.Empty;
                            StockCategory.Text = string.Empty;
                            StockName.Text = string.Empty;
                            StockWeight.Text = string.Empty;
                            LoadGridView1();
                            MessageBox.Show("Stock created successfully");
                        }
                    }
                    else if (table.Rows.Count == 0)
                    {
                        string companyname = "local company";
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into Stock (stockname,stockweigth,stockcompany,stockcategory,stockdate,stocktime) values ('" + StockName.Text.ToLower() + "','" + StockWeight.Text + "','" + companyname.ToString().ToLower() + "','" + StockCategory.Text.ToLower() + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "')";
                        cmd.ExecuteNonQuery();
                        StockCompany.Text = string.Empty;
                        StockCategory.Text = string.Empty;
                        StockName.Text = string.Empty;
                        StockWeight.Text = string.Empty;
                        LoadGridView1();
                        MessageBox.Show("Stock created successfully");
                    }
                }
                else
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT stockname,stockcategory from Stock where stockname = '" + StockName.Text.ToLower() + "' and stockcategory = '" + StockCategory.Text.ToLower() + "' ", con);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        DialogResult result = MessageBox.Show("Stock already present do you want to add more?\nPress Yes for adding more stock\nPress No to create new stock record." +
                            "By creating a new record your previous stock data will be deleted and new record will be created.", "Stock Already Exist", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            SqlTransaction tran = con.BeginTransaction();

                            SqlCommand cmd1 = new SqlCommand("select * from Stock where stockname = '" + StockName.Text.ToLower() + "' and stockcategory = '" + StockCategory.Text.ToLower() + "' ", con, tran);
                            cmd1.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd1.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);

                                    stockweigth = stockweigth + Convert.ToInt32(StockWeight.Text);
                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + stockweigth + "', stockcompany = '" + StockCompany.Text + "', stockcategory = '" + stockcategory + "', stockdate = '" + DateTime.Now.ToShortDateString() + "', stocktime = '" + DateTime.Now.ToShortTimeString() + "' where stockname = '" + StockName.Text + "' and stockcategory = '" + StockCategory.Text + "'  ", con, tran);
                            cmd2.ExecuteNonQuery();

                            StockCompany.Text = string.Empty;
                            StockCategory.Text = string.Empty;
                            StockName.Text = string.Empty;
                            StockWeight.Text = string.Empty;
                            MessageBox.Show("Stock updated successfully");
                            tran.Commit();
                            LoadGridView1();
                        }
                        else if (result == DialogResult.No)
                        {
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "Delete from Stock where stockname = '" + StockName.Text.ToLower() + "' and stockcategory = '" + StockCategory.Text.ToLower() + "' ";
                            cmd.ExecuteNonQuery();
                            SqlCommand cmd2 = con.CreateCommand();
                            cmd2.CommandType = CommandType.Text;
                            cmd2.CommandText = "insert into Stock (stockname,stockweigth,stockcompany,stockcategory,stockdate,stocktime) values ('" + StockName.Text.ToLower() + "','" + StockWeight.Text + "','" + StockCompany.Text.ToLower() + "','" + StockCategory.Text.ToLower() + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "')";
                            cmd2.ExecuteNonQuery();
                            StockCompany.Text = string.Empty;
                            StockCategory.Text = string.Empty;
                            StockName.Text = string.Empty;
                            StockWeight.Text = string.Empty;
                            LoadGridView1();
                            MessageBox.Show("Stock created successfully");
                        }
                    }
                    else if (table.Rows.Count == 0)
                    {
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into Stock (stockname,stockweigth,stockcompany,stockcategory,stockdate,stocktime) values ('" + StockName.Text.ToLower() + "','" + StockWeight.Text + "','" + StockCompany.Text.ToLower() + "','" + StockCategory.Text.ToLower() + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "')";
                        cmd.ExecuteNonQuery();
                        StockCompany.Text = string.Empty;
                        StockCategory.Text = string.Empty;
                        StockName.Text = string.Empty;
                        StockWeight.Text = string.Empty;
                        LoadGridView1();
                        MessageBox.Show("Stock Created Successfully");
                    }
                }
                con.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Please fill all required fields");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();

            try
            {

                if (StockName.Text == string.Empty)
                {
                    MessageBox.Show("Stock name is required");
                }
                else if (StockWeight.Text == string.Empty)
                {
                    MessageBox.Show("Stock weight is required");
                }
                else if (StockCategory.Text == string.Empty)
                {
                    MessageBox.Show("Stock category is required");
                }
                else if (StockCompany.Text == string.Empty)
                {
                    MessageBox.Show("Stock company is required");
                }
                else
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update Stock set stockname = '" + StockName.Text.ToLower() + "' , stockweigth = '" + StockWeight.Text + "', stockcompany = '" + StockCompany.Text.ToLower() + "', stockcategory = '" + StockCategory.Text.ToLower() + "', stockdate = '" + DateTime.Now.ToShortDateString() + "', stocktime = '" + DateTime.Now.ToShortTimeString() + "' where stockid = '" + StockID + "'  ";
                    cmd.ExecuteNonQuery();
                    StockCompany.Text = string.Empty;
                    StockCategory.Text = string.Empty;
                    StockName.Text = string.Empty;
                    StockWeight.Text = string.Empty;
                    LoadGridView1();
                    MessageBox.Show("Stock updated");
                }

                con.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Please fill all required fields");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from Stock where stockid ='" + StockID + "'";
                cmd.ExecuteNonQuery();
                con.Close();
                StockCompany.Text = string.Empty;
                StockCategory.Text = string.Empty;
                StockName.Text = string.Empty;
                StockWeight.Text = string.Empty;
                LoadGridView1();
                MessageBox.Show("Stock deleted");

            }
            catch (Exception)
            {
                MessageBox.Show("Please fill all required fields");
            }
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgv1.Rows[e.RowIndex];

                StockID = Convert.ToInt32(row.Cells[0].Value);
                StockName.Text = row.Cells[1].Value.ToString();
                StockWeight.Text = row.Cells[2].Value.ToString();
                StockCompany.Text = row.Cells[3].Value.ToString();
                StockCategory.Text = row.Cells[4].Value.ToString();
            }
        }

        public void LoadGridView1()
        {
            dgv1.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select * from Stock";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();

            dgv1.Columns[0].HeaderText = "STOCK ID";
            dgv1.Columns[1].HeaderText = "STOCK NAME";
            dgv1.Columns[2].HeaderText = "STOCK WEIGTH";
            dgv1.Columns[3].HeaderText = "STOCK COMPANY";
            dgv1.Columns[4].HeaderText = "STOCK CATEGORY";
            dgv1.Columns[5].HeaderText = "STOCK DATE";
            dgv1.Columns[6].HeaderText = "STOCK TIME";
        }

        public void dgv_1()
        {
            dgv1.RowTemplate.Height = 42;

            //This Part of Code is for the styling of the Grid Padding
            Padding newPadding = new Padding(0, 10, 0, 10);
            this.dgv1.ColumnHeadersDefaultCellStyle.Padding = newPadding;

            //This Part of Code is for the styling of the Grid Columns
            dgv1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);
            dgv1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // This Part of Code is for the styling of the Grid Border
            this.dgv1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgv1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //This Part of Code is for the styling of the Grid Rows
            dgv1.RowsDefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }


    }
}
