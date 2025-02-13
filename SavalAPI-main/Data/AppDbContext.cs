using Microsoft.EntityFrameworkCore;
using SavalAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SavalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets para todas las entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Encuestado> Encuestados { get; set; }

        public DbSet<Formulario> Formularios { get; set; }
    

        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<FormularioPregunta> FormulariosPreguntas { get; set; }
        public DbSet<OpcionRespuesta> OpcionesRespuestas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<DetalleRespuesta> DetallesRespuestas { get; set; }
        public DbSet<Recomendacion> Recomendaciones { get; set; }
        public DbSet<FactorRiesgo> FactoresRiesgo { get; set; }
        public DbSet<ReglaOpcion> ReglasOpciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tabla Usuario y Rol
            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.IdUsuario);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany()
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rol>()
                .HasKey(r => r.IdRol);

            // Tabla Formulario
            modelBuilder.Entity<Formulario>()
                .HasKey(f => f.IdFormulario);

            // Tabla Pregunta
            modelBuilder.Entity<Pregunta>()
                .HasKey(p => p.IdPregunta);

            // Relación PreguntaFormulario (muchos a muchos)
            modelBuilder.Entity<FormularioPregunta>()
                .HasKey(fp => fp.IdFormularioPregunta);

            modelBuilder.Entity<FormularioPregunta>()
                .HasOne(fp => fp.Formulario)
                .WithMany(f => f.Preguntas)
                .HasForeignKey(fp => fp.IdFormulario);

            modelBuilder.Entity<FormularioPregunta>()
                .HasOne(fp => fp.Pregunta)
                .WithMany(p => p.Formularios)
                .HasForeignKey(fp => fp.IdPregunta);

            // Tabla OpcionRespuesta
            modelBuilder.Entity<OpcionRespuesta>()
                .HasKey(o => o.IdOpcion);

            modelBuilder.Entity<OpcionRespuesta>()
                .HasOne(o => o.Pregunta)
                .WithMany(p => p.Opciones)
                .HasForeignKey(o => o.IdPregunta);

            // Tabla Respuesta
            modelBuilder.Entity<Respuesta>()
                .HasKey(r => r.IdRespuesta);

            modelBuilder.Entity<Respuesta>()
                .HasOne(r => r.Formulario)
                .WithMany()
                .HasForeignKey(r => r.IdFormulario);
            modelBuilder.Entity<Respuesta>()
            .HasOne(r => r.Encuestado)
            .WithMany()
            .HasForeignKey(r => r.IdentificacionEncuestado)
            .OnDelete(DeleteBehavior.SetNull);

            // Tabla DetalleRespuesta

            modelBuilder.Entity<DetalleRespuesta>()
                .HasKey(dr => dr.IdDetalleRespuesta);

            modelBuilder.Entity<DetalleRespuesta>()
                .HasOne(dr => dr.Respuesta)
                .WithMany(r => r.Detalles)
                .HasForeignKey(dr => dr.IdRespuesta)
                .OnDelete(DeleteBehavior.Restrict); // Restringe la eliminación en cascada

            modelBuilder.Entity<DetalleRespuesta>()
                .HasOne(dr => dr.Pregunta)
                .WithMany()
                .HasForeignKey(dr => dr.IdPregunta)
                .OnDelete(DeleteBehavior.Restrict); // Restringe la eliminación en cascada

            modelBuilder.Entity<DetalleRespuesta>()
                .HasOne(dr => dr.Opcion)
                .WithMany()
                .HasForeignKey(dr => dr.IdOpcion)
                .OnDelete(DeleteBehavior.Restrict); // Restringe la eliminación en cascada


            // Tabla Recomendacion
            modelBuilder.Entity<Recomendacion>()
                .HasKey(r => r.IdRecomendacion);

            // Tabla FactorRiesgo
            modelBuilder.Entity<FactorRiesgo>()
                .HasKey(f => f.IdFactor);

            // Tabla ReglaOpcion
            modelBuilder.Entity<ReglaOpcion>()
                .HasKey(ro => ro.IdRegla);

            modelBuilder.Entity<ReglaOpcion>()
                .HasOne(ro => ro.Opcion)
                .WithMany()
                .HasForeignKey(ro => ro.IdOpcion);

            modelBuilder.Entity<ReglaOpcion>()
                .HasOne(ro => ro.Recomendacion)
                .WithMany()
                .HasForeignKey(ro => ro.IdRecomendacion)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ReglaOpcion>()
                .HasOne(ro => ro.FactorRiesgo)
                .WithMany()
                .HasForeignKey(ro => ro.IdFactorRiesgo)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Encuestado>()
            .HasKey(e => e.Identificacion); // PK de la tabla

           
        }

    }
}
