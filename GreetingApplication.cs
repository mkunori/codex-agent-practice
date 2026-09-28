public static class GreetingApplication
{
    public static int Run(string[] args, TextWriter output, TextWriter error)
    {
        var name = "Codex Agent Practice";
        var uppercase = false;
        var repeat = 1;

        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == "--repeat")
            {
                if (i + 1 >= args.Length || !int.TryParse(args[i + 1], out repeat) || repeat <= 0)
                {
                    error.WriteLine("Error: --repeat requires a positive integer.");
                    return 1;
                }

                i++;
                continue;
            }

            if (args[i] == "--uppercase")
            {
                uppercase = true;
                continue;
            }

            if (args[i] != "--name")
            {
                error.WriteLine($"Error: Unknown argument '{args[i]}'.");
                return 1;
            }

            if (i + 1 >= args.Length || string.IsNullOrWhiteSpace(args[i + 1]) || args[i + 1].StartsWith("--"))
            {
                error.WriteLine("Error: --name requires a name.");
                return 1;
            }

            name = args[++i];
        }

        var greeting = $"Hello, {name}!";
        if (uppercase)
        {
            greeting = greeting.ToUpperInvariant();
        }

        for (var i = 0; i < repeat; i++)
        {
            output.WriteLine(greeting);
        }
        return 0;
    }
}
