namespace Oct2026.WindowForm
{
    partial class FrmStudent
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSave = new Button();
            label1 = new Label();
            txtProductCode = new TextBox();
            txtName = new TextBox();
            txtBrand = new TextBox();
            label3 = new Label();
            label4 = new Label();
            btnClear = new Button();
            btnCancel = new Button();
            txtGender = new TextBox();
            label2 = new Label();
            txtColor = new TextBox();
            label5 = new Label();
            txtPrice = new TextBox();
            label6 = new Label();
            txtStockQty = new TextBox();
            label7 = new Label();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Location = new Point(273, 481);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 0;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(190, 51);
            label1.Name = "label1";
            label1.Size = new Size(121, 25);
            label1.TabIndex = 1;
            label1.Text = "Product Code";
            // 
            // txtProductCode
            // 
            txtProductCode.Location = new Point(333, 51);
            txtProductCode.Name = "txtProductCode";
            txtProductCode.Size = new Size(150, 31);
            txtProductCode.TabIndex = 2;
            // 
            // txtName
            // 
            txtName.Location = new Point(333, 113);
            txtName.Name = "txtName";
            txtName.Size = new Size(150, 31);
            txtName.TabIndex = 4;
            // 
            // txtBrand
            // 
            txtBrand.Location = new Point(333, 175);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(150, 31);
            txtBrand.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(190, 181);
            label3.Name = "label3";
            label3.Size = new Size(58, 25);
            label3.TabIndex = 5;
            label3.Text = "Brand";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(190, 119);
            label4.Name = "label4";
            label4.Size = new Size(59, 25);
            label4.TabIndex = 7;
            label4.Text = "Name";
            // 
            // btnClear
            // 
            btnClear.Location = new Point(155, 481);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(112, 34);
            btnClear.TabIndex = 8;
            btnClear.Text = "&Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(391, 481);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(112, 34);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // txtGender
            // 
            txtGender.Location = new Point(333, 235);
            txtGender.Name = "txtGender";
            txtGender.Size = new Size(150, 31);
            txtGender.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(190, 238);
            label2.Name = "label2";
            label2.Size = new Size(69, 25);
            label2.TabIndex = 10;
            label2.Text = "Gender";
            // 
            // txtColor
            // 
            txtColor.Location = new Point(333, 291);
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(150, 31);
            txtColor.TabIndex = 13;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(186, 297);
            label5.Name = "label5";
            label5.Size = new Size(55, 25);
            label5.TabIndex = 12;
            label5.Text = "Color";
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(333, 353);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(150, 31);
            txtPrice.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(190, 356);
            label6.Name = "label6";
            label6.Size = new Size(49, 25);
            label6.TabIndex = 14;
            label6.Text = "Price";
            // 
            // txtStockQty
            // 
            txtStockQty.Location = new Point(333, 415);
            txtStockQty.Name = "txtStockQty";
            txtStockQty.Size = new Size(150, 31);
            txtStockQty.TabIndex = 17;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(190, 415);
            label7.Name = "label7";
            label7.Size = new Size(128, 25);
            label7.TabIndex = 16;
            label7.Text = "Stock Quantity";
            // 
            // FrmStudent
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(863, 527);
            Controls.Add(txtStockQty);
            Controls.Add(label7);
            Controls.Add(txtPrice);
            Controls.Add(label6);
            Controls.Add(txtColor);
            Controls.Add(label5);
            Controls.Add(txtGender);
            Controls.Add(label2);
            Controls.Add(btnCancel);
            Controls.Add(btnClear);
            Controls.Add(label4);
            Controls.Add(txtBrand);
            Controls.Add(label3);
            Controls.Add(txtName);
            Controls.Add(txtProductCode);
            Controls.Add(label1);
            Controls.Add(btnSave);
            Name = "FrmStudent";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Student Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSave;
        private Label label1;
        private TextBox txtProductCode;
        private TextBox txtName;
        private TextBox txtBrand;
        private Label label3;
        private Label label4;
        private Button btnClear;
        private Button btnCancel;
        private TextBox txtGender;
        private Label label2;
        private TextBox txtColor;
        private Label label5;
        private TextBox txtPrice;
        private Label label6;
        private TextBox txtStockQty;
        private Label label7;
    }
}
