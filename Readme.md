## Artemis
	Why Artemis?
	I chose something in Greek, and what's better from ancient Greek mythology reference about the protector of animals?
	My other idea was Schrodinger.
	Artemis project fetches from thecatpi a list of cats, maps it with needed items.

## Features
	- Fetches a list of cats and their related tags from thecatapi.com
	- Stores in local DB.
	- Swagger for api testing.
	- Unit testing

## Technologies
	- .NET Core 8
	- Entity Framework
	- Swagger
	- AutoMapper
	- MSSQL
	- nUnits
	- Moq

## Installation
	1. Clone the repository:
		git clone https://github.com/chersonio/Artemis-TheCatProject.git
	2. Create database:
		- change connection strings > Artemis.API > appsettings.json
		    "DefaultConnection": "Server=YourServer; Database=ArtemisDB; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=True"
		- Plug and play - I wish you luck - that means migrations and DB are created automatically with the correct server in ConStrings.
		Nontheless here are some more instructions
			in package manager console > select Artemis.Data write
			add-migration InitialCommit -StartupProject Artemis.API
			update-database
	3. To get your Api key visit the url below and create a free account
		https://thecatapi.com/ 
		When you get your token add it under appsettings.json > ApiSettings > Token
	4. Start testing > Run
		http://localhost:'port'/swagger/index.html
		add 'port' as found in Artemis.API > Properties > launchSettings.json


## Usage
	1. POST /api/cats/fetch:		fetches 25 cats, avoiding duplicates and stores it to DB
	2. GET /api/cats/ with id:		fetches a specified cat from id
	3. GET /api/cats:				a. fetches amount of cats using pagination
									b. if tag is added then it brings amount of cats that strictly match with tagName criteria using pagination


## Extra Notes
	Unit Testing - I added some cases but they are not complete. I did code first.
	I would like to also include:
		Exception handling (Custom exceptions)
		Correct error messages
		Logging (Serilog)
		Cover extra cases and refactor code and rough spots (Extract interfaces, services, decouple parts)
		Configure Docker file
		Better tagName search mechanism
		etc


### If there is any wrong spelling, it could be intentional!!1 :D 
