﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSCI_2210___Project_3
{
    public class Warehouse
    {
        public List<Dock> Docks { get; }
        public Queue<Truck> Entrance { get; }

        public Warehouse(int dockCount)
        {
            if (dockCount > 15)
            {
                throw new ArgumentException("The maximum number of docks is 15.");
            }

            Docks = new List<Dock>();
            for (int i = 1; i <= dockCount; i++)
            {
                Docks.Add(new Dock($"Dock{i}"));
            }

            Entrance = new Queue<Truck>();
        }

        public Truck GenerateRandomTruck()
        {
            string[] driverNames = { "Luke Berry", "Joe Rutherford", "Jake Gillenwater", "John Doe", "Jeff Bezos" };
            string[] companyNames = { "BTD6", "Fortnite Shopping Cart Delivery", "UPS", "FedEx", "US Mail" };
            Random random = new Random();
            string Driver = driverNames[random.Next(driverNames.Length)];
            string DeliveryCompany = companyNames[random.Next(companyNames.Length)];

            Truck truck = new Truck(Driver, DeliveryCompany);

            int numberCrate = random.Next(1, 15);
            for(int i = 0; i <= numberCrate; i++)
            {
                int id = random.Next(1, 100);
                double cratePrice = random.Next(50, 501); // Random price between $50 and $500
                Crate crate = new Crate(id, cratePrice);
                truck.Load(crate);
            }



            return truck;
        }

        public void Run()
        {
            Random randy = new Random();

            string csvFilePath = "crate_unloading_log.csv";

            for (int time = 1; time <= 100; time++)
            {
                if (randy.Next(100) < 30)
                {
                    Truck newTruck = GenerateRandomTruck();
                    Entrance.Enqueue(newTruck);
                    Console.WriteLine($"Time {time}: Truck arrived -- \n\tDriver: {newTruck.Driver}\n\tCompany: {newTruck.DeliveryCompany}");//company doesn't print
                }

                foreach (Dock dock in Docks)
                {

                    if (dock.Line.Count > 0)
                    {
                        Truck currentTruck = dock.Line.Peek();
                        if (randy.Next(100) < 20)
                        {
                            double Price = randy.Next(50, 501);
                            dock.UnloadCrate(Price);
                            Console.WriteLine($"Time {time}: Unloaded crate from Truck --\nDriver: {currentTruck.Driver}\nCompany: {currentTruck.DeliveryCompany}\nCrate Price: ${Price}");
                            dock.SendOff();

                            string scenario = dock.Line.Count > 1
                                ? "More trucks in line"
                                : dock.Line.Count == 1
                                ? "No more trucks in line"
                                : "No more trucks waiting";

                            string logEntry = $"{time},{currentTruck.Driver},{currentTruck.DeliveryCompany},{currentTruck.Unload().Id},{Price},{scenario}\n";
                            AppendToCsvFile(csvFilePath, logEntry);
                            static void AppendToCsvFile(string csvfilePath, string logEntry)
                            {
                                try
                                {
                                    // Append the new data to the CSV file
                                    using (StreamWriter writer = new StreamWriter(csvfilePath, true))
                                    {
                                        writer.WriteLine(logEntry);
                                    }

                                    Console.WriteLine("Data appended successfully.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error appending data to CSV file: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("\nSimulation completed. Final Statistics --");
            foreach (Dock dock in Docks)
            {
                Console.WriteLine($"Dock {dock.ID} --\nTotal Sales: ${dock.TotalSales}\nTotal Crates: {dock.TotalCrates}\nTotal Trucks:{dock.TotalTrucks}\nTime In Use: {dock.TimeInUse}\n Time Not In Use: {dock.TimeNotInUse}");
            }
        }
    }
}


/*
JOES CODE FOR WAREHOUSE
---------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSCI_2210___Project_3
{
    public class Warehouse
    {
        public List<Dock> Docks { get; }
        public Queue<Truck> Entrance { get; }

        public Warehouse(int dockCount)
        {
            if (dockCount > 15)
            {
                throw new ArgumentException("The maximum number of docks is 15.");
            }

            Docks = new List<Dock>();
            for (int i = 1; i <= dockCount; i++)
            {
                Docks.Add(new Dock($"Dock{i}"));
            }

            Entrance = new Queue<Truck>();
        }

        private Truck GenerateRandomTruck()
        {
            string[] driverNames = { "Luke Berry", "Joe Rutherford", "Jake Gillenwater", "John Doe", "Jeff Bezos" };
            string[] companyNames = { "BTD6", "Fortnite Shopping Cart Delivery", "UPS", "FedEx", "US Mail" };
            Random random = new Random();
            string Driver = driverNames[random.Next(driverNames.Length)];
            string DeliveryCompany = companyNames[random.Next(companyNames.Length)];

            Truck truck = new Truck(Driver, DeliveryCompany);

            int numberCrate = random.Next(1, 15);
            for (int i = 0; i <= numberCrate; i++)
            {
                int id = random.Next(1, 100);
                double cratePrice = random.Next(50, 501); // Random price between $50 and $500
                Crate crate = new Crate(id, cratePrice);
                truck.Load(crate);
            }



            return truck;
        }

        public void Run()
        {
            Random randy = new Random();

            string csvFilePath = "crate_unloading_log.csv";
            File.WriteAllText(csvFilePath, "Time,Driver,Company,CrateId,CrateValue,Scenario\n");

            for (int time = 1; time <= 100; time++)
            {
                if (randy.Next(100) < 30)
                {
                    Truck newTruck = GenerateRandomTruck();
                    Entrance.Enqueue(newTruck);
                    Console.WriteLine($"Time {time}: Truck arrived -- \nDriver: {newTruck.Driver}\nCompany: {newTruck.DeliveryCompany}");
                }

                foreach (Dock dock in Docks)
                {
                    if (randy.Next(100) < 20)
                    {
                        Truck currentTruck = GenerateRandomTruck();
                        // Add truck to the dock line
                        dock.JoinLine(currentTruck);
                        Console.WriteLine($"Time {time}: Truck joined the line --\nDriver: {currentTruck.Driver}\nCompany: {currentTruck.DeliveryCompany}");
                    }

                    if (dock.Line.Count > 0)
                    {
                        Truck currentTruck = dock.SendOff();
                        if (currentTruck != null)
                        {
                            double Price = randy.Next(50, 501);
                            dock.UnloadCrate(Price);
                            Console.WriteLine($"Time {time}: Unloaded crate from Truck --\nDriver: {currentTruck.Driver}\nCompany: {currentTruck.DeliveryCompany}\nCrate Price: ${Price:F2}");

                            Crate unloadedCrate = currentTruck.Unload();

                            string scenario = dock.Line.Count > 0
                                ? "A crate was unloaded, but the truck still has more crates to unload"
                                : "A crate was unloaded, Another truck is already in the Dock";

                            if (unloadedCrate != null)
                            {
                                string logEntry = $"{time},{currentTruck.Driver},{currentTruck.DeliveryCompany},{unloadedCrate.Id},{Price},{scenario}\n";
                                try
                                {
                                    using (StreamWriter writer = new StreamWriter(csvFilePath, true))
                                    {
                                        writer.WriteLine(logEntry);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error writing to CSV file: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Simulation completed. Final Statistics --");
            foreach (Dock dock in Docks)
            {
                Console.WriteLine($"----- {dock.ID} -----\nTotal Sales: ${dock.TotalSales:F2}\nTotal Crates: {dock.TotalCrates}\nTotal Trucks: {dock.TotalTrucks}\nTime In Use: {dock.TimeInUse}\nTime Not In Use: {dock.TimeNotInUse}");
            }
        }

    }
}

*/