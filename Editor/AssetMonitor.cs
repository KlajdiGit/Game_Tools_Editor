using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Tools_Week4_Editor.Editor
{
    public delegate void AssetsUpdated();

    internal enum AssetTypes
    {
        MODEL,
        TEXTURE,
        FONT,
        AUDIO,
        EFFECT
    }

    internal class AssetMonitor
    {
        public event AssetsUpdated OnAssetsUpdated;

        private readonly FileSystemWatcher _watcher = null;
        public Dictionary<AssetTypes, List<String>> Assets { get; private set; } = new();
        private readonly string _metaInfo = string.Empty;

        internal AssetMonitor(string asset)
        {
            _metaInfo = Path.Combine(asset, ".mgstats");
            _watcher = new FileSystemWatcher(asset);
            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Filter = "*.mgstats";
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
        }

        public void UpdateAssetDB()
        {
            bool updated = false;
            //AssetTypes assetType = AssetTypes.MODEL;
            using var inStream = new FileStream(_metaInfo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var streamReader = new StreamReader(inStream);
            string[] content = streamReader.ReadToEnd().Split(Environment.NewLine);

            foreach (string line in content)
            {
                if (string.IsNullOrEmpty(line)) continue;
                string[] fields = line.Split(',');
                if (fields[0] == "Source File") continue;
                if(fields[2] == "\"ModelProcessor\"")
                {
                    if (!Assets.ContainsKey(AssetTypes.MODEL)) Assets.Add(AssetTypes.MODEL, new());
                    string assetName = Path.GetFileNameWithoutExtension(fields[1]);
                    if (Assets[AssetTypes.MODEL].Contains(assetName)) continue;
                    Assets[AssetTypes.MODEL].Add(assetName);
                }
            }

            if (updated) OnAssetsUpdated?.Invoke();
        }

        private bool AddAsset(AssetTypes assetType, string assetName)
        {
            if (!Assets.ContainsKey(assetType)) Assets.Add(assetType, new());
            string assetNameWithoutExtension = Path.GetFileNameWithoutExtension(assetName);
            Assets[assetType].Add(assetNameWithoutExtension);
            return true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            UpdateAssetDB();
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            UpdateAssetDB();
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Assets.Clear();
            OnAssetsUpdated?.Invoke();
        }
    }
}
