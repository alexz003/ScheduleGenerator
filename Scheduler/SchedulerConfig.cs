using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    public class SchedulerConfig
    {
        private Random r = new Random();
        private static Employee[] roster;
        private double[] seRatio;
        private double[] predSales;
        //private static double[] predSales;
        private static int workWeek = 0;
        private static HashSet<String> positions = new HashSet<String>();
        private String[] shiftTimes = { "Morning", "Night" };
        private int[] weekHours;
        private String[] allPositions;


        public SchedulerConfig(double[] seRatio, double[] predicSales, int workWeek)
        {
            this.seRatio = seRatio;
            SchedulerConfig.setWorkWeek(workWeek);
            predSales = predicSales;
            roster = Form1.employees.ToArray();
            shiftTimes = Form1.Shifts;
            resetSchedule();
            setAllPositions();
            this.weekHours = getWeekHours();
        }

        /*
        public static double[] predSales
        {
            get { return predSales; }
            set { predSales = value; }
        }
        */

        public static Employee[] Roster
        {
            get { return roster; }
            set { roster = value; }
        }

        public bool isPossibleSchedule()
        {
            for (int x = 0; x < this.allPositions.Length; x++)
            {
                for (int y = 0; y < peoplePerShift().GetLength(0); y++)
                {
                    int totalShifts = 0;

                    foreach (Employee e in roster)
                    {
                        if (e.getPositions().Contains(this.allPositions[x]))
                        {
                            int[] a = e.getMaxShiftsPerDay();
                            
                            totalShifts += e.getMaxShiftsPerDay()[y];

                        }
                    }
                    if (totalShifts < peoplePerShift()[y,x])
                        return false;
                }
            }
            return true;
        }

        public double[] getSeRatio()
        {
            return this.seRatio;
        }

        public void setSeRatio(double[] seRatio)
        {
            this.seRatio = seRatio;
        }

        private int[] getWeekHours()
        {
            int[] weekShifts = new int[allPositions.Length];
            for (int x = 0; x < allPositions.Length; x++)
            {
                weekShifts[x] = 0;
                for (int y = 0; y < workWeek; y++)
                {
                    weekShifts[x] += (int)Math.Round(predSales[y] / seRatio[x]);
                }
                weekShifts[x] = (int)Math.Round(weekShifts[x] / getPeopleForShifts(allPositions[x]));
            }
            return weekShifts;
        }

        public double getPeopleForShifts(String allPositions2)
        {
            int positionCount = 0;
            foreach (Employee e in roster)
                foreach (String s in e.getPositions())
                    if (s.Contains(allPositions2))
                        positionCount++;
            return positionCount;
        }

        private void setAllPositions()
        {
            String tempPositions = "";
            foreach (Employee e in roster)
            {
                foreach (String s in e.getPositions())
                {
                    if (!tempPositions.Contains(s) && !tempPositions.Equals(""))
                        tempPositions += "," + s;
                    else if (!tempPositions.Contains(s))
                        tempPositions += s;
                }
            }
            allPositions = tempPositions.Split(',');
        }

        public void resetSchedule()
        {
            foreach (Employee e in roster)
            {
                e.resetSchedule();
            }
        }

        public void displayScheduleByPosition()
        {
            foreach (String s in positions)
            {
                Console.WriteLine("\n\nName                    Monday             Tuesday             Wednesday             Thursday             Friday             Saturday             Sunday");
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------");

                foreach (Employee e in roster)
                {
                    if (e.getPositions().Contains(s))
                    {
                        Console.Write("%n%-24s", e.getName());
                        for (int x = 0; x < getWorkWeek(); x++)
                        {
                            if (e.getWorkSchedule()[x].Contains("" + s.ToCharArray()[0]))
                                Console.Write("%-20s", e.getWorkSchedule()[x]);
                            else
                                Console.Write("%-20s", "None");
                        }
                    }
                }
            }
        }

        public void displaySchedule()
        {
            Console.WriteLine("Name                    Monday             Tuesday             Wednesday             Thursday             Friday             Saturday             Sunday");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------");

            for (int y = 0; y < roster.Length; y++)
            {
                Console.Write("%-24s%-19s%-20s%-22s%-21s%-19s%-21s%-19s%n", roster[y].getName(), roster[y].getWorkSchedule()[0],
                        roster[y].getWorkSchedule()[1], roster[y].getWorkSchedule()[1],
                        roster[y].getWorkSchedule()[3], roster[y].getWorkSchedule()[4],
                        roster[y].getWorkSchedule()[5], roster[y].getWorkSchedule()[6]);
            }
        }

        public int[] getWeekByVolume()
        {

            
            double[] weekSales = new double[workWeek];
            double[] unsorted = new double[workWeek];
            int[] volumeOrder = new int[workWeek];

            weekSales = predSales;
            unsorted = predSales;
            
            Array.Sort<double>(weekSales);
            int index = 0;

            for (int x = 0; x < weekSales.Length; x++)
            {
                for (int y = 0; y < unsorted.Length; y++)
                {
                    if (weekSales[x] == unsorted[y])
                    {
                        volumeOrder[index] = y;
                        index++;
                    }
                }
            }
            return volumeOrder;
        }

        public Employee[] createScheduleByWeekVolume()
        {
            if (isPossibleSchedule())
            {
                EmployeeQueue eq = new EmployeeQueue(roster);
                Queue<Employee> queue = eq.queueRandom(r);
                long brokenCount = 0;

                foreach (int i in getWeekByVolume())
                {
                    for (int p = 0; p < allPositions.Length; p++)
                    {
                        for (int l = 0; l < shiftTimes.Length; l++)
                        {
                            for (int z = 0; z < peoplePerShift()[i, p]; z++)
                            {
                                bool shiftFilled = false;
                                bool checkDoubles = false;
                                do
                                {
                                    Employee tempEmployee = queue.Dequeue();
                                    if (tempEmployee.getName().Equals("Alex Four"))
                                    {
                                        shiftFilled = false;
                                        int a = 1;
                                        a++;
                                    }
                                    if (tempEmployee.canWork(i, shiftTimes[l], allPositions[p], checkDoubles, weekHours[p]))
                                    {

                                        foreach (Employee e in roster)
                                        {
                                            if (e.getName().Equals(tempEmployee.getName()))
                                            {
                                                e.setSchedule(i, shiftTimes[l], allPositions[p]);
                                                shiftFilled = true;
                                            }
                                        }
                                    }
                                    if (queue.Count == 0 && !shiftFilled)
                                    {
                                        brokenCount++;
                                        if (brokenCount > 1000)
                                        {
                                            return null;
                                        }
                                        queue = eq.queueRandom(r);
                                        checkDoubles = true;
                                        int b = 0;
                                        int totalEmployeesPosition = 0;
                                        foreach (Employee e in roster)
                                        {
                                            int a = 0;
                                            foreach (String positions in e.getPositions())
                                            {
                                                if (positions.Equals(allPositions[p]))
                                                {
                                                    totalEmployeesPosition++;
                                                    foreach (String s in e.getWorkSchedule())
                                                    {
                                                        if (!s.Equals("None"))
                                                        {
                                                            a += s.Split('/').Length;
                                                            if (a == weekHours[p])
                                                                b++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (b == totalEmployeesPosition)
                                        {
                                            weekHours[p]++;
                                        }
                                    }
                                    if (shiftFilled)
                                        queue = eq.queueRandom(r);


                                } while (!shiftFilled);
                            }
                        }
                    }
                }
            }
            return roster;
        }

        public Employee[] createSchedule2()
        {
            if (isPossibleSchedule())
            {
                EmployeeQueue eq = new EmployeeQueue(roster);
                Queue<Employee> queue = eq.queueRandom(r);
                long brokenCount = 0;

                for (int w = 0; w < getWorkWeek(); w++)
                {
                    for (int p = 0; p < allPositions.Length; p++)
                    {
                        for (int l = 0; l < shiftTimes.Length; l++)
                        {
                            for (int z = 0; z < peoplePerShift()[w,p]; z++)
                            {
                                bool shiftFilled = false;
                                bool checkDoubles = false;
                                do
                                {
                                    Employee tempEmployee = queue.Dequeue();
                                    if (tempEmployee.getName().Equals("Alex Four"))
                                    {
                                        shiftFilled = false;
                                        int a = 1;
                                        a++;
                                    }
                                    if (tempEmployee.canWork(w, shiftTimes[l], allPositions[p], checkDoubles, weekHours[p]))
                                    {
                                        
                                        foreach (Employee e in roster)
                                        {
                                            if (e.getName().Equals(tempEmployee.getName()))
                                            {
                                                e.setSchedule(w, shiftTimes[l], allPositions[p]);
                                                shiftFilled = true;
                                            }
                                        }
                                    }
                                    if (queue.Count == 0 && !shiftFilled)
                                    {
                                        brokenCount++;
                                        if (brokenCount > 1000)
                                        {
                                            return null;
                                        }
                                        queue = eq.queueRandom(r);
                                        checkDoubles = true;
                                        int b = 0;
                                        int totalEmployeesPosition = 0;
                                        foreach (Employee e in roster)
                                        {
                                            int a = 0;
                                            foreach (String positions in e.getPositions())
                                            {
                                                if (positions.Equals(allPositions[p]))
                                                {
                                                    totalEmployeesPosition++;
                                                    foreach (String s in e.getWorkSchedule())
                                                    {
                                                        if (!s.Equals("None"))
                                                        {
                                                            a += s.Split('/').Length;
                                                            if (a == weekHours[p])
                                                                b++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (b == totalEmployeesPosition)
                                        {
                                            weekHours[p]++;
                                        }
                                    }
                                    if (shiftFilled)
                                        queue = eq.queueRandom(r);


                                } while (!shiftFilled);
                            }
                        }
                    }
                }
            }
            return roster;
        }

        private int[,] peoplePerShift()
        {
            int[,] perShift = new int[getWorkWeek(), getSeRatio().Length];

            int ax = perShift.GetLength(0);
            int ay = perShift.GetLength(1);

            for (int x = 0; x < perShift.GetLength(0); x++)
            {
                for (int y = 0; y < perShift.GetLength(1); y++)
                {
                    if (((predSales[x] / seRatio[y]) / shiftTimes.Length) < 1)
                        perShift[x,y] = 1;
                    else
                        perShift[x,y] = (int)Math.Round(((predSales[x] / seRatio[y]) / shiftTimes.Length), MidpointRounding.AwayFromZero);
                }
            }

            return perShift;
        }

        public static int getWorkWeek()
        {
            return workWeek;
        }

        public static void setWorkWeek(int workWeek)
        {
            SchedulerConfig.workWeek = workWeek;
        }

        public void saveFullSchedule()
        {//throws IOException {
            Console.WriteLine("Name                    Monday             Tuesday             Wednesday             Thursday             Friday             Saturday             Sunday");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------");

            for (int y = 0; y < roster.Length; y++)
            {
                Console.Write("%-24s%-19s%-20s%-22s%-21s%-19s%-21s%-19s%n", roster[y].getName(), roster[y].getWorkSchedule()[0],
                        roster[y].getWorkSchedule()[1], roster[y].getWorkSchedule()[1],
                        roster[y].getWorkSchedule()[3], roster[y].getWorkSchedule()[4],
                        roster[y].getWorkSchedule()[5], roster[y].getWorkSchedule()[6]);
            }
        }

        //public void saveSchedule(String position) {//throws IOException {

        //        Console.WriteLine("What should the file be called?(schedule.txt)");
        //        System.IO.File file = new System.IO.File(in.next());
			
        //        if(file.exists())
        //            file.delete();
			
        //        file.createNewFile();
        //        Formatter fmt = new Formatter(file);
        //            fmt.format("%n%nName                    Monday             Tuesday             Wednesday             Thursday             Friday             Saturday             Sunday%n");
        //            fmt.format("--------------------------------------------------------------------------------------------------------------------------------------------------------%n");
				
        //            foreach(Employee e in roster) {
        //                if(e.getPositions().Contains(position)) {
        //                    fmt.format("%n%-24s", e.getName());
        //                    for(int x = 0; x < getWorkWeek(); x++) {
        //                        if(e.getWorkSchedule()[x].Contains("" + position.charAt(0)))
        //                            fmt.format("%-20s", e.getWorkSchedule()[x]);
        //                        else
        //                            fmt.format("%-20s", "None");
        //                    }
        //                }
        //            }
        //        fmt.close();
        //}

        //public void saveSchedule() {//throws IOException {
		
        //    Console.WriteLine("\nDo you wish to save the currently generated schedule?(Yes/No)");
        //    if(in.nextLine().toLowerCase().toCharArray()[0] == 'y') {
        //        Console.WriteLine("What should the file be called?(schedule.txt)");
        //        File file = new File(in.next());
			
        //        if(file.exists())
        //            file.delete();
			
        //        file.createNewFile();
        //        Formatter fmt = new Formatter(file);
        //        foreach(String s in positions) {
        //            fmt.format("%n%nName                    Monday             Tuesday             Wednesday             Thursday             Friday             Saturday             Sunday%n");
        //            fmt.format("--------------------------------------------------------------------------------------------------------------------------------------------------------%n");
				
        //            foreach(Employee e in roster) {
        //                if(e.getPositions()).toString().Contains(s)) {
        //                    fmt.format("%n%-24s", e.getName());
        //                    for(int x = 0; x < getWorkWeek(); x++) {
        //                        if(e.getWorkSchedule()[x].Contains("" + s.charAt(0)))
        //                            fmt.format("%-20s", e.getWorkSchedule()[x]);
        //                        else
        //                            fmt.format("%-20s", "None");
        //                    }
        //                }
        //            }
        //        }
        //        fmt.close();
        //    }
        //}
    }
}
