# logic-and-function-app-test
## Azure CLI command for creating auth credentials for pushing logic app

`az ad sp create-for-rbac --name <logic-app-name> --role contributor --scopes /subscriptions/<subscription-id>/resourceGroups/<resource-group> --sdk-auth`
