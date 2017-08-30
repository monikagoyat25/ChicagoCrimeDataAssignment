using CSVtoJSONChicagoCrimeData;
using System.Reflection;

[assembly: AssemblyTitle("CsvToJsonAssembly")]
[assembly: AssemblyVersionAttribute("1.0.0")]

public class Program
    {
        static void Main(string[] args)
        {
            DataLogic obj = new DataLogic();
            obj.CsvToJson();
            obj.WriteData();
        }
    }
