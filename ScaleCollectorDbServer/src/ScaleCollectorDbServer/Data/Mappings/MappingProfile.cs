using AutoMapper;
using ScaleCollectorDbServer.Contracts.v1.Requests;
using ScaleCollectorDbServer.Contracts.v1.Responses;
using ScaleCollectorDbServer.Data.Entities;

namespace ScaleCollectorDbServer.Data.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ModelKit, ModelKitResponse>()
                .ForMember(x => x.Reference, a => a.MapFrom(bv => bv.Reference.Select(r => r.ReferenceId)))
                .ForMember(x => x.ReferenceOf, a => a.MapFrom(bv => bv.ReferenceOf.Select(r => r.ReferenceId)));

            CreateMap<ModelKitPostRequest, ModelKit>();
            CreateMap<ModelKitPutRequest, ModelKit>();


            CreateMap<Scale, ScaleResponse>();
        }
    }
}
