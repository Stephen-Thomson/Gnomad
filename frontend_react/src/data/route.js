// represents a route in our database.
export default class Route
{
    title = '';
    pins = [];
    id = 0;
    userId = 0;

    constructor(title, pins)
    {
        this.title = title;
        this.pins = pins;
    }
}