//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: Contains code to get all pins in radius or with tags.
//
//################################################################

// internal imports.
import { get, isAuthenticated} from './api';

// this function gets pins based on a radius and
// an array of tags.
export default async function getPins(radius = 100, tags = [])
{
    if (!isAuthenticated()) return null;

    const radius_string = 'radius=' + radius;
    const tags_string = 'tags=' + String(tags);

    const response = await get('pins/get?' + radius_string + '&' + tags_string);

    return response;
}