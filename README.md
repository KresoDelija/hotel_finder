# Hotel API
Simple API with CRUD operations and list ordering by location and price

API has 2 controllers one for administration with methods for Create, Update, Delete, Load by ID and List all items and anouther for clients that only has one method that lists all hotels but orders them by given location and has implemented pagination.

API is writen with .net8 C# and with using EntityFramework to work with data. Data is stored in SQL database.

To simplify development project is dockerized and dev SQL database is deployed on SQL server that is in docker container that is started together with the API by docker-compose and API is using SQL sa account in that database and database is initialized with some example hotels.
