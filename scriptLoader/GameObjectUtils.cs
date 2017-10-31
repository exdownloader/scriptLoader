using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace scriptLoader
{
    public static class GameObjectUtils
    {
        #region Components

        public static T AttachComponent<T>(GameObject go) where T : Component
        {
            RemoveComponent<T>(go);
            return go.AddComponent<T>();
        }

        public static bool RemoveComponent<T>(GameObject go) where T : Component
        {
            var removed = false;
            foreach (var comp in go.GetComponents<Component>().Where(x => x.GetType().Name == typeof(T).Name))
            {
                removed = true;
                Object.DestroyImmediate(comp);
            }
            return removed;
        }

        public static bool AttachComponentToValidScene<T>(string[] valid) where T : Component
        {
            var scene = GetActiveScene();
            if (!valid.Contains(scene.name)) return false;
            AttachComponent<T>(scene.GetRootGameObjects()[0]);
            return true;
        }

        public static bool AttachComponentToValidScene<T>(string valid) where T : Component
        {
            return AttachComponentToValidScene<T>(new[] { valid });
        }

        public static Component[] GetComponents(GameObject go)
        {
            return go.GetComponents<Component>();
        }

        public static void PrintComponents(GameObject go)
        {
            foreach (var comp in go.GetComponents<Component>())
                D.Log(comp.GetType());
        }

        #endregion

        #region GameObjects

        public static GameObject CreateGameObject(Transform parent, string id)
        {
            RemoveGameObject(parent, id);
            var go = new GameObject(id);
            go.transform.parent = parent;
            return go;
        }

        public static GameObject CreateGameObject(GameObject parent, string id)
        {
            return CreateGameObject(parent.transform, id);
        }

        public static bool RemoveGameObject(Transform parent, string id)
        {
            var removed = false;
            Transform child = null;
            while ((child = parent.FindChild(id)) != null)
            {
                removed = true;
                Object.DestroyImmediate(child.gameObject, true);
            }
            return removed;
        }

        public static bool RemoveGameObject(GameObject parent, string id)
        {
            return RemoveGameObject(parent.transform, id);
        }

        public static GameObject[] GetRootGameObjects()
        {
            return GetActiveScene().GetRootGameObjects();
        }

        public static void PrintRootGameObjects()
        {
            foreach (var go in GetRootGameObjects())
                D.Log(go);
        }

        public static void PrintChildren(GameObject go, uint levels = 1)
        {
            PrintChildrenRecursive(go.transform, levels, 0);
        }

        private static void PrintChildrenRecursive(Transform t, uint levels, int level)
        {
            if (level >= levels) return;
            var tabulation = new string('\t', level);
            level++;
            foreach (Transform child in t)
            {
                D.Log(tabulation + child.gameObject);
                PrintChildrenRecursive(child, levels, level);
            }
        }

        #endregion

        #region Scenes

        public static UnityEngine.SceneManagement.Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }

        public static string GetActiveSceneName()
        {
            return GetActiveScene().name;
        }

        #endregion
    }

    public static class GameObjectExtensions
    {
        public static T AttachComponent<T>(this GameObject go) where T : Component
        {
            return GameObjectUtils.AttachComponent<T>(go);
        }

        public static bool RemoveComponent<T>(this GameObject go) where T : Component
        {
            return GameObjectUtils.RemoveComponent<T>(go);
        }

        public static Component[] GetComponents(this GameObject go)
        {
            return GameObjectUtils.GetComponents(go);
        }

        public static void PrintComponents(this GameObject go)
        {
            GameObjectUtils.PrintComponents(go);
        }

        public static void PrintChildren(this GameObject go, uint levels = 1)
        {
            GameObjectUtils.PrintChildren(go, levels);
        }
    }
}