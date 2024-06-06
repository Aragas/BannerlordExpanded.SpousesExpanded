﻿using BannerlordExpanded.SpousesExpanded.Polygamy.Behaviors;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;

namespace BannerlordExpanded.SpousesExpanded.Polygamy.Patches
{
    [HarmonyPatchCategory("PolygamyModule")]
    [HarmonyPatch(typeof(MarriageAction), "ApplyInternal")]
    public static class MarriageActionPatch
    {
        [HarmonyPrefix]
        static void Prefix(Hero firstHero, Hero secondHero, bool showNotification)
        {
            if (firstHero == Hero.MainHero || secondHero == Hero.MainHero)
            {
                if (Hero.MainHero.Spouse != null)
                {
                    Campaign.Current.GetCampaignBehavior<PlayerPolygamyBehavior>().AddSpouse(Hero.MainHero.Spouse);
                    Hero.MainHero.Spouse = null;
                }

            }
        }
    }
}
