using UnityEditor;

public class BuildAssetBundles {

    [MenuItem("Asset Bundles/Build AssetBundles")]
    public static void BundleAssets() {
        BuildPipeline.BuildAssetBundles("AssetBundles");
        AssetDatabase.RemoveUnusedAssetBundleNames();
    }
}
