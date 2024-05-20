# Json to SQL Server (SSIS C# Script code)

## Overview
This project implements a script task for SQL Server Integration Services (SSIS) that consumes data from a web service and loads it into a SQL Server database. It uses basic authentication to connect to the web service and performs bulk inserts into the specified target table in an efficient way that directly connects the source endpoint with the target table without using any space in the memory. 

## Useful implementation knowledge 
- Bulkcopy item needs a live stream data connection as source object.
- IDataReader is the DataReader interface. It provides the specifications for implementing non-buffered, forward-only, read-only access to data retrieved from a data source, like a StreamReader.
- For each entity in your endpoint source, you'll need one object class and one reader class inside the script. Follow the provided code as guide.
- I strongly recommend implementing the BaseJsonReader class too, since it will implement all the unnecessary stuff from the IDataReader interface. Therefore your classes will be easier to read.

## Requirements
- Microsoft SQL Server
- SQL Server Integration Services (SSIS)
- Access to a web service that provides data in JSON format

## Configurations
Before running the script, make sure to configure the following variables in the SSIS project:
- `EndpointUser`: Username for web service authentication.
- `EndopintPass`: Password for web service authentication.
- `SqlDestinationString`: Connection string to the SQL Server database.
- `TargetTable`: Name of the table in the database where the data will be loaded.
- `Entity`: Http header example.
- `ExecutionDate`: Http header example.
- `ModeDelta`: Http header example.

## Usage
1. Configure the project and user variables according to your environment and specific data needs.
2. Deploy the SSIS package to your SQL Server environment.
3. Execute the package to start the data loading process.

## Error Handling
The script includes detailed error handling that captures failures both in connecting to the web service and in the data loading process, ensuring that any issues are logged appropriately for review.
