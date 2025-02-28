using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace YTMembers;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        CVSReader();
    }

    static void CVSReader()
    {
        Console.Write($"Where is the file located? \nPath: ");
        string filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        Console.Write($"Enter the name of the membership tier (Caps Sensitive): \n");
        List<string> tierName = new List<string>();
        while (true)
        {
            Console.Write($"{tierName.Count + 1}. Tier: ");
            tierName.Add(Console.ReadLine());

            Console.Write("Do you want to add another tier? (Y/N): ");
            if (Console.ReadLine().ToLower() == "n")
            {
                break;
            }
        }

        using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
        using (StreamWriter sw = new StreamWriter("Members_" + IntToMonth() + ".txt"))
        {
            Dictionary<string, List<string>> tierMembers = tierName.ToDictionary(tier => tier, tier => new List<string>());

            while (!sr.EndOfStream)
            {
                string[] values = sr.ReadLine().Split(',');
                if (tierMembers.ContainsKey(values[2]))
                {
                    tierMembers[values[2]].Add(values[0]);
                }
            }

            foreach (var tier in tierMembers)
            {
                sw.WriteLine($"Tier - {tier.Key}: ");
                foreach (string name in tier.Value)
                {
                    sw.WriteLine(name);
                }
                sw.WriteLine();
            }
        }
    }

    public static string IntToMonth()
    {
        return DateTime.Now.ToString("MMMM");
    }
}