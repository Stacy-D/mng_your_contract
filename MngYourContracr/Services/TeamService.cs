using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Service
{
    public class TeamService : Service<Team>
    {
        public TeamService(CompanyContext context)
            : base(context)
        {
        }
    }
}