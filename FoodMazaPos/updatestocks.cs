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
    public partial class updatestocks : Form
    {
        int StockID;

        public updatestocks()
        {
            InitializeComponent();
        }

        private void updatestocks_Load(object sender, EventArgs e)
        {
            LoadGridView1();
            dgv_1();
            ToFillGridview();
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
                    MessageBox.Show("Stock updated");
                    RefreshGrid();
                }

                con.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Please fill all required fields");
            }
        }

        public void RefreshGrid()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                string query = "SELECT * from Stock where stockweigth = '0' ";
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
            catch (Exception)
            {
                MessageBox.Show("Error updating grid veiw");
            }
        }

        public void ToFillGridview()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                string query = "SELECT * from Stock where stockweigth = '0' ";
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
            catch (Exception)
            {
                MessageBox.Show("Error finding stock value 0");
            }
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv1_CellClick_1(object sender, DataGridViewCellEventArgs e)
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
    }
}