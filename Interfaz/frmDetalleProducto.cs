using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransactionsDB.Clases;
using TransactionsDB.ConectionDB;

namespace TransactionsDB.Interfaz
{
    public partial class frmDetalleProducto : Form
    {
        public Producto ProductoCreado { get; private set; }

        private readonly Conexion _datosProducto;

        public frmDetalleProducto()
        {
            InitializeComponent();
            _datosProducto = new Conexion();
            ProductoCreado = null;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtCodigoBarras.Text.Length < 7 || txtCodigoBarras.Text.Length > 13)
            {
                MessageBox.Show("El Código de Barras debe de tener una longitud entre 7 y 13 caracteres", "Error en Código de barras", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCodigoBarras.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El Código de Barras y el Nombre son obligatorios.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (int.Parse(txtPrecio.Text) < 0 || int.Parse(txtStock.Text) < 0)
                {
                    MessageBox.Show("El Precio y el Stock deben ser números enteros válidos\n " +
                        "mayores a 0 para precio y meyores o iguales a 0 para stock.", "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (FormatException)
            {
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || !int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("El Precio y el Stock deben ser números válidos", "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            

            Producto nuevoProducto = new Producto
                {
                    CodigoDeBarras = txtCodigoBarras.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Precio = decimal.Parse(txtPrecio.Text.Trim()),
                    Stock = int.Parse(txtStock.Text.Trim())
            };

                try
                {
                    int nuevoId = _datosProducto.AgregarProducto(nuevoProducto);

                    if (nuevoId > 0)
                    {
                        nuevoProducto.Id = nuevoId;
                        this.ProductoCreado = nuevoProducto;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                }
            
        }
}
