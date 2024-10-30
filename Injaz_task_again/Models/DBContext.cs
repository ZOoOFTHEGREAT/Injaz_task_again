using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Injaz_task_again.Models
{
    public class DBContext : IdentityDbContext<IdentityUser>
    {
        public DBContext(DbContextOptions<DBContext> options): base(options)
        {
        }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Questions)
                .WithOne(q => q.Poll)
                .HasForeignKey(q => q.PollId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Answer>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Poll>().HasData(
                new Poll { Id = 1, Title = "Poll 1" },
                new Poll { Id = 2, Title = "Poll 2" }
            );

            modelBuilder.Entity<Question>().HasData(
                new Question { Id = 1, Text = "Poll 1 - Question 1", PollId = 1 },
                new Question { Id = 2, Text = "Poll 1 - Question 2", PollId = 1 },
                new Question { Id = 3, Text = "Poll 2 - Question 1", PollId = 2 },
                new Question { Id = 4, Text = "Poll 2 - Question 2", PollId = 2 }
            );

            modelBuilder.Entity<Answer>().HasData(
                // Answers for Poll 1 - Question 1
                new Answer { Id = 1, Text = "Answer 1", QuestionId = 1 },
                new Answer { Id = 2, Text = "Answer 2", QuestionId = 1 },
                // Answers for Poll 1 - Question 2
                new Answer { Id = 3, Text = "Answer 1", QuestionId = 2 },
                new Answer { Id = 4, Text = "Answer 2", QuestionId = 2 },
                // Answers for Poll 2 - Question 1
                new Answer { Id = 5, Text = "Answer 1", QuestionId = 3 },
                new Answer { Id = 6, Text = "Answer 2", QuestionId = 3 },
                // Answers for Poll 2 - Question 2
                new Answer { Id = 7, Text = "Answer 1", QuestionId = 4 },
                new Answer { Id = 8, Text = "Answer 2", QuestionId = 4 }
            );
        }

    }
}
