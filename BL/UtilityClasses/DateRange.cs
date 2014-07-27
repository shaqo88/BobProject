using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.UtilityClasses
{
    /// <summary>
    /// Represents a range of two dates.
    /// </summary>
    public class DateRange
    {
        private DateTime m_startDate;
        private DateTime m_endDate;

        /// <summary>
        /// Gets the date respresenting the start of this range.
        /// </summary>
        public DateTime Start
        {
            get
            {
                return m_startDate; //value type symantics
            }
        }
        /// <summary>
        /// Gets the date respresenting the end of this range.
        /// </summary>
        public DateTime End
        {
            get
            {
                return m_endDate;
            }
        }

        /// <summary>
        /// Constructs a new DateRange using the supplied start and end dates.
        /// </summary>
        /// <param name="startDate">The start of the range.
        /// <param name="endDate">The end of the range.
        public DateRange(DateTime startDate, DateTime endDate)
        {
            this.m_startDate = startDate;
            this.m_endDate = endDate;
        }

        /// <summary>
        /// Returns a string representation of this range in the following format: dd/MM/yyyy - dd/MM/yyyy
        /// </summary>
        /// <returns>A string representation of this range.</returns>
        public override string ToString()
        {
            DateTime dateTime = this.Start;
            string str1 = dateTime.ToString("dd/MM/yyyy");
            string str2 = " - ";
            dateTime = this.End;
            string str3 = dateTime.ToString("dd/MM/yyyy");
            return str1 + str2 + str3;
        }

        /// <summary>
        /// Returns the TimeSpan that this DateRange covers.
        /// </summary>
        /// <returns>A TimeSpan.</returns>
        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(this.m_endDate.Ticks - this.m_startDate.Ticks);
        }

        /// <summary>
        /// Confirms if the supplied date falls between the dates of this range.
        /// </summary>
        /// <param name="date">The date to check for.
        /// <returns>true if the date is between the start and end dates of this range (inclusive).</returns>
        public bool IsInRange(DateTime date)
        {
            return date >= this.m_startDate && date <= this.m_endDate;
        }

        /// <summary>
        /// Confirms if the supplied dates fall between the dates of this range.
        /// </summary>
        /// <param name="dates">An array of dates to check for.
        /// <returns>true if -ALL- dates supplied fall between the start date and end date of this range.</returns>
        public bool IsInRange(params DateTime[] dates)
        {
            return dates.All(d => IsInRange(d));
        }

        /// <summary>
        /// Checks if the supplied daterange is between the dates of this range.
        /// </summary>
        /// <param name="dates">The DateRange to check.
        /// <returns>true if the DateRange starts and ends within this DateRange.</returns>
        public bool IsInRange(DateRange dates)
        {
            return IsInRange(dates.Start, dates.End);
        }
    }

}
