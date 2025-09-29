using WebApplication16.Enums;
using WebApplication16.Models;
using System.Text;
using System.Web;
using System.Text.Json;
using System.Collections.Generic;

namespace WebApplication16.Services
{
    /// <summary>
    /// مسئول تولید کدهای HTML برای رندر کردن انواع فیلدهای فرم است.
    /// </summary>
    public class FieldRendererService : IFieldRendererService
    {
        public string RenderField(FormField field, string namePrefix = "")
        {
            // از نام برنامه‌نویسی استفاده می‌کند. اگر خالی بود، از Id استفاده می‌کند.
            string name = string.IsNullOrEmpty(field.Name) ? $"field_{field.Id}" : field.Name;

            string requiredAttribute = field.IsRequired ? "required" : "";

            var sb = new StringBuilder();
            string cssClasses = !string.IsNullOrEmpty(field.CssClasses) ? field.CssClasses : "";

            // اضافه کردن اتریبیوت‌های داده برای منطق شرطی
            string conditionalLogicData = !string.IsNullOrEmpty(field.ConditionalLogicJson) ?
                $"data-conditional='{HttpUtility.HtmlEncode(field.ConditionalLogicJson)}'" : "";

            // کلاس 'conditional-field' برای هدف قرار دادن با جاوااسکریپت
            sb.AppendLine($"<div class=\"mb-3 form-group conditional-field {cssClasses}\" data-field-id=\"{field.Id}\" {conditionalLogicData}>");

            sb.AppendLine($"<label for=\"field-{field.Id}\" class=\"form-label\">{field.Label}{(field.IsRequired ? " <span class=\"text-danger\">*</span>" : "")}</label>");

            switch (field.FieldType)
            {
                case FieldType.Text:
                    sb.AppendLine($"<input type=\"text\" id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control\" {requiredAttribute} />");
                    break;
                case FieldType.TextArea:
                    sb.AppendLine($"<textarea id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control\" rows=\"3\" {requiredAttribute}></textarea>");
                    break;
                case FieldType.Number:
                    sb.AppendLine($"<input type=\"number\" id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control\" {requiredAttribute} />");
                    break;
                case FieldType.Email:
                    sb.AppendLine($"<input type=\"email\" id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control\" {requiredAttribute} />");
                    break;
                case FieldType.Password:
                    sb.AppendLine($"<input type=\"password\" id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control\" {requiredAttribute} />");
                    break;
                case FieldType.Date:
                    sb.AppendLine($"<input type=\"date\" id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control\" {requiredAttribute} />");
                    break;
                case FieldType.Color:
                    sb.AppendLine($"<input type=\"color\" id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control form-control-color\" {requiredAttribute} />");
                    break;
                case FieldType.Checkbox:
                    sb.AppendLine("<div class=\"form-check\">");
                    // چک‌باکس‌ها نیاز به نام منحصر به فرد دارند
                    sb.AppendLine($"<input type=\"checkbox\" id=\"field-{field.Id}\" name=\"{name}\" class=\"form-check-input\" value=\"true\" />");
                    sb.AppendLine($"<label for=\"field-{field.Id}\" class=\"form-check-label\">{field.Label}</label>");
                    sb.AppendLine("</div>");
                    break;
                case FieldType.RadioButton:
                    // فیلدهای رادیویی
                    var radioOptions = JsonSerializer.Deserialize<List<string>>(field.SettingsJson ?? "[]");
                    if (radioOptions != null)
                    {
                        foreach (var option in radioOptions)
                        {
                            string radioOptionId = $"field-{field.Id}-{option.Replace(" ", "-")}";
                            sb.AppendLine("<div class=\"form-check form-check-inline\">");
                            sb.AppendLine($"<input type=\"radio\" id=\"{radioOptionId}\" name=\"{name}\" class=\"form-check-input\" value=\"{option}\" {requiredAttribute} />");
                            sb.AppendLine($"<label for=\"{radioOptionId}\" class=\"form-check-label\">{option}</label>");
                            sb.AppendLine("</div>");
                        }
                    }
                    break;
                case FieldType.Dropdown:
                    // لیست کشویی
                    var dropdownOptions = JsonSerializer.Deserialize<List<string>>(field.SettingsJson ?? "[]");
                    if (dropdownOptions != null)
                    {
                        sb.AppendLine($"<select id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control\" {requiredAttribute}>");
                        if (!field.IsRequired) sb.AppendLine("<option value=\"\">انتخاب کنید...</option>");
                        foreach (var option in dropdownOptions)
                        {
                            sb.AppendLine($"<option value=\"{option}\">{option}</option>");
                        }
                        sb.AppendLine($"</select>");
                    }
                    break;
                case FieldType.FileUpload:
                    sb.AppendLine($"<input type=\"file\" id=\"field-{field.Id}\" name=\"{name}\" class=\"form-control\" {requiredAttribute} />");
                    break;
                case FieldType.Hidden:
                    // فیلد مخفی، بدون نمایش label
                    sb.Remove(sb.Length - 1, 1); // حذف <label> قبلی
                    sb.AppendLine($"<input type=\"hidden\" id=\"field-{field.Id}\" name=\"{name}\" />");
                    break;
                default:
                    sb.AppendLine($"<p class=\"text-danger\">نوع فیلد پشتیبانی نمی‌شود: {field.FieldType}</p>");
                    break;
            }

            sb.AppendLine("</div>");

            return sb.ToString();
        }
    }
}
