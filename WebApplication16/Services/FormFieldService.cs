using Microsoft.EntityFrameworkCore;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Enums;
using WebApplication16.Models;

namespace WebApplication16.Services
{
    public class FormFieldService : IFormFieldService
    {
        private readonly WebApplication16Context _context;

        public FormFieldService(WebApplication16Context context)
        {
            _context = context;
        }

        public async Task<FormField> AddFieldToFormAsync(int formId, FieldType fieldType)
        {
            var lastField = await _context.FormFields
                                          .Where(f => f.FormId == formId)
                                          .OrderByDescending(f => f.Order)
                                          .FirstOrDefaultAsync();

            var newField = new FormField
            {
                FormId = formId,
                FieldType = fieldType,
                Label = $"فیلد جدید ({fieldType})",
                Order = (lastField?.Order ?? 0) + 1,
                IsRequired = false
            };

            _context.FormFields.Add(newField);
            await _context.SaveChangesAsync();
            return newField;
        }

        public async Task DeleteFieldAsync(int fieldId)
        {
            var field = await _context.FormFields.FindAsync(fieldId);
            if (field != null)
            {
                _context.FormFields.Remove(field);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<FormField?> GetFieldByIdAsync(int fieldId)
        {
            return await _context.FormFields.AsNoTracking().FirstOrDefaultAsync(f => f.Id == fieldId);
        }

        public async Task<FormField> UpdateFieldAsync(int fieldId, string label, bool isRequired)
        {
            var field = await _context.FormFields.FindAsync(fieldId);
            if (field == null)
            {
                throw new KeyNotFoundException("Field not found.");
            }

            field.Label = label;
            field.IsRequired = isRequired;

            await _context.SaveChangesAsync();
            return field;
        }

        public async Task UpdateFieldOrderAsync(int formId, List<int> fieldIds)
        {
            var fields = await _context.FormFields
                                       .Where(f => f.FormId == formId)
                                       .ToListAsync();

            for (int i = 0; i < fieldIds.Count; i++)
            {
                var field = fields.FirstOrDefault(f => f.Id == fieldIds[i]);
                if (field != null)
                {
                    field.Order = i + 1;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
