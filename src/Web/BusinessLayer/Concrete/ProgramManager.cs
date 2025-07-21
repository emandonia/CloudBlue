using BusinessLayer.Abstract;
using BusinessLayer.ClientMessages;
using Common.Helpers.TextMethots;
using Common.Response;
using DataLayer.Abstract;
using DataLayer.EntityFramework;
using EntityLayer.Dtos.ProgramDtos;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public sealed class ProgramManager : IProgramService
    {
        private readonly IProgramDal _ProgramDal;
        private readonly TextsCheckMethots _textsCheckMethos;

        public ProgramManager(IProgramDal ProgramDal, TextsCheckMethots textsCheckMethos)
        {
            _ProgramDal = ProgramDal;
            _textsCheckMethos = textsCheckMethos;
        }



        public async Task<ResultDto<CreateProgramDtos>> CreateAsync(CreateProgramDtos entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<ListProgramDtos>> DeleteAsync(ListProgramDtos entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<List<ListProgramDtos>>> FindAsync(Expression<Func<Program, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<List<ListProgramDtos>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<ListProgramDtos>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<List<ListProgramDtos>>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Program, bool>>? filter = null, Expression<Func<Program, object>>? orderBy = null, bool ascending = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<UpdateProgramDtos>> UpdateAsync(UpdateProgramDtos entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private async Task<ResultDto<ListProgramDtos>> CheckProgramNameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

