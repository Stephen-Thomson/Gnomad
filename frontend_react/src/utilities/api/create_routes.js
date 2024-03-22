//################################################################
//
// Authors: Bryce Schultz
// Edited By: Stephen Thomson
// Date: 5/1/2023
// 
// Purpose: Contains code to create a route.
//
//################################################################

// internal imports.
import { get, post, isAuthenticated } from './api';

// this function takes a pin object and will call
// the backend api to create it.
export default async function createRoute(route)
{
    if (!isAuthenticated()) return null;

    const response = await post('routes/create', route);
    return response;
}