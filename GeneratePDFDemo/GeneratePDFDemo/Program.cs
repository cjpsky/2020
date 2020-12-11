using System;
using System.IO;
using Newtonsoft.Json;

namespace GeneratePDFDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var data = File.ReadAllText(path + "\\data.json");
            ComparisonQuotaValue comparisonQuotaValue = JsonConvert.DeserializeObject<ComparisonQuotaValue>(data);

            var outPath = GeneratePDF.ObjectToPdf(comparisonQuotaValue, path);
            Console.WriteLine("pdf生成成功,请到下面路径查看");
            Console.WriteLine(outPath);
        }
    }
}