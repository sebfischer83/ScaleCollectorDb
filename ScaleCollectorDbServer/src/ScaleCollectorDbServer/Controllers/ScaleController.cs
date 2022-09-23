using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScaleCollectorDbServer.Contracts.v1.Responses;
using ScaleCollectorDbServer.Data;

namespace ScaleCollectorDbServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScaleController : ControllerBase
    {
        private readonly ILogger<ModelKitsController> _logger;
        private readonly ExtendedScaleDbContext _dbContext;
        private readonly IMapper _mapper;

        public ScaleController(ILogger<ModelKitsController> logger, ExtendedScaleDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ScaleResponse>> GetAll()
        {
            var x = _mapper.ProjectTo<ScaleResponse>(_dbContext._context.Scales.AsNoTracking(), null).AsEnumerable();

            return Ok(x);
        }
    }
}
