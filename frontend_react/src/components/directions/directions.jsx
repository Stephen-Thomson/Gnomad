//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: React component for rendering the directions
//
//################################################################

// external imports.
import React, { Component } from 'react';

// internal imports.
import './directions.css';

// this class renders the Directions component
export default class directions extends Component 
{
  constructor(props) 
  {
    super(props);

    this.current_direction = "Take the next right"
  }

  render() 
  {
    return (
      <div id='directions-wrapper'>
        <div id='next-turn'>{this.current_direction}</div>
      </div>
    );
  }
}
