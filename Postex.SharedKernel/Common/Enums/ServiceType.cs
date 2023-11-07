using System.ComponentModel;

namespace Postex.SharedKernel.Common.Enums
{
    public enum ServiceType
    {
        [Description("جمع آوری و توزیع")] DistributionAndCollectionService = 1,
        [Description("جمع آوری ")] CollectionService = 2,
        [Description("توزیع")] DistributionService = 3
    }
}