namespace Authorization
{
    public static class JWTTokenCache
    {
        private static HashSet<string>? cache;

        private static object cacheLock = new object();
        public static HashSet<string> AppCache
        {
            get
            {
                lock (cacheLock)
                {
                    if (cache == null)
                    {
                        cache = new HashSet<string>();
                    }
                    return cache;
                }
            }
        }
    }
}
