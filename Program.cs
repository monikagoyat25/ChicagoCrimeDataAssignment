using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CrimeDataFinalCSVtoJSON
{
   class Program
    {
        static void Main(string[] args)
        {
            FileStream freader = new FileStream(@"C:\Users\Training\Downloads\crimedata.csv", FileMode.Open, FileAccess.ReadWrite);
            FileStream fwriter = new FileStream(@"C:\Users\Training\source\repos\CSVtoJSON-ChicagoCrimeData\CSVtoJSON-ChicagoCrimeData\data\barchartdata.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            FileStream fwriter1 = new FileStream(@"C:\Users\Training\source\repos\CSVtoJSON-ChicagoCrimeData\CSVtoJSON-ChicagoCrimeData\data\linechartdata.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            FileStream fwriter2 = new FileStream(@"C:\Users\Training\source\repos\CSVtoJSON-ChicagoCrimeData\CSVtoJSON-ChicagoCrimeData\data\piechartdata.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(fwriter);
            StreamWriter writer1 = new StreamWriter(fwriter1);
            StreamWriter writer2 = new StreamWriter(fwriter2);
            StreamReader reader = new StreamReader(freader);
            String[] Heading = new String[6] { "Year", "Over$500", "Under$500", "Year", "Arrest", "NotArrest"  };
            String[] pieheading = new String[4] { "IndexCrime", "NonIndexCrime", "VoilentCrime", "PropertyCrime" };
            int[] TheftPerYearUnder500 = new int[16];
            int[] TheftPerYearOver500 = new int[16];
            int[] Assault_Arrest_peryear = new int[16];
            int[] Assault_notArrest_peryear = new int[16];
            int[] CrimeCategory = new int[4];
            string line = string.Empty;
            int x = 0, count = 0;
            while ((line = reader.ReadLine()) != null)
            {
                count++;
                string[] values = line.Split('"');
                if (values.Length > 1)
                {
                    for (int i = 1; i < values.Length; i += 2)
                    {
                        values[i] = values[i].Replace(",", " ");
                    }
                }
                line = string.Empty;
                foreach (var a1 in values)
                {
                    line += a1;
                }
                string[] val = line.Split(',');
                if (count == 1)
                    continue;
                else
                {
                    Int32 year;
                    Int32.TryParse(val[17], out year);
                    if (year >= 2001 && year <= 2016)
                    {
                        x = year - 2001;
                        int one = (val[6] == "OVER $500") ? TheftPerYearOver500[x] += 1 : ((val[6] == "$500 AND UNDER") ? TheftPerYearUnder500[x] += 1 : 0);
                        int two = ((val[5] == "ASSAULT") && (val[8].ToLower() == "true")) ? Assault_Arrest_peryear[x] += 1 : ((val[5] == "ASSAULT" && val[8].ToLower() == "false") ? Assault_notArrest_peryear[x] += 1 : 0);
                    }
                    if (year == 2015)
                    {
                        if (val[14] == "01A" || val[14] == "02" || val[14] == "03" || val[14] == "04A" || val[14] == "04B" || val[14] == "05" || val[14] == "06" || val[14] == "07" || val[14] == "09")
                            CrimeCategory[0] += 1;
                        if (val[14] == "01B" || val[14] == "08A" || val[14] == "08B" || val[14] == "10" || val[14] == "11" || val[14] == "12" || val[14] == "13" || val[14] == "14" || val[14] == "15" || val[14] == "16" || val[14] == "17" || val[14] == "18" || val[14] == "19" || val[14] == "20" || val[14] == "22" || val[14] == "24" || val[14] == "26")
                            CrimeCategory[1] += 1;
                        if (val[14] == "01A" || val[14] == "02" || val[14] == "03" || val[14] == "04A" || val[14] == "04B")
                            CrimeCategory[2] += 1;
                        if (val[14] == "05" || val[14] == "06" || val[14] == "07" || val[14] == "09")
                            CrimeCategory[3] += 1;
                    }
                }
            }
            freader.Flush();
            reader.Dispose();
            // BAR CHART  
            writer.WriteLine("[");
            for (int p = 0; p < 16; p++)
            {
                int a1 = p + 2001;
                writer.WriteLine("{ \n \"" + Heading[0] + "\":\""+ a1 + "\" , \n \"" + Heading[1]+"\":\""+TheftPerYearOver500[p]+"\" , \n \""+Heading[2]+"\":\"" + TheftPerYearUnder500[p]+"\"\n }");
                if (p < 15)
                    writer.Write(',');
            }
            writer.WriteLine(" ]");
            fwriter.Flush(); writer.Flush();
            // MULTISERIES LINE CHART
            writer1.WriteLine("{\n\"Assault\": [");
            for (int p = 0; p < 16; p++)
            {
                int a2 = p + 2001;
                writer1.WriteLine("{ \n \"" + Heading[3] + "\":\"" + a2 + "\" , \n \"" + Heading[4] + "\":\"" + Assault_Arrest_peryear[p] + "\" , \n \"" + Heading[5] + "\":\"" + Assault_notArrest_peryear[p] + "\"\n }");
                if (p < 15)
                    writer1.Write(',');
            }
            writer1.WriteLine(" ] \n }");
            fwriter1.Flush(); writer1.Flush();
            // Pie Chart
            writer2.WriteLine("[");
            for (int p = 0; p < 4; p++)
            {
                writer2.WriteLine("{ \n \"Category\":\"" + pieheading[p] + "\" , \n \"Value\":\"" + CrimeCategory[p] + "\"\n } ");
                if (p < 3) writer2.Write(',');
            }
            writer2.WriteLine("\n]");
            fwriter2.Flush(); writer2.Flush();
        }
    }
}
