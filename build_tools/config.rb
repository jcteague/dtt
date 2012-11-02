require File.join('./','build_tools','helper.rb')

root_dir = File.expand_path('.')
RestServiceBuildTools = File.join(root_dir, "build_tools")
RestServiceDatabaseConfig = File.join(RestServiceBuildTools,"database.yml")
RestServiceRoot = File.join(root_dir,'src','TeamNotification_Server')
RestServiceMigrations= File.join(RestServiceRoot,'db','migrate')
RestDeployFolder= File.join(root_dir,'deploy')
RestServiceData = File.join(RestServiceRoot, 'data')

VisualStudioRoot = File.join(root_dir, 'src', 'TeamNotification_VisualStudio')
VisualStudioSolution = File.join(VisualStudioRoot, 'TeamNotification.sln')
VisualStudioReleaseFolder = File.join(VisualStudioRoot, 'TeamNotification_Package', 'bin', 'Release')
VisualStudioManifest = File.join(VisualStudioRoot, 'TeamNotification_Package', 'source.extension.vsixmanifest')

BuildToolsRoot = File.join(root_dir, 'build_tools')
TemplatesFolder = File.join(BuildToolsRoot, 'templates')
EnvironmentFoler = File.join(BuildToolsRoot, 'environment')

NginXFolder = File.join(TemplatesFolder, 'nginx')
NginXTemplate = File.join(NginXFolder, 'nginx_server.config.erb')

VarnishFolder = File.join(TemplatesFolder, 'varnish')
VarnishDefault = File.join(VarnishFolder, 'varnish')
VarnishTemplate = File.join(VarnishFolder, 'varnish_server.config.erb')

STunnelFolder = File.join(TemplatesFolder, 'stunnel')
STunnelDefault = File.join(STunnelFolder, 'stunnel')
STunnelTemplate = File.join(STunnelFolder, 'stunnel_server.config.erb')
