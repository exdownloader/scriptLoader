## GameObjectExtensions (GameObject extension methods)
***

`GameObjectExtensions` provides syntactic sugar for operations in `GameObjectUtils`<br/>


#### Extension Methods:

```csharp
T AttachComponent<T>(this GameObject go) where T : Component

GameObject go = ...;
var comp = go.AttachComponent<SphereCollider>();
```

Attaches and returns a component of type `T` to a GameObject.<br/>
This method will attempt to remove any components of type `T` from the GameObject if they exist.

```csharp
bool RemoveComponent<T>(this GameObject go) where T : Component

GameObject go = ...;
var removed = go.RemoveComponent<SphereCollider>();
```

Removes all components of type `T` from a GameObject.<br/>
Returns `true` if at least one removal occurred, otherwise `false`


```csharp
Component[] GetComponents(this GameObject go)

GameObject go = ...;
var components = go.GetComponents();
```

Returns an array of components attached to a GameObject.


```csharp
void PrintComponents(this GameObject go)

GameObject go = ...;
go.PrintComponents();
```

Iterates components attached to a GameObject and prints them to the console.


```csharp
void PrintChildren(this GameObject go, uint levels = 1)

GameObject go = ...;
go.PrintChildren(3);
```

Iterates the children of a GameObjectand recursively prints their children to console.<br/>
Recursion lasts up to a depth of `levels`