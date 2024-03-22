# Author: Stephen Thomson
# Date: 2/3/2023
# Purpose: Goes through the pins table, finds repeat entries, and removes the duplicates
#           of both wifi and bathroom tags.

import mysql.connector

# Connect to the database
connection = mysql.connector.connect(user='codenome', password='Codenome!1',
                              host='travel.bryceschultz.com', database='codenome_testing')


try:
    #Run through bathroom tags
    with connection.cursor() as cursor:
        # Select rows with duplicate latitude and longitude values
        sql = """
            SELECT ROUND(latitude, 1), ROUND(longitude, 1), COUNT(*)
            FROM pins
            WHERE tag_bathroom = 1
            GROUP BY ROUND(latitude, 1), ROUND(longitude,1)
            HAVING COUNT(*) > 1;
        """
        cursor.execute(sql)
        duplicates = cursor.fetchall()

        # Remove the duplicate rows
        for duplicate in duplicates:
            latitude, longitude, count = duplicate
            print(f"Latitude: {latitude}, Longitude: {longitude}")
            input("Enter to continue")
            sql = f"""
                DELETE FROM pins
                WHERE latitude = {latitude} AND longitude = {longitude} AND tag_bathroom = 1
                LIMIT {count-1};
            """
            cursor.execute(sql)

    # Commit the changes
    connection.commit()

    #Run through wifi tags
    with connection.cursor() as cursor:
        # Select rows with duplicate latitude and longitude values
        sql = """
            SELECT latitude, longitude, COUNT(*)
            FROM pins
            WHERE tag_wifi = 1
            GROUP BY latitude, longitude
            HAVING COUNT(*) > 1;
        """
        cursor.execute(sql)
        duplicates = cursor.fetchall()

        # Remove the duplicate rows
        for duplicate in duplicates:
            latitude, longitude, count = duplicate
            sql = f"""
                DELETE FROM pins
                WHERE latitude = {latitude} AND longitude = {longitude} AND tag_bathroom = 1
                LIMIT {count-1};
            """
            cursor.execute(sql)

    # Commit the changes
    connection.commit()

    #Run through wifi tags
    with connection.cursor() as cursor:
        # Select rows with duplicate latitude and longitude values
        sql = """
            SELECT latitude, longitude, COUNT(*)
            FROM pins
            WHERE tag_wifi = 1
            GROUP BY latitude, longitude
            HAVING COUNT(*) > 1;
        """
        cursor.execute(sql)
        duplicates = cursor.fetchall()

        # Remove the duplicate rows
        for duplicate in duplicates:
            latitude, longitude, count = duplicate
            sql = f"""
                DELETE FROM pins
                WHERE latitude = {latitude} AND longitude = {longitude} AND tag_wifi = 1
                LIMIT {count-1};
            """
            cursor.execute(sql)

    # Commit the changes
    connection.commit()
finally:
    # Close the connection
    connection.close()
