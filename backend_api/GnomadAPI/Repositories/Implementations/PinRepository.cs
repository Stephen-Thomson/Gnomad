/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 12/28/2022
*
* Purpose: Holds the functions for table modifications and access.
*
************************************************************************************************/

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using TravelCompanionAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//using CommunityToolkit.Mvvm.DependencyInjection;

namespace TravelCompanionAPI.Data
{
    //******************************************************************************
    //This class updates the Pins table, inheriting from IDataRepository.
    //No new methods added.
    //Implements getById, getAll, and add.
    //******************************************************************************
    public class PinRepository : IPinRepository
    {
        //Defines tables for sql
        const string PIN_TABLE = "pins";
        const string TAG_TABLE = "pin_tags";
        const string ROUTES_TABLE = "routes";
        const string ROUTE_PINS_TABLE = "route_pins";
        const string USER_TABLE = "user_review";
        /// <summary>
        /// Constructor
        /// </summary>
        public PinRepository()
        { }

        /// <summary>
        /// Gets a pin from its id
        /// </summary>
        /// <returns>
        /// Returns a Pin with all of its dara.
        ///</returns>
        public Pin getById(int id)
        {
            Pin pin = null;
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT id, user_id, longitude, latitude, title, street FROM " + PIN_TABLE + " WHERE(`id` = @Id);";
                command.Parameters.AddWithValue("Id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pin = new Pin();
                        pin.Id = reader.GetInt32(0);
                        pin.UserId = reader.GetInt32(1);
                        pin.Longitude = reader.GetDouble(2);
                        pin.Latitude = reader.GetDouble(3);
                        pin.Title = reader.GetString(4);
                        pin.Street = reader.GetString(5);
                    }
                }
            }

