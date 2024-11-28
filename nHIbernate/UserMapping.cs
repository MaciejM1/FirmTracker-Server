using FluentNHibernate.Mapping;
using FirmTracker_Server.Entities;

public class UserMapping : ClassMap<User>
{
    public UserMapping()
    {
        Table("Users"); // The name of your table in the database

        Id(x => x.UserId); // Mapping the Id property
        Map(x => x.Email); // Mapping other properties
        Map(x => x.PassHash);
        Map(x => x.Role);
        // Add other mappings as needed
    }
}
