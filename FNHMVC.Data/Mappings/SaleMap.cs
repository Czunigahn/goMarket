using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.Model;
using FluentNHibernate.Mapping;


namespace FNHMVC.Data.Mappings
{
    public class SaleMap : ClassMap<Sale>
    {
        public SaleMap()
        {
            Id(x => x.SaleId).GeneratedBy.Identity();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.Cost).Not.Nullable();
            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.Picture).Nullable();
            Map(x => x.YouTubeLink).Nullable();
            Map(x => x.Created).Not.Nullable();
            Map(x => x.Modified).Nullable();
            Map(x => x.Activated).Not.Nullable();

            Map(x => x.PendingChange).Not.Nullable().Default("0");
            Map(x => x.ActiveForSales).Nullable().Default("1");

            References(x => x.Category).Not.Nullable();
            References(x => x.User).Not.Nullable();


            Map(x => x.Altitude).Nullable();
            Map(x => x.Latitude).Nullable();
            Map(x => x.TookItHome).Nullable();


            Map(x => x.HasDeal).Nullable();
            Map(x => x.DescriptionDeal).Nullable();
            Map(x => x.CostDeal).Nullable();
            Map(x => x.DateFromDeal).Nullable();
            Map(x => x.DateToDeal).Nullable();

            Map(x => x.Spotlight).Nullable();
            Map(x => x.SpotlightApprove).Nullable();

            HasMany(x => x.GoodDeals).LazyLoad().Inverse().Cascade.All();
            HasMany(x => x.UserReviews).LazyLoad().Inverse().Cascade.All();
            HasMany(x => x.SalePendingChange).LazyLoad().Inverse().Cascade.All();
            HasMany(x => x.SaleImages).LazyLoad().Inverse().Cascade.All();
            HasMany(x => x.Cart).LazyLoad().Inverse().Cascade.All();

        }
    }
}
