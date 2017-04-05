using MngYourContracr.MngYourContractDatabase;

namespace MngYourContracr.Service
{
    public class TaskService : Service<Task>
    {
        public TaskService(CompanyContext context)
            : base(context)
        {
        }
    }
}