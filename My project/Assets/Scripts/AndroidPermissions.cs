using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

/// <summary>
/// Handles Android runtime permissions for the Karaoke app
/// </summary>
public class AndroidPermissions : MonoBehaviour
{
    void Start()
    {
        RequestPermissions();
    }
    
    /// <summary>
    /// Request necessary Android permissions
    /// </summary>
    private void RequestPermissions()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        
        // For recording features (if you add them later)
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }
    
    /// <summary>
    /// Check if all required permissions are granted
    /// </summary>
    public bool HasAllPermissions()
    {
#if UNITY_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) &&
               Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead);
#else
        return true;
#endif
    }
}

