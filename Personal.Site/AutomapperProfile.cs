using AutoMapper;
using Personal.Site.Models.HomeViewModels;
using Personal.Site.Models.SharedViewModels;
using Personal.Site.Settings;

namespace Personal.Site
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<LayoutSettings, LayoutViewModel>();
        }
    }
}
