using MediRecords.Dto.DocumentDtos;
using MediRecords.Domain.Entities;
using MediRecords.Domain.Enums;
using MediRecords.Utility;
using Microsoft.EntityFrameworkCore;

namespace MediRecords.Repository.DocumentRepository;

public class DocumentRepository : IDocumentRepository
{
    private readonly MediRecordsDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DocumentRepository(MediRecordsDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
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

            // Validate file type
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(request.File.FileName).ToLower();
            
            if (!allowedExtensions.Contains(fileExtension))
                throw new MediRecordsException("Only PDF, JPG, PNG files are allowed.");

            // Validate DocType
            if (!Enum.TryParse<DocumentType>(request.DocType, ignoreCase: true, out var docType))
                throw new MediRecordsException("Invalid document type. Allowed types: Referral, Consent, Report, Photo.");

            // Create directory structure
            var documentsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "Documents", $"PatientID_{request.PatientId}");
            
            if (!Directory.Exists(documentsFolder))
                Directory.CreateDirectory(documentsFolder);

            // Generate unique filename with timestamp
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{timestamp}_{docType.ToString().ToLower()}{fileExtension}";
            var filePath = Path.Combine(documentsFolder, fileName);

            // Save file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            // Create database record
            var document = new Document
            {
                PatientID = request.PatientId,
                EncounterID = request.EncounterId,
                DocType = docType,
                FileURI = Path.Combine("Documents", $"PatientID_{request.PatientId}", fileName).Replace("\\", "/"),
                FileName = request.File.FileName,
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
