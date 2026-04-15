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
/// At start of turn, randomly activate one of the effects that wasn't used last turn:
/// 1. Reduce all hand card costs by 1 this turn (scales with stacks)
/// 2. Gain 2 Strength and 2 Dexterity (scales with stacks)
/// 3. Heal 3 HP (scales with stacks)
/// The same effect cannot activate two turns in a row.
/// </summary>
public sealed class SlausRevolvingWheelPower : sts2_char_portalcraftPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // Track which effect was last used to avoid repeating it
    private int _lastEffect = -1;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;

        Flash();

        // Build list of available effects (all except the last one used)
        var available = new List<int> { 0, 1, 2 };
        if (_lastEffect >= 0)
        {
            available.Remove(_lastEffect);
        }

        int index = player.RunState.Rng.Shuffle.NextInt(available.Count);
        int effect = available[index];
        _lastEffect = effect;

        switch (effect)
        {
            case 0:
                // Reduce all hand card costs by (1 * stacks) this turn
                var handCards = PileType.Hand.GetPile(player).Cards.ToList();
                foreach (var card in handCards)
                {
                    card.EnergyCost.AddThisTurn(-Amount);
                }
                break;

            case 1:
                // Gain (2 * stacks) Strength and (2 * stacks) Dexterity
                await PowerCmd.Apply<StrengthPower>(Owner, 2m * Amount, Owner, null);
                await PowerCmd.Apply<DexterityPower>(Owner, 2m * Amount, Owner, null);
                break;

            case 2:
                // Heal (3 * stacks) HP
                await CreatureCmd.Heal(Owner, 3m * Amount);
                break;
        }
    }
}
