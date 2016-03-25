﻿Public Interface IDBModule
    Inherits IDBChained
    Inherits IDBParent

    ReadOnly Property ModuleKey As String
    'ReadOnly Property DBSchemas As Dictionary(Of String, IDBSchema)
    Function LoadRevisions() As Object
    Function AddSchema(schemaName As String, descriptor As IDBSchemaDescriptor, Optional createRevision As IDBRevision = Nothing) As IDBSchema

End Interface