﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        private readonly FileSystemWatcher m_watcher = null;
        public Dictionary<AssetTypes, List<string>> Assets { get; private set; } = new();
        private readonly string m_metaInfo = string.Empty;

        internal AssetMonitor(string asset)
        {
            m_metaInfo = Path.Combine(asset, ".mgstats");
            m_watcher = new FileSystemWatcher(asset);
            m_watcher.Changed += OnChanged;
            m_watcher.Created += OnCreated;
            m_watcher.Deleted += OnDeleted;
            m_watcher.Filter = "*.mgstats";
            m_watcher.IncludeSubdirectories = false;
            m_watcher.EnableRaisingEvents = true;
        }

        private void UpdateAssetDB()
        {
            bool updated = false;
            using var inStream = new FileStream(m_metaInfo, FileMode.Open,
                                                 FileAccess.Read, FileShare.ReadWrite);
            using var streamReader = new StreamReader(inStream);
            string[] content = streamReader.ReadToEnd().Split(Environment.NewLine);
            foreach(string line in content)
            {
                if(string.IsNullOrEmpty(line)) continue;
                string[] fields = line.Split(',');
                if (fields[0] == "Source File") continue;
                if (fields[2] == "\"ModelProcessor\"") ;
                {
                    if (!Assets.ContainsKey(AssetTypes.MODEL)) Assets.Add(AssetTypes.MODEL, new());
                    string assetName = Path.GetFileNameWithoutExtension(fields[1]);
                    if (Assets[AssetTypes.MODEL].Contains(assetName)) continue;
                    Assets[AssetTypes.MODEL].Add(assetName);
                    updated = true;
                }
            }
            if (updated) OnAssetsUpdated?.Invoke();
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
