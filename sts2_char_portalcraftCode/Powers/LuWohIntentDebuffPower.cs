using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace sts2_char_portalcraft.sts2_char_portalcraftCode.Powers;

/// <summary>
/// Player buff applied by Lu Woh when cost is not 2.
/// For the next 3 enemy turns, any enemy that intends to attack
/// receives -4 Strength for that turn.
/// Amount = turns remaining.
/// </summary>
public sealed class LuWohIntentDebuffPower : sts2_char_portalcraftPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private const int StrengthReduction = 4;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if (side != CombatSide.Enemy) return;

        bool flashed = false;
        foreach (Creature enemy in combatState.HittableEnemies)
        {
            if (enemy.Monster?.IntendsToAttack ?? false)
            {
                if (!flashed)
                {
                    Flash();
                    flashed = true;
                }

                await PowerCmd.Apply<LuWohStrengthPower>(enemy, StrengthReduction, Owner, null);
            }
        }

        await PowerCmd.Decrement(this);
    }
}
