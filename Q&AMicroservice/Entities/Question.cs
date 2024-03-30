namespace Q_AMicroservice.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Answered { get; set; }
        public int DoctorId { get; set; }
    }
}
