//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: Contains functions to set and get cookies from the
// the browser.
//
//################################################################

// setCookie takes a name and a value, it then sets the cookie 
// to the value.
export function setCookie(name, value, expire_days = 1) 
{
  const d = new Date();

  d.setTime(d.getTime() + expire_days * 24 * 60 * 60 * 1000);

  let expires = 'expires=' + d.toUTCString();

  document.cookie = name + '=' + value + ';' + expires + ';path=/';
}

// getCookie returns the value of a specified cookie
// will return and empty string if no value is defined.
export function getCookie(name) 
{
  name += '=';
  let decodedCookie = decodeURIComponent(document.cookie);
  let ca = decodedCookie.split(';');

  for (let i = 0; i < ca.length; i++) 
  {
    let c = ca[i];
    while (c.charAt(0) === ' ') 
    {
      c = c.substring(1);
    }

    if (c.indexOf(name) === 0) 
    {
      return c.substring(name.length, c.length);
    }
  }
  return '';
}
