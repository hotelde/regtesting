# regtesting [![buildstatus](https://ci.appveyor.com/api/projects/status/github/hotelde/regtesting?branch=master)](https://ci.appveyor.com/project/regtesting/regtesting/branch/master)
A C# powered uitestserver for running and managing distributed tests with selenium.

## Introduction
Regtesting contains a server and a node component.
Also there are two different components to start tests:
- A localtool for quick tests during development
- A buildtask to start tests during deployments

## Install & Config

### Setup Server
- Copy app.config.example to app.config
- Edit all the app settings to include for example the path to the test files
- Fireup the server

### Setup Nodes
- Copy app.config.example to app.config
- Edit the endpoint address of the server.
- Copy some nodes to a few different vms with different browsers.

To start a node:
```
RegTesting.Node.exe NODENAME BROWSER [BROWSER] ...
```
- NODENAME: Any string to identify the node in the server. If you use the hostname of the node vm here, you can remote reboot the machine from the webportal
- BROWSER: At least one browser that is supported by the node. Must match a browser name known to the server, else it won't be useful
