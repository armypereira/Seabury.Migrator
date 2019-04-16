declare @id int 
select @id = 1
while @id >=1 and @id <= 100000
begin
    insert into Report values(@id, 'Demo' + convert(varchar(5), @id),' ')
    select @id = @id + 1
end