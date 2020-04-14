Imports System.Collections.Generic
Public Class DBMacros
    Private Sub New()

    End Sub

#Region "Tables"

    Public Shared Function AddDBTableID(tableName As String, sch As IDBSchema, rev As DBRevision) As IDBTable
        Dim ret As IDBTable = Nothing

        ret = sch.AddTable(tableName, New DBTableDescriptor() With {.CreatorFieldName = "ID", .CreatorFieldDescriptor = DBFieldIDDescriptor(False)},
                    New DBRevision(rev))

        ret.AddConstraint(New DBPrimaryKeyConstraintDescriptor("ID"),
                          New DBRevision(rev))

        'ret.AddClaims(rev.Created)
        Return ret
    End Function

    Public Shared Function AddDBTableIDNaziv(tableName As String, sch As IDBSchema, rev As DBRevision) As IDBTable
        Dim ret As IDBTable = Nothing

        ret = AddDBTableID(tableName, sch, rev)

        ret.AddField("Naziv", DBFieldNazivDescriptor(False),
                     New DBRevision(rev))

        Return ret
    End Function

#End Region

#Region "Fields"
#Region "Descriptors"
    Public Shared Function DBFieldActiveDescriptor() As DBFieldDescriptor
        Return New DBFieldDescriptor() With {.FieldType = New DBFieldTypeBoolean, .Nullable = True}
    End Function

    Public Shared Function DBFieldDateTimeDescriptor(nullable As Boolean) As DBFieldDescriptor
        Return New DBFieldDescriptor() With {.FieldType = New DBFieldTypeDatetime, .Nullable = nullable}
    End Function

    Public Shared Function DBFieldIDDescriptor(nullable As Boolean) As DBFieldDescriptor
        Return New DBFieldDescriptor() With {.FieldType = New DBFieldTypeGuid, .Nullable = nullable}
    End Function

    Public Shared Function DBFieldNazivDescriptor(nullable As Boolean) As DBFieldDescriptor
        Return New DBFieldDescriptor() With {.FieldType = New DBFieldTypeNvarchar, .Nullable = nullable, .Size = 512}
    End Function

    Public Shared Function DBFieldBitDescriptor(nullable As Boolean) As DBFieldDescriptor
        Return New DBFieldDescriptor() With {.FieldType = New DBFieldTypeBoolean, .Nullable = nullable}
    End Function

    Public Shared Function DBFieldLongTextDescriptor(nullable As Boolean) As DBFieldDescriptor
        Return New DBFieldDescriptor() With {.FieldType = New DBFieldTypeNvarchar, .Nullable = nullable, .Size = -1}
    End Function

    Public Shared Function DBFieldIntegerDescriptor(nullable As Boolean) As DBFieldDescriptor
        Return New DBFieldDescriptor() With {.FieldType = New DBFieldTypeInteger, .Nullable = nullable, .DefaultValue = "0"}
    End Function
#End Region

    Public Shared Function AddForeignKeyFieldID(fieldName As String, nullable As Boolean, table As IDBTable, pkFullTableName As String, rev As DBRevision) As IDBField
        Dim ret As IDBField
        ret = table.AddField(fieldName, DBFieldIDDescriptor(nullable),
                       New DBRevision(rev))

        Dim ls As New List(Of String)(New String() {fieldName})

        table.AddConstraint(New DBForeignKeyConstraintDescriptor(New List(Of String)(New String() {fieldName}), pkFullTableName, New List(Of String)(New String() {"ID"})),
                New DBRevision(rev))

        Return ret
    End Function
#End Region


End Class
