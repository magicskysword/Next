using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkySwordKill.Next.Utils;

public static class UnityUtils
{
    public static GameObject FindGameObject(this Scene scene, string name)
    {
        var rootGameObjects = scene.GetRootGameObjects();
        var first = name.IndexOf('/');
        if (first == -1)
        {
            return rootGameObjects.FirstOrDefault(x => x.name == name);
        }
        else
        {
            var rootName = name.Substring(0, first);
            var root = rootGameObjects.FirstOrDefault(x => x.name == rootName);
            if (root == null)
            {
                return null;
            }
            return root.transform.Find(name.Substring(first + 1)).gameObject;
        }
    }
    
    public static GameObject CopyGameObject(this GameObject gameObject, Transform parent = null, string name = null)
    {
        var copy = Object.Instantiate(gameObject, parent != null ? parent : gameObject.transform.parent, true);
        if (name != null)
        {
            copy.name = name;
        }

        copy.transform.localScale = gameObject.transform.localScale;
        return copy;
    }
    
    public static T CopyGameObject<T>(this T component, Transform parent = null, string name = null) where T : Component
    {
        var copy = CopyGameObject(component.gameObject, parent, name);

        return copy.GetComponent<T>();
    }
    
    public static void Move(this Transform transform, Vector3 position)
    {
        transform.position += position;
    }
    
    public static void MoveLocal(this Transform transform, Vector3 position)
    {
        transform.localPosition += position;
    }

    public static Sprite ToSprite(this Texture2D texture2D)
    {
        var sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        return sprite;
    }
}