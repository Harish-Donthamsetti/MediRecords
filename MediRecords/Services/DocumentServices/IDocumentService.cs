using MediRecords.Dto.DocumentDtos;

namespace MediRecords.Services.DocumentServices;

public interface IDocumentService
{
    Task<DocumentUploadResponseDto> UploadDocumentAsync(DocumentUploadRequestDto request, int userId);
}
