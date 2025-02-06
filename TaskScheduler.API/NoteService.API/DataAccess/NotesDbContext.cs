using Microsoft.EntityFrameworkCore;
using NoteService.API.Configurations;
using NoteService.API.Entities;

namespace NoteService.API.DataAccess
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new NotesConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectsConfiguration());
            modelBuilder.ApplyConfiguration(new TagProjectsConfiguration());
            modelBuilder.ApplyConfiguration(new TagNotesConfiguration());
        }

        public DbSet<NotesEntity> Notes { get; set; }
        public DbSet<ProjectsEntity> Projects { get; set; }
        public DbSet<TagProjectsEntity> TagProjects { get; set; }
        public DbSet<TagNotesEntity> TagNotes { get; set; }
    }
}