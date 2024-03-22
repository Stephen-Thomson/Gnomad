import requests
import json

# Your Yelp Fusion API key
api_key = "Qsw9t_oiqQlr39da5yTCJKM5ldb8h8dZLzCy1ZmOPOOsZB7R3s5S22r199ZAry22LBzAjCB4AItJMOtHo0irGq1PA3Q_71g2vl3MlXTLmmIUzLJUT0S7A1CThaLRY3Yx"

# Coordinates for bounding box (min_lat, min_lon, max_lat, max_lon)
bounding_box = (41.991794, -124.566244, 46.292035, -116.463504)

# Yelp Fusion API endpoint for businesses search
endpoint = "https://api.yelp.com/v3/businesses/search"

# Search parameters
params = {
    "term": "bathroom",
    "categories": "toilets",
    "location": "oregon",
    "radius": 40000,
}

# Send GET request to Yelp Fusion API
headers = {
    "Authorization": f"Bearer {api_key}"
}
response = requests.get(endpoint, headers=headers, params=params)

# Check status code of the API response
if response.status_code != 200:
    print("API request failed with status code:", response.status_code)
    print("Response content:", response.text)
    exit()

# Parse JSON response
data = json.loads(response.text)

# Print businesses information
for business in data["businesses"]:
    print(business["name"])
    print(business["location"]["address1"])
    print(business["location"]["city"])
    print(business["location"]["state"])
    print(business["location"]["zip_code"])
    print()

