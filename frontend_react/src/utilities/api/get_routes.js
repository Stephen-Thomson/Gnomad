//################################################################
//
// Authors: Stephen Thomson
// Date: 5/9/2023
// 
// Purpose: Contains code to get all routes for the current user
//
//################################################################

// internal imports.
import { get, isAuthenticated} from './api';

//This gets all routes for the current user
export default async function getRoutes()
{
    if (!isAuthenticated()) return null;

    const response = await get('routes/all');

    return response;
}