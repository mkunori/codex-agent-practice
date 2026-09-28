var cases = new (string Name, string[] Args, string Output, string Error, int ExitCode)[]
{
    ("No arguments", [], "Hello, Codex Agent Practice!", "", 0),
    ("Named greeting", ["--name", "Alice"], "Hello, Alice!", "", 0),
    ("Missing name", ["--name"], "", "Error: --name requires a name.", 1),
    ("Unknown argument", ["--unknown"], "", "Error: Unknown argument '--unknown'.", 1),
    ("Uppercase default greeting", ["--uppercase"], "HELLO, CODEX AGENT PRACTICE!", "", 0),
    ("Uppercase after name", ["--name", "Alice", "--uppercase"], "HELLO, ALICE!", "", 0),
    ("Uppercase before name", ["--uppercase", "--name", "Alice"], "HELLO, ALICE!", "", 0),
    ("Missing name before uppercase", ["--name", "--uppercase"], "", "Error: --name requires a name.", 1),
    ("Missing name after uppercase", ["--uppercase", "--name"], "", "Error: --name requires a name.", 1),
    ("Unknown argument with uppercase", ["--uppercase", "--unknown"], "", "Error: Unknown argument '--unknown'.", 1)
};

var failures = 0;
foreach (var test in cases)
{
    try
    {
        using var output = new StringWriter();
        using var error = new StringWriter();
        var exitCode = GreetingApplication.Run(test.Args, output, error);

        AssertEqual("stdout", Line(test.Output), output.ToString());
        AssertEqual("stderr", Line(test.Error), error.ToString());
        AssertEqual("exit code", test.ExitCode, exitCode);
        Console.WriteLine($"PASS: {test.Name}");
    }
    catch (Exception exception)
    {
        failures++;
        Console.Error.WriteLine($"FAIL: {test.Name}: {exception.Message}");
    }
}

Console.WriteLine($"{cases.Length - failures} passed, {failures} failed.");
return failures == 0 ? 0 : 1;

static string Line(string value) => value.Length == 0 ? "" : value + Environment.NewLine;

static void AssertEqual<T>(string field, T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"{field}: expected [{expected}], actual [{actual}]");
    }
}
