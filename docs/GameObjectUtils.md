## GameObjectUtils (GameObject helper methods)
***

`GameObjectUtils` is used to assist with GameObject and Component operations.<br/>
scriptLoader can execute a script multiple times in  one session, so removal of old objects becomes necessary.<br/>
Methods in this class aim to clean up after themselves in a limited way.

#### Static Methods:

```csharp
T AttachComponent<T>(GameObject go) where T : Component

GameObject go = ...;
var comp = GameObjectUtils.AttachComponent<SphereCollider>(go);
```

Attaches and returns a component of type `T` to GameObject `go`<br/>
This method will attempt to remove any components of type `T` from `go` if they exist.

```csharp
bool RemoveComponent<T>(GameObject go) where T : Component

GameObject go = ...;
var removed = GameObjectUtils.RemoveComponent<SphereCollider>(go);
```

Removes all components of type `T` from GameObject `go`<br/>
Returns `true` if at least one removal occurred, otherwise `false`

```csharp
bool AttachComponentToValidScene<T>(string[] valid) where T : Component

var attached = GameObjectUtils.AttachComponentToValidScene<SphereCollider>(new[] {"Title", "Game"});
```

Attempts to attach a component of type `T` to the current scene.<br/>
If the current scene name exists in provided string array `valid`, a component will be attached to the first root gameobject in the scene.<br/>
The component is attached after removing existing components of the same type.<br/>
Returns `true` if the component was attached, otherwise `false`

```csharp
bool AttachComponentToValidScene<T>(string valid) where T : Component

var attached = GameObjectUtils.AttachComponentToValidScene<SphereCollider>("Title");
```

Attempts to attach a component of type `T` to the current scene.<br/>
If the current scene name equals string `valid`, a component will be attached to the first root gameobject in the scene.<br/>
The component is attached after removing existing components of the same type.<br/>
Returns `true` if the component was attached, otherwise `false`


```csharp
Component[] GetComponents(GameObject go)

GameObject go = ...;
var components = GameObjectUtils.GetComponents(go);
```

Returns an array of components attached to GameObject `go`


```csharp
void PrintComponents(GameObject go)

GameObject go = ...;
GameObjectUtils.PrintComponents(go);
```

Iterates components attached to GameObject `go` and prints them to the console.


```csharp
GameObject CreateGameObject(Transform parent, string id)

Transform t = ...;
var newGo = GameObjectUtils.CreateGameObject(t, "NEW_GO");
```

Creates and returns a GameObject with name `id` as a child of `parent`


```csharp
GameObject CreateGameObject(GameObject parent, string id)

GameObject go = ...;
var newGo = GameObjectUtils.CreateGameObject(go, "NEW_GO");
```

Creates and returns a GameObject with name `id` as a child of `parent`

```csharp
bool RemoveGameObject(Transform parent, string id)

Transform t = ...;
var removed = GameObjectUtils.RemoveGameObject(t, "NEW_GO");
```

Removes any GameObject with name `id` from `parent`

```csharp
bool RemoveGameObject(GameObject parent, string id)

GameObject go = ...;
var removed = GameObjectUtils.RemoveGameObject(go, "NEW_GO");
```

Removes any GameObject with name `id` from `parent`<br/>
Returns true if removal occurred, otherwise false.


```csharp
GameObject[] GetRootGameObjects()

var rootObjects = GameObjectUtils.GetRootGameObjects();
```

Returns an array containing the root GameObjects in the current scene.


```csharp
void PrintRootGameObjects()

GameObjectUtils.PrintRootGameObjects();
```

Iterates the root GameObjects in the current scene and prints them to console.


```csharp
void PrintChildren(GameObject go, uint levels = 1)

GameObject go = ...;
GameObjectUtils.PrintChildren(go, 3);
```

Iterates the children of GameObject `go` and recursively prints their children to console.<br/>
Recursion lasts up to a depth of `levels`

```csharp
Scene GetActiveScene()

var currentScene = GameObjectUtils.GetActiveScene();
```

Returns a reference to the current active scene.

```csharp
string GetActiveSceneName()

var currentSceneName = GameObjectUtils.GetActiveSceneName();
```

Returns the name of the current active scene.