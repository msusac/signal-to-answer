namespace SignalToAnswer.Constants
{
    public static class GameStatus
    {
        public static readonly int WAITING_FOR_PLAYERS_TO_CONNECT = 1;
        public static readonly int WAITING_FOR_PLAYERS_TO_ACCEPT_INVITE = 2;
        public static readonly int PLAYERS_CONNECTED = 3;
        public static readonly int MATCH_CREATED = 4;
        public static readonly int QUESTIONS_CREATED = 5;
        public static readonly int RESULT_BOARD_CREATED = 6;
        public static readonly int READY_FOR_GAME = 7;
        public static readonly int GAME_IN_PROGRESS = 8;
        public static readonly int GAME_END = 9;
        public static readonly int PLAYER_DISCONNECTED = 10;
        public static readonly int CANCELLED = 11;
    }
}
