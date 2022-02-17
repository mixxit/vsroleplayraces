using Vintagestory.API.Common;
using System;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.API.Client;
using Foundation.Extensions;

namespace vsroleplayraces.src
{
    public class VSRoleplayRacesMod : ModSystem
    {
        private ICoreAPI api;
        public override void StartPre(ICoreAPI api)
        {
            VSRoleplayRacesModConfigFile.Current = api.LoadOrCreateConfig<VSRoleplayRacesModConfigFile>(typeof(VSRoleplayRacesMod).Name + ".json");
            base.StartPre(api);
        }
        public override void Start(ICoreAPI api)
        {
            this.api = api;
            base.Start(api);
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            base.StartClientSide(api);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);
        }
        public override double ExecuteOrder()
        {
            /// Worldgen:
            /// - GenTerra: 0 
            /// - RockStrata: 0.1
            /// - Deposits: 0.2
            /// - Caves: 0.3
            /// - Blocklayers: 0.4
            /// Asset Loading
            /// - Json Overrides loader: 0.05
            /// - Load hardcoded mantle block: 0.1
            /// - Block and Item Loader: 0.2
            /// - Recipes (Smithing, Knapping, Clayforming, Grid recipes, Alloys) Loader: 1
            /// 
            return 1.1;
        }
    }

    public class VSRoleplayRacesModConfigFile
    {
        public static VSRoleplayRacesModConfigFile Current { get; set; }
    }
}
