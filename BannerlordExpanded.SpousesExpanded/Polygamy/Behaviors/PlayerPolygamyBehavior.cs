﻿using BannerlordExpanded.SpousesExpanded.Utility;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerlordExpanded.SpousesExpanded.Polygamy.Behaviors
{
    internal class PlayerPolygamyBehavior : CampaignBehaviorBase
    {
        Dictionary<Hero, MBList<Hero>> _secondarySpouses = new Dictionary<Hero, MBList<Hero>>(); // Key is which Main Hero, 
        MBList<Hero> _DEPRECATEDsecondarySpouses = new MBList<Hero>();

        public override void RegisterEvents()
        {
            //CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, RefreshSpouseVisit);
            //CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("BannerlordExpanded.SpousedExpanded_SecondarySpousesDictionary", ref _secondarySpouses);
            dataStore.SyncData("BannerlordExpanded.SpousedExpanded_SecondarySpouses", ref _DEPRECATEDsecondarySpouses);

            if (_DEPRECATEDsecondarySpouses != null && _secondarySpouses.IsEmpty())
            {
                InformationManager.DisplayMessage(new InformationMessage("[BE - Spouses Expanded] Outdated mod save detected!\nConverting mod save to versions v1.2.10 and above..."));
                _secondarySpouses.Add(Hero.MainHero, _DEPRECATEDsecondarySpouses);
            }

        }


        public bool IsSpouse(Hero hero)
        {
            if (Hero.MainHero.Spouse == hero || hero.Spouse == Hero.MainHero)
            {

                return true;
            }
            else
            {
                MBList<Hero> spouses;
                return (_secondarySpouses.TryGetValue(Hero.MainHero, out spouses) && spouses.Contains(hero));
            }
        }

        public void SetPrimarySpouse(Hero hero)
        {
            if (!IsSpouse(hero))
            {
                throw new System.Exception($"[BannerlordExpanded.SpousesExpanded]: Tried setting Hero {hero.Name} as primary spouse even though he/she is not a spouse.");
            }


            if (Hero.MainHero.Spouse != hero)
            {

                MBList<Hero> spouses;
                if (!_secondarySpouses.TryGetValue(Hero.MainHero, out spouses))
                {
                    spouses.Remove(hero);
                    spouses.Add(Hero.MainHero.Spouse);

                    SpousesExpandedUtil.SetHeroSpouse(Hero.MainHero, hero);
                    SpousesExpandedUtil.SetHeroSpouse(hero, Hero.MainHero);
                }


            }
        }

        public MBList<Hero> GetPlayerSpouses()
        {
            MBList<Hero> spouses;
            if (!_secondarySpouses.TryGetValue(Hero.MainHero, out spouses))
            {
                spouses = new MBList<Hero>();
                _secondarySpouses.Add(Hero.MainHero, spouses);
            }
            return spouses;
        }

        public void AddSpouse(Hero hero)
        {

            MBList<Hero> spouses;
            if (!_secondarySpouses.TryGetValue(Hero.MainHero, out spouses))
            {
                spouses = new MBList<Hero>();
                _secondarySpouses.Add(Hero.MainHero, spouses);
            }
            spouses.Add(hero);
        }

        public bool RemoveSpouse(Hero hero)
        {
            MBList<Hero> spouses;
            if (!_secondarySpouses.TryGetValue(Hero.MainHero, out spouses))
            {
                return false;
            }

            if (Hero.MainHero.Spouse == hero)
            {
                if (spouses.Count > 0)
                {
                    SpousesExpandedUtil.SetHeroSpouse(Hero.MainHero, spouses[0]);
                }
                else
                    SpousesExpandedUtil.SetHeroSpouse(Hero.MainHero, null);
                return true;
            }
            else if (spouses.Contains(hero))
            {
                _secondarySpouses.Remove(hero);
                return true;
            }
            else
            {
                // Hero is not a spouse?!
            }

            return false;
        }
    }
}
