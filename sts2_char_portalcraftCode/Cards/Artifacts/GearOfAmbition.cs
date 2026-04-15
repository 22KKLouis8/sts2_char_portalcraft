using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace sts2_char_portalcraft.sts2_char_portalcraftCode.Cards.Artifacts;

public sealed class GearOfAmbition : ArtifactCard
{
    public override ArtifactTier Tier => ArtifactTier.T0_Artifact;

    public GearOfAmbition() : base(ArtifactType.Artifact, TargetType.Self) { }

    protected override Task OnRawPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        return Task.CompletedTask;
    }
}
