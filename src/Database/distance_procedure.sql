
-- Procedure to get 3 closest branches from an input location lat and lng
CREATE OR ALTER PROCEDURE pGetThreeClosestBranches(@lat FLOAT, @lng FLOAT)
AS
BEGIN;
SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;
UPDATE dbo.Branches
SET Distance = SQRT(POWER(@lat - Latitude, 2) + POWER(@lng - Longitude, 2)) * 100 -- kms

SELECT TOP 3 *
FROM dbo.Branches
ORDER BY Distance ASC

COMMIT TRANSACTION;
END;