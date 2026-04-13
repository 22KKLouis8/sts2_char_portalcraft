using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace sts2_char_portalcraft.sts2_char_portalcraftCode.Powers;

/// <summary>
/// Slaus, Revolving Wheel of Fortune power.
/// At start of turn, randomly activate one of the remaining effects:
/// 1. Reduce all hand card costs by 1 this turn
/// 2. Gain 2 Strength and 2 Dexterity
/// 3. Heal 3 HP
/// Each effect can only activate once. The power is removed when all three have been used.
/// </summary>
public sealed class SlausRevolvingWheelPower : sts2_char_portalcraftPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // Track which effects are still available (indices 0, 1, 2)
    private readonly List<int> _remainingEffects = new() { 0, 1, 2 };

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;
        if (_remainingEffects.Count == 0) return;

        Flash();

        int index = player.RunState.Rng.Shuffle.NextInt(_remainingEffects.Count);
        int effect = _remainingEffects[index];
        _remainingEffects.RemoveAt(index);

        switch (effect)
        {
            case 0:
                // Reduce all hand card costs by 1 this turn
                var handCards = PileType.Hand.GetPile(player).Cards.ToList();
                foreach (var card in handCards)
                {
                    card.EnergyCost.AddThisTurn(-1);
                }
                break;

            case 1:
                // Gain 2 Strength and 2 Dexterity
                await PowerCmd.Apply<StrengthPower>(Owner, 2m, Owner, null);
                await PowerCmd.Apply<DexterityPower>(Owner, 2m, Owner, null);
                break;

            case 2:
                // Heal 3 HP
                await CreatureCmd.Heal(Owner, 3m);
                break;
        }

        // Remove the power when all effects have been used
        if (_remainingEffects.Count == 0)
        {
            await PowerCmd.Remove(this);
        }
    }
}
