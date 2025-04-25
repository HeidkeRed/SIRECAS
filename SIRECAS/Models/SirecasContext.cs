using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SIRECAS.Models;

public partial class SirecasContext : DbContext
{
    public SirecasContext()
    {
    }

    public SirecasContext(DbContextOptions<SirecasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcabadosMuro> AcabadosMuros { get; set; }

    public virtual DbSet<AcabadosPiso> AcabadosPisos { get; set; }

    public virtual DbSet<AcabadosPlafone> AcabadosPlafones { get; set; }

    public virtual DbSet<ArteSacroFoto> ArteSacroFotos { get; set; }

    public virtual DbSet<Cimiento> Cimientos { get; set; }

    public virtual DbSet<Cubierta> Cubiertas { get; set; }

    public virtual DbSet<DanoFoto> DanoFotos { get; set; }

    public virtual DbSet<DanoObservable> DanoObservables { get; set; }

    public virtual DbSet<DatosHistorico> DatosHistoricos { get; set; }

    public virtual DbSet<DescripcionDelEdificio> DescripcionDelEdificios { get; set; }

    public virtual DbSet<DocumentoLegal> DocumentoLegals { get; set; }

    public virtual DbSet<ElementoArteSacro> ElementoArteSacros { get; set; }

    public virtual DbSet<Entrepiso> Entrepisos { get; set; }

    public virtual DbSet<Estructura> Estructuras { get; set; }

    public virtual DbSet<Identificacion> Identificacions { get; set; }

    public virtual DbSet<Instalacione> Instalaciones { get; set; }

    public virtual DbSet<PlanimetriaExistente> PlanimetriaExistentes { get; set; }

    public virtual DbSet<Puerta> Puertas { get; set; }

    public virtual DbSet<RegistroTrabajo> RegistroTrabajos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventana> Ventanas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SIRECAS;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcabadosMuro>(entity =>
        {
            entity.HasKey(e => e.IdAcabadoMuro).HasName("PK__Acabados__BC4F00BF4BAC3F29");

            entity.ToTable("Acabados_Muros");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Acabados_Muros_IdIdentificacion").IsUnique();

            entity.Property(e => e.Aparente).HasDefaultValue(false);
            entity.Property(e => e.BaseCemento).HasDefaultValue(false);
            entity.Property(e => e.OtrosAcabados).HasMaxLength(200);
            entity.Property(e => e.Pintura).HasDefaultValue(false);
            entity.Property(e => e.Yeso).HasDefaultValue(false);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.AcabadosMuro)
                .HasForeignKey<AcabadosMuro>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Acabados___IdIde__5165187F");
        });

