# Json to SQL Server (SSIS C# Script code)

## Description
This project implements a script task for SQL Server Integration Services (SSIS) that consumes data from a web service and loads it into a SQL Server database. It uses basic authentication to connect to the web service and performs bulk inserts into the specified target table in an efficient way that directly connects the source endpoint with the target table without using any space in the memory.

## Requirements
- Microsoft SQL Server
- SQL Server Integration Services (SSIS)
- Access to a web service that provides data in JSON format

## Configuration
Before running the script, make sure to configure the following variables in the SSIS project:
- `EndpointUser`: Username for web service authentication.
- `EndopintPass`: Password for web service authentication.
- `SqlDestinationString`: Connection string to the SQL Server database.
- `TargetTable`: Name of the table in the database where the data will be loaded.
- `Entity`: Specific data entity to request from the web service.
- `ExecutionDate`: Execution date for delta data (if applicable).
- `ModeDelta`: Boolean indicating whether to run in delta mode.

## Usage
1. Configure the project and user variables according to your environment and specific data needs.
2. Deploy the SSIS package to your SQL Server environment.
3. Execute the package to start the data loading process.

## Error Handling
The script includes detailed error handling that captures failures both in connecting to the web service and in the data loading process, ensuring that any issues are logged appropriately for review.

## Contributions
Contributions to the project are welcome. If you wish to contribute, please fork the repository, make your changes, and submit a pull request.

## License
This project is licensed under the MIT License. See the `LICENSE` file for more details.


