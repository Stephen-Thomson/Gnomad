//################################################################
//
// Authors: Bryce Schultz, Stephen Thomson
// Date: 12/19/2022
// 
// Purpose: The pin creator component.
//
//################################################################

import { useState } from "react";
import event from "../../utilities/event";

import './pin_creator.css';

export default function PinCreator()
{
  // variable states.
  const [pinName, setPinName] = useState('');
  const [pinType, setPinType] = useState('');

  // error states.
  const [nameError, setNameError] = useState('');
  const [typeError, setTypeError] = useState('');

  // this function verifies that the input in each field is valid.
  // if its not valid it sets the correct error state.
  const validateInput = () =>
  {
    let status = true;

    if (pinName.trim().length === 0)
    {
      setNameError('This cannot be left blank.');
      status = false;
    }

    if (pinName.length > 25)
    {
      setNameError('Exceeds max of 25 characters.');
      status = false;
    }

    if (pinType.trim().length === 0)
    {
      setTypeError('This cannot be left blank.');
      status = false;
    }

    return status;
  }

  const submit = () =>
  {
    // if the input is fine, call create pin with the validated data.
    if (validateInput())
    {
      let pin = 
      {
        name: pinName, 
        type: pinType
      };

      event.emit('create-pin', {pin});
      close();
    }
  }

  const close = () =>
  {
    event.emit('close-pin-creator');
  }

  // HTML for the pin creation window.
  return (
    <div id='pin-creator'>
      <div id='creator-header'>
        <h2>Create a Pin</h2>
      </div>

      {/* section to enter pin name */}
      <div id='creator-body'>
        <div className='input-section'>
          <span id='input-label-wrapper'><label>Pin Name</label> <label className='error'>{nameError}</label></span>
          <input 
            className='text-input' 
            type='text' 
            onChange={(event) => 
            {
              setPinName(event.target.value); 
              setNameError('');
            }} 
          />
        </div>
        
        {/* section to enter pin type */}
        <div className='input-section'>
          <span id='input-label-wrapper'><label>Pin Type</label> <label className='error'>{typeError}</label></span>
          <select 
            className='text-input' 
            id="pin-type" 
            value={pinType} 
            onChange={(e) => setPinType(e.target.value)}
          >
            <option value="">--Select--</option>
            <option value="Bathroom">Bathroom</option>
            <option value="Supercharger">Supercharger</option>
            <option value="Diesel">Diesel</option>
            <option value="Wi-Fi">Free Wi-Fi</option>
            <option value="Pin">Gnome (Misc)</option>
            <option value="Fuel">Regular Fuel</option>
          </select>
        </div>
        
        {/* section with the buttons */}
        <div className='input-section row gap-10'>
          <button className='button' onClick={close}>Cancel</button>
          <button className='button' onClick={submit}>Click to Place Pin</button>
        </div>
      </div>
    </div>
  );
}