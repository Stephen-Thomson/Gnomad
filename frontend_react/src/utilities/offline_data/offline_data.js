// internal imports.
import { local_get, local_store } from "../local_storage";

// define some keys for the storage of various things.
const routes_key = 'routes';
const pins_key = 'pins';
const has_local_data_key = 'local_data';

export function saveRoutes(routes)
{
    local_store(has_local_data_key, true);
    local_store(routes_key, routes);
}

export function loadRoutes()
{
    return local_get(routes_key);
}

export function savePins(pins)
{
    local_store(has_local_data_key, true);
    local_store(pins_key, pins);
}

export function loadPins()
{
    return local_get(pins_key);
}

export function hasLocalData()
{
    return local_get(has_local_data_key);
}