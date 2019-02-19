using System.Collections.Generic;

namespace Company.Function
{
    public class WorkOrder
    {
        public string WorkOrderId { get; private set; }
        public Approver ApprovedBy { get; private set; }
        public Requester RequestedBy { get; private set; }
        public string ItemRequested { get; private set; }
        public string RequestType { get; private set; }
        public RequestDetail RequestDetails { get; private set; }
        public string CostCentre { get; private set; }

        public WorkOrder(string workOrderId, Approver approvedBy, Requester requestedBy,
        string itemRequested, string requestType,
        RequestDetail requestDetails, string costCentre)
        {
            this.WorkOrderId = workOrderId;
            this.ApprovedBy = approvedBy;
            this.RequestedBy = requestedBy;
            this.ItemRequested = itemRequested;
            this.RequestType = requestType;
            this.RequestDetails = requestDetails;
            this.CostCentre = costCentre;
        }
    }

    public class Approver
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Requester
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class RequestDetail
    {
        public string ClusterName { get; set; }
        public string Role { get; set; }
        public ICollection<Server> Servers { get; private set; }
        public ICollection<User> Users { get; private set; }

        // Initialize an empty List for Servers and Users the first time RequestDetail is instantiated
        public RequestDetail()
        {
            this.Servers = new List<Server>();
            this.Users = new List<User>();
        }
    }

    public class Server
    {
        public string Name { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
    }
}
