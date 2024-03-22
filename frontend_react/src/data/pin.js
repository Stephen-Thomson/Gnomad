export default class Pin
{
    id = -1;
    userId = 0;
    longitude = 0;
    latitude = 0;
    title = '';
    street = '';
    tags = [];

    constructor(
        id,
        userId,
        longitude,
        latitude,
        title,
        street,
        tags)
    {
        this.id = id;
        this.userId = userId;
        this.longitude = longitude;
        this.latitude = latitude;
        this.title = title;
        this.street = street;
        this.tags = tags;
    }

    addTag(tag)
    {
        this.tags.push(tag);
    }
}