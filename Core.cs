using MelonLoader;
using HarmonyLib;
using UnityEngine;
using Il2CppQuantum_Game;
using Il2CppList = Il2CppSystem.Collections.Generic.List<Il2CppQuantum.EntityRef>;
using Il2CppQuantum;
using Il2CppQuantum.Core;
using Il2CppPhoton.Deterministic;
using Il2CppView_Humanoid;
using Il2CppQuantum_Systems;
using Il2CppQuantum.Prototypes;
using System.Security;
using Il2CppQuantum_Weapons;
using System.Runtime.CompilerServices;
using Il2CppQuantum_Core;
using System.Configuration;
using System.Net.Http.Headers;
using Unity.Collections;
using Il2CppSystem.Threading;
using Il2CppQuantum_HoverBike;
using Il2CppView_Entities;
using Il2CppQuantum_HoverBikeShared;

[assembly: MelonInfo(typeof(KingsRansom.Core), "KingsRansom", "1.0.0", "RosePT-10", null)]
[assembly: MelonGame("Videocult", "Airframe")]

namespace KingsRansom
{
    public class Core : MelonMod
    {
        internal static MelonLogger.Instance Log => Melon<Core>.Instance.LoggerInstance;
        internal static KingsRansom.Core Inst => Melon<Core>.Instance;

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            Log.Msg("Balanced, As It Should Be.");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            
            // -- Ammo changes --
            // Change magazine size (and damage for SMG)
            var smg = WeaponStats.stats[(int)EquipmentID.SMG];
            smg.gunStats.magasineSize = 200;
            smg.gunStats.startAmmo = 200;
            smg.gunStats.damage.machineDamage = 3;
            smg.gunStats.damage.organicDamage = 3;
            WeaponStats.stats[(int)EquipmentID.SMG] = smg;

            var minigun = WeaponStats.stats[(int)EquipmentID.Minigun];
            minigun.gunStats.magasineSize = 560;
            minigun.gunStats.startAmmo = 560;
            WeaponStats.stats[(int)EquipmentID.Minigun] = minigun;

            var shotgun = WeaponStats.stats[(int)EquipmentID.Shotgun];
            shotgun.gunStats.startAmmo = 8;
            shotgun.gunStats.pickupAmmo = 6;
            shotgun.gunStats.fireDelay = 30;
            WeaponStats.stats[(int)EquipmentID.Shotgun] = shotgun;

            var blaster = WeaponStats.stats[(int)EquipmentID.Blaster];
            blaster.gunStats.magasineSize = 6;
            blaster.gunStats.startAmmo = 24;
            blaster.gunStats.pickupAmmo = 18;
            WeaponStats.stats[(int)EquipmentID.Blaster] = blaster;

            var rebar = WeaponStats.stats[(int)EquipmentID.RebarGun];
            rebar.gunStats.startAmmo = 16;
            WeaponStats.stats[(int)EquipmentID.RebarGun] = rebar;
            
            var revolver = WeaponStats.stats[(int)EquipmentID.Revolver];
            revolver.gunStats.startAmmo = 18;
            WeaponStats.stats[(int)EquipmentID.Revolver] = revolver;

            var plasma = WeaponStats.stats[(int)EquipmentID.PlasmaPistol];
            plasma.gunStats.startAmmo = 100;
            WeaponStats.stats[(int)EquipmentID.PlasmaPistol] = plasma;
            
            var kalashnikov = WeaponStats.stats[(int)EquipmentID.Kalashnikov];
            kalashnikov.gunStats.startAmmo = 108;
            WeaponStats.stats[(int)EquipmentID.Kalashnikov] = kalashnikov;
            
            
            // Change grenade ammo
            var cherry = WeaponStats.stats[(int)EquipmentID.CherryBomb];
            cherry.secondaryStats.startAmount = 6;
            //cherry.secondaryStats.onFootStartAmount = 20;
            //cherry.secondaryStats.throwAnimationSpeed = 0.5;
            WeaponStats.stats[(int)EquipmentID.CherryBomb] = cherry;

