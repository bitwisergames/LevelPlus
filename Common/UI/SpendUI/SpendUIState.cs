// Copyright (c) Bitwiser.
// Licensed under the Apache License, Version 2.0.

using LevelPlus.Common.Configs;
using Microsoft.Xna.Framework;
using Terraria.UI;

namespace LevelPlus.Common.UI.SpendUI;

public class SpendUIState : UIState
{
  private Vector2 placement;
  private SpendUIPanel spendPanel;

  public override void OnInitialize()
  {
    placement = new Vector2(UIConfig.Instance.SpendUILeft, UIConfig.Instance.SpendUITop);

    spendPanel = new SpendUIPanel();
    spendPanel.Left.Set(placement.X, 0f);
    spendPanel.Top.Set(placement.Y, 0f);
    spendPanel.SetSnapPoint("Origin", 0, placement);

    Append(spendPanel);
  }

  public override void OnDeactivate()
  {
    spendPanel = null;
  }
}
