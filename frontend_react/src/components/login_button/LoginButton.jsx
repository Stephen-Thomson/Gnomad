//################################################################
//
// Authors: Bryce Schultz, Andrew Rice, Stephen Thomson
// Date: 12/19/2022
// 
// Purpose: Creates the google login react component
//
//################################################################

// external imports.
import { useRef, useState, useEffect } from 'react';

// internal imports.
import login, { logout, isLoggedIn } from '../../utilities/api/login';
import './login_button.css';

const client_id = '55413052184-k25ip3n0vl3uf641htstqn71pg9p01fl.apps.googleusercontent.com';

// this function renders the login button.
export function LoginButton() 
{
  // reference to the google login button div.
  const g_sso = useRef(null);

  // use state to manage the state of the login button.
  const [logged_in, setLoggedIn] = useState(false);

  const gLogin = async (response) =>
  {
    if (response === undefined)
    {
      console.log("Login error.");
      return;
    }

    const token = response.credential;

    const user = await login(token);

    if (user !== undefined)
    {
      setLoggedIn(true);
    }
  }

  const gLogout = () =>
  {
    setLoggedIn(false);
    logout();
  }

  useEffect(()=>
  {
    if (isLoggedIn())
    {
      setLoggedIn(true);
    }
  }, []);

  useEffect(() => 
  {
    if (g_sso.current) 
    {
      window.google.accounts.id.initialize(
      {
        client_id: client_id,
        callback: (res, error) => 
        {
          if (error === undefined)
          {
            gLogin(res);
          }
          else
          {
            console.log("A login error occurred.")
          }
        },
      });

      window.google.accounts.id.renderButton(g_sso.current, 
      {
        theme: 'default',
        size: 'medium',
        type: 'standard',
        text: 'signin',
        shape: 'pill',
        logo_alignment: 'left',
      });
    }
    // disable invalid warning about g_sso.current
    // not needing to be a dependency, it does.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [g_sso.current]);

  if (logged_in === false)
  {
    return (
      <div ref={g_sso}/>
    );
  }
  else
  {
    return (
      <button className='user-button' onClick={gLogout}>Logout</button>
    );
  }
}