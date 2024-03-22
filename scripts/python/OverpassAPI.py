# Author: Stephen Thomson
# Date: 2/3/2023
# Purpose: Access the OverPass API and downloads wifi and bathroom
#           locations, then inserts them into the testing pins table.

import requests
import mysql.connector
import json

#Connect to database
cnx = mysql.connector.connect(user='codenome', password='Codenome!1',
                              host='travel.bryceschultz.com', database='codenome_testing')
cursor = cnx.cursor()

#Set url
overpass_url = "http://overpass-api.de/api/interpreter"

# Coordinates for bounding box (min_lat, min_lon, max_lat, max_lon)
bounding_box = (41.991794, -124.566244, 46.292035, -116.463504)

#Set overpass query
overpass_query = f"""
[out:json];
node["amenity"~"toilets|wifi"](bbox={bounding_box[0]},{bounding_box[1]},{bounding_box[2]},{bounding_box[3]});
out body;
"""

#Get response
response = requests.get(overpass_url, params={'data': overpass_query})
data = response.json()

#Parse and insert the data
for element in data['elements']:
    title = element.get('tags', {}).get('name', 'N/A')
    street = element.get('tags', {}).get('addr:street', 'N/A')
    tag_bathroom = 1 if 'toilets' in element.get('tags', {}).get('amenity', '') else 0
    tag_wifi = 1 if 'wifi' in element.get('tags', {}).get('amenity', '') else 0
    lat = element['lat']
    lon = element['lon']
    cursor.execute("INSERT INTO pins (title, street, tag_bathroom, tag_wifi, Latitude, Longitude, user_id) VALUES (%s, %s, %s, %s, %s, %s, 0)", (title, street, tag_bathroom, tag_wifi, lat, lon))

# Commit the changes and close the connection
cnx.commit()
cursor.close()
cnx.close()
