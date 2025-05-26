using Microsoft.EntityFrameworkCore;

namespace SurveyApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Form> Forms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Form - Question
            modelBuilder.Entity<Form>()
                .HasMany(f => f.Questions)
                .WithOne(q => q.Form)
                .HasForeignKey(q => q.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // Form - Response
            modelBuilder.Entity<Form>()
                .HasMany(f => f.Responses)
                .WithOne(r => r.Form)
                .HasForeignKey(r => r.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question - Choice
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Choices)
                .WithOne(c => c.Question)
                .HasForeignKey(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question - Answer
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // << Ubah dari Cascade

            // Response - Answer
            modelBuilder.Entity<Response>()
                .HasMany(r => r.Answers)
                .WithOne(a => a.Response)
                .HasForeignKey(a => a.ResponseId)
                .OnDelete(DeleteBehavior.Restrict); // << Ubah dari Cascade

            // Answer - Choice (optional)
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Choice)
                .WithMany()
                .HasForeignKey(a => a.ChoiceId)
                .OnDelete(DeleteBehavior.Restrict); // Tetap restrict
        }
    }
}
