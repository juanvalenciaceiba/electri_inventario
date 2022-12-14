using Npgsql;
using RepuestosInventario.src.dominio;
using RepuestosInventario.src.trasnversal;
using System.Data;
using System.Windows.Forms;

namespace RepuestosInventario.src.repositorio.repositorioPostgreSQL
{
    public class repuestoPostgreSQLConsulta : repuestoInterfaceConsulta
    {
        EdicionData edicion = new EdicionData();

        public log inicioSesion(string usuario, string contrasena)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                log login;

                string sqlConsulta = "select * from usuario WHERE usuario='" + usuario + "'and contrasena='" + contrasena + "';";

                NpgsqlCommand comando = new NpgsqlCommand(sqlConsulta, objetoConexion.establecerConexion());
                NpgsqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {                 
                    login =  log.build(reader["usuario"].ToString(), reader["contrasena"].ToString());
                }
                else
                {
                    MessageBox.Show("EL usuario o la contraseña son incorrectos ");
                    login = null;
                }
                objetoConexion.cerrarConexion();
                return login;
            }
            catch
            {
                MessageBox.Show("No se puedo Iniciar sesion");
                return null;
            }
        }

        public void mostrarRepuestos(DataGridView tablaRespuestos)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();

                string sqlConsulta = "select * from repuesto";

                tablaRespuestos.DataSource = null;
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlConsulta, objetoConexion.establecerConexion());

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                tablaRespuestos.DataSource = dataTable;

                edicion.asignarTituloColumnasDimencion(tablaRespuestos);
                edicion.aplicarFormatoNumerico(tablaRespuestos);
                edicion.columnasSoloLectura(tablaRespuestos);

                objetoConexion.cerrarConexion();
            }
            catch
            {
                MessageBox.Show("No se puedo mostrar la informacion");
            }
        }

        public void mostrarRepuestosPorNombre(DataGridView tablaRespuestos, string nombre)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                string sqlConsulta = "select * from repuesto WHERE nombre='" + nombre + "';";
                tablaRespuestos.DataSource = null;

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlConsulta, objetoConexion.establecerConexion());
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                tablaRespuestos.DataSource = dataTable;
                edicion.asignarTituloColumnasDimencion(tablaRespuestos);
                edicion.aplicarFormatoNumerico(tablaRespuestos);
                edicion.columnasSoloLectura(tablaRespuestos);

                objetoConexion.cerrarConexion();

            }
            catch
            {
                MessageBox.Show("No se puedo mostrar la informacion");
            }
        }

        public void mostrarRepuestosPorReferencia(DataGridView tablaRespuestos, string referencia)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                string sqlConsulta = "select * from repuesto WHERE referencia='" + referencia + "';";
                tablaRespuestos.DataSource = null;

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlConsulta, objetoConexion.establecerConexion());
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                tablaRespuestos.DataSource = dataTable;

                edicion.asignarTituloColumnasDimencion(tablaRespuestos);
                edicion.aplicarFormatoNumerico(tablaRespuestos);
                edicion.columnasSoloLectura(tablaRespuestos);

                objetoConexion.cerrarConexion();
            }
            catch
            {
                MessageBox.Show("No se puedo mostrar la Referencia");
            }
        }

        public repuesto mostrarRepuestosPorReferenciaParaModificar(string referencia)
        {
            try
            {
                PostgreSQLConfiguration objetoConexion = new PostgreSQLConfiguration();
                repuesto repuesto;

                string sqlConsulta = "select * from repuesto WHERE referencia='" + referencia + "';";

                NpgsqlCommand comando = new NpgsqlCommand(sqlConsulta, objetoConexion.establecerConexion());
                NpgsqlDataReader reader = comando.ExecuteReader();

                if(reader.Read())
                {
                     repuesto = repuesto.build(reader["referencia"].ToString(),reader["nombre"].ToString(),
                       reader["marca"].ToString(), short.Parse(reader["cantidad"].ToString()),
                       int.Parse(reader["precio"].ToString()), int.Parse(reader["costo"].ToString()));
                }
                else
                {
                    MessageBox.Show("No se trajo la informacion");
                    repuesto = null;
                }                                          
                objetoConexion.cerrarConexion();
                return repuesto;

            }
            catch
            {
                MessageBox.Show("No se puedo mostrar la informacion");
                return null;
            }
        }

        public void seleccionarRepuesto(DataGridView tablaRespuestos, TextBox referencia)
        {
            try {
                referencia.Text = tablaRespuestos.CurrentRow.Cells[1].Value.ToString();
            }
            catch {
                MessageBox.Show("No se puedo seleccionar la informacion");
            }
        }
    }
}
