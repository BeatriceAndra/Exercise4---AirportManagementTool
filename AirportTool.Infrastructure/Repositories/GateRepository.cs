using AirportTool.Application.Interfaces.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repository
{
    public class GateRepository : Repository<Domain.Entities.Gate>, IGateRepository
    {
        private readonly AirportManagementContext _context;
        private readonly IMapper _mapper;

        public GateRepository(AirportManagementContext context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Entities.Gate?> GetByCodeAsync(string gateCode)
        {
            var gate = await _context.Gate.FirstOrDefaultAsync(g => g.Code == gateCode);

            return gate == null ? null : _mapper.Map<Domain.Entities.Gate>(gate);
        }

        public async Task<IEnumerable<Domain.Entities.Gate>> GetAllAsync()
        {
            var gates = await _context.Gate.ToListAsync();

            return _mapper.Map<IEnumerable<Domain.Entities.Gate>>(gates);
        }
    }
}
