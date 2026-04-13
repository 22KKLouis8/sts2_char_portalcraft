using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using sts2_char_portalcraft.sts2_char_portalcraftCode.Character;

namespace sts2_char_portalcraft.sts2_char_portalcraftCode.Cards;

[Pool(typeof(sts2_char_portalcraftCardPool))]
public sealed class EustaceHowlOfThunder : sts2_char_portalcraftCard
{
    private int BaseDamage => IsUpgraded ? 18 : 14;
    private int BoostedDamage => IsUpgraded ? 22 : 18;

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(14m, ValueProp.Move),
    };

    public EustaceHowlOfThunder() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        System.ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        bool boosted = EnergyCost.GetResolved() != 2;

        int damage = boosted ? BoostedDamage : BaseDamage;
        await DamageCmd.Attack(damage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        if (boosted)
        {
            var handCards = PileType.Hand.GetPile(Owner).Cards.ToList();
            if (handCards.Count > 0)
            {
                var target = Owner.RunState.Rng.Shuffle.NextItem(handCards);
                target.EnergyCost.AddThisTurn(-1);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
