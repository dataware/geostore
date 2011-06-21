<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="GeoStore" generation="1" functional="0" release="0" Id="c8c76ed2-9376-4c20-b0d7-0d5781aa1324" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="GeoStoreGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="GeoStoreServiceWebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/GeoStore/GeoStoreGroup/LB:GeoStoreServiceWebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="GeoStoreServiceWebRole:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/GeoStore/GeoStoreGroup/MapGeoStoreServiceWebRole:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="GeoStoreServiceWebRole:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/GeoStore/GeoStoreGroup/MapGeoStoreServiceWebRole:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="GeoStoreServiceWebRole:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/GeoStore/GeoStoreGroup/MapGeoStoreServiceWebRole:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="GeoStoreServiceWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/GeoStore/GeoStoreGroup/MapGeoStoreServiceWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="GeoStoreServiceWebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/GeoStore/GeoStoreGroup/MapGeoStoreServiceWebRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:GeoStoreServiceWebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/GeoStore/GeoStoreGroup/GeoStoreServiceWebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapGeoStoreServiceWebRole:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/GeoStore/GeoStoreGroup/GeoStoreServiceWebRole/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapGeoStoreServiceWebRole:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/GeoStore/GeoStoreGroup/GeoStoreServiceWebRole/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapGeoStoreServiceWebRole:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/GeoStore/GeoStoreGroup/GeoStoreServiceWebRole/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapGeoStoreServiceWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/GeoStore/GeoStoreGroup/GeoStoreServiceWebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapGeoStoreServiceWebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/GeoStore/GeoStoreGroup/GeoStoreServiceWebRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="GeoStoreServiceWebRole" generation="1" functional="0" release="0" software="D:\personalContainer\GeoStore\GeoStore\bin\Debug\GeoStore.csx\roles\GeoStoreServiceWebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;GeoStoreServiceWebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;GeoStoreServiceWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="GeoStoreServiceWebRole.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/GeoStore/GeoStoreGroup/GeoStoreServiceWebRoleInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="GeoStoreServiceWebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="e0521d82-4901-47ad-ac95-361e0442981b" ref="Microsoft.RedDog.Contract\ServiceContract\GeoStoreContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="377c8142-ab01-4626-ab78-4aabf3f2dcac" ref="Microsoft.RedDog.Contract\Interface\GeoStoreServiceWebRole:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/GeoStore/GeoStoreGroup/GeoStoreServiceWebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>