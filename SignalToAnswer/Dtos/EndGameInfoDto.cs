namespace SignalToAnswer.Dtos
{
    public class EndGameInfoDto
    {
        public int GameType { get; set; }

        public EndGameInfoDto(int gameType)
        {
            GameType = gameType;
        }
    }
}
