using System.Data;

namespace TableFox.Kernel.Extensions
{
    /// <summary>
    /// DataRowExtension is a class that extends the DataRow class.
    /// </summary>
    public static class DataRowExtension
    {
        /// <summary>
        /// Tries to get the value of a field in a DataRow.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<T>(this DataRow row, string columnName, out T? value)
        {
            if (row == null)
            {
                value = default;
                return false;
            }

            value = row.Field<T>(columnName: columnName);
            return value != null;
        }

        /// <summary>
        /// Gets the value of a field in a DataRow.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static object GetValue(this DataRow row, string columnName, Type type)
        {
            if (row == null)
                throw new NullReferenceException("Row is null");
            if (columnName == null)
                throw new NullReferenceException("columnName is null");

            var value = row.Field<object>(columnName: columnName);

            if (value == null)
                throw new NullReferenceException($"Failed to retrieve field with column name {columnName}");

            return Convert.ChangeType(value, type); ;
        }

        /// <summary>
        /// Sets the value of a field in a DataRow.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static DataRow SetValue<T>(this DataRow row, string columnName, T? value)
        {
            if (row == null)
                throw new NullReferenceException("Row is null");

            row.SetField(columnName: columnName, value: value);
            return row;
        }


    }
}
