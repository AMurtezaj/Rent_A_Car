
namespace API

{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarDto>();
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }


}