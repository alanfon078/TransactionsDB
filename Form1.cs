using System.Data;
using TransactionsDB.Clases;
using TransactionsDB.ConectionDB;

namespace TransactionsDB
{
    public partial class Form1 : Form
    {
        private readonly Conexion datosProducto;

        public Form1()
        {
            InitializeComponent();
            datosProducto = new Conexion();
            ConfigurarGrid();
        }

        /// <summary>
        /// Prepara el DataGridView con las columnas necesarias
        /// </summary>
        private void ConfigurarGrid()
        {
            dgvProductos.ColumnCount = 5;
            dgvProductos.Columns[0].Name = "ID";
            dgvProductos.Columns[1].Name = "CodigoBarras";
            dgvProductos.Columns[2].Name = "Nombre";
            dgvProductos.Columns[3].Name = "Precio";
            dgvProductos.Columns[4].Name = "Stock";

            dgvProductos.Columns["ID"].Visible = false;

            dgvProductos.ReadOnly = true;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Evento KeyPress para el TextBox del código de barras.
        /// Se activa al presionar "Enter".
        /// </summary>
        private void txtCodigoBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BuscarYAgregarProducto();
            }
        }

        private void BuscarYAgregarProducto()
        {
            string codigo = txtCodigoBarras.Text.Trim();
            if (string.IsNullOrEmpty(codigo))
            {
                return;
            }

            try
            {
                Producto producto = datosProducto.BuscarProductoPorCodigo(codigo);

                if (producto != null)
                {
                    dgvProductos.Rows.Add(
                        producto.Id,
                        producto.CodigoDeBarras,
                        producto.Nombre,
                        producto.Precio,
                        producto.Stock
                    );
                }
                else
                {
                    MessageBox.Show("Producto no encontrado o ya está descontinuado.", "No encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show("Error al buscar producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                txtCodigoBarras.Clear();
                txtCodigoBarras.Focus();
            }
        }

        /// <summary>
        /// Evento Click para el botón de Descontinuar
        /// </summary>
        private void btnDescontinuar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                int idProducto = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["ID"].Value);
                string nombreProducto = dgvProductos.SelectedRows[0].Cells["Nombre"].Value.ToString();

                var confirmacion = MessageBox.Show($"¿Seguro que desea descontinuar '{nombreProducto}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    
                    bool exito = datosProducto.DescontinuarProducto(idProducto);

                    if (exito)
                    {
                        MessageBox.Show("Producto descontinuado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dgvProductos.Rows.Remove(dgvProductos.SelectedRows[0]);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo descontinuar el producto (quizás fue eliminado por otro usuario).", "Fallo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Si el backend lanza un error (ej. la transacción falló), lo mostramos
                MessageBox.Show("Error al descontinuar: " + ex.Message, "Error de Transacción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}