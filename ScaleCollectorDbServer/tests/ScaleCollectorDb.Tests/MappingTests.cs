using AutoMapper;
using ScaleCollectorDb.Tests.Fake;
using ScaleCollectorDbServer.Contracts.v1.Responses;
using ScaleCollectorDbServer.Data.Entities;
using ScaleCollectorDbServer.Data.Mappings;
using Shouldly;

namespace ScaleCollectorDb.Tests
{
    public class MappingTests
    {
        [Fact]
        public void IsProfileValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void ModelKitToModelKitResponseTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            var fakeModelkit = ModelKitFake.GetFake();
            ModelKit modelKit = fakeModelkit.Generate();
            modelKit.Reference = new List<ModelKitReference>();
            modelKit.Reference.Add(new ModelKitReference() { Id = 1, ModelKitId = modelKit.Id, ReferenceId = 1999 });

            var a = mapper.Map<ModelKit, ModelKitResponse>(modelKit);
            a.ShouldNotBeNull();
            a.Title.ShouldBe(modelKit.Title);
            a.ReferenceOf?.Count.ShouldBe(0);
            a.Reference.Count.ShouldBe(1);
            a.Reference[0].ShouldBe(modelKit.Reference.First().Id);
        }
    }
}