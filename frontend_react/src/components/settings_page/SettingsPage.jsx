// SettingsPage

import event from "../../utilities/event";
import './settings_page.css';
import logo from '../../images/ColoredGnomadLogo.png';

// Purpose: creates a window to allow the user to create a route.
export default function SettingsPage()
{
  // this function closes the settings page.
  const close = () =>
  {
    event.emit('close-settings-page');
  }

  // HTML for the settings window.
  return (
      <div id='settings-page'>
          <div id='gnomad-header'>
              <h1>Gnomad</h1>
          </div>

          <div id='gnomad-logo'>
            <img src={logo} alt="Gnomad logo" />
          </div>

          <div id ='author-section'>
            <p>Gnomad was developed by the Codenomes:</p>
            <p>Andrew Ramirez, Andrew Rice, Bryce Schultz, Stephen Thomson, and Karter Zwetschke</p>
          </div>

          {/* cancel button */}
          <div id='cancel-button'>
            <button className='button' onClick={close}>Cancel</button>
          </div>
      </div>
  );
}