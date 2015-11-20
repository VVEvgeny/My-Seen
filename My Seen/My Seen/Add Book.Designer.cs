namespace My_Seen
{
    partial class Add_Book
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Add_Book));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.errorProvider.SetError(this.textBox1, resources.GetString("textBox1.Error"));
            this.errorProvider.SetIconAlignment(this.textBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("textBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.textBox1, ((int)(resources.GetObject("textBox1.IconPadding"))));
            this.textBox1.Name = "textBox1";
            // 
            // dateTimePicker1
            // 
            resources.ApplyResources(this.dateTimePicker1, "dateTimePicker1");
            this.errorProvider.SetError(this.dateTimePicker1, resources.GetString("dateTimePicker1.Error"));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.errorProvider.SetIconAlignment(this.dateTimePicker1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("dateTimePicker1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.dateTimePicker1, ((int)(resources.GetObject("dateTimePicker1.IconPadding"))));
            this.dateTimePicker1.Name = "dateTimePicker1";
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.comboBox1, resources.GetString("comboBox1.Error"));
            this.comboBox1.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.comboBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comboBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.comboBox1, ((int)(resources.GetObject("comboBox1.IconPadding"))));
            this.comboBox1.Name = "comboBox1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.errorProvider.SetError(this.textBox2, resources.GetString("textBox2.Error"));
            this.errorProvider.SetIconAlignment(this.textBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("textBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.textBox2, ((int)(resources.GetObject("textBox2.IconPadding"))));
            this.textBox2.Name = "textBox2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.errorProvider.SetError(this.button1, resources.GetString("button1.Error"));
            this.errorProvider.SetIconAlignment(this.button1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("button1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.button1, ((int)(resources.GetObject("button1.IconPadding"))));
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // comboBox2
            // 
            resources.ApplyResources(this.comboBox2, "comboBox2");
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.comboBox2, resources.GetString("comboBox2.Error"));
            this.comboBox2.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.comboBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comboBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.comboBox2, ((int)(resources.GetObject("comboBox2.IconPadding"))));
            this.comboBox2.Name = "comboBox2";
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.errorProvider.SetError(this.button2, resources.GetString("button2.Error"));
            this.errorProvider.SetIconAlignment(this.button2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("button2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.button2, ((int)(resources.GetObject("button2.IconPadding"))));
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Add_Book
            // 
            this.AcceptButton = this.button1;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Add_Book";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.Add_Book_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button2;
    }
}