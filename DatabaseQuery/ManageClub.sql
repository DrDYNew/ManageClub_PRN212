USE [ManageClub]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 24/03/2025 11:56:27 CH ******/
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
PRIMARY KEY CLUSTERED 
(
	[AttendanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClubFinances]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClubFinances](
	[FinanceID] [int] IDENTITY(1,1) NOT NULL,
	[ClubID] [int] NOT NULL,
	[TransactionType] [nvarchar](10) NULL,
	[price] [float] NOT NULL,
	[Description] [text] NULL,
	[TransactionDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[FinanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClubMembers]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClubMembers](
	[MembershipID] [int] IDENTITY(1,1) NOT NULL,
	[ClubID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[JoinDate] [datetime] NULL,
	[Member_Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[MembershipID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clubs]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clubs](
	[ClubID] [int] IDENTITY(1,1) NOT NULL,
	[ClubName] [varchar](100) NOT NULL,
	[Description] [text] NULL,
	[EstablishedDate] [date] NULL,
	[PresidentID] [int] NULL,
	[ClubStatus] [nvarchar](20) NOT NULL,
	[Total_cost] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ClubID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventFeedback]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventFeedback](
	[FeedbackID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Rating] [int] NULL,
	[Comments] [text] NULL,
	[CreatedAt] [datetime] NULL,
	[Status] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FeedbackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventParticipants]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventParticipants](
	[EventParticipantID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Reason] [nvarchar](200) NULL,
	[RegistrationDate] [datetime] NULL,
	[Status] [nvarchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EventParticipantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	[Event_Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[ClubID] [int] NOT NULL,
	[Semester] [nvarchar](20) NOT NULL,
	[MemberChanges] [text] NULL,
	[EventSummary] [text] NULL,
	[ParticipationStats] [text] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 24/03/2025 11:56:27 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	[DateJoined] [datetime] NULL,
	[Status] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Attendance] ON 

INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (1, 1, 1, CAST(N'2025-03-01T10:00:00.000' AS DateTime), CAST(N'2025-04-01T12:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (2, 1, 6, CAST(N'2025-03-01T10:10:00.000' AS DateTime), CAST(N'2025-04-01T12:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (3, 2, 7, CAST(N'2025-03-05T09:15:00.000' AS DateTime), CAST(N'2025-04-05T11:30:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (4, 3, 8, CAST(N'2025-03-10T18:10:00.000' AS DateTime), CAST(N'2025-04-10T20:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (5, 4, 9, CAST(N'2025-03-15T19:05:00.000' AS DateTime), CAST(N'2025-04-15T21:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (6, 5, 10, CAST(N'2025-03-20T08:20:00.000' AS DateTime), CAST(N'2025-04-20T10:45:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (7, 1, 11, CAST(N'2025-03-01T10:30:00.000' AS DateTime), CAST(N'2025-04-01T12:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (8, 2, 12, CAST(N'2025-03-05T09:40:00.000' AS DateTime), CAST(N'2025-04-05T11:50:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (9, 3, 13, CAST(N'2025-03-10T18:20:00.000' AS DateTime), CAST(N'2025-04-10T20:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (10, 4, 14, CAST(N'2025-03-15T19:30:00.000' AS DateTime), CAST(N'2025-04-15T21:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([AttendanceID], [EventID], [UserID], [CheckInTime], [CheckOutTime]) VALUES (11, 5, 15, CAST(N'2025-03-20T08:45:00.000' AS DateTime), CAST(N'2025-04-20T11:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Attendance] OFF
GO
SET IDENTITY_INSERT [dbo].[ClubFinances] ON 

INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (1, 1, N'Income', 200, N'Membership fees', CAST(N'2025-03-01T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (2, 1, N'Expense', 50, N'Chessboard purchase', CAST(N'2025-03-02T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (3, 2, N'Income', 150.5, N'Membership fees', CAST(N'2025-03-05T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (4, 2, N'Expense', 75.25, N'Camera equipment', CAST(N'2025-03-06T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (5, 3, N'Income', 100.25, N'Fundraising event', CAST(N'2025-03-10T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (6, 3, N'Expense', 30, N'Venue rental', CAST(N'2025-03-11T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (7, 4, N'Income', 300, N'Membership fees', CAST(N'2025-03-15T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (8, 4, N'Expense', 100, N'Instrument purchase', CAST(N'2025-03-16T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (9, 5, N'Income', 250.75, N'Sponsorship', CAST(N'2025-03-20T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (10, 5, N'Expense', 50, N'Software licenses', CAST(N'2025-03-21T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (11, 6, N'Income', 180, N'Membership fees', CAST(N'2025-03-12T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (12, 6, N'Expense', 90, N'Robot parts', CAST(N'2025-03-13T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (13, 7, N'Income', 220, N'Membership fees', CAST(N'2025-03-18T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (14, 7, N'Expense', 110, N'Costumes', CAST(N'2025-03-19T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (15, 8, N'Income', 150, N'Membership fees', CAST(N'2025-03-22T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (16, 8, N'Expense', 75, N'Lab equipment', CAST(N'2025-03-23T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (17, 9, N'Income', 110, N'Membership fees', CAST(N'2025-03-25T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (19, 10, N'Income', 130, N'Membership fees', CAST(N'2025-03-28T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (20, 10, N'Expense', 65, N'Gaming equipment', CAST(N'2025-03-29T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (21, 11, N'Income', 140, N'Membership fees', CAST(N'2025-03-30T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (22, 11, N'Expense', 70, N'Math books', CAST(N'2025-03-31T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (23, 12, N'Income', 160, N'Membership fees', CAST(N'2025-04-01T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (24, 12, N'Expense', 80, N'Books', CAST(N'2025-04-02T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (25, 13, N'Income', 170, N'Membership fees', CAST(N'2025-04-03T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (26, 13, N'Expense', 85, N'Cooking tools', CAST(N'2025-04-04T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (27, 14, N'Income', 190, N'Membership fees', CAST(N'2025-04-05T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (28, 14, N'Expense', 95, N'Telescope', CAST(N'2025-04-06T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (29, 15, N'Income', 210, N'Membership fees', CAST(N'2025-04-07T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (30, 15, N'Expense', 105, N'History books', CAST(N'2025-04-08T00:00:00.000' AS DateTime))
INSERT [dbo].[ClubFinances] ([FinanceID], [ClubID], [TransactionType], [price], [Description], [TransactionDate]) VALUES (31, 1, N'Income', 100, N'Mua d?', CAST(N'2025-03-24T14:42:09.070' AS DateTime))
SET IDENTITY_INSERT [dbo].[ClubFinances] OFF
GO
SET IDENTITY_INSERT [dbo].[ClubMembers] ON 

INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (1, 1, 3, CAST(N'2025-03-01T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (2, 1, 6, CAST(N'2025-03-10T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (3, 2, 7, CAST(N'2025-03-15T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (4, 3, 8, CAST(N'2025-03-20T00:00:00.000' AS DateTime), N'Inactive')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (5, 4, 9, CAST(N'2025-03-25T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (6, 5, 10, CAST(N'2025-03-30T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (7, 6, 11, CAST(N'2025-03-10T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (8, 7, 12, CAST(N'2025-03-15T00:00:00.000' AS DateTime), N'Inactive')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (9, 8, 13, CAST(N'2025-03-20T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (10, 9, 14, CAST(N'2025-03-25T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (11, 10, 15, CAST(N'2025-03-30T00:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (12, 1, 5, CAST(N'2025-03-24T09:46:08.763' AS DateTime), N'Active')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (13, 2, 5, CAST(N'2025-03-24T14:44:42.153' AS DateTime), N'Pending')
INSERT [dbo].[ClubMembers] ([MembershipID], [ClubID], [UserID], [JoinDate], [Member_Status]) VALUES (14, 3, 5, CAST(N'2025-03-24T14:47:02.403' AS DateTime), N'Active')
SET IDENTITY_INSERT [dbo].[ClubMembers] OFF
GO
SET IDENTITY_INSERT [dbo].[Clubs] ON 

INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (1, N'Chess Club', N'A club for chess enthusiasts ABCXYZ', CAST(N'2025-01-15' AS Date), 2, N'Active', CAST(150.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (2, N'Photography Club abc', N'Exploring the art of photography.', CAST(N'2015-03-10' AS Date), 5, N'Active', CAST(75.25 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (3, N'Debate Club', N'Sharpen your debating skills.', CAST(N'2025-03-20' AS Date), 2, N'Active', CAST(70.25 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (4, N'Music Club', N'For music lovers and performers.', CAST(N'2025-02-05' AS Date), 5, N'Active', CAST(200.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (5, N'Coding Club ABC', N'Learn and build software projects.', CAST(N'2025-01-01' AS Date), 2, N'Active', CAST(200.75 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (6, N'Robotics Club', N'Exploring robotics and automation.', CAST(N'2025-03-12' AS Date), 5, N'Active', CAST(90.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (7, N'Drama Club', N'Performing arts and theater.', CAST(N'2025-09-15' AS Date), 2, N'Active', CAST(110.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (8, N'Science Club', N'Scientific discoveries and experiments.', CAST(N'2025-03-30' AS Date), 5, N'Active', CAST(75.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (9, N'Sports Club', N'Various sports activities.', CAST(N'2025-02-05' AS Date), 2, N'Active', CAST(60.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (10, N'Gaming Club', N'Competitive and casual gaming.', CAST(N'2025-02-20' AS Date), 5, N'Active', CAST(65.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (11, N'Math Club', N'Solving mathematical challenges.', CAST(N'2025-01-10' AS Date), 2, N'Active', CAST(70.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (12, N'Literature Club', N'Book discussions and writing.', CAST(N'2025-03-22' AS Date), 5, N'Active', CAST(80.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (13, N'Culinary Club', N'Cooking and food appreciation.', CAST(N'2025-01-14' AS Date), 2, N'Active', CAST(85.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (14, N'Astronomy Club', N'Exploring the universe.', CAST(N'2025-01-18' AS Date), 5, N'Active', CAST(95.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (15, N'History Club', N'Learning about historical events.', CAST(N'2025-03-25' AS Date), 2, N'Active', CAST(105.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (16, N'FPTU', N'FPTU Very good', CAST(N'2025-03-23' AS Date), 5, N'Active', CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[Clubs] ([ClubID], [ClubName], [Description], [EstablishedDate], [PresidentID], [ClubStatus], [Total_cost]) VALUES (17, N'Music Club 123', N'For music lovers and performers.', CAST(N'2025-03-24' AS Date), 2, N'Active', CAST(0.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[Clubs] OFF
GO
SET IDENTITY_INSERT [dbo].[EventFeedback] ON 

INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (1, 1, 3, 5, N'Amazing tournament!', CAST(N'2025-04-01T14:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (2, 2, 6, 4, N'Great photo walk, learned a lot.', CAST(N'2025-04-05T12:30:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (3, 3, 7, 3, N'Debate was intense but fun.', CAST(N'2025-04-10T20:15:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (4, 4, 8, 5, N'Music Jam was fantastic!', CAST(N'2025-04-15T22:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (5, 5, 9, 4, N'Hackathon was challenging but rewarding.', CAST(N'2025-04-20T11:45:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (6, 1, 10, 5, N'Chess tournament was well-organized.', CAST(N'2025-04-25T13:20:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (7, 2, 11, 4, N'Enjoyed the photo walk.', CAST(N'2025-04-30T10:10:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (8, 3, 12, 3, N'Debate night was interesting.', CAST(N'2025-05-01T19:30:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (9, 4, 13, 5, N'Music Jam was a blast!', CAST(N'2025-05-05T21:00:00.000' AS DateTime), N'Active')
INSERT [dbo].[EventFeedback] ([FeedbackID], [EventID], [UserID], [Rating], [Comments], [CreatedAt], [Status]) VALUES (10, 5, 14, 4, N'Hackathon was a great experience.', CAST(N'2025-05-10T09:15:00.000' AS DateTime), N'Active')
SET IDENTITY_INSERT [dbo].[EventFeedback] OFF
GO
SET IDENTITY_INSERT [dbo].[EventParticipants] ON 

INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (1, 1, 3, NULL, CAST(N'2025-03-20T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (2, 2, 6, NULL, CAST(N'2025-03-25T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (3, 3, 7, NULL, CAST(N'2025-03-30T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (4, 4, 8, NULL, CAST(N'2025-04-01T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (5, 5, 9, NULL, CAST(N'2025-04-05T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (6, 1, 10, NULL, CAST(N'2025-04-10T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (7, 2, 11, NULL, CAST(N'2025-04-15T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (8, 3, 12, NULL, CAST(N'2025-04-20T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (9, 4, 13, NULL, CAST(N'2025-04-25T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (10, 5, 14, NULL, CAST(N'2025-04-30T00:00:00.000' AS DateTime), N'attended')
INSERT [dbo].[EventParticipants] ([EventParticipantID], [EventID], [UserID], [Reason], [RegistrationDate], [Status]) VALUES (11, 1, 5, NULL, CAST(N'2025-03-24T14:43:51.117' AS DateTime), N'Registered')
SET IDENTITY_INSERT [dbo].[EventParticipants] OFF
GO
SET IDENTITY_INSERT [dbo].[Events] ON 

INSERT [dbo].[Events] ([EventID], [EventName], [Description], [EventDate], [Location], [OrganizerID], [ClubID], [MaxParticipants], [Event_Status]) VALUES (1, N'Chess Tournament', N'Annual chess competition.', CAST(N'2025-04-01T10:00:00.000' AS DateTime), N'Room A-101', 2, 1, 20, N'Coming soon')
INSERT [dbo].[Events] ([EventID], [EventName], [Description], [EventDate], [Location], [OrganizerID], [ClubID], [MaxParticipants], [Event_Status]) VALUES (2, N'Photo Walk', N'Outdoor photography event.', CAST(N'2025-04-05T09:00:00.000' AS DateTime), N'City Park', 5, 2, 15, N'Coming soon')
INSERT [dbo].[Events] ([EventID], [EventName], [Description], [EventDate], [Location], [OrganizerID], [ClubID], [MaxParticipants], [Event_Status]) VALUES (3, N'Debate Night', N'Debate on current issues.', CAST(N'2025-04-10T18:00:00.000' AS DateTime), N'Auditorium', 2, 3, 30, N'Coming soon')
INSERT [dbo].[Events] ([EventID], [EventName], [Description], [EventDate], [Location], [OrganizerID], [ClubID], [MaxParticipants], [Event_Status]) VALUES (4, N'Music Jam', N'Live music performances.', CAST(N'2025-04-15T19:00:00.000' AS DateTime), N'Club Room', 5, 4, 25, N'Coming soon')
INSERT [dbo].[Events] ([EventID], [EventName], [Description], [EventDate], [Location], [OrganizerID], [ClubID], [MaxParticipants], [Event_Status]) VALUES (5, N'Code Hackathon', N'24-hour coding challenge.', CAST(N'2025-04-20T08:00:00.000' AS DateTime), N'Lab B-202', 2, 5, 50, N'Coming soon')
SET IDENTITY_INSERT [dbo].[Events] OFF
GO
SET IDENTITY_INSERT [dbo].[Reports] ON 

INSERT [dbo].[Reports] ([ReportID], [ClubID], [Semester], [MemberChanges], [EventSummary], [ParticipationStats], [CreatedDate]) VALUES (1, 1, N'Spring 2025', N'2 new members', N'Chess Tournament successful', N'80% attendance', CAST(N'2025-03-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Reports] ([ReportID], [ClubID], [Semester], [MemberChanges], [EventSummary], [ParticipationStats], [CreatedDate]) VALUES (2, 2, N'Spring 2025', N'1 member left', N'Photo Walk planned', N'90% participation', CAST(N'2025-03-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Reports] ([ReportID], [ClubID], [Semester], [MemberChanges], [EventSummary], [ParticipationStats], [CreatedDate]) VALUES (3, 3, N'Spring 2025', N'No changes', N'Debate Night upcoming', N'70% expected', CAST(N'2025-03-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Reports] ([ReportID], [ClubID], [Semester], [MemberChanges], [EventSummary], [ParticipationStats], [CreatedDate]) VALUES (4, 4, N'Spring 2025', N'1 member inactive', N'Music Jam cancelled', N'50% attendance', CAST(N'2025-03-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Reports] ([ReportID], [ClubID], [Semester], [MemberChanges], [EventSummary], [ParticipationStats], [CreatedDate]) VALUES (5, 5, N'Spring 2025', N'3 new members', N'Hackathon scheduled', N'85% participation', CAST(N'2025-03-20T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Reports] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Club President')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'Member')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (1, N'John Doe', N'a@gmail.com', N'1', 1, CAST(N'1990-05-15' AS Date), N'1234567890', N'123 Main St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (2, N'Jane Smith', N'b@gmail.com', N'1', 2, CAST(N'1992-08-22' AS Date), N'0987654321', N'456 Oak St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (3, N'Mike Johnson', N'c@gmail.com', N'1', 3, CAST(N'1995-03-10' AS Date), N'5551234567', N'789 Pine St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (4, N'Emily Brown', N'd@gmail.com', N'1', 3, CAST(N'1998-12-01' AS Date), N'4449876543', N'101 Elm St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (5, N'David Lee', N'dungbd07@gmail.com', N'1', 2, CAST(N'1988-07-19' AS Date), N'3334567890', N'202 Birch St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (6, N'Lisa Green', N'lisa@gmail.com', N'1', 3, CAST(N'1993-04-18' AS Date), N'1112223333', N'222 Cedar St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (7, N'James White', N'james@gmail.com', N'1', 3, CAST(N'1991-07-23' AS Date), N'9998887777', N'333 Maple St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (8, N'Emma Black', N'emma@gmail.com', N'1', 3, CAST(N'1997-02-12' AS Date), N'6665554444', N'444 Willow St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (9, N'Oliver Gray', N'oliver@gmail.com', N'1', 3, CAST(N'1996-06-25' AS Date), N'1231231234', N'555 Aspen St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Inactive')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (10, N'Sophia Blue', N'sophia@gmail.com', N'1', 3, CAST(N'1994-09-30' AS Date), N'3213214321', N'666 Redwood St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (11, N'Jack Brown', N'jack@gmail.com', N'1', 3, CAST(N'1998-03-15' AS Date), N'7776665555', N'777 Oakwood St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (12, N'Mia White', N'mia@gmail.com', N'1', 3, CAST(N'1995-11-10' AS Date), N'8889991111', N'888 Birchwood St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (13, N'Henry Black', N'henry@gmail.com', N'1', 3, CAST(N'1990-08-07' AS Date), N'2223334444', N'999 Sycamore St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Inactive')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (14, N'Charlotte Pink', N'charlotte@gmail.com', N'1', 3, CAST(N'1992-05-20' AS Date), N'5556667777', N'1001 Cedarwood St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [Password], [RoleID], [DateOfBirth], [PhoneNumber], [Address], [DateJoined], [Status]) VALUES (15, N'Mason Yellow', N'mason@gmail.com', N'1', 3, CAST(N'1999-12-31' AS Date), N'3332221111', N'1002 Pinewood St', CAST(N'2025-03-23T22:25:26.093' AS DateTime), N'Active')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__8A2B6160E4B61495]    Script Date: 24/03/2025 11:56:27 CH ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D1053484B5AF58]    Script Date: 24/03/2025 11:56:27 CH ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClubFinances] ADD  DEFAULT (getdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[ClubMembers] ADD  DEFAULT (getdate()) FOR [JoinDate]
GO
ALTER TABLE [dbo].[EventFeedback] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[EventParticipants] ADD  DEFAULT (getdate()) FOR [RegistrationDate]
GO
ALTER TABLE [dbo].[Reports] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [DateJoined]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ClubFinances]  WITH CHECK ADD FOREIGN KEY([ClubID])
REFERENCES [dbo].[Clubs] ([ClubID])
GO
ALTER TABLE [dbo].[ClubMembers]  WITH CHECK ADD FOREIGN KEY([ClubID])
REFERENCES [dbo].[Clubs] ([ClubID])
GO
ALTER TABLE [dbo].[ClubMembers]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Clubs]  WITH CHECK ADD FOREIGN KEY([PresidentID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[EventFeedback]  WITH CHECK ADD FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[EventFeedback]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[EventParticipants]  WITH CHECK ADD FOREIGN KEY([EventID])
REFERENCES [dbo].[Events] ([EventID])
GO
ALTER TABLE [dbo].[EventParticipants]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD FOREIGN KEY([ClubID])
REFERENCES [dbo].[Clubs] ([ClubID])
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD FOREIGN KEY([OrganizerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD FOREIGN KEY([ClubID])
REFERENCES [dbo].[Clubs] ([ClubID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[EventFeedback]  WITH CHECK ADD CHECK  (([Rating]>=(1) AND [Rating]<=(5)))
GO
