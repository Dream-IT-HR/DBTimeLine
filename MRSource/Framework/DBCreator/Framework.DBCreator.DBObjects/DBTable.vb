﻿Public Class DBTable
    Inherits DBObject
    Implements IDBTable

    Public Sub New()

    End Sub

    Public Sub New(descriptor As IDBTableDescriptor)
        MyClass.New

        Me.Descriptor = descriptor
        If Not String.IsNullOrWhiteSpace(descriptor.CreatorFieldName) AndAlso descriptor.CreatorFieldDescriptor IsNot Nothing Then
            AddField(descriptor.CreatorFieldName, descriptor.CreatorFieldDescriptor)
        End If
    End Sub

    Public Overrides ReadOnly Property ObjectType As eDBObjectType
        Get
            Return eDBObjectType.Table
        End Get
    End Property

    Public Function AddConstraint(constraintName As String, descriptor As IDBObjectDescriptor, Optional createRevision As IDBRevision = Nothing) As IDBObject Implements IDBTable.AddConstraint
        'TODO - ovdje promijeniti signature, staviti constraintName da je opcionalan, ovdje ga kalkulirati ako je prazan i pozvati adddbobject
        Return MyBase.AddDBObject(constraintName, descriptor, createRevision)
    End Function

    Public Function AddField(fieldName As String, descriptor As IDBFieldDescriptor, Optional createRevision As IDBRevision = Nothing) As IDBObject Implements IDBTable.AddField
        Return MyBase.AddDBObject(fieldName, descriptor, createRevision)
    End Function

End Class