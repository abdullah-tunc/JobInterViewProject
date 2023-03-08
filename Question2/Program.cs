using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "response.json");

            string jsonText = File.ReadAllText(jsonFilePath);
            JArray jsonArray = JArray.Parse(jsonText);

            var annotations = new List<Annotation>();

            for (int i = 1; i < jsonArray.Count; i++)
            {
                var annotation = jsonArray[i];
                string description = (string)annotation["description"];
                var vertices = annotation["boundingPoly"]["vertices"];

                int minX = int.MaxValue, minY = int.MaxValue;
                int maxX = int.MinValue, maxY = int.MinValue;

                foreach (var vertex in vertices)
                {
                    int x = (int)vertex["x"];
                    int y = (int)vertex["y"];

                    if (x < minX) minX = x;
                    if (y < minY) minY = y;
                    if (x > maxX) maxX = x;
                    if (y > maxY) maxY = y;
                }

                int xCoord = (minX + maxX) / 2;
                int yCoord = (minY + maxY) / 2;

                var newAnnotation = new Annotation(description, xCoord, yCoord);

                bool merged = false;

                foreach (var item in annotations)
                {
                    if (item.IsClose(newAnnotation))
                    {
                        item.Merge(newAnnotation);
                        merged = true;
                        break;
                    }
                }

                if (!merged)
                {
                    annotations.Add(newAnnotation);
                }
            }

            int index = 1;

            foreach (var annotation in annotations)
            {
                Console.WriteLine($"{index++}| {annotation.Description}");
            }

            Console.ReadLine();
        }
    }

    public class Annotation
    {
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Annotation(string description, int x, int y)
        {
            Description = description;
            X = x;
            Y = y;
        }

        public bool IsClose(Annotation other)
        {
            return Math.Abs(X - other.X) <= 10 && Math.Abs(Y - other.Y) <= 10;
        }

        public void Merge(Annotation other)
        {
            Description += $" {other.Description}";
            X = (X + other.X) / 2;
            Y = (Y + other.Y) / 2;
        }
    }
}
