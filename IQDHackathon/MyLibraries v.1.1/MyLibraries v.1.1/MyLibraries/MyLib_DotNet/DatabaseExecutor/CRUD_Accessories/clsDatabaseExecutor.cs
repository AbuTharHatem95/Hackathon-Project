using System.Data;
using Microsoft.Data.SqlClient;

namespace MyLib_DotNet.DatabaseExecutor.CRUD_Accessories
{
    public partial class clsDatabaseExecutor : absDatabaseHelper
    {
        protected static int? _ExecuteNonQuery(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteWithRetry(() => __ExecuteCommand(query, type, parameters, cmd => cmd.ExecuteNonQuery()), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteNonQuery));

        protected static object? _ExecuteScalar(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteWithRetry(() => __ExecuteCommand(query, type, parameters, cmd => cmd.ExecuteScalar()), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteScalar));
        
        protected static Dictionary<string, object>? _ExecuteSingleRecord(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                return __ExecuteCommand(query, type, parameters, cmd =>
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                                row[columnName] = value;
                            }
                            return row;
                        }
                    }
                    return null;
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteSingleRecord));
        }

        //protected static List<object[]>? _ExecuteReader(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //{
        //    return _ExecuteWithRetry(() =>
        //    {
        //        return __ExecuteCommand(query, type, parameters, cmd =>
        //        {
        //            List<object[]> result = new List<object[]>();
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    object[] row = new object[reader.FieldCount];

        //                    for (int i = 0; i < reader.FieldCount; i++)
        //                        row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);

        //                    result.Add(row);
        //                }
        //            }
        //            return result;
        //        });
        //    }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReader));
        //}

        protected static List<Dictionary<string, object>>? _ExecuteReader(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                return __ExecuteCommand(query, type, parameters, cmd =>
                {
                    List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                                row[columnName] = value;
                            }
                            result.Add(row);
                        }
                    }
                    return result;
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReader));
        }

        //protected static List<T>? _ExecuteReader<T>(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500) //where T : class 
        //{
        //    return _ExecuteWithRetry(() =>
        //    {
        //        return __ExecuteCommand(query, type, parameters, cmd =>
        //        {
        //            List<T> result = new List<T>();
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    object[] row = new object[reader.FieldCount];
        //                    reader.GetValues(row);
        //                    result.Add(JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(row)));
        //                }
        //            }
        //            return result;
        //        });
        //    }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReader));
        //}

        protected static DataTable? _ExecuteDataAdapter(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                return __ExecuteCommand(query, type, parameters, cmd =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteDataAdapter));
        }

        protected static bool _ExecuteTransaction(List<(string query, SqlParameter[] parameters, CommandType type)> commands, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var (query, parameters, type) in commands)
                            {
                                using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, transaction))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _LogEvent($"Transaction failed: {ex.Message}");
                            throw;
                        }
                    }
                }
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteTransaction));
        }

        private static T __ExecuteCommand<T>(string query, CommandType type, SqlParameter[]? parameters, Func<SqlCommand, T> commandExecutor, SqlConnection? externalConnection = null, SqlTransaction? externalTransaction = null)
        {
            _ValidateQuery(query);
            SqlConnection connection = externalConnection ?? new SqlConnection(_connectionString);
            try
            {
                if (externalConnection == null)
                    connection.Open();
                using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, externalTransaction))
                {
                    return commandExecutor(command);
                }
            }
            finally
            {
                if (externalConnection == null)
                    connection.Dispose();
            }
        }

        private static SqlCommand __PrepareCommand(SqlConnection connection, string query, CommandType type, SqlParameter[]? parameters, SqlTransaction? transaction)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = type;
            if (transaction != null)
                command.Transaction = transaction;
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            return command;
        }
    }
    public partial class clsDatabaseExecutor
    {
        protected static async Task<int?> _ExecuteNonQueryAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteWithRetryAsync(() => __ExecuteCommandAsync(query, type, parameters, async cmd => await cmd.ExecuteNonQueryAsync()), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteNonQueryAsync));

        protected static async Task<object?> _ExecuteScalarAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteWithRetryAsync(() => __ExecuteCommandAsync(query, type, parameters, async cmd => await cmd.ExecuteScalarAsync()), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteScalarAsync));

        protected static async Task<Dictionary<string, object>?> _ExecuteSingleRecordAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return await _ExecuteWithRetryAsync(async () =>
            {
                return await __ExecuteCommandAsync(query, type, parameters, async cmd =>
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = await reader.IsDBNullAsync(i) ? DBNull.Value : reader.GetValue(i);
                                row[columnName] = value;
                            }
                            return row;
                        }
                    }
                    return null;
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteSingleRecordAsync));
        }

        //protected static async Task<List<object[]>?> _ExecuteReaderAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //{
        //    return await _ExecuteWithRetryAsync(async () =>
        //    {
        //        return await __ExecuteCommandAsync(query, type, parameters, async cmd =>
        //        {
        //            List<object[]> result = new List<object[]>();
        //            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    object[] row = new object[reader.FieldCount];

        //                    for (int i = 0; i < reader.FieldCount; i++)
        //                        row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);

        //                    result.Add(row);
        //                }
        //            }
        //            return result;
        //        });
        //    }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReaderAsync));
        //}

        protected static async Task<List<Dictionary<string, object>>?> _ExecuteReaderAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return await _ExecuteWithRetryAsync(async () =>
            {
                return await __ExecuteCommandAsync(query, type, parameters, async cmd =>
                {
                    List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = await reader.IsDBNullAsync(i) ? DBNull.Value : reader.GetValue(i);
                                row[columnName] = value;
                            }
                            result.Add(row);
                        }
                    }
                    return result;
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReaderAsync));
        }

        //protected static async Task<List<T>?> _ExecuteReaderAsync<T>(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //{
        //    return await _ExecuteWithRetryAsync(async () =>
        //    {
        //        return await __ExecuteCommandAsync(query, type, parameters, async cmd =>
        //        {
        //            List<T> result = new List<T>();
        //            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    object[] row = new object[reader.FieldCount];
        //                    reader.GetValues(row);
        //                    result.Add(JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(row)));
        //                }
        //            }
        //            return result;
        //        });
        //    }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReaderAsync));
        //}

        protected static async Task<DataTable?> _ExecuteDataAdapterAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return await _ExecuteWithRetryAsync(async () =>
            {
                return await __ExecuteCommandAsync(query, type, parameters, async cmd =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));
                        return dataTable;
                    }
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteDataAdapterAsync));
        }

        protected static async Task<bool> _ExecuteTransactionAsync(List<(string query, SqlParameter[] parameters, CommandType type)> commands, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return await _ExecuteWithRetryAsync(async () =>
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var (query, parameters, type) in commands)
                            {
                                using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, transaction))
                                {
                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _LogEvent($"Transaction failed: {ex.Message}");
                            throw;
                        }
                    }
                }
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteTransactionAsync));
        }

        private static async Task<T> __ExecuteCommandAsync<T>(string query, CommandType type, SqlParameter[]? parameters, Func<SqlCommand, Task<T>> commandExecutor, SqlConnection? externalConnection = null, SqlTransaction? externalTransaction = null)
        {
            _ValidateQuery(query);
            SqlConnection connection = externalConnection ?? new SqlConnection(_connectionString);
            try
            {
                if (externalConnection == null)
                    await connection.OpenAsync();
                using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, externalTransaction))
                {
                    return await commandExecutor(command);
                }
            }
            finally
            {
                if (externalConnection == null)
                    connection.Dispose();
            }
        }
    }
}
