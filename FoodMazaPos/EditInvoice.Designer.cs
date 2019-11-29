namespace FoodMazaPos
{
    partial class EditInvoice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditInvoice));
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TotalQty = new System.Windows.Forms.Label();
            this.ActualAmount = new System.Windows.Forms.Label();
            this.TotalAmount = new System.Windows.Forms.Label();
            this.BtnDiscount = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.txtDiscount = new System.Windows.Forms.TextBox();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.InvoiceNumber = new System.Windows.Forms.TextBox();
            this.SearchInvoice = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.DVPrintDocument = new System.Drawing.Printing.PrintDocument();
            this.DVPrintPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.DVPrintPreviewDialog2 = new System.Windows.Forms.PrintPreviewDialog();
            this.DVPrintDocument2 = new System.Drawing.Printing.PrintDocument();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(625, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(305, 41);
            this.label3.TabIndex = 2;
            this.label3.Text = "TOTAL AMOUNT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(314, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 41);
            this.label2.TabIndex = 1;
            this.label2.Text = "ACTUAL AMOUNT";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "TOTAL QUANTITY";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalQty
            // 
            this.TotalQty.AutoSize = true;
            this.TotalQty.Dock = System.Windows.Forms.DockStyle.Top;
            this.TotalQty.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalQty.Location = new System.Drawing.Point(0, 56);
            this.TotalQty.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.TotalQty.Name = "TotalQty";
            this.TotalQty.Size = new System.Drawing.Size(311, 19);
            this.TotalQty.TabIndex = 3;
            this.TotalQty.Text = "0";
            this.TotalQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ActualAmount
            // 
            this.ActualAmount.AutoSize = true;
            this.ActualAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.ActualAmount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActualAmount.Location = new System.Drawing.Point(311, 56);
            this.ActualAmount.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.ActualAmount.Name = "ActualAmount";
            this.ActualAmount.Size = new System.Drawing.Size(311, 19);
            this.ActualAmount.TabIndex = 4;
            this.ActualAmount.Text = "0";
            this.ActualAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TotalAmount
            // 
            this.TotalAmount.AutoSize = true;
            this.TotalAmount.Dock = System.Windows.Forms.DockStyle.Top;
            this.TotalAmount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalAmount.Location = new System.Drawing.Point(622, 56);
            this.TotalAmount.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.TotalAmount.Name = "TotalAmount";
            this.TotalAmount.Size = new System.Drawing.Size(311, 19);
            this.TotalAmount.TabIndex = 5;
            this.TotalAmount.Text = "0";
            this.TotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnDiscount
            // 
            this.BtnDiscount.BackColor = System.Drawing.Color.RoyalBlue;
            this.BtnDiscount.FlatAppearance.BorderSize = 0;
            this.BtnDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDiscount.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDiscount.ForeColor = System.Drawing.Color.White;
            this.BtnDiscount.Location = new System.Drawing.Point(943, 46);
            this.BtnDiscount.Margin = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.BtnDiscount.Name = "BtnDiscount";
            this.BtnDiscount.Size = new System.Drawing.Size(213, 45);
            this.BtnDiscount.TabIndex = 7;
            this.BtnDiscount.Text = "DISCOUNT";
            this.BtnDiscount.UseVisualStyleBackColor = false;
            this.BtnDiscount.Click += new System.EventHandler(this.BtnDiscount_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(220, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 41);
            this.label4.TabIndex = 8;
            this.label4.Text = "PKR";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnPrint.BackColor = System.Drawing.Color.RoyalBlue;
            this.BtnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPrint.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrint.ForeColor = System.Drawing.Color.White;
            this.BtnPrint.Location = new System.Drawing.Point(1247, 11);
            this.BtnPrint.Margin = new System.Windows.Forms.Padding(0, 0, 30, 30);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(227, 53);
            this.BtnPrint.TabIndex = 4;
            this.BtnPrint.Text = "PRINT EDITED INVOICE";
            this.BtnPrint.UseVisualStyleBackColor = false;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // txtDiscount
            // 
            this.txtDiscount.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscount.ForeColor = System.Drawing.Color.Black;
            this.txtDiscount.Location = new System.Drawing.Point(10, 5);
            this.txtDiscount.Margin = new System.Windows.Forms.Padding(10, 5, 10, 0);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.Size = new System.Drawing.Size(197, 35);
            this.txtDiscount.TabIndex = 7;
            this.txtDiscount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dgv1
            // 
            this.dgv1.AllowUserToAddRows = false;
            this.dgv1.AllowUserToResizeColumns = false;
            this.dgv1.AllowUserToResizeRows = false;
            this.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv1.BackgroundColor = System.Drawing.Color.White;
            this.dgv1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv1.GridColor = System.Drawing.Color.DarkGray;
            this.dgv1.Location = new System.Drawing.Point(10, 81);
            this.dgv1.Margin = new System.Windows.Forms.Padding(10);
            this.dgv1.Name = "dgv1";
            this.dgv1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv1.ShowCellErrors = false;
            this.dgv1.ShowCellToolTips = false;
            this.dgv1.ShowRowErrors = false;
            this.dgv1.Size = new System.Drawing.Size(1484, 544);
            this.dgv1.TabIndex = 2;
            this.dgv1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellClick);
            this.dgv1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellEndEdit);
            // 
            // InvoiceNumber
            // 
            this.InvoiceNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InvoiceNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InvoiceNumber.Location = new System.Drawing.Point(15, 22);
            this.InvoiceNumber.Margin = new System.Windows.Forms.Padding(15, 22, 0, 11);
            this.InvoiceNumber.Multiline = true;
            this.InvoiceNumber.Name = "InvoiceNumber";
            this.InvoiceNumber.Size = new System.Drawing.Size(1203, 38);
            this.InvoiceNumber.TabIndex = 0;
            this.InvoiceNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SearchInvoice
            // 
            this.SearchInvoice.BackColor = System.Drawing.Color.RoyalBlue;
            this.SearchInvoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SearchInvoice.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchInvoice.ForeColor = System.Drawing.Color.White;
            this.SearchInvoice.Location = new System.Drawing.Point(5, 20);
            this.SearchInvoice.Margin = new System.Windows.Forms.Padding(5, 20, 5, 10);
            this.SearchInvoice.Name = "SearchInvoice";
            this.SearchInvoice.Size = new System.Drawing.Size(202, 41);
            this.SearchInvoice.TabIndex = 2;
            this.SearchInvoice.Text = "SEARCH INVOICE";
            this.SearchInvoice.UseVisualStyleBackColor = false;
            this.SearchInvoice.Click += new System.EventHandler(this.SearchInvoice_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefresh.BackgroundImage")));
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(222, 20);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(54, 41);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.45255F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.54745F));
            this.tableLayoutPanel6.Controls.Add(this.SearchInvoice, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnRefresh, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(1218, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(286, 71);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.TotalQty, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.ActualAmount, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.TotalAmount, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.BtnDiscount, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.16667F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.83333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1244, 106);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.96997F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.03003F));
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtDiscount, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(933, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(311, 41);
            this.tableLayoutPanel5.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgv1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.666283F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.29459F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.1542F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1504, 741);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.04738F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.95262F));
            this.tableLayoutPanel2.Controls.Add(this.InvoiceNumber, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1504, 71);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.73067F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.26933F));
            this.tableLayoutPanel3.Controls.Add(this.BtnPrint, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 635);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1504, 106);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // DVPrintDocument
            // 
            this.DVPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.DVPrintDocument_PrintPage);
            // 
            // DVPrintPreviewDialog
            // 
            this.DVPrintPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.DVPrintPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.DVPrintPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.DVPrintPreviewDialog.Enabled = true;
            this.DVPrintPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("DVPrintPreviewDialog.Icon")));
            this.DVPrintPreviewDialog.Name = "DVPrintPreviewDialog";
            this.DVPrintPreviewDialog.Visible = false;
            // 
            // DVPrintPreviewDialog2
            // 
            this.DVPrintPreviewDialog2.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.DVPrintPreviewDialog2.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.DVPrintPreviewDialog2.ClientSize = new System.Drawing.Size(400, 300);
            this.DVPrintPreviewDialog2.Enabled = true;
            this.DVPrintPreviewDialog2.Icon = ((System.Drawing.Icon)(resources.GetObject("DVPrintPreviewDialog2.Icon")));
            this.DVPrintPreviewDialog2.Name = "DVPrintPreviewDialog2";
            this.DVPrintPreviewDialog2.Visible = false;
            // 
            // DVPrintDocument2
            // 
            this.DVPrintDocument2.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.DVPrintDocument2_PrintPage);
            // 
            // EditInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1504, 741);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditInvoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "EDIT INVOICE";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.EditInvoice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TotalQty;
        private System.Windows.Forms.Label ActualAmount;
        private System.Windows.Forms.Label TotalAmount;
        private System.Windows.Forms.Button BtnDiscount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.TextBox txtDiscount;
        protected System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.TextBox InvoiceNumber;
        private System.Windows.Forms.Button SearchInvoice;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Drawing.Printing.PrintDocument DVPrintDocument;
        private System.Windows.Forms.PrintPreviewDialog DVPrintPreviewDialog;
        private System.Windows.Forms.PrintPreviewDialog DVPrintPreviewDialog2;
        private System.Drawing.Printing.PrintDocument DVPrintDocument2;
    }
}