using MediRecords.Dto.DocumentDtos;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Utility;
using Microsoft.EntityFrameworkCore;

namespace MediRecords.Repository.DocumentRepository;

public class DocumentRepository : IDocumentRepository
{
    private readonly MediRecordsDbContext _dbContext;

    public DocumentRepository(MediRecordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DocumentUploadResponseDto> UploadDocumentAsync(DocumentUploadRequestDto request, int userId)
    {
        try
        {
            // Validate patient exists
            var patient = await _dbContext.Patients
                .FirstOrDefaultAsync(p => p.PatientId == request.PatientId);
            
            if (patient == null)
                throw new MediRecordsException($"Patient with ID {request.PatientId} not found.");

            // Validate encounter exists
            var encounter = await _dbContext.Encounters
                .FirstOrDefaultAsync(e => e.EncounterId == request.EncounterId);
            
            if (encounter == null)
                throw new MediRecordsException($"Encounter with ID {request.EncounterId} not found.");

            // Validate file
            if (request.File == null || request.File.Length == 0)
                throw new MediRecordsException("File is required.");

            if (request.File.Length > 10 * 1024 * 1024) // 10MB max
                throw new MediRecordsException("File size cannot exceed 10MB.");

            // Validate file type and get MIME type
            var allowedMimeTypes = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" }
            };

            var fileExtension = Path.GetExtension(request.File.FileName).ToLower();
            
            if (!allowedMimeTypes.ContainsKey(fileExtension))
                throw new MediRecordsException("Only PDF, JPG, PNG files are allowed.");

            var mimeType = allowedMimeTypes[fileExtension];

            // Validate DocType
            if (!Enum.TryParse<DocumentType>(request.DocType, ignoreCase: true, out var docType))
                throw new MediRecordsException("Invalid document type. Allowed types: Referral, Consent, Report, Photo.");

            // Read file into byte array
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await request.File.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            // Create database record with file stream
            var document = new Document
            {
                PatientID = request.PatientId,
                EncounterID = request.EncounterId,
                DocType = docType,
                FileName = request.File.FileName,
                FileType = mimeType,
                FileData = fileData,
                UploadedBy = userId,
                UploadedDate = DateTime.Now,
                Status = DocumentStatus.Active,
                ProviderOnlyVisibility = request.ProviderOnlyVisibility
            };

            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync();

            // Get uploader user name
            var uploader = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            var uploaderName = uploader?.Name ?? "Unknown";

            return new DocumentUploadResponseDto
            {
                DocumentId = document.DocumentID,
                PatientId = document.PatientID,
                EncounterId = document.EncounterID,
                DocType = document.DocType.ToString(),
                FileName = document.FileName ?? string.Empty,
                FileType = document.FileType ?? string.Empty,
                UploadedBy = uploaderName,
                UploadedDate = document.UploadedDate,
                Status = document.Status.ToString(),
                Message = "Document uploaded successfully"
            };
        }
        catch (MediRecordsException)
        {
            throw;
        }
        catch (DbUpdateException ex)
        {
            throw new MediRecordsException($"Database error while uploading document: {ex.InnerException?.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new MediRecordsException($"Error uploading document: {ex.Message}", ex);
        }
    }
}
