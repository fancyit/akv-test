namespace akvelon_test
{
    public static class Utills
    {
        internal static void Merge<T>(ref T t, T u)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.GetValue(u) != null && prop.Name != "Id")
                {
                    prop.SetValue(t, prop.GetValue(u));
                }
            }
        }
    }
}
