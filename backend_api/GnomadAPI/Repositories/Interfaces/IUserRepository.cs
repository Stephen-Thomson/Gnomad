/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: Defines the default functions for dependency injection. Declares getById, getAll, and add.
*
************************************************************************************************/

using System.Collections.Generic;
using TravelCompanionAPI.Models;

namespace TravelCompanionAPI.Data
{
    public interface IUserRepository
    {
        //******************************************************************************
        //This class defines the default functions for dependency injection
        //******************************************************************************

        public User getById(int id);

        public int getId(User data);

        public List<User> getAll();

        public bool add(User data);

        public bool contains(User data);

        public void review(int id, int pinid, int vote);

        public bool voted(int uid, int pinid);

        public int getVote(int uid, int pinid);

        public void cancelReview(int id, int pinid);

    }
}
