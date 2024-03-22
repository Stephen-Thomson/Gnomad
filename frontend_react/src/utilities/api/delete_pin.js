//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: Contains code to create a user pin (sticker)
//
//################################################################

// internal imports.
import { get, post, isAuthenticated } from './api';

// this function takes a pin object and will call
// the backend api to create it.
export default async function deletePin(pinId)
{
    if (!isAuthenticated()) return null;

    const response = await post(`pins/delete/${pinId}`);
    return response;
}