        modelBuilder.Entity<AcabadosPiso>(entity =>
        {
            entity.HasKey(e => e.IdAcabadoPiso).HasName("PK__Acabados__EEF650E74316F928");

            entity.ToTable("Acabados_Pisos");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Acabados_Pisos_IdIdentificacion").IsUnique();

            entity.Property(e => e.ConcretoPulido).HasDefaultValue(false);
            entity.Property(e => e.Mosaico).HasDefaultValue(false);
            entity.Property(e => e.OtrosAcabados).HasMaxLength(200);
            entity.Property(e => e.PisoCeramico).HasDefaultValue(false);
            entity.Property(e => e.Tierra).HasDefaultValue(false);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.AcabadosPiso)
                .HasForeignKey<AcabadosPiso>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Acabados___IdIde__48CFD27E");
        });

        modelBuilder.Entity<AcabadosPlafone>(entity =>
        {
            entity.HasKey(e => e.IdAcabadoPlafon).HasName("PK__Acabados__922166F05441852A");

            entity.ToTable("Acabados_Plafones");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Acabados_Plafones_IdIdentificacion").IsUnique();

            entity.Property(e => e.AparenteSinAcabadoFinal).HasDefaultValue(false);
            entity.Property(e => e.BaseCemento).HasDefaultValue(false);
            entity.Property(e => e.BaseYeso).HasDefaultValue(false);
            entity.Property(e => e.OtrosAcabados).HasMaxLength(200);
            entity.Property(e => e.Pintura).HasDefaultValue(false);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.AcabadosPlafone)
                .HasForeignKey<AcabadosPlafone>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Acabados___IdIde__59FA5E80");
        });

        modelBuilder.Entity<ArteSacroFoto>(entity =>
        {
            entity.HasKey(e => e.IdFoto).HasName("PK__ArteSacr__007D321D7B5B524B");

            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.RutaArchivo).HasMaxLength(300);

            entity.HasOne(d => d.IdElementoNavigation).WithMany(p => p.ArteSacroFotos)
                .HasForeignKey(d => d.IdElemento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArteSacro__IdEle__7D439ABD");
        });

        modelBuilder.Entity<Cimiento>(entity =>
        {
            entity.HasKey(e => e.IdCimientos).HasName("PK__Cimiento__C16E7C8B1920BF5C");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Cimientos_IdIdentificacion").IsUnique();

            entity.Property(e => e.LosaCimentacion).HasDefaultValue(false);
            entity.Property(e => e.MamposteriaPiedra).HasDefaultValue(false);
            entity.Property(e => e.OtrosCimientos).HasMaxLength(200);
            entity.Property(e => e.TierraCompactada).HasDefaultValue(false);
            entity.Property(e => e.ZapatasAisladas).HasDefaultValue(false);
            entity.Property(e => e.ZapatasCorridas).HasDefaultValue(false);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.Cimiento)
                .HasForeignKey<Cimiento>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cimientos__IdIde__1FCDBCEB");
        });

        modelBuilder.Entity<Cubierta>(entity =>
        {
            entity.HasKey(e => e.IdCubiertas).HasName("PK__Cubierta__662AED5236B12243");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Cubiertas_IdIdentificacion").IsUnique();

            entity.Property(e => e.Boveda).HasDefaultValue(false);
            entity.Property(e => e.Concreto).HasDefaultValue(false);
            entity.Property(e => e.Cupula).HasDefaultValue(false);
            entity.Property(e => e.DosAguas).HasDefaultValue(false);
            entity.Property(e => e.Lamina).HasDefaultValue(false);
            entity.Property(e => e.LosaPlana).HasDefaultValue(false);
            entity.Property(e => e.OtrasCubiertas).HasMaxLength(200);
            entity.Property(e => e.Palma).HasDefaultValue(false);
            entity.Property(e => e.TresCuatroAguas).HasDefaultValue(false);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.Cubierta)
                .HasForeignKey<Cubierta>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cubiertas__IdIde__403A8C7D");
        });

        modelBuilder.Entity<DanoFoto>(entity =>
        {
            entity.HasKey(e => e.IdFoto).HasName("PK__DanoFoto__007D321D04E4BC85");

            entity.ToTable("DanoFoto");

            entity.Property(e => e.Observacion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RutaArchivo)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDanoNavigation).WithMany(p => p.DanoFotos)
                .HasForeignKey(d => d.IdDano)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DanoFoto_DanoObservable");
        });

        modelBuilder.Entity<DanoObservable>(entity =>
        {
            entity.HasKey(e => e.IdDano).HasName("PK__DanoObse__F2989F6B00200768");

            entity.ToTable("DanoObservable");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Zona)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithMany(p => p.DanoObservables)
                .HasForeignKey(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DanoObservable_Identificacion");
        });

        modelBuilder.Entity<DatosHistorico>(entity =>
        {
            entity.HasKey(e => e.IdDatoHistorico).HasName("PK__DatosHis__3173143F70DDC3D8");

            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithMany(p => p.DatosHistoricos)
                .HasForeignKey(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DatosHist__IdIde__72C60C4A");
        });

        modelBuilder.Entity<DescripcionDelEdificio>(entity =>
        {
            entity.HasKey(e => e.IdDescripcion).HasName("PK__Descripc__130EFED6145C0A3F");

            entity.ToTable("DescripcionDelEdificio");

            entity.Property(e => e.FotografiaFachada).HasMaxLength(255);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithMany(p => p.DescripcionDelEdificios)
                .HasForeignKey(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Descripci__IdIde__164452B1");
        });

        modelBuilder.Entity<DocumentoLegal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC071332DBDC");

            entity.ToTable("DocumentoLegal");

            entity.Property(e => e.FechaSubida)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreDocumento).HasMaxLength(200);
            entity.Property(e => e.RutaArchivo).HasMaxLength(300);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithMany(p => p.DocumentoLegals)
                .HasForeignKey(d => d.IdIdentificacion)
                .HasConstraintName("FK_DocumentoLegal_Identificacion");
        });

        modelBuilder.Entity<ElementoArteSacro>(entity =>
        {
            entity.HasKey(e => e.IdElemento).HasName("PK__Elemento__4ED15E2D75A278F5");

            entity.ToTable("ElementoArteSacro");

            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.Seccion).HasMaxLength(50);
            entity.Property(e => e.Subtipo).HasMaxLength(50);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithMany(p => p.ElementoArteSacros)
                .HasForeignKey(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ElementoA__IdIde__787EE5A0");
        });

        modelBuilder.Entity<Entrepiso>(entity =>
        {
            entity.HasKey(e => e.IdEntrepisos).HasName("PK__Entrepis__3E8E4FCD2E1BDC42");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Entrepisos_IdIdentificacion").IsUnique();

            entity.Property(e => e.LosaMaciza).HasDefaultValue(false);
            entity.Property(e => e.LosaReticular).HasDefaultValue(false);
            entity.Property(e => e.NoAplica).HasDefaultValue(false);
            entity.Property(e => e.OtrosEntrepisos).HasMaxLength(200);
            entity.Property(e => e.ViguetaBovedilla).HasDefaultValue(false);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.Entrepiso)
                .HasForeignKey<Entrepiso>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Entrepiso__IdIde__33D4B598");
        });

        modelBuilder.Entity<Estructura>(entity =>
        {
            entity.HasKey(e => e.IdEstructura).HasName("PK__Estructu__5A11D9DE22AA2996");

            entity.ToTable("Estructura");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Estructura_IdIdentificacion").IsUnique();

            entity.Property(e => e.ColumnaConcreto).HasDefaultValue(false);
            entity.Property(e => e.ColumnaPiedra).HasDefaultValue(false);
            entity.Property(e => e.MuroAdobe).HasDefaultValue(false);
            entity.Property(e => e.MuroBahareque).HasDefaultValue(false);
            entity.Property(e => e.MuroBlock).HasDefaultValue(false);
            entity.Property(e => e.MuroLadrillo).HasDefaultValue(false);
            entity.Property(e => e.MuroPiedra).HasDefaultValue(false);
            entity.Property(e => e.OtroTipoColumna).HasMaxLength(200);
            entity.Property(e => e.OtroTipoMuro).HasMaxLength(200);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.Estructura)
                .HasForeignKey<Estructura>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Estructur__IdIde__2B3F6F97");
        });

        modelBuilder.Entity<Identificacion>(entity =>
        {
            entity.HasKey(e => e.IdIdentificacion).HasName("PK__Identifi__D0E8A53D0F975522");

            entity.ToTable("Identificacion");

            entity.Property(e => e.Calle).HasMaxLength(100);
            entity.Property(e => e.CodigoPostal).HasMaxLength(10);
            entity.Property(e => e.Colonia).HasMaxLength(100);
            entity.Property(e => e.ConstruccionesColindantes).HasMaxLength(200);
            entity.Property(e => e.CoordenadaX).HasColumnType("decimal(10, 7)");
            entity.Property(e => e.CoordenadaY).HasColumnType("decimal(10, 7)");
            entity.Property(e => e.CorreoElectronico).HasMaxLength(100);
            entity.Property(e => e.EntreCalles).HasMaxLength(150);
            entity.Property(e => e.Fondo).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Frente).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.M2construccion)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("M2Construccion");
            entity.Property(e => e.M2terreno)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("M2Terreno");
            entity.Property(e => e.Municipio).HasMaxLength(100);
            entity.Property(e => e.NombreParroquia).HasMaxLength(150);
            entity.Property(e => e.NombreTitular).HasMaxLength(100);
            entity.Property(e => e.NumeroEdificio).HasMaxLength(20);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.UbicacionManzana).HasMaxLength(20);
        });

        modelBuilder.Entity<Instalacione>(entity =>
        {
            entity.HasKey(e => e.IdInstalacion).HasName("PK_Instalac_03A6C0396C190EBB");

            entity.Property(e => e.ObservacionesElectrica).HasMaxLength(300);
            entity.Property(e => e.ObservacionesGas).HasMaxLength(300);
            entity.Property(e => e.ObservacionesHidraulica).HasMaxLength(300);
            entity.Property(e => e.ObservacionesOtro).HasMaxLength(300);
            entity.Property(e => e.ObservacionesSanitaria).HasMaxLength(300);
            entity.Property(e => e.ObservacionesTelefonia).HasMaxLength(300);

            entity.HasOne(d => d.IdIdentificacionNavigation)
                .WithOne(p => p.Instalacione)
                .HasForeignKey<Instalacione>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InstalaciIdIde_6E01572D");

        });

        modelBuilder.Entity<PlanimetriaExistente>(entity =>
        {
            entity.HasKey(e => e.IdPlanimetria).HasName("PK__Planimet__B55A0E2C0E6E26BF");

            entity.ToTable("PlanimetriaExistente");

            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.RutaArchivo).HasMaxLength(300);
            entity.Property(e => e.TipoPlanimetria).HasMaxLength(100);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithMany(p => p.PlanimetriaExistentes)
                .HasForeignKey(d => d.IdIdentificacion)
                .HasConstraintName("FK_Planimetria_Identificacion");
        });

        modelBuilder.Entity<Puerta>(entity =>
        {
            entity.HasKey(e => e.IdPuerta).HasName("PK__Puertas__4A275A435CD6CB2B");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Puertas_IdIdentificacion").IsUnique();

            entity.Property(e => e.Aluminio).HasDefaultValue(false);
            entity.Property(e => e.Herreria).HasDefaultValue(false);
            entity.Property(e => e.Madera).HasDefaultValue(false);
            entity.Property(e => e.OtrosMateriales).HasMaxLength(200);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.Puerta)
                .HasForeignKey<Puerta>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puertas__IdIdent__619B8048");
        });

        modelBuilder.Entity<RegistroTrabajo>(entity =>
        {
            entity.HasKey(e => e.IdTrabajo).HasName("PK__Registro__4FB29E3409A971A2");

            entity.ToTable("RegistroTrabajo");

            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.EmpresaResponsable).HasMaxLength(200);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.TipoRegistro).HasMaxLength(100);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithMany(p => p.RegistroTrabajos)
                .HasForeignKey(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Trabajo_Identificacion");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C7F60ED59");

            entity.HasIndex(e => e.NombreRol, "UQ__Roles__4F0B537F023D5A04").IsUnique();

            entity.Property(e => e.NombreRol).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97060DEAE8");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D1053408EA5793").IsUnique();

            entity.Property(e => e.Autorizado).HasDefaultValue(false);
            entity.Property(e => e.Contraseña).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuarios__IdRol__0AD2A005");
        });

        modelBuilder.Entity<Ventana>(entity =>
        {
            entity.HasKey(e => e.IdVentana).HasName("PK__Ventanas__D16E86096477ECF3");

            entity.HasIndex(e => e.IdIdentificacion, "IX_Ventanas_IdIdentificacion").IsUnique();

            entity.Property(e => e.Aluminio).HasDefaultValue(false);
            entity.Property(e => e.Herreria).HasDefaultValue(false);
            entity.Property(e => e.Madera).HasDefaultValue(false);
            entity.Property(e => e.OtrosMateriales).HasMaxLength(200);

            entity.HasOne(d => d.IdIdentificacionNavigation).WithOne(p => p.Ventana)
                .HasForeignKey<Ventana>(d => d.IdIdentificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ventanas__IdIden__693CA210");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
