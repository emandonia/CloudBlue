using EntityLayer.Dtos.ProgramDtos;
using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IProgramService : IGenericService<Program, CreateProgramDtos, UpdateProgramDtos,ListProgramDtos>
    {
        // You can add Custom Operations
    }
}
