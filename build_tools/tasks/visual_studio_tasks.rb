require File.join('./','build_tools','config.rb')
require 'albacore'
begin
    require 'FileUtils'
    def copy(source, dest)
    	FileUtils.copy source, dest
    end
rescue LoadError
    def copy(source, dest)
    	sh "cp #{source}, #{dest}"
    end
end

namespace :visual_studio do

  task :build => [
    :msbuild, :copy_in_destination
  ]

  msbuild :msbuild do |msb|
    msb.properties = { :configuration => :Release }
    msb.targets = [ :Clean, :Build ]
    msb.solution = VisualStudioSolution
  end

  task :copy_in_destination do
    copy File.join(VisualStudioReleaseFolder, 'TeamNotification_Package.vsix'), '.'
  end

end
