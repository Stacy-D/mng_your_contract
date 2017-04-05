using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Service
{
    public class ProjectService : Service<Project>
    {
        public ProjectService(CompanyContext context)
            : base(context)
        {
        }
    }
}