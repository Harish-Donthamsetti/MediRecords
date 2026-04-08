using MediRecords.Domain.Entities;
using MediRecords.Dto.NursingNoteDtos;
using MediRecords.Models;
using MediRecords.Repository.NursingNoteRepository;
using MediRecords.Utility;
using MediRecords.Domain.Enums;

namespace MediRecords.Services.NursingNoteServices;

public class NursingNoteService : INursingNoteService
{
    private readonly INursingNoteRepository _nursingNoteRepository;

    public NursingNoteService(INursingNoteRepository nursingNoteRepository)
    {
        _nursingNoteRepository = nursingNoteRepository;
    }

    public async Task<NursingNoteResponseDto> AddNoteAsync(int encounterId, NursingNoteCreateRequestDto requestDto)
    {
        var encounter = await _nursingNoteRepository.GetEncounterByIdAsync(encounterId);
        if (encounter == null)
        {
            throw new MediRecordsException("Encounter not found");
        }

        if (encounter.Status != EncounterStatus.Open)
        {
            throw new MediRecordsException("Encounter is closed");
        }

        var note = new NursingNote
        {
            EncounterId = encounterId,
            Notes = requestDto.Notes,
            RecordedBy = requestDto.RecordedBy,
            RecordedDate = DateTime.Now
        };

        var addedNote = await _nursingNoteRepository.AddAsync(note);

        return new NursingNoteResponseDto
        {
            NursingNoteId = addedNote.NursingNoteId,
            EncounterId = addedNote.EncounterId,
            Notes = addedNote.Notes ?? string.Empty,
            RecordedBy = addedNote.RecordedBy,
            RecordedDate = addedNote.RecordedDate
        };
    }

    public async Task<NursingNoteResponseDto> UpdateNoteAsync(int noteId, NursingNoteUpdateRequestDto requestDto)
    {
        var existingNote = await _nursingNoteRepository.GetByIdAsync(noteId);
        if (existingNote == null)
        {
            throw new MediRecordsException("Nursing note not found");
        }

        var encounter = await _nursingNoteRepository.GetEncounterByIdAsync(existingNote.EncounterId);
        if (encounter == null)
        {
            throw new MediRecordsException("Encounter not found");
        }

        if (encounter.Status != EncounterStatus.Open)
        {
            throw new MediRecordsException("Encounter is closed");
        }

        existingNote.Notes = requestDto.Notes;
        existingNote.RecordedBy = requestDto.RecordedBy;
        existingNote.RecordedDate = DateTime.Now;

        await _nursingNoteRepository.UpdateAsync(existingNote);

        return new NursingNoteResponseDto
        {
            NursingNoteId = existingNote.NursingNoteId,
            EncounterId = existingNote.EncounterId,
            Notes = existingNote.Notes ?? string.Empty,
            RecordedBy = existingNote.RecordedBy,
            RecordedDate = existingNote.RecordedDate
        };
    }
}
