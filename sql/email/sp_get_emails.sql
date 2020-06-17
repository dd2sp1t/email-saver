USE email_saver_db;
GO
CREATE PROCEDURE sp_get_emails AS
BEGIN
	SELECT id, registration_date, sender, recipient, subject, text, tags
	FROM email e	
END;