using System.Configuration;

namespace SelfPopulatingTableFromAnnotation.Sql_Adapter.BonesBuilder
{
    class SQLDataBase
    {
        
        public static string StoredProcCheck = 
            "SELECT * " +
            "FROM sysobjects " +
            "WHERE id = object_id(N'[dbo].[{0}]') " +
            "and OBJECTPROPERTY(id, N'IsProcedure') = 1";
        public static string FunctionCheck =
           "SELECT * " +
           "FROM sys.objects " +
           "WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') " +
           "AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' )";
        public static string TableCheck =
            "SELECT * " +
            "FROM sys.tables " +
            "WHERE name = '{0}'";
        //EXAMPLE USAGE: SELECT Id, Descr FROM CSVDemo WHERE Id IN(SELECT* FROM dbo.CSVToTable(@LIST))
        public static string CSVToTable =
            "CREATE FUNCTION[dbo].[CSVToTable] (@InStr VARCHAR(MAX)) " +
            "RETURNS @TempTab TABLE " +
            "(id NVARCHAR(max) not null) " +
            "AS " +
            "BEGIN " +
            ";" +//Ensure input ends with comma 
            "SET @InStr = REPLACE(@InStr + ',', ',,', ',') " +
            "DECLARE @SP INT " +
            "DECLARE @VALUE NVARCHAR(max) " +
            "WHILE PATINDEX('%,%', @INSTR ) <> 0 " +
            "BEGIN "+
            "SELECT  @SP = PATINDEX('%,%', @INSTR) "+
            "SELECT @VALUE = LEFT(@INSTR, @SP - 1) "+
            "SELECT @INSTR = STUFF(@INSTR, 1, @SP, '') "+
            "INSERT INTO @TempTab(id) VALUES(@VALUE) "+
            "END "+
            "RETURN "+
            "END";
        public static string CreateDeleteAction =
            "CREATE PROCEDURE  [dbo].[DeleteAction] " +
            "@actionName nvarchar(max), " +
            "@CSL nvarchar(max) " +
            "AS " +
            "BEGIN " +
            "SET NOCOUNT ON; " +
            "Delete dbo.{0} " +
            "from dbo.{0} " +
            "where actionName = @actionName and instanceID IN(SELECT* FROM dbo.CSVToTable(@CSL))" +
            "END ";
        public static string CreateGetActionByActionName =
            "CREATE PROCEDURE [dbo].[getActionByActionName] " +
            "@actionName nvarchar(MAX), " +
            "@CSL nvarchar(max) " +
            "AS " +
            "BEGIN " +
            "SET NOCOUNT ON; " +
            "SELECT * from {0} where actionName = @actionName and instanceID IN(SELECT* FROM dbo.CSVToTable(@CSL))" +
            "END ";
        public static string CreateGetActionInformation =
            "CREATE PROCEDURE [dbo].[getActionInformation] " +
            "@CSL nvarchar(max) " +
            "AS " +
            "BEGIN " +
            "SET NOCOUNT ON; " +
            "SELECT* from {0} where instanceID IN(SELECT* FROM dbo.CSVToTable(@CSL))" +
            "END ";
        public static string CreateInsertAction =
            "CREATE PROCEDURE [dbo].[InstertAction] " +
            "@actionName nvarchar(MAX), " +
            "@requiredParameters nvarchar(MAX), " +
            "@optionalParameters nvarchar(MAX), " +
            "@actionDescription nvarchar(MAX), " +
            "@id nvarchar(max) " +
            "AS " +
            "BEGIN " +
            "SET NOCOUNT ON; " +
            "Insert into dbo.{0} " +
            "( " +
            "actionName, " +
            "requiredParameters, " +
            "optionalParameters, " +
            "actionDescription, " +
            "instanceID " +
            ") " +
            "Values " +
            "( " +
            "@actionName, " +
            "@requiredParameters, " +
            "@optionalParameters, " +
            "@actionDescription, " +
            "@id " +
            ") " +
            "END ";
        public static string CreateUpdateAction =
            "CREATE PROCEDURE [dbo].[UpdateAction] " +
            "@actionName nvarchar(MAX), " +
            "@requiredParameters nvarchar(MAX), " +
            "@optionalParameters nvarchar(MAX), " +
            "@actionDescription nvarchar(MAX), " +
            "@CSL nvarchar(max) " +
            "AS " +
            "BEGIN " +
            "SET NOCOUNT ON; " +
            "Update dbo.{0} " +
            "set requiredParameters = @requiredParameters, optionalParameters = @optionalParameters, actionDescription = @actionDescription " +
            "Where actionName = @actionName and instanceID IN(SELECT* FROM dbo.CSVToTable(@CSL)); " +
            "END ";
        public static string CreateAction_Information =
            "CREATE TABLE [dbo].[{0}]( " +
            "[id][int] IDENTITY(1,1) NOT NULL, " +
            "[actionName] [nvarchar](max) NOT NULL, " +
            "[requiredParameters] [nvarchar](max) NULL, " +
            "[optionalParameters] " +
            "[nvarchar](max) NULL, " +
            "[actionDescription] " +
            "[nvarchar](max) NULL, " +
            "[instanceID] " +
            "[nvarchar](max) NOT NULL " +
            ") ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";

        
        public static void Build()
        {
            string ActionInformationTableName = ConfigurationManager.AppSettings.Get("Action_Information");
            AddFunction(CSVToTable, "CSVToTable");
            AddTable("Action_Information", CreateAction_Information);
            AddProcedure("getActionByActionName", CreateGetActionByActionName, ActionInformationTableName);
            AddProcedure("getActionInformation", CreateGetActionInformation, ActionInformationTableName);
            AddProcedure("InstertAction", CreateInsertAction, ActionInformationTableName);
            AddProcedure("UpdateAction", CreateUpdateAction, ActionInformationTableName);
            AddProcedure("DeleteAction", CreateDeleteAction, ActionInformationTableName);
        }

