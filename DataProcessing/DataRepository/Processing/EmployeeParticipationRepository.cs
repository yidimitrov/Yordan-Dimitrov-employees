using AutoMapper;
using BusinessModels.Csv;
using DataModels.Csv;
using DataRepository.Interfaces;
using DataRepository.Maps;

namespace DataRepository.Processing
{
    public class EmployeeParticipationRepository : IEmployeeParticipationRepository
    {
        public EmployeeParticipationRepository(IParsable parsable)
        {
            _parsable = parsable;
            _mapper = MapperConfig.InitializeAutoMapper();
        }

        private readonly IParsable _parsable;
        private readonly Mapper _mapper;

        public IEnumerable<EmployeeProjectParicipation> GetEmployeesData(IEnumerable<string> csvrows)
        {
            // Parse csv rows to data models
            var csvModels = _parsable.ParseCsvRows<CsvEmployeeProjectParicipation>(csvrows);
            // Maps data to businees model
            var models = _mapper.Map<IEnumerable<CsvEmployeeProjectParicipation>, IEnumerable<EmployeeProjectParicipation>>(csvModels);

            return models.ToArray();
        }
    }
}
