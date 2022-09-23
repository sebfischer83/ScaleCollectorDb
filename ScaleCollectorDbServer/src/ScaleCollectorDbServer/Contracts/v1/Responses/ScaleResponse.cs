using Microsoft.AspNetCore.Mvc;

namespace ScaleCollectorDbServer.Contracts.v1.Responses
{
    public class ScaleResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
