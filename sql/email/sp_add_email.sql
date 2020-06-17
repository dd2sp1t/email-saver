USE email_saver_db;
GO
CREATE PROCEDURE sp_add_email
	@registration_date DATETIME,
	@sender NVARCHAR(100),
	@recipient NVARCHAR(100),
	@subject NVARCHAR(100),
	@text NVARCHAR(2000),
	@tags NVARCHAR(2000)
AS
BEGIN
	INSERT INTO email(registration_date, sender, recipient, subject, text, tags)
		OUTPUT inserted.id
		VALUES(@registration_date, @sender, @recipient, @subject, @text, @tags)
END;