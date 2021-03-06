/****** Object:  Database [PaystraxDatabase]    Script Date: 17-Aug-21 10:15:37 AM ******/
CREATE DATABASE [PaystraxDatabase]
GO

USE [PaystraxDatabase]
GO

/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 17-Aug-21 10:15:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NULL,
	[Email] [varchar](150) NOT NULL,
	[WalletAddress] [varchar](150) NOT NULL,
	[Password] [varchar](150) NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentRequests]    Script Date: 17-Aug-21 10:15:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestGuid] [uniqueidentifier] NOT NULL,
	[Reason] [varchar](500) NULL,
	[FromAddress] [varchar](50) NULL,
	[ToAddress] [varchar](50) NULL,
	[Amount] [decimal](18, 8) NULL,
	[AmountInSatoshi] [bigint] NULL,
	[PaymentStatus] [int] NULL,
	[Expiry] [varchar](50) NULL,
	[CreationTimeUtc] [datetime] NULL,
	[CancellationTimeUtc] [datetime] NULL,
	[PaymentTimeUtc] [datetime] NULL,
 CONSTRAINT [PK_PaymentRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ChangeRequestStatus]    Script Date: 17-Aug-21 10:15:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ChangeRequestStatus]
    @Id INT,
    @PaymentStatus INT,
    @CancellationTimeUtc Datetime = NULL,
    @PaymentTimeUtc Datetime = NULL
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    DECLARE @Check INT;
    -- Insert statements for procedure here
    IF (@CancellationTimeUtc is NOT NULL)
    BEGIN
        Update PaymentRequests
        set PaymentStatus = @PaymentStatus,
            CancellationTimeUtc = @CancellationTimeUtc
        where Id = @Id
    END
    ELSE
    BEGIN
        Update PaymentRequests
        set PaymentStatus = @PaymentStatus,
            PaymentTimeUtc = @PaymentTimeUtc
        where Id = @Id
    END

    if (@@ROWCOUNT > 0)
        SET @check = 1

    SELECT @check;
END
GO
/****** Object:  StoredProcedure [dbo].[CreateRequest]    Script Date: 17-Aug-21 10:15:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- [Createrequest] '1DF0E1EF-2FEA-41A0-8E49-64FDC42EB3C9', 'Payment for work', 'PQEf8KCNpLgzGA17m61LSKxwF3bwG1XYQ7', 'PSNtsXiY8BoWduHFqPe71CsfZiCaLhVNJE', '0.3', '300', '1', '2021-08-16 17:28:05.893'
CREATE PROCEDURE [dbo].[CreateRequest]
    @RequestGuid UNIQUEIDENTIFIER,
    @Reason VARCHAR(350),
    @FromAddress VARCHAR(150),
    @ToAddress VARCHAR(150),
    @Amount DECIMAL(18, 8),
    @AmountInSatoshi BIGINT,
    @PaymentStatus INT,
    @Expiry DATETIME
AS
BEGIN

    SET nocount ON;

    INSERT INTO paymentrequests
    (
        RequestGuid,
        Reason,
        FromAddress,
        ToAddress,
        Amount,
        AmountInSatoshi,
        PaymentStatus,
        Expiry,
        CreationTimeUtc
    )
    VALUES
    (@RequestGuid, @Reason, @FromAddress, @ToAddress, @Amount, @AmountInSatoshi, @PaymentStatus, @Expiry, Getutcdate())

	SELECT SCOPE_IDENTITY()

END
GO
/****** Object:  StoredProcedure [dbo].[GetAllRequests]    Script Date: 17-Aug-21 10:15:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllRequests]
    --@UserId int
    @WalletAddress VARCHAR(50)
AS
BEGIN

    SET NOCOUNT ON;
    Select Id,
           RequestGuid,
           Reason,
           FromAddress,
           ToAddress,
           Amount,
           AmountInSatoshi,
           PaymentStatus,
           CreationTimeUtc
    from PaymentRequests
    where FromAddress = @WalletAddress
          OR ToAddress = @WalletAddress

END
GO
/****** Object:  StoredProcedure [dbo].[GetRequestByGuid]    Script Date: 17-Aug-21 10:15:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetRequestByGuid] @RequestGuid Uniqueidentifier
AS
BEGIN

    SET NOCOUNT ON;

    Select Id,
           RequestGuid,
           Reason,
           FromAddress,
           ToAddress,
           Amount,
           AmountInSatoshi,
           PaymentStatus,
		   Expiry,
           CreationTimeUtc,
           CancellationTimeUtc,
           PaymentTimeUtc
    from PaymentRequests
    where RequestGuid = @RequestGuid
END
GO
/****** Object:  StoredProcedure [dbo].[LoginUser]    Script Date: 17-Aug-21 10:15:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[LoginUser]    Script Date: 17-Aug-21 1:50:19 AM ******/

CREATE PROCEDURE [dbo].[LoginUser]
    @Email VARCHAR(150),
    @Password VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    Select Id,
           FirstName,
           LastName,
           Email,
           WalletAddress
    from AspNetUsers
    where Email = @Email
          AND Password = @Password
END
GO
