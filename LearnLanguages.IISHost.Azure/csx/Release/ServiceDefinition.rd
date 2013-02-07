<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="LearnLanguages.IISHost.Azure" generation="1" functional="0" release="0" Id="0a578948-f610-465d-9916-9332a7eb31f4" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="LearnLanguages.IISHost.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="LearnLanguages.IISHost:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/LB:LearnLanguages.IISHost:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="LearnLanguages.IISHost:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/MapLearnLanguages.IISHost:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="LearnLanguages.IISHostInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/MapLearnLanguages.IISHostInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:LearnLanguages.IISHost:Endpoint1">
          <toPorts>
            <inPortMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/LearnLanguages.IISHost/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapLearnLanguages.IISHost:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/LearnLanguages.IISHost/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapLearnLanguages.IISHostInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/LearnLanguages.IISHostInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="LearnLanguages.IISHost" generation="1" functional="0" release="0" software="C:\Work\LearnLanguages\LearnLanguages.IISHost.Azure\csx\Release\roles\LearnLanguages.IISHost" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;LearnLanguages.IISHost&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;LearnLanguages.IISHost&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/LearnLanguages.IISHostInstances" />
            <sCSPolicyUpdateDomainMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/LearnLanguages.IISHostUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/LearnLanguages.IISHostFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="LearnLanguages.IISHostUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="LearnLanguages.IISHostFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="LearnLanguages.IISHostInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="8f82db7f-a5e7-4420-b142-784941834b86" ref="Microsoft.RedDog.Contract\ServiceContract\LearnLanguages.IISHost.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="5ab83d58-5a65-4d23-ba3e-59c85f22b74a" ref="Microsoft.RedDog.Contract\Interface\LearnLanguages.IISHost:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/LearnLanguages.IISHost.Azure/LearnLanguages.IISHost.AzureGroup/LearnLanguages.IISHost:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>