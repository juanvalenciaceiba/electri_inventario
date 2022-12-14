using RepuestosInventario.src.dominio;
using RepuestosInventario.src.repositorio.repositorioPostgreSQL;
using System;
using System.Windows.Forms;


namespace RepuestosInventario
{
    public partial class FormInventario : Form
    {
        public FormInventario()
        {
            InitializeComponent();
            panelBusqueda.Visible = false;
            ocultargroupbox();
        }

        private void ocultargroupbox()
        {
            groupBoxRegistro.Visible = false;
            groupBoxLista.Visible = false;
            groupBoxVenta.Visible = false;
            groupBoxBuscarReferencia.Visible = false;
            groupBoxBuscarNombre.Visible = false;
            groupBoxActualizar.Visible = false;
        }
        private void ocultarGroup()
        {
            if(groupBoxRegistro.Visible == true)
               groupBoxRegistro.Visible = false;
            if (groupBoxLista.Visible == true)
                groupBoxLista.Visible = false;
            if (groupBoxVenta.Visible == true)
                groupBoxVenta.Visible = false;
            if (groupBoxBuscarReferencia.Visible == true)
                groupBoxBuscarReferencia.Visible = false;
            if (groupBoxBuscarNombre.Visible == true)
                groupBoxBuscarNombre.Visible = false;
            if (groupBoxActualizar.Visible == true)
                groupBoxActualizar.Visible = false;
        }
        private void ocultarSubMenu()
        {
            panelBusqueda.Visible = false;
        }
        private void mostrarSubMenu()
        {
            if(panelBusqueda.Visible == false)
            {
                panelBusqueda.Visible = true;
            }
        }
        void formatoMoneda(TextBox textBox) 
        {
            decimal monto;
            if(!textBox.Text.Equals(""))
            {
                monto = Convert.ToDecimal(textBox.Text);
                textBox.Text = monto.ToString("#,#");
            }
        }
        private void guardar_Click_1(object sender, EventArgs e)
        {
            formatoMoneda(precio);
            repuesto repuesto = repuesto.build(referencia.Text,nombre.Text, marca.Text, short.Parse(cantidad.Text), double.Parse(precio.Text), double.Parse(costo.Text));
            referencia.Text = "";
            nombre.Text = "";
            marca.Text = "";
            cantidad.Text = "";
            precio.Text = "";
            costo.Text = "";
            repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();
            repuestoPosgreSQLComando repuestosComando = new repuestoPosgreSQLComando();
            repuestosComando.guardarRepuesto(repuesto);
            repuestosConsulta.mostrarRepuestos(listaRepuestos);
        }

        private void listaRepuestos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            repuestoPostgreSQLConsulta repuestos = new repuestoPostgreSQLConsulta();
            repuestos.seleccionarRepuesto(listaRepuestos, referenciaModificar);
        }

        private void retiroIngreso_Click(object sender, EventArgs e)
        {
            short cant = 0;

            if (referenciaModificar.Text != "" && cantidadModificar.Text != "")
            {
                repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();
                repuesto repuesto = repuestosConsulta.mostrarRepuestosPorReferenciaParaModificar(referenciaModificar.Text);
                
                if(ingresoCheck.Checked)
                {
                    cant = (short)(repuesto.Cantidad + short.Parse(cantidadModificar.Text));
                    this.retiroIngresoCantidad(referenciaModificar.Text, cant);

                }
                else if (ventaCheck.Checked)
                {
                    cant = (short)(repuesto.Cantidad - short.Parse(cantidadModificar.Text));
                    if(cant >= 0)
                    {
                        this.retiroIngresoCantidad(referenciaModificar.Text, cant);
                    }
                    else
                    {
                        MessageBox.Show("No tiene cantidad suficientes de esta referencia", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Error);                      
                    }
                }
                else
                {
                    MessageBox.Show("No a selecionado ninguna operacion");
                }
                repuestosConsulta.mostrarRepuestos(listaRepuestos);
                repuestosConsulta.mostrarRepuestosPorReferencia(tablaRetiro, referenciaModificar.Text);
            }
            else
            {
                MessageBox.Show("Deba ingresar una Referencia ó cantidad");
            }
        }
        private void buscarReferenciaBT_Click(object sender, EventArgs e)
        {
       
            repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();
            repuestosConsulta.mostrarRepuestosPorReferencia(tablaBusquedaReferencia, busquedaReferenciaText.Text);

        }
        private void buscarNombreBT_Click(object sender, EventArgs e)
        {
            repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();
            repuestosConsulta.mostrarRepuestosPorNombre(tablaBusquedaNombre, nombreBuscar.Text);
        }

