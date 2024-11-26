using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonTypes.Language.LanguageSupport;

namespace Datamodels.Logic
{
    public class RoleLogic : BaseLogic
    {
        public RoleLogic(Context context) : base(context) { }

        public IEnumerable<object> GetRolesSelects() => context.Roles.OrderBy(r => r.Id).Select(r => new
        {
            id = r.Id,
            name = new LanguageObject(r.NameEn, r.NameEs),
            description = new LanguageObject(r.DescriptionEn, r.DescriptionEs)
        }).ToList();
    }
}
