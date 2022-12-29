using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using SchoolService.Data;

namespace SchoolService.SyncDataServices.Grpc
{
    public class GrpcSchoolService : GrpcShkola.GrpcShkolaBase
    {
        private readonly IShkolaRepo _repository;
        private readonly IMapper _mapper;

        public GrpcSchoolService(IShkolaRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<ShkolaResponse> GetAllShkolas(GetAllRequest request, ServerCallContext context)
        {
            var response = new ShkolaResponse();
            var shkolas = _repository.GetAllShkolas();

            foreach(var plat in shkolas)
            {
                response.Shkola.Add(_mapper.Map<GrpcShkolaModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}