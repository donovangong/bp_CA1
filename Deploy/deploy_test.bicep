param appName string
param location string = 'francecentral'

/*
resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: 'BPPlan'
  location: location
  sku: {
    name: 'F1'
    tier: 'Free'
    size: 'F1'
    capacity: 1
  }
  properties: {
    reserved: false
  }
}
*/

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' existing = {
  name: 'BPPlan'
}

resource webApp 'Microsoft.Web/sites@2023-12-01' existing = {
  name: appName
}

output webAppName string = webApp.name
output appServicePlanName string = appServicePlan.name
