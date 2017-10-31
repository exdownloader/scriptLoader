## Reflect (Reflection)
***

`Reflect` is used to access non-public fields.<br/>

Methods in this class use a default BindingFlags of:<br/>
`BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy`<br/>
Alternative BindingFlags values can  be provided to override the default.

#### Static Methods:

```csharp
FieldInfo FetchField(object o, string f, BindingFlags bf = _bf)

GameObject someObject = ...;
var fi = Reflect.FetchField(someObject, "privateFieldName");
fi.SetValue(someObject, 123);
```

Fetches a field `f` from object `o` using binding flags `bf`


```csharp
T FetchValue<T>(object o, string f, BindingFlags bf = _bf)

GameObject someObject = ...;
var privateField = Reflect.FetchValue<int>(someObject, "privateFieldName");
```

Fetches a value `f` of type `T` from object `o` using binding flags `bf`