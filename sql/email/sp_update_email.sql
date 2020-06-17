USE email_saver_db;
GO
CREATE PROCEDURE sp_update_email
	@id UNIQUEIDENTIFIER,
	@registration_date DATETIME,
	@sender NVARCHAR(100),
	@recipient NVARCHAR(100),
	@subject NVARCHAR(100),
	@text NVARCHAR(2000),
	@tags NVARCHAR(2000)
AS
BEGIN
	UPDATE email
		SET	registration_date = @registration_date,
			sender = @sender,
			recipient = @recipient,
			subject = @subject,
			text = @text,
			tags = @tags
		WHERE id = @id
END;