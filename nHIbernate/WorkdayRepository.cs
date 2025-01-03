using FirmTracker_Server.Entities;
using FirmTracker_Server.nHibernate;
using static NHibernate.Engine.Query.CallableParser;
using FirmTracker_Server.Models;

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
                    User = user,
                    Absence = ""
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
    
    public void AddAbsence(int userId, string absenceType, DateTime startTime, DateTime endTime)
    {
        using (var session = SessionFactory.OpenSession())
        using (var transaction = session.BeginTransaction())
        {
            try
            {
                var user = session.Get<User>(userId);
                if (user == null) throw new Exception("User not found");

                // Create a new workday entry for the absence
                var workday = new Workday
                {
                    User = user,
                    StartTime = startTime,
                    EndTime = endTime,
                    Absence = absenceType  // Store the absence type as a string
                };

                session.Save(workday);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("An error occurred while adding the absence", ex);
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
                        Absence = w.Absence,
                    })
                    .ToList();

                foreach (var workday in workdays)
                {
                    if(workday.Absence!="")
                    {
                        workday.WorkedHours = TimeSpan.Zero;
                    }
                }

                return workdays;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching workdays", ex);
            }
        }
    }
    public DayDetailsDto GetDayDetails(string mail, DateTime date)
    {
        using (var session = SessionFactory.OpenSession())
        {
            try
            {
                // Fetch workdays for the specified user on the given date
                var startOfDay = date.Date;
                var endOfDay = startOfDay.AddDays(1);

                var workdays = session.Query<Workday>()
                    .Where(w => w.User.Email == mail && w.StartTime >= startOfDay && w.StartTime < endOfDay)
                    .Select(w => new Workday
                    {
                        StartTime = w.StartTime,
                        EndTime = w.EndTime ?? DateTime.Today.AddHours(17),
                        Absence = w.Absence,
                    })
                    .ToList();

                TimeSpan totalWorkedHours = TimeSpan.Zero;

                // Calculate total worked hours and adjust if there's an absence
                foreach (var workday in workdays)
                {
                    if (string.IsNullOrEmpty(workday.Absence))
                    {
                        totalWorkedHours += workday.WorkedHours;
                    }
                }

                return new DayDetailsDto
                {
                    Email = mail,
                    Date = date,
                    TotalWorkedHours = totalWorkedHours.ToString(@"hh\:mm\:ss"),
                    WorkdayDetails = workdays
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the day's details", ex);
            }
        }
    }
    public DayDetailsLoggedUserDto GetDayDetailsForLoggedUser(int userId, DateTime date)
    {
        using (var session = SessionFactory.OpenSession())
        {
            try
            {
                // Fetch workdays for the specified user on the given date
                var startOfDay = date.Date;
                var endOfDay = startOfDay.AddDays(1);

                var workdays = session.Query<Workday>()
                    .Where(w => w.User.UserId == userId && w.StartTime >= startOfDay && w.StartTime < endOfDay)
                    .Select(w => new Workday
                    {
                        StartTime = w.StartTime,
                        EndTime = w.EndTime ?? DateTime.Today.AddHours(17),
                        Absence = w.Absence,
                    })
                    .ToList();

                TimeSpan totalWorkedHours = TimeSpan.Zero;

                // Calculate total worked hours and adjust if there's an absence
                foreach (var workday in workdays)
                {
                    if (string.IsNullOrEmpty(workday.Absence))
                    {
                        totalWorkedHours += workday.WorkedHours;
                    }
                }

                return new DayDetailsLoggedUserDto
                {
                    UserId = userId,
                    Date = date,
                    TotalWorkedHours = totalWorkedHours.ToString(@"hh\:mm\:ss"),
                    WorkdayDetails = workdays
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the day's details", ex);
            }
        }
    }
    public List<Workday> GetWorkdaysByLoggedUser(string userId)
    {
        using (var session = SessionFactory.OpenSession())
        {
            try
            {
               int  parsedUserId = Int32.Parse(userId);
                var workdays = session.Query<Workday>()
                    .Where(w => w.User.UserId == parsedUserId)
                    .Select(w => new Workday
                    {
                        Id = w.Id,
                        StartTime = w.StartTime,
                        EndTime = w.EndTime ?? DateTime.Today.AddHours(17),
                        WorkedHours = (w.EndTime ?? DateTime.Today.AddHours(17)) - w.StartTime,
                        Absence = w.Absence,
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
