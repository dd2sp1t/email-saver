USE email_saver_db;
GO
CREATE PROCEDURE sp_get_emails_for_period
	@start_date DATETIME,
	@end_date DATETIME
AS
BEGIN
	SELECT id, registration_date, sender, recipient, subject, text, tags
	FROM email e
	WHERE registration_date BETWEEN @start_date AND @end_date
END;