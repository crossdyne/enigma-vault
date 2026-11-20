using AutoMapper;
using EnigmaVault.Secret.Domain.Models;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Application.Mappers.Profiles
{
    internal sealed class VaultItemProfile : Profile
    {
        public VaultItemProfile()
        {
            CreateMap<VaultItem, EncryptedVaultResponse>()
             .ConstructUsing(src => new EncryptedVaultResponse(
                 src.Id.ToString(),
                 src.VaultType.ToString(),                                    
                 src.DateAdded,
                 src.DateUpdated,                                
                 src.DeletedAt,
                 src.IsFavorite,
                 src.IsArchive,
                 src.IsInTrash,
                 src.EncryptedOverview, 
                 src.EncryptedDetails,    
                 new List<string>(src.Tags.Select(x => x.TagId.ToString())),
                 src.IconId.ToString()
             ));
        }
    }
}