// One-shot project setup. Run "Tools/Project Setup (One-shot)" once, then delete this file.
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public static class ProjectSetup
{
    const string CompanyName = "dustjd";
    const string ProductName = "조선귀문디펜스";
    const string AppId = "com.dustjd.joseongwimundefense";

    static readonly string[] Packages =
    {
        "com.unity.ugui",                       // uGUI + TextMeshPro
        "com.unity.render-pipelines.universal",
        "com.unity.2d.sprite",
        "com.unity.inputsystem",
        "com.unity.addressables",
        "com.unity.test-framework",
    };

    static AddAndRemoveRequest _request;

    [MenuItem("Tools/Project Setup (One-shot)")]
    static void Run()
    {
        var android = NamedBuildTarget.Android;

        PlayerSettings.companyName = CompanyName;
        PlayerSettings.productName = ProductName;
        PlayerSettings.SetApplicationIdentifier(android, AppId);
        PlayerSettings.bundleVersion = "0.1.0";
        PlayerSettings.Android.bundleVersionCode = 1;

        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.SetScriptingBackend(android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        AssetDatabase.SaveAssets();
        Debug.Log("[ProjectSetup] Player settings applied. Installing packages...");

        _request = Client.AddAndRemove(Packages);
        EditorApplication.update += Poll;
    }

    static void Poll()
    {
        if (!_request.IsCompleted) return;
        EditorApplication.update -= Poll;

        if (_request.Status == StatusCode.Success)
            Debug.Log("[ProjectSetup] Packages installed. Done.");
        else
            Debug.LogError($"[ProjectSetup] Package install failed: {_request.Error.message}");
    }
}
