/**
 *@copyright(C) 2023 RiverExplorer? Games LLC, All Rights Reserved.
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
using UnityEngine;
using UnityEditor;
using System.IO;

namespace RiverExplorer.Tools.Editor
{

    [InitializeOnLoad]
    public class StartUp
    {

#if UNITY_EDITOR
        static StartUp()
        {
            Load();

            return;
        }

        static private void Load()
        {
            string AppData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);

            string DataDir = AppData + "/" + PlayerSettings.companyName;

            if (!Directory.Exists(DataDir)) {
                Directory.CreateDirectory(DataDir);
            }
            string DataFile = AppData + "/" + PlayerSettings.companyName + "/" + PlayerSettings.productName;

            if (File.Exists(DataFile)) {

                string AllText = File.ReadAllText(DataFile);

                string[] Lines = AllText.Split('\n');

                // Line 1 is the key store password.
                // Line 2 is the key - store alias password.
                // Line 3 is the key store name.
                //
                string KeyStorePassword = Lines[0].Trim();
                string Password = Lines[1].Trim();
                string KeyStoreName = Lines[2].Trim();

                PlayerSettings.keystorePass = KeyStorePassword;
                PlayerSettings.keyaliasPass = Password;
                PlayerSettings.Android.keystoreName = KeyStoreName;

            } else {
                Debug.Log("---------------------------------------------------------------------------------------------\n"
                + "Can not open: " + DataFile + ", this is not an error, you might not have one (click to see details).\n"
                + "   It is a simple text file (no filename extension). The first line is the key store password.\n"
                + "   The second line is the product password in the key store.\n"
                + "   The third line is the location of the key-store.\n"
                + "   Unity remembers the Alias name, so that is not stored.\n"
                + "   Example contents (no spaces before or after the values):\n"
                + "\n"
                + "  MyKeyStorePassword\n"
                + "  MyAppPassword\n"
                + "  C:\\Users\\MyName\\Location\\To\\Keystore\\file\n"
                + "\n"
                + "---------------------------------------------------------------------------------------------\n");
            }

            return;
        }

        [MenuItem("RiverExplorer/Tools/Editor/LoadKeyStore")]
        static public void LoadFromFile()
        {
            Load();
        }


#endif

    }
}
