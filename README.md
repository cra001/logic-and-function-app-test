# logic-and-function-app-test
## Azure CLI command for creating auth credentials for pushing logic app

az ad sp create-for-rbac --name joined-workflow --role contributor --scopes /subscriptions/<subscription-id>/resourceGroups/joined-workflow-resource-group /subscriptions/<subscription-id>/resourceGroups/datasource_resource_group --sdk-auth
