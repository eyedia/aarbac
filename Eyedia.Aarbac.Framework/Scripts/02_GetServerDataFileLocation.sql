select db.name as dbname, type_desc as filetype, physical_name as location 
from sys.master_files mf 
inner join sys.databases db on db.database_id = mf.database_id
where db.name = 'master'