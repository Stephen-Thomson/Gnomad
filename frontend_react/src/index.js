//################################################################
//
// Authors: Bryce Schultz
// Date: 12/19/2022
// 
// Purpose: Sets up the root page element and renders App
//
//################################################################

// external imports.
import React from 'react';
import ReactDOM from 'react-dom/client';

// pwa related imports.
import * as serviceWorkerRegistration from './pwa_code/serviceWorkerRegistration';
import reportWebVitals from './pwa_code/reportWebVitals';

// internal imports.
import './index.css';
import App from './App';

// get the root element and create a react root.
const root = ReactDOM.createRoot(document.getElementById('root'));

// render the App component to the root of the document.
root.render (
    <App/>
);

// register the service worker to make the PWA cache correctly.
serviceWorkerRegistration.register();

// report web vitals for future improvements.
reportWebVitals();
