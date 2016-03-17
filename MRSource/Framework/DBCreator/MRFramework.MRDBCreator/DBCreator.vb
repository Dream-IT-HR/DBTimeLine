﻿Imports MRFramework.MRPersisting.Factory

Public Class DBCreator
    Implements IDBChained

    Public ReadOnly Property DBModules As New List(Of DBModule)

    Public ReadOnly Property SourceDBSqlRevisions As New List(Of DBSqlRevision)
    Public ReadOnly Property ExecutedDBSqlRevisions As New List(Of DBSqlRevision)
    Public ReadOnly Property DBSchemas As New Dictionary(Of String, DBSchema)
    Public ReadOnly Property DBTables As New Dictionary(Of String, DBTable)
    Public ReadOnly Property DBFields As New Dictionary(Of String, DBField)

    Public Property Parent As IDBChained Implements IDBChained.Parent

    Public Function AddModule(dBModule As DBModule) As DBModule
        DBModules.Add(dBModule)
        dBModule.Parent = Me

        Return dBModule
    End Function

    Public Function GetModules() As List(Of DBModule)
        Dim ret As New List(Of DBModule)

        ret.AddRange(DBModules)

        Return ret
    End Function

    Public Sub LoadExecutedDBSqlRevisionsFromDB(cnn As Common.DbConnection, Optional trn As Common.DbTransaction = Nothing)
        ExecutedDBSqlRevisions.Clear()

        Using per As New DBSqlRevision.DBSqlRevisionPersister With {.CNN = cnn, .PagingEnabled = False}
            With per.OrderItems
                .Add(New MRCore.MROrderItem("Created", MRCore.Enums.Enums.eOrderDirection.Ascending))
                .Add(New MRCore.MROrderItem("Granulation", MRCore.Enums.Enums.eOrderDirection.Ascending))
                .Add(New MRCore.MROrderItem("DBObjectType", MRCore.Enums.Enums.eOrderDirection.Ascending))
                .Add(New MRCore.MROrderItem("DBRevisionType", MRCore.Enums.Enums.eOrderDirection.Ascending))
                .Add(New MRCore.MROrderItem("DBObjectFullName", MRCore.Enums.Enums.eOrderDirection.Ascending))
            End With

            ' TODO - per.GetData generira bezvezan query, treba smisliti nesto drugo, stignem
            ' ovako:
            ' SELECT Case ID, Created, Granulation, DBObjectType, DBRevisionType, DBObjectFullName, DBObjectName, SchemaName FROM Common.Revision
            ' ORDER BY  Created ASC, Granulation ASC, DBObjectType ASC, DBRevisionType ASC, DBObjectFullName ASC OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY

            Dim dicExecutedRevisions As Dictionary(Of Object, MRPersisting.Core.IMRDLO) = per.GetData(trn)
            For Each kv As KeyValuePair(Of Object, MRPersisting.Core.IMRDLO) In dicExecutedRevisions
                Dim sqlRevision As New DBSqlRevision(kv.Value)
                ExecutedDBSqlRevisions.Add(sqlRevision)
            Next
        End Using
    End Sub

    Public Sub ExecuteDBSqlRevisions(cnn As Common.DbConnection, Optional trn As Common.DbTransaction = Nothing)
        Dim nonExecutedRevisions = SourceDBSqlRevisions.Except(ExecutedDBSqlRevisions, New DBSqlRevision.DBSqlRevisionEqualityComparer).ToList()
        For i As Integer = 0 To nonExecutedRevisions.Count - 1
            Dim rev As DBSqlRevision = nonExecutedRevisions(i)
            Dim sql As String = rev.Parent.Parent.GetSqlCreate
        Next
    End Sub

#Region "System objects"

    Private Sub CreateRevisionTable()
        Using cnn As IDbConnection = MRC.GetConnection
            Using cmd As IDbCommand = MRC.GetCommand
                Try
                    cmd.CommandText =
<string>
IF OBJECT_ID('Common.Revision') IS NULL
BEGIN
	CREATE TABLE [Common].[Revision]
	(
		[ID] [uniqueidentifier] NOT NULL,
		[Created] [date] NOT NULL,
		[Granulation] [int] NOT NULL,
		[DBObjectFullName] [varchar](250) NOT NULL,
        [DBObjectName] [varchar](50) NOT NULL,
		[DBObjectType] [varchar](50) NOT NULL,
		[DBRevisionType] [varchar](50) NOT NULL,
        [SchemaName] [varchar](50),
        [Description] [nvarchar](max) NULL,
		CONSTRAINT [PK_Revision] PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)
	)
END
IF EXISTS(SELECT TOP 1 1 FROM sys.indexes WHERE name='IX_CommonRevision_Sort' AND object_id = OBJECT_ID('Common.Revision'))
BEGIN
	DROP INDEX IX_CommonRevision_Sort ON Common.Revision 
END
CREATE INDEX IX_CommonRevision_Sort ON Common.Revision (Created ASC, Granulation ASC, DBObjectType ASC, DBRevisionType ASC, DBObjectFullName ASC, ID ASC) include (DBObjectName, schemaname)

</string>.Value
                    cmd.Connection = cnn
                    If cnn.State <> ConnectionState.Open Then
                        cnn.Open()
                    End If

                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    If Debugger.IsAttached Then
                        Debugger.Break()
                    End If
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Public Sub CreateSystemObjects()
        CreateRevisionTable()
    End Sub

#End Region

End Class
