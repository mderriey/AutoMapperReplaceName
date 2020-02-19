using AutoMapper;
using Xunit;

namespace AutoMapperReplaceName
{
    public class Source
    {
        public int My_Property { get; set; }
    }

    public class Destination
    {
        public int MyProperty { get; set; }
    }

    public class SourceToDestinationProfile : Profile
    {
        public SourceToDestinationProfile(bool addReplaceName)
        {
            if (addReplaceName)
            {
                ReplaceMemberName("_", string.Empty);
            }

            CreateMap<Source, Destination>();
        }
    }

    public class Tests
    {
        [Fact]
        public void ReplaceInGlobalConfigWhenMapIsCreatedInProfileDoesntWork()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ReplaceMemberName("_", string.Empty);
                cfg.AddProfile(new SourceToDestinationProfile(false));
            });

            // Throws
            //
            // AutoMapper.AutoMapperConfigurationException : 
            // Unmapped members were found. Review the types and members below.
            // Add a custom mapping expression, ignore, add a custom resolver, or modify the source / destination type
            // For no matching constructor, add a no - arg ctor, add optional arguments, or map all of the constructor parameters
            // ==================================================================
            // Source->Destination(Destination member list)
            // AutoMapperReplaceName.Source->AutoMapperReplaceName.Destination(Destination member list)
            //
            // Unmapped properties:
            // MyProperty
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void ReplaceInProfileWhenMapIsCreatedInProfileWorks()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SourceToDestinationProfile(true));
            });

            // Passes
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void ReplaceInGlobalConfigWhenMapIsCreatedInGlobalConfigWorks()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ReplaceMemberName("_", string.Empty);
                cfg.CreateMap<Source, Destination>();
            });

            // Passes
            config.AssertConfigurationIsValid();
        }
    }
}
