using System;

namespace Company.Function
{
    public static class ParseTextIntoObject
    {
        public static WorkOrder TextToParse(string text)
        {
            const string workorder_expr = @"Work Order.([^\.]*)";
            const string approved_by_name_expr = @"Approved by:[\r\n]{1,2}.*Name:\s*([^\r\n]*)";
            const string approved_by_email_expr = @"Approved by:[\r\n]{1,2}.*[\r\n]{1,2}Email:\s*([^\r\n]*)";
            const string requester_name_expr = @"Requester:[\r\n]{1,2}.*Name:\s*([^\r\n]*)";
            const string requester_email_expr = @"Requester:[\r\n]{1,2}.*[\r\n]{1,2}Email:\s*([^\r\n]*)";
            const string item_requested_expr = @"\s*Item requested:\s*([^\r\n]*)";
            const string request_type_expr = @"\s*Request type:\s*([^\r\n]*)";
            const string cluster_name_expr = @"\s*Cluster Name:\s*([^\r\n]*)";
            const string role_expr = @"\s*Role:\s*([^\r\n]*)";
            const string servers_expr = @"\s*Server\(s\) to be added:[\s\w,]*(?=\nUser)";
            const string users_expr = @"\s*User\(s\) to be added:[\s\w,()]*(?=\nPayment)";
            const string cost_centre_expr = @"\s*Cost Centre:\s*([^\r\n]*)";
            const string server_trim_label_expr = @"\s*Server\(s\) to be added:\s*";
            const string user_trim_label_expr = @"\s*User\(s\) to be added:\s*";
            const string servers_split_expr = @"\s*,\s*";
            const string users_split_expr = @"(?<=\))\,\s";
            const string users_with_no_department_split_expr = @"\s*,\s*";

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

                // Servers array to object
                servers = RegexFind.Split(servers, server_trim_label_expr, workorder_id)[1];
                string[] arrServers = RegexFind.Split(servers, servers_split_expr, workorder_id);
                foreach (var server in arrServers)
                {
                    requestDetail.Servers.Add(new Server { Name = server });
                }

                // Users array to object
                users = RegexFind.Split(users, user_trim_label_expr, workorder_id)[1];
                string[] arrUsers;
                if (users.Contains('(') && users.Contains(')'))
                {
                    arrUsers = RegexFind.Split(users, users_split_expr, workorder_id);
                }
                else
                {
                    arrUsers = RegexFind.Split(users, users_with_no_department_split_expr, workorder_id);
                }
                foreach (var user in arrUsers)
                {
                    requestDetail.Users.Add(new User { Name = user });
                }

                var workOrder = new WorkOrder(workorder_id, approver, requester, item_requested,
                request_type, requestDetail, cost_centre);

                return workOrder;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"ERROR: {e.Message}");
                InsertIntoSql.Log($"Error while parsing the email body: {e.Message}", text, InsertIntoSql.LogType.error);
                throw e;
            }
        }
    }
}
