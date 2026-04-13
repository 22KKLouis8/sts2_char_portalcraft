using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using sts2_char_portalcraft.sts2_char_portalcraftCode.Character;
using sts2_char_portalcraft.sts2_char_portalcraftCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace sts2_char_portalcraft.sts2_char_portalcraftCode.Cards;

[Pool(typeof(sts2_char_portalcraftCardPool))]
public abstract class sts2_char_portalcraftCard : CustomCardModel
{
    public override bool CanBeGeneratedInCombat => Rarity != CardRarity.Token && Rarity != CardRarity.Basic;

    //Image size:
    //Normal art: 1000x760
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190
    
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected sts2_char_portalcraftCard(int cost, CardType type, CardRarity rarity, TargetType target)
        : base(cost, type, rarity, target, showInCardLibrary: rarity != CardRarity.Token)
    {
    }

    protected sts2_char_portalcraftCard(int cost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary)
        : base(cost, type, rarity, target, showInCardLibrary: showInCardLibrary)
    {
    }
}
