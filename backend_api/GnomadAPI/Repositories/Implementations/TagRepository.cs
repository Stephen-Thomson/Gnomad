/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 12/28/2022
*
* Purpose: Holds the functions for table modifications and access.
*
************************************************************************************************/

using MySql.Data.MySqlClient;
using System.Collections.Generic;
using TravelCompanionAPI.Models;
using System.Data;

namespace TravelCompanionAPI.Data
{
    //******************************************************************************
    //This class updates the Tags table, inheriting from IDataRepository.
    //No new methods added.
    //Implements getById, getAll, and add.
    //******************************************************************************
    public class TagRepository : ITagRepository
    {
        const string TABLE = "tags";
        //Connection strings should be in secrets.json. Check out the resources tab in Discord to update yours (or ask Andrew).

        public TagRepository()
        { }

        /// <summary>
        /// Gets a tag by its id
        /// </summary>
        /// <returns>
        /// A tag with the specified id
        /// </returns>
        public Tag getById(int id)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            Tag tag = null;

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = DatabaseConnection.getInstance().getConnection();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT id, type FROM + " + TABLE + " WHERE(`id` = @Id);";
                command.Parameters.AddWithValue("Id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tag = new Tag();
                        tag.Id = reader.GetInt32(0);
                        tag.Type = reader.GetString(1);
                    }
                }
            }

            connection.Close();

            return tag;
        }

        /// <summary>
        /// Gets all tags
        /// </summary>
        /// <returns>
        /// A list of all Tags
        /// </returns>
        public List<Tag> getAll()
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            List<Tag> tags = new List<Tag>();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT id, type FROM " + TABLE + ";";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tag tag = new Tag();
                        tag.Id = reader.GetInt32(0);
                        tag.Type = reader.GetString(1);
                        tags.Add(tag);
                    }
                }
            }

            connection.Close();

            return tags;
        }

        /// <summary>
        /// Adds a tag
        /// </summary>
        /// <returns>
        /// Returns a boolean, true if added to database.
        /// </returns>
        public bool add(Tag tag)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO " + TABLE + " (id, type) VALUES (@ID, @Type);";
                command.Parameters.AddWithValue("@ID", tag.Id);
                command.Parameters.AddWithValue("@Type", tag.Type);

                command.ExecuteNonQuery();
            }

            connection.Close();

            return true; //TODO: Error handling here.
        }
    }
}