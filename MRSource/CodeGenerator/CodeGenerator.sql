SELECT 
*
FROM 
	[INFORMATION_SCHEMA].Columns cols
ORDER BY 
	tble_catalog, table_Schema  
	�, table_name, ordinal_position, column_name