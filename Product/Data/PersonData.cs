using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using Person_Namespace.Models;

namespace Person_Namespace.Data
{
    public class PersonData
    {
        private readonly string _connection;

        private IDbConnection CreateConnection() => new SqliteConnection(_connection);

        public PersonData(string connectionString)
        {
            _connection = connectionString;
        }

        public int ValidId(int Id)
        {
            var connection = new SqliteConnection(_connection);
            connection.Open();
            var sql = "SELECT Id FROM Person";
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
            } else
            {
                var validId = 1;
                for (int i = 0; i < ids.Count; i++) {
                    if (ids[i] == validId)
                    {
                        validId++;
                    } else {
                        return validId;
                    }
                }
            }
            
            return ids.Count + 1;
        }

        public List<Person> Search(string myString)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Person WHERE Name LIKE @Name";
            var list = connection.Query<Person>(sql, new { Name = $"%{myString}%" }).ToList();
            return list;
        }

        public void AddPerson(Person person)
        {
            person.Id = ValidId(person.Id);

            using var connection = CreateConnection();
            var sql = "INSERT INTO Person (Id, Name) VALUES (@Id, @Name)";
            connection.Execute(sql, person);
        }

        public void DeletePerson(int Id)
        { 
            using var connection = CreateConnection();
            var sql = "DELETE FROM Person WHERE Id = @Id";
            connection.Execute(sql, new { Id = Id });
        }

        public void EditPerson(Person person)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE Person
                SET 
                    Name = @Name
                WHERE Id = @Id";
            connection.Execute(sql, person);
        }

        public IEnumerable<Person> GetAllPeople()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Person";
            return connection.Query<Person>(sql).ToList();
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Person";
            return await connection.QueryAsync<Person>(sql);
        }

        public async Task<IEnumerable<Person>> UpdatePersonTable(List<Person> people)
        {            
            using var connection = CreateConnection();
            var deleteSql = "DELETE FROM Person";
            await connection.ExecuteAsync(deleteSql);

            var insertSql = "INSERT INTO Person (Id, Name) VALUES (@Id, @Name)";
            await connection.ExecuteAsync(insertSql, people);
            
            return people;
        }
    }
}