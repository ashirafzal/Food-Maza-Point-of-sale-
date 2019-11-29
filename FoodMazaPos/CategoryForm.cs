using Foodies;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FoodMazaPos
{
    public partial class CategoryForm : Form
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

        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {

        }

        private void browseimage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dialog.FileName.ToString();
                pictureBox1.ImageLocation = imgLocation;
            }
        }

        private void CreateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.con);

                byte[] images = null;
                FileStream Stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
                BinaryReader brs = new BinaryReader(Stream);
                images = brs.ReadBytes((int)Stream.Length);

                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                string sqlQuery = "insert into Category(CategoryName,CategoryImage) values ('" + txtCategory.Text.ToLower() + "',@images)";
                cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.Add(new SqlParameter("@images", images));
                cmd.ExecuteNonQuery();
                con.Close();
                txtCategory.Clear();
                pictureBox1.Image = null;
                pictureBox1.Refresh();
                MessageBox.Show("Category Created Successfull");
            }
            catch (Exception)
            {
                MessageBox.Show("Fields can't be Empty");
            }
        }
    }
}
