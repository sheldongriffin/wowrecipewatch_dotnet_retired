/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
BULK INSERT Races
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\Races.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO
BULK INSERT Realms
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\Realms.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO
BULK INSERT Tradeskills
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\Tradeskills.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO
BULK INSERT Classes
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\Classes.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO
BULK INSERT Characters
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\Characters.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO
BULK INSERT CharacterTradeskills
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\CharacterTradeskills.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO
BULK INSERT CharacterRecipes
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\CharacterRecipes.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO
BULK INSERT Items
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\Items.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO
BULK INSERT Recipes
FROM '$(OutputPath)\sql\$(DeploymentConfiguration)\Data\Recipes.csv'
WITH (
	FIELDTERMINATOR = '\t',
	KEEPNULLS, KEEPIDENTITY
)
GO