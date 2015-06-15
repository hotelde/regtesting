using AutoMapper;
using RegTesting.Contracts;
using RegTesting.Contracts.DTO;
using RegTesting.Contracts.Domain;
using RegTesting.Mvc.Models;

namespace RegTesting.Mvc.App_Start
{
	/// <summary>
	/// Init the AutoMapper
	/// </summary>
	public static class AutoMapperInit
	{
		/// <summary>
		/// Create the Mappings for AutoMapper
		/// </summary>
		public static void CreateMappings()
		{
			Mapper.CreateMap<ITestJobManager, TestJobManagerDto>();
			Mapper.CreateMap<TestJob, TestJobDto>();
			Mapper.CreateMap<TestJobDto, TestJobModel>();
			Mapper.CreateMap<WorkItem, Result>();
			Mapper.CreateMap<TestWorkerDto, TestWorkerModel>();

			Mapper.CreateMap<Browser, BrowserDto>().ReverseMap();
			Mapper.CreateMap<BrowserDto, BrowserModel>().ReverseMap();

			Mapper.CreateMap<Language, LanguageDto>().ReverseMap();
			Mapper.CreateMap<LanguageDto, LanguageModel>().ReverseMap();

			Mapper.CreateMap<Tester, TesterDto>().ReverseMap();
			Mapper.CreateMap<TesterDto, TesterModel>().ReverseMap();

			Mapper.CreateMap<Testsystem, TestsystemDto>().ReverseMap();
			Mapper.CreateMap<TestsystemDto, TestsystemModel>().ReverseMap();

			Mapper.CreateMap<Testsuite, TestsuiteDto>().ReverseMap();
			Mapper.CreateMap<TestsuiteDto, TestsuiteModel>().ReverseMap();

			Mapper.CreateMap<Testcase, TestcaseDto>().ReverseMap();
			Mapper.CreateMap<TestcaseDto, TestcaseModel>().ReverseMap();

			Mapper.CreateMap<CreateTestsuiteModel, TestsuiteDto>();

		}
	}
}