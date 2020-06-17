USE email_saver_db;
GO

CREATE PROCEDURE sp_add_email_tag
	@email_id UNIQUEIDENTIFIER,
	@tag_id UNIQUEIDENTIFIER
AS
BEGIN
	INSERT INTO email_tag(email_id, tag_id)
		OUTPUT inserted.email_id /* ----> kostyl <---- */
		VALUES(@email_id, @tag_id)
END;