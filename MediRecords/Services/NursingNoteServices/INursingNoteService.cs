using MediRecords.Dto.NursingNoteDtos;

namespace MediRecords.Services.NursingNoteServices;

public interface INursingNoteService
{
    Task<NursingNoteResponseDto> AddNoteAsync(int encounterId, NursingNoteCreateRequestDto requestDto);
    Task<NursingNoteResponseDto> UpdateNoteAsync(int noteId, NursingNoteUpdateRequestDto requestDto);
}
