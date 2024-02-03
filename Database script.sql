CREATE TABLE ContactData (
	Id INT IDENTITY(1,1) PRIMARY KEY, 
	FullName VARCHAR(255),
	PhoneNumber VARCHAR(20),
	Birthdate DATE
	);
GO

CREATE PROCEDURE CustomSelect
AS
BEGIN
SELECT *
FROM ContactData
END;
GO


CREATE PROCEDURE AddContact
    @Name VARCHAR(255),
    @number VARCHAR(20),
	@date DATE
AS
BEGIN
    INSERT INTO ContactData
	VALUES (@Name,@number, @date);
END;
GO


CREATE PROCEDURE DeleteContact
    @Id INT
AS
BEGIN
    DELETE FROM ContactData
	WHERE Id = @Id
END;
GO


CREATE PROCEDURE UpdateContact
    @Id INT,
	@newName VARCHAR(255),
    @newNumber VARCHAR(20),
	@newDate DATE
AS
BEGIN
UPDATE ContactData
SET FullName = @newName, PhoneNumber = @newNumber, Birthdate = @newDate
WHERE Id = @Id
END;
GO