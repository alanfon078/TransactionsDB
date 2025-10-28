using MySql.Data.MySqlClient;
using System;
using System.Data;
using TransactionsDB.Clases;

namespace TransactionsDB.ConectionDB
{
    internal class Conexion
    {

        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection("server=localhost; database=TransactionsDB; user=root; pwd=root");
            conexion.Open();
            return conexion;
        }

        /// <summary>
        /// Busca un producto activo por su código de barras.
        /// </summary>
        /// <param name="codigo">El código de barras a buscar</param>
        /// <returns>Un objeto Producto si se encuentra, o null si no existe o está descontinuado.</returns>
        public Producto BuscarProductoPorCodigo(string codigo)
        {
            Producto producto = null;
            string query = "SELECT id, codigoDeBarras, nombre, precio, stock FROM product " +
                           "WHERE codigoDeBarras = @codigo AND descontinuado = 0";

            using (MySqlConnection conn = Conexion.ObtenerConexion())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigo);

                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                producto = new Producto
                                {
                                    Id = reader.GetInt32("id"),
                                    CodigoDeBarras = reader.GetString("codigoDeBarras"),
                                    Nombre = reader.GetString("nombre"),
                                    Precio = reader.GetDecimal("precio"),
                                    Stock = reader.GetInt32("stock")
                                };
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error al buscar producto: " + ex.Message);
                        throw new Exception("Error de base de datos al buscar el producto.", ex);
                    }
                }
            } 

            return producto;
        }

        /// <summary>
        /// REQUISITO: Descontinúa un producto usando una transacción.
        /// </summary>
        /// <param name="productoId">El ID del producto a descontinuar</param>
        /// <returns>True si la transacción fue exitosa, False en caso contrario.</returns>
        public bool DescontinuarProducto(int productoId)
        {
            bool exito = false;

            using (MySqlConnection conn = Conexion.ObtenerConexion())
            {
                // 1. Iniciar la transacción
                MySqlTransaction transaction = null;

                try
                {
                    transaction = conn.BeginTransaction();

                    string query = "UPDATE product SET descontinuado = 1 WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", productoId);
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            // 2. Si todo va bien, confirma (Commit) la transacción
                            transaction.Commit();
                            exito = true;
                        }
                        else
                        {
                            // 3. Si no se afectaron filas (ID no existía), revierte (Rollback)
                            transaction.Rollback();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // 3. Si hay cualquier error de SQL, revierte (Rollback)
                    Console.WriteLine("Error en transacción: " + ex.Message);
                    try
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception rbEx)
                    {
                        Console.WriteLine("Error crítico al hacer rollback: " + rbEx.Message);
                    }

                    throw new Exception("Error al procesar la transacción para descontinuar.", ex);
                }
            }

            return exito;
        }
        /// <summary>
        /// Agrega un nuevo producto a la base de datos usando una transacción.
        /// </summary>
        /// <param name="producto">El objeto Producto con los nuevos datos</param>
        /// <returns>El ID del producto recién creado, o 0 si falla.</returns>
        public int AgregarProducto(Producto producto)
        {
            int nuevoId = 0;

            string query = @"INSERT INTO product (codigoDeBarras, nombre, precio, stock, descontinuado) 
                     VALUES (@codigo, @nombre, @precio, @stock, 0);
                     SELECT LAST_INSERT_ID();";

            using (MySqlConnection conn = Conexion.ObtenerConexion())
            {
                MySqlTransaction transaction = null;
                try
                {
                    transaction = conn.BeginTransaction();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@codigo", producto.CodigoDeBarras);
                        cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                        cmd.Parameters.AddWithValue("@precio", producto.Precio);
                        cmd.Parameters.AddWithValue("@stock", producto.Stock);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            nuevoId = Convert.ToInt32(result);
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error en transacción de inserción: " + ex.Message);
                    try
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback(); // Revertimos por algun error
                        }
                    }
                    catch (Exception rbEx)
                    {
                        Console.WriteLine("Error crítico al hacer rollback: " + rbEx.Message);
                    }

                    if (ex.Number == 1062)
                    {
                        throw new Exception("Error: El código de barras '" + producto.CodigoDeBarras + "' ya existe en la base de datos.", ex);
                    }

                    throw new Exception("Error al procesar la transacción para agregar el producto.", ex);
                }
            }

            return nuevoId;
        }
    }
}