## DataUtils (Data Import/Export)
***

`DataUtils` is used to import and export data.

#### Static Methods:

```csharp
void SaveTex2D(Texture2D tex, string name, TextureFormat fmt = TextureFormat.ARGB32)

Texture2D tex = ...;
DataUtils.SaveTex2D(tex, "file.png");
```

Saves provided Texture2D `tex` to an output file.<br/>
Even though any file name can be provided, the texture is encoded to PNG.<br/>
The output file path `name` is relative to the application executable path.