﻿Public Interface IDBModule
    Inherits IDBChained
    Inherits IDBParent

    ReadOnly Property ModuleKey As String
    Property DefaultSchemaName As String

    Sub LoadRevisions()
    Function AddSchema(schemaName As String, descriptor As IDBSchemaDescriptor, Optional createRevision As IDBRevision = Nothing) As IDBSchema

End Interface
