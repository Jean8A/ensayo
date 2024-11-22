using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ENSAYO.Models;

public partial class EnsayoContext : DbContext
{
    public EnsayoContext()
    {
    }

    public EnsayoContext(DbContextOptions<EnsayoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abono> Abonos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; }

    public virtual DbSet<DetalleReservaServicio> DetalleReservaServicios { get; set; }

    public virtual DbSet<EstadosReserva> EstadosReservas { get; set; }

    public virtual DbSet<Habitacione> Habitaciones { get; set; }

    public virtual DbSet<ImagenAbono> ImagenAbonos { get; set; }

    public virtual DbSet<ImagenHabitacion> ImagenHabitacions { get; set; }

    public virtual DbSet<ImagenPaquete> ImagenPaquetes { get; set; }

    public virtual DbSet<ImagenServicio> ImagenServicios { get; set; }

    public virtual DbSet<Imagene> Imagenes { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Paquete> Paquetes { get; set; }

    public virtual DbSet<PaqueteServicio> PaqueteServicios { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<PermisosRole> PermisosRoles { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoHabitacione> TipoHabitaciones { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-5QOGGJ7\\SQLEXPRESS;Database=ENSAYO;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación muchos a muchos entre Habitacione e Imagene a través de ImagenHabitacion
        modelBuilder.Entity<ImagenHabitacion>()
            .HasOne(ih => ih.IdHabitacionNavigation)  // Cada ImagenHabitacion tiene una Habitacione
            .WithMany(h => h.ImagenHabitacions)        // Una Habitacione puede tener muchas ImagenHabitacion
            .HasForeignKey(ih => ih.IdHabitacion)    // Llave foránea a Habitacione
            .OnDelete(DeleteBehavior.Cascade);         // Eliminación en cascada

        //modelBuilder.Entity<ImagenHabitacion>()
        //    .HasOne(ih => ih.IdImagenNavigation)      // Cada ImagenHabitacion tiene una Imagene
        //    .WithMany(i => i.ImagenHabitacions)       // Una Imagene puede estar en muchas ImagenHabitacion
        //    .HasForeignKey(ih => ih.IdImagen);        // Llave foránea a Imagene

        modelBuilder.Entity<ImagenHabitacion>(entity =>
        {
            // Relación con Habitacione
            entity.HasOne(ih => ih.IdHabitacionNavigation)  // Cada ImagenHabitacion pertenece a una Habitacione
                .WithMany(h => h.ImagenHabitacions)         // Una Habitacione puede tener muchas ImagenHabitacions
                .HasForeignKey(ih => ih.IdHabitacion)       // Llave foránea a Habitacione
                .HasConstraintName("FK__ImagenHab__IdHab__440B1D61");

            // Relación con Imagene
            entity.HasOne(ih => ih.IdImagenNavigation)      // Cada ImagenHabitacion tiene una Imagene
                .WithMany(i => i.ImagenHabitacions)         // Una Imagene puede estar en muchas ImagenHabitacion
                .HasForeignKey(ih => ih.IdImagen)           // Llave foránea a Imagene
                .HasConstraintName("FK__ImagenHab__IdIma__44FF419A");

        });



        modelBuilder.Entity<Abono>(entity =>
        {
            entity.HasKey(e => e.IdAbono).HasName("PK__Abono__A4693DA75046CECF");

            entity.ToTable("Abono");

            entity.Property(e => e.CantAbono).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.FechaAbono)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Abonos)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__Abono__IdReserva__75A278F5");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.NroDocumento).HasName("PK__Clientes__CC62C91CBF5C3D4F");

            entity.Property(e => e.Apellidos)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Clientes__IdRol__5EBF139D");

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdTipoDocumento)
                .HasConstraintName("FK__Clientes__IdTipo__5FB337D6");
        });

        modelBuilder.Entity<DetalleReservaPaquete>(entity =>
        {
            entity.HasKey(e => e.DetalleReservaPaquete1).HasName("PK__DetalleR__2E8BFF2543E3F05D");

            entity.ToTable("DetalleReservaPaquete");

            entity.Property(e => e.DetalleReservaPaquete1).HasColumnName("DetalleReservaPaquete");
            entity.Property(e => e.Costo).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__DetalleRe__IdPaq__70DDC3D8");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__DetalleRe__IdRes__71D1E811");
        });

