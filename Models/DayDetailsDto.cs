﻿/*
 * This file is part of FirmTracker - Server.
 *
 * FirmTracker - Server is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * FirmTracker - Server is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with FirmTracker - Server. If not, see <https://www.gnu.org/licenses/>.
 */
using FirmTracker_Server.nHibernate;

namespace FirmTracker_Server.Models
{
    public class DayDetailsDto
    {
        public required string Email { get; set; }
        public DateTime Date { get; set; }
        public required string TotalWorkedHours { get; set; }
        public required List<Workday> WorkdayDetails { get; set; }
    }
}