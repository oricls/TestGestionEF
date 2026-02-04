-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF OBJECT_ID('AttributeTaskToUser', 'P') IS NOT NULL
    DROP PROCEDURE AttributeTaskToUser;
GO

-- =============================================
-- Author:		Oriane Clesse
-- Create date: 04/02/2026
-- Description:	Permet d'attibuer une tâche à un utilisateur
-- =============================================
CREATE PROCEDURE AttributeTaskToUser 
	-- Add the parameters for the stored procedure here
	@Username NVARCHAR(100),
	@Taskname NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @UserId INT;
	DECLARE @TaskId INT;

	-- Recup l'utilisateur 
	SELECT @UserId = Id   
	FROM dbo.Users
	WHERE Name LIKE '%' + @Username + '%' OR FirstName LIKE '%' + @Username + '%'
	--WHERE Name = @Username

	IF @UserId IS NULL
    BEGIN
        RAISERROR('Utilisateur introuvable', 16, 1);
        RETURN;
    END

	-- Recup la tache
	SELECT @TaskId = Id
    FROM dbo.Tasks
    WHERE Name = @TaskName;

    IF @TaskId IS NULL
    BEGIN
        RAISERROR('Tâche introuvable', 16, 1);
        RETURN;
    END

	UPDATE dbo.Tasks
    SET UserId = @UserId
    WHERE Name = @Taskname;
	
END
GO
