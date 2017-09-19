using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var input_file = GetInputFile(args);
            var input_data = ParseFile(input_file);
            var name_data = ProcessNameData(input_data);
            WriteOuputFile("Names.csv",name_data);
            var address_data = SortAddressData(input_data);
            WriteOuputFile("Address.csv",address_data);
        }

        public static DataTable ProcessNameData(List<Person> input_data)
        {
            var out_data = new DataTable();
            out_data.Columns.Add("Name");
            out_data.Columns.Add("Count");


            var names = new List<string>();
            names.AddRange(input_data.Select(a => a.FirstName));
            names.AddRange(input_data.Select(a => a.LastName));

            var summ_data = names.GroupBy(a => a)
                            .OrderByDescending(a => a.Count())
                            .ThenBy(a => a.Key)
                            .ToDictionary(a => a.Key, a => a.Count());

            foreach (var item in summ_data)
            {
                var row = out_data.NewRow();
                row["Name"] = item.Key;
                row["Count"] = item.Value;
                out_data.Rows.Add(row);
            }
            return out_data;
        }

        public static DataTable SortAddressData(List<Person> input_data)
        {
            var out_data = new DataTable();
            out_data.Columns.Add("Address");
            input_data.Sort((a, b) => a.StreetName.CompareTo(b.StreetName));
            foreach (var person in input_data)
            {
                out_data.Rows.Add(person.Address);
            }
            return out_data;
        }

        public static void WriteOuputFile(string filename,DataTable data)
        {
            using (var output_file = new System.IO.StreamWriter(filename))
            {
                string line;
                foreach(var row in data.AsEnumerable())
                {
                    line = String.Join(",", row.ItemArray);
                    output_file.WriteLine(line);
                }
            }
        }

        public static List<Person> ParseFile(string input_file)
        {
            var data = new List<Person>();
            var lines = File.ReadAllLines(input_file).ToList<string>();
            foreach (var line in lines)
            {
                if (line.StartsWith("FirstName,LastName,")) continue;
                var fields = line.Split(',');
                data.Add(new Person { FirstName = fields[0],
                                      LastName = fields[1],
                                      Address = fields[2],
                                      PhoneNo = fields[3] });
            }
            return data;
        }

        public static string GetInputFile(string[] args)
        {
            var input_filename = "data.csv";
            if (args != null && args.Length > 0) 
                input_filename = args[0].ToString();
            if(!File.Exists(input_filename))
                throw new FileNotFoundException("Input file '{0}' not found",input_filename);
            return input_filename;
        }
    }
}
