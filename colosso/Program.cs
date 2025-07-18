using System;
using System.Collections.Generic;
using System.IO;

class GroceryItem
{
    public string? ID;
    public string? Name;
    public int Quantity;
    public double Price;
    public double Total => Quantity * Price;
}

class Program
{
    static void Main()
    {
        List<GroceryItem> items = new List<GroceryItem>();
        string? inputFile, outputFile;

        Console.Write("Enter input file path (e.g., groceries.txt): ");
        inputFile = Console.ReadLine();

        Console.Write("Enter output file path (e.g., receipt.txt): ");
        outputFile = Console.ReadLine();

        if (string.IsNullOrEmpty(inputFile) || string.IsNullOrEmpty(outputFile))
        {
            Console.WriteLine("❌ File paths cannot be empty. Exiting.");
            return;
        }

        ReadData(inputFile, items);

        if (items.Count == 0)
        {
            Console.WriteLine("❌ No valid items found. Exiting.");
            return;
        }

        double subtotal = 0, tax = 0, grandTotal = 0;
        subtotal = CalculateSubTotal(items, ref tax, ref grandTotal);

        PrintReceipt(items, subtotal, tax, grandTotal);
        WriteReceiptToFile(outputFile, items, subtotal, tax, grandTotal);

        Console.WriteLine("✅ Receipt saved to file.");
    }

    static void ReadData(string filename, List<GroceryItem> items)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("❌ File not found.");
            return;
        }

        foreach (string line in File.ReadLines(filename))
        {
            string[] parts = line.Split(',');
            if (parts.Length == 4)
            {
                try
                {
                    items.Add(new GroceryItem
                    {
                        ID = parts[0],
                        Name = parts[1],
                        Quantity = int.Parse(parts[2]),
                        Price = double.Parse(parts[3])
                    });
                }
                catch
                {
                    Console.WriteLine($"⚠️ Skipping invalid line: {line}");
                }
            }
        }
    }

    static double CalculateSubTotal(List<GroceryItem> items, ref double tax, ref double grandTotal)
    {
        double subtotal = 0;
        foreach (var item in items)
            subtotal += item.Total;

        tax = subtotal * 0.16;
        grandTotal = subtotal + tax;
        return subtotal;
    }

    static void PrintReceipt(List<GroceryItem> items, double subtotal, double tax, double grandTotal)
    {
        Console.WriteLine("\n======= SHOPPING RECEIPT =======");
        Console.WriteLine("ID\tName\tQty\tPrice\tTotal");
        foreach (var item in items)
        {
            Console.WriteLine($"{item.ID}\t{item.Name}\t{item.Quantity}\t{item.Price:F2}\t{item.Total:F2}");
        }
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Subtotal: {subtotal:F2}");
        Console.WriteLine($"Tax (16%): {tax:F2}");
        Console.WriteLine($"Grand Total: {grandTotal:F2}");
        Console.WriteLine("================================\n");
    }

    static void WriteReceiptToFile(string filename, List<GroceryItem> items, double subtotal, double tax, double grandTotal)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine("======= SHOPPING RECEIPT =======");
            writer.WriteLine("ID\tName\tQty\tPrice\tTotal");
            foreach (var item in items)
            {
                writer.WriteLine($"{item.ID}\t{item.Name}\t{item.Quantity}\t{item.Price:F2}\t{item.Total:F2}");
            }
            writer.WriteLine("--------------------------------");
            writer.WriteLine($"Subtotal: {subtotal:F2}");
            writer.WriteLine($"Tax (16%): {tax:F2}");
            writer.WriteLine($"Grand Total: {grandTotal:F2}");
            writer.WriteLine("================================");
        }
    }
}
