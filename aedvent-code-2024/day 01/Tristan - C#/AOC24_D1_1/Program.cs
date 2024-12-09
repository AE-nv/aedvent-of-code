using System.Collections.Generic;
using System.IO;
using System.Linq;

string input;

try
{
    StreamReader sr = new StreamReader("../../../../../../../input/day1_1.txt");
    input = sr.ReadLine();

    List<int> list1 = new List<int>();
    List<int> list2 = new List<int>();

    while (input != null)
    {
        int spaceIndex = input.IndexOf(' ');

        int number1 = int.Parse(input.Substring(0, spaceIndex));
        int number2 = int.Parse(input.Substring(spaceIndex + 1));

        list1.Add(number1);
        list2.Add(number2);

        input = sr.ReadLine();
    }
    sr.Close();

    System.Console.WriteLine("Got all input into lists...");

    list1.Sort();
    list2.Sort();

    var mappedResult = list1.Select((x, index) => x - list2[index]);

    int res = mappedResult.Select(Math.Abs).Sum();
    System.Console.WriteLine(res);

}
catch (Exception e)
{
    Console.WriteLine("Something went wrong: " + e);
}