        private static void AddTable(string configurationTableNameKey, string cmdText)
        {
            Command cmd = new Command(AddParameter(TableCheck, ConfigurationManager.AppSettings.Get(configurationTableNameKey)));
            cmd.SetCommandType(System.Data.CommandType.Text);
            if (!DAO.QueryReturnsRows(cmd))
            {
                cmd.Dispose();
                cmd = new Command(AddParameter(cmdText, ConfigurationManager.AppSettings.Get(configurationTableNameKey)));
                cmd.SetCommandType(System.Data.CommandType.Text);
                DAO.ExecuteNonQuery(cmd);
            }
            cmd.Dispose();
        }
        private static void AddProcedure(string configurationProcedureNameKey, string cmdText, string tableName)
        {
            Command cmd = new Command(AddParameter(StoredProcCheck, ConfigurationManager.AppSettings.Get(configurationProcedureNameKey)));
            cmd.SetCommandType(System.Data.CommandType.Text);
            if (!DAO.QueryReturnsRows(cmd))
            {
                cmd.Dispose();
                cmd = new Command(AddParameter(cmdText, tableName));
                cmd.SetCommandType(System.Data.CommandType.Text);
                DAO.ExecuteNonQuery(cmd);
            }
            cmd.Dispose();
        }

        private static void AddFunction(string cmdText, string functionName)
        {
            Command cmd = new Command(AddParameter(FunctionCheck, functionName));
            cmd.SetCommandType(System.Data.CommandType.Text);
            if (!DAO.QueryReturnsRows(cmd))
            {
                cmd.Dispose();
                cmd = new Command(cmdText);
                cmd.SetCommandType(System.Data.CommandType.Text);
                DAO.ExecuteNonQuery(cmd);
            }
            cmd.Dispose();
        }

        public static string AddParameter(string input, object parameter)
        {
            return string.Format(input, parameter);
        }
        public static string AddParameter(string input, object[] parameters)
        {
            return string.Format(input, parameters);
        }
    }
}
