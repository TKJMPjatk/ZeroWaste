using AutoMapper;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public class PhotoMapperHelper : IPhotoMapperHelper
    {
        private readonly IMapper _mapper;
        public PhotoMapperHelper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public IEnumerable<PhotoVM> Map(IEnumerable<Photo> photos)
        {
            var photosMapped = _mapper.Map<IEnumerable<PhotoVM>>(photos);
            return photosMapped;
        }
    }
}
