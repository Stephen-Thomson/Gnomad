//################################################################
//
// Authors: Stephen Thomson, Bryce Schultz
// Date: 4/11/2023
// 
// Purpose: Contains code to send the query from the search bar
//      in the Routes page to the global search function 
//      in the backend.
//
//################################################################

// internal import.
import { get, isAuthenticated} from './api';

// this function uses the get function to send
// a search query to the backend global search
// function and returns a json response.
export default async function searchPins(query)
{
    if (!isAuthenticated()) return null;

    const response = await get('pins/getGlobal/' + query);

    return response;
}