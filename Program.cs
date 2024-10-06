
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Reflection;


while (true) {
    Console.WriteLine("1 - Task 1\n2 - Task 2\nElse - Exit");
    int choise = int.Parse(Console.ReadLine());
    if (choise == 1)
    {
        Console.WriteLine("Enter the setting 1");
        string setting1 = Console.ReadLine();
        Console.WriteLine("Enter the setting 2");
        string setting2 = Console.ReadLine();
        var settings = new Settings(setting1,setting2);
        string fileName = "config.ini";
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        string[] parameters = {
            "Setting1="+settings.Setting1,
            "Setting2=Value2"+settings.Setting2
        };

        using (StreamWriter writer = new StreamWriter(fullPath, true))
        {
            foreach (var parameter in parameters)
            {
                writer.WriteLine(parameter);
            }
        }
        if (File.Exists(fullPath))
        {
            Console.WriteLine($"Параметры из файла '{fileName}':");
            string[] lines = File.ReadAllLines(fullPath);

            foreach (var line in lines)
            {
                Console.WriteLine(line.Split("=")[1]); 
            }
        }

    }
    else if(choise == 2)
    {
        Console.WriteLine("Enter the name");
        string name = Console.ReadLine();
        Console.WriteLine("Enter the age");
        int age = int.Parse(Console.ReadLine());
        var user = new User(name,age);
        Validate(user);
    }
    else
    {
        break;
    }
}




static void Validate<T>(T obj)
{
    var properties = typeof(T).GetProperties();
    foreach (var property in properties)
    {
        var attr = property.GetCustomAttribute<RequiredAttribute>();
        if (attr != null && property.GetValue(obj) == null)
        {
            Console.WriteLine(attr.ErrorMessage);
        }
    }
}
[AttributeUsage(AttributeTargets.Property)]
public class ParamsAttribute : Attribute
{
    public string IniFileName { get; }

    public ParamsAttribute(string iniFileName)
    {
        IniFileName = iniFileName;
    }
}
public class Settings
{

    [Params("config.ini")]
    public string Setting1 { get; set; }

    [Params("config.ini")]
    public string Setting2 { get; set; }


    public Settings(string Setting1, string Setting2)
    {
        this.Setting1 = Setting1;
        this.Setting2 = Setting2;
    }
}
public class IniFileOutput
{
    public static string GetSetting(string iniFilePath, string key)
    {
        var lines = File.ReadAllLines(iniFilePath);
        foreach (var line in lines)
        {
            var parts = line.Split('=');
            if (parts[0].Trim() == key)
                return parts.Length > 1 ? parts[1].Trim() : null;
        }
        return null;
    }

}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
    public string ErrorMessage { get; }

    public RequiredAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

public class User
{
    public string Name { get; set; }

    public int Age { get; set; }

    public User(string Name, int Age)
    {
        this.Name = Name;
        this.Age = Age;
    }
}

