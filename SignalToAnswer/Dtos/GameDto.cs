namespace SignalToAnswer.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int InviteId { get; set; }

        public string InvitedBy { get; set; }
    }
}
