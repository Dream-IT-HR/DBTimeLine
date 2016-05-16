﻿Option Strict On
Imports System.Text

Public Class DBRevision
    Implements IDBRevision

    Public Property DBRevisionType As eDBRevisionType Implements IDBRevision.DBRevisionType
    Public Property Parent As IDBObject Implements IDBRevision.Parent
    Public Property Created As Date Implements IDBRevision.Created
    Public Property Granulation As Integer Implements IDBRevision.Granulation
    Public Property PreSqlTask As RevisionTaskDelegate Implements IDBRevision.PreSqlTask
    Public Property PostSqlTask As RevisionTaskDelegate Implements IDBRevision.PostSqlTask


    Private Sub New()

    End Sub

    Public Sub New(created As Date, granulation As Integer, ByVal dBRevisionType As eDBRevisionType, Optional preSqlTask As RevisionTaskDelegate = Nothing, Optional postSqlTask As RevisionTaskDelegate = Nothing)
        MyClass.New()
        Me.DBRevisionType = dBRevisionType
        Me.Created = created
        Me.Granulation = granulation
        Me.PreSqlTask = preSqlTask
        Me.PostSqlTask = postSqlTask
    End Sub

    Public Sub New(revision As DBRevision)
        MyClass.New(revision.Created, revision.Granulation, revision.DBRevisionType, preSqlTask:=revision.PreSqlTask, postSqlTask:=revision.PostSqlTask)
    End Sub

    Public Function GetSql() As String Implements IDBRevision.GetSql
        Dim sbSql As New StringBuilder()

        If PreSqlTask IsNot Nothing Then
            sbSql.Append(PreSqlTask.Invoke(Me, Parent.DBTimeLiner.DBType) & vbNewLine)
        End If
        If Parent IsNot Nothing Then
            Select Case DBRevisionType
                Case eDBRevisionType.Create
                    sbSql.Append(Parent.GetSqlCreate(Parent.DBTimeLiner.DBSqlGenerator))
                Case eDBRevisionType.Modify
                    sbSql.Append(Parent.GetSqlModify(Parent.DBTimeLiner.DBSqlGenerator))
                Case eDBRevisionType.Delete
                    sbSql.Append(Parent.GetSqlDelete(Parent.DBTimeLiner.DBSqlGenerator))
                Case eDBRevisionType.Task, eDBRevisionType.AlwaysExecuteTask
                    ' Do nothing, tasks do not change db structure
                Case Else
                    Throw New NotSupportedException("eDBRevisionType")
            End Select
        End If
        If PostSqlTask IsNot Nothing Then
            sbSql.Append(vbNewLine & PostSqlTask.Invoke(Me, Parent.DBTimeLiner.DBType))
        End If

        Return sbSql.ToString
    End Function

End Class
