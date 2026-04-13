using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using sts2_char_portalcraft.sts2_char_portalcraftCode.Cards;

namespace sts2_char_portalcraft.sts2_char_portalcraftCode.Powers;

/// <summary>
/// Temporary Strength debuff applied by Lu Woh, Light Personified.
/// All enemies lose Strength until end of their turn.
/// </summary>
public class LuWohStrengthPower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<LuWohLightPersonified>();

    protected override bool IsPositive => false;
}
