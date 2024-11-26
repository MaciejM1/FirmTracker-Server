using FirmTracker_Server.Entities;
using FirmTracker_Server.nHibernate;

public class WorkdayRepository
{
    public void StartWorkday(int userId)
    {
        using (var session = SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            try
            {
                // Check if there is an existing workday that hasn't been stopped yet
                var ongoingWorkday = session.Query<Workday>()
                    .Where(w => w.User.UserId == userId && w.EndTime == null)
                    .OrderByDescending(w => w.StartTime)
                    .FirstOrDefault();

                if (ongoingWorkday != null)
                {
                    // If there is an ongoing workday, throw an exception or return a specific message
                    throw new Exception("Previous workday wasn't stopped yet.");
                }

                // Fetch the user entity
                var user = session.Get<User>(userId);
                if (user == null) throw new Exception("User not found");

                // Create a new workday if there is no ongoing one
                var workday = new Workday
                {
                    StartTime = DateTime.Now,
                    User = user
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

    public bool StopWorkday(int userId)
    {
        using (var session = SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            try
            {
                var workday = session.Query<Workday>()
                    .Where(w => w.User.UserId == userId && w.EndTime == null)
                    .OrderByDescending(w => w.StartTime)
                    .FirstOrDefault();

                if (workday == null)
                {
                    return false; // No ongoing workday found
                }

                workday.EndTime =  DateTime.Now;

                session.Update(workday);
                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("An error occurred while stopping the workday", ex);
            }
        }
    }

    public List<Workday> GetWorkdaysByUser(string email)
    {
        using (var session = SessionFactory.OpenSession())
        {
            try
            {
                var workdays = session.Query<Workday>()
                    .Where(w => w.User.Email == email)
                    .Select(w => new Workday
                    {
                        Id = w.Id,
                        StartTime = w.StartTime,
                        EndTime = w.EndTime ?? DateTime.Today.AddHours(17),
                        WorkedHours = (w.EndTime ?? DateTime.Today.AddHours(17)) - w.StartTime,
                    })
                    .ToList();

                return workdays;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching workdays", ex);
            }
        }
    }
}
