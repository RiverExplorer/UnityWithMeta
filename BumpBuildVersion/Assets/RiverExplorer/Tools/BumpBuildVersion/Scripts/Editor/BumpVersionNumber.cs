/**
* @copyright (C) 2023 RiverExplorer® Games LLC, All Rights Reserved.
*
* RiverExplorer is a registered trademark of RiverExplorer Games LLC.
*
* THIS CODE IS NOT OPEN SOURCE AND IS THE PROPERTY OF THE COPYRIGHT
* HOLDER.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
* "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
* LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
* A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
* HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
* SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
* TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
* OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
* OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
* NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
* OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*
* PENALTIES FOR COPYRIGHT INFRINGEMENT INCLUDE CIVIL AND CRIMINAL
* PENALTIES. IN GENERAL, ANYONE FOUND LIABLE FOR CIVIL COPYRIGHT
* INFRINGEMENT MAY BE ORDERED TO PAY EITHER ACTUAL DAMAGES OR "STATUTORY"
* DAMAGES AFFIXED AT NOT LESS THAN $750 AND NOT MORE THAN $30,000 PER
* WORK INFRINGED. FOR "WILLFUL" INFRINGEMENT, A COURT MAY AWARD UP
* TO $150,000 PER WORK INFRINGED. A COURT CAN, IN ITS DISCRETION, ALSO
* ASSESS COSTS AND ATTORNEYS' FEES. FOR DETAILS,
* SEE TITLE 17, UNITED STATES CODE, SECTIONS 504, 505.
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
