//################################################################
//
// Authors: Karter Zwetschke
// Date: 12/19/2022
// 
// Purpose: Contains code to get all cell data
//
//################################################################

// internal imports.
import { get } from './api';

export default async function getAllCoords(max_pass, latMin, lngMin, latMax, lngMax) {
    const responseLatLng = await get('h3_oregon_data/allCoordsSingle/' + String(max_pass) + '/' + String(latMin) + '/'
        + String(lngMin) + '/' + String(latMax) + '/' + String(lngMax));

    return responseLatLng;
}