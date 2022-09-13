# AddUp.CommonLogging

[![Build](https://github.com/addupsolutions/AddUp.CommonLogging/workflows/Build/badge.svg)](https://github.com/addupsolutions/AddUp.CommonLogging/actions?query=workflow%3ABuild)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=addupsolutions_AddUp.CommonLogging&metric=alert_status)](https://sonarcloud.io/dashboard?id=addupsolutions_AddUp.CommonLogging)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=addupsolutions_AddUp.CommonLogging&metric=coverage)](https://sonarcloud.io/dashboard?id=addupsolutions_AddUp.CommonLogging)
[![NuGet](https://img.shields.io/nuget/v/AddUp.CommonLogging.svg)](https://www.nuget.org/packages/AddUp.CommonLogging/)

A replacement for [net-commons' Common Logging](https://net-commons.github.io/common-logging/).

This project is born out of need here at AddUp's. We have a huge .NET 4.7.2-based codebase that quite heavily relies on **Common.Logging**; and we are currently in the process of migrating parts of this code to .NET 6. Although it would be possible and probably desirable to replace **Common.Logging**-based code by something else, for practical and non-regression purpose, it appears using a .NET 6-friendly port of **Common.Logging** is useful. Hence this project.

Although technically this is *not* a fork of <https://github.com/net-commons/common-logging> (because the original repository contains far too much code I don't want here), it aims at being nearly API-compatible with its inspiration.

Here are the main differences:

* Only .NET Standard 2.0 is targeted
* Packages, Assemblies and Namespaces are named `AddUp.CommonLogging.*`
* The two original `Common.Logging.Core` and `Common.Logging` packages are merged into a unique `AddUp.CommonLogging` package (and assembly)
* For now, only NLog is supported (since version 4.5)
* Obsolete methods were removed

## Usage

Using `AddUp.CommonLogging` is very similar to using `Common.Logging`. One mainly has to replace occurrences of `Common.Logging` by `AddUp.CommonLogging` both in C# source code and in App.config.

In libraries, instead of referencing the `Common.Logging` or `Common.Logging.Core` nuget packages, reference `AddUp.CommonLogging`.

In application code, given NLog is the concrete logging implementation, reference `AddUp.CommonLogging.NLog` instead of `Common.Logging.NLog40` (or any other NLog-suffixed package).

A typical `App.config` file delegating logging to NLog would look like this (here again, `Common.Logging` is replaced by `AddUp.CommonLogging` be it in type or assembly names). 
Also note that the section group should be `addup` and not `common` (so that it does not collide with eventual Common.Logging configuration):

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <sectionGroup name="addup">
      <section name="logging" type="AddUp.CommonLogging.ConfigurationSectionHandler, AddUp.CommonLogging" />
    </sectionGroup>
  </configSections>
  <addup>
    <logging>
      <factoryAdapter type="AddUp.CommonLogging.NLog.NLogLoggerFactoryAdapter, AddUp.CommonLogging.NLog">
        <arg key="configType" value="FILE" />
        <arg key="configFile" value="~/nlog.config" />
      </factoryAdapter>
    </logging>
  </addup>
</configuration>
```

## History

### [Version 1.0.2 - 2022/09/13](https://github.com/addupsolutions/AddUp.CommonLogging/releases/tag/v1.0.2)

* No change apart from the PDBs being included in the `nupkg` files

### [Version 1.0.1 - 2022/03/21](https://github.com/addupsolutions/AddUp.CommonLogging/releases/tag/v1.0.1)

* BUGFIX: the configuration section group name should be `addup` and not `common`.

### [Version 1.0.0 - 2022/03/01](https://github.com/addupsolutions/AddUp.CommonLogging/releases/tag/v1.0.0)

Initial version
