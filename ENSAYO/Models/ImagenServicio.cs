using System;
using System.Collections.Generic;

namespace ENSAYO.Models;

public partial class ImagenServicio
{
    public int IdImagenServi { get; set; }

    public int? IdImagen { get; set; }

    public int? IdServicio { get; set; }

    public virtual Imagene? IdImagenNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }
}