            var molotov = WeaponStats.stats[(int)EquipmentID.Molotov];
            molotov.secondaryStats.startAmount = 2;
            //molotov.secondaryStats.onFootStartAmount = 20;
            //molotov.secondaryStats.throwAnimationSpeed = 0.5;
            WeaponStats.stats[(int)EquipmentID.Molotov] = molotov;

            //var brick = WeaponStats.stats[(int)EquipmentID.Brick];
            //brick.secondaryStats.throwAnimationSpeed = 0.5;
            //WeaponStats.stats[(int)EquipmentID.Brick] = brick;

            //var shuriken = WeaponStats.stats[(int)EquipmentID.Shuriken];
            //shuriken.secondaryStats.throwAnimationSpeed = 1;
            //WeaponStats.stats[(int)EquipmentID.Shuriken] = shuriken;
            
            var caltrops = WeaponStats.stats[(int)EquipmentID.Caltrops];
            caltrops.secondaryStats.startAmount = 60;
            //caltrops.secondaryStats.throwAnimationSpeed = 0.5;
            WeaponStats.stats[(int)EquipmentID.Caltrops] = caltrops;

            //var grenade = WeaponStats.stats[(int)EquipmentID.FragGrenade];
            //grenade.secondaryStats.throwAnimationSpeed = 0.5;
            //WeaponStats.stats[(int)EquipmentID.FragGrenade] = grenade;

            // Change melee damage
            var pipe = WeaponStats.stats[(int)EquipmentID.Pipe];
            pipe.meleeStats.damage.machineDamage = 25;
            pipe.meleeStats.damage.organicDamage = 25;
            WeaponStats.stats[(int)EquipmentID.Pipe] = pipe;

            var chainsaw = WeaponStats.stats[(int)EquipmentID.Chainsaw];
            chainsaw.meleeStats.damage.stunFac = 0;
            WeaponStats.stats[(int)EquipmentID.Chainsaw] = chainsaw;

            var chain = WeaponStats.stats[(int)EquipmentID.Chain];
            chain.onlyDamageWeaponStats.damageB.machineDamage = 6;
            chain.onlyDamageWeaponStats.damageB.organicDamage = 6;
            chain.onlyDamageWeaponStats.damage.machineDamage = 11;
            chain.onlyDamageWeaponStats.damage.organicDamage = 11;
            chain.meleeStats.damage.stunFac = 1;
            WeaponStats.stats[(int)EquipmentID.Chain] = chain;
        }

        // Remove green money checkpoint rings
        [HarmonyPatch(typeof(CheckPointSystem), "SpawnCheckpoint")]
        private class CheckPointSystem__SpawnCheckpoint_Patch
        {
            public static bool Prefix(CheckPointSystem __instance, ref CheckpointType type)
            {
                // yellow rings still allowed
                if (type == CheckpointType.Arena || type == CheckpointType.Race)
                {
                    return false;
                }
                else
                {
                    type = CheckpointType.Arena;
                    return true;
                }
            }
        }

        /*
        public static Dictionary<EntityRef, EquipmentID> ammo_queue = new Dictionary<EntityRef, EquipmentID>();
        public static Dictionary<EntityRef, EquipmentID> ammo_queue_failed = new Dictionary<EntityRef, EquipmentID>();
        [HarmonyPatch(typeof(Frame.FrameEvents), nameof(Frame.FrameEvents.HumanoidGrabEquipment))]
        private partial class FrameEvents__HumanoidGrabEquipment_Patch
        {   
            public static void Postfix(EquipmentID eqId, EntityRef equipment)
            {
                ammo_queue.Add(equipment, eqId);
            }
        }   
        */
        /*
        // Prevent cherry bomb and molotov ammo from changing
        [HarmonyPatch(typeof(RaceGameStateExtensions), nameof(RaceGameStateExtensions.OnFootSecondaryStartingAmounts))]
        private class RaceGameStateExtensions_OnFootSecondaryStartingAmounts_Patch
        {
            public static void Postfix(ref bool __result)
            {   
                __result = false;
            }
        }   
        */
       
