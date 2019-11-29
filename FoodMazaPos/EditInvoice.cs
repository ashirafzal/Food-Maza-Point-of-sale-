using Foodies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodMazaPos
{
    public partial class EditInvoice : Form
    {
        int totalQuantity = 0; double actualAmount = 0; int invoiceid2 = 0;
        string discount, totalAmount, DRactualAmount;
        string CUSTID, CUSTNAME, CUSTCONTACT, ORDERID;
        //double discountpercentageForAmount = 0;

        // For Bill variables
        string INVOICEID_1, INVOICEID_2, Total_Qty, Actual_Amount, Total_Amount, _TotalWithGST, _Discount;

        string INVOICEID2;

        private void BtnPrint_Click(object sender, EventArgs e)
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

                //
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
                    const string caption = "Transaction Limit";
                    var result = MessageBox.Show(message, caption,
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Exclamation);
                }
                else
                {
                    int invoiceid3 = invoiceid2;
                    string itemname;
                    string itemqty;
                    string itemprice;
                    double itempricewithGST;
                    double GST;
                    double GStunit = 0.17;
                    string billtotal = TotalAmount.Text;
                    string totalqty = TotalQty.Text;
                    double totalGSTcalcualtion = Convert.ToDouble(TotalAmount.Text) * GStunit;
                    double totalAmountwithGST = Convert.ToDouble(TotalAmount.Text) + totalGSTcalcualtion;
                    double discount = Convert.ToDouble(discountpercentageForAmount);
                    double singleitemcollectiveamount;

                    SqlConnection con = new SqlConnection(Helper.con);
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "delete from Bill where InvioceID ='" + invoiceid3 + "'";
                    cmd.ExecuteNonQuery();

                    string Custname = "Valuable Customer";
                    string CustContact = "***********";
                    string OrderType = "Food Item";
                    string OrderCategory = "Food";

                    SqlCommand cmd4;
                    cmd4 = new SqlCommand("update Customer set CustName =  '" + Custname + "', Contact = '" + CustContact + "', OrderTime = '" + DateTime.Now.ToShortTimeString() + "', OrderDate = '" + DateTime.Now.Date + "' where CustID = '" + CUSTID + "' ", con);
                    cmd4.ExecuteNonQuery();
                    SqlCommand cmd5;
                    cmd5 = new SqlCommand("update Orders set CustID = '" + CUSTID + "', OrderType = '" + OrderType.ToString() + "', OrderCategory = '" + OrderCategory.ToString() + "', Ordertime = '" + DateTime.Now.ToShortTimeString() + "', OrderDate = '" + DateTime.Now.Date + "' ", con);
                    cmd5.ExecuteNonQuery();

                    SqlCommand cmd6;
                    for (int i = 0; i < dgv1.Rows.Count; i++)
                    {
                        itemname = Convert.ToString(dgv1.Rows[i].Cells[4].Value);
                        itemqty = Convert.ToString(dgv1.Rows[i].Cells[5].Value);
                        itemprice = Convert.ToString(dgv1.Rows[i].Cells[6].Value);
                        singleitemcollectiveamount = Convert.ToDouble(dgv1.Rows[i].Cells[7].Value);
                        GST = Convert.ToDouble(dgv1.Rows[i].Cells[7].Value) * GStunit;
                        itempricewithGST = Convert.ToDouble(dgv1.Rows[i].Cells[7].Value) + GST;
                        cmd6 = new SqlCommand("insert into Bill values ('" + invoiceid2 + "','" + CUSTID + "','" + ORDERID + "','" + CUSTNAME + "','" + itemname.ToString() + "','" + itemqty.ToString() + "','" + itemprice.ToString() + "','" + singleitemcollectiveamount.ToString() + "','" + itempricewithGST.ToString() + "','" + DateTime.Now.ToShortTimeString() + "','" + DateTime.Now.Date + "','" + totalqty.ToString() + "','" + ActualAmount.Text.ToString() + "','" + billtotal.ToString() + "','" + totalAmountwithGST.ToString() + "','" + discount.ToString() + "')", con);
                        cmd6.ExecuteNonQuery();
                    }

                    SqlCommand cmd7;
                    cmd7 = new SqlCommand("insert into EditedBill values ('" + invoiceid2 + "')", con);
                    cmd7.ExecuteNonQuery();

                    con.Close();
                }

                //if ((DVPrintPreviewDialog != null))
                //{
                //    DVPrintPreviewDialog = new PrintPreviewDialog();
                //    ((Form)DVPrintPreviewDialog).WindowState = FormWindowState.Maximized;
                //}

                //DVPrintPreviewDialog.Document = DVPrintDocument;
                //DVPrintPreviewDialog.Show();

                DVPrintDocument.Print();
                DVPrintDocument.Dispose();

                //if ((DVPrintPreviewDialog2 != null))
                //{
                //    DVPrintPreviewDialog2 = new PrintPreviewDialog();
                //    ((Form)DVPrintPreviewDialog2).WindowState = FormWindowState.Maximized;
                //}

                //DVPrintPreviewDialog2.Document = DVPrintDocument2;
                //DVPrintPreviewDialog2.Show();

                DVPrintDocument2.Print();
                DVPrintDocument2.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

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

        private void dgv1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int qty;
                int rate;
                int amount;

                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dgv1.Rows[e.RowIndex];

                    ActualAmount.Text = "0";
                    TotalQty.Text = "0";
                    totalQuantity = 0; actualAmount = 0;

                    this.dgv1.EditMode = DataGridViewEditMode.EditOnEnter;

                    qty = Convert.ToInt32(row.Cells[5].Value);
                    rate = Convert.ToInt32(row.Cells[6].Value);
                    amount = qty * rate;
                    row.Cells[7].Value = amount;
                }

                for (int i = 0; i < dgv1.Rows.Count; ++i)
                {
                    totalQuantity += Convert.ToInt32(dgv1.Rows[i].Cells[5].Value);
                    actualAmount += Convert.ToDouble(dgv1.Rows[i].Cells[7].Value);
                }

                txtDiscount.Visible = true;
                ActualAmount.Text = actualAmount.ToString();
                TotalQty.Text = totalQuantity.ToString();
                TotalAmount.Text = actualAmount.ToString();
                txtDiscount.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }

        private void BtnDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                decimal discountAmountinPKR = Convert.ToInt32(txtDiscount.Text);
                decimal labelActualAmount = Convert.ToInt32(TotalAmount.Text);
                decimal ActualAmount = labelActualAmount - discountAmountinPKR;
                TotalAmount.Text = ActualAmount.ToString();
                discountpercentageForAmount = Convert.ToDouble(discountAmountinPKR / labelActualAmount * 100);
                txtDiscount.Text = string.Empty;
                txtDiscount.Visible = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter discount in Numbers"
                    + "\nHint 100 , 200 , 300 etc");
                txtDiscount.Text = string.Empty;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgv1.Refresh();
            LoadGridView();

            totalQuantity = 0;
            actualAmount = 0;
            invoiceid2 = 0;

            ActualAmount.Text = "0";
            TotalQty.Text = "0";
            TotalAmount.Text = "0";
            txtDiscount.Text = string.Empty;
            txtDiscount.Visible = false;
            InvoiceNumber.Text = string.Empty;
            InvoiceNumber.Focus();
        }

        private void SearchInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                ActualAmount.Text = "0";
                TotalQty.Text = "0";
                TotalAmount.Text = "0";

                int invoiceid = Convert.ToInt32(InvoiceNumber.Text);
                invoiceid2 = Convert.ToInt32(InvoiceNumber.Text);

                SqlConnection con = new SqlConnection(Helper.con);
                con.Open();
                string query = "select * from Bill where InvioceID  = '" + invoiceid + "' ";
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgv1.DataSource = dt;

                dgv1.Columns[0].HeaderText = "INVOICE ID";
                dgv1.Columns[1].HeaderText = "CUST ID";
                dgv1.Columns[2].HeaderText = "ORDER ID";
                dgv1.Columns[3].HeaderText = "CUST NAME";
                dgv1.Columns[4].HeaderText = "PRODUCT NAME";
                dgv1.Columns[5].HeaderText = "PRODUCT QTY";
                dgv1.Columns[6].HeaderText = "PRODUCT RATE";
                dgv1.Columns[7].HeaderText = "PRODUCT AMOUNT";
                dgv1.Columns[8].HeaderText = "GST AMOUNT";
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

                SqlTransaction tran = con.BeginTransaction();

                SqlCommand cmd2 = new SqlCommand("select top 1 ActualAmount,TotalAmount,DiscountInPercent from Bill where InvioceID = '" + InvoiceNumber.Text + "' ", con, tran);
                cmd2.ExecuteNonQuery();

                InvoiceNumber.Text = "";
                InvoiceNumber.Focus();

                totalQuantity = 0;

                using (SqlDataReader dr = cmd2.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        totalAmount = dr["TotalAmount"].ToString();
                        discount = dr["DiscountInPercent"].ToString();
                        DRactualAmount = dr["ActualAmount"].ToString();
                    }
                }

                TotalAmount.Text = totalAmount;
                txtDiscount.Text = discount;
                ActualAmount.Text = DRactualAmount.ToString();

                for (int i = 0; i < dgv1.Rows.Count; ++i)
                {
                    totalQuantity += Convert.ToInt32(dgv1.Rows[i].Cells[5].Value);
                }

                TotalQty.Text = totalQuantity.ToString();

                con.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("InvoiceID cannot be blank");
            }
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgv1.Rows[e.RowIndex];

                InvoiceNumber.Text = row.Cells[0].Value.ToString();
                CUSTID = row.Cells[1].Value.ToString(); ;
                CUSTNAME = row.Cells[3].Value.ToString(); ;
                ORDERID = row.Cells[2].Value.ToString(); ;
            }
        }

        private void DVPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            SqlConnection con = new SqlConnection(Helper.con);
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            SqlCommand cmd1 = new SqlCommand("select top 1 InvioceID2 from EditedBill order by InvioceID2 DESC", con, tran);
            cmd1.ExecuteNonQuery();

            using (SqlDataReader dr = cmd1.ExecuteReader())
            {
                while (dr.Read())
                {
                    //INVOICEID_1 = Convert.ToString(dr["InvioceID2"]);
                    INVOICEID_1 = Convert.ToString(dgv1.Rows[0].Cells[0].Value);

                    SqlCommand cmd2 = new SqlCommand("select InvioceID,Totalqty," +
                "TotalAmount,TotalAmountWithGST,ActualAmount," +
                "DiscountInPercent from Bill where InvioceID = '" + INVOICEID_1 + "' ", con, tran);
                    cmd2.ExecuteNonQuery();

                    using (SqlDataReader dr2 = cmd2.ExecuteReader())
                    {
                        while (dr2.Read())
                        {
                            INVOICEID_2 = Convert.ToString(dr2["InvioceID"]);
                            Total_Qty = Convert.ToString(dr2["Totalqty"]);
                            Total_Amount = Convert.ToString(dr2["TotalAmount"]);
                            _TotalWithGST = Convert.ToString(dr2["TotalAmountWithGST"]);
                            Actual_Amount = Convert.ToString(dr2["ActualAmount"]);
                            _Discount = Convert.ToString(dr2["DiscountInPercent"]);
                        }
                    }
                }
            }

            tran.Commit();
            con.Close();

            e.Graphics.DrawString("FOODIES CLUB", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, new Point(60, 30));
            //e.Graphics.DrawString("GST# 222-333-123456", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(130, 70));
            e.Graphics.DrawString("Invoice ID :", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(10, 70));
            e.Graphics.DrawString(INVOICEID_2.ToString(), new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(90, 70));
            e.Graphics.DrawString("Transaction Date :", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(10, 90));
            e.Graphics.DrawString(DateTime.Now.Date.ToShortDateString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(130, 90));
            e.Graphics.DrawString("Transaction Time :", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(10, 110));
            e.Graphics.DrawString(DateTime.Now.ToShortTimeString(), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(130, 110));
            //
            e.Graphics.DrawString("------------------------------------------------------",
               new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 130));
            e.Graphics.DrawString("SALES ITEMS", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(80, 150));
            e.Graphics.DrawString("------------------------------------------------------",
            new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, 170));
            //
            e.Graphics.DrawString("PRODUCT NAME", new Font("Arial", 7, FontStyle.Bold), Brushes.Black, new Point(5, 190));
            e.Graphics.DrawString("QUANTITY", new Font("Arial", 7, FontStyle.Bold), Brushes.Black, new Point(120, 190));
            e.Graphics.DrawString("RATE", new Font("Arial", 7, FontStyle.Bold), Brushes.Black, new Point(180, 190));
            e.Graphics.DrawString("AMOUNT", new Font("Arial", 7, FontStyle.Bold), Brushes.Black, new Point(220, 190));

            int position = 210;

            for (int i = 0; i < dgv1.Rows.Count; i++)
            {
                position = position + 20;
                e.Graphics.DrawString(Convert.ToString(dgv1.Rows[i].Cells[4].Value), new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(5, position));
                e.Graphics.DrawString(Convert.ToString(dgv1.Rows[i].Cells[5].Value) + ".00", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(135, position));
                e.Graphics.DrawString(Convert.ToString(dgv1.Rows[i].Cells[6].Value) + ".00", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(175, position));
                e.Graphics.DrawString(Convert.ToString(dgv1.Rows[i].Cells[7].Value) + ".00", new Font("Arial", 8, FontStyle.Regular), Brushes.Black, new Point(225, position));
            }
            //
            e.Graphics.DrawString("------------------------------------------------------",
           new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, position + 20));
            //
            e.Graphics.DrawString("Actual Amount", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(10, position + 40));
            e.Graphics.DrawString(Actual_Amount.ToString() + ".00", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(190, position + 40));
            e.Graphics.DrawString("Total Quantity", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(10, position + 60));
            e.Graphics.DrawString(Total_Qty.ToString() + ".00", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(190, position + 60));
            e.Graphics.DrawString("Total Amount", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(10, position + 80));
            e.Graphics.DrawString(Total_Amount.ToString() + ".00", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(190, position + 80));
            e.Graphics.DrawString("Discount", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(10, position + 100));
            e.Graphics.DrawString(discountpercentageForAmount.ToString() + " %", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, new Point(190, position + 100));
            //
            e.Graphics.DrawString("------------------------------------------------------",
           new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(10, position + 120));
            //
            e.Graphics.DrawString("TOKEN NO " + INVOICEID_2.ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new Point(10, position + 140));
        }

        double discountpercentageForAmount = 0;

        public EditInvoice()
        {
            InitializeComponent();
            InvoiceNumber.Focus();
        }

        private void EditInvoice_Load(object sender, EventArgs e)
        {
            dgv_1();
            LoadGridView();
            ActualAmount.Text = "0";
            TotalQty.Text = "0";
            TotalAmount.Text = "0";
            txtDiscount.Visible = false;
        }

        public void LoadGridView()
        {
            dgv1.Refresh();
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

            Padding newPadding2 = new Padding(10, 0, 10, 0);
            dgv1.RowsDefaultCellStyle.Padding = newPadding2;

            //this Line of Code made the dgv1 Text Middle Center
            dgv1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

    }
}
