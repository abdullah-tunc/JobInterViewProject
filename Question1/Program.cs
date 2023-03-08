const string validCharacters = "ACDEFGHKLMNPRTXYZ234579";


start: Console.WriteLine("Üretilecek kod sayısını giriniz:");
string input = Console.ReadLine();

int number;
bool isNumber = int.TryParse(input, out number);

if (isNumber)
{
    var codes = GenerateCodes(number);
    Console.WriteLine($"{number} Adet benzersiz kod üretildi");


    foreach (var code in codes)
        Console.WriteLine(code);
}
else
{
    Console.WriteLine("Girilen değer bir sayı değildir.");
    goto start;
}



Console.WriteLine("Kontrol edilecek kodu giriniz:");
string codeInput = Console.ReadLine();

var isValid = CheckCode(codeInput);
if (isValid)
    Console.WriteLine("Girilen kod geçerlidir");
else
    Console.WriteLine("Girilen kod geçersizdir");

Console.ReadLine();
static List<string> GenerateCodes(int count)
{
    var codes = new List<string>();
    var random = new Random();

    while (codes.Count < count)
    {
        var code = new string(Enumerable.Range(0, 8).Select(_ => validCharacters[random.Next(validCharacters.Length)]).ToArray());

        if (!codes.Contains(code))
            codes.Add(code);
    }

    return codes;
}

static bool CheckCode(string code)
{
    return (code.Length == 8) && code.All(c => validCharacters.Contains(c));
}
