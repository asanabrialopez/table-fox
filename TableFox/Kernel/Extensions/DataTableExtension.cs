using TableFox.Kernel.Mapper;
using System.Data;

namespace TableFox.Kernel.Extensions
{
    /// <summary>
    /// DataRowExtension is a class that extends the DataRow class.
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// Adds a row to a DataTable.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static DataTable AddRow(this DataTable dt, params KeyValuePair<string, object>[] pairs)
        {
            if (dt == null)
                throw new NullReferenceException("DataTable is null");

            dt.AddColumns(pairs.Select(s => s.Key).ToList());

            var row = dt.NewRow();

            for (int i = 0; i < pairs.Length; i++)
            {
                if (!dt.Columns.Contains(pairs[i].Key))
                    dt.Columns.Add(pairs[i].Key);
                row.SetValue(pairs[i].Key, pairs[i].Value);
            }

            dt.Rows.Add(row);
            return dt;
        }

        /// <summary>
        /// Adds a list of columns to a DataTable.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="cols"></param>
        /// <exception cref="NullReferenceException"></exception>
        public static void AddColumns(this DataTable dt, List<string> cols)
        {
            if (dt == null)
                throw new NullReferenceException("DataTable is null");

            for (int i = 0; i < cols.Count(); i++)
            {
                if (!dt.Columns.Contains(cols[i]))
                    dt.Columns.Add(cols[i]);
            }
        }

        /// <summary>
        /// Maps a DataTable to a list of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static IEnumerable<T> Map<T>(this DataTable dataTable) where T : class, new()
        {
            if (dataTable == null)
                throw new NullReferenceException("DataTable is null");

            if (dataTable.Rows == null || dataTable.Rows.Count == 0)
                throw new NullReferenceException("DataTable nos has rows");

            var result = MapperCollection.Invoke(
                rows: dataTable.Rows.Cast<DataRow>().ToArray(),
                type: typeof(T));

            return result.Cast<T>();
        }
    }
}
