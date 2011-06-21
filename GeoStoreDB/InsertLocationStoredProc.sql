USE GeoStoreDB
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE [dbo].[InsertLocation]
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertLocation] 
	-- Add the parameters for the stored procedure here
	@accuracy float,
	@altitude float,
	@bearing float,
	@latitude float,
	@longitude float,
	@provider varchar(255),
	@speed float,
	@extras varchar(255),
	@measurementTime datetime2(7),
	@entryID bigint,
	@processingMethod varchar(50),
	@deviceType varchar(50),
	@deviceId varchar(50),
	@sensorType varchar(50),
	@sensorModel varchar(50),
	@IntersensorAgreement bit,
	@solutionConfidence float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    Insert location
    (accuracy, altitude, bearing, latitude, longitude, geoPosition, provider, speed, extras, measurementTime, entryID, 
     processingMethod, deviceType, deviceId, sensorType, sensorModel, IntersensorAgreement, solutionConfidence)
    Values(@accuracy,@altitude,@bearing,@latitude, @longitude, Geography::Point(@latitude, @longitude, 4326),
    @provider,@speed,@extras,@measurementTime,@entryID,@processingMethod,@deviceType,
    @deviceId,@sensorType,@sensorModel,@IntersensorAgreement,@solutionConfidence); 
END
GO