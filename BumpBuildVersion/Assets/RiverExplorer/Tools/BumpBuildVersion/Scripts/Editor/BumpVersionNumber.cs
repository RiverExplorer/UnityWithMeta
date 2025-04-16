/**
 * Project: Phoenix
 * Time-stamp: <2025-03-24 14:02:35 doug>
 *
 * @file BumpVersionNumber.cs
 * @author Douglas Mark Royer
 * @date 24-FEB-2025
 *
 * @Copyright(C) 2025 by Douglas Mark Royer (A.K.A. RiverExplorer)
 *
 * Licensed under the MIT License. See LICENSE file
 * or https://opensource.org/licenses/MIT for details.
 *
 * RiverExplorer is a trademark of Douglas Mark Royer
 *
 * Unless otherwise specified, all of this code is original
 * code by the author.
 */

using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace RiverExplorer.App
{

    /**
     * @class BumpVersionNumber
     * This simple class just bumps the build version number of the game with the time stamp of the build.
     * It also bumps the Android build code if the platform is Android.
     */
    public class BumpVersionNumber
        : IPreprocessBuildWithReport
    {
        Int32 IOrderedCallback.callbackOrder => 0;

        /**
         * The hook we use to bump the version number.
         * 
         * @param report The build report. Which we do not use, but it is supplied by Unity.
         */
        void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
        {
          
            UpdateVersion();

            return;
        }

        /**
         * Get the current version number from player settings and bump it.
         * If an Android build, bump that version number as well.
         */
        private void UpdateVersion()
        {
            string[] Parts = PlayerSettings.bundleVersion.Split('.');

            string Date = DateTime.UtcNow.ToString("u");

            int Major = 0;
            int Minor = 0;

            Date = Date.Replace("-", "");
            Date = Date.Replace(" ", "T");
            Date = Date.Replace(":", "");

#if PLATFORM_ANDROID
            int AndroidBuildCode = PlayerSettings.Android.bundleVersionCode + 1;
            PlayerSettings.Android.bundleVersionCode = AndroidBuildCode;
#endif
            if (Parts.Length == 1) {

                // Has never been build before and no version at all was set.
                //
                Major = int.Parse(Parts[0]) + 1;
                Minor = 0; ;

                PlayerSettings.bundleVersion = string.Format("0.{0}.{1}", Minor, Date);

            } else if (Parts.Length > 1) {

                Major = int.Parse(Parts[0]);
                Minor = int.Parse(Parts[1]);

                //Minor++;      // Uncomment this line to bump the minor version number as well.

                // Create the new version string and set it in player settings.
                //
                PlayerSettings.bundleVersion = string.Format("{0}.{1}.{2}", Parts[0], Minor, Date);
            }

            // Show the results in the  logs.
            //
#if PLATFORM_ANDROID
            Debug.Log("Building: " + Application.productName + " Version:" + PlayerSettings.bundleVersion + " / " + AndroidBuildCode);

#else
            Debug.Log("Building: " + Application.productName + " Version:" + PlayerSettings.bundleVersion);
#endif

            return;
        }
    }
}
