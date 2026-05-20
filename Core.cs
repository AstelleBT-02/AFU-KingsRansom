using MelonLoader;
using HarmonyLib;
using UnityEngine;
using Il2CppQuantum_Game;
using Il2CppQuantum;
using Il2CppQuantum.Core;
using Il2CppPhoton.Deterministic;
using Il2CppView_Humanoid;
using Il2CppQuantum_Systems;
using Il2CppQuantum.Prototypes;
using System.Security;
using Il2CppQuantum_Weapons;
using System.Runtime.CompilerServices;

[assembly: MelonInfo(typeof(KingsRansom.Core), "KingsRansom", "1.0.0", "RosePT-10", null)]
[assembly: MelonGame("Videocult", "Airframe")]

namespace KingsRansom
{
    public class Core : MelonMod
    {
        internal static MelonLogger.Instance Log => Melon<Core>.Instance.LoggerInstance;
        internal static KingsRansom.Core Inst => Melon<Core>.Instance;

        // Remove green money checkpoint rings
        [HarmonyPatch(typeof(CheckPointSystem), "SpawnCheckpoint")]
        private class CheckPointSystem__SpawnCheckpoint_Patch
        {
            public static bool Prefix(CheckPointSystem __instance, CheckpointType type)
            {
                // yellow rings still allowed
                if (type == CheckpointType.Arena || type == CheckpointType.Race)
                {
                    return false;
                }
                return true;
            }
        }

        /*
        [HarmonyPatch(typeof(GunSystem), "OnAdded")]
        private class GunSystem__OnAdded_Patch
        {
            unsafe public static void Postfix()
            {
                Log.Msg("detected");
                //component->ammo = 100;  
            }
        }
        */
        // Check if currently in an on foot arena
        static bool is_on_foot = false;
        static Il2CppSystem.Collections.Generic.List<EntityRef> ammo_list = new Il2CppSystem.Collections.Generic.List<EntityRef>();
        [HarmonyPatch(typeof(FrameContext), "OnFrameSimulationBegin")]
        private class Simulate
        {
            public static void Postfix(FrameBase f)
            {     
                Frame ff = f.Cast<Frame>();

                // Determine if currently in an on foot arena
                if (ff.RuntimeConfig.gameSetup.gameMode != GameMode.Sandbox)
            {
                ArenaType arena_type = ff.GetSingleton<RaceGameState>().currArenaType;
                RaceGameStateMode arena_mode = ff.GetSingleton<RaceGameState>().mode;
                //Log.Msg("current mod is: " + arena_mode);
                if (arena_mode == RaceGameStateMode.Arena)
                {
                    if (arena_type == ArenaType.OnFoot)
                    {
                        is_on_foot = true;
                    }
                    else
                    {
                        is_on_foot = false;
                    }
                }
                    else
                {
                    is_on_foot = false;
                }
            }
                // Ammo changes
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
        [HarmonyPatch(typeof(PickupSpawnSystem), "SpawnPickup")]
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
                            price = 75;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Chainsaw:
                            price = 200;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Laser_Melee:
                            price = 100;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Katana:
                            price = 500;
                            if (is_on_foot == true) price = price * 2;
                            return true;  
                        case EquipmentID.TrafficSign:
                            price = 50;
                            if (is_on_foot == true) price = price * 2;
                            return true;    

                        // -- GUNS --
                        case EquipmentID.Blaster:
                            price = 100;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.SMG:
                            price = 100;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.RebarGun:
                            price = 125;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Shotgun:
                            price = 225;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Revolver:
                            price = 225;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.PlasmaPistol:
                            price = 150;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Kalashnikov:
                            price = 300;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Minigun:
                            price = 500;
                            if (is_on_foot == true) price = price * 2;
                            return true;

                        // -- THROWABLES --
                        case EquipmentID.Brick:
                            price = 100;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.Molotov:
                            price = 150;
                            if (is_on_foot == true) price = price * 2;
                            return true;
                        case EquipmentID.CherryBomb:
                            price = 200;
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
                            //  if (is_on_foot == true) price = price * 2;
                            return false;
                        case EquipmentID.Shuriken:
                            price = 225;
                            if (is_on_foot == true) price = price * 2;
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
    }
}