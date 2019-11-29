using ClosedXML.Excel;
using Foodies;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FoodMazaPos
{
    public partial class ExportToExcel : Form
    {
        public ExportToExcel()
        {
            InitializeComponent();
        }

        private void ExportToExcel_Load(object sender, EventArgs e)
        {
            //DataGridVeiwDesignLoad
            dgv_1();
            dgv_2();
            dgv_3();
            dgv_4();
            dgv_5();
            dgv_6();
            dgv_7();

            /* LOADING ALL THE GRIDVIEWS DATA*/
            LoadGridView1();
            LoadGridView2();
            LoadGridView3();
            LoadGridView4();
            LoadGridView5();
            LoadGridView6();
            LoadGridView7();
        }

        public void LoadGridView1()
        {
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select * from Bill";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();

            dgv1.Columns[0].HeaderText = "INVOICE ID";
            dgv1.Columns[1].HeaderText = "CUST ID";
            dgv1.Columns[2].HeaderText = "ORDER ID";
            dgv1.Columns[3].HeaderText = "CUST NAME";
            dgv1.Columns[4].HeaderText = "PRODUCT NAME";
            dgv1.Columns[5].HeaderText = "PRODUCT QTY";
            dgv1.Columns[6].HeaderText = "PRODUCT RATE";
            dgv1.Columns[7].HeaderText = "PRODUCT AMOUNT";
            dgv1.Columns[8].HeaderText = "GST AMOUNT";
            dgv1.Columns[8].Visible = false;
            dgv1.Columns[9].HeaderText = "ORDER TIME";
            dgv1.Columns[10].HeaderText = "ORDER DATE";
            dgv1.Columns[11].HeaderText = "TOTAL QTY";
            dgv1.Columns[11].Visible = false;
            dgv1.Columns[12].HeaderText = "ACTUAL AMOUNT";
            dgv1.Columns[12].Visible = false;
            dgv1.Columns[13].HeaderText = "TOTAL AMOUNT";
            dgv1.Columns[13].Visible = false;
            dgv1.Columns[14].HeaderText = "TOTAL GST AMOUNT";
            dgv1.Columns[14].Visible = false;
            dgv1.Columns[15].HeaderText = "DISCOUNT";
            dgv1.Columns[15].Visible = false;
        }

        public void LoadGridView2()
        {
            dgv2.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select CategoryId,CategoryName from Category";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv2.DataSource = dt;
            con.Close();

            dgv2.Columns[0].HeaderText = "CATGEORY ID";
            dgv2.Columns[1].HeaderText = "CATGEORY NAME";
        }

        public void LoadGridView3()
        {
            dgv3.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select * from Customer";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv3.DataSource = dt;
            con.Close();

            dgv3.Columns[0].HeaderText = "CUST ID";
            dgv3.Columns[1].HeaderText = "CUST NAME";
            dgv3.Columns[2].HeaderText = "CUST CONTACT";
            dgv3.Columns[3].HeaderText = "ORDER TIME";
            dgv3.Columns[4].HeaderText = "ORDER DATE";
        }

        public void LoadGridView4()
        {
            dgv4.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select * from Orders";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv4.DataSource = dt;
            con.Close();

            dgv4.Columns[0].HeaderText = "ORDER ID";
            dgv4.Columns[1].HeaderText = "CUST ID";
            dgv4.Columns[2].HeaderText = "ORDER TYPE";
            dgv4.Columns[3].HeaderText = "ORDER CATEGORY";
            dgv4.Columns[4].HeaderText = "ORDER TIME";
            dgv4.Columns[5].HeaderText = "ORDER DATE";
        }

        public void LoadGridView5()
        {
            dgv5.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select ProductId,ProductName,ProductPrice,ProductCategory from Products";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv5.DataSource = dt;
            con.Close();

            dgv5.Columns[0].HeaderText = "PRODUCT ID";
            dgv5.Columns[1].HeaderText = "PRODUCT NAME";
            dgv5.Columns[2].HeaderText = "PRODUCT PRICE";
            dgv5.Columns[3].HeaderText = "PRODUCT CATEGORY";
        }

        public void LoadGridView6()
        {
            dgv6.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select * from Sales";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv6.DataSource = dt;
            con.Close();

            dgv6.Columns[0].HeaderText = "ORDER ID";
            dgv6.Columns[1].HeaderText = "CUST ID";
            dgv6.Columns[2].HeaderText = "CUST NAME";
            dgv6.Columns[3].HeaderText = "CONTACT";
            dgv6.Columns[4].HeaderText = "ORDER TYPE";
            dgv6.Columns[5].HeaderText = "ORDER CATEGORY";
            dgv6.Columns[6].HeaderText = "ORDER TIME";
            dgv6.Columns[6].HeaderText = "ORDER DATE";
        }

        public void LoadGridView7()
        {
            dgv7.Refresh();
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            string query = "select * from Stock";
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv7.DataSource = dt;
            con.Close();

            dgv7.Columns[0].HeaderText = "STOCK ID";
            dgv7.Columns[1].HeaderText = "STOCK NAME";
            dgv7.Columns[2].HeaderText = "STOCK WEIGTH";
            dgv7.Columns[3].HeaderText = "STOCK COMPANY";
            dgv7.Columns[4].HeaderText = "STOCK CATEGORY";
            dgv7.Columns[5].HeaderText = "STOCK DATE";
            dgv7.Columns[6].HeaderText = "STOCK TIME";
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
            dgv1.RowsDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public void dgv_2()
        {
            dgv2.RowTemplate.Height = 42;

            //This Part of Code is for the styling of the Grid Padding
            Padding newPadding = new Padding(0, 10, 0, 10);
            this.dgv2.ColumnHeadersDefaultCellStyle.Padding = newPadding;

            //This Part of Code is for the styling of the Grid Columns
            dgv2.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);
            dgv2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // This Part of Code is for the styling of the Grid Border
            this.dgv2.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgv2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //This Part of Code is for the styling of the Grid Rows
            dgv2.RowsDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public void dgv_3()
        {
            dgv3.RowTemplate.Height = 42;

            //This Part of Code is for the styling of the Grid Padding
            Padding newPadding = new Padding(0, 10, 0, 10);
            this.dgv3.ColumnHeadersDefaultCellStyle.Padding = newPadding;

            //This Part of Code is for the styling of the Grid Columns
            dgv3.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);
            dgv3.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // This Part of Code is for the styling of the Grid Border
            this.dgv3.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgv3.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //This Part of Code is for the styling of the Grid Rows
            dgv3.RowsDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv3.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public void dgv_4()
        {
            dgv4.RowTemplate.Height = 42;

            //This Part of Code is for the styling of the Grid Padding
            Padding newPadding = new Padding(0, 10, 0, 10);
            this.dgv4.ColumnHeadersDefaultCellStyle.Padding = newPadding;

            //This Part of Code is for the styling of the Grid Columns
            dgv4.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);
            dgv4.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // This Part of Code is for the styling of the Grid Border
            this.dgv4.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgv4.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //This Part of Code is for the styling of the Grid Rows
            dgv4.RowsDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv4.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public void dgv_5()
        {
            dgv5.RowTemplate.Height = 42;

            //This Part of Code is for the styling of the Grid Padding
            Padding newPadding = new Padding(0, 10, 0, 10);
            this.dgv5.ColumnHeadersDefaultCellStyle.Padding = newPadding;

            //This Part of Code is for the styling of the Grid Columns
            dgv5.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);
            dgv5.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // This Part of Code is for the styling of the Grid Border
            this.dgv5.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgv5.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //This Part of Code is for the styling of the Grid Rows
            dgv5.RowsDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv5.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public void dgv_6()
        {
            dgv6.RowTemplate.Height = 42;

            //This Part of Code is for the styling of the Grid Padding
            Padding newPadding = new Padding(0, 10, 0, 10);
            this.dgv6.ColumnHeadersDefaultCellStyle.Padding = newPadding;

            //This Part of Code is for the styling of the Grid Columns
            dgv6.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);
            dgv6.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // This Part of Code is for the styling of the Grid Border
            this.dgv6.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgv6.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //This Part of Code is for the styling of the Grid Rows
            dgv6.RowsDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv6.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public void dgv_7()
        {
            dgv7.RowTemplate.Height = 42;

            //This Part of Code is for the styling of the Grid Padding
            Padding newPadding = new Padding(0, 10, 0, 10);
            this.dgv7.ColumnHeadersDefaultCellStyle.Padding = newPadding;

            //This Part of Code is for the styling of the Grid Columns
            dgv7.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);
            dgv7.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // This Part of Code is for the styling of the Grid Border
            this.dgv7.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dgv7.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //This Part of Code is for the styling of the Grid Rows
            dgv7.RowsDefaultCellStyle.Font = new Font("Arial", 11F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv7.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void label_BillRecord_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void label_Category_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void label_Customers_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void label_Orders_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage5;
        }

        private void label_Products_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage5;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
        }

        private void label_Sales_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage7;
        }

        private void label_Stocks_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage7;
        }

        private void StockToExcel_Click(object sender, EventArgs e)
        {
            //Creating DataTable.
            DataTable dt = new DataTable();

            //Adding the Columns.
            foreach (DataGridViewColumn column in dgv1.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows.
            foreach (DataGridViewRow row in dgv1.Rows)
            {
                dt.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }

            //Exporting to Excel.
            string folderPath = "C:\\Food Maza\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Bill Records");

                //Set the color of Header Row.
                //A resembles First Column while C resembles Third column.
                wb.Worksheet(1).Cells("A1:C1").Style.Fill.BackgroundColor = XLColor.DarkGreen;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    //A resembles First Column while C resembles Third column.
                    //Header row is at Position 1 and hence First row starts from Index 2.
                    string cellRange = string.Format("A{0}:C{0}", i + 1);
                    if (i % 2 != 0)
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.GreenYellow;
                    }
                    else
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.Yellow;
                    }

                }
                //Adjust widths of Columns.
                wb.Worksheet(1).Columns().AdjustToContents();

                //Save the Excel file.
                wb.SaveAs(folderPath + "Bill Records.xlsx");
            }
        }

        private void categorytoExcel_Click(object sender, EventArgs e)
        {
            //Creating DataTable.
            DataTable dt = new DataTable();

            //Adding the Columns.
            foreach (DataGridViewColumn column in dgv2.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows.
            foreach (DataGridViewRow row in dgv2.Rows)
            {
                dt.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }

            //Exporting to Excel.
            string folderPath = "C:\\Food Maza\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Category");

                //Set the color of Header Row.
                //A resembles First Column while C resembles Third column.
                wb.Worksheet(1).Cells("A1:C1").Style.Fill.BackgroundColor = XLColor.DarkGreen;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    //A resembles First Column while C resembles Third column.
                    //Header row is at Position 1 and hence First row starts from Index 2.
                    string cellRange = string.Format("A{0}:C{0}", i + 1);
                    if (i % 2 != 0)
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.GreenYellow;
                    }
                    else
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.Yellow;
                    }

                }
                //Adjust widths of Columns.
                wb.Worksheet(1).Columns().AdjustToContents();

                //Save the Excel file.
                wb.SaveAs(folderPath + "Category Records.xlsx");
            }
        }

        private void customerToExcel_Click(object sender, EventArgs e)
        {
            //Creating DataTable.
            DataTable dt = new DataTable();

            //Adding the Columns.
            foreach (DataGridViewColumn column in dgv3.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows.
            foreach (DataGridViewRow row in dgv3.Rows)
            {
                dt.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }

            //Exporting to Excel.
            string folderPath = "C:\\Food Maza\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Customers");

                //Set the color of Header Row.
                //A resembles First Column while C resembles Third column.
                wb.Worksheet(1).Cells("A1:C1").Style.Fill.BackgroundColor = XLColor.DarkGreen;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    //A resembles First Column while C resembles Third column.
                    //Header row is at Position 1 and hence First row starts from Index 2.
                    string cellRange = string.Format("A{0}:C{0}", i + 1);
                    if (i % 2 != 0)
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.GreenYellow;
                    }
                    else
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.Yellow;
                    }

                }
                //Adjust widths of Columns.
                wb.Worksheet(1).Columns().AdjustToContents();

                //Save the Excel file.
                wb.SaveAs(folderPath + "Customer Records.xlsx");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Creating DataTable.
            DataTable dt = new DataTable();

            //Adding the Columns.
            foreach (DataGridViewColumn column in dgv4.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows.
            foreach (DataGridViewRow row in dgv4.Rows)
            {
                dt.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }

            //Exporting to Excel.
            string folderPath = "C:\\Food Maza\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Orders");

                //Set the color of Header Row.
                //A resembles First Column while C resembles Third column.
                wb.Worksheet(1).Cells("A1:C1").Style.Fill.BackgroundColor = XLColor.DarkGreen;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    //A resembles First Column while C resembles Third column.
                    //Header row is at Position 1 and hence First row starts from Index 2.
                    string cellRange = string.Format("A{0}:C{0}", i + 1);
                    if (i % 2 != 0)
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.GreenYellow;
                    }
                    else
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.Yellow;
                    }

                }
                //Adjust widths of Columns.
                wb.Worksheet(1).Columns().AdjustToContents();

                //Save the Excel file.
                wb.SaveAs(folderPath + "Order Records.xlsx");
            }
        }

        private void ProductsToExcel_Click(object sender, EventArgs e)
        {
            //Creating DataTable.
            DataTable dt = new DataTable();

            //Adding the Columns.
            foreach (DataGridViewColumn column in dgv5.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows.
            foreach (DataGridViewRow row in dgv5.Rows)
            {
                dt.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }

            //Exporting to Excel.
            string folderPath = "C:\\Food Maza\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Products");

                //Set the color of Header Row.
                //A resembles First Column while C resembles Third column.
                wb.Worksheet(1).Cells("A1:C1").Style.Fill.BackgroundColor = XLColor.DarkGreen;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    //A resembles First Column while C resembles Third column.
                    //Header row is at Position 1 and hence First row starts from Index 2.
                    string cellRange = string.Format("A{0}:C{0}", i + 1);
                    if (i % 2 != 0)
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.GreenYellow;
                    }
                    else
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.Yellow;
                    }

                }
                //Adjust widths of Columns.
                wb.Worksheet(1).Columns().AdjustToContents();

                //Save the Excel file.
                wb.SaveAs(folderPath + "Products Record.xlsx");
            }
        }

        private void SalesToExcel_Click(object sender, EventArgs e)
        {
            //Creating DataTable.
            DataTable dt = new DataTable();

            //Adding the Columns.
            foreach (DataGridViewColumn column in dgv6.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows.
            foreach (DataGridViewRow row in dgv6.Rows)
            {
                dt.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }

            //Exporting to Excel.
            string folderPath = "C:\\Food Maza\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Sales");

                //Set the color of Header Row.
                //A resembles First Column while C resembles Third column.
                wb.Worksheet(1).Cells("A1:C1").Style.Fill.BackgroundColor = XLColor.DarkGreen;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    //A resembles First Column while C resembles Third column.
                    //Header row is at Position 1 and hence First row starts from Index 2.
                    string cellRange = string.Format("A{0}:C{0}", i + 1);
                    if (i % 2 != 0)
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.GreenYellow;
                    }
                    else
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.Yellow;
                    }

                }
                //Adjust widths of Columns.
                wb.Worksheet(1).Columns().AdjustToContents();

                //Save the Excel file.
                wb.SaveAs(folderPath + "Sales Record.xlsx");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Creating DataTable.
            DataTable dt = new DataTable();

            //Adding the Columns.
            foreach (DataGridViewColumn column in dgv7.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows.
            foreach (DataGridViewRow row in dgv7.Rows)
            {
                dt.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }

            //Exporting to Excel.
            string folderPath = "C:\\Food Maza\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Stocks");

                //Set the color of Header Row.
                //A resembles First Column while C resembles Third column.
                wb.Worksheet(1).Cells("A1:C1").Style.Fill.BackgroundColor = XLColor.DarkGreen;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    //A resembles First Column while C resembles Third column.
                    //Header row is at Position 1 and hence First row starts from Index 2.
                    string cellRange = string.Format("A{0}:C{0}", i + 1);
                    if (i % 2 != 0)
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.GreenYellow;
                    }
                    else
                    {
                        wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.Yellow;
                    }

                }
                //Adjust widths of Columns.
                wb.Worksheet(1).Columns().AdjustToContents();

                //Save the Excel file.
                wb.SaveAs(folderPath + "Stocks Record.xlsx");
            }
        }

    }
}