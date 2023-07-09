using System.Collections;
using System.Data;
using System.Reflection;
using TableFox.Kernel.Attributes;
using TableFox.Kernel.Extensions;

namespace TableFox.Kernel.Mapper
{
    /// <summary>
    /// MapperCollection is a class that maps a DataRow[] to a List of objects.
    /// </summary>
    public static class MapperCollection
    {
        /// <summary>
        /// Invokes the mapping.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<object> Invoke(DataRow[] rows, Type type)
        {
            var propertiesOfMain = type.GetProperties();

            var propPk = propertiesOfMain.FirstOrDefault(
                prop => Attribute.IsDefined(prop, typeof(PrimaryKey)));

            Dictionary<object, DataRow[]> group = rows
                            .GroupBy(g => g.GetValue(propPk.Name, propPk.PropertyType))
                            .ToDictionary(x => x.Key, v => v.ToArray());

            var main = group
                .Select(member => member.Value.First())
                .ToArray();

            var t = new List<object>();
            var f = new Dictionary<object, DataRow>();
            for (int i = 0; i < main.Length; i++)
            {
                DataRow? row = main[i];
                t.Add(MapRow(rows, row, type, null));
            }

            return t;
        }

        /// <summary>
        /// Creates a list of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IList CreateList(Type type)
        {
            Type genericListType = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericListType);
        }

       /// <summary>
       /// Maps a DataRow to an object.
       /// </summary>
       /// <param name="source"></param>
       /// <param name="row"></param>
       /// <param name="typeParam"></param>
       /// <param name="pks"></param>
       /// <returns></returns>
        private static object MapRow(DataRow[]? source, DataRow row, Type typeParam, Dictionary<string, object> pks)
        {
            if (pks == null)
                pks = new Dictionary<string, object>();

            var result = Activator.CreateInstance(typeParam);
            var props = result.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                var type = Nullable.GetUnderlyingType(props[i].PropertyType) ?? props[i].PropertyType;

                if (row.Table.Columns.Contains(props[i].Name))
                {
                    if (!type.IsCollection())
                    {
                        var value = ConvertCell(row[props[i].Name], type);
                        props[i].SetValue(result, value);

                        if(IsPrimeryKey(props[i]))
                            pks[props[i].Name] = value;
                    }
                }
                else
                {
                    if (type.IsClass && type != typeof(string) && source != null)
                    {
                        if (type.IsCollection())
                        {

                            List<DataRow> tmp = source.ToList();
                            foreach(var pk in pks)
                                tmp = tmp.Where(w => w.Field<object>(pk.Key).ToString() == pk.Value.ToString()).ToList();


                            PropertyInfo propertyChild = props[i];
                            Type typeChild = propertyChild.PropertyType;
                            Type itemType = typeChild.GetGenericArguments()[0];
                            var resLines = CreateList(itemType);

                            foreach (var re in tmp)
                            {
                                var fg = MapRow(source, re, itemType, pks);
                                resLines.Add(fg);
                            }


                            propertyChild.SetValue(result, resLines, null);
                        }
                        else
                        {
                            props[i].SetValue(result, MapRow(source, row, type, pks));
                        }

                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Indicates if the given property is a primary key.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static bool IsPrimeryKey(PropertyInfo prop)
        {
            return Attribute.IsDefined(prop, typeof(PrimaryKey));
        }

        /// <summary>
        /// Indicates if the given property is a foreign key.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsAnyNull(object source, Type type)
        {
            return type == typeof(DBNull) || source == null || source == DBNull.Value;
        }

        /// <summary>
        /// Converts a cell to the given type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object ConvertCell(object source, Type type)
        {
            if (IsAnyNull(source, type))
                return GetDefault(type);
            else if (type == typeof(string))
                return source.ToString();
            else if (type == typeof(int))
                return Convert.ToInt32(source.ToString());
            else if (type == typeof(bool))
                return ConvertToBool(source);
            else if (type == typeof(DateTime))
                return Convert.ToDateTime(source.ToString());
            else if (type == typeof(double))
                return Convert.ToDouble(source.ToString());
            else if (type == typeof(decimal))
                return Convert.ToDecimal(source.ToString());
            else
                return source;
        }

        /// <summary>
        /// Converts a cell to a boolean.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static bool ConvertToBool(object source)
        {
            if (bool.TryParse(source.ToString(), out bool boolValue))
                return boolValue;
            else
                return source.ToString().ToLower() == "y" || source.ToString().ToLower() == "s";
        }

        /// <summary>
        /// Gets the default value of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }
    }
}
