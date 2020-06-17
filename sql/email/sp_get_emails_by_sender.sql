USE email_saver_db;
GO
CREATE PROCEDURE sp_get_emails_by_sender
	@sender NVARCHAR(100)
AS
BEGIN
	SELECT id, registration_date, sender, recipient, subject, text, tags
	FROM email e
	WHERE e.sender = @sender
END;