using AutoMapper;
using RegTesting.Contracts;
using RegTesting.Contracts.Domain;
using RegTesting.Contracts.DTO;

namespace RegTesting.Service
{
	/// <summary>
	/// Init the AutoMapper
	/// </summary>
	public static class AutoMapperServerInit
	{
		/// <summary>
		/// Create the Mappings for AutoMapper
		/// </summary>
		public static void CreateMappings()
		{
			Mapper.CreateMap<TestJob, ITestJobManager>();
			Mapper.CreateMap<ITestJobManager, TestJobManagerDto>();
			Mapper.CreateMap<TestJob, TestJobDto>();
			Mapper.CreateMap<ITestWorker, TestWorkerDto>();
			Mapper.CreateMap<WorkItem, Result>();
			Mapper.CreateMap<WorkItem, HistoryResult>();
			Mapper.CreateMap<WorkItem, WorkItemDto>();

			Mapper.CreateMap<HistoryResult, Result>();

			Mapper.CreateMap<Browser, BrowserDto>().ReverseMap();
			Mapper.CreateMap<Language, LanguageDto>().ReverseMap();
			Mapper.CreateMap<Tester, TesterDto>().ReverseMap();
			Mapper.CreateMap<Testcase, TestcaseDto>().ReverseMap();
			Mapper.CreateMap<Testsystem, TestsystemDto>().ReverseMap();
			Mapper.CreateMap<Testsuite, TestsuiteDto>().ReverseMap();



		}
	}
}