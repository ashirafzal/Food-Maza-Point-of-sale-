using Foodies;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace FoodMazaPos
{
    public partial class Login : Form
    {
        string User_name = "ashirxyz", category = "admin";
        string druser, drcategory;
        SqlConnection con = new SqlConnection(Helper.con);

        public Login()
        {
            InitializeComponent();
            username.Focus();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //CheckInternetavaibility();
            //PcChecking();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            con.Open();
            try
            {
                if (username.Text == "pos" || password.Text == "pos123")
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into LoginDetails (Loginuser,usercategory,time,date) values ('" + User_name + "','" + category + "','" + DateTime.Now.ToShortTimeString() + "','" + DateTime.Now.ToShortDateString() + "')";
                    cmd.ExecuteNonQuery();
                    Cashier main = new Cashier();
                    this.Hide();
                    main.Show();
                }
                else if (username.Text == string.Empty || password.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Username and Password");
                }
                else
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT username,password from users where username = '" + username.Text.ToLower() + "' and password = '" + password.Text.ToLower() + "' ", con);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    //Transaction start
                    SqlTransaction tran = con.BeginTransaction();

                    if (table.Rows.Count > 0)
                    {
                        SqlCommand cmd10 = new SqlCommand("select * from users where username = '" + username.Text.ToLower() + "' and password = '" + password.Text.ToLower() + "' ", con, tran);
                        cmd10.ExecuteNonQuery();

                        using (SqlDataReader dr = cmd10.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                druser = Convert.ToString(dr["username"]);
                                drcategory = Convert.ToString(dr["category"]);
                            }
                        }
                        tran.Commit();
                        // Transaction closed
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into LoginDetails (Loginuser,usercategory,time,date) values ('" + druser + "','" + drcategory + "','" + DateTime.Now.ToShortTimeString() + "','" + DateTime.Now.ToShortDateString() + "')";
                        cmd.ExecuteNonQuery();

                        Cashier main = new Cashier();
                        this.Hide();
                        main.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Please enter correct username & password or use the default username and password for login",
                    "User Mishandling", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            con.Close();
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

        public void PcChecking()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                                                 // Get the IP  
#pragma warning disable CS0618 // Type or member is obsolete
            string myIP = Dns.GetHostByName(hostName: hostName).AddressList[0].ToString();
#pragma warning restore CS0618 // Type or member is obsolete

            SqlDataAdapter adapter = new SqlDataAdapter("select top 1 Hostname,IpAddress from PcChecker", con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                SqlCommand cmd1 = new SqlCommand("select top 1 Hostname,IpAddress from PcChecker", con, tran);
                cmd1.ExecuteNonQuery();

                using (SqlDataReader dr = cmd1.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string drhostName = Convert.ToString(dr["Hostname"]);
                        string drmyIP = Convert.ToString(dr["IpAddress"]);

                        if (drhostName == hostName && drmyIP == myIP)
                        {
                            //IP and Host Matched
                            //Need to do nothing
                        }
                        else
                        {
                            MessageBox.Show("This Application is registered on another PC.\n" +
                                "you cannot use this app on another PC except the PC on which you first installed it.",
                                "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                tran.Commit();
                con.Close();
            }
            else
            {
                //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-9CBGPDG\ASHIRAFZAL;Initial Catalog=foodtime2;Integrated Security=True;Pooling=False");
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into PcChecker(Hostname,IpAddress) values ('" + hostName + "','" + myIP + "')";
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
