using System;
using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Foundation.Extensions
{
    //https://github.com/ZigTheHedge/vsmods/blob/ba2e6bb73f68ee4083b88362e92fc54e91c7001b/mods/streamdc/src/ApiExtensions.cs
    public static class ApiExtensions
    {
        public static TConfig LoadOrCreateConfig<TConfig>(this ICoreAPI api, string filename) where TConfig : new()
        {
            try
            {
                var loadedConfig = api.LoadModConfig<TConfig>(filename);
                if (loadedConfig != null)
                {
                    return loadedConfig;
                }
            }
            catch (Exception e)
            {
                api.World.Logger.Error("{0}", "Failed loading file ("+filename+"), error "+e+". Will initialize new one");
            }

            var newConfig = new TConfig();
            api.StoreModConfig(newConfig, filename);
            return newConfig;
        }

        public static string GetWorldId(this ICoreAPI api)
        {
            if (api == null || api.World == null)
                return null;

            return api.World.Seed.ToString();
        }

        /// <summary>
        /// These data files are per world 
        /// </summary>
        public static TData LoadOrCreateDataFile<TData>(this ICoreAPI api, string filename) where TData : new()
        {
            var path = Path.Combine(GamePaths.DataPath, "ModData", GetWorldId(api), filename);
            try
            {
                if (File.Exists(path))
                {
                    var content = File.ReadAllText(path);
                    return JsonUtil.FromString<TData>(content);
                }
            }
            catch (Exception e)
            {
                api.World.Logger.Log(EnumLogType.Error, "Failed loading file ("+path+"), error "+e+". Will initialize new one");
            }
            var newData = new TData();
            SaveDataFile(api, filename, newData);
            return newData;
        }

        /// <summary>
        /// These data files are per world 
        /// </summary>
        public static void SaveDataFile<TData>(this ICoreAPI api, string filename, TData data) where TData : new()
        {
            var path = Path.Combine(GamePaths.DataPath, "ModData", GetWorldId(api), filename);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                var content = JsonUtil.ToString(data);
                File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                api.World.Logger.Log(EnumLogType.Error, "Failed loading file ("+path+"), error "+e+". Will initialize new one");
            }
        }

    }
}