        modelBuilder.Entity<DetalleReservaServicio>(entity =>
        {
            entity.HasKey(e => e.IdDetalleReservaServicio).HasName("PK__DetalleR__D3D91A5A27D05290");

            entity.ToTable("DetalleReservaServicio");

            entity.Property(e => e.Costo).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.IdReserva)
                .HasConstraintName("FK__DetalleRe__IdRes__6E01572D");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__DetalleRe__IdSer__6D0D32F4");
        });

        modelBuilder.Entity<EstadosReserva>(entity =>
        {
            entity.HasKey(e => e.IdEstadoReserva).HasName("PK__EstadosR__3E654CA8A077A052");

            entity.ToTable("EstadosReserva");

            entity.Property(e => e.NombreEstadoReserva)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Habitacione>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion).HasName("PK__Habitaci__8BBBF90198C62D49");

            entity.Property(e => e.Costo).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreHabitacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTipoHabitacionNavigation).WithMany(p => p.Habitaciones)
                .HasForeignKey(d => d.IdTipoHabitacion)
                .HasConstraintName("FK__Habitacio__IdTip__4316F928");
            //---------------------------------
            //entity.Property(e => e.Estado).HasDefaultValue(true);
            // Configuración para la relación con ImagenHabitacions
            entity.HasMany(h => h.ImagenHabitacions)
                .WithOne(ih => ih.IdHabitacionNavigation)
                .HasForeignKey(ih => ih.IdHabitacion)
                .HasConstraintName("FK__ImagenHab__IdHab__440B1D61");
        });

        modelBuilder.Entity<ImagenAbono>(entity =>
        {
            entity.HasKey(e => e.IdImagenAbono).HasName("PK__ImagenAb__A40FAE53ACB5656E");

            entity.ToTable("ImagenAbono");

            entity.HasOne(d => d.IdAbonoNavigation).WithMany(p => p.ImagenAbonos)
                .HasForeignKey(d => d.IdAbono)
                .HasConstraintName("FK__ImagenAbo__IdAbo__797309D9");

            entity.HasOne(d => d.IdImagenNavigation).WithMany(p => p.ImagenAbonos)
                .HasForeignKey(d => d.IdImagen)
                .HasConstraintName("FK__ImagenAbo__IdIma__787EE5A0");
        });

        modelBuilder.Entity<ImagenHabitacion>(entity =>
        {
            entity.HasKey(e => e.IdImagenHabi).HasName("PK__ImagenHa__5B5FF6ADD722B4AB");

            entity.ToTable("ImagenHabitacion");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.ImagenHabitacions)
                .HasForeignKey(d => d.IdHabitacion)
                .HasConstraintName("FK__ImagenHab__IdHab__46E78A0C");

            entity.HasOne(d => d.IdImagenNavigation).WithMany(p => p.ImagenHabitacions)
                .HasForeignKey(d => d.IdImagen)
                .HasConstraintName("FK__ImagenHab__IdIma__45F365D3");
        });

        modelBuilder.Entity<ImagenPaquete>(entity =>
        {
            entity.HasKey(e => e.IdImagenPaque).HasName("PK__ImagenPa__AD53DF94487CC4D2");

            entity.ToTable("ImagenPaquete");

            entity.HasOne(d => d.IdImagenNavigation).WithMany(p => p.ImagenPaquetes)
                .HasForeignKey(d => d.IdImagen)
                .HasConstraintName("FK__ImagenPaq__IdIma__5535A963");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.ImagenPaquetes)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__ImagenPaq__IdPaq__5629CD9C");
        });

        modelBuilder.Entity<ImagenServicio>(entity =>
        {
            entity.HasKey(e => e.IdImagenServi).HasName("PK__ImagenSe__3C03784C52F16FFB");

            entity.ToTable("ImagenServicio");

            entity.HasOne(d => d.IdImagenNavigation).WithMany(p => p.ImagenServicios)
                .HasForeignKey(d => d.IdImagen)
                .HasConstraintName("FK__ImagenSer__IdIma__4E88ABD4");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ImagenServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__ImagenSer__IdSer__4F7CD00D");
        });

        modelBuilder.Entity<Imagene>(entity =>
        {
            entity.HasKey(e => e.IdImagen).HasName("PK__Imagenes__B42D8F2AFDCD8D32");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__MetodoPa__6F49A9BE678EA3DC");

            entity.ToTable("MetodoPago");

            entity.Property(e => e.NomMetodoPago)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.HasKey(e => e.IdPaquete).HasName("PK__Paquetes__DE278F8BB7831F40");

            entity.Property(e => e.Costo).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NomPaquete)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Paquetes)
                .HasForeignKey(d => d.IdHabitacion)
                .HasConstraintName("FK__Paquetes__IdHabi__52593CB8");
        });

        modelBuilder.Entity<PaqueteServicio>(entity =>
        {
            entity.HasKey(e => e.IdPaqueteServicio).HasName("PK__PaqueteS__DE5C2BC23DCF499F");

            entity.ToTable("PaqueteServicio");

            entity.Property(e => e.Costo).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdPaqueteNavigation).WithMany(p => p.PaqueteServicios)
                .HasForeignKey(d => d.IdPaquete)
                .HasConstraintName("FK__PaqueteSe__IdPaq__59063A47");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.PaqueteServicios)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__PaqueteSe__IdSer__59FA5E80");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permisos__0D626EC8262DDC90");

            entity.Property(e => e.NomPermiso)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PermisosRole>(entity =>
        {
            entity.HasKey(e => e.IdPermisosRoles).HasName("PK__Permisos__8C257ED975284C72");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.PermisosRoles)
                .HasForeignKey(d => d.IdPermiso)
                .HasConstraintName("FK__PermisosR__IdPer__3C69FB99");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.PermisosRoles)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__PermisosR__IdRol__3B75D760");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reservas__0E49C69DE9D498A9");

            entity.Property(e => e.FechaFinalizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaReserva)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdEstadoReserva).HasDefaultValueSql("('1')");
            entity.Property(e => e.Iva)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("IVA");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdEstadoReservaNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEstadoReserva)
                .HasConstraintName("FK__Reservas__IdEsta__693CA210");

            entity.HasOne(d => d.MetodoPagoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MetodoPago)
                .HasConstraintName("FK__Reservas__Metodo__6A30C649");

            entity.HasOne(d => d.NroDocumentoClienteNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.NroDocumentoCliente)
                .HasConstraintName("FK__Reservas__IdEsta__68487DD7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C02AAEBC4");

            entity.Property(e => e.NomRol)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__Servicio__2DCCF9A2596EFC75");

            entity.Property(e => e.Costo).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.NomServicio)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTipoServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdTipoServicio)
                .HasConstraintName("FK__Servicios__IdTip__4BAC3F29");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento).HasName("PK__TipoDocu__3AB3332F9ADCBAAC");

            entity.ToTable("TipoDocumento");

            entity.Property(e => e.NomTipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoHabitacione>(entity =>
        {
            entity.HasKey(e => e.IdTipoHabitacion).HasName("PK__TipoHabi__AB75E87C7AB1133E");

            entity.Property(e => e.NombreTipoHabitacion)
                .HasMaxLength(20)
                .IsUnicode(false);
            // Configuración de la propiedad Estado
            entity.Property(e => e.Estado)
                .IsRequired(); // Asegura que la propiedad Estado no sea nula
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.IdTipoServicio).HasName("PK__TipoServ__E29B3EA7E4675266");

            entity.Property(e => e.NombreTipoServicio)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdInventario).HasName("PK_Inventario");

            entity.Property(e => e.NombreItem)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(e => e.Habitacion)
                .WithMany(h => h.Inventarios)
                .HasForeignKey(e => e.IdHabitacion)
                .OnDelete(DeleteBehavior.SetNull); // Mantiene el inventario al eliminar una habitación
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
