using MySql.Data.MySqlClient;
using TransactionsDB;

namespace ProyectoBD.Conexion
{
    internal class Conexion
    {
        public Conexion()
        {
        }

        // Método para establecer la conexión con la base de datos
        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conexion = new MySqlConnection("server=localhost; database=TransactionsDB; user=root; pwd=root");
            conexion.Open();
            return conexion;
        }

        /// <summary>
        /// Verifica las credenciales del usuario en la base de datos.
        /// </summary>
        /// <param name="user">Nombre de usuario</param>
        /// <param name="password">Contraseña sin encriptar</param>
        /// <returns>True si el login es exitoso, False en caso contrario.</returns>
        public bool Login(string user, string password)
        {
            bool loginExitoso = false;
            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    string query = "SELECT COUNT(*) FROM user WHERE user = @user AND password = SHA2(@password, 256)";
                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@user", user);
                    comando.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(comando.ExecuteScalar());

                    if (count > 0)
                    {
                        loginExitoso = true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Manejar la excepción (por ejemplo, mostrar un mensaje de error)
                Console.WriteLine("Error de conexión: " + ex.Message);
            }
            return loginExitoso;
        }

        /// <summary>
        /// Agrega un nuevo usuario a la base de datos.
        /// </summary>
        /// <param name="usuario">Objeto clsUser con los datos del nuevo usuario</param>
        /// <returns>True si el registro es exitoso, False en caso contrario.</returns>
        public bool RegistrarUsuario(clsUser usuario)
        {
            bool exito = false;
            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    string query = @"INSERT INTO user (nombre, apellidos, user, password, correo, telefono, fechaNacimiento) 
                                     VALUES (@nombre, @apellidos, @user, SHA2(@password, 256), @correo, @telefono, @fechaNacimiento)";

                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@apellidos", usuario.Apellidos);
                    comando.Parameters.AddWithValue("@user", usuario.User);
                    comando.Parameters.AddWithValue("@password", usuario.Password);
                    comando.Parameters.AddWithValue("@correo", usuario.Correo);
                    comando.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    comando.Parameters.AddWithValue("@fechaNacimiento", usuario.FechaNacimiento);

                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        exito = true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al registrar usuario: " + ex.Message);
            }
            return exito;
        }

        /// <summary>
        /// Obtiene una lista de todos los usuarios registrados en la base de datos.
        /// </summary>
        /// <returns>Una lista de objetos clsUser.</returns>
        public List<clsUser> ObtenerUsuarios()
        {
            List<clsUser> listaUsuarios = new List<clsUser>();
            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    string query = "SELECT nombre, apellidos, user, status, correo, telefono, fechaNacimiento, fechaCreacion FROM user";
                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    MySqlDataReader reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        clsUser usuario = new clsUser
                        {
                            Nombre = reader.GetString("nombre"),
                            Apellidos = reader.GetString("apellidos"),
                            User = reader.GetString("user"),
                            Status = reader.GetBoolean("status"),
                            Correo = reader.GetString("correo"),
                            Telefono = reader.GetString("telefono"),
                            FechaNacimiento = reader.GetDateTime("fechaNacimiento").ToString("yyyy-MM-dd"),
                            FechaCreacion = reader.GetDateTime("fechaCreacion").ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        listaUsuarios.Add(usuario);
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al obtener usuarios: " + ex.Message);
            }
            return listaUsuarios;
        }

        public bool ValidarUsuario(string user)
        {
            bool UsuarioDisponible = true;
            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    string query = "SELECT COUNT(*) FROM user WHERE user = @user";
                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@user", user);

                    int count = Convert.ToInt32(comando.ExecuteScalar());

                    if (count > 0)
                    {
                        UsuarioDisponible = false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error de conexión: " + ex.Message);
            }
            return UsuarioDisponible;
        }

    }
}