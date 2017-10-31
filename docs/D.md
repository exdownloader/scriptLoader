## D (Logging)
***

`D` is used to log text to the console.

The console can be activated by passing in the `--verbose` flag on the CLI.

#### Static Methods:

```csharp
void Log(string msg);

D.Log("console");  //console
```

Prints `msg` to the console.


```csharp
void Log(object msg);

D.Log(42);  //42
```

Casts `msg` to string and prints to the console. If `msg` is null, `NULL` is printed instead.


```csharp
void Log(string msg, params object[] p);

D.Log("Hello {0}!", "World"); //Hello World!
```

Prints a formatted string to the console, wrapper for: `string.format(msg, p);`<br/>
If either param is null, prints `NULL` instead.