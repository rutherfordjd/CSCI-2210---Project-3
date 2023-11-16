using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI_2210___Project_3
{
    public class Dock
    {
        public string ID { get; }
        public Queue<Truck> Line { get; }
        public double TotalSales { get; private set; }
        public int TotalCrates { get; private set; }
        public int TotalTrucks { get; private set; }
        public int TimeInUse { get; private set; }
        public int TimeNotInUse { get; private set; }

        public Dock(string id)
        {
            ID = id;
            Line = new Queue<Truck>();
        }


       public void JoinLine(Truck truck)
        {
            Line.Enqueue(truck);
        }

        public Truck SendOff()
        {
            if (Line.Count > 0)
            {
                var truck = Line.Dequeue();
                TotalTrucks++;
                TimeInUse++;
                return truck;
            }
            else
            {
                TimeNotInUse++;
                Console.WriteLine("There are no trucks to send off.");
                return null;
            }
        }

        public void UnloadCrate(double Price)
        {
            TotalSales += Price;
            TotalCrates++;
        }
    }
}
