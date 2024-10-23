/*
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

using System.Text.Json.Serialization;

namespace FirmTracker_Server.nHibernate
{
    public class LogsMapping
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Level { get; set; }
        public virtual string Message { get; set; }
        public virtual string  Exception { get; set; } 
    }
}
