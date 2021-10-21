# dotnet-mmksi-middleware
This repo is sample middleware apps from PT. Berlian Sistem Infomasi who located in Yogyakarta. I make this because i want to compare performance between Go and .Net


## How to run
1. Clone this repository.
2. Type `dotnet watch run` at your terminal
3. Run `http://localhost:8080/signin` at your postman/insomnia.

## Project Structure
```
Middleware
└── Program.cs
└── Startup.cs
└── Models
└── transport       # standarization request from client and response data
|   ├── request
|   ├── response
└── Controller
```