using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionsDB.ConectionDB;

namespace TransactionsDB.Clases
{
    public class Producto
    {
        public int Id { get; set; }
        public string CodigoDeBarras { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
