using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;


//Create a RoommateRepository and implement only the GetById method. It should take in
//a int id as a parameter and return a Roommate object. The trick: When you add a
//menu option for searching for a roommate by their Id, the output to the screen
//should output their first name, their rent portion, and the name of the room they
//occupy. Hint: You'll want to use a JOIN statement in your SQL query

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
                {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                    {
                    cmd.CommandText = $@"SELECT Id, FirstName, RentPortion, r.Name AS 'RoomName'
                                         FROM Roommate rm
                                         JOIN Room r ON rm.roomId = r.Id 
                                         WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MovedInDate")),
                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                            }
                        };


                    }
                    return roommate;
                }
            }
        }
    }
}