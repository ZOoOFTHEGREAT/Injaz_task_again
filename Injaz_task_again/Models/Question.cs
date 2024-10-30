namespace Injaz_task_again.Models
{
    public class Question
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public int PollId { get; set; }
        public Poll? Poll { get; set; }
        public ICollection<Answer>? Answers { get; set; }

    }
}
