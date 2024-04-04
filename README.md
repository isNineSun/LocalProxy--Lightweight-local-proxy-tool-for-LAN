<p align="center">
  <img src="https://raw.githubusercontent.com/isNineSun/img_repository/main/aako8-4otcw-001.png" height=120>
</p>

# LocalProxyTool--Lightweight local proxy tool for LAN
![GitHub repo size](https://img.shields.io/github/repo-size/isNineSun/LocalProxy--Lightweight-local-proxy-tool-for-LAN)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/isNineSun/LocalProxy--Lightweight-local-proxy-tool-for-LAN/total)
![GitHub issues](https://img.shields.io/github/issues/isNineSun/LocalProxy--Lightweight-local-proxy-tool-for-LAN)
![GitHub License](https://img.shields.io/github/license/isNineSun/LocalProxy--Lightweight-local-proxy-tool-for-LAN)

## 目录
- [LocalProxyTool--Lightweight local proxy tool for LAN](#localproxytool--lightweight-local-proxy-tool-for-lan)
  - [目录](#目录)
  - [简介](#简介)
  - [🚀快速使用](#快速使用)
  - [⭐添加排除规则](#添加排除规则)
  - [💻自动从服务器拉取配置](#自动从服务器拉取配置)

## 简介
LocalProxyTool一个轻量化的本地代理服务工具，当你局域网内拥有一个代理服务器并开启局域网共享后，你可以通过此工具简单的使局域网内其他设备获得代理服务，而不需要去针对windows进行一个个配置。如果在代理服务器上进行了相应的配置，工具支持自动从服务器拉取代理配置，而无需手动配置。
## 🚀快速使用
打开软件，分别在Server和Port输入框内输入代理服务器的IP地址与端口号，点击Proxy即可开启代理服务（**前提需要在局域网内有一个可以正常运行的代理服务器**）。    
![](https://raw.githubusercontent.com/isNineSun/img_repository/main/d04c58d6e02c95894555d49b44bfbc38.png)    

## ⭐添加排除规则
**本设置为可选项**，可以在Exclude框内填入不想要代理监管的域名或者网络地址，支持泛域名，多条域名可以以分号进行分隔，如：我想排除Youtube，使其不走代理流量，则可以配置``www.youtube.com``或者``*.youtube.com``    
![](https://raw.githubusercontent.com/isNineSun/img_repository/main/lp.png)    

## 💻自动从服务器拉取配置

**（功能开发中......）**    

点击设置，在ConfigPath中填入配置文件路径，可以是本地路径或者网络路径（git仓库路径、FTP服务器文件路径），也计划增加通过Web服务获取配置文件的方式。
配置好路径后点击Update，工具会自动获取配置文件中的ip和端口，只需要服务器上放一个配置文件，局域网内所有设备都可以自动更新。    
![](https://raw.githubusercontent.com/isNineSun/img_repository/main/334c2d9fa02dfe6a1793f763e3b10c49.png)    