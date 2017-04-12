using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Service
{
   
    public class ProjectService : Service<Project>
    {
        private TeamService teamService;
        public ProjectService(CompanyContext context)
            : base(context)
        {
            this.teamService = new TeamService(context);
        }


        public void assignTeam(int teamId, int projectId)
        {
            var project = this.GetByID(projectId);
                project.TeamId = teamId;
                this.context.SaveChanges();
            
        }

        public void updateBudget(decimal outgoings, int projectId) {
            var project = this.GetByID(projectId);
            project.outgoings = outgoings;
            project.income = project.budget - project.outgoings;
            this.context.SaveChanges();
        }
    }
}