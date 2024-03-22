//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: Contains code to log the user in.
//
//################################################################

// internal imports.
import { post } from './api';
import { session_store, session_get } from '../session_storage';
import { setCookie } from '../cookies';
import event from '../event';

// named constant to keep track of storage key.
const logged_in_key = 'logged_in';

// this function makes a post request to user/login
// the user from the database is returned with correct
// user id.
export default async function login(token)
{
    // set the id_token cookie.
    setCookie('id_token', 'Bearer ' + token);

    // send a post request to login.
    let user = await post('user/login');

    // if the user was logged in successfully, set 
    // the session variable for logged_in to true.
    if (user !== undefined)
    {
        session_store(logged_in_key, true);
        event.emit('user-login');
    }

    // return the user
    return user;
}

export async function logout()
{
    // on logout set the session variable to 
    // false, and remove the id_token cookie.
    session_store(logged_in_key, false);
    event.emit('user-logout');
}

export function isLoggedIn()
{
    return session_get(logged_in_key);
}