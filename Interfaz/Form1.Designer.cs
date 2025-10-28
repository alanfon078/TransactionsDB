namespace TransactionsDB
{
    partial class Form1
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
            lblCodigo = new Label();
            txtCodigoBarras = new TextBox();
            dgvProductos = new DataGridView();
            btnDescontinuar = new Button();
            btnBuscar = new Button();
            btnAgregar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // lblCodigo
            // 
            lblCodigo.AutoSize = true;
            lblCodigo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCodigo.Location = new Point(34, 30);
            lblCodigo.Name = "lblCodigo";
            lblCodigo.Size = new Size(132, 20);
            lblCodigo.TabIndex = 0;
            lblCodigo.Text = "Código de Barras:";
            // 
            // txtCodigoBarras
            // 
            txtCodigoBarras.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCodigoBarras.Location = new Point(172, 23);
            txtCodigoBarras.Name = "txtCodigoBarras";
            txtCodigoBarras.Size = new Size(494, 27);
            txtCodigoBarras.TabIndex = 1;
            txtCodigoBarras.KeyPress += txtCodigoBarras_KeyPress;
            // 
            // dgvProductos
            // 
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(34, 76);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.Size = new Size(734, 334);
            dgvProductos.TabIndex = 2;
            // 
            // btnDescontinuar
            // 
            btnDescontinuar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDescontinuar.BackColor = Color.IndianRed;
            btnDescontinuar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDescontinuar.ForeColor = Color.White;
            btnDescontinuar.Location = new Point(603, 429);
            btnDescontinuar.Name = "btnDescontinuar";
            btnDescontinuar.Size = new Size(165, 43);
            btnDescontinuar.TabIndex = 3;
            btnDescontinuar.Text = "DESCONTINUAR";
            btnDescontinuar.UseVisualStyleBackColor = false;
            btnDescontinuar.Click += btnDescontinuar_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.Font = new Font("Segoe UI", 9F);
            btnBuscar.Location = new Point(672, 26);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(96, 29);
            btnBuscar.TabIndex = 4;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnAgregar
            // 
            btnAgregar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAgregar.BackColor = Color.DeepSkyBlue;
            btnAgregar.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            btnAgregar.ForeColor = Color.White;
            btnAgregar.Location = new Point(432, 429);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(165, 43);
            btnAgregar.TabIndex = 5;
            btnAgregar.Text = "Agregar Nuevo Producto";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += this.btnAgregar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 495);
            Controls.Add(btnAgregar);
            Controls.Add(btnDescontinuar);
            Controls.Add(dgvProductos);
            Controls.Add(txtCodigoBarras);
            Controls.Add(lblCodigo);
            Controls.Add(btnBuscar);
            Name = "Form1";
            Text = "Gestión de Productos";
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCodigo;
        private TextBox txtCodigoBarras;
        private DataGridView dgvProductos;
        private Button btnDescontinuar;
        private Button btnBuscar;
        private Button btnAgregar;
    }
}