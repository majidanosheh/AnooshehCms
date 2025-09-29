using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApplication16.Areas.Identity.DataAccess;
using WebApplication16.Models;
using WebApplication16.Enums;
using System;
using System.Text.Json;

namespace WebApplication16.Services
{
    public class FormFieldService : IFormFieldService
    {
        private readonly WebApplication16Context _context;
        private readonly IRazorViewToStringRenderer _viewRenderer;

        public FormFieldService(WebApplication16Context context, IRazorViewToStringRenderer viewRenderer)
        {
            _context = context;
            _viewRenderer = viewRenderer;
        }

        public async Task<FormField?> AddFieldToFormAsync(int formId, string fieldType)
        {
            if (!Enum.TryParse(typeof(FieldType), fieldType, true, out var type))
            {
                return null;
            }

            var form = await _context.Forms.Include(f => f.FormFields).FirstOrDefaultAsync(f => f.Id == formId);
            if (form == null) return null;

            var defaultLabel = "فیلد جدید";
            var newField = new FormField
            {
                FormId = formId,
                FieldType = (FieldType)type,
                Label = defaultLabel,
                Name = GenerateName(defaultLabel),
                IsRequired = false,
                Order = form.FormFields.Any() ? form.FormFields.Max(f => f.Order) + 1 : 1,
                // اضافه کردن تنظیمات پیش فرض برای فیلدهای خاص
                SettingsJson = ((FieldType)type) switch
                {
                    FieldType.Dropdown => JsonSerializer.Serialize(new List<string> { "گزینه ۱", "گزینه ۲" }),
                    FieldType.RadioButton => JsonSerializer.Serialize(new List<string> { "گزینه ۱", "گزینه ۲" }),
                    _ => null
                },
                // اضافه کردن منطق شرطی پیش فرض
                ConditionalLogicJson = null // منطق شرطی به صورت پیش فرض خاموش است
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

        public async Task<string?> UpdateFieldAsync(FormField updatedField)
        {
            var field = await _context.FormFields.FindAsync(updatedField.Id);
            if (field == null) return null;

            // فقط فیلدهایی که باید از فرانت‌اند به‌روز شوند را کپی می‌کنیم
            field.Label = updatedField.Label;
            field.Name = updatedField.Name;
            field.IsRequired = updatedField.IsRequired;
            field.CssClasses = updatedField.CssClasses;

            // به‌روزرسانی تنظیمات و منطق شرطی
            if (updatedField.SettingsJson != null) field.SettingsJson = updatedField.SettingsJson;
            if (updatedField.ValidationRulesJson != null) field.ValidationRulesJson = updatedField.ValidationRulesJson;
            if (updatedField.ConditionalLogicJson != null) field.ConditionalLogicJson = updatedField.ConditionalLogicJson;

            if (string.IsNullOrWhiteSpace(field.Name))
            {
                field.Name = GenerateName(updatedField.Label);
            }

            await _context.SaveChangesAsync();

            // توجه: برای نمایش در UI مدیریت، به HTML نیاز داریم.
            return await _viewRenderer.RenderViewToStringAsync("~/Areas/Admin/Views/Forms/_FormFieldItem.cshtml", field);
        }

        public async Task UpdateFieldOrderAsync(int formId, List<int> fieldIds)
        {
            var fields = await _context.FormFields.Where(f => f.FormId == formId).ToListAsync();

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

        private string GenerateName(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                return $"field_{Guid.NewGuid().ToString("N")[..8]}";

            string sanitized = Regex.Replace(label.Trim(), @"[^a-zA-Z0-9\s-]", "");
            sanitized = Regex.Replace(sanitized, @"\s+", "_").ToLower();

            return $"{sanitized}_{Guid.NewGuid().ToString("N")[..4]}";
        }
    }
}
