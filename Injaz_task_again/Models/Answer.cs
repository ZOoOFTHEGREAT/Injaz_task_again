namespace Injaz_task_again.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public int SelectionCount { get; set; } = 0;
        public int QuestionId { get; set; }
        public Question? Question { get; set; }

    }
}
