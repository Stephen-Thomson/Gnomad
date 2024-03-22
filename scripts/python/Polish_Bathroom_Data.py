# Author: Stephen Thomson
# Date: 2/3/2023
# Purpose: This program is to polish the bathroom data in the database.
# It makes sure the entire street address is in the address column.

import mysql.connector
import re

# Connect to the database
db = mysql.connector.connect(
    host="travel.bryceschultz.com",
    user="codenome",
    password="Codenome!1",
    database="codenome_testing"
)

# Create a cursor object to interact with the database
cursor = db.cursor()

# Select all rows where the tag_bathroom equals 1
query = "SELECT * FROM pins WHERE tag_bathroom = 1"

# Execute the query
cursor.execute(query)

# Loop through the results
for row in cursor.fetchall():
    # If the state abbreviation is anywhere in the street string
    if re.search(r'\b[A-Z]{2}\b', row[5]):
        # Update the street column with the extracted street
        street = re.sub(r'\b[A-Z]{2}\b', '', row[5]).strip()
        update_query = "UPDATE pins SET street = %s WHERE id = %s"
        cursor.execute(update_query, (street, row[0]))
    else:
        # Update the street column with the entire title string
        update_query = "UPDATE pins SET street = %s WHERE id = %s"
        cursor.execute(update_query, (row[4], row[0]))
    print("Running")

# Commit the changes to the database
db.commit()

# Close the cursor and database connections
cursor.close()
db.close()

