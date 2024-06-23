# Questionnaire
 
 This is an assessment project

 The Environment:

 1. Tools :
	1. .Net Core 8
	2. Visual Studio 2022
	
2.	Database
	1. Azure Cosmos DB NoSql
    	2. All the connection strings, databases, containers are all in the appsetting.
        3. To use this project and run successfully, please, change the cosmos setting in the appsetting to ypur either emulator in your local or your micrososoft cosmos credential 

3. Project Structure 
	Clean Architecture is used
	The project contains three main Folders

		1.	Questionnaire.API
			This conatins the controller and appsettings
			Controllers are:
				1. QuestionTye controller : This is the CRUD for Question Types
				2. Template : This is the CRUD for setting up question template
				3. Program controller: This is the CRUD for Program
				4. Application controller: This is used for client to submit Personal Information and the questionnaire

		2.	Questionnaire.Core 
			This contains all the DTOs, Interfaces and Models

		3. Questionnaire.Infstructure
			This contains Configurations, Data and the Repositories




