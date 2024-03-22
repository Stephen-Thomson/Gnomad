// RouteCreator

import { useEffect, useState } from "react";

import searchPins from '../../utilities/api/search_pins';
import event from "../../utilities/event";
import SearchBar from "../search_bar/SearchBar";
import Route from "../../data/route"
import createRoute from "../../utilities/api/create_routes";

import './route_creator.css';

// Purpose: creates a window to allow the user to create a route.
export default function RouteCreator()
{
  // variable states.
  const [routeName, setRouteName] = useState('');
  const [routePins, setRoutePins] = useState([]);
  const [routePinsHtml, setRoutePinsHtml] = useState([]);
  const [pinResults, setPinResults] = useState([]);

  // error states.
  const [nameError, setNameError] = useState('');
  const [pinsError, setPinsError] = useState('');

  // this function verifies that the input in each field is valid.
  // if its not valid it sets the correct error state.
  const validateInput = () =>
  {
    let status = true;

    // validate the route name is not empty.
    if (routeName.trim().length === 0)
    {
      setNameError('This cannot be left blank.');
      status = false;
    }

    if (routeName.length > 25)
    {
      setNameError('Exceeds max of 25 characters.');
      status = false;
    }

    // validate there is at least 1 pin in the route.
    if (routePins.length <= 0)
    {
      setPinsError('You must have at least one pin.');
      status =  false;
    }

    return status;
  }

  // called on create route button click.
  const submit = async () =>
  {
    // validate the route.
    if (validateInput())
    {
      // create the route object.
      let route = new Route(routeName, routePins);
      // call the createRoute backend endpoint.
      let response = await createRoute(route);

      // if the response is null, the create route
      // failed and errors can be handled here.
      if (response == null)
      {
        console.log("Create route call failed.");
      }
      else
      {
        // if the route was created successfully,
        // close the route creator.
        close();
      }
    }
  } 

  // this function closes the route creator.
  const close = () =>
  {
    event.emit('close-route-creator');
  }

  // called when a query is searched for.
  const search = async (searchQuery) =>
  {
    // remove whitespace from beginning and end of the query.
    let query = searchQuery.trim();

    // if the query is not empty...
    if (query.length !== 0)
    {
      // get the pins matching the search from backend.
      let pins = await searchPins(searchQuery);

      if (pins === null)
      {
        console.log("Failed to get routes with search: " + searchQuery);
        return;
      }

      // set the pins to a pin list.
      setPinResults(pins.map((pin, index) => <ResultPin key={index} pin={pin}/>));
    }
  }

  // update the pins list when the routePins change.
  useEffect(() =>
  {
    setRoutePinsHtml(
      routePins.map((pin, index) => <RoutePin index={index} key={index} pin={pin}></RoutePin>)
    );

  }, [routePins]);

  // add a pin to the route.
  const addToRoute = (pin) =>
  {
    // clear the error message if there is one.
    setPinsError('');
    // add the route to the array.
    setRoutePins(list => [...list, pin]);
  }

  // remove a pin from the route by index.
  const removeFromRoute = (index) =>
  {
    // make a copy of the array.
    var new_array = [...routePins];
    
    // if the index is valid remove the element from the array.
    if (index !== -1) 
    {
      new_array.splice(index, 1);
      // set the routePins array to the new array.
      setRoutePins(new_array);
    }
  }

  // a component to hold the search result pins.
  const ResultPin = ({pin}) =>
  {
    return (
      <li className='list-item'>
        <div className='result-pin'>
          <span className='add-pin-title'>{pin.title}</span>
          <button className='add-pin-button' onClick={() => addToRoute(pin)}>+</button>
        </div>
      </li>
    );
  }

  // a component to hold the route pins.
  const RoutePin = ({pin, index}) =>
  {
    return (
      <li className='list-item'>
        <div className='result-pin'>
          <span className='add-pin-title'>{pin.title}</span>
          <button className='add-pin-button' onClick={() => removeFromRoute(index)}>-</button>
        </div>
      </li>
    );
  }

  // HTML for the pin creation window.
  return (
      <div id='route-creator'>
          <div id='creator-header'>
              <h2>Create a Route</h2>
          </div>

          <div id='creator-body'>
              {/* section to enter route name */}
              <div className='input-section'>
                  <span id='input-label-wrapper'><label>Route Name</label> <label className='error'>{nameError}</label></span>
                  <input 
                      className='text-input' 
                      type='text' 
                      onChange={(event) => 
                      {
                          setRouteName(event.target.value); 
                          setNameError('');
                      }} 
                  />
              </div>

              <div className='input-section' id='route-creator-section'>
                  <div id='route-pins-picker'>
                      <div className='split-section' id='search-pins-list-wrapper'>
                          <div>
                            <label>Search for pins to add</label>
                            <SearchBar onSubmit={search}/>
                          </div>
                          <div id='pins-search-results'>
                            <ul className='pins-list'>
                                {pinResults}
                            </ul>
                          </div>
                      </div>
                      <div className='split-section'>
                        <span id='input-label-wrapper'><label>Pins</label><label className='error'>{pinsError}</label></span>
                          <ul className='pins-list'>
                            {routePinsHtml}
                          </ul>
                      </div>
                  </div>
              </div>
              
              {/* section with the buttons */}
              <div className='input-section row gap-10'>
                  <button className='button' onClick={close}>Cancel</button>
                  <button className='button' onClick={submit}>Create Route</button>
              </div>
          </div>
      </div>
  );
}