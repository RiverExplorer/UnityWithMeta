/**
 * Project: Phoenix
 * Time-stamp: <2025-03-24 14:02:35 doug>
 *
 * @file DefaultSceneLoader.cs
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
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class DefaultSceneLoader
{
    static DefaultSceneLoader()
    {
        EditorApplication.update += LoadDefaultScene;
    }

    static void LoadDefaultScene()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;
        EditorApplication.update -= LoadDefaultScene;
        EditorSceneManager.OpenScene("Assets/Scenes/YOURSCENE.unity");
    }
}
