namespace SignalToAnswer.Constants
{
    public static class GameStatus
    {
        public static readonly int WAITING_FOR_PLAYERS_TO_CONNECT = 1;
        public static readonly int WAITING_FOR_PLAYERS_TO_ACCEPT_INVITE = 2;
        public static readonly int PLAYERS_CONNECTED = 3;
        public static readonly int GAME_IN_PROGRESS = 4;
        public static readonly int GAME_END = 5;
        public static readonly int PLAYER_DISCONNECTED = 6;
        public static readonly int PLAYER_DISCONNECTED_DURING_GAME = 7;
        public static readonly int CANCELLED = 8;
        public static readonly int WAITING_FOR_PLAYERS_TO_REPLAY = 9;
        public static readonly int REPLAYING = 10;
        public static readonly int REPLAY_CANCELLED = 11;
    }
}
