using Foodies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodMazaPos
{
    public partial class ManageCategory : Form
    {
        string imgLocation = "";

        // For Resizing Of the Form
        private int _lastFormSize;

        //ForData gridViewstyling getter and setters
        public System.Windows.Forms.DataGridViewCellStyle RowHeadersDefaultCellStyle { get; set; }

        //For Padding the getter and setters 
        public new System.Windows.Forms.Padding Padding { get; set; }

        //For Changing Form FontSize a/c to change in size
        public int CUSTOM_CONTENT_HEIGHT { get; private set; }


        private int GetFormArea(Size size)
        {
            return size.Height * size.Width;
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            float scaleFactor = (float)GetFormArea(control.Size) / (float)_lastFormSize;

            ResizeFont(this.Controls, scaleFactor);

            _lastFormSize = GetFormArea(control.Size);

        }

        private void ResizeFont(Control.ControlCollection coll, float scaleFactor)
        {
            foreach (Control c in coll)
            {
                if (c.HasChildren)
                {
                    ResizeFont(c.Controls, scaleFactor);
                }
                else
                {
                    //if (c.GetType().ToString() == "System.Windows.Form.Label")
                    if (true)
                    {
                        // scale font
                        c.Font = new Font(c.Font.FontFamily.Name, c.Font.Size * scaleFactor);
                    }
                }
            }
        }

        public void LoadGridView1()
        {
            dgv1.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select * from Category";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();

            dgv1.Columns[0].HeaderText = "CATEGORY ID";
            dgv1.Columns[1].HeaderText = "CATEGORY NAME";
            dgv1.Columns[2].HeaderText = "CATEGORY IMAGE";

            for (int i = 0; i < dgv1.Columns.Count; i++)
                if (dgv1.Columns[i] is DataGridViewImageColumn)
                {
                    ((DataGridViewImageColumn)dgv1.Columns[i]).ImageLayout = DataGridViewImageCellLayout.Stretch;
                    break;
                }
        }

        public void dgv_1()
        {
            dgv1.RowTemplate.Height = 200;

            //This Part of Code is for the styling of the Grid Padding
            Padding newPadding = new Padding(10, 8, 0, 8);
            this.dgv1.ColumnHeadersDefaultCellStyle.Padding = newPadding;

            //This Part of Code is for the styling of the Grid Columns
            dgv1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Regular);

            // This Part of Code is for the styling of the Grid Border
            this.dgv1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgv1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //This Part of Code is for the styling of the Grid Rows
            dgv1.RowsDefaultCellStyle.Font = new Font("Arial", 12F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Byte[] img = (Byte[])dgv1.CurrentRow.Cells[2].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox1.Image = Image.FromStream(ms);

            CategoryID.Text = dgv1.CurrentRow.Cells[0].Value.ToString();
            CategoryName.Text = dgv1.CurrentRow.Cells[1].Value.ToString();

        }

        private void dgv1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dgv1.Rows[e.RowIndex].ErrorText = "Concisely describe the error and how to fix it";
            e.Cancel = true;
        }

        public ManageCategory()
        {
            InitializeComponent();
        }

        private void ManageCategory_Load(object sender, EventArgs e)
        {
            dgv_1();
            LoadGridView1();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dialog.FileName.ToString();
                pictureBox1.ImageLocation = imgLocation;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] images = null;
                FileStream Stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
                BinaryReader brs = new BinaryReader(Stream);
                images = brs.ReadBytes((int)Stream.Length);

                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                int a1 = Convert.ToInt16(CategoryID.Text);
                string sqlQuery = "update Category set CategoryName = @CategoryName, CategoryImage = @images where CategoryId = '" + a1 + "'  ";
                cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.Add(new SqlParameter("@CategoryName", CategoryName.Text.ToLower()));
                cmd.Parameters.Add(new SqlParameter("@images", images));
                var N = cmd.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                con.Close();
                CategoryID.Text = string.Empty;
                CategoryName.Text = string.Empty;
                LoadGridView1();
                MessageBox.Show("Category updated");
            }
            catch (Exception)
            {
                MessageBox.Show("Please fill all required fields");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from Products where ProductCategory = '" + CategoryName.Text.ToLower() + "'  ", con);
            DataTable table = new DataTable();
            adapter.Fill(table);

            DialogResult result = MessageBox.Show("There are " + table.Rows.Count.ToString() + " products in this category.These Products will also be deleted with this category.Are you sure you want to continue this ? ", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from Category where CategoryName ='" + CategoryName.Text.ToLower() + "'";
                cmd.ExecuteNonQuery();

                SqlCommand cmd1 = con.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "delete from Products where ProductCategory ='" + CategoryName.Text.ToLower() + "'";
                cmd1.ExecuteNonQuery();

                CategoryID.Text = string.Empty;
                CategoryName.Text = string.Empty;
                LoadGridView1();
                MessageBox.Show("Category and products under this category deleted successfully");
            }
            else if (result == DialogResult.No)
            {
                //Do nothing
            }
            con.Close();
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Byte[] img = (Byte[])dgv1.CurrentRow.Cells[2].Value;
            //MemoryStream ms = new MemoryStream(img);
            //pictureBox1.Image = Image.FromStream(ms);

            CategoryID.Text = dgv1.CurrentRow.Cells[0].Value.ToString();
            CategoryName.Text = dgv1.CurrentRow.Cells[1].Value.ToString();
        }
    }
}
