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
    public partial class users : Form
    {
        string UserID;

        public users()
        {
            InitializeComponent();
        }

        private void users_Load(object sender, EventArgs e)
        {
            dgv_1();
            LoadGridView1();
        }

        public void LoadGridView1()
        {
            try
            {
                dgv1.Refresh();
                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                string query = "select * from users";
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgv1.DataSource = dt;
                con.Close();

                dgv1.Columns[0].HeaderText = "USER ID";
                dgv1.Columns[1].HeaderText = "USER NAME";
                dgv1.Columns[2].HeaderText = "PASSWORD";
                dgv1.Columns[3].HeaderText = "ROLE / STATUS";
            }
            catch (Exception e)
            {
                MessageBox.Show("" + e.Message.ToString());
            }
        }

        public void dgv_1()
        {
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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into users (username,password,category) values ('" + Username.Text.ToLower() + "','" + Password.Text.ToLower() + "','" + category.Text.ToLower() + "')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("User Created Successfully");
                con.Close();
                Username.Text = string.Empty;
                Password.Text = string.Empty;
                category.Text = string.Empty;
                LoadGridView1();
            }
            catch (Exception)
            {
                MessageBox.Show("Please fill all required fields");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update users set username = '" + Username.Text.ToLower() + "' , password = '" + Password.Text.ToLower() + "', category = '" + category.Text.ToLower() + "' where Id = '" + UserID + "'  ";
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("User updated successfully");
                Username.Text = string.Empty;
                Password.Text = string.Empty;
                category.Text = string.Empty;
                LoadGridView1();
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
                cmd.CommandText = "delete from users where Id ='" + UserID + "'";
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("User deleted successfully");
                Username.Text = string.Empty;
                Password.Text = string.Empty;
                category.Text = string.Empty;
                LoadGridView1();
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

                UserID = row.Cells[0].Value.ToString();
                Username.Text = row.Cells[1].Value.ToString();
                Password.Text = row.Cells[2].Value.ToString();
                category.Text = row.Cells[3].Value.ToString();
            }
        }
    }
}