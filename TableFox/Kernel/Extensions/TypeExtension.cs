namespace TableFox.Kernel.Extensions
{
    /// <summary>
    /// TypeExtension is a class that extends the Type class.
    /// </summary>
    internal static class TypeExtension
    {
        /// <summary>
        /// Checks if a type is a collection.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsCollection(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
