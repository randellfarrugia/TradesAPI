

# Trades API

This project consists of an API and a console application. The api accepts request on 3 endpoints

```GET /trades```
```GET /trades{id}```
```POST /trades ```

For creating a new trade, the below sample can be used :

```
{
    "User": "john_doe",
    "CurrencyCode": "USD",
    "Amount": 123.45,
    "Fee": 8.0,
    "TradeDate": "2024-12-08T00:00:00Z"
}
```

## Setup

This project runs a .net application inside a docker container. Inside the same container there is an MSSQL instance and a RabbitMQ instance. These are started automatically with the container by running the below commands

```
docker build -t tradesapi -f TradesApi.Api/Dockerfile .
docker build -t tradesapi -f TradeLogger.ConsoleApp/Dockerfile .

docker-compose up --build
```

Once the container is up, run the [init sql script](https://github.com/randellfarrugia/TradesAPI/blob/main/sql-scripts/init.sql) found in [/sql-scripts](https://github.com/randellfarrugia/TradesAPI/tree/main/sql-scripts) to initialize the Database.
In order to connect to the database, use any sql management tool and connect by entering the below credentials

```
host : localhost, <your database port from docker as shown below>
username : sa
password : SecurePassword123!
```
![DockerDBPORT](https://i.ibb.co/1dWHkcd/image.png)

## Usage

Create a request on postman using the request above to create a trade. To get all trades simply hit /trades with a GET request and if you want a specific trade you can perform a GET on /trades/{tradeid}

Alternatively, there is also a ```/swagger``` endpoint which can be used to test these requests

The console app - Trade Logger will log any trades that are created by the API. To view these logs simply go on the docker container and click on View Details on the trade-logger container

![DockerTradeLoggerLogs](https://i.ibb.co/hytYvsY/image.png)

In order to login to the RabbitMQ console, simply click the link from the docker container and login using username : guest, password guest

### Note about Security

The passwords and secrets have been commited to github in plain text in a .env file for simplicity of this project. In a real world scenario these would be retrieved via something along the lines of a secret manager
