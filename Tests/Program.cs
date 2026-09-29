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
    ("Unknown argument with uppercase", ["--uppercase", "--unknown"], "", "Error: Unknown argument '--unknown'.", 1),
    ("Repeat default greeting", ["--repeat", "3"], Repeat("Hello, Codex Agent Practice!", 3), "", 0),
    ("Repeat named greeting", ["--name", "Alice", "--repeat", "2"], Repeat("Hello, Alice!", 2), "", 0),
    ("Repeat uppercase greeting", ["--uppercase", "--repeat", "2"], Repeat("HELLO, CODEX AGENT PRACTICE!", 2), "", 0),
    ("Name uppercase repeat", ["--name", "Alice", "--uppercase", "--repeat", "2"], Repeat("HELLO, ALICE!", 2), "", 0),
    ("Name repeat uppercase", ["--name", "Alice", "--repeat", "2", "--uppercase"], Repeat("HELLO, ALICE!", 2), "", 0),
    ("Uppercase name repeat", ["--uppercase", "--name", "Alice", "--repeat", "2"], Repeat("HELLO, ALICE!", 2), "", 0),
    ("Uppercase repeat name", ["--uppercase", "--repeat", "2", "--name", "Alice"], Repeat("HELLO, ALICE!", 2), "", 0),
    ("Repeat name uppercase", ["--repeat", "2", "--name", "Alice", "--uppercase"], Repeat("HELLO, ALICE!", 2), "", 0),
    ("Repeat uppercase name", ["--repeat", "2", "--uppercase", "--name", "Alice"], Repeat("HELLO, ALICE!", 2), "", 0),
    ("Repeat once", ["--repeat", "1"], "Hello, Codex Agent Practice!", "", 0),
    ("Missing repeat value", ["--repeat"], "", "Error: --repeat requires a positive integer.", 1),
    ("Zero repeat", ["--repeat", "0"], "", "Error: --repeat requires a positive integer.", 1),
    ("Negative repeat", ["--repeat", "-1"], "", "Error: --repeat requires a positive integer.", 1),
    ("Non-numeric repeat", ["--repeat", "abc"], "", "Error: --repeat requires a positive integer.", 1),
    ("Fractional repeat", ["--repeat", "1.5"], "", "Error: --repeat requires a positive integer.", 1),
    ("Overflow repeat", ["--repeat", "2147483648"], "", "Error: --repeat requires a positive integer.", 1),
    ("Option instead of repeat value", ["--repeat", "--uppercase"], "", "Error: --repeat requires a positive integer.", 1),
    ("Missing name with repeat", ["--repeat", "2", "--name"], "", "Error: --name requires a name.", 1),
    ("Unknown argument with repeat", ["--repeat", "2", "--unknown"], "", "Error: Unknown argument '--unknown'.", 1),
    ("Custom greeting", ["--greeting", "Hi"], "Hi, Codex Agent Practice!", "", 0),
    ("Greeting before name", ["--greeting", "Hi", "--name", "Alice"], "Hi, Alice!", "", 0),
    ("Greeting after name", ["--name", "Alice", "--greeting", "Hi"], "Hi, Alice!", "", 0),
    ("Uppercase custom greeting", ["--greeting", "Hi", "--uppercase"], "HI, CODEX AGENT PRACTICE!", "", 0),
    ("Repeat custom greeting", ["--greeting", "Hi", "--repeat", "2"], Repeat("Hi, Codex Agent Practice!", 2), "", 0),
    ("All options with custom greeting", ["--name", "Alice", "--greeting", "Hi", "--uppercase", "--repeat", "2"], Repeat("HI, ALICE!", 2), "", 0),
    ("Custom greeting last", ["--repeat", "2", "--uppercase", "--name", "Alice", "--greeting", "Hi"], Repeat("HI, ALICE!", 2), "", 0),
    ("Missing greeting", ["--greeting"], "", "Error: --greeting requires a greeting.", 1),
    ("Uppercase instead of greeting", ["--greeting", "--uppercase"], "", "Error: --greeting requires a greeting.", 1),
    ("Name option instead of greeting", ["--greeting", "--name", "Alice"], "", "Error: --greeting requires a greeting.", 1),
    ("Empty greeting", ["--greeting", ""], "", "Error: --greeting requires a greeting.", 1),
    ("Whitespace greeting", ["--greeting", "  "], "", "Error: --greeting requires a greeting.", 1),
    ("Missing name with custom greeting", ["--greeting", "Hi", "--name"], "", "Error: --name requires a name.", 1),
    ("Invalid repeat with custom greeting", ["--greeting", "Hi", "--repeat", "0"], "", "Error: --repeat requires a positive integer.", 1),
    ("Unknown argument with custom greeting", ["--greeting", "Hi", "--unknown"], "", "Error: Unknown argument '--unknown'.", 1),
    ("Punctuation", ["--punctuation", "?"], "Hello, Codex Agent Practice?", "", 0),
    ("Name and punctuation", ["--name", "Alice", "--punctuation", "?"], "Hello, Alice?", "", 0),
    ("Greeting and punctuation", ["--greeting", "Hi", "--punctuation", "?"], "Hi, Codex Agent Practice?", "", 0),
    ("Uppercase and punctuation", ["--uppercase", "--punctuation", "?"], "HELLO, CODEX AGENT PRACTICE?", "", 0),
    ("Repeat and punctuation", ["--repeat", "2", "--punctuation", "?"], Repeat("Hello, Codex Agent Practice?", 2), "", 0),
    ("All options with punctuation", ["--name", "Alice", "--greeting", "Hi", "--punctuation", "?", "--uppercase", "--repeat", "2"], Repeat("HI, ALICE?", 2), "", 0),
    ("Punctuation first", ["--punctuation", "?", "--repeat", "2", "--uppercase", "--greeting", "Hi", "--name", "Alice"], Repeat("HI, ALICE?", 2), "", 0),
    ("Missing punctuation", ["--punctuation"], "", "Error: --punctuation requires a value.", 1),
    ("Option instead of punctuation", ["--punctuation", "--uppercase"], "", "Error: --punctuation requires a value.", 1),
    ("Empty punctuation", ["--punctuation", ""], "", "Error: --punctuation requires a single non-whitespace character.", 1),
    ("Whitespace punctuation", ["--punctuation", " "], "", "Error: --punctuation requires a single non-whitespace character.", 1),
    ("Multiple punctuation characters", ["--punctuation", "?!"], "", "Error: --punctuation requires a single non-whitespace character.", 1),
    ("Unrestricted character type", ["--punctuation", "x"], "Hello, Codex Agent Practicex", "", 0),
    ("Unicode punctuation", ["--punctuation", "😀"], "Hello, Codex Agent Practice😀", "", 0),
    ("Missing name with punctuation", ["--punctuation", "?", "--name"], "", "Error: --name requires a name.", 1),
    ("Missing greeting with punctuation", ["--punctuation", "?", "--greeting"], "", "Error: --greeting requires a greeting.", 1),
    ("Invalid repeat with punctuation", ["--punctuation", "?", "--repeat", "0"], "", "Error: --repeat requires a positive integer.", 1),
    ("Unknown argument with punctuation", ["--punctuation", "?", "--unknown"], "", "Error: Unknown argument '--unknown'.", 1)
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

static string Repeat(string value, int count) => string.Join(Environment.NewLine, Enumerable.Repeat(value, count));

static void AssertEqual<T>(string field, T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"{field}: expected [{expected}], actual [{actual}]");
    }
}
