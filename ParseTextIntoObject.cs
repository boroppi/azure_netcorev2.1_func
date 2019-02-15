using System;

namespace Company.Function
{
    public static class ParseTextIntoObject
    {
        public static WorkOrder TextToParse(string text)
        {
            const string workorder_expr = @"Work Order.([^\.]*)";
            const string approved_by_name_expr = @"Approved by:[\n\r].*Name:\s*([^\n\r]*)";
            const string approved_by_email_expr = @"Approved by:[\n\r].*[\n\r]*Email:\s*([^\n\r]*)";
            const string requester_name_expr = @"Requester:[\n\r].*Name:\s*([^\n\r]*)";
            const string requester_email_expr = @"Requester:[\n\r].*[\n\r]*Email:\s*([^\n\r]*)";
            const string item_requested_expr = @"\s*Item requested:\s*([^\n\r]*)";
            const string request_type_expr = @"\s*Request type:\s*([^\n\r]*)";
            const string cluster_name_expr = @"\s*Cluster Name:\s*([^\n\r]*)";
            const string role_expr = @"\s*Role:\s*([^\n\r]*)";
            const string servers_expr = @"\s*Server\(s\) to be added:[\s\w,]*(?=\nUser)";
            const string users_expr = @"\s*User\(s\) to be added:\s*([^\n\r]*)";
            const string cost_centre_expr = @"\s*Cost Centre:\s*([^\n\r]*)";

            try
            {
                var workorder_id = RegexFind.FindString(text, workorder_expr, "work order");
                var approved_by_name = RegexFind.FindString(text, approved_by_name_expr, "approver name");
                var approved_by_email = RegexFind.FindString(text, approved_by_email_expr, "approver email");
                var requester_name = RegexFind.FindString(text, requester_name_expr, "requester name");
                var requester_email = RegexFind.FindString(text, requester_email_expr, "requester email");
                var item_requested = RegexFind.FindString(text, item_requested_expr, "item requested");
                var request_type = RegexFind.FindString(text, request_type_expr, "request type");
                var cluster_name = RegexFind.FindString(text, cluster_name_expr, "cluster name");
                var role = RegexFind.FindString(text, role_expr, "role");
                var servers = RegexFind.FindString(text, servers_expr, "servers");
                var users = RegexFind.FindString(text, users_expr, "users");
                var cost_centre = RegexFind.FindString(text, cost_centre_expr, "cost centre");

                var approver = new Approver();
                approver.Name = approved_by_name;
                approver.Email = approved_by_email;

                var requester = new Requester();
                requester.Name = requester_name;
                requester.Email = requester_email;

                var requestDetail = new RequestDetail();
                requestDetail.ClusterName = cluster_name;
                requestDetail.Role = role;


                // TODO: Servers and USERS
                requestDetail.Servers = null;
                requestDetail.Users = null;

                var workOrder = new WorkOrder(workorder_id, approver, requester, item_requested,
                request_type, requestDetail, cost_centre);

                return workOrder;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"ERROR: {e.Message}");
                throw e;
            }


        }
    }


}
