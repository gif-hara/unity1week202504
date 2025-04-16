namespace unity1week202504
{
    public static class Define
    {
        public enum DanceType
        {
            Default = 0,
            Left = 1,
            Right = 2,
            Up = 3,
            Down = 4,
            Fail = 5,
        }

        public enum GameState
        {
            Initialize = 0,
            InGame = 1,
            Lose = 2,
            Win = 3,
        }
    }
}
