//################################################################
//
// Authors: Stephen Thomson
// Date: 5/13/2023
// 
// Purpose: Contains code to connect the voting system
//          to the backend.
//
//################################################################

// internal imports.
import { get, post, isAuthenticated } from './api';

// this function gets the rating of a pin,
// from 0 to 5.
export async function getPinRating(pinId)
{
    if (!isAuthenticated()) return null;

    const response = await get(`pins/getReview/${pinId}`);
    return response;
  
}

// this function sends the users vote on a
// pin to the backend.
export async function ratePin(pinId, vote)
{
    if (!isAuthenticated()) return null;

    const response = await post(`user/review/${pinId}/${vote}`);
    return response;
  
}

// this function sends a users cancellation of
// their vote to the backend.
export async function cancelVote(pinId)
{
    if (!isAuthenticated()) return null;

    const response = await post(`user/cancelVote/${pinId}`);
    return response;
  
}

export async function haveVoted(pinId)
{
    if (!isAuthenticated()) return null;

    const response = await get(`user/voted/${pinId}`);
    return response;
  
}

export async function getVote(pinId)
{
    if (!isAuthenticated()) return null;

    const response = await get(`user/getVote/${pinId}`);
    return response;
}

export async function aRemove(pinId)
{
    if (!isAuthenticated()) return null;

    const response = await post(`pins/autodelete/${pinId}`);
    return response;
  
}