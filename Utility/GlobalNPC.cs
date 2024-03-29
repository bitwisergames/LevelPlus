using LevelPlus.Utility;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LevelPlus {
  class levelplusGlobalNPC : GlobalNPC {

    public override bool InstancePerEntity => true;

    public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment) {
      base.ApplyDifficultyAndPlayerScaling(npc, numPlayers, balance, bossAdjustment);
      if (LevelPlusConfig.Instance.ScalingEnabled) {
        float averageLevel = 0;
        foreach (Player player in Main.player) {
          if (player.active) {
            averageLevel += player.GetModPlayer<LevelPlusModPlayer>().level;
          }
        }
        averageLevel /= numPlayers;
        
        float healthMultiplier = 1 + averageLevel * LevelPlusConfig.Instance.ScalingHealth;
        float damageMultiplier = 1 + averageLevel * LevelPlusConfig.Instance.ScalingDamage;
        float defenseAddition = averageLevel * LevelPlusConfig.Instance.ScalingDefense;
        
        npc.lifeMax = (int)Math.Clamp(npc.lifeMax * healthMultiplier, 0, 2147483000);
        npc.damage = (int)Math.Clamp(npc.damage * damageMultiplier, 0, 2147483000);
        npc.defense = (int)Math.Clamp(npc.defense + defenseAddition, 0, 2147483000);
      }
    }


    public override void OnKill(NPC npc) {
      base.OnKill(npc);

      if (npc.type != NPCID.TargetDummy && !npc.SpawnedFromStatue && !npc.friendly && !npc.townNPC && !npc.immortal && !npc.CountsAsACritter) {
        ulong amount;
        if (npc.boss) {
          amount = (ulong)(npc.lifeMax * LevelPlusConfig.Instance.BossXP);
        }
        else {
          amount = (ulong)(npc.lifeMax * LevelPlusConfig.Instance.MobXP);
        }
        
        // If the mob died before being touched by a player, no xp is awarded.
        if (npc.lastInteraction == 255) {
          return;
        }

        // Bestiary increments only when player kills the mob. Double the xp for the first kill.
        int killCount = Main.BestiaryTracker.Kills.GetKillCount(npc);
        if (killCount == 1)
        {
          amount *= 2;
          if (Main.netMode != NetmodeID.Server && ClientConfig.Instance.enablePopups) {
            CombatText.NewText(npc.getRect(), Color.Aqua, Language.GetTextValue("Mods.LevelPlus.Popups.BestiaryUnlock"), true, false);
          }
        }

        if (Main.netMode == NetmodeID.SinglePlayer) {
          Main.LocalPlayer.GetModPlayer<LevelPlusModPlayer>().AddXp(amount);
        }
        else if (Main.netMode == NetmodeID.Server) {
          for (int i = 0; i < npc.playerInteraction.Length; ++i) {
            if (npc.playerInteraction[i]) {
              ModPacket packet = LevelPlus.Instance.GetPacket();
              packet.Write((byte)PacketType.XP);
              packet.Write(amount);
              packet.Send(i);
            }
          }
        }
      }
    }
  }
}

