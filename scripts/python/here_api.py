# Author: Stephen Thomson
# Date: 2/3/2023
# Purpose: Calculates the circles needed to cover the given
#           bounding box, then sends requests to the HERE API
#           to download bathroom and wifi locations.

from sys import argv, stderr, exit
from json import dumps
import logging
import http.client
import json
import mysql.connector
import math
http.client.HTTPConnection.debuglevel = 1

logging.basicConfig()
logging.getLogger().setLevel(logging.DEBUG)
requests_log = logging.getLogger('requests.packages.urllib3')
requests_log.propagate = True
requests_log.setLevel(logging.DEBUG)

try:
    from requests_oauthlib import OAuth1
except ImportError as e:
    raise ImportError(f'{e.msg}\nPackage available at https://pypi.org/project/requests-oauthlib')

try:
    from requests import get, post, HTTPError
except ImportError as e:
    raise ImportError(f'{e.msg}\nPackage available at https://pypi.org/project/requests')

# Connect/Insert wifi to database function
def insert_wifi(title, street, latitude, longitude):
    try:
        cnx = mysql.connector.connect(user='codenome', password='Codenome!1', host='travel.bryceschultz.com', database='codenome_testing')
        cursor = cnx.cursor()
     
        # Construct the INSERT statement
        stmt = "INSERT INTO pins (title, street, latitude, longitude, user_id, tag_wifi) VALUES (%s, %s, %s, %s, 0, 1)"
        values = (title, street, latitude, longitude)

        # Testing: print the values of the variables
        print("cnx:", cnx)
        print("cursor:", cursor)
        print("stmt:", stmt)
        print("values:", values)
    
        # Execute the statement
        result = cursor.execute(stmt, values)
        #Print result to test for error code
        print("result:", result)
    
        # Commit the changes to the database
        cnx.commit()

    except Exception as e:
        print("Error:", e)

    # Close the connection to the database
    cnx.close()

# Connect/Insert bathroom to database function
def insert_bathroom(title, street, latitude, longitude):
    try:
        cnx = mysql.connector.connect(user='codenome', password='Codenome!1', host='travel.bryceschultz.com', database='codenome_testing')
        cursor = cnx.cursor()
     
        # Construct the INSERT statement
        stmt = "INSERT INTO pins (title, street, latitude, longitude, user_id, tag_bathroom) VALUES (%s, %s, %s, %s, 0, 1)"
        values = (title, street, latitude, longitude)

        # Testing: print the values of the variables
        print("cnx:", cnx)
        print("cursor:", cursor)
        print("stmt:", stmt)
        print("values:", values)
    
        # Execute the statement
        result = cursor.execute(stmt, values)
        #Print result to test for error code
        print("result:", result)
    
        # Commit the changes to the database
        cnx.commit()

    except Exception as e:
        print("Error:", e)

    # Close the connection to the database
    cnx.close()
    

    
usage = """Usage:
    hgs_access_test.py <key-id> <key_secret>
"""
clid = "7rCT77hOpd9VBv8K0DAxag"
clsec = "OyOPKetreHVGT5Ry5koisPp84dI4OjD7wTNrHYDsSllkk54-P5_Blkq2KobpEA6QY0BPvkqyyBClzrS7xtU90w"

# Retrieve token
try:
    data = {
        'grantType': 'client_credentials',
        'clientId': clid,
        'clientSecret': clsec
        }
except IndexError:
    stderr.write(usage)
    exit(1)

response = post(
    url='https://account.api.here.com/oauth2/token',
    auth=OAuth1(clid, client_secret=clsec) ,
    headers= {'Content-type': 'application/json'},
    data=dumps(data)).json()

try:
    token = response['accessToken']
    token_type = response['tokenType']
    expire_in = response['expiresIn']
except KeyError as e:
    print(dumps(response, indent=2))
    exit(1)

# Use it in HERE Geocoding And Search query header
headers = {'Authorization': f'{token_type} {token}'}

# Box with Grid corner Coordinates
min_lat = 41.991794
max_lat = 46.292035
min_lng = -124.566244
max_lng = -116.463504

# Set Values and Calculate Number of Circles Needed
width = math.fabs(((max_lng * -1) - (min_lng * -1)) * 111.32)
height = math.fabs((max_lat - min_lat) * 111.32)
circle_radius = 250
circle_degrees = circle_radius / 111.32
box_area = width * height
circle_area = 3.14 * circle_radius * circle_radius
num_circles = math.floor(box_area / circle_area)

#Testing print
print("Min Lat: ", min_lat)
print("Max Lat: ", max_lat)
print("Min Lng: ", min_lng)
print("Max Lng: ", max_lng)
print("Width: ", width)
print("Height: ", height)
print("Radius: ", circle_radius)
print("Radius Degrees: ", circle_degrees)
print("Box Area: ", box_area)
print("Circle Area: ", circle_area)
print("num_circles: ",num_circles)

#Pause for testing
#cont = input("Enter to Cotinue. ")

# Set Start Point
x = min_lat + circle_degrees
y = min_lng + circle_degrees

#Testing Print
print("X-Lat: ", x)
print("Y-Lng: ", y)

#Pause for testing
cont = input("Enter to Cotinue. ")

# Generate center points
center_points = []
for i in range(int(num_circles)):
    #Append
    center_points.append((x, y))

    #Test Print
    print(center_points)
    print("Lat: ", x, "Lng: ", y)
    
    #Next Coordinates
    y += 2 * circle_degrees
    if y > max_lng:
        y = min_lng + circle_degrees
        x += 2 * circle_degrees

#Pause for testing
#cont = input("Enter to Cotinue. ")

# Iterate through the center points
for point in center_points:
    # Construct the search query
    search_query = f'https://discover.search.hereapi.com/v1/discover?in=circle:{point[0]},{point[1]};r=250000&q=wifi'

    #Print to test
    print("Search: ", search_query)
    
    #Peform Search
    search_results = dumps(get(search_query, headers=headers).json(), indent=2)

    # Parse JSON
    pdata = json.loads(search_results)

    #Test Print
    print(pdata)

    #Iterate through items to create place
    for item in pdata['items']:
        place = {
            'title': item['title'],
            'street': item['address']['label'],
            'latitude': item['position']['lat'],
            'longitude': item['position']['lng']
        }
        #Call the function
        insert_wifi(place['title'], place['street'], place['latitude'], place['longitude'])

        # Print for testing
        print(place)

# Iterate through the center points
for point in center_points:
    # Construct the search query
    search_query = f'https://discover.search.hereapi.com/v1/discover?in=circle:{point[0]},{point[1]};r=250000&q=bathroom'

    #Print to test
    print("Search: ", search_query)
    
    #Peform Search
    search_results = dumps(get(search_query, headers=headers).json(), indent=2)

    # Parse JSON
    pdata = json.loads(search_results)

    #Test Print
    print(pdata)

    #Iterate through items to create place
    for item in pdata['items']:
        place = {
            'title': item['title'],
            'street': item['address']['label'],
            'latitude': item['position']['lat'],
            'longitude': item['position']['lng']
        }
        #Call the function
        insert_bathroom(place['title'], place['street'], place['latitude'], place['longitude'])

    
    # Pause each iteration to choose continue or exit
    #cont = input("Enter to Continue, n to exit. ")
    #if cont.lower() == "n":
        #break

    
