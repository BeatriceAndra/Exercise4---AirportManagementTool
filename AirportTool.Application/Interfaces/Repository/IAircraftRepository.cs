using AirportTool.Application.Interfaces.Repositories;
using AirportTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Interfaces.Repository
{
    public interface IAircraftRepository : IRepository<Aircraft>
    {
    }
}
