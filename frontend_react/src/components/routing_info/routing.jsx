//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: The routing info component.
//
//################################################################

// external imports.
import React, { Component } from 'react';

// internal imports.
import './routing.css';

// this class renders the Routing component
export default class Routing extends Component 
{
  constructor(props) 
  {
    super(props);

    this.stop = "Your heart";
    this.stop_arrival_time = "2:48";
    this.destination = "Passing JP";
    this.distance = "2 many miles";
    this.trip_arrival_time = "Never";
  }

  render() 
  {
    return (
      <div id='routing-wrapper'>
        <section id='general-next-stop-info'>
          <div id='next-stop'>
            <div id='next-stop-text'>Next Stop:</div>
            &nbsp;
            <div id='stop'>{this.stop}</div>
          </div>
          <div id='next-stop-arrival-time'>
            <div id='next-stop-arrival-time-text'>Arrival Time:</div>
            &nbsp;
            <div id='next-stop-arrival-time'>{this.stop_arrival_time}</div>
          </div>
        </section>
        <hr id='divider' />
        <section id='general-destination-info'>
          <div id='destination'>
            <div id='destination-text'>Destination:</div>
            &nbsp;
            <div id='dest'>{this.destination}</div>
          </div>
          <div id='distance'>
            <div id='distance-text'>Distance:</div>
            &nbsp;
            <div id='dist'>{this.distance}</div>
          </div>
          <div id='final-arrival-time'>
            <div id='final-arrival-time-text'>Arrival Time:</div>
            &nbsp;
            <div id='final-arrival-time'>{this.trip_arrival_time}</div>
          </div>
        </section>
      </div>
    );
  }
}
