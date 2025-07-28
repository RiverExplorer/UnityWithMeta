
#if PLATFORM_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

using RiverExplorer.Speech;

namespace RiverExplorer.App
{
    public class AndroidPermissions
        : MonoBehaviour
    {
        static public bool HaveMicPermissions
        {
            get;
            private set;
        } = false; 
        
        static public bool HaveEyeTrackingPermission
        {
            get;
            private set;
        } = false;

        static public bool HaveBodyTrackingPermission
        {
            get;
            private set;
        } = false;

        static public bool HaveFaceTrackingPermission
        {
            get;
            private set;
        } = false;

        //private const string STORAGE_PERMISSION = "android.permission.READ_EXTERNAL_STORAGE"; 
        private const string MicPermission_s = "android.permission.RECORD_AUDIO";
        private const string EyeTrackingPermission_s = "com.oculus.permission.EYE_TRACKING";
        private const string BodyTrackingPermission_s = "com.oculus.permission.BODY_TRACKING";
        private const string FaceTrackingPermission_s = "com.oculus.permission.FACE_TRACKING";

        // Function to be called first (by UI button)
        // For example, click on Avatar to change it from the device gallery
        public void OnBrowseGalleryButtonPress()
        {
            if (Application.platform == RuntimePlatform.Android) {

                if (!CheckMicPermissions()) {
                    Debug.LogWarning("Missing permission to browse device gallery, please grant the permission first");

                    // Your code to show in-game pop-up with the explanation why you need this permission (required for Google Featuring program)
                    // This pop-up should include a button "Grant Access" linked to the function "OnGrantButtonPress" below
                    //
                    OnGrantButtonPress(); // dmr

                    return;
                }
            } else {
                HaveMicPermissions = true;
                HaveEyeTrackingPermission = false;
                HaveBodyTrackingPermission = false;
                HaveFaceTrackingPermission = false;
            }

            // Your code to browse Android Gallery
            //Debug.Log("Browsing Gallery...");
        }

        static public void CheckAndGetPermissions(bool HasEyeTracking, bool HasBodyTracking, bool HasFaceTracking)
        {
            List<string> RequestList = new List<string>();

            string[] Request;

            RequestList.Add(EyeTrackingPermission_s);

            if (HasEyeTracking && !HasEyeTracking) {
                RequestList.Add(EyeTrackingPermission_s);
            }
            if (HasBodyTracking && !HasBodyTracking) {
                RequestList.Add(BodyTrackingPermission_s);
            }
            if (HasFaceTracking && !HasFaceTracking) {
                RequestList.Add(FaceTrackingPermission_s);
            }
            RequestList.Add(MicPermission_s);

            Request = RequestList.ToArray();

            Permission.RequestUserPermissions(Request);

        }
       
        public static bool CheckBodyTrackingPermission()
        {
            HaveBodyTrackingPermission = AndroidPermissionsManager.IsPermissionGranted(BodyTrackingPermission_s);

            return (HaveBodyTrackingPermission);
        }

        public static bool CheckFaceTrackingPermission()
        {
            HaveFaceTrackingPermission = AndroidPermissionsManager.IsPermissionGranted(FaceTrackingPermission_s);

            return (HaveFaceTrackingPermission);
        }

        public static bool CheckMicPermissions()
        {
            //HaveMicPermissions =  AndroidPermissionsManager.IsPermissionGranted(MicPermission_s);
            HaveMicPermissions = Permission.HasUserAuthorizedPermission(Permission.Microphone);

            //#if SAY_FOUND_MIC
            if (Microphone.devices.Length > 0) {

                //TextToSpeech.Announce("Found microphones.");
            }

            foreach (string Mic in Microphone.devices) {
                //TextToSpeech.Announce(Mic);
            }
            return (HaveMicPermissions);
        }

        static public bool GetMicPermission()
        {
            Permission.RequestUserPermission(Permission.Microphone);

            return(CheckMicPermissions());
        }

        public void OnGrantButtonPress()
        {
            AndroidPermissionsManager.RequestPermission(new[] {
            MicPermission_s
        },
            new AndroidPermissionCallback(
                grantedPermission => {
                    // The permission was successfully granted, restart the change avatar routine.
                    //
                    OnBrowseGalleryButtonPress();
                },
                deniedPermission => {
                    // The permission was denied.
                    //
                },
                deniedPermissionAndDontAskAgain => {
                    // The permission was denied, and the user has selected "Don't ask again"
                    // Show in-game pop-up message stating that the user can change permissions in Android Application Settings
                    // if he changes his mind (also required by Google Featuring program)
                    //
                }));
        }
    }
    public class AndroidPermissionCallback
        : AndroidJavaProxy
    {
        private event Action<string> OnPermissionGrantedAction;
        private event Action<string> OnPermissionDeniedAction;
        private event Action<string> OnPermissionDeniedAndDontAskAgainAction;

        public AndroidPermissionCallback(Action<string> onGrantedCallback, Action<string> onDeniedCallback, Action<string> onDeniedAndDontAskAgainCallback)
            : base("com.unity3d.plugin.UnityAndroidPermissions$IPermissionRequestResult2")
        {
            if (onGrantedCallback != null) {
                OnPermissionGrantedAction += onGrantedCallback;
            }
            if (onDeniedCallback != null) {
                OnPermissionDeniedAction += onDeniedCallback;
            }
            if (onDeniedAndDontAskAgainCallback != null) {
                OnPermissionDeniedAndDontAskAgainAction += onDeniedAndDontAskAgainCallback;
            }
        }

        // Handle permission granted
        public virtual void OnPermissionGranted(string permissionName)
        {
            //Debug.Log("Permission " + permissionName + " GRANTED");
            //
            if (OnPermissionGrantedAction != null) {
                OnPermissionGrantedAction(permissionName);
            }
        }

        // Handle permission denied
        public virtual void OnPermissionDenied(string permissionName)
        {
            //Debug.Log("Permission " + permissionName + " DENIED!");
            if (OnPermissionDeniedAction != null) {
                OnPermissionDeniedAction(permissionName);
            }
        }

        // Handle permission denied and 'Don't ask again' selected
        // ReleaseNote: falls back to OnPermissionDenied() if action not registered
        public virtual void OnPermissionDeniedAndDontAskAgain(string permissionName)
        {
            //Debug.Log("Permission " + permissionName + " DENIED and 'Don't ask again' was selected!");
            if (OnPermissionDeniedAndDontAskAgainAction != null) {
                OnPermissionDeniedAndDontAskAgainAction(permissionName);
            } else if (OnPermissionDeniedAction != null) {
                // Fall back to OnPermissionDeniedAction
                OnPermissionDeniedAction(permissionName);
            }
        }
    }

    public class AndroidPermissionsManager
    {
        private static AndroidJavaObject m_Activity;
        private static AndroidJavaObject m_PermissionService;

        private static AndroidJavaObject GetActivity()
        {
            if (m_Activity == null) {
                AndroidJavaClass     unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                m_Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return m_Activity;
        }

        private static AndroidJavaObject GetPermissionsService()
        {
            return m_PermissionService ??
                (m_PermissionService = new AndroidJavaObject("com.unity3d.plugin.UnityAndroidPermissions"));
        }

        public static bool IsPermissionGranted(string permissionName)
        {
            return GetPermissionsService().Call<bool>("IsPermissionGranted", GetActivity(), permissionName);
        }

        public static void RequestPermission(string permissionName, AndroidPermissionCallback callback)
        {
            RequestPermission(new[] { permissionName }, callback);
        }

        public static void RequestPermission(string[] permissionNames, AndroidPermissionCallback callback)
        {
            GetPermissionsService().Call("RequestPermissionAsync", GetActivity(), permissionNames, callback);
        }
    }
}
#endif // PLATFORM_ANDROID
