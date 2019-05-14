# Hair Salon Website

#### Epicodus Independent Project C# Week 3: A website for hair salons to track stylists and their clients.

#### By Sarah Angel

## Description

#### This program will allow a hair salon to create a list of stylists and add clients to the individual stylists.

## Specs
* The program can collect a stylists name.
* The program can store the stylist to a list of other stylists names.
* The program can store the stylist list to a database.
* The program can collect a client name under an individual stylists name.
* The program can collect multiple client names as a list under the stylist.
* The program can store the client information to a database.

## Setup/Installation Requirements

### Re-Create database/tables
1. Start MAMP and click Start Servers.
2. Run Application in console.
3. In MySql run following commands to build database and tables:
\ > CREATE DATABASE sarah_angel;
\ > USE sarah_angel;
\ > CREATE TABLE stylists (id serial PRIMARY KEY, stylistName VARCHAR(255));
\ > CREATE TABLE clients (id serial PRIMARY KEY, clientName VARCHAR(255), stylist_id INT);

### Other Requirements
1. Download .NET Core 2.2.103 Sdk install it.
2. Clone https://github.com/sangel217/HairSalon.Solution.
3. To run the program, navigate to the location of the HairSalon folder then execute: $ dotnet add package MySqlConnector / $ dotnet restore / $ dotnet build / $ dotnet run.
4. Open Url localhost:5000.

## Known Bugs
- Client name doesn't appear on Views/Clients/Show.cshtml

## Technologies Used
* C#
* .NET Core App 2.2.103 and ASP.NET Core
* Atom text editor
* github

## Support and contact

Email: sangel217@hotmail.com

## License
Copyright (c) 2019 Copyright Sarah Angel All Rights Reserved.
This software is licensed under the MIT license
