using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    class EmployeeQueue
    {
        private Employee[] roster;

        public EmployeeQueue(Employee[] employees)
        {
            this.roster = employees;
        }

        public Queue<Employee> queueRandom(Random r)
        {
            Queue<Employee> queue = new Queue<Employee>();
            List<int> list = new List<int>();

            for (int x = r.Next(roster.Length); queue.Count < roster.Length; x = r.Next(roster.Length))
            {
                if (!list.Contains(x))
                {
                    queue.Enqueue(roster[x]);
                    list.Add(x);
                }
            }
            return queue;
        }
    }
}
