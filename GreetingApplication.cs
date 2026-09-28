public static class GreetingApplication
{
    public static int Run(string[] args, TextWriter output, TextWriter error)
    {
        var name = "Codex Agent Practice";
        var uppercase = false;

        for (var i = 0; i < args.Length; i++)
        {
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
        output.WriteLine(uppercase ? greeting.ToUpperInvariant() : greeting);
        return 0;
    }
}
