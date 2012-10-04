require File.join('./','build_tools','config.rb')
require 'albacore'
require 'FileUtils'

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
    FileUtils.copy File.join(VisualStudioReleaseFolder, 'TeamNotification_Package.vsix'), '.'
  end

end
