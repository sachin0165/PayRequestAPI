# PayRequest Web API

### PayRequest (PayStrax) Web API

PayRequest is an e-invoicing engine to send and get paid in crypto.

## Inspiration

We wanted to build something that users can use to send and get paid in Cryptocurrencies, and also tracks financial data. Not just limited to that but reduce human errors while sending tokens and typing wallet addresses.

## Blockchain In Use

We have used smart contracts to send payments with tracking every record using a smart contract. We have also used the off-chain database to query the dataset. (Sending every request to the blockchain would be expensive)

## Configuration

- ASP.NET Core 2.2 SDK
- MSSQL server 
- Visual Studio 2017
- Strati Blockchain 

## Frontend

We have used Angular 12 for front-end. The frontend application located in another repository here: [Web Frontend](https://github.com/sachin0165/PayRequest-Web-Demo)

## Backend

We have used ASP.NET Core 2.2 API as our backend.

## Database Script

Run database script (located in the repo) to create require schema and user data. We have default users as below:

```
User 1
Email: roger@gmail.com
Password: Admin

User 2
Email: cz@gmail.com
Password: Admin

User 3
Email: justin@gmail.com
Password: Admin
```
**Note:**
> Make sure to set ConnectionString And SmartContract Address to the `apssettings.json` file 

## API Methods
We have below methods to interact with the SmartContract endpoints.
### Login
Using the Login method you can get an access token along with the user wallet address.

```
curl --location --request POST 'http://localhost:58433/api/auth/login' \
--header 'Content-Type: application/json' \
--data-raw '{
    "email": "justin@gmail.com",
    "password": "Admin"
}'
```
### Create Request
Using this method user can create a payment request.

```
curl --location --request POST 'http://localhost:58433/api/payment-request/create' \
--header 'Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9' \
--header 'Content-Type: application/json' \
--data-raw '{
    "reason": "Work Invoice",
    "fromAddress": "PUh7CgvkiDouGTJgFGNMnksdgmFZuAQ1hu",
    "toAddress": "PD7vaHkcUR7eTW8q2yP7gJ8BUPB1gkQUqK",
    "amount": 10,
    "expiry": "2021-08-18"
}'
```
## Get All Requests
This endpoint returns available requests from the database.

```
curl --location --request GET 'http://localhost:58433/api/payment-request/get-all' \
--header 'Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9'
```
## Get Request Detail by Guid
This endpoint will return request details base on `guid`.

```
curl --location --request GET 'http://localhost:58433/api/payment-request/b855b91a-4c95-4aef-be78-14fd2fe6ba6e' \
--header 'Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9'
```
## Cancel Request
If a request creator wants to cancel the request, they can perform a cancel action using this endpoint.

```
curl --location --request PUT 'http://localhost:58433/api/payment-request/B855B91A-4C95-4AEF-BE78-14FD2FE6BA6E/cancel' \
--header 'Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9' \
--data-raw ''
``` 

## Pay Request

A recipient can pay for the request using this endpoint.
```
curl --location --request PUT 'http://localhost:58433/api/payment-request/1BA2FCA5-BE2B-4B94-8C08-F0C0703C5633/pay' \
--header 'Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9' \
--data-raw ''
```
## Get Balance

This endpoint helps to get the user balance of particular tokens.
```
curl --location --request GET 'http://localhost:58433/api/payment-request/get-balance' \
--header 'Authorization: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9'
```