﻿using System;
using System.Collections.Generic;

namespace ENSAYO.Models;

public partial class DetalleReservaServicio
{
    public int IdDetalleReservaServicio { get; set; }

    public int? IdServicio { get; set; }

    public int? IdReserva { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Costo { get; set; }

    public virtual Reserva? IdReservaNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }
}
