using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnigmaVault.Secret.Application.Common;
using EnigmaVault.Secret.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Features.Folders.Queries.GetAll
{
    internal sealed class GetAllFoldersQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllFoldersQuery, List<FolderResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<FolderResponse>> Handle(GetAllFoldersQuery request, CancellationToken cancellationToken)
            => await _context.Set<Folder>()
                .Where(f => f.UserId == request.UserId)
                    .ProjectTo<FolderResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}