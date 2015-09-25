using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace CurrencyLoader.DataAccess
{
    public class DynamicSqlOrm : DynamicObject
    {
        private readonly string _connectonString;

        private const string UpsertQuery = "DECLARE @ratesCount AS INTEGER SELECT @ratesCount = Count(1) FROM {0} {1} " + "IF(@ratesCount <> 0) " + "BEGIN " + "	{2} {1} " + "END " + "ELSE " + "BEGIN " + "  {3} " + "END";
        private const string SelectQuery = "SELECT * FROM {0} {1}";

        public DynamicSqlOrm()
        {
            _connectonString = Settings.Default.connectionString;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder,
            object[] args, out object result)
        {
            var name = binder.Name.Replace("Upsert", string.Empty);
            var collapsed = args[0].Collapse();
            var whereCondition = binder.ToWhereStatement();
            if (binder.Name.EndsWith("Upsert"))
            {
                var insert = collapsed.ToInsertStatement(name);
                var update = collapsed.ToUpdateStatement(name);
                var query = string.Format(UpsertQuery, name, whereCondition, update, insert);
                result = ExecuteQuery(query, collapsed.Values.ToArray());
            }
            else
            {
                var query = string.Format(SelectQuery,name, whereCondition);
                result = Read(query, collapsed.Values.ToArray());    
            }
            
            return true;
        }

        private object ExecuteQuery(string commandText, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectonString))
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }


        private ICollection<dynamic> Read(string commandText,
            params SqlParameter[] parameters)
        {
            ICollection<dynamic> expandos = new List<dynamic>();
            using (var connection = new SqlConnection(_connectonString))
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddRange(parameters);
                    var reader = command.ExecuteReader();
                    var columns = Enumerable.Range(0, reader.FieldCount)
                        .Select(reader.GetName);
                    while (reader.Read())
                    {
                        dynamic expando = new ExpandoObject();
                        IDictionary<string, object> data = expando;
                        foreach (var column in columns)
                        {
                            data.Add(column, reader[column]);
                        }
                        expandos.Add(expando);
                    }
                }
                return expandos;
            }
        }
        
    }
}
