USE email_saver_db;
GO
CREATE PROCEDURE sp_delete_email
	@id UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM email WHERE email.id = @id
END;