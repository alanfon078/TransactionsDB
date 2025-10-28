using System.Data;
using TransactionsDB.Clases;
using TransactionsDB.ConectionDB;
using TransactionsDB.Interfaz;

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
        /// Verifica si un producto ya existe en el DataGridView
        /// </summary>
        /// <param name="codigoDeBarras">El código a buscar en el grid</param>
        /// <returns>True si ya existe, False si no</returns>
        private bool ProductoYaEstaEnGrid(string codigoDeBarras)
        {
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                if (row.Cells["CodigoBarras"].Value != null &&
                    row.Cells["CodigoBarras"].Value.ToString() == codigoDeBarras)
                {
                    return true; // Ya existe
                }
            }
            return false; // No se encontró
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
                    if (ProductoYaEstaEnGrid(producto.CodigoDeBarras))
                    {
                        MessageBox.Show("Este producto ya ha sido agregado a la lista.", "Producto duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        dgvProductos.Rows.Add(
                            producto.Id,
                            producto.CodigoDeBarras,
                            producto.Nombre,
                            producto.Precio,
                            producto.Stock
                        );
                    }
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
                MessageBox.Show("Error al descontinuar: " + ex.Message, "Error de Transacción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento Click para el botón de Buscar.
        /// Ejecuta la misma acción que presionar Enter.
        /// </summary>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarYAgregarProducto();
        }

        /// <summary>
        /// Abre el formulario de detalle para crear un nuevo producto.
        /// </summary>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Abrimos el formulario de detalle como un diálogo
            using (frmDetalleProducto formularioDetalle = new frmDetalleProducto())
            {
                // Mostramos el formulario y esperamos a que se cierre
                if (formularioDetalle.ShowDialog(this) == DialogResult.OK)
                {
                    // Si el usuario guardó (DialogResult.OK), recogemos el producto
                    Producto productoNuevo = formularioDetalle.ProductoCreado;

                    if (productoNuevo != null)
                    {
                        // Validamos si ya está en el grid (por si acaso)
                        if (ProductoYaEstaEnGrid(productoNuevo.CodigoDeBarras))
                        {
                            MessageBox.Show("El producto ya estaba en la lista.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Agregamos el nuevo producto al grid
                            dgvProductos.Rows.Add(
                                productoNuevo.Id,
                                productoNuevo.CodigoDeBarras,
                                productoNuevo.Nombre,
                                productoNuevo.Precio,
                                productoNuevo.Stock
                            );

                            MessageBox.Show("Producto agregado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

    }
}