namespace akvelon_test
{
    public static class Utills
    {
        /// <summary>
        /// Merge two obbjects of one type
        /// In case of merging Projects, TaskItems array is ignored
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="t">destination object</param>
        /// <param name="u">source object</param>
        internal static void Merge<T>(ref T t, T u) where T: class
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.GetValue(u) != null && prop.Name != "Id" && prop.Name != "TaskItems")
                {
                    prop.SetValue(t, prop.GetValue(u));
                }
            }
        }
    }
}
