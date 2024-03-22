/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 1/23/23
*
* Purpose: An enum for the pin values.
*
************************************************************************************************/

namespace TravelCompanionAPI.Data
{
    //******************************************************************************
    //This class updates the Tags table, inheriting from IDataRepository.
    //No new methods added.
    //Implements getById, getAll, and add.
    //******************************************************************************
    public static class TagValues
    {
        public enum tags
        {
            FreeBathroom = 1,
            CustomerOnlyBathroom = 2,
            TeslaSupercharger = 3,
            Regular = 4,
            Diesel = 5,
            FreeWiFi = 6,
            CustomerOnlyWiFi = 7,
            Electric = 8,
            Sticker = 9
        }
    }

}