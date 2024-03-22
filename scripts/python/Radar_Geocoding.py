# Author: Stephen Thomson
# Date: 2/17/2023
# Purpose: Uses Radar Geocoding to fill in missing data in the table using
#           the latitude and longitude of rows missing title and street.

import mysql.connector
import requests

def fill_location_data(conn, cursor):
    # Get rows with N/A in title or street columns
    cursor.execute("SELECT id, latitude, longitude FROM pins WHERE title = 'N/A' OR street = 'N/A'")
    rows = cursor.fetchall()

    for row in rows:
        # Send request to Radar Geocoding API
        res = requests.get(f"https://api.radar.io/v1/geocode/reverse?lat={row[1]}&lon={row[2]}")
        data = res.json()

        if 'address' in data:
            address = data['address']
            title = address.get('name', 'N/A')
            street = address.get('formattedAddress', 'N/A')

            # Update row in database
            cursor.execute("UPDATE pins SET title = %s, street = %s WHERE id = %s", (title, street, row[0]))
            conn.commit()

# Connect to MySQL database
conn = mysql.connector.connect(
    host="travel.bryceschultz.com",
    user="codenome",
    password="Codenome!1",
    database="codenome_testing"
)
cursor = conn.cursor()

# Fill in location data
fill_location_data(conn, cursor)

# Close cursor and connection
cursor.close()
conn.close()
