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
    public partial class ProductPrice : Form
    {

        // For Resizing Of the Form
        private int _lastFormSize;

        //ForData gridViewstyling getter and setters
        public System.Windows.Forms.DataGridViewCellStyle RowHeadersDefaultCellStyle { get; set; }

        //For Padding the getter and setters 
        public new System.Windows.Forms.Padding Padding { get; set; }

        //For Changing Form FontSize a/c to change in size
        public int CUSTOM_CONTENT_HEIGHT { get; private set; }

        private void UpdateFont()
        {
            //Change cell font
            foreach (DataGridViewColumn c in dgv1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 12F, GraphicsUnit.Pixel);
            }
        }

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


        public ProductPrice()
        {
            InitializeComponent();
        }

        private void ProductPrice_Load(object sender, EventArgs e)
        {
            dgv_1();
            txtSearchPrice.Focus();
            LoadGridView1();
        }

        public void dgv_1()
        {
            dgv1.RowTemplate.Height = 40;

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

        public void LoadGridView1()
        {
            dgv1.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select * from Products";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();

            dgv1.Columns[0].HeaderText = "PRODUCT ID";
            dgv1.Columns[1].HeaderText = "PRODUCT NAME";
            dgv1.Columns[2].HeaderText = "PRODUCT PRICE";
            dgv1.Columns[3].HeaderText = "PRODUCT CATEGORY";
            dgv1.Columns[4].HeaderText = "PRODUCT IMAGE";
            dgv1.Columns[3].Visible = false;
            dgv1.Columns[4].Visible = false;

            for (int i = 0; i < dgv1.Columns.Count; i++)
                if (dgv1.Columns[i] is DataGridViewImageColumn)
                {
                    ((DataGridViewImageColumn)dgv1.Columns[i]).ImageLayout = DataGridViewImageCellLayout.Stretch;
                    break;
                }
        }

        private void SearchPrice_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtSearchPrice.Text == string.Empty)
                {
                    MessageBox.Show("Product name cannot be blank");
                    txtSearchPrice.Focus();
                }
                else
                {
                    SqlConnection con = new SqlConnection(Helper.con);
                    con.Open();
                    string query = "select * from Products where ProductName = '" + txtSearchPrice.Text + "' ";
                    SqlCommand cmd = new SqlCommand(query, con);
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dgv1.DataSource = dt;
                    con.Close();

                    txtSearchPrice.Text = string.Empty;
                    txtSearchPrice.Focus();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error searching product name");
            }
        }
    }
}
