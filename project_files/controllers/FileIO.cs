using Sandelio_app_1.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Sandelio_app_1.controllers
{
    internal class Element
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Weight { get; set; }
        public string Picture { get; set; }

        public override string ToString()
        {
            return $"{Name}, {Width}x{Length}x{Height}, {Weight}kg";
        }
    }

    internal class ElementWithNr : Element
    {
        public string OrderNumber { get; set; }
        public string ClientName {get;set;}

        public ElementWithNr(Element element, string orderNr, string clientName)
        {
            OrderNumber = orderNr;
            ClientName = clientName;
            Name = element.Name;
            Amount = element.Amount;
            Length = element.Length;
            Height = element.Height;
            Width = element.Width;
            Weight = element.Weight;
            Picture = element.Picture;
        }
    }

    internal class Order
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string ClientInfo { get; set; }
        public bool Alone { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Element> Elements { get; set; }

        public string LoadingDate { get; set; }
        public string ClientName { get; set; }
    }

    internal class FileIO
    {
        // reads the file and returns a list of orders
        public static List<Order> ReadFile(string path)
        {
            string json = File.ReadAllText(path);
            List<Order> orders = JsonSerializer.Deserialize<List<Order>>(json);
            return orders;
        }
        internal class SettingsJson
        {
            // this class has a dictionary for countries and a string for currentSelectedCountry
            public int selectedCountryIndex { get; set; }
            public List<CountryOptions> countriesList { get; set; }
        }
        public static void SaveSettings()
        {
            JsonSerializerOptions serializerOptions = new()
            {
                WriteIndented = true
            };
            SettingsJson temporarySettings = new SettingsJson()
            {
                selectedCountryIndex = Settings.SelectedConfigIndex,
                countriesList = Settings.countriesList
            };
            string json = JsonSerializer.Serialize(temporarySettings, serializerOptions);
            // check if A folder called "settings" exists, if not create it
            // then write all data to a file called "settings.json"
            if (!Directory.Exists("settings"))
            {
                Directory.CreateDirectory("settings");
            }
            File.WriteAllText("settings\\settings.json", json);
        }
        public static void LoadSettings()
        {
            // if file does not exist
            // Check if file exists, if not 
            SettingsJson tempSettings = new();
            if (!File.Exists("settings\\settings.json"))
            {
                tempSettings.selectedCountryIndex = 0;
                tempSettings.countriesList = new();
                tempSettings.countriesList.Add(new CountryOptions());
            }
            else
            {
                // if file does exist
                string json = File.ReadAllText("settings\\settings.json");
                tempSettings = JsonSerializer.Deserialize<SettingsJson>(json);
            }
            Settings.SelectedConfigIndex = tempSettings.selectedCountryIndex;
            Settings.countriesList = tempSettings.countriesList;
        }

        // writes the list of orders to the file
        public static void WriteOrders(List<Order> orders, string path)
        {
            JsonSerializerOptions serializerOptions = new()
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(orders, serializerOptions);
            File.WriteAllText(path, json);
        }

        // generates random orders. used for testing
        public static List<Order> GenerateOrders()
        {
            List<Order> orders = new();
            Random rnd = new();
            for (int i = 0; i < 6; i++)
            {
                Order order = new()
                {
                    Number = i + 1,
                    Name = "Order " + (i + 1),
                    ClientInfo = "Client information " + (i + 1),
                    Alone = rnd.Next(0, 2) == 1,
                    Address = "Address " + (i + 1),
                    PostCode = (i + 1).ToString(),
                    City = "City " + (i + 1),
                    Country = "Country " + (i + 1),
                    Elements = new List<Element>()
                };
                for (int j = 0; j < rnd.Next(1, 5); j++)
                {
                    Element element = new()
                    {
                        Name = "Element " + (j + 1),
                        Amount = rnd.Next(1, 4),
                        Width = rnd.Next(100, 200) / 5 * 5,
                        Height = rnd.Next(1000, 2000) / 10 * 10,
                        Length = rnd.Next(900, 3000) / 5 * 5,
                        Weight = rnd.Next(20, 100)
                    };
                    order.Elements.Add(element);
                }
                orders.Add(order);
            }
            return orders;
        }

        // a function that takes a list of orders and creates pallets for them
        public static List<Pallet> CreatePallets(List<Order> orders, string filepath)
        {
            // if Settings.IsAlone = true then set each order.Alone to true
            if (Settings.IsAlone)
            {
                foreach (Order order in orders)
                {
                    order.Alone = true;
                }
            }


            List<Pallet> pallets = new();
            // where order.Alone is false, take that order's items and add them to a separate list
            List<ElementWithNr> elements = new();
            string adress = "";
            string postcode = "";
            string city = "";
            string country = "";
            string orderNumber = "";
            foreach (Order order in orders)
            {
                if (!order.Alone)
                {
                    orderNumber += order.Name + ", ";
                    adress = order.Address;
                    postcode = order.PostCode;
                    city = order.City;
                    country = order.Country;
                    foreach (var element in order.Elements)
                    {
                        elements.Add(new ElementWithNr(element, order.Name, order.ClientName));
                    }
                }
            }
            // remove all orders where order.Alone is false
            for (int h = orders.Count - 1; h >= 0; h--)
            {
                if (!orders[h].Alone)
                {
                    orders.RemoveAt(h);
                }
            }
            // convert all elements to Item list
            List<Item> items = new();
            foreach (var element in elements)
            {
                for (int i = 0; i < element.Amount; i++)
                {
                    items.Add(new Item(element.Name, element.Width, element.Length, element.Height, element.Weight, filepath + "\\" + element.Picture, element.OrderNumber, element.ClientName));
                }
            }
            // while items.Count > 0, create a new pallet and add it to the list
            while (items.Count > 0)
            {
                Pallet pallet = new(pallets.Count + 1)
                {
                    Adress = adress,
                    PostCode = postcode,
                    City = city,
                    Country = country
                };
                items = pallet.Initialize(items);
                pallets.Add(pallet);
            }

            foreach (Order order in orders)
            {
                // convert order list to Item list
                items = new();
                foreach (var element in order.Elements)
                {
                    for (int i = 0; i < element.Amount; i++)
                    {
                        items.Add(new Item(element.Name, element.Width, element.Height, element.Length, element.Weight, filepath + "\\" + element.Picture, order.Name, order.ClientName));
                    }
                }
                // while list not empty create pallet and initialize it with items list
                while (items.Count > 0)
                {
                    Pallet pallet = new(pallets.Count + 1)
                    {
                        Adress = order.Address,
                        PostCode = order.PostCode,
                        City = order.City,
                        Country = order.Country
                    };
                    pallet.Initialize(items);
                    pallets.Add(pallet);
                }
            }
            return pallets;
        }

        public static string[] GetGIFs(string path)
        {
            string[] files = Directory.GetFiles(path, "*.gif");
            return files;
        }
    }
}