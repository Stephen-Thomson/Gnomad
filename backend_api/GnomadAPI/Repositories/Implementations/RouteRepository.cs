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
using GnomadAPI.Models;
using System;
using System.Security.Principal;
using System.Security.Claims;

namespace TravelCompanionAPI.Data
{
    //******************************************************************************
    //This class updates the Tags table, inheriting from IDataRepository.
    //No new methods added.
    //Implements getById, getAll, and add.
    //******************************************************************************
    public class RouteRepository : IRouteRepository
    {
        const string TABLE = "routes";
        const string LINK_TABLE = "route_pins";
        
        IPinRepository m_pin_repo;

        public RouteRepository(IPinRepository pin_repo)
        {
            m_pin_repo = pin_repo;
        }

        /// <summary>
        /// Gets a tag by its id
        /// </summary>
        /// <returns>
        /// A tag with the specified id
        /// </returns>
        public Route getById(int id, int user_id)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            Route route = null;

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = DatabaseConnection.getInstance().getConnection();
                command.CommandType = CommandType.Text;

                command.CommandText = @"SELECT id, user_id, title FROM " + TABLE + " WHERE(`id` = @Id AND `user_id` = @USERID);";
                command.Parameters.AddWithValue("Id", id);
                command.Parameters.AddWithValue("USERID", user_id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        route = new Route();
                        route.Id = reader.GetInt32(0);
                        route.UserId = reader.GetInt32(1);
                        route.Title = reader.GetString(2);
                    }
                }
            }

            if (route != null)
            {
                using (MySqlCommand command2 = new MySqlCommand())
                {
                    command2.Connection = connection;
                    command2.CommandType = CommandType.Text;
                    command2.CommandText = @"SELECT pin_id FROM " + LINK_TABLE + " WHERE(`route_id` = @Id) ORDER BY position ASC;";

                    command2.Parameters.AddWithValue("Id", route.Id);

                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            //Sorted in ascending order because Add() adds to back
                            route.Pins.Add(m_pin_repo.getById(reader2.GetInt32(0)));
                        }
                    }
                }
            }

            connection.Close();

            return route; //TODO: Error handling if route is null? (404 shows up) Don't have to, null is valid output.
        }

        /// <summary>
        /// Gets all routes
        /// </summary>
        /// <returns>
        /// A list of all routes
        /// </returns>
        public List<Route> getAll(int userId)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();
            List<Route> routes = new List<Route>();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"SELECT id, user_id, title FROM " + TABLE + " WHERE(user_id = @USERID);";
                command.Parameters.AddWithValue("@USERID", userId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Route route = new Route();
                        route.Id = reader.GetInt32(0);
                        route.UserId = reader.GetInt32(1);
                        route.Title = reader.GetString(2);
                        routes.Add(route);
                    }
                }
            }

            using (MySqlCommand command2 = new MySqlCommand())
            {
                command2.Connection = connection;
                command2.CommandType = CommandType.Text;
                command2.CommandText = "SELECT pin_id FROM " + LINK_TABLE + " WHERE(`route_id` = @Id) ORDER BY position ASC;";

                MySqlParameter idParameter;

                foreach (Route route in routes)
                {
                    idParameter = new MySqlParameter("Id", route.Id);
                    command2.Parameters.Add(idParameter);

                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            route.Pins.Add(m_pin_repo.getById(reader2.GetInt32(0)));
                        }
                    }
                    command2.Parameters.Remove(idParameter);
                }
            }

            connection.Close();

            return routes;
        }

        /// <summary>
        /// Adds a tag
        /// </summary>
        /// <returns>
        /// Returns a boolean, true if added to database.
        /// </returns>
        public bool add(Route route)
        {
            MySqlConnection connection = DatabaseConnection.getInstance().getConnection();

            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO " + TABLE + " (id, user_id, title) VALUES (@ID, @USERID, @TITLE);";
                command.Parameters.AddWithValue("@ID", route.Id);
                command.Parameters.AddWithValue("@USERID", route.UserId);
                command.Parameters.AddWithValue("TITLE", route.Title);

                command.ExecuteNonQuery();

                route.Id = (int)command.LastInsertedId;
            }

            using (MySqlCommand command2 = new MySqlCommand())
            {
                command2.Connection = connection;
                command2.CommandType = CommandType.Text;
                command2.CommandText = "INSERT INTO " + LINK_TABLE + " (route_id, pin_id, position) VALUES (@ROUTEID, @PINID, @POSITION);";

                MySqlParameter routeIdParameter;
                MySqlParameter pinIdParameter;
                MySqlParameter positionParameter;
                int pinPosition = 0; //Set to 0, but will start at 1 since incremented at beginning of loop.

                foreach (Pin pin in route.Pins)
                {
                    pinPosition++; //Increment each time through the loop.
                    routeIdParameter = new MySqlParameter("ROUTEID", route.Id);
                    pinIdParameter = new MySqlParameter("PINID", pin.Id);
                    positionParameter = new MySqlParameter("POSITION", pinPosition);

                    command2.Parameters.Add(routeIdParameter);
                    command2.Parameters.Add(pinIdParameter);
                    command2.Parameters.Add(positionParameter);

                    using (MySqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            route.Pins.Add(m_pin_repo.getById(reader2.GetInt32(0)));
                        }
                    }

                    command2.Parameters.Remove(routeIdParameter);
                    command2.Parameters.Remove(pinIdParameter);
                    command2.Parameters.Remove(positionParameter);
                }
            }

            connection.Close();

            return true; //TODO: Error handling here.
        }
    }
}