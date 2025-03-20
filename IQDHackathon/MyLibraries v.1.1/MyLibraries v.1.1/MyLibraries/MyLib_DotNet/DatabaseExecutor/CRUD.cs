using Microsoft.Data.SqlClient;
using System.Data;
using MyLib_DotNet.DatabaseExecutor.CRUD_Accessories;

namespace MyLib_DotNet.DatabaseExecutor
{
    public partial class CRUD : clsDatabaseExecutor
    {
        private CRUD() { }

        public static int? Add(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ConvertToIntOrDBNull(_ExecuteScalar(query, parameters, type, retryAttempts, retryDelayMilliseconds) ?? DBNull.Value);

        public static Dictionary<string, object>? Get(string query, SqlParameter[] parameters, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteSingleRecord(query, parameters, type, retryAttempts, retryDelayMilliseconds);

        public static Dictionary<string, object>? GetByColumnValue(string query, string columnName, object columnValue, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteSingleRecord(query, _CreateSqlParameterArray(columnName, columnValue), type, retryAttempts, retryDelayMilliseconds);

        //public static object[]? Get(string query, SqlParameter[] parameters, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => _ExecuteReader(query, parameters, type, retryAttempts, retryDelayMilliseconds)?.FirstOrDefault();

        //public static object[]? GetByColumnValue(string query, string columnName, object columnValue, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => _ExecuteReader(query, _CreateSqlParameterArray(columnName, columnValue), type, retryAttempts, retryDelayMilliseconds)?.FirstOrDefault();

        //public static T GetByColumnValue<T>(string query, string columnName, object columnValue, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => _ExecuteReader<T>(query, _CreateSqlParameterArray(columnName, columnValue, userId, userColumnName), type, retryAttempts, retryDelayMilliseconds).FirstOrDefault();
        
        public static List<Dictionary<string, object>>? GetAllAsList(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteReader(query, parameters, type, retryAttempts, retryDelayMilliseconds);

        //public static List<object[]>? GetAllAsList(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => _ExecuteReader(query, parameters, type, retryAttempts, retryDelayMilliseconds) ?? null;

        //public static List<T> GetAllAsList<T>(string query, SqlParameter[] parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => _ExecuteReader<T>(query, parameters, type, retryAttempts, retryDelayMilliseconds);

        public static DataTable? GetAllAsDataTable(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteDataAdapter(query, parameters, type, retryAttempts, retryDelayMilliseconds);

        public static bool Update(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _IsSuccessfulOperation(_ExecuteNonQuery(query, parameters, type, retryAttempts, retryDelayMilliseconds));

        public static bool Delete(string query, string columnName, object columnValue, string? userColumnName = null, int? userId = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _IsSuccessfulOperation(_ExecuteNonQuery(query, _CreateSqlParameterArray(columnName, columnValue, userColumnName, userId), type, retryAttempts, retryDelayMilliseconds));

        public static bool Delete(string query, SqlParameter[]? parameters, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _IsSuccessfulOperation(_ExecuteNonQuery(query, parameters, type, retryAttempts, retryDelayMilliseconds));

        public static bool IsExist(string query, string columnName, object columnValue, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteScalar(query, _CreateSqlParameterArray(columnName, columnValue), type, retryAttempts, retryDelayMilliseconds) != null;

        public static bool ExecuteBatchOperations(List<(string query, SqlParameter[] parameters, CommandType type)> commands)
            => _ExecuteTransaction(commands);

        public static bool AddRecords(string query, string tableName, List<Dictionary<string, object>> records, string? userColumnName = null, int? userId = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            if (type == CommandType.StoredProcedure)
            {
                SqlParameter tableParam = _CreateStructuredSqlParameterFromDictionary(tableName, records, userColumnName, userId);
                return _IsSuccessfulOperation(_ExecuteNonQuery(query, new SqlParameter[] { tableParam }, CommandType.StoredProcedure, retryAttempts, retryDelayMilliseconds));
            }
            else
            {
                query = _GenerateBulkInsertQuery(tableName, records);
                List<SqlParameter> parameters = _GenerateSqlParametersForMultipleRecords(records, userColumnName, userId);
                return _IsSuccessfulOperation(_ExecuteNonQuery(query, parameters.ToArray(), type, retryAttempts, retryDelayMilliseconds));
            }
        }

        public static bool UpdateRecords(string query, string tableName, string columnName, object columnValue, List<int> ids, Dictionary<string, object> updateValues, string? userColumnName = null, int? userId = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            if (type == CommandType.StoredProcedure)
            {
                SqlParameter tableParam = _CreateStructuredSqlParameter(columnName, ids);
                SqlParameter updateParam = _CreateStructuredSqlParameterFromDictionary(tableName, new List<Dictionary<string, object>> { updateValues }, userColumnName, userId);
                return _IsSuccessfulOperation(_ExecuteNonQuery(query, new SqlParameter[] { tableParam, updateParam }, CommandType.StoredProcedure, retryAttempts, retryDelayMilliseconds));
            }
            else
            {
                query = _GenerateBulkUpdateQuery(tableName, updateValues, columnName, ids);
                List<SqlParameter> parameters = _GenerateSqlParametersFromDictionary(updateValues, userColumnName, userId);
                return _IsSuccessfulOperation(_ExecuteNonQuery(query, parameters.ToArray(), type, retryAttempts, retryDelayMilliseconds));
            }
        }

        public static bool DeleteRecordsByIds(string query, string tableName, string columnName, object columnValue, List<int> ids, string? userColumnName = null, int? userId = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            if (type == CommandType.StoredProcedure)
                return Delete(query, new SqlParameter[] { _CreateStructuredSqlParameter(columnName, ids) }, CommandType.StoredProcedure, retryAttempts, retryDelayMilliseconds);
            else
                return Delete(_GenerateDeleteQuery(tableName, columnName, ids), columnName, 0, userColumnName, userId, type, retryAttempts, retryDelayMilliseconds);
        }
    }
    public partial class CRUD
    {
        public static async Task<int?> AddAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ConvertToIntOrDBNull(await _ExecuteScalarAsync(query, parameters, type, retryAttempts, retryDelayMilliseconds) ?? DBNull.Value);

        public static async Task<Dictionary<string, object>?> GetAsync(string query, SqlParameter[] parameters, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteSingleRecordAsync(query, parameters, type, retryAttempts, retryDelayMilliseconds);

        public static async Task<Dictionary<string, object>?> GetByColumnValueAsync(string query, string columnName, object columnValue, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteSingleRecordAsync(query, _CreateSqlParameterArray(columnName, columnValue), type, retryAttempts, retryDelayMilliseconds);

        //public static async Task<object[]?> GetAsync(string query, SqlParameter[] parameters, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => (await _ExecuteReaderAsync(query, parameters, type, retryAttempts, retryDelayMilliseconds))?.FirstOrDefault();

        //public static async Task<object[]?> GetByColumnValueAsync(string query, string columnName, object columnValue, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => (await _ExecuteReaderAsync(query, _CreateSqlParameterArray(columnName, columnValue), type, retryAttempts, retryDelayMilliseconds))?.FirstOrDefault();

        //public static async Task<T> GetByColumnValueAsync<T>(string query, string columnName, object columnValue, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => (await _ExecuteReaderAsync<T>(query, _CreateSqlParameterArray(columnName, columnValue, userId, userColumnName), type, retryAttempts, retryDelayMilliseconds)).FirstOrDefault();

        public static async Task<List<Dictionary<string, object>>?> GetAllAsListAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteReaderAsync(query, parameters, type, retryAttempts, retryDelayMilliseconds);

        //public static async Task<List<object[]>?> GetAllAsListAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => await _ExecuteReaderAsync(query, parameters, type, retryAttempts, retryDelayMilliseconds) ?? null;

        //public static async Task<List<T>> GetAllAsListAsync<T>(string query, SqlParameter[] parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        //    => await _ExecuteReaderAsync<T>(query, parameters, type, retryAttempts, retryDelayMilliseconds);

        public static async Task<DataTable?> GetAllAsDataTableAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteDataAdapterAsync(query, parameters, type, retryAttempts, retryDelayMilliseconds);

        public static async Task<bool> UpdateAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _IsSuccessfulOperation(await _ExecuteNonQueryAsync(query, parameters, type, retryAttempts, retryDelayMilliseconds));

        public static async Task<bool> DeleteAsync(string query, string columnName, object columnValue, string? userColumnName = null, int? userId = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _IsSuccessfulOperation(await _ExecuteNonQueryAsync(query, _CreateSqlParameterArray(columnName, columnValue, userColumnName, userId), type, retryAttempts, retryDelayMilliseconds));

        public static async Task<bool> DeleteAsync(string query, SqlParameter[]? parameters, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _IsSuccessfulOperation(await _ExecuteNonQueryAsync(query, parameters, type, retryAttempts, retryDelayMilliseconds));

        public static async Task<bool> IsExistAsync(string query, string columnName, object columnValue, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteScalarAsync(query, _CreateSqlParameterArray(columnName, columnValue), type, retryAttempts, retryDelayMilliseconds) != null;

        public static async Task<bool> ExecuteBatchOperationsAsync(List<(string query, SqlParameter[] parameters, CommandType type)> commands)
            => await _ExecuteTransactionAsync(commands);

        public static async Task<bool> AddRecordsAsync(string query, string tableName, List<Dictionary<string, object>> records, string? userColumnName = null, int? userId = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            if (type == CommandType.StoredProcedure)
            {
                SqlParameter tableParam = _CreateStructuredSqlParameterFromDictionary(tableName, records, userColumnName, userId);
                return _IsSuccessfulOperation(await _ExecuteNonQueryAsync(query, new SqlParameter[] { tableParam }, CommandType.StoredProcedure, retryAttempts, retryDelayMilliseconds));
            }
            else
            {
                query = _GenerateBulkInsertQuery(tableName, records);
                List<SqlParameter> parameters = _GenerateSqlParametersForMultipleRecords(records, userColumnName, userId);
                return _IsSuccessfulOperation(await _ExecuteNonQueryAsync(query, parameters.ToArray(), type, retryAttempts, retryDelayMilliseconds));
            }
        }

        public static async Task<bool> UpdateRecordsAsync(string query, string tableName, string columnName, object columnValue, List<int> ids, Dictionary<string, object> updateValues, string? userColumnName = null, int? userId = null, CommandType type = CommandType.StoredProcedure, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            if (type == CommandType.StoredProcedure)
            {
                SqlParameter tableParam = _CreateStructuredSqlParameter(columnName, ids);
                SqlParameter updateParam = _CreateStructuredSqlParameterFromDictionary(tableName, new List<Dictionary<string, object>> { updateValues });
                return _IsSuccessfulOperation(await _ExecuteNonQueryAsync(query, new SqlParameter[] { tableParam, updateParam }, CommandType.StoredProcedure, retryAttempts, retryDelayMilliseconds));
            }
            else
            {
                query = _GenerateBulkUpdateQuery(tableName, updateValues, columnName, ids);
                List<SqlParameter> parameters = _GenerateSqlParametersFromDictionary(updateValues, userColumnName, userId);
                return _IsSuccessfulOperation(await _ExecuteNonQueryAsync(query, parameters.ToArray(), type, retryAttempts, retryDelayMilliseconds));
            }
        }

        public static async Task<bool> DeleteRecordsByIdsAsync(string query, string tableName, string columnName, object columnValue, List<int> ids, string? userColumnName = null, int? userId = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            if (type == CommandType.StoredProcedure)
                return await DeleteAsync(query, new SqlParameter[] { _CreateStructuredSqlParameter(columnName, ids) }, CommandType.StoredProcedure, retryAttempts, retryDelayMilliseconds);
            else
                return await DeleteAsync(_GenerateDeleteQuery(tableName, columnName, ids), columnName, 0, userColumnName, userId, type, retryAttempts, retryDelayMilliseconds);
        }
    }
}
