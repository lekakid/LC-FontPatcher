# LC-FontPatcher

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/c11faea3-9c86-495a-99d4-ed56742ecf66)

Change in-game font to other font asset  
Fix chat input bug on IME input

## Config

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/ca6112ab-38f5-4386-aca8-60bd872bf367)

## default font assets

- 00 default: English('$' fixed)
- 01 kr: Korean (Orbit.ttf / DungGuenMo.ttf)
- 02 jp: Japanese (Pretendard.ttf / Best-Ten.otf)

Each font is tried in the order of its name.

- "Hello": in-game(if used) => 00 default
- "한글": in-game(if used) => 00 default => 01 kr
- "日本語": in-game(if used) => 00 default => 01 kr => 02 jp

## How to create another language's font AssetBundle

### Initialize project

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/6475b6c9-37dc-47b1-a837-2461d505869e)

Create a new project with Unity 2022.3.9

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/f9f126a2-ec7b-4632-b0fc-4e50658ccd16)

Open Window > TextMeshPro > Font Asset Creator

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/f3942a57-be3b-4966-96a6-563a9756a934)

Import TMP Essentials

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

After once initialize project, you don't need to repeat this step

### How to make font asset

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/06739b1e-64ee-4d9c-81e9-4212b5a1895c)

Add ttf/otf font file that want to be import

![image](https://github.com/lekakid/LC-SignalTranslatorAligner/assets/1362809/e573005b-a4b3-4185-8c81-a69993fb5b87)

- Sampling Point Size: 10n (Recommend 90 or 80)
- Padding: n (if samapling point size is 80, set 8)
- Render Mode: RASTER

Generate font asset with your language's character set

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/e509d526-af39-4ab3-b8c2-4420d73b048a)

Save with the name below

- Normal: Default game font
- Transmit: Signal translator's HUD font

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/ed204743-4926-4cd0-a8d3-fe97dcbb8046)

(Recommend) Set line height to 98.1, ascent line 72.
These values are in-game setting.

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/c9f7b3e7-55d9-4d52-8cf8-deef54b69b28)

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/bf54eb54-1540-4449-aa7b-6d68fe1fa536)

Set font's material shader to Distance Field

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/6cc99b7d-081a-4643-999d-b1a24ccacd3d)

Set font's texture filter mode to Bilinear

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/e613af8c-dfff-4775-8a7b-c0c3c8a93304)

Select Normal.asset, Transmit.asset, add to AssetBundle  
Set AssetBundle's name what you want (e.g. "jp", "cn", etc)

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/e99c9ba1-17cb-4565-8b5f-6b5da9041ff1)

Build AssetBundles

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/5e77314d-db60-4370-b2f2-7452f8d78ec6)

Copy AssetBundles file

![image](https://github.com/lekakid/LC-FontPatcher/assets/1362809/66d07c17-3252-404d-ac56-8d00fed1dcdb)

Browse "fonts" folder of mod folder, then paste AssetBundles file.
