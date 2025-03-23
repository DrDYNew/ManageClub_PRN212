USE master
GO

CREATE DATABASE ManageClub
GO
USE [ManageClub]
GO

-- Tạo các bảng (giữ nguyên từ script của bạn, chỉ sửa lỗi cú pháp nhỏ)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[AttendanceID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[CheckInTime] [datetime] NULL,
	[CheckOutTime] [datetime] NULL,
PRIMARY KEY CLUSTERED ([AttendanceID] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ClubFinances](
	[FinanceID] [int] IDENTITY(1,1) NOT NULL,
	[ClubID] [int] NOT NULL,
	[TransactionType] [nvarchar](10) NULL,
	[Amount] [decimal](10, 2) NOT NULL,
	[Description] [text] NULL,
	[TransactionDate] [datetime] NULL,
PRIMARY KEY CLUSTERED ([FinanceID] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[ClubMembers](
	[MembershipID] [int] IDENTITY(1,1) NOT NULL,
	[ClubID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[JoinDate] [datetime] NULL,
	[Member_Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED ([MembershipID] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Clubs](
	[ClubID] [int] IDENTITY(1,1) NOT NULL,
	[ClubName] [varchar](100) NOT NULL,
	[Description] [text] NULL,
	[EstablishedDate] [date] NULL,
	[PresidentID] [int] NULL,
	[ClubStatus] [nvarchar](20) NOT NULL,
	[Total_cost] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED ([ClubID] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[EventFeedback](
	[FeedbackID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Rating] [int] NULL,
	[Comments] [text] NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED ([FeedbackID] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[EventParticipants](
	[EventParticipantID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Reason] [nvarchar](200) NULL,
	[RegistrationDate] [datetime] NULL,
	[Status] [nvarchar](15) NOT NULL,
PRIMARY KEY CLUSTERED ([EventParticipantID] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Events](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[EventName] [varchar](100) NOT NULL,
	[Description] [text] NULL,
	[EventDate] [datetime] NOT NULL,
	[Location] [nvarchar](200) NOT NULL,
	[OrganizerID] [int] NOT NULL,
	[ClubID] [int] NULL,
	[MaxParticipants] [int] NULL,
	[Event_Status] [nvarchar](20),
PRIMARY KEY CLUSTERED ([EventID] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Reports](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[ClubID] [int] NOT NULL,
	[Semester] [nvarchar](20) NOT NULL,
	[MemberChanges] [text] NULL,
	[EventSummary] [text] NULL,
	[ParticipationStats] [text] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED ([ReportID] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED ([RoleID] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[RoleID] [int] NOT NULL,
	[DateOfBirth] [date] NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[Address] [text] NULL,
	[AvatarURL] [nvarchar](MAX) NULL,
	[DateJoined] [datetime] NULL,
	[Status] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED ([UserID] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ClubFinances] ADD DEFAULT (GETDATE()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[ClubMembers] ADD DEFAULT (GETDATE()) FOR [JoinDate]
GO
ALTER TABLE [dbo].[EventFeedback] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[EventParticipants] ADD DEFAULT (GETDATE()) FOR [RegistrationDate]
GO
ALTER TABLE [dbo].[Reports] ADD DEFAULT (GETDATE()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD DEFAULT (GETDATE()) FOR [DateJoined]
GO

ALTER TABLE [dbo].[Attendance] WITH CHECK ADD FOREIGN KEY([EventID]) REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[Attendance] WITH CHECK ADD FOREIGN KEY([UserID]) REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ClubFinances] WITH CHECK ADD FOREIGN KEY([ClubID]) REFERENCES [dbo].[Clubs] ([ClubID])
GO
ALTER TABLE [dbo].[ClubMembers] WITH CHECK ADD FOREIGN KEY([ClubID]) REFERENCES [dbo].[Clubs] ([ClubID])
GO
ALTER TABLE [dbo].[ClubMembers] WITH CHECK ADD FOREIGN KEY([UserID]) REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Clubs] WITH CHECK ADD FOREIGN KEY([PresidentID]) REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[EventFeedback] WITH CHECK ADD FOREIGN KEY([EventID]) REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[EventFeedback] WITH CHECK ADD FOREIGN KEY([UserID]) REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[EventParticipants] WITH CHECK ADD FOREIGN KEY([EventID]) REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[EventParticipants] WITH CHECK ADD FOREIGN KEY([UserID]) REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Events] WITH CHECK ADD FOREIGN KEY([ClubID]) REFERENCES [dbo].[Clubs] ([ClubID])
GO
ALTER TABLE [dbo].[Events] WITH CHECK ADD FOREIGN KEY([OrganizerID]) REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Reports] WITH CHECK ADD FOREIGN KEY([ClubID]) REFERENCES [dbo].[Clubs] ([ClubID])
GO
ALTER TABLE [dbo].[Users] WITH CHECK ADD FOREIGN KEY([RoleID]) REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[EventFeedback] WITH CHECK ADD CHECK (([Rating]>=(1) AND [Rating]<=(5)))
GO

SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED ([RoleName] ASC)
GO
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED ([Email] ASC)
GO

-- Insert dữ liệu theo thứ tự logic
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT INTO [dbo].[Roles] ([RoleID], [RoleName]) 
VALUES 
    (1, N'Admin'),
    (2, N'Club President'),
    (3, N'Member'),
    (4, N'User')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO

SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [AvatarURL], [Status])
VALUES 
    (1, N'John Doe', 'a@gmail.com', '1', 1, '1990-05-15', '1234567890', '123 Main St', NULL, 'Active'),
    (2, N'Jane Smith', 'b@gmail.com', '1', 2, '1992-08-22', '0987654321', '456 Oak St', NULL, 'Active'),
    (3, N'Mike Johnson', 'c@gmail.com', '1', 3, '1995-03-10', '5551234567', '789 Pine St', NULL, 'Active'),
    (4, N'Emily Brown', 'd@gmail.com', '1', 4, '1998-12-01', '4449876543', '101 Elm St', NULL, 'Inactive'),
    (5, N'David Lee', 'dungbd07@gmail.com', '1', 2, '1988-07-19', '3334567890', '202 Birch St', NULL, 'Active')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO

SET IDENTITY_INSERT [dbo].[Clubs] ON
INSERT INTO [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost])
VALUES 
    (1, 'Chess Club', 'A club for chess enthusiasts.', '2020-01-15', 2, 'Active', 0), 
    (2, 'Photography Club', 'Exploring the art of photography.', '2019-06-10', 5, 'Active', 0), 
    (3, 'Debate Club', 'Sharpen your debating skills.', '2021-03-20', 2, 'Active', 0),
    (4, 'Music Club', 'For music lovers and performers.', '2018-11-05', 5, 'Active', 0), 
    (5, 'Coding Club', 'Learn and build software projects.', '2022-09-01', 2, 'Active', 0)
SET IDENTITY_INSERT [dbo].[Clubs] OFF
GO

SET IDENTITY_INSERT [dbo].[ClubFinances] ON
INSERT INTO [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [Amount], [Description], [TransactionDate])
VALUES 
    (1, 1, 'Income', 200.00, 'Membership fees', '2025-03-01'),
    (2, 2, 'Expense', 150.50, 'Camera equipment', '2025-03-05'),
    (3, 3, 'Income', 100.25, 'Fundraising event', '2025-03-10'),
    (4, 4, 'Expense', 300.00, 'Venue rental', '2025-03-15'),
    (5, 5, 'Income', 250.75, 'Sponsorship', '2025-03-20')
SET IDENTITY_INSERT [dbo].[ClubFinances] OFF
GO

UPDATE [dbo].[Clubs]
SET [Total_cost] = (
    SELECT SUM(
        CASE 
            WHEN [TransactionType] = 'Income' THEN [Amount]
            WHEN [TransactionType] = 'Expense' THEN -[Amount]
            ELSE 0 
        END
    )
    FROM [dbo].[ClubFinances]
    WHERE [dbo].[ClubFinances].[ClubID] = [dbo].[Clubs].[ClubID]
)
WHERE EXISTS (
    SELECT 1 
    FROM [dbo].[ClubFinances] 
    WHERE [dbo].[ClubFinances].[ClubID] = [dbo].[Clubs].[ClubID]
)
GO

SET IDENTITY_INSERT [dbo].[Events] ON
INSERT INTO [dbo].[Events] ([EventID], [EventName], [Description], [EventDate], [Location], [OrganizerID], [ClubID], [MaxParticipants], [Event_Status])
VALUES 
    (1, 'Chess Tournament', 'Annual chess competition.', '2025-04-01 10:00:00', 'Room A-101', 2, 1, 20, 'Coming soon'),
    (2, 'Photo Walk', 'Outdoor photography event.', '2025-04-05 09:00:00', 'City Park', 5, 2, 15, 'Coming soon'),
    (3, 'Debate Night', 'Debate on current issues.', '2025-04-10 18:00:00', 'Auditorium', 2, 3, 30, 'Coming soon'),
    (4, 'Music Jam', 'Live music performances.', '2025-04-15 19:00:00', 'Club Room', 1, 4, 25, 'Coming soon'),
    (5, 'Code Hackathon', '24-hour coding challenge.', '2025-04-20 08:00:00', 'Lab B-202', 3, 5, 50, 'Coming soon')
SET IDENTITY_INSERT [dbo].[Events] OFF
GO

SET IDENTITY_INSERT [dbo].[ClubMembers] ON
INSERT INTO [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status])
VALUES 
    (1, 1, 3, '2020-02-01', 'Active');
    
SET IDENTITY_INSERT [dbo].[ClubMembers] OFF
GO

SET IDENTITY_INSERT [dbo].[Attendance] ON
INSERT INTO [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime])
VALUES 
    (1, 1, 1, '2025-04-01 10:00:00', '2025-04-01 12:00:00');
SET IDENTITY_INSERT [dbo].[Attendance] OFF
GO

SET IDENTITY_INSERT [dbo].[EventParticipants] ON
INSERT INTO [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status])
VALUES 
    (1, 1, 1, 'Interested in chess', '2025-03-20', 'attended');
    
SET IDENTITY_INSERT [dbo].[EventParticipants] OFF
GO

SET IDENTITY_INSERT [dbo].[EventFeedback] ON
INSERT INTO [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt])
VALUES 
    (1, 1, 1, 4, 'Great event, well organized.', '2025-04-01 13:00:00');
    
SET IDENTITY_INSERT [dbo].[EventFeedback] OFF
GO

SET IDENTITY_INSERT [dbo].[Reports] ON
INSERT INTO [dbo].[Reports] ([ReportID], [ClubID], [Semester], [MemberChanges], [EventSummary], [ParticipationStats], [CreatedDate])
VALUES 
    (1, 1, 'Spring 2025', '2 new members', 'Chess Tournament successful', '80% attendance', '2025-03-20'),
    (2, 2, 'Spring 2025', '1 member left', 'Photo Walk planned', '90% participation', '2025-03-20'),
    (3, 3, 'Spring 2025', 'No changes', 'Debate Night upcoming', '70% expected', '2025-03-20'),
    (4, 4, 'Spring 2025', '1 member inactive', 'Music Jam cancelled', '50% attendance', '2025-03-20'),
    (5, 5, 'Spring 2025', '3 new members', 'Hackathon scheduled', '85% participation', '2025-03-20')
SET IDENTITY_INSERT [dbo].[Reports] OFF
GO