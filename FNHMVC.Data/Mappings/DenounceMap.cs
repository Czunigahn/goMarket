using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class DenounceMap : ClassMap<Denounce>
    {
        public DenounceMap()
        {
            Id(x => x.DenounceId).GeneratedBy.Identity();
            References(x => x.UserDenouncing).Not.Nullable();
            References(x => x.UserToDenounce).Nullable();
            References(x => x.SaleToDenounce).Nullable();
            Map(x => x.Reason).Not.Nullable();
            Map(x => x.Comment).Not.Nullable();
            Map(x => x.Created).Not.Nullable();
            Map(x => x.TookAction).Not.Nullable();
        }
    }
}