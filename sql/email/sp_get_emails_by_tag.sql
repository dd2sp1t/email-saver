USE email_saver_db;
GO
CREATE PROCEDURE sp_get_emails_by_tag
	@tag NVARCHAR(100)
AS
BEGIN
	SELECT e.id, registration_date, sender, recipient, subject, text, tags
	FROM email_tag et
	JOIN tag t
		ON et.tag_id = t.id
		AND t.name = @tag
	JOIN email e
		ON et.email_id = e.id
END;