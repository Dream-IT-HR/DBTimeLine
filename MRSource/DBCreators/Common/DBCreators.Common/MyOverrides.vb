﻿Option Strict On

Imports Framework.DBTimeLine

Public Class myfieldDesc
    Inherits DBFieldDescriptor

    Public Property NekiNoviKojegNemaUGetDescriptoru As String

    Public Sub New()
        MyBase.New
    End Sub
    Public Sub New(descriptor As myfieldDesc)
        MyBase.New(descriptor)

        NekiNoviKojegNemaUGetDescriptoru = descriptor.NekiNoviKojegNemaUGetDescriptoru
    End Sub

    Public Overrides Function GetDBObjectInstance(Optional parent As IDBChained = Nothing) As IDBObject
        Return New myField() With {.Parent = parent, .Descriptor = Me}
    End Function
End Class

Public Class myField
    Inherits DBField

    Public Overrides Function GetSqlCreate(sqlGenerator As IDBSqlGenerator) As String
        Dim sql As String = MyBase.GetSqlCreate(sqlGenerator)

        sql &= NekiNoviKojegNemaUGetDescriptoru

        Return sql
    End Function

    Public Property NekiNoviKojegNemaUGetDescriptoru As String


End Class