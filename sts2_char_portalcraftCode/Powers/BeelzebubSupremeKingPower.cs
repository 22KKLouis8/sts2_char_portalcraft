using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace sts2_char_portalcraft.sts2_char_portalcraftCode.Powers;

/// <summary>
/// Beelzebub power: Whenever an enemy takes damage, it takes +2 damage.
/// Whenever you gain block, you gain +2 block.
/// Amount controls the bonus (default 2).
/// </summary>
public sealed class BeelzebubSupremeKingPower : sts2_char_portalcraftPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // Only apply when the power owner is dealing damage
        if (dealer != Owner) return 0m;
        // Only for powered attacks (card/move damage)
        if (!props.HasFlag(ValueProp.Move) || props.HasFlag(ValueProp.Unpowered)) return 0m;
        return Amount;
    }

    public override decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay)
    {
        // Only apply when the power owner is gaining block
        if (cardSource != null)
        {
            if (cardSource.Owner.Creature != Owner) return 0m;
        }
        else if (Owner != target)
        {
            return 0m;
        }
        // Only for powered block (card/move block)
        if (!props.HasFlag(ValueProp.Move) || props.HasFlag(ValueProp.Unpowered)) return 0m;
        return Amount;
    }
}
