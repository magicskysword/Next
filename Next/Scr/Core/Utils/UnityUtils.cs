using System.Linq;
using FairyGUI;
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
        var sprite = Main.Res.GetSpriteCache(texture2D);
        return sprite;
    }
    
    public static Texture2D ToTexture2D(this RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }
    
    public static bool IsChildOf(this GObject gObject, GObject parent)
    {
        var parentGObject = gObject.parent;
        while (parentGObject != null)
        {
            if (parentGObject == parent)
            {
                return true;
            }
            parentGObject = parentGObject.parent;
        }
        return false;
    }
    
    public static T FindParent<T>(this GObject gObject) where T : GObject
    {
        var parentGObject = gObject.parent;
        while (parentGObject != null)
        {
            if (parentGObject is T parent)
            {
                return parent;
            }
            parentGObject = parentGObject.parent;
        }
        return null;
    }
}