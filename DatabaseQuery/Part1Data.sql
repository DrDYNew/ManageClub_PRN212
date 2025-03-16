USE ManageClub
GO

INSERT INTO Roles (RoleName) VALUES 
('Admin'),
('Member'),
('Club President');
INSERT INTO Users (FullName, Email, Password, RoleID, DateOfBirth, PhoneNumber, Address, AvatarURL, Status) VALUES
-- 3 Admins
('Nguyễn Văn A', 'admin1@example.com', 'Admin@123', 1, '1990-05-12', '0987654321', 'Hà Nội, Việt Nam', 'https://example.com/avatar1.jpg', 'Active'),
('Trần Hoàng B', 'admin2@example.com', 'Admin@456', 1, '1985-09-22', '0911123456', 'Hải Phòng, Việt Nam', 'https://example.com/avatar2.jpg', 'Active'),
('Lê Minh C', 'admin3@example.com', 'Admin@789', 1, '1992-07-18', '0909988776', 'Đà Nẵng, Việt Nam', 'https://example.com/avatar3.jpg', 'Active'),

-- 4 Members
('Phạm Thị D', 'member1@example.com', 'Member@123', 2, '1995-11-05', '0912456789', 'Hồ Chí Minh, Việt Nam', 'https://example.com/avatar4.jpg', 'Active'),
('Hoàng Đức E', 'member2@example.com', 'Member@456', 2, '1998-03-15', '0903344556', 'Cần Thơ, Việt Nam', 'https://example.com/avatar5.jpg', 'Active'),
('Ngô Thị F', 'member3@example.com', 'Member@789', 2, '1999-06-28', '0988776655', 'Huế, Việt Nam', 'https://example.com/avatar6.jpg', 'Active'),
('Đinh Văn G', 'member4@example.com', 'Member@321', 2, '1997-01-30', '0933221100', 'Bình Dương, Việt Nam', 'https://example.com/avatar7.jpg', 'Active'),

-- 3 Club Presidents
('Trương Bảo H', 'clubpresident1@example.com', 'President@123', 3, '1988-12-02', '0977554433', 'Quảng Ninh, Việt Nam', 'https://example.com/avatar8.jpg', 'Active'),
('Võ Hải I', 'clubpresident2@example.com', 'President@456', 3, '1994-08-14', '0966443322', 'Nha Trang, Việt Nam', 'https://example.com/avatar9.jpg', 'Active'),
('Lý Nhật K', 'clubpresident3@example.com', 'President@789', 3, '1993-05-20', '0955332211', 'Hà Nam, Việt Nam', 'https://example.com/avatar10.jpg', 'Active');
