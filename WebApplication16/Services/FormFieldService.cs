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

        public async Task<FormField?> AddFieldToFormAsync(int formId, string fieldType)
        {
            if (!Enum.TryParse<FieldType>(fieldType, true, out var typeEnum))
            {
                return null;
            }

            var form = await _context.Forms.Include(f => f.FormFields).FirstOrDefaultAsync(f => f.Id == formId);
            if (form == null) return null;

            var maxOrder = form.FormFields.Any() ? form.FormFields.Max(f => f.Order) : -1;

            var newField = new FormField
            {
                FormId = formId,
                FieldType = typeEnum,
                Label = $"فیلد {typeEnum}",
                Name = $"{typeEnum.ToString().ToLower()}_{Guid.NewGuid().ToString("N")[..8]}",
                Order = maxOrder + 1
            };

            _context.FormFields.Add(newField);
            await _context.SaveChangesAsync();
            return newField;
        }

        public async Task<bool> DeleteFieldAsync(int fieldId)
        {
            var field = await _context.FormFields.FindAsync(fieldId);
            if (field == null) return false;

            _context.FormFields.Remove(field);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FormField?> GetFieldByIdAsync(int fieldId)
        {
            return await _context.FormFields.AsNoTracking().FirstOrDefaultAsync(f => f.Id == fieldId);
        }

        public async Task<FormField?> UpdateFieldAsync(FormField fieldData)
        {
            var field = await _context.FormFields.FindAsync(fieldData.Id);
            if (field == null) return null;

            field.Label = fieldData.Label;
            field.IsRequired = fieldData.IsRequired;

            await _context.SaveChangesAsync();
            return field;
        }

        public async Task UpdateFieldOrderAsync(int formId, List<int> fieldIds)
        {
            var fields = await _context.FormFields.Where(f => f.FormId == formId).ToListAsync();
            for (int i = 0; i < fieldIds.Count; i++)
            {
                var field = fields.FirstOrDefault(f => f.Id == fieldIds[i]);
                if (field != null)
                {
                    field.Order = i;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}



//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WebApplication16.Areas.Identity.DataAccess;
//using WebApplication16.Enums;
//using WebApplication16.Models;

//namespace WebApplication16.Services
//{
//    public class FormFieldService : IFormFieldService
//    {
//        private readonly WebApplication16Context _context;

//        public FormFieldService(WebApplication16Context context)
//        {
//            _context = context;
//        }

//        public async Task<FormField> AddFieldToFormAsync(int formId, string fieldType)
//        {
//            // This safely parses the string from JavaScript into a C# enum
//            if (!Enum.TryParse<FieldType>(fieldType, true, out var fieldTypeEnum))
//            {
//                return null; // Invalid field type, return null to indicate failure
//            }

//            var form = await _context.Forms.Include(f => f.FormFields).FirstOrDefaultAsync(f => f.Id == formId);
//            if (form == null) return null;

//            var newField = new FormField
//            {
//                FormId = formId,
//                FieldType = fieldTypeEnum,
//                Label = $"New {fieldType} Field", // Default label
//                IsRequired = false,
//                Order = form.FormFields.Any() ? form.FormFields.Max(f => f.Order) + 1 : 1
//            };

//            _context.FormFields.Add(newField);
//            await _context.SaveChangesAsync();
//            return newField;
//        }

//        public async Task UpdateFieldOrderAsync(int formId, List<int> fieldIds)
//        {
//            var fields = await _context.FormFields.Where(f => f.FormId == formId).ToListAsync();
//            for (int i = 0; i < fieldIds.Count; i++)
//            {
//                var field = fields.FirstOrDefault(f => f.Id == fieldIds[i]);
//                if (field != null)
//                {
//                    field.Order = i + 1; // Order is 1-based
//                }
//            }
//            await _context.SaveChangesAsync();
//        }

//        public async Task<FormField> GetFieldByIdAsync(int fieldId)
//        {
//            return await _context.FormFields.AsNoTracking().FirstOrDefaultAsync(f => f.Id == fieldId);
//        }

//        public async Task<FormField> UpdateFieldAsync(FormField updatedField)
//        {
//            var field = await _context.FormFields.FindAsync(updatedField.Id);
//            if (field == null) return null;

//            // Update properties from the incoming object
//            field.Label = updatedField.Label;
//            field.IsRequired = updatedField.IsRequired;
//            // Add other properties to update in the future (e.g., Placeholder, OptionsJson)

//            await _context.SaveChangesAsync();
//            return field;
//        }

//        public async Task DeleteFieldAsync(int fieldId)
//        {
//            var field = await _context.FormFields.FindAsync(fieldId);
//            if (field != null)
//            {
//                _context.FormFields.Remove(field);
//                await _context.SaveChangesAsync();
//            }
//        }
//        public async Task<FormField?> UpdateFieldAsync(int id, string label, bool isRequired)
//        {
//            var field = await _context.FormFields.FindAsync(id);
//            if (field == null) return null;

//            field.Label = label;
//            field.IsRequired = isRequired;
//            await _context.SaveChangesAsync();

//            return field;
//        }
//    }
//}

