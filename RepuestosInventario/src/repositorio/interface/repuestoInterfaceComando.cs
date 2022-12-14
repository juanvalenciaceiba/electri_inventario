using RepuestosInventario.src.dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepuestosInventario.src.repositorio
{
    public interface repuestoInterfaceComando
    {
        void guardarRepuesto(repuesto repuesto);
        void modificarRepuesto(string referencia, short cantidad);
        void modificarRepuestoPrecio(string referencia, double precio, double costo);
    }
}
