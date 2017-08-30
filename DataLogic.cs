using System;
using System.IO;
using System.Text;

namespace CSVtoJSONChicagoCrimeData
{
    public class DataLogic
    {
        String[] Heading = new String[6] { "Year", "Over$500", "Under$500", "Year", "Arrest", "NotArrest" };
        String[] pieheading = new String[4] { "IndexCrime", "NonIndexCrime", "VoilentCrime", "PropertyCrime" };
        int[] TheftPerYearUnder500 = new int[16];
        int[] TheftPerYearOver500 = new int[16];
        int[] Assault_Arrest_peryear = new int[16];
        int[] Assault_notArrest_peryear = new int[16];
        int[] CrimeCategory = new int[4];
        StringBuilder file = new StringBuilder();
        StringBuilder file1 = new StringBuilder();
        StringBuilder file2 = new StringBuilder();
        public void CsvToJson()
        {
            FileStream freader = new FileStream(@"C:\Users\Training\Downloads\crimedata.csv", FileMode.Open, FileAccess.ReadWrite);
            StreamReader reader = new StreamReader(freader);
            string line = string.Empty;
            int x = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("ASSAULT") || line.Contains("OVER $500") || line.Contains("$500 AND UNDER")|| line.Contains("2015"))
                {
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

                    Int32 year;
                    Int32.TryParse(val[17], out year);
                    if (year >= 2001 && year <= 2016)
                    {
                        if (val[6] == "OVER $500")
                        {
                            x = year - 2001;
                            TheftPerYearOver500[x] += 1;
                        }
                        else if (val[6] == "$500 AND UNDER")
                        {
                            x = year - 2001;
                            TheftPerYearUnder500[x] += 1;
                        }

                        if ((val[5] == "ASSAULT") && (val[8].ToLower() == "true"))
                        {
                            x = year - 2001;
                            Assault_Arrest_peryear[x] += 1;
                        }
                        else if (val[5] == "ASSAULT" && val[8].ToLower() == "false")
                        {
                            x = year - 2001;
                            Assault_notArrest_peryear[x] += 1;
                        }
                    }
                    if (year == 2015)
                    {
                        if (val[14] == "01A" || val[14] == "02" || val[14] == "03" || val[14] == "04A" || val[14] == "04B" || val[14] == "05" || val[14] == "06" || val[14] == "07" || val[14] == "09")
                        {
                            CrimeCategory[0] += 1;
                        }
                        if (val[14] == "01B" || val[14] == "08A" || val[14] == "08B" || val[14] == "10" || val[14] == "11" || val[14] == "12" || val[14] == "13" || val[14] == "14" || val[14] == "15" || val[14] == "16" || val[14] == "17" || val[14] == "18" || val[14] == "19" || val[14] == "20" || val[14] == "22" || val[14] == "24" || val[14] == "26")
                        {
                            CrimeCategory[1] += 1;
                        }
                        if (val[14] == "01A" || val[14] == "02" || val[14] == "03" || val[14] == "04A" || val[14] == "04B")
                        {
                            CrimeCategory[2] += 1;
                        }
                        if (val[14] == "05" || val[14] == "06" || val[14] == "07" || val[14] == "09")
                        {
                            CrimeCategory[3] += 1;
                        }
                    }
                }
            }
            freader.Flush();
            reader.Dispose();
        }
        public void WriteData()
        {
            // BAR CHART  
            FileStream fwriter = new FileStream(@"data\barchartdata.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(fwriter);
            file.AppendFormat("[");
            for (int p = 0; p < 16; p++)
            {
                int a = p + 2001;
                file.Append("{ \n").AppendFormat("\"{0}\":\"{1}\"", Heading[0], a).Append(',').Append('\n').AppendFormat("\"{0}\":\"{1}\"", Heading[1], TheftPerYearOver500[p]).Append(',').Append('\n').AppendFormat("\"{0}\":\"{1}\"", Heading[2], TheftPerYearUnder500[p]).Append("\n }");
                if (p < 15)
                    file.Append(',');
                
                file.Append('\n');
            }
            file.Append("]");
            writer.WriteLine(file);
            fwriter.Flush(); writer.Flush(); writer.Dispose();
            // MULTISERIES LINE CHART
            FileStream fwriter1 = new FileStream(@"data\linechartdata.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer1 = new StreamWriter(fwriter1);
            file1.AppendFormat("[");
            for (int p = 0; p < 16; p++)
            {
                int a = p + 2001;
                file1.Append("{ \n").AppendFormat("\"{0}\":\"{1}\"", Heading[3], a).Append(',').Append('\n').AppendFormat("\"{0}\":\"{1}\"", Heading[4], Assault_Arrest_peryear[p]).Append(',').Append('\n').AppendFormat("\"{0}\":\"{1}\"", Heading[5], Assault_notArrest_peryear[p]).Append("\n }");
                if (p < 15)
                    file1.Append(',');
                
                file1.Append('\n');
            }
            file1.Append("]");
            writer1.WriteLine(file1);
            fwriter1.Flush(); writer1.Flush(); writer1.Dispose();
            // Pie Chart
            FileStream fwriter2 = new FileStream(@"data\piechartdata.json", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer2 = new StreamWriter(fwriter2);
            file2.Append("[");
            for (int p = 0; p < 4; p++)
            {
                file2.Append("{ \n").AppendFormat("\"Category\":\"{0}\"", pieheading[p]).Append(" ,\n ").AppendFormat("\"Value\":\"{0}\"", CrimeCategory[p]).Append("\n }");
                if (p < 3) file2.Append(',');
            }
            file2.Append("\n]");
            writer2.WriteLine(file2);
            fwriter2.Flush(); writer2.Flush(); writer2.Dispose();
        }
    }
}
