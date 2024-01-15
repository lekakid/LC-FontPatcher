# LC-FontPatcher

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/c11faea3-9c86-495a-99d4-ed56742ecf66)

Change in-game font to other font asset  
Fix chat input bug on IME input

## Config

⚠️ Configuration files are generated after launching the game, with the mod installed, at least once.

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/68ca94a5-52a3-4525-bc78-c6ef4c672fed)

## default font assets

- 00 default: English('$' fixed)
- 01 kr: Korean
- 02 jp: Japanese

Each font is tried in the order of its name.

- "Hello": in-game(if used) => 00 default
- "한글": in-game(if used) => 00 default => 01 kr
- "日本語": in-game(if used) => 00 default => 01 kr => 02 jp

## How to create another language's font AssetBundle

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/6475b6c9-37dc-47b1-a837-2461d505869e)

Create a new project with Unity 2022.3.9

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/f9f126a2-ec7b-4632-b0fc-4e50658ccd16)

Open Window > TextMeshPro > Font Asset Creator

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/f3942a57-be3b-4966-96a6-563a9756a934)

Import TMP Essentials

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/06739b1e-64ee-4d9c-81e9-4212b5a1895c)

Add ttf font file that want to be import

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/8b5ee584-ea08-4fa1-845e-020e845f13bc)

Generate font asset with your language's character set  
If you want to use the in-game alphabet font, exclude the alphabet from the character set

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/e509d526-af39-4ab3-b8c2-4420d73b048a)

Save with the name below

- Normal: Default game font
- Transmit: Signal translator's HUD font

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/945a98bc-5cbb-427e-a3e6-03167c6decfd)

Create "Editor" Folder

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/466e8270-6bc7-4a41-99a3-410a3acdd943)

Create script file, then paste below source in file

```cs
// https://docs.unity3d.com/Manual/AssetBundles-Workflow.html
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (Directory.Exists(assetBundleDirectory))
        {
            Directory.Delete(assetBundleDirectory, true);
        }
        Directory.CreateDirectory(assetBundleDirectory);
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
    }
}
```

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/e613af8c-dfff-4775-8a7b-c0c3c8a93304)

Select Normal.asset, Transmit.asset, add to AssetBundle  
Set AssetBundle's name what you want (e.g. "jp", "cn", etc)

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/e99c9ba1-17cb-4565-8b5f-6b5da9041ff1)

Build AssetBundles

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/5e77314d-db60-4370-b2f2-7452f8d78ec6)

Copy AssetBundles file

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/66d07c17-3252-404d-ac56-8d00fed1dcdb)

Browse "fonts" folder of mod folder, then paste AssetBundles file.
