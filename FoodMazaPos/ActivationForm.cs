using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Foodies;

namespace FoodMazaPos
{
    public partial class ActivationForm : Form
    {
        SqlConnection con = new SqlConnection(Helper.con);

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "uJgAuCnjXHkp1egiwoUwDdNazHszBH1S4KERuqas",
            BasePath = "https://foodtime-3fae4.firebaseio.com/",
        };

        IFirebaseClient client;

        public ActivationForm()
        {
            InitializeComponent();
        }

        public void Firebase()
        {
            client = new FireSharp.FirebaseClient(config);

            if (client != null)
            {
                //MessageBox.Show("Connection is established");
            }
        }

        private void ActivationForm_Load(object sender, EventArgs e)
        {
            Firebase();
        }

        private void ClosrForm_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Data keydata = new Data
                {
                    key = textBox1.Text
                };

                FirebaseResponse response2 = await client.GetTaskAsync("KeyData/" + textBox1.Text);
                Data obj = response2.ResultAs<Data>();

                if (obj.key.ToString() == textBox1.Text.ToString())
                {
                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                                                         // Get the IP  
#pragma warning disable CS0618 // Type or member is obsolete
                    string myIP = Dns.GetHostByName(hostName: hostName).AddressList[0].ToString();
#pragma warning restore CS0618 // Type or member is obsolete
                    //MessageBox.Show("My IP Address is :" + myIP);

                    var HostInfo = new Activeinfo
                    {
                        host = hostName,
                        IP = myIP,
                        Key = textBox1.Text
                    };

                    var Activation = new ActivationConfirmation
                    {
                        host = hostName,
                        IP = myIP,
                        Key = textBox1.Text,
                        AppStatus = "active",
                    };

                    try
                    {
                        FirebaseResponse response4 = await client.GetTaskAsync("HostInfo/" + textBox1.Text);
                        Activeinfo activeinfo = response4.ResultAs<Activeinfo>();

                        //MessageBox.Show("Retreive data from firebase\n" + "Key : " + activeinfo.Key + "Host : " + activeinfo.host + "IP : " + activeinfo.IP + "");

                        if (activeinfo.Key.ToString().Equals(textBox1.Text) && activeinfo.IP == myIP
                            && activeinfo.host.ToString() == hostName)
                        {
                            SetResponse response5 = await client.SetTaskAsync("ActivationConfirmation/" + textBox1.Text, Activation);
                            Activeinfo result5 = response5.ResultAs<Activeinfo>();

                            FirebaseResponse response6 = await client.GetTaskAsync("ActivationConfirmation/" + textBox1.Text);
                            ActivationConfirmation activeconfirmation = response6.ResultAs<ActivationConfirmation>();

                            if (activeconfirmation.AppStatus.Equals("active"))
                            {
                                string Appstatus = "activated";
                                con.Open();
                                SqlCommand cmd = con.CreateCommand();
                                cmd.CommandType = CommandType.Text;
                                string sqlQuery = "insert into Activation(Appstatus) values ('" + Appstatus + "')";
                                cmd = new SqlCommand(sqlQuery, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                                MessageBox.Show("Activation Done\nYou need to again open app for further use");
                                Application.Exit();
                            }
                            else
                            {
                                MessageBox.Show("Activation Failed");
                            }
                        }
                        else if (activeinfo.Key.ToString() == textBox1.ToString() && activeinfo.IP == myIP
                            && activeinfo.host.ToString() != hostName)
                        {
                            MessageBox.Show("Please contact the developer or visit the product website for obtaining a new key."
                           + "\nThis key has been already used in another PC and licensed to it.");
                        }
                        else if (activeinfo.Key.ToString() == textBox1.ToString() && activeinfo.IP != myIP
                            && activeinfo.host.ToString() == hostName)
                        {
                            MessageBox.Show("Please contact the developer or visit the product website for obtaining a new key."
                           + "\nThis key has been already used in another PC and licensed to it.");
                        }
                        else if (activeinfo.Key.ToString() != textBox1.ToString() && activeinfo.IP == myIP
                            && activeinfo.host.ToString() == hostName)
                        {
                            MessageBox.Show("Key mismatched try");
                        }

                    }
                    catch (Exception)
                    {
                        SetResponse response3 = await client.SetTaskAsync("HostInfo/" + textBox1.Text, HostInfo);
                        Activeinfo result3 = response3.ResultAs<Activeinfo>();
                        try
                        {
                            FirebaseResponse response4 = await client.GetTaskAsync("HostInfo/" + textBox1.Text);
                            Activeinfo activeinfo = response4.ResultAs<Activeinfo>();

                            if (activeinfo.Key.ToString().Equals(textBox1.Text) && activeinfo.IP == myIP
                                && activeinfo.host.ToString() == hostName)
                            {
                                SetResponse response5 = await client.SetTaskAsync("ActivationConfirmation/" + textBox1.Text, Activation);
                                Activeinfo result5 = response5.ResultAs<Activeinfo>();

                                FirebaseResponse response6 = await client.GetTaskAsync("ActivationConfirmation/" + textBox1.Text);
                                ActivationConfirmation activeconfirmation = response6.ResultAs<ActivationConfirmation>();

                                if (activeconfirmation.AppStatus.Equals("active"))
                                {
                                    string Appstatus = "activated";
                                    con.Open();
                                    SqlCommand cmd = con.CreateCommand();
                                    cmd.CommandType = CommandType.Text;
                                    string sqlQuery = "insert into Activation(Appstatus) values ('" + Appstatus + "')";
                                    cmd = new SqlCommand(sqlQuery, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    MessageBox.Show("Activation Done\nYou need to again open app for further use");
                                    Application.Exit();
                                }
                                else
                                {
                                    MessageBox.Show("Activation Failed");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please contact the developer or visit the product website for obtaining a new key."
                         + "\nThis key has been already used in another PC and licensed to it.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid key or key mismatched\n" +
                    "Please contact the developer or visit the product website for obtaining a new key.");
            }
        }
    }
}
