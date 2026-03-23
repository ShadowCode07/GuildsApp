CREATE TABLE [User] (
  [UserId]       INT           NOT NULL,
  [Username]     VARCHAR(100)  NOT NULL,
  [DisplayName]  VARCHAR(100)  NOT NULL,
  [PasswordHash] VARCHAR(255)  NOT NULL,
  [CreatedAt]    DATETIME      NOT NULL,
  [LastLoginAt]  DATETIME      NULL,
  [XpTotal]      INT           NOT NULL DEFAULT 0,
  [Level]        INT           NOT NULL DEFAULT 1,
  [IsDeleted]    BIT           NOT NULL DEFAULT 0,
  PRIMARY KEY ([UserId])
);

CREATE TABLE [Community] (  
  [CommunityId]     INT          NOT NULL,
  [CreatedByUserId] INT          NOT NULL,
  [Slug]            VARCHAR(100) NOT NULL,
  [Name]            VARCHAR(100) NOT NULL,
  [IsPrivate]       BIT          NOT NULL DEFAULT 0,
  [IsArchived]      BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY ([CommunityId]),
  CONSTRAINT [FK_Community_CreatedByUserId]
    FOREIGN KEY ([CreatedByUserId])
      REFERENCES [User]([UserId])
);

CREATE TABLE [CommunityMember] (
  [UserId]      INT          NOT NULL,
  [CommunityId] INT          NOT NULL,
  [Role]        VARCHAR(50)  NOT NULL,
  [JoinedAt]    DATETIME     NOT NULL,
  [IsBanned]    BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY ([UserId], [CommunityId]),
  CONSTRAINT [FK_CommunityMember_UserId]
    FOREIGN KEY ([UserId])
      REFERENCES [User]([UserId]),
  CONSTRAINT [FK_CommunityMember_CommunityId]
    FOREIGN KEY ([CommunityId])
      REFERENCES [Community]([CommunityId])
);

CREATE TABLE [Post] (
  [PostId]       INT           NOT NULL,
  [AuthorUserId] INT           NOT NULL,
  [CommunityId]  INT           NOT NULL,
  [Title]        VARCHAR(300)  NOT NULL,
  [CreatedAt]    DATETIME      NOT NULL,
  [IsDeleted]    BIT           NOT NULL DEFAULT 0,
  PRIMARY KEY ([PostId]),
  CONSTRAINT [FK_Post_AuthorUserId]
    FOREIGN KEY ([AuthorUserId])
      REFERENCES [User]([UserId]),
  CONSTRAINT [FK_Post_CommunityId]
    FOREIGN KEY ([CommunityId])
      REFERENCES [Community]([CommunityId])
);

CREATE TABLE [Comment] (
  [CommentId]       INT      NOT NULL,
  [PostId]          INT      NOT NULL,
  [AuthorUserId]    INT      NOT NULL,
  [ParentCommentId] INT      NULL,
  [Body]            TEXT     NOT NULL,
  [IsDeleted]       BIT      NOT NULL DEFAULT 0,
  PRIMARY KEY ([CommentId]),
  CONSTRAINT [FK_Comment_PostId]
    FOREIGN KEY ([PostId])
      REFERENCES [Post]([PostId]),
  CONSTRAINT [FK_Comment_AuthorUserId]
    FOREIGN KEY ([AuthorUserId])
      REFERENCES [User]([UserId]),
  CONSTRAINT [FK_Comment_ParentCommentId]
    FOREIGN KEY ([ParentCommentId])
      REFERENCES [Comment]([CommentId])
);

CREATE TABLE [Session] (
  [SessionId]    INT           NOT NULL,
  [UserId]       INT           NOT NULL,
  [SessionToken] VARCHAR(255)  NOT NULL,
  [ExpiresAt]    DATETIME      NOT NULL,
  [IsRevoked]    BIT           NOT NULL DEFAULT 0,
  PRIMARY KEY ([SessionId]),
  CONSTRAINT [FK_Session_UserId]
    FOREIGN KEY ([UserId])
      REFERENCES [User]([UserId])
);

CREATE TABLE [PostVote] (
  [PostId] INT      NOT NULL,
  [UserId] INT      NOT NULL,
  [Value]  TINYINT  NOT NULL,
  PRIMARY KEY ([PostId], [UserId]),
  CONSTRAINT [FK_PostVote_PostId]
    FOREIGN KEY ([PostId])
      REFERENCES [Post]([PostId]),
  CONSTRAINT [FK_PostVote_UserId]
    FOREIGN KEY ([UserId])
      REFERENCES [User]([UserId])
);