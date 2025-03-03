﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCI_2210___Project_3
{
    public class Truck
    {
        public string Driver { get; }
        public string DeliveryCompany { get; }
        public Stack<Crate> Trailer { get; }

       public Truck(string driver, string deliveryCompany)
        {
            Driver = driver;
            deliveryCompany = DeliveryCompany;
            Trailer = new Stack<Crate>();
            
        }

        public void Load(Crate crate)
        {
            Trailer.Push(crate);
        }

       public Crate Unload()
        {
           if(Trailer.Count > 0)
            {
                return Trailer.Pop();
            }
           else
            {
                Console.WriteLine("The trailer is empty.");
                return null;
            }
        }
    }
}
