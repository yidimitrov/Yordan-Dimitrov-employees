using AutoMapper;
using BusinessModels.Csv;
using DataModels.Csv;

namespace DataRepository.Maps
{
    public class MapperConfig
    {
        public static Mapper InitializeAutoMapper()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CsvEmployeeProjectParicipation, EmployeeProjectParicipation>()
                // Provide mapping logic for NULL value for DateTo property
                .ForMember(dest => dest.DateTo, act => act.MapFrom(src =>
                        src.DateTo ?? DateTime.Now));
            });

            var mapper = new Mapper(config);
            return mapper;
        }

    }
}
