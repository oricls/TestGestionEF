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

--IF OBJECT_ID('AddTask', 'P') IS NOT NULL
--    DROP PROCEDURE AttributeTaskToUser;
--GO

-- =============================================
-- Author:		Oriane Clesse
-- Create date: 04/02/2026
-- Description:	Permet d'ajouter une tâche en bd. Quand une tache est crée, elle n'est attibuée à aucun utilisateur et son état IsCompted est init à false
-- =============================================
CREATE PROCEDURE AddTask
	-- Add the parameters for the stored procedure here
	@TaskName NVARCHAR(100),
	@TaskDescription NVARCHAR(350)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.Tasks (Name, Description)
	VALUES(@TaskName, @TaskDescription);
END
GO
