namespace SmartGridPlugin
{
    partial class SmartPluginConfigurador
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGrid = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbPlataforma = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCamara = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numOvershootHorizontal = new System.Windows.Forms.NumericUpDown();
            this.numOvershootVertical = new System.Windows.Forms.NumericUpDown();
            this.numOverlap = new System.Windows.Forms.NumericUpDown();
            this.numSidelap = new System.Windows.Forms.NumericUpDown();
            this.numAltura = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.numAngulo = new System.Windows.Forms.NumericUpDown();
            this.tabDivisionAreas = new System.Windows.Forms.TabPage();
            this.numFranjas = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.lblFranjas = new System.Windows.Forms.Label();
            this.numDespMaximo = new System.Windows.Forms.NumericUpDown();
            this.numAreaMaxima = new System.Windows.Forms.NumericUpDown();
            this.cmbTipoDivision = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.lblArea = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabGrid.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOvershootHorizontal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOvershootVertical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOverlap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSidelap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAltura)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngulo)).BeginInit();
            this.tabDivisionAreas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFranjas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDespMaximo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAreaMaxima)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1MinSize = 85;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblArea);
            this.splitContainer1.Panel2.Controls.Add(this.label14);
            this.splitContainer1.Panel2.Controls.Add(this.btnAceptar);
            this.splitContainer1.Panel2MinSize = 5;
            this.splitContainer1.Size = new System.Drawing.Size(844, 624);
            this.splitContainer1.SplitterDistance = 463;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGrid);
            this.tabControl1.Controls.Add(this.tabDivisionAreas);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(844, 463);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGrid
            // 
            this.tabGrid.BackColor = System.Drawing.Color.DarkGray;
            this.tabGrid.Controls.Add(this.tableLayoutPanel1);
            this.tabGrid.Location = new System.Drawing.Point(4, 30);
            this.tabGrid.Margin = new System.Windows.Forms.Padding(4);
            this.tabGrid.Name = "tabGrid";
            this.tabGrid.Padding = new System.Windows.Forms.Padding(4);
            this.tabGrid.Size = new System.Drawing.Size(836, 429);
            this.tabGrid.TabIndex = 0;
            this.tabGrid.Text = "Grid";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.00363F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.1971F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.49456F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.30471F));
            this.tableLayoutPanel1.Controls.Add(this.cmbPlataforma, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbCamara, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label7, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.numOvershootHorizontal, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.numOvershootVertical, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.numOverlap, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.numSidelap, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.numAltura, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label12, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.numAngulo, 2, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(828, 421);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cmbPlataforma
            // 
            this.cmbPlataforma.FormattingEnabled = true;
            this.cmbPlataforma.Items.AddRange(new object[] {
            "Hemav-6",
            "Hemav-8",
            "Hemav Wings",
            "Hemav Pro"});
            this.cmbPlataforma.Location = new System.Drawing.Point(352, 53);
            this.cmbPlataforma.Name = "cmbPlataforma";
            this.cmbPlataforma.Size = new System.Drawing.Size(271, 29);
            this.cmbPlataforma.TabIndex = 2;
            this.cmbPlataforma.Text = "Selecciona la plataforma...";
            this.cmbPlataforma.SelectedIndexChanged += new System.EventHandler(this.cmbPlataforma_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(167, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 45);
            this.label1.TabIndex = 3;
            this.label1.Text = "Plataforma";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 45);
            this.label2.TabIndex = 4;
            this.label2.Text = "Cámara";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 45);
            this.label3.TabIndex = 5;
            this.label3.Text = "Altitud [m]";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(107, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(226, 45);
            this.label4.TabIndex = 6;
            this.label4.Text = "Overshoot Horizontal [m]";
            // 
            // cmbCamara
            // 
            this.cmbCamara.FormattingEnabled = true;
            this.cmbCamara.Items.AddRange(new object[] {
            "GoPro 3+",
            "Panasonic Lumix GH3",
            "Sony NEX 7"});
            this.cmbCamara.Location = new System.Drawing.Point(352, 98);
            this.cmbCamara.Name = "cmbCamara";
            this.cmbCamara.Size = new System.Drawing.Size(271, 29);
            this.cmbCamara.TabIndex = 7;
            this.cmbCamara.Text = "Selecciona la cámara...";
            this.cmbCamara.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(119, 275);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 45);
            this.label5.TabIndex = 10;
            this.label5.Text = "Overshoot Vertical [m]";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(164, 320);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 45);
            this.label6.TabIndex = 11;
            this.label6.Text = "Overlap [%]";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(168, 365);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 45);
            this.label7.TabIndex = 12;
            this.label7.Text = "Sidelap [%]";
            // 
            // numOvershootHorizontal
            // 
            this.numOvershootHorizontal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numOvershootHorizontal.Location = new System.Drawing.Point(513, 238);
            this.numOvershootHorizontal.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numOvershootHorizontal.Name = "numOvershootHorizontal";
            this.numOvershootHorizontal.Size = new System.Drawing.Size(110, 28);
            this.numOvershootHorizontal.TabIndex = 14;
            this.numOvershootHorizontal.ValueChanged += new System.EventHandler(this.numOvershootHorizontal_ValueChanged);
            // 
            // numOvershootVertical
            // 
            this.numOvershootVertical.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numOvershootVertical.Location = new System.Drawing.Point(513, 283);
            this.numOvershootVertical.Name = "numOvershootVertical";
            this.numOvershootVertical.Size = new System.Drawing.Size(110, 28);
            this.numOvershootVertical.TabIndex = 15;
            this.numOvershootVertical.ValueChanged += new System.EventHandler(this.numOvershootVertical_ValueChanged);
            // 
            // numOverlap
            // 
            this.numOverlap.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numOverlap.DecimalPlaces = 2;
            this.numOverlap.Increment = new decimal(new int[] {
            500,
            0,
            0,
            196608});
            this.numOverlap.Location = new System.Drawing.Point(513, 328);
            this.numOverlap.Name = "numOverlap";
            this.numOverlap.Size = new System.Drawing.Size(110, 28);
            this.numOverlap.TabIndex = 16;
            this.numOverlap.Value = new decimal(new int[] {
            5000,
            0,
            0,
            131072});
            this.numOverlap.ValueChanged += new System.EventHandler(this.numOverlap_ValueChanged);
            // 
            // numSidelap
            // 
            this.numSidelap.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numSidelap.DecimalPlaces = 2;
            this.numSidelap.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numSidelap.Location = new System.Drawing.Point(513, 373);
            this.numSidelap.Name = "numSidelap";
            this.numSidelap.Size = new System.Drawing.Size(110, 28);
            this.numSidelap.TabIndex = 17;
            this.numSidelap.Value = new decimal(new int[] {
            6000,
            0,
            0,
            131072});
            this.numSidelap.ValueChanged += new System.EventHandler(this.numSidelap_ValueChanged);
            // 
            // numAltura
            // 
            this.numAltura.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numAltura.Location = new System.Drawing.Point(513, 193);
            this.numAltura.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numAltura.Name = "numAltura";
            this.numAltura.Size = new System.Drawing.Size(110, 28);
            this.numAltura.TabIndex = 13;
            this.numAltura.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numAltura.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(172, 140);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(96, 45);
            this.label12.TabIndex = 18;
            this.label12.Text = "Ángulo [º]";
            // 
            // numAngulo
            // 
            this.numAngulo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numAngulo.Location = new System.Drawing.Point(513, 148);
            this.numAngulo.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.numAngulo.Name = "numAngulo";
            this.numAngulo.Size = new System.Drawing.Size(110, 28);
            this.numAngulo.TabIndex = 19;
            // 
            // tabDivisionAreas
            // 
            this.tabDivisionAreas.BackColor = System.Drawing.Color.DarkGray;
            this.tabDivisionAreas.Controls.Add(this.numFranjas);
            this.tabDivisionAreas.Controls.Add(this.label13);
            this.tabDivisionAreas.Controls.Add(this.lblFranjas);
            this.tabDivisionAreas.Controls.Add(this.numDespMaximo);
            this.tabDivisionAreas.Controls.Add(this.numAreaMaxima);
            this.tabDivisionAreas.Controls.Add(this.cmbTipoDivision);
            this.tabDivisionAreas.Controls.Add(this.label8);
            this.tabDivisionAreas.Controls.Add(this.label9);
            this.tabDivisionAreas.Controls.Add(this.label10);
            this.tabDivisionAreas.Controls.Add(this.label11);
            this.tabDivisionAreas.Location = new System.Drawing.Point(4, 30);
            this.tabDivisionAreas.Margin = new System.Windows.Forms.Padding(4);
            this.tabDivisionAreas.Name = "tabDivisionAreas";
            this.tabDivisionAreas.Padding = new System.Windows.Forms.Padding(4);
            this.tabDivisionAreas.Size = new System.Drawing.Size(836, 429);
            this.tabDivisionAreas.TabIndex = 1;
            this.tabDivisionAreas.Text = "Division de áreas";
            // 
            // numFranjas
            // 
            this.numFranjas.Location = new System.Drawing.Point(484, 316);
            this.numFranjas.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numFranjas.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numFranjas.Name = "numFranjas";
            this.numFranjas.Size = new System.Drawing.Size(64, 28);
            this.numFranjas.TabIndex = 19;
            this.numFranjas.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numFranjas.ValueChanged += new System.EventHandler(this.numFranjas_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Ubuntu Condensed", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(44, 263);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(138, 23);
            this.label13.TabIndex = 18;
            this.label13.Text = "Tipo de división";
            // 
            // lblFranjas
            // 
            this.lblFranjas.AutoSize = true;
            this.lblFranjas.Font = new System.Drawing.Font("Ubuntu Condensed", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFranjas.Location = new System.Drawing.Point(377, 321);
            this.lblFranjas.Name = "lblFranjas";
            this.lblFranjas.Size = new System.Drawing.Size(67, 23);
            this.lblFranjas.TabIndex = 17;
            this.lblFranjas.Text = "Franjas";
            // 
            // numDespMaximo
            // 
            this.numDespMaximo.DecimalPlaces = 1;
            this.numDespMaximo.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numDespMaximo.Location = new System.Drawing.Point(484, 197);
            this.numDespMaximo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numDespMaximo.Name = "numDespMaximo";
            this.numDespMaximo.Size = new System.Drawing.Size(64, 28);
            this.numDespMaximo.TabIndex = 16;
            this.numDespMaximo.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numDespMaximo.ValueChanged += new System.EventHandler(this.numDespMaximo_ValueChanged);
            // 
            // numAreaMaxima
            // 
            this.numAreaMaxima.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numAreaMaxima.Location = new System.Drawing.Point(484, 145);
            this.numAreaMaxima.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numAreaMaxima.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numAreaMaxima.Name = "numAreaMaxima";
            this.numAreaMaxima.Size = new System.Drawing.Size(64, 28);
            this.numAreaMaxima.TabIndex = 15;
            this.numAreaMaxima.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numAreaMaxima.ValueChanged += new System.EventHandler(this.numAreaMaxima_ValueChanged);
            // 
            // cmbTipoDivision
            // 
            this.cmbTipoDivision.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.cmbTipoDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoDivision.FormattingEnabled = true;
            this.cmbTipoDivision.Items.AddRange(new object[] {
            "Divisiones verticales",
            "Divisiones horizontales",
            "Division con seleccion de franjas horizontales",
            "Division con seleccion de franjas verticales"});
            this.cmbTipoDivision.Location = new System.Drawing.Point(209, 260);
            this.cmbTipoDivision.Name = "cmbTipoDivision";
            this.cmbTipoDivision.Size = new System.Drawing.Size(338, 29);
            this.cmbTipoDivision.TabIndex = 14;
            this.cmbTipoDivision.SelectedIndexChanged += new System.EventHandler(this.cmbTipoDivision_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Ubuntu Condensed", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(554, 199);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 23);
            this.label8.TabIndex = 13;
            this.label8.Text = "m";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Ubuntu Condensed", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(554, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 23);
            this.label9.TabIndex = 12;
            this.label9.Text = "Ha";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Ubuntu Condensed", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(252, 199);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(213, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "Distancia desplazamiento";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Ubuntu Condensed", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(349, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 23);
            this.label11.TabIndex = 8;
            this.label11.Text = "Área máxima";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(670, 58);
            this.btnAceptar.Margin = new System.Windows.Forms.Padding(4);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(115, 50);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(25, 58);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 24);
            this.label14.TabIndex = 1;
            this.label14.Text = "Área:";
            // 
            // lblArea
            // 
            this.lblArea.AutoSize = true;
            this.lblArea.Location = new System.Drawing.Point(87, 58);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(17, 24);
            this.lblArea.TabIndex = 2;
            this.lblArea.Text = "-";
            // 
            // SmartPluginConfigurador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(844, 624);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Ubuntu", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SmartPluginConfigurador";
            this.Text = "Configuración de SmartGrid";
            this.Load += new System.EventHandler(this.SmartPluginConfigurador_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabGrid.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOvershootHorizontal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOvershootVertical)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOverlap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSidelap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAltura)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAngulo)).EndInit();
            this.tabDivisionAreas.ResumeLayout(false);
            this.tabDivisionAreas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFranjas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDespMaximo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAreaMaxima)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGrid;
        private System.Windows.Forms.TabPage tabDivisionAreas;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cmbPlataforma;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCamara;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numOvershootHorizontal;
        private System.Windows.Forms.NumericUpDown numOvershootVertical;
        private System.Windows.Forms.NumericUpDown numOverlap;
        private System.Windows.Forms.NumericUpDown numSidelap;
        private System.Windows.Forms.NumericUpDown numAltura;
        private System.Windows.Forms.NumericUpDown numDespMaximo;
        private System.Windows.Forms.NumericUpDown numAreaMaxima;
        private System.Windows.Forms.ComboBox cmbTipoDivision;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numFranjas;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblFranjas;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown numAngulo;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.Label label14;

    }
}