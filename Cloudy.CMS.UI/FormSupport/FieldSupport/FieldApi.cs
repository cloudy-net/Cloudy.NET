using Poetry.UI.ApiSupport;
using Poetry.UI.FormSupport.ControlSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.FieldSupport
{
    [Api("Field")]
    public class FieldApi
    {
        IFormProvider FormProvider { get; }
        IFieldProvider FieldProvider { get; }
        IControlMatcher ControlMatcher { get; }

        public FieldApi(IFormProvider formProvider, IFieldProvider fieldProvider, IControlMatcher controlMatcher)
        {
            FormProvider = formProvider;
            FieldProvider = fieldProvider;
            ControlMatcher = controlMatcher;
        }

        [Endpoint("GetAllForForm")]
        public IEnumerable<FieldResponse> GetAllForForm(string id)
        {
            var result = new List<FieldResponse>();

            foreach(var field in FieldProvider.GetAllFor(id))
            {
                if (!field.AutoGenerate)
                {
                    continue;
                }

                var control = ControlMatcher.GetFor(field.Type, field.UIHints);
                var embeddedFormId = FormProvider.GetAll().FirstOrDefault(f => f.Type == field.Type);

                if(control == null && embeddedFormId == null)
                {
                    continue;
                }

                result.Add(new FieldResponse
                {
                    Id = field.Id,
                    Control = control,
                    EmbeddedFormId = embeddedFormId?.Id,
                    IsSortable = field.IsSortable,
                    Group = field.Group,
                });
            }

            return result;
        }

        public class FieldResponse
        {
            public string Id { get; set; }
            public ControlReference Control { get; set; }
            public string EmbeddedFormId { get; set; }
            public bool IsSortable { get; set; }
            public string Group { get; set; }
        }
    }
}
