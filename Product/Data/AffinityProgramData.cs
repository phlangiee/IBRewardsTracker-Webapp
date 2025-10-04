using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using AffinityProgram_Namespace.Models;

namespace AffinityProgram_Namespace.Data
{
    public class AffinityProgramData
    {
        private readonly string _connection;

        private IDbConnection CreateConnection() => new SqliteConnection(_connection);

        public AffinityProgramData(string connectionString)
        {
            _connection = connectionString;
        }

        public int ValidId(int Id)
        {
            var connection = new SqliteConnection(_connection);
            connection.Open();
            var sql = "SELECT Id FROM AffinityProgram";
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

        public List<AffinityProgram> Search(string query)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM AffinityProgram WHERE ProgramCompany LIKE @ProgramCompany";
            var list = connection.Query<AffinityProgram>(sql, new { ProgramCompany = $"%{query}%" }).ToList();
            return list;
        }


        public void AddProgram(AffinityProgram program)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO AffinityProgram (Id, TypeID, ProgramCompany, AffinityNum, Level) VALUES (@Id, @TypeID, @ProgramCompany, @AffinityNum, @Level)";
            connection.Execute(sql, program);
        }

        public void DeleteProgram(int Id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM AffinityProgram WHERE Id = @Id";
            connection.Execute(sql, new { Id = Id  });
        }

        public void EditProgram(AffinityProgram program)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE AffinityProgram
                SET 
                    TypeID = @TypeID,
                    ProgramCompany = @ProgramCompany,
                    AffinityNum = @AffinityNum,
                    Level = @Level
                WHERE Id = @Id";
            connection.Execute(sql, program);
        }

        public IEnumerable<AffinityProgram> GetAllPrograms()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM AffinityProgram";
            return connection.Query<AffinityProgram>(sql).ToList();
        }

        public async Task<IEnumerable<AffinityProgram>> GetAllProgramsAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM AffinityProgram";
            return await connection.QueryAsync<AffinityProgram>(sql);
        }

        public async Task<IEnumerable<AffinityProgram>> UpdateProgramTable(List<AffinityProgram> programs)
        {
            using var connection = CreateConnection();
            var deleteSql = "DELETE FROM AffinityProgram";
            await connection.ExecuteAsync(deleteSql);

            var insertSql = "INSERT INTO AffinityProgram (Id, TypeID, ProgramCompany, AffinityNum, Level) VALUES (@Id, @TypeID, @ProgramCompany, @AffinityNum, @Level)";
            await connection.ExecuteAsync(insertSql, programs);

            return programs;
        }

    }
}