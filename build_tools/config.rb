require File.join('./','build_tools','helper.rb')

root_dir = File.expand_path('.')
RestServiceBuildTools = File.join(root_dir, "build_tools")
RestServiceDatabaseConfig = File.join(RestServiceBuildTools,"database.yml")
RestServiceRoot = File.join(root_dir,'src','TeamNotification_Server')
RestServiceMigrations= File.join(RestServiceRoot,'db','migrate')
RestDeployFolder= File.join(root_dir,'deploy')

VisualStudioRoot = File.join(root_dir, 'src', 'TeamNotification_VisualStudio')
VisualStudioSolution = File.join(VisualStudioRoot, 'TeamNotification.sln')
VisualStudioReleaseFolder = File.join(VisualStudioRoot, 'TeamNotification_Package', 'bin', 'Release')