        private void retiroIngresoCantidad(string referencia , short cantidad)
        {
            repuestoPosgreSQLComando repuestosComando = new repuestoPosgreSQLComando();
            repuestosComando.modificarRepuesto(referenciaModificar.Text, cantidad);
        }
        private void ventaCheck_CheckedChanged(object sender, EventArgs e)
        {
            if(ventaCheck.Checked == true)
            {
                ingresoCheck.Checked = false;
            }
        }
        private void ingresoCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ingresoCheck.Checked == true)
            {
                ventaCheck.Checked = false;
            }
        }
        private void modificarValorBT_Click(object sender, EventArgs e)
        {
            repuestoPosgreSQLComando repuestosComando = new repuestoPosgreSQLComando();
            repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();

            if(precioModificar.Text == "")
            {
                precioModificar.Text = "0";
            }
            if (costoModificar.Text == "")
            {
                costoModificar.Text = "0";
            }
            repuestosComando.modificarRepuestoPrecio(referenciaModificarPrecio.Text,double.Parse(precioModificar.Text),double.Parse(costoModificar.Text));
            repuestosConsulta.mostrarRepuestosPorReferencia(listaRepuestos, referenciaModificarPrecio.Text);
        }
        private void precio_MouseMove(object sender, MouseEventArgs e)
        {
            formatoMoneda(precio);
        }
        private void costo_MouseMove(object sender, MouseEventArgs e)
        {
            formatoMoneda(costo);
        }
        private void precioModificar_MouseMove(object sender, MouseEventArgs e)
        {
            formatoMoneda(precioModificar);
        }
        private void costoModificar_MouseMove(object sender, MouseEventArgs e)
        {
            formatoMoneda(costoModificar);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin login = new FormLogin();
            login.Show();
        }

        private void buscarMenu_Click(object sender, EventArgs e)
        {
            mostrarSubMenu();
        }

        private void registrarMenu_Click(object sender, EventArgs e)
        {
            ocultarSubMenu();
            ocultarGroup();
            groupBoxRegistro.Visible = true;
        }

        private void listaProductosMenu_Click(object sender, EventArgs e)
        {
            repuestoPostgreSQLConsulta repuestosConsulta = new repuestoPostgreSQLConsulta();
            repuestosConsulta.mostrarRepuestos(listaRepuestos);
            ocultarSubMenu();
            ocultarGroup();
            groupBoxLista.Visible = true;
        }
        private void ventaMenu_Click(object sender, EventArgs e)
        {
            ocultarSubMenu();
            ocultarGroup();
            referenciaModificar.Text = "";
            cantidadModificar.Text = "";
            groupBoxVenta.Visible = true;
        }
        private void buscarReferenciaMenu_Click(object sender, EventArgs e)
        {
            ocultarGroup();
            busquedaReferenciaText.Text = "";
            groupBoxBuscarReferencia.Visible = true;
        }

        private void bucarNombreMenu_Click(object sender, EventArgs e)
        {
            ocultarGroup();
            nombreBuscar.Text = "";
            groupBoxBuscarNombre.Visible = true;

        }
        private void actualizarMenu_Click(object sender, EventArgs e)
        {
            ocultarSubMenu();
            ocultarGroup();
            referenciaModificarPrecio.Text = "";
            precioModificar.Text = "";
            costoModificar.Text = "";
            groupBoxActualizar.Visible = true;
        }
        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
