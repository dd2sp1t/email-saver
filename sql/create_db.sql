CREATE DATABASE email_saver_db

USE email_saver_db

CREATE TABLE email(
	id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
	registration_date DATETIME NOT NULL,
	sender NVARCHAR(100) NOT NULL,
	recipient NVARCHAR(100) NOT NULL,
	subject NVARCHAR(100) NOT NULL,
	text NVARCHAR(2000) NOT NULL,
	tags NVARCHAR(2000) NOT NULL
)

CREATE TABLE tag (
	id UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
	name NVARCHAR(50) NOT NULL
)

CREATE TABLE email_tag (
	email_id UNIQUEIDENTIFIER,
	tag_id UNIQUEIDENTIFIER
	
	CONSTRAINT pk_et PRIMARY KEY (email_id, tag_id),
	CONSTRAINT fk_et_email_id FOREIGN KEY (email_id) REFERENCES email (id) ON DELETE CASCADE,
	CONSTRAINT fk_et_tag_id FOREIGN KEY (tag_id) REFERENCES tag (id) ON DELETE CASCADE
)