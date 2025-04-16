/**
 * Project: Phoenix
 * Time-stamp: <2025-03-24 14:02:35 doug>
 *
 * @file StartUp.cs
 * @author Douglas Mark Royer
 * @date 24-FEB-2025
 *
 * @Copyright(C) 2025 by Douglas Mark Royer (A.K.A. RiverExplorer)
 *
 * Licensed under the MIT License. See LICENSE file
 * or https://opensource.org/licenses/MIT for details.
 *
 * RiverExplorer is a trademark of Douglas Mark Royer
 * Unless otherwise specified, all of this code is original
 * code by the author.
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
