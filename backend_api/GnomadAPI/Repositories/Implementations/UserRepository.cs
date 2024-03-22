/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Holds the functions for table modifications and access.
*
************************************************************************************************/

using MySql.Data.MySqlClient;
using System.Collections.Generic;
using TravelCompanionAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;

namespace TravelCompanionAPI.Data
{
    //******************************************************************************
    //
    // This class updates the User table, inheriting from IDataRepository.
    // No new methods added.
    //
    //******************************************************************************
    public class UserRepository : IUserRepository
    {
        const string TABLE = "users";
        const string PTABLE = "pins";
        const string RTABLE = "user_review";

        public UserRepository()
        { }

        /// <summary>
        /// Gets a user from their id
        /// </summary>
        /// <returns>
        /// Returns the user with the specified id
        /// </returns>
        public User getById(int id)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            User user = null;
            using (MySqlCommand command = new MySqlCommand())
            {

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT id, email, profile_photo_url, first_name, last_name FROM " + TABLE + " WHERE id = @Id;";
                command.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User();
                        user.Id = reader.GetInt32(0);
                        user.Email = reader.GetString(1);
                        user.ProfilePhotoURL = reader.GetString(2);
                        user.FirstName = reader.GetString(3);
                        user.LastName = reader.GetString(4);
                    }
                }
            }

            connection.Close();
            return user;
        }

        /// <summary>
        /// Gets the id of a specified user
        /// </summary>
        /// <returns>
        /// Returns the id of the specified user
        /// </returns>
        public int getId(User user)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            int id = -1;
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT id FROM " + TABLE + " WHERE email = @Email;";
                command.Parameters.AddWithValue("@Email", user.Email);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
            }

            connection.Close();

            return id;
        }

        /// <summary>
        /// Gets a list of all users
        /// </summary>
        /// <returns>
        /// A list of all Users
        /// </returns>
        public List<User> getAll()
        {

            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            List<User> users = new List<User>();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT id, email, profile_photo_url, first_name, last_name FROM " + TABLE + ";";


                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User();
                        user.Id = reader.GetInt32(0);
                        user.Email = reader.GetString(1);
                        user.ProfilePhotoURL = reader.GetString(2);
                        user.FirstName = reader.GetString(3);
                        user.LastName = reader.GetString(4);
                        users.Add(user);
                    }
                }
            }
            connection.Close();
            return users;

        }

        /// <summary>
        /// Checks if the user exists
        /// </summary>
        /// <returns>
        /// Returns a boolean, true if the user exists, else false.
        /// </returns>
        public bool contains(User user)
        {

            bool exists = false;
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT COUNT(id) FROM " + TABLE + " WHERE email = @Email;";

                command.Parameters.AddWithValue("@Email", user.Email);


                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) != 0)
                        {
                            exists = true;
                        }
                    }
                }
            }
            connection.Close();
            return exists;
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <returns>
        /// Returns a boolean, true if added successfully, else false.
        /// </returns>
        public bool add(User user)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO " + TABLE + " (email, profile_photo_url, first_name, last_name) VALUES (@Email, @ProfilePhotoURL, @FirstName, @LastName);";
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@ProfilePhotoURL", user.ProfilePhotoURL);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);

                command.ExecuteNonQuery();
            }

            connection.Close();
            return true; //TODO: Error handling here.
        }

        //Register a user's review of a pin - vote values: 0 = down vote, 1 = up vote
        public void review(int id, int pinid, int vote)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            //Update the pins table
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE " + PTABLE + " SET " + (vote == 1 ? "up_vote = up_vote + 1" : "down_vote = down_vote + 1") + " WHERE id = @Id;";
                command.Parameters.AddWithValue("@Id", pinid);

                command.ExecuteNonQuery();

            }

            //Update the user_review table
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO " + RTABLE + " (user_id, pin_id, review) VALUES (@UserId, @PinId, @Review);";
                command.Parameters.AddWithValue("@UserId", id);
                command.Parameters.AddWithValue("@PinId", pinid);
                command.Parameters.AddWithValue("@Review", vote);

                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        //Check to see if user already voted on this pin
        public bool voted(int uid, int pinid)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            //Looks for if row exists
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM " + RTABLE + " WHERE user_id=@UserId AND pin_id=@PinId;";
                command.Parameters.AddWithValue("@UserId", uid);
                command.Parameters.AddWithValue("@PinId", pinid);

                int count = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                //If row exists, count > 0 so return true, else return false
                return count > 0;
            }
        }

        //Checks if the vote exists, then returns the value of that user's vote. Returns -1 if it doesn't exist.
        public int getVote(int uid, int pinid)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            int review = -1;

            if (voted(uid, pinid))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT review FROM " + RTABLE + " WHERE user_id=@UserId AND pin_id=@PinId;";
                    command.Parameters.AddWithValue("@UserId", uid);
                    command.Parameters.AddWithValue("@PinId", pinid);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            review = reader.GetInt32("review");
                        }
                    }

                    connection.Close();
                }
            }
            else
            {
                connection.Close();
            }

            return review;
        }

        //Cancel a User's Vote
        public void cancelReview(int id, int pinid)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            int review = -1;

            // Retrieve the review value from the user_review table
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT review FROM " + RTABLE + " WHERE user_id = @UserId AND pin_id = @PinId;";
                command.Parameters.AddWithValue("@UserId", id);
                command.Parameters.AddWithValue("@PinId", pinid);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        review = reader.GetInt32(0);
                    }
                }
            }

            // Remove the row from the user_review table
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM " + RTABLE + " WHERE user_id = @UserId AND pin_id = @PinId;";
                command.Parameters.AddWithValue("@UserId", id);
                command.Parameters.AddWithValue("@PinId", pinid);

                command.ExecuteNonQuery();
            }

            // Update the up_vote or down_vote column in the pins table
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE " + PTABLE + " SET " + (review == 1 ? "up_vote = up_vote - 1" : "down_vote = down_vote - 1") + " WHERE id = @Id;";
                command.Parameters.AddWithValue("@Id", pinid);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
    
}