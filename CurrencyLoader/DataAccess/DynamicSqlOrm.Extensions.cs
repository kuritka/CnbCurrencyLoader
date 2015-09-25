using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace CurrencyLoader.DataAccess
{
        public static class DynamicSqlOrmExtensions
        {
            
            public static Dictionary<string, SqlParameter> Collapse(this object o)
            {
                var properties = o.GetType().GetProperties();
                return properties.ToDictionary(propertyInfo => propertyInfo.Name,
                    propertyInfo =>  new SqlParameter(propertyInfo.Name, propertyInfo.GetValue(o)));
            }

            public static string ToInsertStatement(this IDictionary<string, SqlParameter> dictionary, string tableName)
            {
                var statement = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                    tableName, string.Join(", ", dictionary.Keys), string.Join(", ", dictionary.Keys.Select(d=>"@"+d)));
                return statement;
            }


            public static string ToUpdateStatement(this IDictionary<string, SqlParameter> dictionary, string tableName)
            {
                var statement = string.Format("UPDATE {0} SET {1}",
                    tableName, string.Join(",",dictionary.Select(d=>string.Format("{0}=@{1}", d.Key, d.Key) )));
                return statement;
            }



            public static string ToWhereStatement(this InvokeMemberBinder binder)
            {
                return string.Concat(" WHERE ", string.Join(" AND ",
                    binder.CallInfo.ArgumentNames.Select(d => string.Format("{0}=@{0}", d))));
            }
        }
}
