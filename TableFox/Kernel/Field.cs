namespace TableFox.Kernel
{
    /// <summary>
    /// Field is a class that represents a KeyValuePair.
    /// </summary>
    public static class Field
    {
        /// <summary>
        /// Creates a KeyValuePair.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static KeyValuePair<string, object> Of(string key, object value)
        {
            return new KeyValuePair<string, object>(key, value);
        }
    }
}
