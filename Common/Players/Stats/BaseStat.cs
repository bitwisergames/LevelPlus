// Copyright (c) Bitwiser.
// Licensed under the Apache License, Version 2.0.

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace LevelPlus.Common.Players.Stats;

/// The base stat that all stats (or extentions of the player for stats) should inherit from
public abstract class BaseStat
{
  /// The localized name of this stat 
  public LocalizedText Name => Language.GetText(NameKey);

  /// The localized list of bonuses this stat provides
  public LocalizedText Description => Language.GetText(DescriptionKey).WithFormatArgs(DescriptionArgs);

  /// The arugments that go toward the localized description
  protected abstract object[] DescriptionArgs { get; }

  /// The internal handle of the stat
  public abstract string Id { get; }

  /// The name key for localization of this stat
  protected virtual string NameKey => LevelPlus.Instance.LocalizationPrefix + "Stats." + Id + ".DisplayName";

  /// The list of bonuses key for the localization of what this stat provides
  protected virtual string DescriptionKey => LevelPlus.Instance.LocalizationPrefix + "Stats." + Id + ".Tooltip";

  /// The file path of the icon for this stat
  public virtual string IconPath => LevelPlus.Instance.AssetPath + "Textures/UI/Icons/" + Id;

  /// The amount of points invested in this stat
  public virtual int Value { get; protected internal set; }

  /// Load data from the StatPlayer
  public void LoadData(TagCompound tag)
  {
    Value = tag.ContainsKey(Id) ? tag.GetAsInt(Id) : 0;
    LevelPlus.Instance.Logger.DebugFormat($"Load-{Id}: {Value.ToString()}");
    Load(tag);
  }

  /// Save data to the StatPlayer
  public void SaveData(TagCompound tag)
  {
    tag.Set(Id, Value, true);
    LevelPlus.Instance.Logger.DebugFormat($"Save-{Id}: {Value.ToString()}");
    Save(tag);
  }

  /// Called on LoadData, load any additional data here
  public virtual void Load(TagCompound tag)
  {
  }

  /// Called on SaveData, save any additional data here
  public virtual void Save(TagCompound tag)
  {
  }

  /// Called on PostUpdateMiscEffects
  public virtual void ModifyPlayer(Player player)
  {
  }

  /// Called on GetFishingLevel
  public virtual void ModifyFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
  {
  }

  /// Called on PostUpdateRunSpeeds
  public virtual void ModifyRunSpeeds(Player player)
  {
  }

  /// Called on UpdateLifeRegen
  public virtual void ModifyLifeRegen(Player player)
  {
  }

  /// Called on OnConsumeMana
  public virtual void ModifyOnConsumeMana(Player player, Item item, int manaConsumed)
  {
  }

  /// Can the player can consume ammo
  public virtual bool CanConsumeAmmo(Item weapon, Item ammo)
  {
    return true;
  }
}