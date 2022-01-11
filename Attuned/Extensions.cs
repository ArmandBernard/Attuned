using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Attuned
{
    public static class Extensions
    {
        /// <summary>
        /// Convert a list of objects to a datatable using their properties. Bear in mind this does 
        /// NOT include Fields i.e. those without getters / setters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            Type type = typeof(T);
            // get the properties that belong to this type. 
            PropertyInfo[] properties = type.GetProperties();
            
            DataTable dataTable = new DataTable();

            foreach (PropertyInfo info in properties)
            {
                // create a new column with matching name and type as the property
                dataTable.Columns.Add(new DataColumn(
                    info.Name,
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType)
                    );
            }

            // Fill the data into the DataTable
            foreach (T entity in items)
            {
                // get all property values
                object[] values = properties.Select(p => p.GetValue(entity)).ToArray();

                // create a new row using them
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
