using Newtonsoft.Json.Linq;

internal class Program
{
    static void Main()
    {
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "response.json");
 
        string jsonText = File.ReadAllText(jsonFilePath);
        JArray jsonArray = JArray.Parse(jsonText);
        int i = 0;

        int prevXCoord = 0;
        int prevYCoord = 0;

        foreach (var annotation in jsonArray)
        {
            if (i != 0)
            {
                string description = (string)annotation["description"];
                var vertices = annotation["boundingPoly"]["vertices"];

                int minX = int.MinValue , minY = int.MinValue;
                int maxX = int.MaxValue, maxY = int.MaxValue;

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

                int newXCoord = xCoord;
                int newYCoord = yCoord + description.Length + 2;  
                
                if (newYCoord == prevYCoord)  
                {
                  
                    Console.SetCursorPosition(prevXCoord, newYCoord);
                    Console.Write($"{i}| {description}  "); 
                    prevXCoord += description.Length + 4;  
                }
                else
                {
                     
                    Console.SetCursorPosition(newYCoord, newXCoord);
                    Console.WriteLine($"{i}| {description}");
                    prevXCoord = newXCoord;
                    prevYCoord = newYCoord;
                }
            }

            i++;
        }
    }
}
