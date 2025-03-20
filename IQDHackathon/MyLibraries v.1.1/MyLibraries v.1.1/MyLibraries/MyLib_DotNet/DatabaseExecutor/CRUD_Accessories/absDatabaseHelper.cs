using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyLib_DotNet.DatabaseExecutor.CRUD_Accessories
{
    public abstract class absDatabaseHelper
    {
        protected static readonly string _sourceName;
        protected static readonly string _connectionString;

        public delegate void ErrorLoggedEventHandler(string message);
        public static event ErrorLoggedEventHandler? OnErrorLogged;

        private static readonly HashSet<int> __transientErrorNumbers = new HashSet<int>
        {
            -2, 1205, 4060, 10928, 10929, 40197, 40501, 40613, 233, 64, 18401, 18456, 20, 17197, 11001, 10053, 10054, 10060
        };

        static absDatabaseHelper()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                _sourceName = configuration["DataSource"] ?? string.Empty;
                if (string.IsNullOrWhiteSpace(_sourceName))
                    throw new Exception("The 'DataSource' setting is missing or empty in the configuration file.");

                _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
                if (string.IsNullOrWhiteSpace(_connectionString))
                    throw new Exception("The 'DefaultConnection' setting is missing or empty in the configuration file.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while loading configuration settings: {ex.Message}");
            }
        }

        protected static T? _ExecuteWithRetry<T>(Func<T> operation, byte retryAttempts, ushort retryDelayMilliseconds, string operationName)
        {
            byte attempt = 0;
            while (attempt < retryAttempts)
            {
                try
                {
                    return operation();
                }
                catch (SqlException ex) when (__IsTransientError(ex))
                {
                    attempt++;
                    _LogEvent($"Transient SQL error in '{operationName}', attempt {attempt}: {ex.Message}");
                    if (attempt >= retryAttempts)
                        return default;
                    Thread.Sleep(retryDelayMilliseconds);
                }
                catch (TimeoutException ex)
                {
                    attempt++;
                    _LogEvent($"Timeout in '{operationName}', attempt {attempt}: {ex.Message}");
                    if (attempt >= retryAttempts)
                        return default;
                    Thread.Sleep(retryDelayMilliseconds);
                }
                catch (Exception ex)
                {
                    _LogEvent($"Unexpected error in '{operationName}': {ex.Message}");
                    throw;
                }
            }
            return default;
        }

        protected static async Task<T?> _ExecuteWithRetryAsync<T>(Func<Task<T>> operation, byte retryAttempts, ushort retryDelayMilliseconds, string operationName)
        {
            byte attempt = 0;
            while (attempt < retryAttempts)
            {
                try
                {
                    return await operation();
                }
                catch (SqlException ex) when (__IsTransientError(ex))
                {
                    attempt++;
                    _LogEvent($"Transient SQL error in '{operationName}', attempt {attempt}: {ex.Message}");
                    if (attempt >= retryAttempts)
                        return default;
                    await Task.Delay(retryDelayMilliseconds);
                }
                catch (TimeoutException ex)
                {
                    attempt++;
                    _LogEvent($"Timeout in '{operationName}', attempt {attempt}: {ex.Message}");
                    if (attempt >= retryAttempts)
                        return default;
                    await Task.Delay(retryDelayMilliseconds);
                }
                catch (Exception ex)
                {
                    _LogEvent($"Unexpected error in '{operationName}': {ex.Message}");
                    throw;
                }
            }
            return default;
        }

        protected static void _LogEvent(string message)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    if (!EventLog.SourceExists(_sourceName))
                    {
                        EventLog.CreateEventSource(_sourceName, "Application");
                    }
                    EventLog.WriteEntry(_sourceName, message, EventLogEntryType.Error);
                }
                catch { /* تجاهل الأخطاء */ }
            }
            else
            {
                Console.WriteLine($"[ERROR] {message}");
            }
            OnErrorLogged?.Invoke(message);
            __LogErrorInConsole(message);
        }

        private static bool __IsTransientError(SqlException ex)
            => ex.Errors?.Count > 0 && ex.Errors.Cast<SqlError>().Any(error => __transientErrorNumbers.Contains(error.Number));

        [Conditional("DEBUG")]
        private static void __LogErrorInConsole(string message) => Console.WriteLine(message);

        protected static int? _ConvertToIntOrDBNull(object result) => result != null ? Convert.ToInt32(result) : null;

        protected static bool _IsSuccessfulOperation(int? result) => result != null && result > 0;

        protected static List<string> _ExtractColumnNames(List<Dictionary<string, object>> records) => records.First().Keys.ToList();

        protected static DataTable _ConvertRecordsToDataTable(List<Dictionary<string, object>> records)
        {
            DataTable dataTable = new DataTable();
            var columnNames = _ExtractColumnNames(records);
            foreach (var column in columnNames)
                dataTable.Columns.Add(column, records.First()[column]?.GetType() ?? typeof(object));

            foreach (var record in records)
            {
                var row = dataTable.NewRow();
                foreach (var column in columnNames)
                    row[column] = record[column] ?? DBNull.Value;
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        protected static SqlParameter[] _CreateSqlParameterArray(string columnName, object columnValue, string? userColumnName = null, int? userId = null)
        {
            __ValidateColumnName(columnName);
            __ValidateColumnValue(columnValue);

            var parameters = new List<SqlParameter> { new SqlParameter($"@{columnName}", columnValue) };

            if (userId.HasValue && !string.IsNullOrEmpty(userColumnName))
                parameters.Add(new SqlParameter($"@{userColumnName}", userId));

            return parameters.ToArray();
        }

        protected static List<SqlParameter> _GenerateSqlParametersFromDictionary(Dictionary<string, object> updateValues, string? userColumnName = null, int? userId = null)
        {
            var parameters = updateValues.Select(kv => new SqlParameter($"@{kv.Key}", kv.Value ?? DBNull.Value)).ToList();

            if (userId.HasValue && !string.IsNullOrEmpty(userColumnName))
                parameters.Add(new SqlParameter($"@{userColumnName}", userId));

            return parameters;
        }

        protected static List<SqlParameter> _GenerateSqlParametersForMultipleRecords(List<Dictionary<string, object>> records, string? userColumnName = null, int? userId = null)
        {
            var columnNames = _ExtractColumnNames(records);
            List<SqlParameter> parameters = new List<SqlParameter>();

            for (int i = 0; i < records.Count; i++)
            {
                foreach (var column in columnNames)
                    parameters.Add(new SqlParameter($"@{column}{i}", records[i][column] ?? DBNull.Value));
            }
            if (userId.HasValue && !string.IsNullOrEmpty(userColumnName))
                parameters.Add(new SqlParameter($"@{userColumnName}", userId));

            return parameters;
        }

        protected static SqlParameter _CreateStructuredSqlParameter(string columnName, List<int> ids)
        {
            __ValidateColumnName(columnName);
            __ValidateListIds(ids);
            DataTable valuesTable = new DataTable();
            valuesTable.Columns.Add(columnName, typeof(int));
            foreach (var value in ids)
                valuesTable.Rows.Add(value);

            var parameter = new SqlParameter("@Ids", SqlDbType.Structured)
            {
                TypeName = $"{columnName}TableType",
                Value = valuesTable
            };
            return parameter;
        }

        protected static SqlParameter _CreateStructuredSqlParameterFromDictionary(string tableName, List<Dictionary<string, object>> records, string? userColumnName = null, int? userId = null)
        {
            __ValidateTableName(tableName);
            __ValidateListDictionares(records);
            DataTable dataTable = _ConvertRecordsToDataTable(records);

            var parameter = new SqlParameter("@Records", SqlDbType.Structured)
            {
                TypeName = $"{tableName}TableType",
                Value = dataTable
            };
            return parameter;
        }

        protected static string _GenerateDeleteQuery(string tableName, string columnName, List<int> ids)
        {
            __ValidateTableName(tableName);
            __ValidateColumnName(columnName);
            __ValidateListIds(ids);
            return $"DELETE FROM {tableName} WHERE {columnName} IN ({string.Join(",", ids)})";
        }

        protected static string _GenerateBulkInsertQuery(string tableName, List<Dictionary<string, object>> records)
        {
            var columnNames = _ExtractColumnNames(records);
            string columns = string.Join(", ", columnNames);
            string values = string.Join(", ", records.Select((r, i) => $"({string.Join(", ", columnNames.Select(c => $"@{c}{i}"))})"));
            return $"INSERT INTO {tableName} ({columns}) VALUES {values}";
        }

        protected static string _GenerateBulkUpdateQuery(string tableName, Dictionary<string, object> updateValues, string columnName, List<int> ids)
        {
            string setClause = string.Join(", ", updateValues.Select(kv => $"{kv.Key} = @{kv.Key}"));
            return $"UPDATE {tableName} SET {setClause} WHERE {columnName} IN ({string.Join(",", ids)})";
        }

        //protected static List<SqlParameter> _GenerateSqlParametersForMultipleRecords(List<Dictionary<string, object>> records, int? userId = null, string? userColumnName = null)
        //{
        //    var columnNames = _ExtractColumnNames(records);
        //    List<SqlParameter> parameters = new List<SqlParameter>();

        //    for (int i = 0; i < records.Count; i++)
        //    {
        //        foreach (var column in columnNames)
        //            parameters.Add(new SqlParameter($"@{column}{i}", records[i][column] ?? DBNull.Value));
        //    }
        //    return parameters;
        //}

        //protected static List<SqlParameter> _GenerateSqlParametersFromDictionary(Dictionary<string, object> updateValues, int? userId = null, string? userColumnName = null)
        //    => updateValues.Select(kv => new SqlParameter($"@{kv.Key}", kv.Value ?? DBNull.Value)).ToList();

        [Conditional("DEBUG")]
        private static void __ValidateListDictionares(List<Dictionary<string, object>> records)
        {
            if (records == null || records.Count == 0)
                throw new ArgumentException("Records list cannot be null or empty.");
        }

        [Conditional("DEBUG")]
        protected static void _ValidateDictionary(Dictionary<string, object> updateValues)
        {
            if (updateValues == null || updateValues.Count == 0)
                throw new ArgumentException("Update values cannot be null or empty.");
        }

        [Conditional("DEBUG")]
        private static void __ValidateListIds(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                throw new ArgumentException("ID list cannot be null or empty.");
        }

        [Conditional("DEBUG")]
        protected static void _ValidateQuery(string query) => __ValidateInput(query, "Query cannot be null or empty.");


        [Conditional("DEBUG")]
        private static void __ValidateTableName(string tableName) => __ValidateInput(tableName, "Table name cannot be null or empty.");

        [Conditional("DEBUG")]
        private static void __ValidateColumnName(string columnName) => __ValidateInput(columnName, "Column name cannot be null or empty.");

        [Conditional("DEBUG")]
        private static void __ValidateColumnValue(object columnValue)
        {
            if (columnValue == null ||
                columnValue is string str && string.IsNullOrWhiteSpace(str) ||
                columnValue is ICollection collection && collection.Count == 0)
            {
                throw new ArgumentException($"The value for column is invalid or empty.");
            }
        }

        [Conditional("DEBUG")]
        private static void __ValidateInput(string s, string messag)
        {
            if (!string.IsNullOrWhiteSpace(s) && string.IsNullOrWhiteSpace(s))
                throw new ArgumentException(messag);
        }
    }
}
