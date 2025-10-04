using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using Type_Namespace.Models;

namespace Type_Namespace.Data
{
    public class TypeData
    {
        private readonly string _connection;

        private IDbConnection CreateConnection() => new SqliteConnection(_connection);

        public TypeData(string connectionString)
        {
            _connection = connectionString;
        }

        public int ValidId(int Id)
        {
            var connection = new SqliteConnection(_connection);
            connection.Open();
            var sql = "SELECT Id FROM Type";
            using var read = new SqliteCommand(sql, connection);
            using var reader = read.ExecuteReader();

            List<int> ids = new List<int>();

            while (reader.Read())
            {
                var id = reader.GetInt32(0);

                ids.Add(id);
            }

            reader.Close();
            connection.Close();

            if (!ids.Contains(Id))
            {
                return Id;
            }
            else
            {
                var validId = 1;
                for (int i = 0; i < ids.Count; i++)
                {
                    if (ids[i] == validId)
                    {
                        validId++;
                    }
                    else
                    {
                        return validId;
                    }
                }
            }

            return ids.Count + 1;
        }

        public List<RewardType> Search(string myString)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Type WHERE Description LIKE @Description";
            var list = connection.Query<RewardType>(sql, new { Description = $"%{myString}%" }).ToList();
            return list;
        }

        public void AddType(RewardType type)
        {
            type.Id = ValidId(type.Id);

            using var connection = CreateConnection();
            var sql = "INSERT INTO Type (Id, Description) VALUES (@Id, @Description)";
            connection.Execute(sql, type);
        }

        public void DeleteType(int Id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM Type WHERE Id = @Id";
            connection.Execute(sql, new { Id = Id });
        }

        public void EditType(RewardType type)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE Type
                SET 
                    Description = @Description
                WHERE Id = @Id";
            connection.Execute(sql, type);
        }

        public IEnumerable<RewardType> GetAllTypes()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Type";
            return connection.Query<RewardType>(sql).ToList();
        }

        public async Task<IEnumerable<RewardType>> GetAllTypesAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Type";
            return await connection.QueryAsync<RewardType>(sql);
        }

        public async Task<IEnumerable<RewardType>> UpdateTypeTable(List<RewardType> types)
        {
            using var connection = CreateConnection();
            var deleteSql = "DELETE FROM Type";
            await connection.ExecuteAsync(deleteSql);

            var insertSql = "INSERT INTO Type (Id, Description) VALUES (@Id, @Description)";
            await connection.ExecuteAsync(insertSql, types);

            return types;
        }

    }
}