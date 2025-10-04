using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using AffinityCard_Namespace.Models;

namespace AffinityCard_Namespace.Data
{
    public class AffinityCardData
    {
        private readonly string _connection;

        private IDbConnection CreateConnection() => new SqliteConnection(_connection);

        public AffinityCardData(string connectionString)
        {
            _connection = connectionString;
        }

        public void AddCard(AffinityCard card)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO AffinityCard (Id, AffinityProgramID, PersonID, RewardCompany, Points, DateOpen, DateClose, AnnualFee, CreditLine, Notes) VALUES (@Id, @AffinityProgramID, @PersonID, @RewardCompany, @Points, @DateOpen, @DateClose, @AnnualFee, @CreditLine, @Notes)";
            connection.Execute(sql, card);
        }

        public int ValidId(int Id)
        {
            var connection = new SqliteConnection(_connection);
            connection.Open();
            var sql = "SELECT Id FROM AffinityCard";
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

        public List<AffinityCard> Search(string query)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM AffinityCard WHERE RewardCompany LIKE @RewardCompany";
            var list = connection.Query<AffinityCard>(sql, new { RewardCompany = $"%{query}%" }).ToList();
            return list;
        }

        public void DeleteCard(int Id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM AffinityCard WHERE Id = @Id";
            connection.Execute(sql, new { Id = Id });
        }

        public void EditCard(AffinityCard card) {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE AffinityCard
                SET 
                    AffinityProgramID = @AffinityProgramID,
                    PersonID = @PersonID,
                    RewardCompany = @RewardCompany,
                    Points = @Points,
                    DateOpen = @DateOpen,
                    DateClose = @DateClose,
                    AnnualFee = @AnnualFee,
                    CreditLine = @CreditLine,
                    Notes = @Notes
                WHERE Id = @Id";
            connection.Execute(sql, card);
        }

        public void EditPoints(int Id, int Points)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE AffinityCard
                SET 
                    Points = @Points
                WHERE Id = @Id";
            connection.Execute(sql, new { Id, Points }); 
        }

        public IEnumerable<AffinityCard> GetAllCards()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM AffinityCard";
            return connection.Query<AffinityCard>(sql).ToList();
        }

        public async Task<IEnumerable<AffinityCard>> GetAllCardsAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM AffinityCard";
            return await connection.QueryAsync<AffinityCard>(sql);
        }

        public async Task<IEnumerable<AffinityCard>> UpdateCardTable(List<AffinityCard> cards)
        {
            using var connection = CreateConnection();
            var deleteSql = "DELETE FROM AffinityCard";
            await connection.ExecuteAsync(deleteSql);

            var insertSql = "INSERT INTO AffinityCard (Id, AffinityProgramID, PersonID, RewardCompany, Points, DateOpen, DateClose, AnnualFee, CreditLine, Notes) VALUES (@Id, @AffinityProgramID, @PersonID, @RewardCompany, @Points, @DateOpen, @DateClose, @AnnualFee, @CreditLine, @Notes)";
            await connection.ExecuteAsync(insertSql, cards);

            return cards;
        }


    }
}