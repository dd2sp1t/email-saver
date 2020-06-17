USE email_saver_db;
GO
CREATE PROCEDURE sp_get_email
	@id UNIQUEIDENTIFIER
AS
BEGIN
	SELECT id, registration_date, sender, recipient, subject, text, tags
	FROM email e
	WHERE e.id = @id
END;