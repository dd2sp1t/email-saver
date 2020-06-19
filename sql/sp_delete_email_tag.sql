USE email_saver_db;
GO

CREATE PROCEDURE sp_delete_email_tag
	@email_id UNIQUEIDENTIFIER,
	@tag_id UNIQUEIDENTIFIER
AS

IF (SELECT COUNT(tag_id) FROM email_tag WHERE tag_id = @tag_id) = 1
	BEGIN
		DELETE FROM tag WHERE id = @tag_id /* ----> CASCADE ON DELETE <---- */
	END;
ELSE
	BEGIN
		DELETE FROM email_tag WHERE email_id = @email_id AND tag_id = @tag_id
	END;