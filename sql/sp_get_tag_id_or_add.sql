USE email_saver_db;
GO
CREATE PROCEDURE sp_get_tag_id_or_add
	@name NVARCHAR(50)
AS
IF (SELECT COUNT(id) FROM tag WHERE name = @name) <> 0
	BEGIN
		SELECT id FROM tag WHERE name = @name
	END
ELSE
	BEGIN
		INSERT INTO tag(name)
			OUTPUT inserted.id
			VALUES(@name)
	END;