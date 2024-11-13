using FirmTracker_Server.Entities;
using NHibernate;
using System;

namespace FirmTracker_Server.nHibernate
{
    public class WorkdayRepository
    {
        public void StartWorkday(int userId)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    // Fetch the user entity by its ID
                    var user = session.Get<User>(userId); // Assuming User is a mapped entity
                    if (user == null)
                    {
                        throw new Exception("User not found");
                    }

                    // Create a new Workday and assign the User reference
                    var workday = new Workday
                    {
                        StartTime = DateTime.Now,
                        User = user // Set the User reference here
                    };

                    session.Save(workday);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("An error occurred while starting the workday", ex);
                }
            }
        }
    }
}
