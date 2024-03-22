//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: Contains function to save and get objects in the
// session storage (on device temp storage).
//
//################################################################

// this function stores some object 
// at some key in the session storage.
export function session_store(key, value)
{
    sessionStorage.setItem(key, JSON.stringify(value));
}

// this function gets some object
// from session storage at some key.
export function session_get(key)
{
    return JSON.parse(sessionStorage.getItem(key));
}