            if (pin != null)
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT tag_id FROM " + TAG_TABLE + " WHERE(`pin_id` = @Id);";
                    command.Parameters.AddWithValue("Id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pin.Tags.Add(reader.GetInt32(0));
                        }
                    }
                }
            }

            connection.Close();

            return pin;
        }

        /// <summary>
        /// Gets all pins in the specified area, defaulting to Oregon Tech
        /// </summary>
        /// <returns>
        /// A list of all Pins in the specified area.
        /// </returns>
        public List<Pin> getAllInArea(double minLat = 0, double minLong = 0, double maxLat = 0, double maxLong = 0)
        {
            /*double minLat = latStart - latRange;
            double maxLat = latStart + latRange;
            double minLong = longStart - longRange;
            double maxLong = longStart + longRange;
            */

            List<Pin> pins_in_area = new List<Pin>();
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT id, user_id, longitude, latitude, title, street FROM " + PIN_TABLE +
                                    " WHERE(longitude > @minLong AND longitude < @maxLong AND latitude > @minLat AND latitude < @maxLat) LIMIT 100;";

                command.Parameters.AddWithValue("@minLong", minLong);
                command.Parameters.AddWithValue("@minLat", minLat);
                command.Parameters.AddWithValue("@maxLong", maxLong);
                command.Parameters.AddWithValue("@maxLat", maxLat);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pin pin = new Pin();
                        pin.Id = reader.GetInt32(0);
                        pin.UserId = reader.GetInt32(1);
                        pin.Longitude = reader.GetDouble(2);
                        pin.Latitude = reader.GetDouble(3);
                        pin.Title = reader.GetString(4);
                        pin.Street = reader.GetString(5);

                        pins_in_area.Add(pin);
                    }
                }
            }


            using (MySqlCommand command2 = new MySqlCommand())
            {
                command2.Connection = connection;
                command2.CommandType = CommandType.Text;
                command2.CommandText = @"SELECT tag_id FROM " + TAG_TABLE + " WHERE(`pin_id` = @Id);";

                MySqlParameter idParameter;

                foreach (Pin pin in pins_in_area)
                {
                    idParameter = new MySqlParameter("Id", pin.Id);
                    command2.Parameters.Add(idParameter);

                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            pin.Tags.Add(reader2.GetInt32(0));
                        }
                    }
                    command2.Parameters.Remove(idParameter);
                }
            }

            connection.Close();

            return pins_in_area;
        }

        /// <summary>
        /// Gets all pins
        /// </summary>
        /// <returns>
        /// A list of all Pins
        /// </returns>
        public List<Pin> getAll()
        {
            List<Pin> pins = new List<Pin>();
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT id, user_id, longitude, latitude, title, street FROM " + PIN_TABLE + " LIMIT 50;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pin pin = new Pin();
                        pin.Id = reader.GetInt32(0);
                        pin.UserId = reader.GetInt32(1);
                        pin.Longitude = reader.GetDouble(2);
                        pin.Latitude = reader.GetDouble(3);
                        pin.Title = reader.GetString(4);
                        pin.Street = reader.GetString(5);

                        pins.Add(pin);
                    }
                }
            }


            using (MySqlCommand command2 = new MySqlCommand())
            {
                command2.Connection = connection;
                command2.CommandType = CommandType.Text;
                command2.CommandText = "SELECT tag_id FROM " + TAG_TABLE + " WHERE(`pin_id` = @Id);";

                MySqlParameter idParameter;

                foreach (Pin pin in pins)
                {
                    idParameter = new MySqlParameter("Id", pin.Id);
                    command2.Parameters.Add(idParameter);

                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            pin.Tags.Add(reader2.GetInt32(0));
                        }
                    }
                    command2.Parameters.Remove(idParameter);
                }
            }
            connection.Close();
            return pins;
        }

        //Gets pins that have the search bar input string in the title
        public List<Pin> getByName(string searchString)
        {
            List<Pin> pins = new List<Pin>();
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                //Search the pins table title column for strings with the search string in it.
                //Set all to lower to dismiss case sensitivity
                command.CommandText = @"SELECT id, user_id, longitude, latitude, title, street FROM " + PIN_TABLE + " WHERE(LOWER(title) LIKE LOWER(@SearchString));";
                command.Parameters.AddWithValue("@SearchString", "%" + searchString + "%");

                //Go through and add to pin list
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pin pin = new Pin();
                        pin.Id = reader.GetInt32(0);
                        pin.UserId = reader.GetInt32(1);
                        pin.Longitude = reader.GetDouble(2);
                        pin.Latitude = reader.GetDouble(3);
                        pin.Title = reader.GetString(4);
                        pin.Street = reader.GetString(5);

                        pins.Add(pin);
                    }
                }
            }
            //Add tag to the pins in the pin list
            using (MySqlCommand command2 = new MySqlCommand())
            {
                command2.Connection = connection;
                command2.CommandType = CommandType.Text;
                command2.CommandText = "SELECT tag_id FROM " + TAG_TABLE + " WHERE(`pin_id` = @Id);";

                MySqlParameter idParameter;

                foreach (Pin pin in pins)
                {
                    idParameter = new MySqlParameter("Id", pin.Id);
                    command2.Parameters.Add(idParameter);

                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            pin.Tags.Add(reader2.GetInt32(0)); //Gets the tag
                        }
                    }
                    command2.Parameters.Remove(idParameter); //Don't need id
                }
            }
            connection.Close();
            return pins;
        }

        /// <summary>
        /// Gets all pins from a specified user
        /// </summary>
        /// <returns>
        /// A list of all Pins from that user
        /// </returns>
        public List<Pin> getAllByUser(int uid)
        {
            List<Pin> pins = new List<Pin>();
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT id, user_id, longitude, latitude, title, street FROM " + PIN_TABLE + " WHERE(`user_id` = @Uid);";
                command.Parameters.AddWithValue("@Uid", uid);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pin pin = new Pin();
                        pin.Id = reader.GetInt32(0);
                        pin.UserId = reader.GetInt32(1);
                        pin.Longitude = reader.GetDouble(2);
                        pin.Latitude = reader.GetDouble(3);
                        pin.Title = reader.GetString(4);
                        pin.Street = reader.GetString(5);

                        pins.Add(pin);
                    }
                }

            }

            using (MySqlCommand command2 = new MySqlCommand())
            {
                command2.Connection = connection;
                command2.CommandType = CommandType.Text;
                command2.CommandText = "SELECT tag_id FROM " + TAG_TABLE + " WHERE(`pin_id` = @Id);";

                MySqlParameter idParameter;

                foreach (Pin pin in pins)
                {
                    idParameter = new MySqlParameter("Id", pin.Id);
                    command2.Parameters.Add(idParameter);

                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            pin.Tags.Add(reader2.GetInt32(0));
                        }
                    }
                    command2.Parameters.Remove(idParameter);
                }
            }

            connection.Close();
            return pins;
        }

        /// <summary>
        /// Adds a pin to the database
        /// </summary>
        /// <returns>
        /// A boolean value, true if entered successfully.
        /// </returns>
        /// TODO: User Verification
        public bool add(Pin pin)
        {
            try
            {
                MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO " + PIN_TABLE + " (user_id, longitude, latitude, title, street) VALUES (@userID, @Longitude, @Latitude, @Title, @Street);";
                    command.Parameters.AddWithValue("@userId", pin.UserId);
                    command.Parameters.AddWithValue("@Longitude", pin.Longitude);
                    command.Parameters.AddWithValue("@Latitude", pin.Latitude);
                    command.Parameters.AddWithValue("@Title", pin.Title);
                    command.Parameters.AddWithValue("@Street", pin.Street);

                    command.ExecuteNonQuery();

                    pin.Id = (int)command.LastInsertedId;
                }

                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO " + TAG_TABLE + " (pin_id, tag_id) VALUES (@pin_id, @tag_id);";
                    command.Parameters.AddWithValue("@pin_id", pin.Id);

                    MySqlParameter tagIdParameter;

                    //Adds each tag as a parameter and sends a new query for each.
                    foreach (int myTag in pin.Tags)
                    {
                        tagIdParameter = new MySqlParameter("@tag_id", myTag);

                        command.Parameters.Add(tagIdParameter);
                        command.ExecuteNonQuery();
                        command.Parameters.Remove(tagIdParameter);
                    }
                }

                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool delete(int pinId)
        {
            List<int> route_ids = new List<int>();
            try
            {
                MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

                //Deletes all instances from pins table
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM " + PIN_TABLE + " WHERE id=@pinId;";
                    command.Parameters.AddWithValue("@pinId", pinId);

                    command.ExecuteNonQuery();
                }

                //Deletes all instances from pin_tags table
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM " + TAG_TABLE + " WHERE pin_id=@pinId;";
                    command.Parameters.AddWithValue("@pinId", pinId);

                    command.ExecuteNonQuery();
                }

                //Deletes all instances from user table
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM " + USER_TABLE + " WHERE pin_id=@pinId;";
                    command.Parameters.AddWithValue("@pinId", pinId);

                    command.ExecuteNonQuery();
                }

                //Selects all instances from route_pins table
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT route_id FROM " + ROUTE_PINS_TABLE + " WHERE pin_id=@pinID;";
                    command.Parameters.AddWithValue("@pinId", pinId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            route_ids.Add(reader.GetInt32(0));
                        }
                    }
                }

                //Deletes all instances from route_pins table
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM " + ROUTE_PINS_TABLE + " WHERE route_id=@routeId;";

                    MySqlParameter routeIdParameter;
                    foreach (int route in route_ids)
                    {
                        routeIdParameter = new MySqlParameter("@routeId", route);
                        command.Parameters.Add(routeIdParameter);

                        command.ExecuteNonQuery();
                        command.Parameters.Remove(routeIdParameter);
                    }
                }

                //Deletes all instances from routes table
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM " + ROUTES_TABLE + " WHERE id=@routeId;";

                    MySqlParameter routeIdParameter;
                    foreach (int route in route_ids)
                    {
                        routeIdParameter = new MySqlParameter("@routeId", route);
                        command.Parameters.Add(routeIdParameter);

                        command.ExecuteNonQuery();
                        command.Parameters.Remove(routeIdParameter);
                    }
                }

                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a pin already exists
        /// </summary>
        /// <returns>
        /// Returns a boolean, true if in the database, else false.
        /// </returns>
        public bool contains(Pin data)
        {
            bool exists = false;
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT COUNT(id) FROM " + PIN_TABLE + " WHERE longitude = @Longitude AND latitude = @Latitude;";
                command.Parameters.AddWithValue("@Longitude", data.Longitude);
                command.Parameters.AddWithValue("@Latitude", data.Latitude);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read(); //Only executes once, since just count.

                    if (int.Parse(reader.GetString(0)) >= 1)
                    {
                        exists = true;
                    }
                }
            }

            connection.Close();
            return exists;
        }

        /// <summary>
        /// Gets all pins with any of the given tags
        /// </summary>
        /// <returns>
        /// A list of pins
        /// </returns>
        public List<Pin> getAllByTag(List<int> tags, double latStart = 0, double longStart = 0, double latRange = 0, double longRange = 0)
        {
            List<Pin> pins;
            List<Pin> deleteList = new List<Pin>();

            if (latStart == 0 && longStart == 0 && latRange == 0 && longRange == 0)
                pins = getAllInArea(); //Defaults to Oregon
            else
                pins = getAllInArea(latStart, longStart, latRange, longRange);

            //Checks if pin is valid, adds to deleteList
            foreach (Pin pin in pins)
            {
                bool delete = true;
                foreach (int tag in tags)
                {
                    if (delete)
                    {
                        if (pin.Tags.Contains(tag))
                        {
                            delete = false;
                        }
                    }
                }
                if (delete)
                {
                    deleteList.Add(pin);
                }
            }

            //Remove all pins without the specified tags
            pins.RemoveAll(pin => (deleteList.Contains(pin)));

            return pins;
        }

        public List<Pin> getAllByAddress(string address)
        {
            List<Pin> pins = new List<Pin>();
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                //Search the pins table for pins with similar streets.
                //Set all to lower to dismiss case sensitivity
                command.CommandText = @"SELECT id, user_id, longitude, latitude, title, street FROM " + PIN_TABLE + " WHERE(LOWER(street) LIKE @address) LIMIT 100;";
                command.Parameters.AddWithValue("@address", "%" + address.ToLower() + "%");

                //Go through and add pin to pins list
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pin pin = new Pin();
                        pin.Id = reader.GetInt32(0);
                        pin.UserId = reader.GetInt32(1);
                        pin.Longitude = reader.GetDouble(2);
                        pin.Latitude = reader.GetDouble(3);
                        pin.Title = reader.GetString(4);
                        pin.Street = reader.GetString(5);

                        pins.Add(pin);
                    }
                }
            }

            //Add tag to the pins in the pin list
            using (MySqlCommand command2 = new MySqlCommand())
            {
                command2.Connection = connection;
                command2.CommandType = CommandType.Text;
                command2.CommandText = "SELECT tag_id FROM " + TAG_TABLE + " WHERE(`pin_id` = @Id);";

                MySqlParameter idParameter;

                foreach (Pin pin in pins)
                {
                    //Sets id for current pin
                    idParameter = new MySqlParameter("Id", pin.Id);
                    command2.Parameters.Add(idParameter);

                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            pin.Tags.Add(reader2.GetInt32(0));
                        }
                    }

                    //Can't rerun query for new pin until old is deleted
                    command2.Parameters.Remove(idParameter);
                }
            }

            connection.Close();

            return pins;
        }

        //Gets pins that have the search bar input string in the street
        public List<Pin> getByCity(string searchString)
        {
            List<Pin> pins = new List<Pin>();
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                //Search the pins table street column for strings with the search string in it.
                //Set all to lower to dismiss case sensitivity
                command.CommandText = @"SELECT id, user_id, longitude, latitude, title, street FROM " + PIN_TABLE + " WHERE(LOWER(street) LIKE LOWER(@SearchString)) LIMIT 100;";
                command.Parameters.AddWithValue("@SearchString", "%" + searchString + "%");

                //Go through and add to pin list
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pin pin = new Pin();
                        pin.Id = reader.GetInt32(0);
                        pin.UserId = reader.GetInt32(1);
                        pin.Longitude = reader.GetDouble(2);
                        pin.Latitude = reader.GetDouble(3);
                        pin.Title = reader.GetString(4);
                        pin.Street = reader.GetString(5);

                        pins.Add(pin);
                    }
                }
            }
            //Add tag to the pins in the pin list
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT tag_id FROM " + TAG_TABLE + " WHERE(`pin_id` = @Id);";

                MySqlParameter idParameter;

                foreach (Pin pin in pins)
                {
                    idParameter = new MySqlParameter("Id", pin.Id);
                    command.Parameters.Add(idParameter);

                    using (MySqlDataReader reader2 = command.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            pin.Tags.Add(reader2.GetInt32(0)); //Gets the tag
                        }
                    }
                    command.Parameters.Remove(idParameter); //Don't need id
                }
            }
            connection.Close();
            return pins;
        }

        //Gets the up_vote and down_vote values by pin id, then returns the ranking: 0 for no votes, else 1 to 5
        public double getAverageVote(int pinid)
        {
            int upVote = 0;
            int downVote = 0;
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT up_vote, down_vote FROM " + PIN_TABLE + " WHERE id=@PinId;";
                command.Parameters.AddWithValue("@PinId", pinid);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        upVote = reader.GetInt32("up_vote");
                        downVote = reader.GetInt32("down_vote");
                    }
                }

                connection.Close();
            }

            if (upVote  == 0 && downVote == 0)
            {
                return 0;
            }
            else if (upVote > 0 && downVote == 0)
            {
                return 5;
            }
            else if (upVote == 0 && downVote > 0)
            {
                return 1;
            }
            else
            {
                int totalVotes = upVote + downVote;
                double upVoteRatio = upVote / totalVotes;
                double ranking = upVoteRatio * 4.0 + 1.0;
                return (int)Math.Round(ranking, MidpointRounding.AwayFromZero);
            }
        }

        //Global Search function that takes in a string and searches for the string in Names, Addresses, and Cities.
        public List<Pin> globalSearch(string searchString)
        {
            List<Pin> pins = new List<Pin>();

            // Get pins by name
            pins.AddRange(getByName(searchString));

            // Get pins by address
            pins.AddRange(getAllByAddress(searchString));

            // Get pins by city
            pins.AddRange(getByCity(searchString));

            // Remove duplicates
            pins = pins.Distinct().ToList();

            return pins;
        }

        //Gets the stickers for the currently logged-in user
        public List<Pin> getStickers(int uid, double latStart = 0, double longStart = 0, double latRange = 0, double longRange = 0)
        {
            //TODO: User validation -- make sure the uid matches the current user.

            List<Pin> stickers;

            if (latStart == 0 && longStart == 0 && latRange == 0 && longRange == 0)
                stickers = getAllByTag(new List<int>() { Convert.ToInt32(TagValues.tags.Sticker) }); //Defaults to Oregon
            else
                stickers = getAllByTag(new List<int>() { Convert.ToInt32(TagValues.tags.Sticker) }, latStart, longStart, latRange, longRange);

            for (int ii = stickers.Count - 1; ii >= 0; ii--)
            {
                //If wrong user, remove from list.
                if (stickers.ElementAt(ii).UserId != uid)
                {
                    stickers.RemoveAt(ii);
                }
            }

            return stickers;
        }

        public bool autoRemove(int pinId)
        {
            try
            {
                int totalVotes = 0;
                MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT up_vote, down_vote FROM " + PIN_TABLE + " WHERE id=@PinId;";
                    command.Parameters.AddWithValue("@PinId", pinId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int upVote = reader.GetInt32("up_vote");
                            int downVote = reader.GetInt32("down_vote");
                            totalVotes = upVote + downVote;
                        }
                    }

                    connection.Close();
                }

                if (totalVotes >= 10)
                {
                    double averageVote = getAverageVote(pinId);

                    if (averageVote <= 1.0)
                    {
                        //connection = DatabaseConnection.getInstance().getConnection();

                        //using (MySqlCommand deleteCommand = new MySqlCommand())
                        //{
                        //    deleteCommand.Connection = connection;
                        //    deleteCommand.CommandType = CommandType.Text;
                        //    deleteCommand.CommandText = "DELETE FROM " + PIN_TABLE + " WHERE id=@PinId;";
                        //    deleteCommand.Parameters.AddWithValue("@PinId", pinId);

                        //    // Delete pin from PIN_TABLE
                        //    deleteCommand.ExecuteNonQuery();

                        //    // Delete related rows from user_review table
                        //    deleteCommand.CommandText = "DELETE FROM " + USER_TABLE + " WHERE pin_id=@PinId;";
                        //    deleteCommand.ExecuteNonQuery();

                        //    // Delete related rows from pins_tag table
                        //    deleteCommand.CommandText = "DELETE FROM " + TAG_TABLE + " WHERE pin_id=@PinId;";
                        //    deleteCommand.ExecuteNonQuery();
                        //}

                        //connection.Close();
                        return delete(pinId);
                    }
                    return true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}