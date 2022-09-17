using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScaleCollectorDbServer.Data;

namespace ScaleCollectorDbServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelKitsController : ControllerBase
    {
        private readonly ILogger<ModelKitsController> _logger;
        private readonly ExtendedScaleDbContext _dbContext;

        public ModelKitsController(ILogger<ModelKitsController> logger, ExtendedScaleDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        
    }
}
