SET ANSI_NULLS ON  
GO  
SET QUOTED_IDENTIFIER ON  
GO  
CREATE PROCEDURE spGetAll 
	@TableName NVARCHAR(Max), @Id NVARCHAR(Max) = null, @Type NVARCHAR(Max) = 'int'
AS 
BEGIN 
  SET NOCOUNT OFF;

  DECLARE @Sql NVARCHAR(MAX);

  SET @Sql = N'SELECT * FROM ' + QUOTENAME(@TableName)

  IF @Id IS NOT NULL
	SET @Sql = @Sql + N' WHERE Id = CAST(''' + @Id +  ''' AS ' + @Type + ')'

 EXECUTE sp_executesql @Sql

END

--EXEc spGetAll @TableName = 'Roles', @Id = '1' or null