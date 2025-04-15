namespace unity1week202504
{
    public static class Messages
    {
        public readonly struct Beat
        {
        }

        public readonly struct ListenDance
        {
            public Define.DanceType DanceType { get; }

            public ListenDance(Define.DanceType danceType)
            {
                DanceType = danceType;
            }
        }
    }
}