       /*
       [HarmonyPatch(typeof(ShopSystem), nameof(ShopSystem.GetRandomItemForPickupSpawn))]
        private class ShopSystem__GetRandomItemForPickupSpawn_Patch
        {
            public static void Postfix()
            {    
                //price = price;
                //equipmentID = equipmentID;
                //pickupType = pickupType;
                //ref int price, ref EquipmentID equipmentID, ref PickupType pickupType
            }
        }
        */
        
       
        // fetch currently exisiting bikes when a bike respawns
        public static Dictionary<EntityRef, HoverBike_View> bike_model_dictionary = new Dictionary<EntityRef, HoverBike_View>();
        public static List<EntityRef> new_bikes = new List<EntityRef>();
        public static int wait_for_bike_model_fetch = 0;
        [HarmonyPatch(typeof(BikeRespawnSystem), nameof(BikeRespawnSystem.SpawnBike))]
        private class BikeRespawnSystem__SpawnBike_Patch
        {
            public static void Postfix(EntityRef __result)
            {    
                new_bikes.Add(__result);
                wait_for_bike_model_fetch = 20;
            }
        }
        
        
        static bool is_on_foot = false;
        public static Dictionary<EntityRef, int> last_boosts = new Dictionary<EntityRef, int>();
        static Il2CppSystem.Collections.Generic.List<EntityRef> ammo_list = new Il2CppSystem.Collections.Generic.List<EntityRef>();
        [HarmonyPatch(typeof(FrameContext), "OnFrameSimulationBegin")]
        private class Simulate
        {
            public static unsafe void Postfix(FrameBase f)
            {     
                Frame ff = f.Cast<Frame>();

                Il2CppList refs = new();
                f.GetAllEntityRefs(refs);

                foreach (EntityRef eref in refs)
                {
                    // increase beamcutter overheat speed
                    if (f.Has<Laser>(eref)) {
                        Laser beam = f.Get<Laser>(eref);
                        beam.overheatSpeed = 0.018F.ToFP(); 
                        f.Set(eref, beam);
                    }

                    // grenade testing
                    if (f.Has<Grenade>(eref)) {
                        Grenade gre = f.Get<Grenade>(eref);
                        if (gre.equipmentID == EquipmentID.CherryBomb && gre.bouncePos == FPVector3.Zero && gre.fuse == 1)
                        {
                            gre.fuse = 40;
                        }
                        if (gre.equipmentID == EquipmentID.FragGrenade && gre.bouncePos == FPVector3.Zero && gre.fuse == 1)
                        {
                            gre.fuse = 50;
                        }
                        f.Set(eref, gre);
                    }

                    // reduce chainsaw total fuel
                    //if (f.Has<Chainsaw>(eref)) {
                    //    Chainsaw chainsaw = f.Get<Chainsaw>(eref);
                    //    if (chainsaw.fuel > 0.51)
                    //    {
                    //        chainsaw.fuel = 0.5;
                    //    }
                    //    f.Set(eref, chainsaw);
                    //}
                    //Log.Msg(wait_for_bike_model_fetch);
                    
                    if (f.Has<HoverBike>(eref)) {
                        if (wait_for_bike_model_fetch > 1)
                        {
                            wait_for_bike_model_fetch--;
                        }
                        else if (wait_for_bike_model_fetch == 1)
                        {
                            Log.Msg(eref);
                            foreach (var c in GameObject.FindObjectsOfType<HoverBike_View>()) {
                                if (c.name == eref.ToString())
                                {   
                                    HoverBike_View bikemodel = c.GetComponent<HoverBike_View>();
                                    bike_model_dictionary.Clear();
                                    bike_model_dictionary.Add(eref, bikemodel);
                                    continue;
                                }
                            }
                            wait_for_bike_model_fetch--;
                        }

                        
                        if (bike_model_dictionary.ContainsKey(eref)) {
                            for (int i = 0; i < bike_model_dictionary.Count; i++)
                            {   
                                HoverBike* bike = f.GetPointer<HoverBike>(eref);
                                HoverBike_View model = bike_model_dictionary[eref];

                                bike->malfunctions = 0;

                                //Log.Msg($"boosts: {bike->boosts}");
                            
                                if (model && model.model == HoverbikeModel.Light && !new_bikes.Contains(eref))
                                {
                                    /*
                                    if (last_boosts.ContainsKey(eref) && last_boosts[eref] < bike->boosts) {
                                        bike->boosts++;
                                        last_boosts[eref] = bike->boosts;
                                    }
                                    else if (last_boosts.ContainsKey(eref)) {
                                        last_boosts[eref] = bike->boosts;
                                    }
                                    else {
                                        last_boosts.Add(eref, bike->boosts);
                                    }
                                    */
                                }
                                //else {new_bikes.Remove(eref); last_boosts.Clear();}
                            }
                        }
                    }
                    
                }
                
                // -- Determine if currently in an on foot arena --
                try
            {
                if (ff.RuntimeConfig.Map.Id.ToString() != "[045D994E46892D5A]")
            {
                ArenaType arena_type = ff.GetSingleton<RaceGameState>().currArenaType;
                RaceGameStateMode arena_mode = ff.GetSingleton<RaceGameState>().mode;
                //Log.Msg("current mod is: " + arena_mode);
                if (arena_mode == RaceGameStateMode.Arena)
                {
                    if (arena_type == ArenaType.OnFoot) {
                        is_on_foot = true;
                    }
                    else { is_on_foot = false; }
                }
                else { is_on_foot = false; }
            }
            }
                catch {}
                
                /*
                // -- Change ammo of weapons as they are picked up --
                if (ammo_queue.Count > 0)
                {
                    foreach (KeyValuePair<Il2CppQuantum.EntityRef, Il2CppQuantum.EquipmentID> pair in ammo_queue)
                    {
                        if (f.Has<Gun>(pair.Key))
                    {   
                        Gun pewpew = f.Get<Gun>(pair.Key);
                        switch (pair.Value)
                        {
                            case EquipmentID.Minigun:
                                if (pewpew.magasine == 280 && pewpew.ammo == 560) {
                                    pewpew.magasine = 560;
                                    pewpew.ammo = 0;
                                } 
                                else {ammo_queue_failed.Add(pair.Key, pair.Value);
                                } break;
                            case EquipmentID.RebarGun:
                                if (pewpew.magasine == 4 && pewpew.ammo == 24) {
                                    pewpew.magasine = 4;
                                    pewpew.ammo = 16;
                                } 
                                else {ammo_queue_failed.Add(pair.Key, pair.Value);
                                } break;
                            case EquipmentID.Blaster:
                                if (pewpew.magasine == 12 && pewpew.ammo == 24) {
                                    pewpew.magasine = 12;
                                    pewpew.ammo = 24;
                                } 
                                else {ammo_queue_failed.Add(pair.Key, pair.Value);
                                } break;
                            case EquipmentID.SMG:
                                if (pewpew.magasine == 100 && pewpew.ammo == 300) {
                                    pewpew.magasine = 200;      
                                    pewpew.ammo = 0;
                                }
                                else { ammo_queue_failed.Add(pair.Key, pair.Value);
                                } break;
                            case EquipmentID.Shotgun:
                                if (pewpew.magasine == 2 && pewpew.ammo == 8) {
                                    //pewpew.magasine = 2;
                                    //pewpew.ammo = 6;
                                } 
                                else {ammo_queue_failed.Add(pair.Key, pair.Value);
                                } break;
                            case EquipmentID.Revolver:
                                if (pewpew.magasine == 6 && pewpew.ammo == 18) {
                                    pewpew.magasine = 6;
                                    pewpew.ammo = 12;
                                } 
                                else {ammo_queue_failed.Add(pair.Key, pair.Value);
                                } break;
                            case EquipmentID.PlasmaPistol:
                                if (pewpew.magasine == 100 && pewpew.ammo == 100) {
                                    pewpew.magasine = 100;
                                    pewpew.ammo = 0;
                                } 
                                else {ammo_queue_failed.Add(pair.Key, pair.Value);
                                } break;
                            case EquipmentID.Kalashnikov:
                                if (pewpew.magasine == 36 && pewpew.ammo == 144) {
                                    pewpew.magasine = 36;
                                    pewpew.ammo = 72;
                                } 
                                else { ammo_queue_failed.Add(pair.Key, pair.Value);  
                                } break;
                            
                            default:
                                break;
                        }

                        f.Set(pair.Key, pewpew);
                    }
                    }
                }
                ammo_queue.Clear();
                //Log.Msg("Ammo queue count: " + ammo_queue.Count);
                //Log.Msg("Failed queue count: " + ammo_queue_failed.Count);
                
                if (ammo_queue_failed.Count > 0) {
                //Log.Msg("detected");
                foreach (KeyValuePair<Il2CppQuantum.EntityRef, Il2CppQuantum.EquipmentID> pair in ammo_queue_failed)
                {
                    ammo_queue.Add(pair.Key, pair.Value);
                } }
                ammo_queue_failed.Clear();
                */
                /*
                Il2CppSystem.Collections.Generic.List<EntityRef> all_Erefs = new Il2CppSystem.Collections.Generic.List<EntityRef>();
                f.GetAllEntityRefs(all_Erefs);
                
                if (all_Erefs.Equals(ammo_list) && ammo_list != null)
                {
                    Il2CppSystem.Collections.Generic.List<EntityRef> all_Erefs_trimmed = all_Erefs;
                    foreach (EntityRef eref in ammo_list)
                    {
                        all_Erefs_trimmed.Remove(eref);
                    }
                    foreach (EntityRef eref in all_Erefs)
                    {
                        if (f.Has<Gun>(eref))
                        {
                            Log.Msg("detected");
                        }
                    }
                    
                }
                ammo_list.Clear();
                foreach (EntityRef eref in all_Erefs)
                {
                    ammo_list.Add(eref);
                }
                */
                /*
                // Killstreak visualizer
                Il2CppSystem.Collections.Generic.List<EntityRef> all_Erefs = new Il2CppSystem.Collections.Generic.List<EntityRef>();
                f.GetAllEntityRefs(all_Erefs);
                
                foreach (EntityRef eref in all_Erefs)
                {
                    if (f.Has<ParticipatingPlayer>(eref))
                    {
                        ParticipatingPlayer parplayer = f.Get<ParticipatingPlayer>(eref);
                        if (parplayer.streak >= 0)
                        {
                            GameObject collider_vis = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            Transform collider_vis_trans = collider_vis.GetComponent<Transform>();
                            GameObject huma = f.Get<Transform>(eref);
                            collider_vis_trans.position = huma.GetComponent<Transform>().position;
                            //collider_vis_trans.localScale = shape.BoxExtents.ToUnityVector3() * 2 + new UnityEngine.Vector3(0.1f,0.1f,0.1f);
                            //collider_vis_trans.rotation = shape.Rotation.ToUnityQuaternion();
                        }
                    }
                }
                */
            }
        }       

        

