/*
 * Hi, Thomas again. Also stole this from Unity Community Wiki
 * You might be wondering: is it ok for me to
 * be stealing all this low-level code to make the endscreens work?
 * Well, the short answer is that it's part of my learning process, 
 * which is what I'm here to do. The long answer is, I won't stop
 * until Lauren tells me to.
 * 
 * 
 * 
 * Anyways, this script just adds some additional methods to UnityEngine
 * that might be useful later on.
 */

using UnityEngine;

static public class UnityEngineExtensions
{
    /// <summary>
    /// Returns the component of Type type. If one doesn't already exist on the GameObject it will be added.
    /// </summary>
    /// <typeparam name="T">The type of Component to return.</typeparam>
    /// <param name="gameObject">The GameObject this Component is attached to.</param>
    /// <returns>Component</returns>
    static public T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }
}