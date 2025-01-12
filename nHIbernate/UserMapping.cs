using FluentNHibernate.Mapping;
using FirmTracker_Server.Entities;

public class UserMapping : ClassMap<User>
{
    public UserMapping()
    {
        Table("Users"); 

        Id(x => x.UserId); 
        Map(x => x.Email); 
        Map(x => x.PassHash);
        Map(x => x.Role);
        
    }
}
