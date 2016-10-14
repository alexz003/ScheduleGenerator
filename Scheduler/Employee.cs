using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class Employee
    {
        /** The name. */
        private String name;

        /** The week length. */
        private int weekLength = 7;

        /** The available. */
        private bool[] available;

        /** The shift. */
        private String[] shift;

        /** The wages. */
        private double[] wages;

        /** The positions. */
        private String[] positions = new String[0];

        /** The work schedule. */
        private String[] workSchedule;// = new String[weekLength];

        /** The max shifts per day. */
        private int[] maxShiftsPerDay;

        /**
         * Instantiates a new employee.
         *
         * @param name Employee's name
         * @param position Employee's position
         * @param wage Employee's wage
         * @param availability Employee's day availability
         * @param shift Employee's shift availability
         */
        public Employee(String name, String position, String wage, bool[] availability, String[] shift)
        {
            this.name = name;
            this.available = availability;
            this.shift = shift;
            this.workSchedule = new String[weekLength];
            this.maxShiftsPerDay = getDefaultShiftsPerDay();
            setPositions(position);
            setWages(wage);
        }

        /**
         * Instantiates a new employee.
         *
         * @param name Employee's name
         * @param position Employee's position
         * @param wage Employee's wage
         * @param availability Employee's availability
         * @param shift Employee's shift
         * @param maxShiftsDay Employee's max number of shifts per day
         */
        public Employee(String name, String position, String wage, bool[] availability, String[] shift, int[] maxShiftsDay)
        {
            this.name = name;
            this.available = availability;
            this.shift = shift;
            this.workSchedule = new String[weekLength];
            this.maxShiftsPerDay = maxShiftsDay;
            setPositions(position);
            setWages(wage);
        }

        /**
         * Gets the default shifts per day.
         *
         * @return the default shifts per day for entire week
         */
        private int[] getDefaultShiftsPerDay() {
		int[] a = new int[weekLength];
		
		for(int x = 0; x < weekLength; x++) {
			if(available[x]) {
				if(this.shift[x].Contains("Any"))
					a[x] = 1;
				else if(this.shift[x].Contains("none"))
					a[x] = 0;
				else
					a[x] = 1;
			}
			else
				a[x] = 0;
		}
		
		return a;
	}

        /**
         * Gets the shift count.
         *
         * @return The total number of shifts this employee can work for the whole week
         */
        public int getShiftCount() {
		int a = 0;
		foreach(String s in workSchedule)
				if(!s.Contains("None"))
					a += s.Split('/').Length;
		return a;
	}

        /**
         * Sets the wages.
         *
         * @param wage Set's wages based on position
         */
        private void setWages(String wage)
        {
            String[] wages = wage.Split('/');
            this.wages = new double[wages.Length];

            for (int x = 0; x < wages.Length; x++)
                this.wages[x] = Double.Parse(wages[x]);
        }

        /**
         * Sets the positions.
         *
         * @param position Set's each position
         */
        private void setPositions(String position)
        {
            this.positions = position.Split('/');
        }

        private void setPositions(String[] positions)
        {
            this.positions = positions;
        }

        /**
         * Can the Employee work shift in question
         *
         * @param dayOfWeek The day of week
         * @param timeOfDay The time of day
         * @param position The position
         * @return true, if employee can work shift
         */
        public bool canWork(int dayOfWeek, String timeOfDay, String position) {
		foreach(String s in positions)
			if(s.Contains(position) && available[dayOfWeek] && workSchedule[dayOfWeek].Equals("None") && (shift[dayOfWeek].Equals(timeOfDay) || shift[dayOfWeek].Equals("Any")))
				return true;
		return false;
	}

        /**
         * Can work.
         *
         * @param dayOfWeek The day of week
         * @param timeOfDay The time of day
         * @param position The position
         * @param multiple Can they work a double shift?
         * @param maxDaysScheduled The max number of shifts this employee can work
         * @return true, if employee can work shift
         */
        public bool canWork(int day, String time, String position, bool multiple, int maxDaysScheduled)
        {
            if (positions.Cast<String>().Contains(position))
            {
                if (this.available[day])
                {
                    if (shift[day].Contains(time) || shift[day].Equals("Any"))
                    {
                        if (this.workSchedule[day].Equals("None"))
                        {
                            if (this.getShiftCount() < maxDaysScheduled)
                                return true;
                        }
                        else if (!this.workSchedule[day].Contains(time) && multiple)
                            return true;
                    }
                }
            }
            return false;
        }

        /**
         * Resets employee's schedule.
         */
        public void resetSchedule()
        {
            for (int x = 0; x < weekLength; x++)
            {
                workSchedule[x] = "None";
            }
        }

        /**
         * Sets employee's schedule.
         *
         * @param dayOfWeek The day of week
         * @param timeOfDay The time of day
         * @param position The position
         */
        public void setSchedule(int dayOfWeek, String timeOfDay, String position)
        {
            if (workSchedule[dayOfWeek].Equals("None"))
                workSchedule[dayOfWeek] = timeOfDay + "[" + position.ToCharArray()[0] + "]";
            else
                workSchedule[dayOfWeek] += "/" + timeOfDay + "[" + position.ToCharArray()[0] + "]";
        }

        /**
         * Gets the max shifts employee can work for entire week.
         *
         * @return The max shifts each day
         */
        public int[] getMaxShiftsPerDay()
        {
            return this.maxShiftsPerDay;
        }

        /**
         * Gets employee's work schedule.
         *
         * @return Employee's work schedule
         */
        public String[] getWorkSchedule()
        {
            return workSchedule;
        }

        /**
         * Gets employee name.
         *
         * @return Employee's name
         */
        public String getName()
        {
            return name;
        }

        /**
         * Gets employee's positions.
         *
         * @return Employee's positions
         */
        public String[] getPositions()
        {
            return positions;
        }

        /**
         * Gets the week length.
         *
         * @return The week length
         */
        public int getWeekLength()
        {
            return weekLength;
        }

        /**
         * Gets employee's available.
         *
         * @return Employee's availability
         */
        public bool[] getAvailablility()
        {
            return available;
        }

        /**
         * Gets shifts available for each day.
         *
         * @return Employee's available shifts
         */
        public String[] getShift()
        {
            return shift;
        }

        /**
         * Gets employee's wage.
         *
         * @return Employee's wage
         */
        public double[] getWage()
        {
            return wages;
        }

        public static int compareTo(Employee e)
        {
            
            return 0;
        }
    }
}
