# Author: Stephen Thomson
# Date: 2/17/2023
# Purpose: Populates pin_tags with bathroom pins from pins.

import mysql.connector

# Connect to MySQL database
mydb = mysql.connector.connect(
    host="travel.bryceschultz.com",
    user="codenome",
    password="Codenome!1",
    database="codenome_testing"
    )

# Create cursor
mycursor = mydb.cursor()

# Select rows from pins table that have 1 in the bathroom column
mycursor.execute("SELECT ID FROM pins WHERE tag_bathroom = 1")

# Fetch all rows that match the query
rows = mycursor.fetchall()
#print(rows)
#input("Press Enter to continue....")
      

# Loop through rows and add to pin_tags table
for row in rows:
    # Insert ID and tag_id=1 into pin_tags table
    sql = "INSERT INTO pin_tags (pin_id, tag_id) VALUES ({}, 1)".format(row[0])
    print("SQL: ", sql)
    mycursor.execute(sql)

    # Print contents of pin_tags table
    #mycursor.execute("SELECT * FROM pin_tags")
    #results = mycursor.fetchall()
    #print("Contents of pin_tags table:")
    #for row in results:
        #print(row)
        
    #input("Press Enter to continue...")
    
# Commit changes to the database
mydb.commit()

# Close cursor and database connection
mycursor.close()
mydb.close()



