# Author: Stephen Thomson
# Date: 2/3/2023
# Purpose: Goes through the pins table, finds latitude's and longitude's
#           that match where one has a bathroom tag and the other has a
#           wifi tag, and combines them into a single entry with both tags.


import mysql.connector

# Connect to the database
conn = mysql.connector.connect(user='codenome', password='Codenome!1',
                              host='travel.bryceschultz.com', database='codenome_testing')


# Fetch all rows with tag_bathroom = 1
query = "SELECT id, latitude, longitude FROM pins WHERE tag_bathroom = 1"
cursor.execute(query)
rows_with_bathroom = cursor.fetchall()

# Fetch all rows with tag_wifi = 1
query = "SELECT id, latitude, longitude FROM pins WHERE tag_wifi = 1"
cursor.execute(query)
rows_with_wifi = cursor.fetchall()

# Create a dictionary to store the merged rows
combined_rows = {}
for id, latitude, longitude in rows_with_bathroom:
    combined_rows[(latitude, longitude)] = (id, 1, 1)
for id, latitude, longitude in rows_with_wifi:
    if (latitude, longitude) in combined_rows:
        continue
    combined_rows[(latitude, longitude)] = (id, 0, 1)

# Update the merged rows
for id, tag_bathroom, tag_wifi in combined_rows.values():
    query = f"UPDATE pins SET tag_bathroom = {tag_bathroom}, tag_wifi = {tag_wifi} WHERE id = {id}"
    cursor.execute(query)

# Delete all duplicate rows
for latitude, longitude in combined_rows.keys():
    query = f"DELETE FROM pins WHERE latitude = {latitude} AND longitude = {longitude} AND id NOT IN (SELECT MIN(id) FROM pins WHERE latitude = {latitude} AND longitude = {longitude} GROUP BY latitude, longitude)"
    cursor.execute(query)

# Commit the changes and close the connection
conn.commit()
cursor.close()
conn.close()
