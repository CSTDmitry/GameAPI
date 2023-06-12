CREATE PROCEDURE [dbo].[spClients_CheckExist]
  @ClientEmail nvarchar(50)
AS
BEGIN
  SELECT Id, ClientName, PasswordHash, PasswordSalt FROM dbo.[Clients] WHERE ClientEmail = @ClientEmail
END
