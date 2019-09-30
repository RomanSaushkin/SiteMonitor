CREATE DATABASE SiteMonitor
GO

USE SiteMonitor

CREATE TABLE dbo.SiteConfiguration 
(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	SiteUrl VARCHAR(MAX) NOT NULL,
	SiteStatusCheckIntervalTypeId INT NOT NULL,
	SiteStatusCheckInterval INT NOT NULL,
	LastUpdated DATETIME NOT NULL
)

CREATE TABLE dbo.SiteStatusCheckIntervalType
(
	Id INT NOT NULL PRIMARY KEY,
	Name VARCHAR(100) NOT NULL
)

INSERT INTO dbo.SiteStatusCheckIntervalType
(
	Id,
	Name
)
VALUES
	(1, 'Seconds'),
	(2, 'Minutes'),
	(3, 'Hours')


ALTER TABLE dbo.SiteConfiguration
   ADD CONSTRAINT FK_SiteConfiguration_SiteStatusCheckIntervalTypeId FOREIGN KEY (SiteStatusCheckIntervalTypeId)
      REFERENCES dbo.SiteStatusCheckIntervalType (Id)

INSERT INTO dbo.SiteConfiguration
(
	SiteUrl,
	SiteStatusCheckIntervalTypeId,
	SiteStatusCheckInterval,
	LastUpdated
)
VALUES
	('https://www.google.com/', 1, 30, GETUTCDATE()),
	('https://mail.ru/', 2, 1, GETUTCDATE()),
	('https://fakesitename.com/', 2, 3, GETUTCDATE())