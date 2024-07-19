CREATE DATABASE RedditDB;
GO

USE RedditDB;
GO

CREATE TABLE SubredditEntity (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nickname NVARCHAR(255) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Created DATETIME NOT NULL,
    Updated DATETIME NULL
);
GO

CREATE TABLE SubredditTopicEntity (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    SubredditId BIGINT NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Url NVARCHAR(2048) NULL,
    UpvoteRatio DECIMAL(5, 2) NOT NULL,
    Upvotes INT NOT NULL,
    Created DATETIME NOT NULL,
    Updated DATETIME NULL,
    FOREIGN KEY (SubredditId) REFERENCES SubredditEntity(Id)
);
GO