        // Change pickup weapon prices
        [HarmonyPatch(typeof(PickupSpawnSystem), nameof(PickupSpawnSystem.SpawnPickup))]
        private class PickupSpawnSystem__SpawnPickup_Patch
        {
            public static bool Prefix(Frame f, ref EquipmentID spawnEquipment, PickupType spawnPickup, ref int price)   
            {   
                //      --- EQUIPMENT PRICES ---
                if (spawnPickup == PickupType.Equipment)
                {
                    switch(spawnEquipment)
                    {
                        // -- MELEE --
                        case EquipmentID.Bat: 
                            price = 0;
                            return true;
                        case EquipmentID.Pipe:
                            price = 50;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Machete:
                            price = 25;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Crowbar:
                            price = 25;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Chain:
                            price = 75;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Sledgehammer:
                            price = 75;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.RiotStick:
                            //price = 75;
                            //if (is_on_foot == true) price = price * 2;
                            return false;
                            //  return true;
                        case EquipmentID.Chainsaw:
                            price = 175;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Laser_Melee:
                            price = 100;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Katana:
                            price = 300;
                            if (is_on_foot == true) price = price * 2;
                            return true;  
                        case EquipmentID.TrafficSign:
                            price = 50;
                            if (is_on_foot == true) price = price * 2;
                            return true;    

                        // -- GUNS --
                        case EquipmentID.Blaster:
                            price = 75;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.SMG:
                            price = 75;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.RebarGun:
                            price = 125;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Shotgun:
                            price = 200;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Revolver:
                            price = 225;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.PlasmaPistol:
                            price = 125;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Kalashnikov:
                            price = 250;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Minigun:
                            price = 300;
                            if (is_on_foot == true) price = price * 2;
                            return true;

                        // -- THROWABLES --
                        case EquipmentID.Brick:
                            price = 125 ;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Molotov:
                            price = 75;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.CherryBomb:
                            price = 50;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Jerrycan:
                            price = 150;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.FragGrenade:
                            price = 200;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Caltrops:
                            //price = 225;
                            if (is_on_foot == true)
                            {
                                //price = price * 2;
                                price = 200;
                                return true;
                            } 
                            return false;
                        case EquipmentID.Shuriken:
                            price = 225;
                            //if (is_on_foot == true) price = price * 2;
                            if (is_on_foot == true) return false;
                            return true;
                        case EquipmentID.Flashbang:
                            // get fucked
                            return false;
                        case EquipmentID.EMPgrenade:
                            // additionally get fucked as well
                            return false;
                        
                        // -- OTHER --
                        case EquipmentID.RiotShield:
                            price = 75;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        
                        default:
                            return true;
                    }
                }
                else if (spawnPickup == PickupType.Boost)
                {   
                    return false;
                }
                else if (spawnPickup == PickupType.Repair)
                {
                    price = 25;
                    if (is_on_foot == true) price = price * 2;  
                    return true;
                }
                else if (spawnPickup == PickupType.Health)
                {
                    price = 25;
                    if (is_on_foot == true) price = price * 2;
                    return true;
                }
                else if (spawnPickup == PickupType.Armor)
                {
                    price = 125;
                    if (is_on_foot == true) price = price * 2;
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }


        // Temp removal of the entire spawn shop so that the mod is at least functional
        [HarmonyPatch(typeof(ShopSystem), "InitiateSpawnShop")]
        private class ShopSystem__InitiateSpawnShop_Patch
        {
            public static bool Prefix()
            {   
                return false;
            }
        }

        // Remove estate door (code entirely taken from Knight-Ragu's TimerMod)
        [HarmonyPatch(typeof(RaceGameModeSystem), nameof(RaceGameModeSystem.ChangeMode))]
        class RaceGameModeSystem__ChangeMode_Patch
        {
            public unsafe static void Postfix(Frame f, ref MapConfig mapConfig, RaceGameState* gameState)
            {
                Il2CppList refs = new();

                f.GetAllEntityRefs(refs);
                foreach (var entity in refs)
                    if (f.Has<PathBlocker>(entity))
                        f.Destroy(entity);
            }
        }   

        /*
        [HarmonyPatch(typeof(HoverBikeSystem.Malfunctions), nameof(HoverBikeSystem.Malfunctions.UpdateMalfunctionStatus))]
        class Malfunctions__UpdateMalfunctionStatus_Patch
        {
            public static bool Prefix()
            {
                return false;
            }
        }  
        */
        /*
        [HarmonyPatch(typeof(HoverbikeStats), nameof(HoverbikeStats.GetStats))]
        class HoverbikeStats__GetStats_Patch
        {
            public static void Prefix(ref HoverbikeModel model)
            {
                //riderBodyType = BodyType.Heavy;
                //ref BodyType riderBodyType    
                model = HoverbikeModel.Heavy;   
            }
        }  
        */
        /*
        [HarmonyPatch(typeof(FrameBase), nameof(FrameBase.Create), [typeof(EntityPrototype)])]
        private class FrameBase__Create_Patch
        {
            public static void Postfix(EntityPrototype prototype, EntityRef __result)
            {   
                if (prototype.name == "PickupPrefabEntityPrototype")
                {
                    PickupPrototype pick;
                    foreach (ComponentPrototype proto in prototype.Container.Components)
                    {
                        //if (proto.ComponentType.Equals(typeof(PickupPrototype))) {

                        pick = proto.TryCast<PickupPrototype>();
                        if (pick != null) {
                        //Log.Msg(Il2CppSystem.Enum.GetName(typeof(EquipmentID), pick.equipmentID));
                        Log.Msg($"{__result} has a price of: {pick.price}");
                    }}
                }
            }
        }
        */

    }
}