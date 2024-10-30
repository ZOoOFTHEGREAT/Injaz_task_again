namespace Injaz_task_again.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public ICollection<Question>? Questions { get; set; }
    }
}