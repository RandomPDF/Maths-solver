CREATE PROCEDURE [dbo].[spBasicDifferentials_FilterByOrigionalFunction]
	@OrigionalFunction nvarchar(20)
AS
BEGIN
	SELECT *
	FROM dbo.BasicDifferentials
	WHERE OrigionalFunction = @OrigionalFunction;
END
