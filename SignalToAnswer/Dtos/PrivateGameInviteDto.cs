namespace SignalToAnswer.Dtos
{
    public class PrivateGameInviteDto
    {
        public string FromUser { get; set; }

        public int GameId { get; set; }

        public int GroupId { get; set; }

        public PrivateGameInviteDto(string fromUser, int gameId, int groupId)
        {
            FromUser = fromUser;
            GameId = gameId;
            GroupId = groupId;
        }
    }
}
