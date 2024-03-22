# Author: Stephen Thomson
# Date: 2/17/2023
# Purpose: Uses Google Geocoding to fill in missing data in the table using
#           the latitude and longitude of rows missing title and street.


import mysql.connector
import requests

# Connect to your MySQL database
db = mysql.connector.connect(
    host="travel.bryceschultz.com",
    user="codenome",
    password="Codenome!1",
    database="codenome_testing"
)

# Create a cursor to perform database operations
cursor = db.cursor()

# Execute a SELECT statement to get all the entries with N/A for title or street
select_query = "SELECT id, latitude, longitude FROM pins WHERE title = 'N/A' OR title = 'Toilets' OR street = 'N/A'"
cursor.execute(select_query)

# Fetch all the results from the SELECT statement
results = cursor.fetchall()

#print("Results: ", results)
#input("Press Enter to continue...")

# Loop through the results
for row in results:
    # Get the latitude and longitude from the result
    lat = row[1]
    lng = row[2]

    print("Lat: ", lat)
    print("Long: ", lng)
    #input("Press Enter to continue...")
    
    # Build the URL for the Google Maps Geocoding API
    url = f"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lng}&key=AIzaSyDGUEpAl5SkvjaNoXoQMZYTU3uuhnOCIOc"

    #print("URL: ", url)
    #input("Press Enter to continue...")
    
    # Send a GET request to the API
    response = requests.get(url)

    print("Response: ", response)
    #input("Press Enter to continue...")
    
    # Check if the request was successful
    if response.status_code == 200:
        # Parse the JSON response
        data = response.json()

        #print("Data: ", data)
        #input("Press Enter to continue...")
        
        if 'results' in data and len(data['results']) > 0:
            #print("If True")
            
            # Get the address components from the response
            address_components = data['results'][0]['address_components']

            # Get the name of the location
            title = data['results'][0]['formatted_address']

            print("Address Components: ", address_components)
            print("Title: ", title)
            #input("Press Enter to continue...")
    
        else:
            #print("Entered Else")
            # Handle the case where no results were found
            continue
        
        #Set street
        street = ''
        
        # Loop through the address components
        for component in address_components:
            # Check if the component is a street number
            if 'street_number' in component['types']:
                street += component['long_name']
            # Check if the component is a route (street name)
            elif 'route' in component['types']:
                street += f" {component['long_name']}"

        #print("Street: ", street)
        #input("Press Enter to continue...")

        # Check if street is empty and set it to title if it is
        if not street:
            street = title
            print("Street Empty")

        # Update the entry in the database with the new title and street
        update_query = "UPDATE pins SET title = %s, street = %s WHERE id = %s"
        values = (title, street, row[0])
        cursor.execute(update_query, values)
        
        # Commit the changes to the database
        db.commit()

        #input("Press Enter to continue...")

# Close the cursor and the database connection
cursor.close()
db.close()
