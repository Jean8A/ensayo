using System;
using System.Collections.Generic;

namespace ENSAYO.Models
{
    public partial class Inventario
    {
        public int IdInventario { get; set; } // ID único para el inventario
        public string NombreItem { get; set; } // Nombre del elemento
        public string? Descripcion { get; set; } // Descripción opcional
        public int? IdHabitacion { get; set; } // Relación opcional con una habitación
        public bool Disponible { get; set; } // Indica si está en uso o disponible

        public virtual Habitacione? Habitacion { get; set; } // Relación con Habitacione
    }

}
