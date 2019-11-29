using Foodies;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace FoodMazaPos
{
    public partial class Cashier : Form
    {
        string INVOICEID2;

        /*Timer for counting minutes*/
        System.Windows.Forms.Timer _timer1 = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer _timer2 = new System.Windows.Forms.Timer();

        /*Declaring product and cashier string,integerandimage type variable to be read by the dr and then saved in these
         declarations*/
        string Productname;
        int Productprice;
        Image ProductImage;
        string Categoryname;
        Image CategoryImage;

        /*Declaring runtime labels and pictureboxes*/
        Panel pnl2;
        Label label3;
        PictureBox picture2;
        Panel pnl;
        PictureBox picture;
        Label label;
        Label Clabel2;

        /*Declaring Global Label as object sender Event Args e*/
        Label currentlable;
        Label currentlable2;
        Label currentlable3;

        /* Declaring variable to check whether the element is present in gridview or not?*/
        string Dgvproductname;
        int DgvProductprice;
        string Dgvcategory;
        int Dgvactualprice;

        /*Declaratioin of variables for the total count for the total amount, total quantity,
         actual amount from the data gird view*/
        int quantity = 1, a, b, c;
        int totalAmount = 0; int totalQuantity = 0;

        int INVOICEID; /* For Invoice Reading through Sql reader*/
        string druser;/*Checking user status*/

        //For billing
        string drInvoiceid, CustID, OrderID, CustName, Product_Name,
               ProductQuantity, ProductRate, ProductAmount, ProductAmountWithGST,
               OrderTime, OrderDate, TotalQty, ActualAmount, TotalAmount, TotalAmountWithGST,
               DiscounInPercent;

        // For Bill variables
        string Total_Qty, Actual_Amount, Total_Amount, _TotalWithGST, _Discount;

        // Connection String //
        SqlConnection con = new SqlConnection(Helper.con);

        /*Getter and setter for the print page width and height to print*/
        public System.Drawing.Printing.PaperSize PaperSize { get; set; }

        //For inventory operations
        // Product name to check in a string

        string zingerburger = "zinger burger";
        string beefburger = "beef burger";
        string mightyburger = "mighty burger";
        string beefdoublepatty = "beef double patty";
        string chickenburger = "chicken burger";
        string doublechickenburger = "double chicken burger";
        string hulk = "hulk";
        string smokeburger = "smoke burger";
        string dhabardoze = "dhabardoze";
        string beefjalapeno = "beef jalapeno";
        string clubsandwich = "club sandwich";
        string chickensandwich = "chicken sandwich";
        string bbqsandwich = "bbq sandwich";
        string deadendsandwich = "dead end sandwich";
        string smokesandwich = "smoke sandwich";
        string malaibotisandwich = "malai boti sandwich";
        string chestbroast = "chest broast";
        string legbroast = "leg broast";
        string gyco = "gyco";
        string smallchickentikka = "small chicken tikka";
        string mediumchickentikka = "medium chicken tikka";
        string largechickentikka = "large chicken tikka";
        string smallchickenfajita = "small chicken fajita";
        string mediumchickenfajita = "medium chicken fajita";
        string largechickenfajita = "large chicken fajita";
        string smallfoodiesspecial = "small foodies special";
        string mediumfoodiesspecial = "medium foodies special";
        string largefoodiesspecial = "large foodies special";
        string smallmalaiboti = "small malai boti";
        string mediummalaiboti = "medium malai boti";
        string largemalaiboti = "large malai boti";
        string smallvegilover = "small vegi lover";
        string mediumvegilover = "medium vegi lover";
        string largevegilover = "large vegi lover";
        string smallsupersupremo = "small super supremo";
        string mediumsupersupremo = "medium super supremo";
        string largesupersupremo = "large super supremo";
        string smallcheeselover = "small cheese lover";
        string mediumcheeselover = "medium cheese lover";
        string largecheeselover = "large cheese lover";
        string chickenstrips = "chicken strips";
        string chickencrispywrap = "chicken crispy wrap";

        // Boolean to check whether product is present in datagridview or not
        bool exist;

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

        public void Timercheckingstock()
        {
            SqlConnection con = new SqlConnection(Helper.con);

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * from Stock where stockweigth = '0' ", con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("A stock item is ended you need to update it.\n" +
                    "Do you want to update it now?", "Stock warning", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    updatestocks updatestock = new updatestocks();
                    updatestock.Show();
                }
                if (result == DialogResult.No)
                {
                    //Do nothing
                }
            }
        }

        public Cashier()
        {
            InitializeComponent();
            CheckingUserStatus();
            //_timer1.Interval = 7200000; // 7,200,000 ms = 2 hours
            //_timer1.Interval = 40000; // 40000 ms = 40 seconds
            //_timer1.Tick += timer1_Tick_1;
            //_timer1.Start();

        }

        private void Cashier_Load(object sender, EventArgs e)
        {
            Dgv_1();
            //CheckInternetavaibility();
            Loadcategory();
            Loadproducts();
            TrailPeriod();
            Appstatus();
        }

        public void Appstatus()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select top 1 Appstatus from Activation", con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    //Do Nothing
                }
                else
                {
                    CheckingTrailPeriod();
                }
            }
            catch (Exception)
            {
                //Do nothing it means table is empty
            }
        }

        public void CheckingTrailPeriod()
        {
            try
            {
                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                SqlCommand cmd = new SqlCommand("select top 1 startingdate,endingdate from TrailDays", con, tran);
                cmd.ExecuteNonQuery();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime startingdate = Convert.ToDateTime(dr["startingdate"]);
                        DateTime endingdate = Convert.ToDateTime(dr["endingdate"]);

                        DateTime currentdate = DateTime.Today;

                        if (currentdate < startingdate)
                        {
                            MessageBox.Show("Set the current date and time to use the application otherwise it will not perform");
                            Application.Exit();
                        }
                        else if (currentdate < endingdate)
                        {
                            SqlConnection con3 = new SqlConnection(Helper.con);
                            con3.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("select * from trailperiodended where ended = 'ended'  ", con3);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            //Transaction start
                            SqlTransaction transaction = con3.BeginTransaction();

                            if (table.Rows.Count > 0)
                            {
                                this.Hide();
                                MessageBox.Show("Set the current date and time to use the application otherwise it will not perform");
                                ActivationForm activation = new ActivationForm();
                                activation.Show();
                            }
                            transaction.Commit();
                            con3.Close();
                        }
                        else if (currentdate > endingdate)
                        {
                            SqlConnection con3 = new SqlConnection(Helper.con);
                            con3.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("select * from trailperiodended where ended = 'ended'  ", con3);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            //Transaction start
                            SqlTransaction transaction = con3.BeginTransaction();

                            if (table.Rows.Count > 0)
                            {
                                this.Hide();
                                MessageBox.Show("Trail Ended");
                                ActivationForm activationForm = new ActivationForm();
                                activationForm.Show();
                            }
                            else
                            {
                                SqlConnection con2 = new SqlConnection(Helper.con);
                                con2.Open();
                                string ended = "ended";
                                SqlCommand cmd2 = con2.CreateCommand();
                                cmd2.CommandType = CommandType.Text;
                                cmd2.CommandText = "insert into trailperiodended(ended) values ('" + ended + "')";
                                cmd2.ExecuteNonQuery();
                                con2.Close();
                                this.Hide();
                                MessageBox.Show("Trail Ended");
                                ActivationForm activationForm = new ActivationForm();
                                activationForm.Show();
                            }
                            transaction.Commit();
                            con3.Close();
                        }
                    }
                }
                tran.Commit();
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
        }

        public void TrailPeriod()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select top 1 startingdate,endingdate from TrailDays", con);
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    //Do nothing
                }
                else
                {
                    try
                    {
                        Ping myPing = new Ping();
                        string host = "google.com";
                        byte[] buffer = new byte[32];
                        int timeout = 1000;
                        PingOptions pingOptions = new PingOptions();
                        PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                        if (reply.Status == IPStatus.Success)
                        {
                            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
                            var response = myHttpWebRequest.GetResponse();
                            string todaysDates = response.Headers["date"];
                            /*return*/
                            var date = DateTime.ParseExact(todaysDates,
                                            "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                            CultureInfo.InvariantCulture.DateTimeFormat,
                                            DateTimeStyles.AssumeUniversal);

                            var timezonedate = date.ToShortDateString();
                            var currentdate = DateTime.Now.ToShortDateString();
                            var expirydate = DateTime.Now.AddDays(7).ToShortDateString();

                            if (timezonedate.Equals(currentdate))
                            {
                                con.Open();
                                SqlCommand cmd = con.CreateCommand();
                                cmd.CommandType = CommandType.Text;
                                string sqlQuery = "insert into TrailDays(startingdate,endingdate) values ('" + currentdate + "','" + expirydate + "')";
                                cmd = new SqlCommand(sqlQuery, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            else
                            {
                                con.Open();
                                SqlCommand cmd = con.CreateCommand();
                                cmd.CommandType = CommandType.Text;
                                string sqlQuery = "insert into TrailDays(startingdate,endingdate) values ('" + currentdate + "','" + expirydate + "')";
                                cmd = new SqlCommand(sqlQuery, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                    catch (PingException)
                    {
                        var currentdate = DateTime.Now.ToShortDateString();
                        var expirydate = DateTime.Now.AddDays(7).ToShortDateString();

                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        string sqlQuery = "insert into TrailDays(startingdate,endingdate) values ('" + currentdate + "','" + expirydate + "')";
                        cmd = new SqlCommand(sqlQuery, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("Error Message : " + ex.Message.ToString());
            }
        }

        public static bool CheckInternetavaibility()
        {
            try
            {
                Ping myPing = new Ping();
                string host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure you are connected to internet \n" +
                    "otherwise some of the app features will not work properly");
                return false;
            }
        }

        public void Dgv_1()
        {
            dgv1.RowTemplate.Height = 32;

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
            dgv1.RowsDefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);

            //this Line of Code made the dgv1 Text Middle Center
            dgv1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        public void Loadcategory()
        {
            using (SqlConnection con = new SqlConnection(Helper.con))
            {
                SqlConnection conn = new SqlConnection(Helper.con);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                SqlCommand cmd10 = new SqlCommand("SELECT * FROM Category", conn, tran);
                cmd10.ExecuteNonQuery();

                string CommandText = "SELECT * FROM Category";
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
                                Categoryname = Convert.ToString(dr2["CategoryName"]);
                                //var bytes = (byte[])dr2[2];
                                //foreach (DataRow dr3 in ds.Tables[0].Rows)
                                //{
                                //    using (MemoryStream ms = new MemoryStream(bytes))
                                //    {
                                //        CategoryImage = Image.FromStream(ms);
                                //    }
                                //}

                                pnl2 = new Panel
                                {
                                    Size = new System.Drawing.Size(180, 50),
                                    Location = new Point(10, 10),
                                    BorderStyle = BorderStyle.FixedSingle,
                                    BackColor = Color.White,
                                    Margin = new Padding(3, 3, 3, 3),
                                };

                                //picture2 = new PictureBox
                                //{
                                //    Size = new System.Drawing.Size(158, 120),
                                //    Location = new Point(10, 10),
                                //    BorderStyle = BorderStyle.FixedSingle,
                                //    //Margin = new Padding(5, 5, 5, 5),
                                //    BackColor = Color.Green,
                                //    SizeMode = PictureBoxSizeMode.StretchImage,
                                //    Image = CategoryImage,
                                //};

                                label3 = new Label
                                {
                                    Size = new System.Drawing.Size(170, 30),
                                    Font = new Font("Arial", 12, FontStyle.Bold),
                                    Location = new Point(5, 10),
                                    //Margin = new Padding(5, 5, 5, 5),
                                    BackColor = Color.White,
                                    ForeColor = Color.Black,
                                    Text = Categoryname,
                                    TextAlign = ContentAlignment.MiddleCenter,
                                };

                                //pnl2.Controls.Add(picture2);
                                pnl2.Controls.Add(label3);
                                flowLayoutPanel2.Controls.Add(pnl2);
                                pnl2.Click += new System.EventHandler(this.pnl2Click);
                                label3.Click += new System.EventHandler(this.label3Click);

                            }
                        }
                    }
                }
            }
        }

        /*Category Label onclick Evenet handler*/
        void label3Click(object sender, EventArgs e)
        {
            currentlable = (Label)sender;
            flowLayoutPanel1.Controls.Clear();
            using (SqlConnection con = new SqlConnection(Helper.con))
            {
                SqlConnection conn = new SqlConnection(Helper.con);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                SqlCommand cmd10 = new SqlCommand("SELECT * FROM Products where ProductCategory = '" + currentlable.Text + "' ", conn, tran);
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
                                Productname = Convert.ToString(dr2["ProductName"]);
                                Productprice = Convert.ToInt32(dr2["ProductPrice"]);
                                //var bytes = (byte[])dr2[4];
                                //foreach (DataRow dr3 in ds.Tables[0].Rows)
                                //{
                                //    using (MemoryStream ms = new MemoryStream(bytes))
                                //    {
                                //        ProductImage = Image.FromStream(ms);
                                //    }
                                //}


                                pnl = new Panel
                                {
                                    Size = new System.Drawing.Size(214, 100),
                                    Location = new Point(10, 10),
                                    BorderStyle = BorderStyle.FixedSingle,
                                    BackColor = Color.White,
                                    Margin = new Padding(5, 5, 5, 5),
                                };

                                //picture = new PictureBox
                                //{
                                //    Size = new System.Drawing.Size(190, 120),
                                //    Location = new Point(10, 10),
                                //    BorderStyle = BorderStyle.FixedSingle,
                                //    //Margin = new Padding(5, 5, 5, 5),
                                //    BackColor = Color.Green,
                                //    SizeMode = PictureBoxSizeMode.StretchImage,
                                //    Image = ProductImage,
                                //};

                                label = new Label
                                {
                                    Size = new System.Drawing.Size(200, 22),
                                    Font = new Font("Arial", 12, FontStyle.Bold),
                                    Location = new Point(5, 30),
                                    //Margin = new Padding(5, 5, 5, 5),
                                    BackColor = Color.White,
                                    ForeColor = Color.Black,
                                    Text = Productname,
                                    TextAlign = ContentAlignment.MiddleCenter,
                                };

                                label2 = new Label
                                {
                                    Size = new System.Drawing.Size(100, 20),
                                    Font = new Font("Arial", 11, FontStyle.Bold),
                                    Location = new Point(60, 50),
                                    BackColor = Color.White,
                                    ForeColor = Color.Black,
                                    Text = Convert.ToString(Productprice),
                                    TextAlign = ContentAlignment.MiddleCenter,
                                };

                                //pnl.Controls.Add(picture);
                                pnl.Controls.Add(label);
                                pnl.Controls.Add(label2);
                                flowLayoutPanel1.Controls.Add(pnl);
                                label.Click += new System.EventHandler(this.labelClick);
                                label2.Click += new System.EventHandler(this.label2Click);

                            }
                        }
                    }
                }
            }
        }

        /*Category Panel onclick Evenet handler*/
        void pnl2Click(object sender, EventArgs e)
        {
            // Do nothing in this DataeventHandler
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
                                Productname = Convert.ToString(dr2["ProductName"]);
                                Productprice = Convert.ToInt32(dr2["ProductPrice"]);
                                //var bytes = (byte[])dr2[4];
                                //foreach (DataRow dr3 in ds.Tables[0].Rows)
                                //{
                                //    using (MemoryStream ms = new MemoryStream(bytes))
                                //    {
                                //        ProductImage = Image.FromStream(ms);
                                //    }
                                //}

                                pnl = new Panel
                                {
                                    Size = new System.Drawing.Size(214, 80),
                                    Location = new Point(10, 10),
                                    BorderStyle = BorderStyle.FixedSingle,
                                    BackColor = Color.White,
                                    Margin = new Padding(5, 5, 5, 5),
                                };

                                //picture = new PictureBox
                                //{
                                //    Size = new System.Drawing.Size(190, 120),
                                //    Location = new Point(10, 10),
                                //    BorderStyle = BorderStyle.FixedSingle,
                                //    //Margin = new Padding(5, 5, 5, 5),
                                //    BackColor = Color.Green,
                                //    SizeMode = PictureBoxSizeMode.StretchImage,
                                //    Image = ProductImage,
                                //};

                                label = new Label
                                {
                                    Size = new System.Drawing.Size(200, 22),
                                    Font = new Font("Arial", 12, FontStyle.Bold),
                                    Location = new Point(5, 20),
                                    //Margin = new Padding(5, 5, 5, 5),
                                    BackColor = Color.White,
                                    ForeColor = Color.Black,
                                    Text = Productname,
                                    TextAlign = ContentAlignment.MiddleCenter,
                                };

                                label2 = new Label
                                {
                                    Size = new System.Drawing.Size(100, 20),
                                    Font = new Font("Arial", 11, FontStyle.Bold),
                                    Location = new Point(60, 50),
                                    //Margin = new Padding(5, 5, 5, 5),
                                    BackColor = Color.White,
                                    ForeColor = Color.Black,
                                    Text = Convert.ToString(Productprice),
                                    TextAlign = ContentAlignment.MiddleCenter,
                                };

                                //pnl.Controls.Add(picture);
                                pnl.Controls.Add(label);
                                pnl.Controls.Add(label2);
                                flowLayoutPanel1.Controls.Add(pnl);
                                label.Click += new System.EventHandler(this.labelClick);
                                label2.Click += new System.EventHandler(this.label2Click);

                            }
                        }
                    }
                }
            }
        }

        void label2Click(object sender, EventArgs e)
        {
            currentlable3 = (Label)sender;
        }

        void labelClick(object sender, EventArgs e)
        {
            try
            {
                currentlable2 = (Label)sender;

                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                SqlCommand cmd10 = new SqlCommand("select * from Products where ProductName = '" + currentlable2.Text + "' ", con, tran);
                cmd10.ExecuteNonQuery();

                using (SqlDataReader dr = cmd10.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Dgvproductname = Convert.ToString(dr["ProductName"]);
                        DgvProductprice = Convert.ToInt32(dr["ProductPrice"]);
                        Dgvcategory = Convert.ToString(dr["ProductCategory"]);
                        Dgvactualprice = Convert.ToInt32(dr["ProductPrice"]);
                    }
                }

                totalAmount = 0; totalQuantity = 0;

                bool Found = false;
                dgv1.AllowUserToAddRows = true;

                if (dgv1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow Row in dgv1.Rows)
                    {
                        if (Convert.ToString(Row.Cells[0].Value) == Dgvproductname)
                        {
                            Found = true;
                            dgv1.AllowUserToAddRows = false;
                        }
                        dgv1.AllowUserToAddRows = false;
                    }
                    if (!Found)
                    {
                        a = Convert.ToInt32(quantity);
                        b = Convert.ToInt32(DgvProductprice);
                        c = a * b;

                        dgv1.Rows.Add(Dgvproductname, a, b, c, Dgvcategory);
                        for (int i = 0; i < dgv1.Rows.Count; ++i)
                        {
                            totalQuantity += Convert.ToInt32(dgv1.Rows[i].Cells[1].Value);
                            totalAmount += Convert.ToInt32(dgv1.Rows[i].Cells[3].Value);
                        }
                        totalQty.Text = totalQuantity.ToString();
                        total_Amount.Text = totalAmount.ToString();
                        act_price.Text = totalAmount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dgv1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int qty;
            int rate;
            int amount;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgv1.Rows[e.RowIndex];

                totalAmount = 0; totalQuantity = 0;
                totalQty.Text = "0";
                total_Amount.Text = "0";

                this.dgv1.EditMode = DataGridViewEditMode.EditOnEnter;

                qty = Convert.ToInt32(row.Cells[1].Value);
                rate = Convert.ToInt32(row.Cells[2].Value);
                amount = qty * rate;
                row.Cells[3].Value = amount;

            }

            for (int i = 0; i < dgv1.Rows.Count; ++i)
            {
                totalQuantity += Convert.ToInt32(dgv1.Rows[i].Cells[1].Value);
                totalAmount += Convert.ToInt32(dgv1.Rows[i].Cells[3].Value);
            }
            totalQty.Text = totalQuantity.ToString();
            total_Amount.Text = totalAmount.ToString();
            act_price.Text = totalAmount.ToString();
        }

        public void CheckingUserStatus()
        {
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            SqlCommand cmd20 = new SqlCommand("select top 1 usercategory from LoginDetails order by Id DESC", con, tran);
            cmd20.ExecuteNonQuery();

            using (SqlDataReader dr = cmd20.ExecuteReader())
            {
                while (dr.Read())
                {
                    druser = Convert.ToString(dr["usercategory"]);
                }
            }

            if (druser == "admin")
            {
                bBBBToolStripMenuItem.Visible = true;
                cCCCToolStripMenuItem.Visible = true;
            }
            else
            {
                bBBBToolStripMenuItem.Visible = false;
                cCCCToolStripMenuItem.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DVPrintDocument2.DefaultPageSettings.PaperSize = new PaperSize("", 280, 75);
                DVPrintDocument2.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

                if (dgv1.Rows.Count <= 2)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 425);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 3)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 475);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 4)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 500);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 5)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 525);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 6)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 550);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 7)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 575);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 8)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 600);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 9)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 625);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 10)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 650);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 11)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 700);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 12)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 725);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 13)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 750);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 14)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 775);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 16)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 800);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 17)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 825);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 18)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 850);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 19)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 975);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 20)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1000);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 21)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1025);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 22)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1050);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 23)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1075);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 24)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1100);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 25)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1125);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 26)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1150);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 27)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1175);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 28)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1200);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 29)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1225);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 30)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1250);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 31)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1275);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 32)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1300);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 33)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1325);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 34)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1350);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 35)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1375);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 36)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1400);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 37)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1425);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 38)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1450);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 39)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1475);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else if (dgv1.Rows.Count <= 40)
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, 1500);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }
                else
                {
                    DVPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("", 280, DVPrintDocument.DefaultPageSettings.PaperSize.Height);
                    DVPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                }

                if (dgv1.Rows.Count == 0)
                {
                    const string message =
                        "Transaction can't be completed because there is no item selected for sale.";
                    const string caption = "Transaction Error";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Stop);
                }
                else if (dgv1.Rows.Count > 40)
                {
                    const string message =
                        "Transaction can't be completed on more than 40 items.\n" +
                        "Please select less than 40 items for transaction.";
                    const string caption = "Paper size limit";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Exclamation);
                }
                else
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * from Bill", con);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        /* Connection String me Integrated Security=True; ko Integrated Security=SSPI; se change karna hoga
                        or phir MultipleActiveResultSets = True connection string me add karna hoga takai Sql Reader ke while
                        condition me aik se sql queries ki queires ko implement kara jasakai*/

                        SqlConnection con = new SqlConnection(Helper.con);
                        con.Open();
                        SqlTransaction tran = con.BeginTransaction();

                        SqlCommand cmd10 = new SqlCommand("select top 1 InvioceID from Bill order by InvioceID DESC", con, tran);
                        cmd10.ExecuteNonQuery();

                        using (SqlDataReader dr = cmd10.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                INVOICEID = Convert.ToInt32(dr["InvioceID"]);
                                INVOICEID = INVOICEID + 1;
                            }
                        }

                        string Custname = "Valuable Customer";
                        string CustContact = "***********";

                        string CUSTID, ORDERID, CUSTNAME, ORDERTIME, ORDERDATE, CUSTCONTACT;
                        string OrderType = "Food Item";
                        string OrderCategory = "Food";

                        SqlCommand cmd1 = new SqlCommand("insert into Customer values ('" + Custname + "','" + CustContact + "','" + DateTime.Now.ToShortTimeString() + "','" + DateTime.Now.ToShortDateString() + "')", con, tran);
                        cmd1.ExecuteNonQuery();
                        SqlCommand cmd2 = new SqlCommand("select top 1 CustID from Customer order by CustID DESC", con, tran);
                        cmd2.ExecuteNonQuery();

                        using (SqlDataReader dr = cmd2.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                CUSTID = dr["CustID"].ToString();
                                SqlCommand cmd3 = new SqlCommand("insert into Orders values ('" + CUSTID + "','" + OrderType.ToString() + "','" + OrderCategory.ToString() + "','" + DateTime.Now.ToShortTimeString() + "','" + DateTime.Now.ToShortDateString() + "')", con, tran);
                                cmd3.ExecuteNonQuery();
                            }
                        }

                        SqlCommand cmd4 = new SqlCommand("select top 1 CustID,CustName,Contact from Customer order by CustID DESC", con, tran);
                        cmd4.ExecuteNonQuery();
                        SqlCommand cmd5 = new SqlCommand("select top 1 OrderID,OrderTime,OrderDate from Orders order by OrderID DESC", con, tran);
                        cmd5.ExecuteNonQuery();

                        using (SqlDataReader dr = cmd4.ExecuteReader())
                        {
                            using (SqlDataReader dr1 = cmd5.ExecuteReader())
                            {
                                string itemname;
                                string itemqty;
                                string itemprice;
                                double itempricewithGST;
                                double GST;
                                double GStunit = 0.17;
                                string billtotal = total_Amount.Text;
                                string totalqty = totalQty.Text;
                                double totalGSTcalcualtion = Convert.ToDouble(total_Amount.Text) * GStunit;
                                double totalAmountwithGST = Convert.ToDouble(total_Amount.Text) + totalGSTcalcualtion;
                                double discount = Convert.ToDouble(per_discount.Text);
                                double singleitemcollectiveamount;

                                while (dr.Read())
                                {
                                    CUSTID = dr["CustID"].ToString();
                                    CUSTNAME = dr["CustName"].ToString();
                                    CUSTCONTACT = dr["Contact"].ToString();
                                    while (dr1.Read())
                                    {
                                        ORDERID = dr1["OrderID"].ToString();
                                        ORDERTIME = dr1["OrderTime"].ToString();
                                        ORDERDATE = dr1["OrderDate"].ToString();

                                        SqlCommand cmd7 = new SqlCommand("insert into Sales values ('" + ORDERID + "','" + CUSTID + "','" + CUSTNAME + "','" + CUSTCONTACT + "','" + OrderType + "','" + OrderCategory + "','" + ORDERTIME + "','" + ORDERDATE + "')", con, tran);
                                        cmd7.ExecuteNonQuery();

                                        SqlCommand cmd6;
                                        for (int i = 0; i < dgv1.Rows.Count; i++)
                                        {
                                            itemname = Convert.ToString(dgv1.Rows[i].Cells[0].Value);
                                            itemqty = Convert.ToString(dgv1.Rows[i].Cells[1].Value);
                                            itemprice = Convert.ToString(dgv1.Rows[i].Cells[2].Value);
                                            singleitemcollectiveamount = Convert.ToDouble(dgv1.Rows[i].Cells[3].Value);
                                            GST = Convert.ToInt32(dgv1.Rows[i].Cells[3].Value) * GStunit;
                                            itempricewithGST = Convert.ToDouble(dgv1.Rows[i].Cells[2].Value) + GST;
                                            cmd6 = new SqlCommand("insert into Bill values ('" + INVOICEID + "','" + CUSTID + "','" + ORDERID + "','" + CUSTNAME + "','" + itemname.ToString() + "','" + itemqty.ToString() + "','" + itemprice.ToString() + "','" + singleitemcollectiveamount.ToString() + "','" + itempricewithGST.ToString() + "','" + ORDERTIME + "','" + ORDERDATE + "','" + totalqty.ToString() + "','" + act_price.Text.ToString() + "','" + billtotal.ToString() + "','" + totalAmountwithGST.ToString() + "','" + discount.ToString() + "')", con, tran);
                                            cmd6.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            tran.Commit();
                            con.Close();
                        }

                        DVPrintDocument.Print();
                        DVPrintDocument.Dispose();

                        DVPrintDocument2.Print();
                        DVPrintDocument2.Dispose();

                        inventoryFunctionality();

                        address.Text = string.Empty;
                        contact.Text = string.Empty;
                        dgv1.Rows.Clear();
                        dgv1.Refresh();
                        DiscountPKR.Text = "";
                        DiscountPercent.Text = "";
                        per_discount.Text = "0";
                        act_price.Text = "0";
                        totalQty.Text = "0";
                        total_Amount.Text = "0";

                        Timercheckingstock();
                    }
                    else
                    {
                        string Custname = "Valuable Customer";
                        string CustContact = "***********";

                        string CUSTID, ORDERID, CUSTNAME, ORDERTIME, ORDERDATE, CUSTCONTACT;
                        string OrderType = "Food Item";
                        string OrderCategory = "Food";

                        int InvocieId = 1;

                        /* Connection String me Integrated Security=True; ko Integrated Security=SSPI; se change karna hoga
                         or phir MultipleActiveResultSets = True connection string me add karna hoga takai Sql Reader ke while
                         condition me aik se sql queries ki queires ko implement kara jasakai*/

                        SqlConnection con = new SqlConnection(Helper.con);
                        con.Open();
                        SqlTransaction tran = con.BeginTransaction();

                        //
                        SqlCommand cmd1 = new SqlCommand("insert into Customer values ('" + Custname + "','" + CustContact + "','" + DateTime.Now.ToShortTimeString() + "','" + DateTime.Now.Date + "')", con, tran);
                        cmd1.ExecuteNonQuery();
                        SqlCommand cmd2 = new SqlCommand("select top 1 CustID from Customer order by CustID DESC", con, tran);
                        cmd2.ExecuteNonQuery();

                        using (SqlDataReader dr = cmd2.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                CUSTID = dr["CustID"].ToString();
                                SqlCommand cmd3 = new SqlCommand("insert into Orders values ('" + CUSTID + "','" + OrderType.ToString() + "','" + OrderCategory.ToString() + "','" + DateTime.Now.ToShortTimeString() + "','" + DateTime.Now.Date + "')", con, tran);
                                cmd3.ExecuteNonQuery();
                            }
                        }

                        SqlCommand cmd4 = new SqlCommand("select top 1 CustID,CustName,Contact from Customer order by CustID DESC", con, tran);
                        cmd4.ExecuteNonQuery();
                        SqlCommand cmd5 = new SqlCommand("select top 1 OrderID,OrderTime,OrderDate from Orders order by OrderID DESC", con, tran);
                        cmd5.ExecuteNonQuery();

                        using (SqlDataReader dr = cmd4.ExecuteReader())
                        {
                            using (SqlDataReader dr1 = cmd5.ExecuteReader())
                            {
                                string itemname;
                                string itemqty;
                                string itemprice;
                                double itempricewithGST;
                                double GST;
                                double GStunit = 0.17;
                                string billtotal = total_Amount.Text;
                                string totalqty = totalQty.Text;
                                double totalGSTcalcualtion = Convert.ToDouble(total_Amount.Text) * GStunit;
                                double totalAmountwithGST = Convert.ToDouble(total_Amount.Text) + totalGSTcalcualtion;
                                double discount = Convert.ToDouble(per_discount.Text);
                                double singleitemcollectiveamount;

                                while (dr.Read())
                                {
                                    CUSTID = dr["CustID"].ToString();
                                    CUSTNAME = dr["CustName"].ToString();
                                    CUSTCONTACT = dr["Contact"].ToString();
                                    while (dr1.Read())
                                    {
                                        ORDERID = dr1["OrderID"].ToString();
                                        ORDERTIME = dr1["OrderTime"].ToString();
                                        ORDERDATE = dr1["OrderDate"].ToString();

                                        SqlCommand cmd7 = new SqlCommand("insert into Sales values ('" + ORDERID + "','" + CUSTID + "','" + CUSTNAME + "','" + CUSTCONTACT + "','" + OrderType + "','" + OrderCategory + "','" + ORDERTIME + "','" + ORDERDATE + "')", con, tran);
                                        cmd7.ExecuteNonQuery();

                                        SqlCommand cmd6;
                                        for (int i = 0; i < dgv1.Rows.Count; i++)
                                        {
                                            itemname = Convert.ToString(dgv1.Rows[i].Cells[0].Value);
                                            itemqty = Convert.ToString(dgv1.Rows[i].Cells[1].Value);
                                            itemprice = Convert.ToString(dgv1.Rows[i].Cells[2].Value);
                                            singleitemcollectiveamount = Convert.ToDouble(dgv1.Rows[i].Cells[3].Value);
                                            GST = Convert.ToInt32(dgv1.Rows[i].Cells[3].Value) * GStunit;
                                            itempricewithGST = Convert.ToDouble(dgv1.Rows[i].Cells[2].Value) + GST;
                                            cmd6 = new SqlCommand("insert into Bill values ('" + InvocieId + "','" + CUSTID + "','" + ORDERID + "','" + CUSTNAME + "','" + itemname.ToString() + "','" + itemqty.ToString() + "','" + itemprice.ToString() + "','" + singleitemcollectiveamount.ToString() + "','" + itempricewithGST.ToString() + "','" + ORDERTIME + "','" + ORDERDATE + "','" + totalqty.ToString() + "','" + act_price.Text.ToString() + "','" + billtotal.ToString() + "','" + totalAmountwithGST.ToString() + "','" + discount.ToString() + "')", con, tran);
                                            cmd6.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                            tran.Commit();
                            con.Close();
                        }

                        DVPrintDocument.Print();
                        DVPrintDocument.Dispose();

                        DVPrintDocument2.Print();
                        DVPrintDocument2.Dispose();

                        inventoryFunctionality();

                        address.Text = string.Empty;
                        contact.Text = string.Empty;
                        dgv1.Rows.Clear();
                        dgv1.Refresh();
                        DiscountPKR.Text = "";
                        DiscountPercent.Text = "";
                        per_discount.Text = "0";
                        act_price.Text = "0";
                        totalQty.Text = "0";
                        total_Amount.Text = "0";

                        Timercheckingstock();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Transaction Failed for Unknown Reason");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to cancel the last transaction ?", "Delete Last Transaction ?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SqlConnection con = new SqlConnection(Helper.con);
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    SqlCommand cmd1 = new SqlCommand("select top 1 InvioceID from Bill order by InvioceID DESC", con, tran);
                    cmd1.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd1.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            INVOICEID = Convert.ToInt32(dr["InvioceID"]);

                            SqlCommand cmd3 = new SqlCommand("select InvioceID,CustID,OrderID,CustName,ProductName,ProductQuantity" +
                                                                ",ProductRate,ProductAmount,ProductAmountWithGST,OrderTime," +
                                                                "OrderDate,Totalqty,ActualAmount,TotalAmount,TotalAmountWithGST," +
                                                                "DiscountInPercent from Bill where InvioceID = '" + INVOICEID + "' ", con, tran);
                            cmd3.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd3.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    drInvoiceid = Convert.ToString(dr2["InvioceID"]);
                                    CustID = Convert.ToString(dr2["CustID"]);
                                    OrderID = Convert.ToString(dr2["OrderID"]);
                                    CustName = Convert.ToString(dr2["CustName"]);
                                    Product_Name = Convert.ToString(dr2["ProductName"]);
                                    ProductQuantity = Convert.ToString(dr2["ProductQuantity"]);
                                    ProductRate = Convert.ToString(dr2["ProductRate"]);
                                    ProductAmount = Convert.ToString(dr2["ProductAmount"]);
                                    ProductAmountWithGST = Convert.ToString(dr2["ProductAmountWithGST"]);
                                    OrderTime = Convert.ToString(dr2["OrderTime"]);
                                    OrderDate = Convert.ToString(dr2["OrderDate"]);
                                    TotalQty = Convert.ToString(dr2["Totalqty"]);
                                    ActualAmount = Convert.ToString(dr2["ActualAmount"]);
                                    TotalAmount = Convert.ToString(dr2["TotalAmount"]);
                                    TotalAmountWithGST = Convert.ToString(dr2["TotalAmountWithGST"]);
                                    DiscounInPercent = Convert.ToString(dr2["DiscountInPercent"]);

                                    SqlCommand cmd4 = new SqlCommand("insert into DeletedBill values ('" + drInvoiceid + "' , '" + CustID + "' , '" + OrderID + "' , '" + CustName + "' , '" + Product_Name + "' , '" + ProductQuantity + "' , '" + ProductRate + "' , '" + ProductAmount + "' , '" + ProductAmountWithGST + "' , '" + OrderTime + "' , '" + OrderDate + "' , '" + TotalQty + "' , '" + ActualAmount + "' , '" + TotalAmount + "' , '" + TotalAmountWithGST + "' , '" + DiscounInPercent + "') ", con, tran);
                                    cmd4.ExecuteNonQuery();
                                }
                            }
                            SqlCommand cmd2 = new SqlCommand("delete from Bill where InvioceID = '" + INVOICEID + "' ", con, tran);
                            cmd2.ExecuteNonQuery();
                            MessageBox.Show("Transaction deleted");
                        }
                        tran.Commit();
                        con.Close();
                    }
                }
                else if (result == DialogResult.No)
                {
                    MessageBox.Show("Transaction exist");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error deleting last transaction");
            }
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                totalAmount = 0; totalQuantity = 0;
                totalQty.Text = totalQuantity.ToString();
                total_Amount.Text = totalAmount.ToString();

                var row = dgv1.CurrentRow;

                if (row == null || row.Index < 0)
                    return;
                var unit = 1;
                var quantity = Convert.ToInt32(row.Cells["qtty"].Value) + unit;
                //assuming you have rate column...
                var rate = Convert.ToInt32(row.Cells["rate"].Value);

                //this line increase quantity
                row.Cells["qtty"].Value = quantity;
                //this line increase amount
                row.Cells["price"].Value = Convert.ToInt32(row.Cells["price"].Value) + rate;


                for (int i = 0; i < dgv1.Rows.Count; ++i)
                {
                    totalQuantity += Convert.ToInt32(dgv1.Rows[i].Cells[1].Value);
                    totalAmount += Convert.ToInt32(dgv1.Rows[i].Cells[3].Value);
                }
                totalQty.Text = totalQuantity.ToString();
                total_Amount.Text = totalAmount.ToString();
                act_price.Text = totalAmount.ToString();

            }
            else if (e.ColumnIndex == 6)
            {
                totalAmount = 0; totalQuantity = 0;
                totalQty.Text.Trim();
                //totalQty.Text = totalQuantity.ToString();
                total_Amount.Text = totalAmount.ToString();

                var row = dgv1.CurrentRow;

                if (row == null || row.Index < 0)
                    return;
                var unit = 1;
                var quantity = Convert.ToInt32(row.Cells["qtty"].Value) - unit;
                //assuming you have rate column...
                var rate = Convert.ToInt32(row.Cells["rate"].Value);

                if (quantity < 1)
                {
                    this.dgv1.Rows.RemoveAt(e.RowIndex);

                    for (int i = 0; i < dgv1.Rows.Count; ++i)
                    {
                        totalQuantity += Convert.ToInt32(dgv1.Rows[i].Cells[1].Value);
                        totalAmount += Convert.ToInt32(dgv1.Rows[i].Cells[3].Value);
                    }
                    totalQty.Text = totalQuantity.ToString();
                    total_Amount.Text = totalAmount.ToString();
                    act_price.Text = totalAmount.ToString();
                    //
                    per_discount.Text = "0";
                    DiscountPercent.Text = "";
                    DiscountPKR.Text = "";
                }
                else
                {
                    //this line decrease quantity
                    row.Cells["qtty"].Value = quantity;
                    //this line decrease amount
                    row.Cells["price"].Value = Convert.ToInt32(row.Cells["price"].Value) - rate;

                    for (int i = 0; i < dgv1.Rows.Count; ++i)
                    {
                        //totalAmount = 0; totalQuantity = 0;
                        totalQuantity += Convert.ToInt32(dgv1.Rows[i].Cells[1].Value);
                        totalAmount += Convert.ToInt32(dgv1.Rows[i].Cells[3].Value);
                    }
                    totalQty.Text = totalQuantity.ToString();
                    total_Amount.Text = totalAmount.ToString();
                    act_price.Text = totalAmount.ToString();
                    //
                    per_discount.Text = "0";
                    DiscountPercent.Text = "";
                    DiscountPKR.Text = "";
                }


            }

            else if (dgv1.CurrentCell.ColumnIndex == 7)
            {
                this.dgv1.Rows.RemoveAt(e.RowIndex);

                totalAmount = 0; totalQuantity = 0;
                totalQty.Text = totalQuantity.ToString();
                total_Amount.Text = totalAmount.ToString();

                for (int i = 0; i < dgv1.Rows.Count; ++i)
                {
                    totalQuantity += Convert.ToInt32(dgv1.Rows[i].Cells[1].Value);
                    totalAmount += Convert.ToInt32(dgv1.Rows[i].Cells[3].Value);
                }
                totalQty.Text = totalQuantity.ToString();
                total_Amount.Text = totalAmount.ToString();
                act_price.Text = totalAmount.ToString();
                //
                per_discount.Text = "0";
                DiscountPercent.Text = "";
                DiscountPKR.Text = "";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            address.Text = string.Empty;
            contact.Text = string.Empty;
            dgv1.Rows.Clear();
            dgv1.Refresh();
            DiscountPKR.Text = "";
            DiscountPercent.Text = "";
            per_discount.Text = "0";
            act_price.Text = "0";
            totalQty.Text = "0";
            total_Amount.Text = "0";
            flowLayoutPanel1.Controls.Clear();
            Loadproducts();
            flowLayoutPanel2.Controls.Clear();
            Loadcategory();
        }

        private void cANCELINVOICESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelInvoice ci = new CancelInvoice();
            ci.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void DVPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            SqlCommand cmd10 = new SqlCommand("select top 1 InvioceID,Totalqty," +
                "TotalAmount,TotalAmountWithGST,ActualAmount," +
                "DiscountInPercent from Bill order by InvioceID DESC", con, tran);
            cmd10.ExecuteNonQuery();

            using (SqlDataReader dr = cmd10.ExecuteReader())
            {
                while (dr.Read())
                {
                    INVOICEID = Convert.ToInt32(dr["InvioceID"]);
                    Total_Qty = Convert.ToString(dr["Totalqty"]);
                    Total_Amount = Convert.ToString(dr["TotalAmount"]);
                    _TotalWithGST = Convert.ToString(dr["TotalAmountWithGST"]);
                    Actual_Amount = Convert.ToString(dr["ActualAmount"]);
                    _Discount = Convert.ToString(dr["DiscountInPercent"]);
                }
            }

            tran.Commit();
            con.Close();

            string address2, contact2;
            if(contact.Text == string.Empty || address.Text == string.Empty)
            {
                contact2 = "***********";
                address2 = "----------------";
            }
            else
            {
                contact2 = contact.Text.ToString();
                address2 = address.Text;
            }

            e.Graphics.DrawString("FOODIES CLUB", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(60, 30));
            //e.Graphics.DrawString("GST# 222-333-123456", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(130, 70));
            e.Graphics.DrawString("Invoice ID :", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(10, 70));
            e.Graphics.DrawString(INVOICEID.ToString(), new Font("Arial",8, FontStyle.Bold), Brushes.Black, new Point(70, 70));
            e.Graphics.DrawString("Date :", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(135, 70));
            e.Graphics.DrawString(DateTime.Now.Date.ToShortDateString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(170, 70));
            e.Graphics.DrawString("Time :", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(10, 90));
            e.Graphics.DrawString(DateTime.Now.ToShortTimeString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(45, 90));
            e.Graphics.DrawString("Contact :", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(130, 90));
            e.Graphics.DrawString(contact2.ToString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(180, 90));
            e.Graphics.DrawString("Address :", new Font("Arial", 7, FontStyle.Regular), Brushes.Black, new Point(10,110));
            e.Graphics.DrawString(address2.ToString(), new Font("Arial", 7, FontStyle.Regular), Brushes.Black, new Point(55, 110));
            //
            e.Graphics.DrawString("------------------------------------------------------",
               new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 130));
            e.Graphics.DrawString("SALES ITEMS", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(80, 150));
            e.Graphics.DrawString("------------------------------------------------------",
            new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 170));
            //
            e.Graphics.DrawString("PRODUCT NAME", new Font("Arial", 7, FontStyle.Bold), Brushes.Black, new Point(5, 195));
            e.Graphics.DrawString("QUANTITY", new Font("Arial", 7, FontStyle.Bold), Brushes.Black, new Point(120, 195));
            e.Graphics.DrawString("RATE", new Font("Arial", 7, FontStyle.Bold), Brushes.Black, new Point(180, 195));
            e.Graphics.DrawString("AMOUNT", new Font("Arial", 7, FontStyle.Bold), Brushes.Black, new Point(220, 195));

            int position = 210;

            for (int i = 0; i < dgv1.Rows.Count; i++)
            {
                position = position + 20;
                e.Graphics.DrawString(Convert.ToString(dgv1.Rows[i].Cells[0].Value), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(5, position));
                e.Graphics.DrawString(Convert.ToString(dgv1.Rows[i].Cells[1].Value) + ".00", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(135, position));
                e.Graphics.DrawString(Convert.ToString(dgv1.Rows[i].Cells[2].Value) + ".00", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(175, position));
                e.Graphics.DrawString(Convert.ToString(dgv1.Rows[i].Cells[3].Value) + ".00", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(225, position));
            }
            //
            e.Graphics.DrawString("------------------------------------------------------",
           new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, position + 20));
            //
            e.Graphics.DrawString("Actual Amount", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(10, position + 40));
            e.Graphics.DrawString(Actual_Amount.ToString() + ".00", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(190, position + 40));
            e.Graphics.DrawString("Total Quantity", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(10, position + 60));
            e.Graphics.DrawString(Total_Qty.ToString() + ".00", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(190, position + 60));
            e.Graphics.DrawString("Total Amount", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(10, position + 80));
            e.Graphics.DrawString(Total_Amount.ToString() + ".00", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(190, position + 80));
            e.Graphics.DrawString("Discount", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(10, position + 100));
            e.Graphics.DrawString(_Discount.ToString()+" %", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(190, position + 100));
            //
            e.Graphics.DrawString("------------------------------------------------------",
           new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, position + 120));
            //
            e.Graphics.DrawString("TOKEN NO " + INVOICEID.ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new Point(10, position + 140));

        }

        private void aAAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory();
            inventory.Show();
        }

        private void DVPrintDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            SqlCommand cmd10 = new SqlCommand("select top 1 InvioceID from Bill order by InvioceID DESC", con, tran);
            cmd10.ExecuteNonQuery();

            using (SqlDataReader dr = cmd10.ExecuteReader())
            {
                while (dr.Read())
                {
                    INVOICEID2 = Convert.ToString(dr["InvioceID"]);
                }
            }

            tran.Commit();
            con.Close();

            e.Graphics.DrawString("TOKEN NO " + INVOICEID2.ToString(), new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(5, 30));
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            //timer1.Stop();
            //timer1.Dispose();
        }

        private void cREATECATEGORIESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoryForm cf = new CategoryForm();
            cf.Show();
        }

        private void mANAGECATEGORIESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageCategory mc = new ManageCategory();
            mc.Show();
        }

        private void pRODUCTPRICESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductPrice pp = new ProductPrice();
            pp.Show();
        }

        private void cREATEPRODUCTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductForm pf = new ProductForm();
            pf.Show();
        }

        private void mANAGEPRODUCTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageProduct mp = new ManageProduct();
            mp.Show();
        }

        private void dAILYSALESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailySales ds = new DailySales();
            ds.Show();
        }

        private void wEEKLYSALESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WeeklySales ws = new WeeklySales();
            ws.Show();
        }

        private void mONTHLYSALESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonthlySales ms = new MonthlySales();
            ms.Show();
        }

        private void aNNUALSALESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnnualSales As = new AnnualSales();
            As.Show();
        }

        private void cLOCKOUTLOGOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }

        private void cLOSEAPPLICATIONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Cashier_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dELETEDINVOICEDATAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeletedInvoice di = new DeletedInvoice();
            di.Show();
        }

        private void sEARCHINVOICESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvoiceDetails id = new InvoiceDetails();
            id.Show();
        }

        private void eDITINVOICEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditInvoice ei = new EditInvoice();
            ei.Show();
        }

        private void cCCCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            users us = new users();
            us.Show();
        }

        private void bBBBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stocks st = new Stocks();
            st.Show();
        }

        private void sALESSUMMARYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesSummary ss = new SalesSummary();
            ss.Show();
        }

        private void eXPORTTOEXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel export = new ExportToExcel();
            export.Show();
        }

        private void aBOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                if (DiscountPKR.Text == "")
                {
                    per_discount.Text = DiscountPercent.Text;
                    decimal labelActualAmount = Convert.ToInt32(act_price.Text);
                    decimal discountvalue = Convert.ToInt32(DiscountPercent.Text);
                    decimal discoutPercentage = discountvalue / 100;
                    decimal remainAmount = discoutPercentage * labelActualAmount;
                    decimal ActualAmount = totalAmount - remainAmount;
                    total_Amount.Text = Convert.ToInt32(ActualAmount).ToString();
                    DiscountPKR.Text = "";
                    DiscountPercent.Text = "";
                }
                else if (DiscountPercent.Text == "")
                {
                    decimal discountAmountinPKR = Convert.ToInt32(DiscountPKR.Text);
                    decimal labelActualAmount = Convert.ToInt32(act_price.Text);
                    decimal ActualAmount = labelActualAmount - discountAmountinPKR;
                    total_Amount.Text = Convert.ToInt32(ActualAmount).ToString();
                    decimal percentageForAmount = discountAmountinPKR / labelActualAmount * 100;
                    per_discount.Text = Convert.ToString(Convert.ToInt32(percentageForAmount));
                    DiscountPKR.Text = "";
                    DiscountPercent.Text = "";
                }
                else
                {
                    MessageBox.Show("Either insert discount in rupees or in percentage");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Discount feild cannot be empty");
            }
        }

        public void inventoryFunctionality()
        {
            //Declaration for Dr read 
            int stockid, stockweigth, newstockweigth;
            string stockname, stockcompany, stockcategory, stockdate, stocktime;

            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            foreach (DataGridViewRow Row in dgv1.Rows)
            {
                for (int i = 0; i < 1; i++)
                {
                    if (Convert.ToString(Row.Cells[0].Value) == zingerburger)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'flay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'bag' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }

                    // else if one

                    else if (Convert.ToString(Row.Cells[0].Value) == mightyburger)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'flay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 2 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }
                    // else if 1 ends

                    // else if 2 start

                    else if (Convert.ToString(Row.Cells[0].Value) == beefburger)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'beef' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'bag' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }
                    // else if 2 ends

                    // else if 3 starts

                    else if (Convert.ToString(Row.Cells[0].Value) == beefdoublepatty)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'beef' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 2 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'cheeze slice' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }
                    // else if 3 ends

                    //else if 4 starts

                    else if (Convert.ToString(Row.Cells[0].Value) == chickenburger)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'flay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'bag' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }
                    // esle if 4 ends 

                    //else if 5 starts

                    else if (Convert.ToString(Row.Cells[0].Value) == doublechickenburger)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'flay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 2 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }
                    //else if ends 

                    //new else if starts

                    else if (Convert.ToString(Row.Cells[0].Value) == hulk)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'flay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'bag' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'beef' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'cheeze slice' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd16.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                        }
                    }
                    // else if ends 

                    // else if starts 

                    else if (Convert.ToString(Row.Cells[0].Value) == smokeburger)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'flay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 2 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'cheeze slice' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd16.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                        }
                    }

                    //else if ends

                    //else if starts 

                    else if (Convert.ToString(Row.Cells[0].Value) == dhabardoze)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'flay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'bag' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'cheeze slice' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd16.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                        }
                    }

                    //else if ends smoke burger

                    //else if starts beef jalapeno

                    else if (Convert.ToString(Row.Cells[0].Value) == beefjalapeno)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'beef' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'ketchup' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'bag' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'mayoneze' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'chili garlic' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'jalapeno' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'cheeze slice' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd16.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                        }
                    }

                    //else if ends beef jalapeno

                    //else if start club sandwich

                    else if (Convert.ToString(Row.Cells[0].Value) == clubsandwich)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'bread' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'chicken' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 45 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'egg' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'garlic' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }

                    //else if end club sandwich

                    //else if starts chicken sandwich

                    else if (Convert.ToString(Row.Cells[0].Value) == chickensandwich)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'bread' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'chicken' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 55 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'egg' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'garlic' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }

                    //else if ends chicken sandwich

                    //else if starts BBQ sandwich

                    else if (Convert.ToString(Row.Cells[0].Value) == bbqsandwich)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'bread' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'chicken' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 45 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'egg' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'garlic' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'sashai ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }

                    //else if ends BBQ sandwich

                    //else if starts Dead End Sandwich

                    else if (Convert.ToString(Row.Cells[0].Value) == deadendsandwich)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'bread' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'chicken' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 45 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'egg' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'garlic' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }

                    //else if end Dead end sandwich

                    //else if starts Smoke sandwich

                    else if (Convert.ToString(Row.Cells[0].Value) == smokesandwich)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'bread' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'chicken' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'egg' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'garlic' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }
                        }
                    }

                    //else if end Smoke sandwich

                    //else if starts Malai boti Sandwich

                    else if (Convert.ToString(Row.Cells[0].Value) == malaibotisandwich)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'bread' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 3 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'chicken' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr2 = cmd2.ExecuteReader())
                            {
                                while (dr2.Read())
                                {
                                    stockid = Convert.ToInt32(dr2["stockid"]);
                                    stockname = Convert.ToString(dr2["stockname"]);
                                    stockweigth = Convert.ToInt32(dr2["stockweigth"]);
                                    stockcompany = Convert.ToString(dr2["stockcompany"]);
                                    stockcategory = Convert.ToString(dr2["stockcategory"]);
                                    stockdate = Convert.ToString(dr2["stockdate"]);
                                    stocktime = Convert.ToString(dr2["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 60 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'egg' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr3 = cmd4.ExecuteReader())
                            {
                                while (dr3.Read())
                                {
                                    stockid = Convert.ToInt32(dr3["stockid"]);
                                    stockname = Convert.ToString(dr3["stockname"]);
                                    stockweigth = Convert.ToInt32(dr3["stockweigth"]);
                                    stockcompany = Convert.ToString(dr3["stockcompany"]);
                                    stockcategory = Convert.ToString(dr3["stockcategory"]);
                                    stockdate = Convert.ToString(dr3["stockdate"]);
                                    stocktime = Convert.ToString(dr3["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr4 = cmd6.ExecuteReader())
                            {
                                while (dr4.Read())
                                {
                                    stockid = Convert.ToInt32(dr4["stockid"]);
                                    stockname = Convert.ToString(dr4["stockname"]);
                                    stockweigth = Convert.ToInt32(dr4["stockweigth"]);
                                    stockcompany = Convert.ToString(dr4["stockcompany"]);
                                    stockcategory = Convert.ToString(dr4["stockcategory"]);
                                    stockdate = Convert.ToString(dr4["stockdate"]);
                                    stocktime = Convert.ToString(dr4["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'garlic' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr5 = cmd8.ExecuteReader())
                            {
                                while (dr5.Read())
                                {
                                    stockid = Convert.ToInt32(dr5["stockid"]);
                                    stockname = Convert.ToString(dr5["stockname"]);
                                    stockweigth = Convert.ToInt32(dr5["stockweigth"]);
                                    stockcompany = Convert.ToString(dr5["stockcompany"]);
                                    stockcategory = Convert.ToString(dr5["stockcategory"]);
                                    stockdate = Convert.ToString(dr5["stockdate"]);
                                    stocktime = Convert.ToString(dr5["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr6 = cmd10.ExecuteReader())
                            {
                                while (dr6.Read())
                                {
                                    stockid = Convert.ToInt32(dr6["stockid"]);
                                    stockname = Convert.ToString(dr6["stockname"]);
                                    stockweigth = Convert.ToInt32(dr6["stockweigth"]);
                                    stockcompany = Convert.ToString(dr6["stockcompany"]);
                                    stockcategory = Convert.ToString(dr6["stockcategory"]);
                                    stockdate = Convert.ToString(dr6["stockdate"]);
                                    stocktime = Convert.ToString(dr6["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd12.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd14.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'cheeze slice' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr7 = cmd16.ExecuteReader())
                            {
                                while (dr7.Read())
                                {
                                    stockid = Convert.ToInt32(dr7["stockid"]);
                                    stockname = Convert.ToString(dr7["stockname"]);
                                    stockweigth = Convert.ToInt32(dr7["stockweigth"]);
                                    stockcompany = Convert.ToString(dr7["stockcompany"]);
                                    stockcategory = Convert.ToString(dr7["stockcategory"]);
                                    stockdate = Convert.ToString(dr7["stockdate"]);
                                    stocktime = Convert.ToString(dr7["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 2 * quantity;

                                        if (newstockweigth > -1)
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }
                                }
                            }

                        }
                    }

                    //Else if end Malai Boti Sandwich

                    //Else if start Chest Broast 

                    else if (Convert.ToString(Row.Cells[0].Value) == chestbroast)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chest' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd4.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                        }
                    }

                    //Elese if ends Chest Broast

                    //Else if start Leg braost

                    else if (Convert.ToString(Row.Cells[0].Value) == legbroast)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'leg' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd4 = new SqlCommand("select * from Stock where stockname = 'bun' ", con, tran);
                            cmd4.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd4.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd5 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd5.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                        }
                    }

                    //Else if ends Leg broast

                    //Else if start Gyco

                    else if (Convert.ToString(Row.Cells[0].Value) == gyco)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'flay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'pita bread' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'fries' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                        }
                    }

                    //Else if ends Gyco

                    //Else if start small chicken tikka

                    else if (Convert.ToString(Row.Cells[0].Value) == smallchickentikka)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 8 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sahsai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'small dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends small chicken tikka

                    //Else if start medium chicken tikka

                    else if (Convert.ToString(Row.Cells[0].Value) == mediumchickentikka)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'medium dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends medium chicken tikka

                    //Else if start large chicken tikka

                    else if (Convert.ToString(Row.Cells[0].Value) == largechickentikka)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'large dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends large chicken tikka

                    //Else if start small chicken fajita)

                    else if (Convert.ToString(Row.Cells[0].Value) == smallchickenfajita)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 8 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'small dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends small chicken fajita

                    //Else if start medium chicken fajita

                    else if (Convert.ToString(Row.Cells[0].Value) == mediumchickenfajita)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'medium dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends medium chicken fajita

                    //Else if start large chicken fajita

                    else if (Convert.ToString(Row.Cells[0].Value) == largechickenfajita)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'large dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends large chicken fajita

                    // Else if start small foodies special

                    else if (Convert.ToString(Row.Cells[0].Value) == smallfoodiesspecial)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 45 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 8 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'small dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd18 = new SqlCommand("select * from Stock where stockname = 'jalapeno' ", con, tran);
                            cmd18.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd18.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd19 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd19.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd19 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd19.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends small foodies special

                    // Else if start medium foodies special

                    else if (Convert.ToString(Row.Cells[0].Value) == mediumfoodiesspecial)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'medium dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd18 = new SqlCommand("select * from Stock where stockname = 'jalapeno' ", con, tran);
                            cmd18.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd18.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd19 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd19.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd19 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd19.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends medium foodies special

                    //Else if start large foodies special

                    else if (Convert.ToString(Row.Cells[0].Value) == largefoodiesspecial)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'large dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd18 = new SqlCommand("select * from Stock where stockname = 'jalapeno' ", con, tran);
                            cmd18.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd18.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd19 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd19.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd19 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd19.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends large foodies special

                    // Else if start small vegi lover

                    else if (Convert.ToString(Row.Cells[0].Value) == smallvegilover)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 8 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'small dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends small vegi lover

                    // Else if start medium vegi lover

                    else if (Convert.ToString(Row.Cells[0].Value) == mediumvegilover)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'medium dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends medium vegi lover

                    //Else if start large vegi lover

                    else if (Convert.ToString(Row.Cells[0].Value) == largevegilover)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'large dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends vegi lover

                    // Else if start small super supremo

                    else if (Convert.ToString(Row.Cells[0].Value) == smallsupersupremo)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 45 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 8 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'small dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends small super supremo

                    // Else if start medium super supremo

                    else if (Convert.ToString(Row.Cells[0].Value) == mediumsupersupremo)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'medium dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends medium super supremo

                    //Else if start large super supremo

                    else if (Convert.ToString(Row.Cells[0].Value) == largesupersupremo)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chickentikkaflay' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'large dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                        }
                    }

                    //Else if ends large super supremo

                    // Else if start small cheese lover

                    else if (Convert.ToString(Row.Cells[0].Value) == smallcheeselover)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 8 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'small dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends small cheese lover

                    // Else if start medium cheese lover

                    else if (Convert.ToString(Row.Cells[0].Value) == mediumcheeselover)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'medium dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                            //
                        }
                    }

                    //Else if ends medium cheese lover

                    //Else if start large cheese lover

                    else if (Convert.ToString(Row.Cells[0].Value) == largecheeselover)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'red sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'large dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd14 = new SqlCommand("select * from Stock where stockname = 'mashroom' ", con, tran);
                            cmd14.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd14.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd15 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd15.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd16 = new SqlCommand("select * from Stock where stockname = 'olive' ", con, tran);
                            cmd16.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd16.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd17 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd17.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                        }
                    }

                    //Else if ends large cheese lover

                    //Else if start small malai boti

                    else if (Convert.ToString(Row.Cells[0].Value) == smallmalaiboti)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'malai boti chicken' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 35 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'green sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 8 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'small dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends small malai botia

                    //Else if start medium malai boti

                    else if (Convert.ToString(Row.Cells[0].Value) == mediummalaiboti)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'malai boti chicken' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 70 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'green sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 15 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'medium dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends medium malai boti

                    //Else if start large malai boti

                    else if (Convert.ToString(Row.Cells[0].Value) == largemalaiboti)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'malai boti chicken' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'cheese' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 120 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd6 = new SqlCommand("select * from Stock where stockname = 'green sauce' ", con, tran);
                            cmd6.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd6.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd7 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd7.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd8 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd8.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd8.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd9 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd9.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd10 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd10.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd10.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd11 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd11.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd12 = new SqlCommand("select * from Stock where stockname = 'large dow' ", con, tran);
                            cmd12.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd12.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd13 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd13.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }
                        }
                    }

                    //Else if ends large chicken tikka

                    //Else if start chicken strips

                    else if (Convert.ToString(Row.Cells[0].Value) == chickenstrips)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'chicken' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 40 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 10 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd4 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd4.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd5 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd5.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd5.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd6 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd6.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd6 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd6.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd7 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd7.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd7.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd8 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd8.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd8 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd8.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    // Else if ends Chicken strips

                    //Else if chicken crispy wrap

                    //Else if start small chicken tikka

                    else if (Convert.ToString(Row.Cells[0].Value) == chickenstrips)
                    {
                        string gridname = Convert.ToString(Row.Cells[0].Value);
                        int quantity = Convert.ToInt32(Row.Cells[1].Value);
                        exist = true;
                        if (exist == true)
                        {
                            //MessageBox.Show("Found");
                            SqlCommand cmd = new SqlCommand("select * from Stock where stockname = 'pita bread' ", con, tran);
                            cmd.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd1 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd2 = new SqlCommand("select * from Stock where stockname = 'mayo' ", con, tran);
                            cmd2.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd2.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 20 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd3 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd3.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd4 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd4.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd5 = new SqlCommand("select * from Stock where stockname = 'sashai' ", con, tran);
                            cmd5.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd5.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd6 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd6.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd6 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd6.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            SqlCommand cmd7 = new SqlCommand("select * from Stock where stockname = 'box' ", con, tran);
                            cmd7.ExecuteNonQuery();

                            using (SqlDataReader dr = cmd7.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    stockid = Convert.ToInt32(dr["stockid"]);
                                    stockname = Convert.ToString(dr["stockname"]);
                                    stockweigth = Convert.ToInt32(dr["stockweigth"]);
                                    stockcompany = Convert.ToString(dr["stockcompany"]);
                                    stockcategory = Convert.ToString(dr["stockcategory"]);
                                    stockdate = Convert.ToString(dr["stockdate"]);
                                    stocktime = Convert.ToString(dr["stocktime"]);

                                    if (stockweigth > 0)
                                    {
                                        newstockweigth = stockweigth - 1 * quantity;

                                        if (newstockweigth > -1)
                                        {

                                            SqlCommand cmd8 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + newstockweigth + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd8.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlCommand cmd8 = new SqlCommand("update Stock set stockname = '" + stockname + "' , stockweigth = '" + 0 + "', stockcompany = '" + stockcompany + "', stockcategory = '" + stockcategory + "', stockdate = '" + stockdate + "', stocktime = '" + stocktime + "' where stockid = '" + stockid + "'  ", con, tran);
                                            cmd8.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        //Do nothing
                                    }

                                }
                            }

                            //
                        }
                    }

                    //Else if ends chicken crispy wrap

                    else
                    {
                        exist = false;
                    }
                }
            }
            tran.Commit();
            con.Close();
        }

        // Code Ends 
    }
}